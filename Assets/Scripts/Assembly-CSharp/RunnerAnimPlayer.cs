public class RunnerAnimPlayer : AnimationSoundPlayer
{
	private Game game;

	public AudioClipInfo run__LeftFoot;

	public AudioClipInfo run__RightFoot;

	public AudioClipInfo superRun__LeftFoot;

	public AudioClipInfo superRun__RightFoot;

	public AudioClipInfo groundLeft;

	public AudioClipInfo groundRight;

	public AudioClipInfo groundLeft_super;

	public AudioClipInfo groundRight_super;

	public AudioClipInfo jump;

	public AudioClipInfo roll;

	public AudioClipInfo landing;

	public AudioClipInfo h_jump;

	public AudioClipInfo h_roll;

	public AudioClipInfo h_kick;

	public AudioClipInfo h_miniKick;

	public AudioClipInfo h_long;

	public AudioClipInfo h_mid;

	public AudioClipInfo h_short;

	public AudioClipInfo h_landingGrind;

	public AudioClipInfo superJump;

	public AudioClipInfo idlePaintSpray;

	public AudioClipInfo idlePaintSprayHigh;

	public AudioClipInfo idlePaintShake;

	public AudioClipInfo deathMovingTrain;

	public AudioClipInfo deathBodyfall;

	public AudioStateLoop audioStateLoop;

	public bool playPaintSound = true;

	private void Awake()
	{
		game = Game.Instance;
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "run",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 6,
			clip = "run",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "run2",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 6,
			clip = "run2",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "run3",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 6,
			clip = "run3",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 6,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 10,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 15,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 20,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 25,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 30,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 35,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 40,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 45,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 50,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 55,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 60,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 65,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 70,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 75,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 80,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 85,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 90,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 95,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 100,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 105,
			clip = "run4_long",
			Audio = run__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 110,
			clip = "run4_long",
			Audio = run__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "superRun",
			Audio = superRun__RightFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 10,
			clip = "superRun",
			Audio = superRun__LeftFoot
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "dodgeLeft",
			Audio = groundLeft
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "dodgeRight",
			Audio = groundRight
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "dodgeLeft",
			Audio = groundLeft
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "dodgeRight",
			Audio = groundRight
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "landing",
			Audio = landing
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "jump",
			Audio = jump,
			Callback = PlayJumpSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "jump_salto",
			Audio = jump,
			Callback = PlayJumpSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "jump2",
			Audio = jump,
			Callback = PlayJumpSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "jump3",
			Audio = jump,
			Callback = PlayJumpSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "jump",
			Audio = groundLeft
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "jump_salto",
			Audio = groundRight
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "jump2",
			Audio = groundLeft
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "jump3",
			Audio = groundLeft
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "roll",
			Audio = roll
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "slide_roll2",
			Audio = roll
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "roll",
			Audio = groundRight
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "slide_roll2",
			Audio = groundRight
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "h_landing",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "h_roll",
			Audio = h_roll
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 4,
			clip = "h_roll",
			Audio = h_miniKick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "h_jump",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump",
			Audio = h_long
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "h_jump2_kickflip",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 4,
			clip = "h_jump2_kickflip",
			Audio = h_long
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 21,
			clip = "h_jump2_kickflip",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "h_jump3_180",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump3_180",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "h_jump3_180",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "h_jump4_360flip",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump4_360flip",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "h_jump4_360flip",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 13,
			clip = "h_jump4_360flip",
			Audio = h_miniKick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 19,
			clip = "h_jump4_360flip",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "h_jump5_Impossible",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump5_Impossible",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "h_jump5_Impossible",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 11,
			clip = "h_jump5_Impossible",
			Audio = h_miniKick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 22,
			clip = "h_jump5_Impossible",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "h_jump6_nollie",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump6_nollie",
			Audio = h_long
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump6_nollie",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 21,
			clip = "h_jump6_nollie",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 3,
			clip = "h_jump7_heelflip",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump7_heelflip",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "h_jump7_heelflip",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 13,
			clip = "h_jump7_heelflip",
			Audio = h_miniKick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 18,
			clip = "h_jump7_heelflip",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "h_jump8_pop shuvit",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump8_pop shuvit",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 4,
			clip = "h_jump8_pop shuvit",
			Audio = h_miniKick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 10,
			clip = "h_jump8_pop shuvit",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump9_fs360",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 3,
			clip = "h_jump9_fs360",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 10,
			clip = "h_jump9_fs360",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 20,
			clip = "h_jump9_fs360",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			clip = "h_jump10_heel360",
			Audio = h_miniKick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 4,
			clip = "h_jump10_heel360",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 5,
			clip = "h_jump10_heel360",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "h_jump10_heel360",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 11,
			clip = "h_jump10_heel360",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 14,
			clip = "h_jump10_heel360",
			Audio = h_miniKick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 20,
			clip = "h_jump10_heel360",
			Audio = h_mid
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "h_jump11_fs salto",
			Audio = h_kick
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "h_jump11_fs salto",
			Audio = h_long
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "h_jump11_fs salto",
			Audio = h_short
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "landing_grind1",
			Audio = h_landingGrind
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "landing_grind2",
			Audio = h_landingGrind
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "landing_grind3",
			Audio = h_landingGrind
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 1,
			clip = "idlePaint",
			Audio = idlePaintSpray,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 28,
			clip = "idlePaint",
			Audio = idlePaintShake,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 100,
			clip = "idlePaint",
			Audio = idlePaintShake,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 150,
			clip = "idlePaint",
			Audio = idlePaintSprayHigh,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 200,
			clip = "idlePaint",
			Audio = idlePaintSpray,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 237,
			clip = "idlePaint",
			Audio = idlePaintSpray,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 260,
			clip = "idlePaint",
			Audio = idlePaintShake,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 310,
			clip = "idlePaint",
			Audio = idlePaintSpray,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 350,
			clip = "idlePaint",
			Audio = idlePaintSpray,
			Callback = PlayIdlePaintSound
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 13,
			clip = "death_movingTrain",
			Audio = deathMovingTrain
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 8,
			clip = "death_lower",
			Audio = deathBodyfall
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 11,
			clip = "death_bounce",
			Audio = deathBodyfall
		});
		AudioClips.Add(new KeyFrameAudio
		{
			KeyFrame = 2,
			clip = "death_upper",
			Audio = deathBodyfall
		});
	}

	public void PlayOrMutePaintSound(bool doPlay)
	{
		playPaintSound = doPlay;
	}

	public void PlayIdlePaintSound(KeyFrameAudio info)
	{
		if (playPaintSound)
		{
			So.Instance.playSound(info.Audio);
		}
	}

	public void PlayJumpSound(KeyFrameAudio info)
	{
		if (game.HasSuperSneakers)
		{
			So.Instance.playSound(superJump);
		}
		else
		{
			So.Instance.playSound(info.Audio);
		}
	}

	public override void PlayKeyframeAnimation(int soundIndex)
	{
		KeyFrameAudio keyFrameAudio = AudioClips[soundIndex];
		if (keyFrameAudio.Callback != null)
		{
			keyFrameAudio.Callback(keyFrameAudio);
		}
		else
		{
			So.Instance.playSound(AudioClips[soundIndex].Audio);
		}
	}
}
