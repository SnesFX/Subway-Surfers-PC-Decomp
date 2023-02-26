using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class DownloadTexture : MonoBehaviour
{
	public string url = "http://www.tasharen.com/misc/logo.png";

	private Material mMat;

	private Texture2D mTex;

	private IEnumerator Start()
	{
		WWW www = new WWW(url);
		yield return www;
		mTex = www.texture;
		if (mTex != null)
		{
			UITexture ut = GetComponent<UITexture>();
			if (ut.material == null)
			{
				Shader shader = Shader.Find("Unlit/Transparent Colored");
				mMat = new Material(shader);
				ut.material = mMat;
			}
			ut.material.mainTexture = mTex;
			ut.MakePixelPerfect();
		}
		www.Dispose();
	}

	private void OnDestroy()
	{
		if (mMat != null)
		{
			Object.Destroy(mMat);
		}
		if (mTex != null)
		{
			Object.Destroy(mTex);
		}
	}
}
