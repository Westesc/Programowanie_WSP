using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TPW.Data.Components;

namespace TPW.Logic.Tests {
    public class BallsLoggerTest {

        private readonly string fileName = "balls-test.json";
        private readonly Vector2 boardSize = new Vector2(150, 100);

        private ITransform transform;
        private IRigidBody rigidbody;

        private LogicAPI ballsLogic;

        [SetUp]
        public void SetUp() {
            ballsLogic = LogicAPI.CreateBalls(boardSize, new DataLayerFixture());
        }

        //private IBallsLogger logger;
        //private IBallLogic ball;
        //
        //
        //
        //[SetUp]
        //public void SetUp() {
        //    logger = new BallsLogger();
        //    //ball = new BallLogic(1, );
        //}
        //
        //[Test]
        //public void AddLogTest() {
        //
        //}
    }
}