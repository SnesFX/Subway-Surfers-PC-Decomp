public class UISlideInCharacterUnlock : UISlideIn
{
	public UILabel CharacterName;

	public void SetupSlideInCharacter(string message)
	{
		base.gameObject.SetActiveRecursively(true);
		CharacterName.text = message.ToUpper();
		SlideIn();
	}
}
