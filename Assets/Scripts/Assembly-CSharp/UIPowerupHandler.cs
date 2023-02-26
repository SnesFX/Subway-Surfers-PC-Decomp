using System.Collections.Generic;
using UnityEngine;

public class UIPowerupHandler : MonoBehaviour
{
	public GameObject PowerupPrefab;

	private Vector3[] slotPositions = new Vector3[4]
	{
		new Vector3(-135f, 10f, 0f),
		new Vector3(20f, 10f, 0f),
		new Vector3(-135f, 50f, 0f),
		new Vector3(20f, 50f, 0f)
	};

	private UIPowerupHelper[] _powerupSlots = new UIPowerupHelper[4];

	private void Update()
	{
		List<ActivePowerup> activePowerups = GameStats.Instance.GetActivePowerups();
		int i = 0;
		for (int num = activePowerups.Count - 1; num >= 0; num--)
		{
			if (_powerupSlots[i] == null)
			{
				GameObject gameObject = NGUITools.AddChild(base.gameObject, PowerupPrefab);
				gameObject.transform.localPosition = slotPositions[i];
				UIPowerupHelper component = gameObject.GetComponent<UIPowerupHelper>();
				component.SetPowerup(activePowerups[num]);
				_powerupSlots[i] = component;
			}
			else
			{
				_powerupSlots[i].SetPowerup(activePowerups[num]);
			}
			i++;
		}
		for (; i < 4; i++)
		{
			if (_powerupSlots[i] != null)
			{
				Object.Destroy(_powerupSlots[i].gameObject);
			}
		}
	}
}
