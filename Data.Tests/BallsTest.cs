using System.Numerics;
using TPW.Data.Components;
using NUnit.Framework;

namespace TPW.Data.Tests {
    public class Tests {
        private DataAPI balls;
        private IBallData ball1, ball2, ball3;

        // Setup:
        // 

        [SetUp]
        public void Setup() {

            ITransform transformTest1 = DataAPI.CreateTransform(new Vector2(5, 10), 1.0f);
            ITransform transformTest2 = DataAPI.CreateTransform(new Vector2(8, 4), 1.5f);
            ITransform transformTest3 = DataAPI.CreateTransform(new Vector2(2, 9), 1.2f);

            IRigidBody rigidBodyTest1 = DataAPI.CreateRigidBody(new Vector2(0.9f, 1.0f), 1.0f);
            IRigidBody rigidBodyTest2 = DataAPI.CreateRigidBody(new Vector2(0.9f, 1.2f), 0.5f);
            IRigidBody rigidBodyTest3 = DataAPI.CreateRigidBody(new Vector2(1.1f, 0.9f), 0.7f);

            balls = DataAPI.CreateBallsList();
            ball1 = DataAPI.CreateBall(0, transformTest1, rigidBodyTest1);
            ball2 = DataAPI.CreateBall(1, transformTest2, rigidBodyTest2);
            ball3 = DataAPI.CreateBall(2, transformTest3, rigidBodyTest3);
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