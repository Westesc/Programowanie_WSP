﻿using TPW.Data.Components;
using System;
using System.Numerics;
using System.Threading;

namespace TPW.Data;

// About
//  It is the interface we're sharing outside.

public abstract class DataAPI {
    public abstract void Add(IBallData newBall);
	public abstract IBallData Get(int index);
	public abstract int GetCount();
	public static DataAPI CreateBallsList() { return new BallsData(); }
    public static ITransform CreateTransform(Vector2 newPosition, float newRadius) { return new Transform(newPosition, newRadius); }
    public static IRigidBody CreateRigidBody(Vector2 newVelocity, float newMass) { return new RigidBody(newVelocity, newMass); }
	public static IBallData CreateBall(int newIdentifier, ITransform newTransfrom, IRigidBody newRigidBody) { return new BallData(newIdentifier, newTransfrom, newRigidBody); }
}