public class UISlideInMissionSetHelper : UISlideIn
{
	public UILabel line2;

	public void SetupSlideInMissionSet(int multiplier)
	{
		base.gameObject.SetActiveRecursively(true);
		line2.text = "Points x" + multiplier;
		SlideIn();
	}
}
