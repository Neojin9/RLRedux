using System;
using System.Runtime.InteropServices;


namespace RogueCastle {
    public static class SleepUtil {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern SleepUtil.EXECUTION_STATE SetThreadExecutionState(SleepUtil.EXECUTION_STATE esFlags);

        public static void DisableScreensaver() {
            if (SleepUtil.SetThreadExecutionState((SleepUtil.EXECUTION_STATE)2147483715u) == 0u)
                SleepUtil.SetThreadExecutionState((SleepUtil.EXECUTION_STATE)2147483651u);
        }

        public static void EnableScreensaver() {
            SleepUtil.SetThreadExecutionState((SleepUtil.EXECUTION_STATE)2147483648u);
        }

        #region Nested type: EXECUTION_STATE

        [Flags]
        private enum EXECUTION_STATE : uint {
            ES_AWAYMODE_REQUIRED = 64u,
            ES_CONTINUOUS = 2147483648u,
            ES_DISPLAY_REQUIRED = 2u,
            ES_SYSTEM_REQUIRED = 1u
        }

        #endregion
    }
}
