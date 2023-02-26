public class MissionInfo
{
	public Mission mission;

	public MissionTemplate template;

	public int progress;

	public bool complete;

	public MissionInfo(Mission mission, MissionTemplate template, int progress, bool complete)
	{
		this.mission = mission;
		this.template = template;
		this.progress = progress;
		this.complete = complete;
	}
}
