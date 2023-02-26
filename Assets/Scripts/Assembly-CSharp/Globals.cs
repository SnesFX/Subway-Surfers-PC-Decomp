public class Globals
{
	public const bool DEBUG = false;

	public const bool DEBUG_FREE_PURCHASES = false;

	public const bool DEBUG_ALL_CHARS = false;

	public const bool DEBUG_ALWAYS_ONLINE = false;

	public const bool DEBUG_FREE_INAPP_PURCHASE = false;

	public static Friend[] GetDebugFriends(int numberOfFriends = 10)
	{
		return new Friend[numberOfFriends];
	}
}
