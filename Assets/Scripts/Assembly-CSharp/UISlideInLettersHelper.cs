public class UISlideInLettersHelper : UISlideIn
{
	public void SetupLetters()
	{
		base.gameObject.SetActiveRecursively(true);
		SlideIn();
	}
}
