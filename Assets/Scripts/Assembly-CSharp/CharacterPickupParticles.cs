using System.Collections;
using UnityEngine;

public class CharacterPickupParticles : MonoBehaviour
{
	public GameObject CoinEFX;

	public GameObject PowerUpEFX;

	public Transform master;

	private Vector3 offset;

	public AudioClipInfo CoinPickup;

	public AudioClipInfo PowerUpPickup;

	public float CoinDistanceForStairway;

	private float lastCoinPosition;

	private int coinStairway;

	private int flyWay;

	private int[] pentatonicScale = new int[17]
	{
		12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
		22, 23, 24, 25, 26, 27, 28
	};

	public void Awake()
	{
		lastCoinPosition = base.transform.position.z;
		offset = base.transform.position - master.position;
	}

	public void PickedUpCoin(Pickup pickup)
	{
		if (80f < pickup.transform.position.y)
		{
			coinStairway = 0;
			CoinPickup.maxPitch = Mathf.Pow(2f, (float)flyWay / 48f);
			CoinPickup.minPitch = Mathf.Pow(2f, (float)flyWay / 48f);
			flyWay++;
		}
		else if (pickup.transform.position.y < 0.1f || (8.795f < pickup.transform.position.y && pickup.transform.position.y < 8.805f) || (9.95f < pickup.transform.position.y && pickup.transform.position.y < 10.05f) || (28.95f < pickup.transform.position.y && pickup.transform.position.y < 29.05f) || (34.95f < pickup.transform.position.y && pickup.transform.position.y < 35.05f))
		{
			flyWay = 0;
			coinStairway = 0;
			CoinPickup.maxPitch = Mathf.Pow(2f, (float)pentatonicScale[coinStairway % pentatonicScale.Length] / 12f) * 0.5f;
			CoinPickup.minPitch = Mathf.Pow(2f, (float)pentatonicScale[coinStairway % pentatonicScale.Length] / 12f) * 0.5f;
		}
		else
		{
			flyWay = 0;
			if (coinStairway < pentatonicScale.Length - 1)
			{
				coinStairway++;
			}
			CoinPickup.maxPitch = Mathf.Pow(2f, (float)pentatonicScale[coinStairway % pentatonicScale.Length] / 12f) * 0.5f;
			CoinPickup.minPitch = Mathf.Pow(2f, (float)pentatonicScale[coinStairway % pentatonicScale.Length] / 12f) * 0.5f;
		}
		So.Instance.playSound(CoinPickup);
		DoCoinEFX();
		lastCoinPosition = pickup.transform.position.y;
	}

	private void DoCoinEFX()
	{
		float zAngle = Random.Range(0f, 360f);
		CoinEFX.transform.Rotate(0f, 0f, zAngle);
		CoinEFX.GetComponent<Animation>().Stop("pickup");
		CoinEFX.GetComponent<Animation>().Play("pickup");
		StartCoroutine(AnimateAlpha(CoinEFX, CoinEFX.GetComponent<Animation>()["pickup"].length));
	}

	public void PickedUpPowerUp()
	{
		So.Instance.playSound(PowerUpPickup);
		PickedUpDefaultPowerUp();
	}

	public void PickedUpDefaultPowerUp()
	{
		DoCoinEFX();
		float zAngle = Random.Range(0f, 360f);
		PowerUpEFX.transform.Rotate(0f, 0f, zAngle);
		PowerUpEFX.GetComponent<Animation>().Stop("pickup");
		PowerUpEFX.GetComponent<Animation>().Play("pickup");
		StartCoroutine(AnimateAlpha(PowerUpEFX, PowerUpEFX.GetComponent<Animation>()["pickup"].length));
	}

	private IEnumerator AnimateAlpha(GameObject efx, float time)
	{
		return pTween.To(time, delegate(float t)
		{
			base.transform.position = master.position + offset;
			efx.GetComponent<Renderer>().material.SetColor("_MainColor", Color.Lerp(Color.white, Color.black, t));
		});
	}

	private IEnumerator TimeScaleTest(float time)
	{
		Time.timeScale = 0.5f;
		yield return new WaitForSeconds(time);
		Time.timeScale = 1f;
	}
}
