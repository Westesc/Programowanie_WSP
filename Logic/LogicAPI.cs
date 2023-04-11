using System;
using System.Collections.Generic;
using System.Numerics;
using TPW.Data;

namespace TPW.Logic;

// About
// - Logical part holds the information that doesn't need to be serialized.

public class OnPositionChangeEventArgs : EventArgs {
	public readonly IBallLogic Ball;

	public OnPositionChangeEventArgs(IBallLogic newBall) {
		Ball = newBall;
	}
}

public abstract class LogicAPI {

	public event EventHandler<OnPositionChangeEventArgs>? PositionChange;
	public abstract void AddBalls(int newCount);
	public abstract void AddBall(Vector2 newPosition);
	public abstract void StartSimulation();
	public abstract void StopSimulation();
	public abstract int GetBallsCount();
	public abstract IList<IBallLogic> GetBalls();

	protected virtual void OnPositionChange(OnPositionChangeEventArgs newArgs) {
		PositionChange?.Invoke(this, newArgs);
	}

	public static LogicAPI CreateBalls(Vector2 newBoardSize, DataAPI? newDataApi = null) {
        newDataApi ??= DataAPI.CreateBallsList(); // same as if (dataApi == null) dataApi = DataAPI.CreateBallsList();
        return new BallsLogic(newDataApi, newBoardSize);
	}
}