using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinJumpCurve : MonoBehaviour
{
	public float speed = 100f;

	public float curveOffset;

	public float coinSpacing = 15f;

	public float beginRatio;

	public float endRatio = 1f;

	public bool superSneakers;

	private Game game;

	private Character character;

	private Running running;

	private static CoinPool coinPool;

	private int previewSteps = 10;

	private List<Transform> coins = new List<Transform>();

	private Group group;

	private bool Initialiseret;

	private int activation;

	private float JumpHeight
	{
		get
		{
			return (!superSneakers) ? character.jumpHeightNormal : character.jumpHeightSuperSneakers;
		}
	}

	public void Awake()
	{
		game = Game.Instance;
		running = Running.Instance;
		character = Character.Instance;
		if (coinPool == null)
		{
			coinPool = CoinPool.Instance;
		}
		TrackObject component = GetComponent<TrackObject>();
		component.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(component.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
		component.OnDeactivate = (TrackObject.OnDeactivateDelegate)Delegate.Combine(component.OnDeactivate, new TrackObject.OnDeactivateDelegate(OnDeactivate));
		group = Utils.FindComponentInThisOrParents<Group>(base.transform);
	}

	public void OnActivate()
	{
		if (activation == 1)
		{
			Debug.Log("CoinJumpCurve has been activate twice. " + Utils.GetLongName(base.transform));
			Debug.Break();
		}
		activation++;
		float num = character.JumpLength(game.currentLevelSpeed, JumpHeight);
		for (float num2 = beginRatio * num; num2 < endRatio * num; num2 += coinSpacing)
		{
			Transform coin = coinPool.GetCoin();
			coin.parent = base.transform;
			coin.position = CalcJumpCurve(num2 / num);
			TrackObject component = coin.GetComponent<TrackObject>();
			component.Activate();
			coins.Add(coin);
		}
		if (group != null)
		{
			group.UpdateChildren();
		}
	}

	public void OnDeactivate()
	{
		foreach (Transform coin in coins)
		{
			coin.GetComponent<TrackObject>().OnDeactivate();
		}
		activation--;
		coinPool.Put(coins);
		coins.Clear();
	}

	private float NormalizedJumpCurve(float z)
	{
		return 4f * z * (1f - z);
	}

	private float InvertedSpeed(float z)
	{
		return NormalizedJumpCurve(z) / Mathf.Sqrt(1f + Mathf.Pow(-8f * z + 4f, 2f));
	}

	private Vector3 CalcJumpCurve(float ratio)
	{
		return CalcJumpCurve(ratio, game.currentLevelSpeed);
	}

	private Vector3 CalcJumpCurve(float ratio, float speed)
	{
		return base.transform.position + base.transform.forward * character.JumpLength(speed, JumpHeight) * (ratio - curveOffset) + base.transform.up * NormalizedJumpCurve(ratio) * JumpHeight;
	}

	public void OnDrawGizmos()
	{
		if (game == null)
		{
			game = Game.Instance;
		}
		if (character == null)
		{
			character = Character.Instance;
		}
		if (running == null)
		{
			running = Running.Instance;
		}
		DrawCurve(game.speed.min, Color.grey);
		DrawCurve(game.speed.max, Color.grey);
		DrawCurve(speed, Color.yellow);
	}

	private void DrawCurve(float speed, Color color)
	{
		Gizmos.color = color;
		Vector3 from = CalcJumpCurve(beginRatio, speed);
		for (int i = 0; i < previewSteps; i++)
		{
			Vector3 vector = CalcJumpCurve((endRatio - beginRatio) * (float)i / (float)(previewSteps - 1) + beginRatio, speed);
			Gizmos.DrawLine(from, vector);
			from = vector;
		}
	}
}
