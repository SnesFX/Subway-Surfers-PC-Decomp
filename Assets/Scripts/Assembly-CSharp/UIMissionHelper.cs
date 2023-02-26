using UnityEngine;

public class UIMissionHelper : MonoBehaviour
{
	public UICheckbox MissionCheckBox1;

	public UICheckbox MissionCheckBox2;

	public UICheckbox MissionCheckBox3;

	public UILabel MissionLabel1;

	public UILabel MissionLabel2;

	public UILabel MissionLabel3;

	public UILabel MissionNumber1;

	public UILabel MissionNumber2;

	public UILabel MissionNumber3;

	private int[] _cachedMissionProgressions = new int[3];

	private int _cachedMissionSet;

	private bool hasCached;

	private bool hasDestroyedGameObjects;

	private MissionInfo[] _currentMissions;

	private bool timeDeathMission;

	private void Update()
	{
		if (hasDestroyedGameObjects)
		{
			return;
		}
		if (!Missions.Instance.HasMoreMissions())
		{
			MissionLabel2.text = "No missions available";
			Object.Destroy(MissionCheckBox1.gameObject);
			Object.Destroy(MissionCheckBox3.gameObject);
			foreach (Transform item in MissionCheckBox2.transform)
			{
				if (item.gameObject != MissionLabel2.gameObject)
				{
					Object.Destroy(item.gameObject);
				}
			}
			Object.Destroy(MissionCheckBox2);
			hasDestroyedGameObjects = true;
			return;
		}
		_currentMissions = Missions.Instance.GetMissionInfo();
		if (hasCached)
		{
			bool flag = true;
			for (int i = 0; i < 3; i++)
			{
				if (_cachedMissionProgressions[i] != _currentMissions[i].progress)
				{
					flag = false;
				}
			}
			if (_cachedMissionSet != Missions.Instance.currentMissionSet)
			{
				flag = false;
			}
			if (flag)
			{
				return;
			}
		}
		MissionLabel2.text = string.Format(_currentMissions[1].template.description, _currentMissions[1].mission.goal, _currentMissions[1].mission.goal - _currentMissions[1].progress);
		MissionLabel3.text = string.Format(_currentMissions[2].template.description, _currentMissions[2].mission.goal, _currentMissions[2].mission.goal - _currentMissions[2].progress);
		MissionCheckBox1.isChecked = _currentMissions[0].complete;
		MissionCheckBox2.isChecked = _currentMissions[1].complete;
		MissionCheckBox3.isChecked = _currentMissions[2].complete;
		hasCached = true;
		LabelAndNumberUpdate(0, MissionLabel1, MissionNumber1);
		LabelAndNumberUpdate(1, MissionLabel2, MissionNumber2);
		LabelAndNumberUpdate(2, MissionLabel3, MissionNumber3);
		_cachedMissionProgressions[0] = _currentMissions[0].progress;
		_cachedMissionProgressions[1] = _currentMissions[1].progress;
		_cachedMissionProgressions[2] = _currentMissions[2].progress;
		_cachedMissionSet = Missions.Instance.currentMissionSet;
	}

	private void LabelAndNumberUpdate(int missionArrayNr, UILabel sendMissionLabel, UILabel sendMissionNumber)
	{
		if (_currentMissions[missionArrayNr].complete)
		{
			string format = ((_currentMissions[missionArrayNr].mission.goal != 1) ? _currentMissions[missionArrayNr].template.ultraShortDescription : _currentMissions[missionArrayNr].template.ultraShortDescriptionSingle);
			sendMissionLabel.text = string.Format(format, _currentMissions[missionArrayNr].mission.goal);
			sendMissionNumber.text = string.Empty;
			return;
		}
		string format2 = ((_currentMissions[missionArrayNr].mission.goal != 1) ? _currentMissions[missionArrayNr].template.description : _currentMissions[missionArrayNr].template.descriptionSingle);
		if (_currentMissions[missionArrayNr].mission.type == Missions.MissionType.TimeDeath)
		{
			if (Game.Instance.isPaused)
			{
				sendMissionLabel.text = string.Format(format2, _currentMissions[missionArrayNr].mission.goal, (int)Game.Instance.GetDuration());
				hasCached = false;
			}
			else
			{
				sendMissionLabel.text = string.Format(format2, _currentMissions[missionArrayNr].mission.goal, 0);
			}
		}
		else
		{
			sendMissionLabel.text = string.Format(format2, _currentMissions[missionArrayNr].mission.goal, _currentMissions[missionArrayNr].mission.goal - _currentMissions[missionArrayNr].progress);
		}
		sendMissionNumber.text = missionArrayNr + 1 + string.Empty;
	}
}
