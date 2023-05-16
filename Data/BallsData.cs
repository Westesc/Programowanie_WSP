using System.Collections.Generic;
using System.Threading.Tasks;

// About
//  It's a list of exsisting in current scene so called spheres.

namespace TPW.Data {
    internal class BallsData : DataAPI {
        private readonly List<IBallData> balls;

        public BallsData() {
            balls = new List<IBallData>();
        }

        public override void Add(IBallData newBall) {
            balls.Add(newBall);
        }

        public override IBallData Get(int index) {
            return balls[index];
        }

        public override int GetCount() {
            return balls.Count;
        }
    }
}
