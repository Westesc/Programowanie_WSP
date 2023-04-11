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

    private readonly DataAPI dataBalls;

    public CancellationTokenSource CancelSimulationSource { get; private set; }			//
    public static readonly int BallRadius = 50;											//

	public BallsLogic(DataAPI newDataBalls, Vector2 newBoardSize) {
        CancelSimulationSource = new CancellationTokenSource();
        dataBalls = newDataBalls;
		BoardSize = newBoardSize;
	}

	public Vector2 BoardSize { get; }

	protected override void OnPositionChange(OnPositionChangeEventArgs newArgs) {
		base.OnPositionChange(newArgs);
	}

	public override void AddBalls(int newCount) {
		for (var i = 0; i < newCount; i++) {
			var randomPoint = GetRandomPointInsideBoard();
			dataBalls.Add(DataAPI.CreateBall(randomPoint));
		}
	}

	private Vector2 GetRandomPointInsideBoard() {
		var rng = new Random();
		var x = rng.Next(BallRadius, (int)(BoardSize.X - BallRadius));
		var y = rng.Next(BallRadius, (int)(BoardSize.Y - BallRadius));

		return new Vector2(x, y);
	}

	public override void AddBall(Vector2 position) {
		if (position.X < 0 || position.X > BoardSize.X || position.Y < 0 || position.Y > BoardSize.Y)
			throw new PositionIsOutOfBoardException();
		dataBalls.Add(DataAPI.CreateBall(position));
	}

	public override void StartSimulation() {
		if (CancelSimulationSource.IsCancellationRequested) return;

		CancelSimulationSource = new CancellationTokenSource();
		for (var i = 0; i < dataBalls.GetCount(); i++) {
			var ball = new BallLogic(dataBalls.Get(i), i, this);
			ball.PositionChange += (_, args) => OnPositionChange(args);
			Task.Factory.StartNew(ball.Simulate, CancelSimulationSource.Token);
		}
	}

	public override void StopSimulation() {
		CancelSimulationSource.Cancel();
	}

	public override int GetBallsCount() {
		return dataBalls.GetCount();
	}

	public override IList<IBallLogic> GetBalls() {
		var ballsList = new List<IBallLogic>();
		for (var i = 0; i < dataBalls.GetCount(); i++) 
			ballsList.Add(new BallLogic(dataBalls.Get(i), i, this));
		return ballsList;
	}
}