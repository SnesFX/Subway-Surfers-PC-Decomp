using UnityEngine;

[AddComponentMenu("GUI/Model Trigger")]
public class ModelTrigger : MonoBehaviour
{
	public enum ModelPosition
	{
		CharacterScreen = 0,
		LoseScreen = 1
	}

	public ModelPosition modelPosition;
}
