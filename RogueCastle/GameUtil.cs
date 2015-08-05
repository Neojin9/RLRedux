using SteamWorksWrapper;


namespace RogueCastle {
    public static class GameUtil {
        public static void UnlockAchievement(string achievementName) {
            SWManager.instance().unlockAchievement(achievementName);
        }
    }
}
