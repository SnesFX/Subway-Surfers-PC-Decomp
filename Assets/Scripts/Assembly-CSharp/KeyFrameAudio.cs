using System;

[Serializable]
public class KeyFrameAudio : KeyFrameAction
{
	public delegate void ExtraKeyframeCall(KeyFrameAudio info);

	public AudioKeyFrameType audioKeyFrameType;

	public AudioClipInfo Audio;

	public ExtraKeyframeCall Callback;
}
