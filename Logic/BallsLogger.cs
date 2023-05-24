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
using System.Timers;

namespace TPW.Logic {

    public interface IBallsLogger {
        void AddQueueLog(IBallLogic ball);
        public void SetTimer(double interval);
    }

    public class BallsLogger : IBallsLogger {

        private readonly string filePath;
        private Task? loggingTask;

        public static System.Timers.Timer? timer;

        private readonly ConcurrentQueue<JObject> ballQueue = new();

        private readonly Mutex queueMutex = new();
        private readonly Mutex fileMutex = new();

        private readonly JArray fileJArray;

        public BallsLogger() {
            string path = Path.GetTempPath();
            filePath = path + "balls.json";

            // Initialize log file.
            if (File.Exists(filePath)) {
				fileMutex.WaitOne();
                try {
                    string input = File.ReadAllText(filePath);
                    fileJArray = JArray.Parse(input);
                    return;
                } catch (JsonReaderException) {
                    fileJArray = new JArray();
                } finally {
					fileMutex.ReleaseMutex();
				}
            }

            // If file doesn't exists create one.
            fileJArray = new JArray();
            File.Create(filePath);
        }

        public void SetTimer(double interval) {
            // Create a timer with a two second interval.
            timer = new System.Timers.Timer(interval);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e) {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            LogToFile();
            //if (loggingTask == null || loggingTask.IsCompleted)
            //    loggingTask = Task.Factory.StartNew(LogToFile);
        }

        public void AddQueueLog(IBallLogic ball) {
            queueMutex.WaitOne();

            try {
                JObject item = JObject.FromObject(ball);
                item["Time"] = DateTime.Now.ToString("HH:mm:ss");
                ballQueue.Enqueue(item);
            } finally {
                queueMutex.ReleaseMutex();
            }
        }

        private void LogToFile() {

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
