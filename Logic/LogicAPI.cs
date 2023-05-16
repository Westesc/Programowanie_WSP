using System;
using System.Collections.Generic;
using System.Numerics;
using TPW.Data;

namespace TPW.Logic;

// About
// - Logical part holds the information that doesn't need to be serialized.

public abstract class LogicAPI {

    public event EventHandler<OnBallChangeEventArgs>? BallChange;
    public abstract void AddBalls(int newCount);
	public abstract void AddBall(int newIdentifier, Vector2 newPosition, float newRadius, Vector2 newVelocity, float newMass);
	public abstract void StartSimulation();
	public abstract void StopSimulation();
	public abstract int GetBallsCount();
	public abstract IBallData GetBall(int index);
    public abstract IList<IBallLogic> GetBalls();

	protected virtual void OnPositionChange(OnBallChangeEventArgs newArguments) {
        BallChange?.Invoke(this, newArguments);
	}

	protected virtual void OnRadiusChange(OnBallChangeEventArgs newArguments) {
        BallChange?.Invoke(this, newArguments);
	}

	public static LogicAPI CreateBalls(Vector2 newBoardSize, DataAPI? newDataApi = null) {
        newDataApi ??= DataAPI.CreateBallsList(); // same as if (dataApi == null) dataApi = DataAPI.CreateBallsList();
        return new BallsLogic(newDataApi, newBoardSize);
	}
}