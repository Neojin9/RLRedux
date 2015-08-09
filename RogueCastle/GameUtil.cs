using SteamWorksWrapper;


namespace RogueCastle {
    public static class GameUtil {
        public static void UnlockAchievement(string achievementName) {
            return;
            SWManager.instance().unlockAchievement(achievementName);
        }
    }
}
