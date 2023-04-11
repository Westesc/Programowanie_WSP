using System.Collections.Generic;
using TPW.Data;

namespace TPW.Logic.Tests;

public class DataLayerFixture : DataAPI {
	private readonly List<IBallData> ballsList;

	public DataLayerFixture() {
		ballsList = new List<IBallData>();
	}

	public override void Add(IBallData ball) {
		ballsList.Add(ball);
	}

	public override IBallData Get(int index) {
		return ballsList[index];
	}

	public override int GetCount() {
		return ballsList.Count;
	}
}