using SteamWorksWrapper;
using System;
namespace RogueCastle
{
	public static class GameUtil
	{
		public static void UnlockAchievement(string achievementName)
		{
			SWManager.instance().unlockAchievement(achievementName);
		}
	}
}
