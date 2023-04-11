using System.Numerics;

namespace TPW.Data;

// About
//  It is the interface we're sharing outside.

public abstract class DataAPI {
	public abstract void Add(IBallData newBall);
	public abstract IBallData Get(int index);
	public abstract int GetCount();
	public static DataAPI CreateBallsList() { return new BallsData(); }
	public static IBallData CreateBall(Vector2 newPosition) { return new BallData(newPosition); }
}