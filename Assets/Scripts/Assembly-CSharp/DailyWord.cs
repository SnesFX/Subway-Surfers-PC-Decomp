using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class DailyWord : MonoBehaviour
{
	private string key = string.Empty;

	private int expireSecondsD;

	private int dayNumberD;

	private string wordD;

	private DateTime GMTTimeD;

	private int expireSecondsS;

	private int dayNumberS;

	private string wordS;

	private DateTime GMTTimeS;

	private string secretkey = "aIN0UXP4NNoANVGi5w3raGAFN1n5OLQZFDhwjs6HoX";

	private static DailyWord _instance;

	public static DailyWord Instance
	{
		get
		{
			return _instance ?? (_instance = UnityEngine.Object.FindObjectOfType(typeof(DailyWord)) as DailyWord);
		}
	}

	private void Start()
	{
		ForceSync();
	}

	public void ForceSync()
	{
		StartCoroutine("DownloadDaily");
	}

	private IEnumerator DownloadDaily()
	{
		key = GenerateKey();
		WWWForm postData = new WWWForm();
		postData.AddField("key", key);
		WWW www = new WWW("http://hoodrunner.kiloo.com/hr_dailyquests.php", postData);
		yield return www;
		if (www.error != null)
		{
			Debug.LogError("www.error: " + www.error.ToString());
			yield break;
		}
		string[] rawData = www.text.Split(';');
		if (SHA1HashCheck(rawData))
		{
			StoreLocalVariables();
			StoreWWWResponse(rawData);
			if (!VerifyDay())
			{
				Debug.LogWarning("VerifyDay failed");
			}
			OverWriteDeviceMemory();
			SendWordAndExpireTime();
		}
	}

	private DateTime ConvertFromUnixTimestamp(double timestamp)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
	}

	private double ConvertToUnixTimestamp(DateTime date)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return Math.Floor((date - dateTime).TotalSeconds);
	}

	private void StoreWWWResponse(string[] rawData)
	{
		dayNumberS = Convert.ToInt32(rawData[0]);
		wordS = rawData[1];
		GMTTimeS = new DateTime(Convert.ToInt32(rawData[2]), Convert.ToInt32(rawData[4]), Convert.ToInt32(rawData[5]), Convert.ToInt32(rawData[6]), Convert.ToInt32(rawData[7]), Convert.ToInt32(rawData[8]));
		expireSecondsS = Convert.ToInt32(rawData[9]);
	}

	private bool SHA1HashCheck(string[] rawData)
	{
		if (rawData.Length < 10)
		{
			for (int i = 0; i < rawData.Length; i++)
			{
			}
			return false;
		}
		string text = rawData[3];
		string sHA1Hash = GetSHA1Hash(rawData[0] + rawData[1] + rawData[2] + key + secretkey + rawData[4] + rawData[5] + rawData[6] + rawData[7] + rawData[8] + rawData[9]);
		if (text == sHA1Hash)
		{
			return true;
		}
		return false;
	}

	private void StoreLocalVariables()
	{
		dayNumberD = DateTime.UtcNow.DayOfYear - 1;
		GMTTimeD = DateTime.UtcNow;
	}

	private void OverWriteDeviceMemory()
	{
		wordD = wordS;
		expireSecondsD = expireSecondsS;
	}

	private bool VerifyDay()
	{
		if (Mathf.Abs(dayNumberS - dayNumberD) <= 0)
		{
			return true;
		}
		return false;
	}

	private void SendWordAndExpireTime()
	{
		PlayerInfo.Instance.InitDailyWord(wordD, GMTTimeS.AddSeconds(expireSecondsD));
	}

	private string GetSHA1Hash(string dataString)
	{
		SHA1 sHA = SHA1.Create();
		byte[] bytes = Encoding.ASCII.GetBytes(dataString);
		byte[] array = sHA.ComputeHash(bytes);
		return BitConverter.ToString(array).Replace("-", string.Empty).ToLowerInvariant();
	}

	private string RandomString(int size)
	{
		StringBuilder stringBuilder = new StringBuilder();
		System.Random random = new System.Random();
		for (int i = 0; i < size; i++)
		{
			char value = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * random.NextDouble() + 65.0)));
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	private int RandomNumber(int min, int max)
	{
		System.Random random = new System.Random();
		return random.Next(min, max);
	}

	private string GenerateKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(RandomString(4));
		stringBuilder.Append(RandomNumber(0, 999));
		stringBuilder.Append(RandomString(2));
		return stringBuilder.ToString();
	}
}
