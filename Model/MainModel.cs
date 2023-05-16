using System;
using System.Numerics;
using TPW.Logic;

public class OnPositionChangeUiAdapterEventArgs : EventArgs {

    public readonly int id;
    public readonly Vector2 position;

    public OnPositionChangeUiAdapterEventArgs(int newId, Vector2 newPosition) {
        position = newPosition;
        id = newId;
    }
}

public class OnRadiusChangeUiAdapterEventArgs : EventArgs {
    public readonly int id;
    public readonly float radius;

    public OnRadiusChangeUiAdapterEventArgs(int newId, float newRadius) {
        radius = newRadius;
        id = newId;
    }
}

namespace TPW.Presentation.Model {

    public class MainModel {
        private readonly Vector2 boardSize;
        private int ballsAmount;
        private LogicAPI ballsLogic;

        public event EventHandler<OnPositionChangeUiAdapterEventArgs>? BallPositionChange;
        public event EventHandler<OnRadiusChangeUiAdapterEventArgs>? BallRadiusChange;

        public MainModel() {
            boardSize = new Vector2(650, 400);
            ballsAmount = 0;
            ballsLogic = LogicAPI.CreateBalls(boardSize);
            ballsLogic.BallChange += (sender, arguments) => {
                BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(arguments.Ball.Id, arguments.Ball.Position));
                BallRadiusChange?.Invoke(this, new OnRadiusChangeUiAdapterEventArgs(arguments.Ball.Id, arguments.Ball.Radius));
            };
        }

        public void StartSimulation() {
            ballsLogic.AddBalls(ballsAmount);
            ballsLogic.StartSimulation();
        }

        public void StopSimulation() {
            ballsLogic.StopSimulation();

            // RESET SO WE CAN START AGAIN
            ballsLogic = LogicAPI.CreateBalls(boardSize);
            ballsLogic.BallChange += (sender, arguments) => {
                BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(arguments.Ball.Id, arguments.Ball.Position));
                BallRadiusChange?.Invoke(this, new OnRadiusChangeUiAdapterEventArgs(arguments.Ball.Id, arguments.Ball.Radius));
            };
        }

        public void SetBallNumber(int amount) { ballsAmount = amount; }

        public int GetBallsCount() { return ballsAmount; }

        public void OnBallPositionChange(OnPositionChangeUiAdapterEventArgs args) {
            BallPositionChange?.Invoke(this, args);
        }
    }

}
