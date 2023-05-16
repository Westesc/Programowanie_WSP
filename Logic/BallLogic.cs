using Data.Components;
using System;
using System.Numerics;
using System.Threading;
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
		ball = DataAPI.CreateBall(newTransform, newRigidBody);
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
		while (!owner.CancelSimulationSource.Token.IsCancellationRequested) {

            // Change Values
            Position = MoveInsideBoard(Radius); 

			// Apply Values
            PositionChange?.Invoke(this, new OnBallChangeEventArgs(this));

            await Task.Delay(16, owner.CancelSimulationSource.Token).ContinueWith(ignored => { });
		}
	}

    // UTIL
    public Vector2 MoveInsideBoard(float ballRadius) {
		Vector2 newPosition = Position + Velocity;

		if (newPosition.X < 0 || newPosition.X > owner.BoardSize.X - ballRadius)
			Velocity = new Vector2(-Velocity.X, Velocity.Y);

		if (newPosition.Y < 0 || newPosition.Y > owner.BoardSize.Y - ballRadius)
            Velocity = new Vector2(Velocity.X, -Velocity.Y);

        return Position + Velocity;
	}

}