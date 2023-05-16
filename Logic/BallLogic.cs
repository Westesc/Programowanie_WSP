using Data.Components;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using TPW.Data;

namespace TPW.Logic;

// About
// 

public interface IBallLogic {
	Vector2 Position { get; set; }
	float Radius { get; set; }
	Vector2 Velocity { get; set; }
	float Mass { get; set; }
	int Id { get; }
}

internal class BallLogic : IBallLogic {

	private readonly IBallData ball;
	private readonly BallsLogic owner;
	private readonly Random random;

	public event EventHandler<OnBallChangeEventArgs>? PositionChange;
    public event EventHandler<OnBallChangeEventArgs>? RadiusChange;

    // CONSTRUCTORS

    public BallLogic(int newId, IBallData newBall, BallsLogic newOwner) {
        random = new Random();
        owner = newOwner;
        ball = newBall;
        Id = newId;
	}

	public BallLogic(int newId, ITransform newTransform, IRigidBody newRigidBody, BallsLogic newOwner) {
		ball = DataAPI.CreateBall(newId, newTransform, newRigidBody);
        random = new Random();
        owner = newOwner;
        Id = newId;
	}

	// PROPERTIES

    public int Id { get; private set; }

    public Vector2 Position {
		get => ball.Transform.Position;
		set => ball.Transform.Position = value;
	}

	public float Radius {
        get => ball.Transform.Radius;
        set => ball.Transform.Radius = value;
    }

	public Vector2 Velocity {
        get => ball.RigidBody.Velocity;
        set => ball.RigidBody.Velocity = value;
    }

    public float Mass {
        get => ball.RigidBody.Mass;
        set => ball.RigidBody.Mass = value;
    }

    // FUNCTIONS

    public async void Simulate() {
        var timer = new Stopwatch();
        float deltaTime = 0.0f;

        while (!owner.CancelSimulationSource.Token.IsCancellationRequested) {

            timer.Start();

            // Change Values
            Position = MoveInsideBoard(Radius, deltaTime); 

			// Apply Values
            PositionChange?.Invoke(this, new OnBallChangeEventArgs(this));

            await Task.Delay(4, owner.CancelSimulationSource.Token).ContinueWith(ignored => { });

            timer.Stop();
            deltaTime = timer.ElapsedMilliseconds / 1000.0f;
            timer.Reset();

        }
	}

    // UTIL

    public Vector2 MoveInsideBoard(float ballRadius, float deltaTime) {
        
        for (int i = 0; i < owner.GetBallsCount(); i++) {
            var other = owner.GetBall(i);

            if (other.Identifier == Id) continue;

            owner.simulationPause.WaitOne();
            try {
                if (BallCollisionLogic.IsBallsCollides(ball, other))
                    BallCollisionLogic.HandleCollision(ball, other);
            } finally {
                owner.simulationPause.ReleaseMutex();
            }

        }

        Vector2 newPosition = Position + Vector2.Multiply(Velocity, deltaTime);

        // BOUNDRY CLAMP
        if (newPosition.X < 0) {
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
            newPosition.X = 0;
        } else if (newPosition.X > owner.BoardSize.X - ballRadius) {
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
            newPosition.X = owner.BoardSize.X - ballRadius;
        }

        if (newPosition.Y < 0) {
            Velocity = new Vector2(Velocity.X, -Velocity.Y);
            newPosition.Y = 0;
        } else if (newPosition.Y > owner.BoardSize.Y - ballRadius) {
            Velocity = new Vector2(Velocity.X, -Velocity.Y);
            newPosition.Y = owner.BoardSize.Y -ballRadius;
        }

        return newPosition;
	}

}