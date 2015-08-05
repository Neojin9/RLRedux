namespace RogueCastle {
    public class LevelEV {
        public const int ENEMY_LEVEL_DIFFICULTY_MOD = 32;
        public const float ENEMY_LEVEL_FAKE_MULTIPLIER = 2.75f;
        public const int ROOM_LEVEL_MOD = 4;
        public const byte TOTAL_JOURNAL_ENTRIES = 25;
        public const int ENEMY_EXPERT_LEVEL_MOD = 4;
        public const int ENEMY_MINIBOSS_LEVEL_MOD = 7;
        public const int LAST_BOSS_MODE1_LEVEL_MOD = 8;
        public const int LAST_BOSS_MODE2_LEVEL_MOD = 10;
        public const int CASTLE_ROOM_LEVEL_BOOST = 0;
        public const int GARDEN_ROOM_LEVEL_BOOST = 2;
        public const int TOWER_ROOM_LEVEL_BOOST = 4;
        public const int DUNGEON_ROOM_LEVEL_BOOST = 6;
        public const int NEWGAMEPLUS_LEVEL_BASE = 128;
        public const int NEWGAMEPLUS_LEVEL_APPRECIATION = 128;
        public const int NEWGAMEPLUS_MINIBOSS_LEVEL_BASE = 0;
        public const int NEWGAMEPLUS_MINIBOSS_LEVEL_APPRECIATION = 0;
        public const bool LINK_TO_CASTLE_ONLY = true;
        public const byte CASTLE_BOSS_ROOM = 1;
        public const byte TOWER_BOSS_ROOM = 6;
        public const byte DUNGEON_BOSS_ROOM = 7;
        public const byte GARDEN_BOSS_ROOM = 5;
        public const byte LAST_BOSS_ROOM = 2;
        public const int LEVEL_CASTLE_LEFTDOOR = 90;
        public const int LEVEL_CASTLE_RIGHTDOOR = 90;
        public const int LEVEL_CASTLE_TOPDOOR = 90;
        public const int LEVEL_CASTLE_BOTTOMDOOR = 90;
        public const int LEVEL_GARDEN_LEFTDOOR = 70;
        public const int LEVEL_GARDEN_RIGHTDOOR = 100;
        public const int LEVEL_GARDEN_TOPDOOR = 45;
        public const int LEVEL_GARDEN_BOTTOMDOOR = 45;
        public const int LEVEL_TOWER_LEFTDOOR = 45;
        public const int LEVEL_TOWER_RIGHTDOOR = 45;
        public const int LEVEL_TOWER_TOPDOOR = 100;
        public const int LEVEL_TOWER_BOTTOMDOOR = 60;
        public const int LEVEL_DUNGEON_LEFTDOOR = 55;
        public const int LEVEL_DUNGEON_RIGHTDOOR = 55;
        public const int LEVEL_DUNGEON_TOPDOOR = 45;
        public const int LEVEL_DUNGEON_BOTTOMDOOR = 100;
        public const string GAME_VERSION = "v1.2.0b";

        public static byte[] DEMENTIA_FLIGHT_LIST = new byte[] {
            9,
            11,
            6,
            7,
            3,
            8,
            31
        };

        public static byte[] DEMENTIA_GROUND_LIST = new byte[] {
            15,
            12,
            2,
            1,
            16,
            20,
            13,
            22,
            28,
            10
        };

        public static byte[] CASTLE_ENEMY_LIST = new byte[] {
            15,
            12,
            9,
            11,
            6,
            3,
            16,
            20,
            8,
            32,
            31,
            28
        };

        public static byte[] GARDEN_ENEMY_LIST = new byte[] {
            15,
            2,
            1,
            5,
            9,
            6,
            7,
            14,
            3,
            19,
            22,
            25,
            31,
            10
        };

        public static byte[] TOWER_ENEMY_LIST = new byte[] {
            12,
            1,
            11,
            6,
            7,
            14,
            3,
            19,
            13,
            22,
            8,
            25,
            32,
            31,
            28,
            33
        };

        public static byte[] DUNGEON_ENEMY_LIST = new byte[] {
            15,
            12,
            2,
            1,
            5,
            9,
            11,
            6,
            7,
            3,
            16,
            20,
            13,
            22,
            8,
            31,
            28,
            10
        };

        public static byte[] CASTLE_ENEMY_DIFFICULTY_LIST;
        public static byte[] GARDEN_ENEMY_DIFFICULTY_LIST;
        public static byte[] TOWER_ENEMY_DIFFICULTY_LIST;
        public static byte[] DUNGEON_ENEMY_DIFFICULTY_LIST;
        public static string[] CASTLE_ASSETSWAP_LIST;
        public static string[] DUNGEON_ASSETSWAP_LIST;
        public static string[] TOWER_ASSETSWAP_LIST;
        public static string[] GARDEN_ASSETSWAP_LIST;
        public static bool SHOW_ENEMY_RADII;
        public static bool ENABLE_PLAYER_DEBUG;
        public static bool UNLOCK_ALL_ABILITIES;
        public static GameTypes.LevelType TESTROOM_LEVELTYPE;
        public static bool TESTROOM_REVERSE;
        public static bool RUN_TESTROOM;
        public static bool SHOW_DEBUG_TEXT;
        public static bool LOAD_TITLE_SCREEN;
        public static bool LOAD_SPLASH_SCREEN;
        public static bool SHOW_SAVELOAD_DEBUG_TEXT;
        public static bool DELETE_SAVEFILE;
        public static bool CLOSE_TESTROOM_DOORS;
        public static bool RUN_TUTORIAL;
        public static bool RUN_DEMO_VERSION;
        public static bool DISABLE_SAVING;
        public static bool RUN_CRASH_LOGS;
        public static bool WEAKEN_BOSSES;
        public static bool ENABLE_OFFSCREEN_CONTROL;
        public static bool ENABLE_BACKUP_SAVING;
        public static bool CREATE_RETAIL_VERSION;
        public static bool SHOW_FPS;
        public static bool SAVE_FRAMES;

        static LevelEV() {
            // Note: this type is marked as 'beforefieldinit'.
            byte[] cASTLE_ENEMY_DIFFICULTY_LIST = new byte[12];
            LevelEV.CASTLE_ENEMY_DIFFICULTY_LIST = cASTLE_ENEMY_DIFFICULTY_LIST;
            byte[] array = new byte[14];
            array[0] = 1;
            array[4] = 1;
            LevelEV.GARDEN_ENEMY_DIFFICULTY_LIST = array;
            LevelEV.TOWER_ENEMY_DIFFICULTY_LIST = new byte[] {
                1,
                0,
                1,
                1,
                1,
                1,
                1,
                1,
                0,
                1,
                1,
                1,
                1,
                1,
                1,
                0
            };
            LevelEV.DUNGEON_ENEMY_DIFFICULTY_LIST = new byte[] {
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            };
            LevelEV.CASTLE_ASSETSWAP_LIST = new[] {
                "BreakableBarrel1_Character",
                "BreakableBarrel2_Character",
                "CastleAssetKnightStatue_Character",
                "CastleAssetWindow1_Sprite",
                "CastleAssetWindow2_Sprite",
                "CastleBGPillar_Character",
                "CastleAssetWeb1_Sprite",
                "CastleAssetWeb2_Sprite",
                "CastleAssetBackTorch_Character",
                "CastleAssetSideTorch_Character",
                "CastleAssetChandelier1_Character",
                "CastleAssetChandelier2_Character",
                "CastleAssetCandle1_Character",
                "CastleAssetCandle2_Character",
                "CastleAssetFireplace_Character",
                "CastleAssetBookcase_Sprite",
                "CastleAssetBookCase2_Sprite",
                "CastleAssetBookCase3_Sprite",
                "CastleAssetUrn1_Character",
                "CastleAssetUrn2_Character",
                "BreakableChair1_Character",
                "BreakableChair2_Character",
                "CastleAssetTable1_Character",
                "CastleAssetTable2_Character",
                "CastleDoorOpen_Sprite",
                "CastleAssetFrame_Sprite"
            };
            LevelEV.DUNGEON_ASSETSWAP_LIST = new[] {
                "BreakableCrate1_Character",
                "BreakableBarrel1_Character",
                "CastleAssetDemonStatue_Character",
                "DungeonSewerGrate1_Sprite",
                "DungeonSewerGrate2_Sprite",
                "",
                "CastleAssetWeb1_Sprite",
                "CastleAssetWeb2_Sprite",
                "DungeonChainRANDOM2_Character",
                "",
                "DungeonHangingCell1_Character",
                "DungeonHangingCell2_Character",
                "DungeonTorch1_Character",
                "DungeonTorch2_Character",
                "DungeonMaidenRANDOM3_Character",
                "DungeonPrison1_Sprite",
                "DungeonPrison2_Sprite",
                "DungeonPrison3_Sprite",
                "",
                "",
                "DungeonBucket1_Character",
                "DungeonBucket2_Character",
                "DungeonTable1_Character",
                "DungeonTable2_Character",
                "DungeonDoorOpen_Sprite",
                ""
            };
            LevelEV.TOWER_ASSETSWAP_LIST = new[] {
                "BreakableCrate1_Character",
                "BreakableCrate2_Character",
                "CastleAssetAngelStatue_Character",
                "TowerHoleRANDOM9_Sprite",
                "TowerHoleRANDOM9_Sprite",
                "",
                "TowerLever1_Sprite",
                "TowerLever2_Sprite",
                "CastleAssetBackTorchUnlit_Character",
                "CastleAssetSideTorchUnlit_Character",
                "DungeonChain1_Character",
                "DungeonChain2_Character",
                "TowerTorch_Character",
                "TowerPedestal2_Character",
                "CastleAssetFireplaceNoFire_Character",
                "BrokenBookcase1_Sprite",
                "BrokenBookcase2_Sprite",
                "",
                "TowerBust1_Character",
                "TowerBust2_Character",
                "TowerChair1_Character",
                "TowerChair2_Character",
                "TowerTable1_Character",
                "TowerTable2_Character",
                "TowerDoorOpen_Sprite",
                "CastleAssetFrame_Sprite"
            };
            LevelEV.GARDEN_ASSETSWAP_LIST = new[] {
                "GardenUrn1_Character",
                "GardenUrn2_Character",
                "CherubStatue_Character",
                "GardenFloatingRockRANDOM5_Sprite",
                "GardenFloatingRockRANDOM5_Sprite",
                "GardenPillar_Character",
                "",
                "",
                "GardenFairy_Character",
                "",
                "GardenVine1_Character",
                "GardenVine2_Character",
                "GardenLampPost1_Character",
                "GardenLampPost2_Character",
                "GardenFountain_Character",
                "GardenBush1_Sprite",
                "GardenBush2_Sprite",
                "",
                "",
                "",
                "GardenMushroom1_Character",
                "GardenMushroom2_Character",
                "GardenTrunk1_Character",
                "GardenTrunk2_Character",
                "GardenDoorOpen_Sprite",
                ""
            };
            LevelEV.SHOW_ENEMY_RADII = false;
            LevelEV.ENABLE_PLAYER_DEBUG = false;
            LevelEV.UNLOCK_ALL_ABILITIES = false;
            LevelEV.TESTROOM_LEVELTYPE = GameTypes.LevelType.CASTLE;
            LevelEV.TESTROOM_REVERSE = false;
            LevelEV.RUN_TESTROOM = false;
            LevelEV.SHOW_DEBUG_TEXT = false;
            LevelEV.LOAD_TITLE_SCREEN = false;
            LevelEV.LOAD_SPLASH_SCREEN = true;
            LevelEV.SHOW_SAVELOAD_DEBUG_TEXT = false;
            LevelEV.DELETE_SAVEFILE = false;
            LevelEV.CLOSE_TESTROOM_DOORS = false;
            LevelEV.RUN_TUTORIAL = false;
            LevelEV.RUN_DEMO_VERSION = false;
            LevelEV.DISABLE_SAVING = false;
            LevelEV.RUN_CRASH_LOGS = false;
            LevelEV.WEAKEN_BOSSES = false;
            LevelEV.ENABLE_OFFSCREEN_CONTROL = false;
            LevelEV.ENABLE_BACKUP_SAVING = true;
            LevelEV.CREATE_RETAIL_VERSION = true;
            LevelEV.SHOW_FPS = false;
            LevelEV.SAVE_FRAMES = false;
        }
    }
}
