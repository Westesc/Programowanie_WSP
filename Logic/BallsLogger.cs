using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TPW.Logic {

    public interface IBallsLogger {
        void AddQueueLog(IBallLogic ball);
    }

    public class BallsLogger : IBallsLogger {

        private readonly string filePath;
        private Task? loggingTask;

        private readonly ConcurrentQueue<JObject> ballQueue = new();

        private readonly Mutex queueMutex = new();
        private readonly Mutex fileMutex = new();

        private readonly JArray fileJArray;

        public BallsLogger() {
            string path = Path.GetTempPath();
            filePath = path + "balls.json";

            // Initialize log file.
            if (File.Exists(filePath)) {
                try {
                    string input = File.ReadAllText(filePath);
                    fileJArray = JArray.Parse(input);
                    return;
                } catch (JsonReaderException) {
                    fileJArray = new JArray();
                }
            }

            // If file doesn't exists create one.
            fileJArray = new JArray();
            File.Create(filePath);
        }

        public void AddQueueLog(IBallLogic ball) {
            queueMutex.WaitOne();

            try {
                JObject item = JObject.FromObject(ball);
                item["Time"] = DateTime.Now.ToString("HH:mm:ss");
                ballQueue.Enqueue(item);

                if (loggingTask == null || loggingTask.IsCompleted)
                    loggingTask = Task.Factory.StartNew(LogToFile);
            } finally {
                queueMutex.ReleaseMutex();
            }
        }

        private async Task LogToFile() {

            // Add logs until queue is empty.
            while (ballQueue.TryDequeue(out JObject ball)) {
                fileJArray.Add(ball);
            }

            // Process data to string and save.
            string output = JsonConvert.SerializeObject(fileJArray);

            fileMutex.WaitOne();

            try {
                File.WriteAllText(filePath, output);
            } finally {
                fileMutex.ReleaseMutex();
            }

        }

        ~BallsLogger() {
            fileMutex.WaitOne();
            fileMutex.ReleaseMutex();
        }

    }
}
