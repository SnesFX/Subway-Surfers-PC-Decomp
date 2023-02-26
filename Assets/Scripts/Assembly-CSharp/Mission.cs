public struct Mission
{
	public Missions.MissionType type;

	public int goal;

	public Mission(Missions.MissionType type, int goal)
	{
		this.type = type;
		this.goal = goal;
	}
}
