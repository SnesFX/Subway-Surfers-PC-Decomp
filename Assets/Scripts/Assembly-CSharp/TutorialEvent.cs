using System.Collections;
using UnityEngine;

public class TutorialEvent : MonoBehaviour
{
	private Game game;

	public bool displayText;

	public string text;

	public bool displayMesh;

	public GameObject mesh;

	public float direction;

	public float time = 1f;

	public bool endTutorial;

	public bool allowHoverboard;

	private Hoverboard hoverboard;

	private Character character;

	private Track track;

	private bool Initialiseret;

	public void Awake()
	{
		game = Game.Instance;
		hoverboard = Hoverboard.Instance;
	}

	public void Update()
	{
		if (!(game == null) && !Initialiseret)
		{
			character = game.character;
			track = game.track;
			Initialiseret = true;
		}
	}

	private IEnumerator ShowArrow()
	{
		mesh.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		mesh.transform.Rotate(new Vector3(0f, 0f, 1f), direction);
		mesh.active = true;
		Vector3 pos = new Vector3(0f, 0f, 20f);
		yield return StartCoroutine(pTween.To(time, delegate(float t)
		{
			mesh.transform.localPosition = Vector3.Lerp(pos - mesh.transform.up * 5f, pos + mesh.transform.up * 5f, t);
			mesh.GetComponent<Renderer>().material.mainTextureOffset = Vector2.Lerp(Vector2.zero, new Vector2(0f, -0.02f), t);
		}));
		mesh.active = false;
	}

	private void OnTriggerExit(Collider collider)
	{
		if (!character.stopColliding && collider.gameObject.name.Equals("Character"))
		{
			if (displayText)
			{
				UIScreenController.Instance.QueueMessage(text);
			}
			if (displayMesh)
			{
				StartCoroutine(ShowArrow());
			}
			if (allowHoverboard)
			{
				hoverboard.isAllowed = true;
			}
			if (endTutorial)
			{
				track.IsRunningOnTutorialTrack = false;
				PlayerInfo.Instance.tutorialCompleted = true;
				track.tutorial = false;
			}
		}
	}
}
