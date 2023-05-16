using System;

namespace TPW.Logic;

public class OnBallChangeEventArgs : EventArgs {
	public readonly IBallLogic Ball;

	public OnBallChangeEventArgs(IBallLogic newBall) {
		Ball = newBall;
	}
}
