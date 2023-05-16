using System.Collections.Generic;
using System.Numerics;
using Logic.Exceptions;
using NUnit.Framework;

namespace TPW.Logic.Tests;

public class BallsLogicTest {

	private LogicAPI ballsLogic;
	private readonly Vector2 boardSize = new Vector2(150, 100);

	private readonly Vector2 sharedVelocity = new Vector2(0, 0);
    private readonly float sharedRadius = 1;
    private readonly float sharedMass = 1;

	[SetUp]
	public void SetUp() {
		ballsLogic = LogicAPI.CreateBalls(boardSize, new DataLayerFixture());
	}

	[Test]
	public void AddBallTest() {
		ballsLogic.AddBall(0, boardSize / 2, sharedRadius, sharedVelocity, sharedMass);
		Assert.AreEqual(1, ballsLogic.GetBallsCount());
		Assert.AreEqual(boardSize / 2, ballsLogic.GetBalls()[0].Position);
	}

	[Test]
	public void AddBallOutOfBoardTest() {
		Assert.Throws<PositionIsOutOfBoardException>((() => ballsLogic.AddBall(
			0,
			boardSize + Vector2.One * 20, 
			sharedRadius, 
			sharedVelocity, 
			sharedMass
        )));

		Assert.Throws<PositionIsOutOfBoardException>((() => ballsLogic.AddBall(
			0,
			Vector2.One * -20,
            sharedRadius,
            sharedVelocity,
            sharedMass
        )));

		Assert.AreEqual(0, ballsLogic.GetBallsCount());
	}


	[Test]
	public void AddBallsTest() {
		ballsLogic.AddBalls(15);
		Assert.AreEqual(15, ballsLogic.GetBallsCount());
	}

	[Test]
	public void SimulationTest() {
		var interactionCount = 0;
		ballsLogic.AddBalls(10);
		Assert.AreEqual(10, ballsLogic.GetBallsCount());

		var startPositionList = new List<Vector2>();
		for (int i = 0; i < ballsLogic.GetBallsCount(); i++)
			startPositionList.Add(ballsLogic.GetBalls()[i].Position);

		ballsLogic.BallChange += (_, _) => {
			interactionCount++;
			if (interactionCount >= 50)
				ballsLogic.StopSimulation();
		};

		ballsLogic.StartSimulation();
		while (interactionCount < 50) { }

		Assert.GreaterOrEqual(interactionCount, 49);

		for (int i = 0; i < ballsLogic.GetBallsCount(); i++)
			if (startPositionList[i] != ballsLogic.GetBalls()[i].Position)
				return;

		Assert.Fail();
	}

	[Test]
	public void CollideBoundryTop() {
        ballsLogic.AddBall(0, new Vector2(40, 0), 10, new Vector2(-1, -1), 20);
        var ball = ballsLogic.GetBall(0);

        Vector2 newPosition = ball.Transform.Position + ball.RigidBody.Velocity;

        Assert.IsTrue(newPosition.Y < 0);
    }

    [Test]
    public void CollideBoundryLeft() {
        ballsLogic.AddBall(0, new Vector2(0, 40), 10, new Vector2(-1, -1), 20);
        var ball = ballsLogic.GetBall(0);

        Vector2 newPosition = ball.Transform.Position + ball.RigidBody.Velocity;

        Assert.IsTrue(newPosition.X < 0);

        // If it would be rly. packed.
        // ball.RigidBody.Velocity = new Vector2(-ball.RigidBody.Velocity.X, ball.RigidBody.Velocity.Y);
        // newPosition.X = 0;
    }

    [Test]
    public void CollideBoundryBottom() {
        ballsLogic.AddBall(0, new Vector2(40, boardSize.Y), 10, new Vector2(1, 1), 20);
        var ball = ballsLogic.GetBall(0);

        Vector2 newPosition = ball.Transform.Position + ball.RigidBody.Velocity;

        Assert.IsTrue(newPosition.Y > boardSize.Y -  ball.Transform.Radius);

        // If it would be rly. packed.
        // ball.RigidBody.Velocity = new Vector2(-ball.RigidBody.Velocity.X, ball.RigidBody.Velocity.Y);
        // newPosition.X = boardSize.X -  ball.Transform.Radius;
    }

    [Test]
    public void CollideBoundryRight() {
        ballsLogic.AddBall(0, new Vector2(boardSize.X, 40), 10, new Vector2(1, 1), 20);
        var ball = ballsLogic.GetBall(0);

        Vector2 newPosition = ball.Transform.Position + ball.RigidBody.Velocity;

        Assert.IsTrue(newPosition.X > boardSize.X -  ball.Transform.Radius);

        // If it would be rly. packed.
        // ball.RigidBody.Velocity = new Vector2(ball.RigidBody.Velocity.X, -ball.RigidBody.Velocity.Y);
        // newPosition.Y = boardSize.Y - ball.Transform.Radius;
    }

    [Test]
	public void CollideBallOther() {
		ballsLogic.AddBall(0, new Vector2(40, 40), 10, new Vector2(0, 0), 20);
        ballsLogic.AddBall(1, new Vector2(35, 35), 10, new Vector2(0, 0), 20);

		var ball = ballsLogic.GetBall(0);
        var other = ballsLogic.GetBall(1);

		Assert.IsTrue(BallCollisionLogic.IsBallsCollides(ball, other));
    }
}