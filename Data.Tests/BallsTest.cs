using System.Numerics;
using NUnit.Framework;

namespace TPW.Data.Tests {
    public class Tests {
        private DataAPI balls;
        private IBallData ball1, ball2, ball3;

        // Setup:
        // 

        [SetUp]
        public void Setup() {
            balls = DataAPI.CreateBallsList();
            ball1 = DataAPI.CreateBall(new Vector2(5, 10));
            ball2 = DataAPI.CreateBall(new Vector2(8, 4));
            ball3 = DataAPI.CreateBall(new Vector2(2, 9));
        }

        // Test:
        // 

        [Test]
        public void AddBallTest() {

            balls.Add(ball1);
            Assert.AreEqual(1, balls.GetCount());
            balls.Add(ball2);
            Assert.AreEqual(2, balls.GetCount());
            balls.Add(ball3);
            Assert.AreEqual(3, balls.GetCount());

            Assert.AreEqual(ball1, balls.Get(0));
            Assert.AreEqual(ball2, balls.Get(1));
            Assert.AreEqual(ball3, balls.Get(2));
        }
    }
}