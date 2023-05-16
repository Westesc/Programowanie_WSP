using Data.Components;
using Logic;
using Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TPW.Data;

namespace TPW.Logic;

// About
// 

internal class BallsLogic : LogicAPI {

    public readonly Mutex simulationPause = new Mutex(false); // CriticalSection Lock
    private readonly DataAPI dataBalls;

    public CancellationTokenSource CancelSimulationSource { get; private set; }         //
    public Vector2 BoardSize { get; }

	public BallsLogic(DataAPI newDataBalls, Vector2 newBoardSize) {
        CancelSimulationSource = new CancellationTokenSource();
        dataBalls = newDataBalls;
		BoardSize = newBoardSize;
	}

	protected override void OnPositionChange(OnBallChangeEventArgs newArgs) {
		base.OnPositionChange(newArgs);
	}

	public override void AddBalls(int newCount) {
		for (var i = 0; i < newCount; i++) {

			// SET PRE SIMULATION VALUES
			var radius = GetRandomRadius();
			var mass = GetRandomMass();
            var spawnPoint = GetRandomPointInsideBoard(radius);
			var spawnVelocity = GetRandomVelocity();

            var transform = DataAPI.CreateTransform(spawnPoint, radius);
			var rigidBody = DataAPI.CreateRigidBody(spawnVelocity, mass);

			dataBalls.Add(DataAPI.CreateBall(i, transform, rigidBody));
		}
	}

    private Vector2 GetRandomPointInsideBoard(float ballRadius) {
        var rng = new Random();
        var isPositionIncorrect = true; 
        int x = 0, y = 0, iteration = 0;

        while (isPositionIncorrect) {
            x = rng.Next((int)ballRadius, (int)(BoardSize.X - ballRadius));
            y = rng.Next((int)ballRadius, (int)(BoardSize.Y - ballRadius));

            var transform = DataAPI.CreateTransform(new Vector2(x, y), ballRadius);

            isPositionIncorrect = IsCollideCircles(transform);
            iteration++;

            if (iteration == 100) {
                // NO AVAILABLE POSITION, BREAK
                isPositionIncorrect = false;
            }
        }

        return new Vector2(x, y);
    }

    private bool IsCollideCircles(ITransform transfrom) {
		for (int i = 0; i < dataBalls.GetCount(); i++)
            if (IsCollideCircle(dataBalls.Get(i).Transform, transfrom))
                return true;
        return false;
    }

    private bool IsCollideCircle(ITransform transfrom, ITransform other) {
        var distanceSquare = 
			(transfrom.Position.X - other.Position.X) * (transfrom.Position.X - other.Position.X) + 
			(transfrom.Position.Y - other.Position.Y) * (transfrom.Position.Y - other.Position.Y);
        var radiusSumSquare = (transfrom.Radius + other.Radius) * (transfrom.Radius + other.Radius);
        return distanceSquare <= radiusSumSquare;
    }

    public float GetRandomRadius() {
        const float radiusScale = 50;
        const float radiusMin = 0;

        var rng = new Random();
        return (float)((rng.NextDouble() + radiusMin) * radiusScale);
    }

    public float GetRandomMass() {
        const float radiusScale = 100;
        const float radiusMin = 0.5f;

        var rng = new Random();
        return (float)((rng.NextDouble() + radiusMin) * radiusScale);
    }

    private Vector2 GetRandomVelocity() {
        var rng = new Random();

        var x = (float)((rng.NextDouble() - 0.5) * 400); // 15 - [-7,5; 7.5], 10 - [-5; 5], 400 - [-200, 200]
        var y = (float)((rng.NextDouble() - 0.5) * 400); //

        return new Vector2(x, y);
    }

    public override void AddBall(
        int newIdentifier,
		Vector2 newPosition, 
		float newRadius,
		Vector2 newVelocity,
		float newMass
	) {

		if (newPosition.X < 0 || newPosition.X > BoardSize.X || newPosition.Y < 0 || newPosition.Y > BoardSize.Y)
			throw new PositionIsOutOfBoardException();

        var transform = DataAPI.CreateTransform(newPosition, newRadius);
        var rigidBody = DataAPI.CreateRigidBody(newVelocity, newMass);

        dataBalls.Add(DataAPI.CreateBall(newIdentifier, transform, rigidBody));
	}

	public override void StartSimulation() {
		if (CancelSimulationSource.IsCancellationRequested) return;

		CancelSimulationSource = new CancellationTokenSource();

        for (var i = 0; i < dataBalls.GetCount(); i++) {
			var ball = new BallLogic(i, dataBalls.Get(i), this);

			// ATTACH CALLBACKS
			ball.PositionChange += (ignored, arguments) => OnPositionChange(arguments);
            ball.RadiusChange += (ignored, arguments) => OnRadiusChange(arguments);

            // CREATING THRED'S
            Task.Factory.StartNew(ball.Simulate, CancelSimulationSource.Token);
		}
	}

	public override void StopSimulation() {
		CancelSimulationSource.Cancel();
	}

	public override int GetBallsCount() {
		return dataBalls.GetCount();
	}

    public override IBallData GetBall(int index) {
        return dataBalls.Get(index);
    }

	public override IList<IBallLogic> GetBalls() {
        var ballsList = new List<IBallLogic>();
		for (var i = 0; i < dataBalls.GetCount(); i++) 
			ballsList.Add(new BallLogic(i, dataBalls.Get(i), this));
		return ballsList;
	}
}