using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Friend
{
	public class Status
	{
		public DateTime lastPokeTime = DateTime.MinValue;

		public int gamesCashedIn;
	}

	public int userid;

	public int score;

	public int meters;

	public int games;

	public IUserProfile gcProfile;

	public FacebookProfile fbProfile;

	public Status status;

	public string name
	{
		get
		{
			if (fbProfile != null)
			{
				return fbProfile.name;
			}
			if (gcProfile != null)
			{
				return gcProfile.userName;
			}
			Debug.LogError("Friend not initialized");
			return null;
		}
	}

	public Texture2D image
	{
		get
		{
			if (fbProfile != null)
			{
				return fbProfile.image;
			}
			if (gcProfile != null)
			{
				return gcProfile.image;
			}
			Debug.LogError("Friend not initialized");
			return null;
		}
	}

	public int relation
	{
		get
		{
			int num = 0;
			if (fbProfile != null)
			{
				num |= 1;
			}
			if (gcProfile != null)
			{
				num |= 2;
			}
			return num;
		}
	}

	public string id
	{
		get
		{
			if (fbProfile != null)
			{
				return fbProfile.id;
			}
			if (gcProfile != null)
			{
				return gcProfile.id;
			}
			return null;
		}
	}

	public int gamesToCashIn
	{
		get
		{
			return games - status.gamesCashedIn;
		}
	}
}
