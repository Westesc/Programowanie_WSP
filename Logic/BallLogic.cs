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
	int Id { get; }
}

internal class BallLogic : IBallLogic {

	private readonly IBallData ball;
	private readonly BallsLogic owner;
	private readonly Random random;

	public event EventHandler<OnPositionChangeEventArgs>? PositionChange;

	public int Id { get; private set; }

	public BallLogic(IBallData newBall, int newId, BallsLogic newOwner) {
        random = new Random();
        owner = newOwner;
        ball = newBall;
        Id = newId;
	}

	public BallLogic(Vector2 newPosition, int newId, BallsLogic newOwner) {
		ball = DataAPI.CreateBall(newPosition);
        random = new Random();
        owner = newOwner;
        Id = newId;
	}

	public Vector2 Position {
		get => ball.Position;
		set => ball.Position = value;
	}

    public async void Simulate() {
		while (!owner.CancelSimulationSource.Token.IsCancellationRequested) {
			Position = GetRandomPointInsideBoard();
			PositionChange?.Invoke(this, new OnPositionChangeEventArgs(this));

			await Task.Delay(32, owner.CancelSimulationSource.Token).ContinueWith(_ => { });
		}
	}

	private Vector2 GetRandomPointInsideBoard() {
		Vector2 translationVector = GetRandomNormalizedVector();
		Vector2 newPosition = Position + translationVector;

		if(newPosition.X < BallsLogic.BallRadius || newPosition.X > owner.BoardSize.X - BallsLogic.BallRadius)
			translationVector.X = - translationVector.X;

		if (newPosition.Y < BallsLogic.BallRadius || newPosition.Y > owner.BoardSize.Y - BallsLogic.BallRadius)
			translationVector.Y = - translationVector.Y;

		return Position + translationVector;
	}

	
	private Vector2 GetRandomNormalizedVector() {
		var x = (float)(random.NextDouble() - 0.5) * 2;
		var y = (float)(random.NextDouble() - 0.5) * 2;
		var result = new Vector2(x, y);

		return Vector2.Normalize(result);
	}
}