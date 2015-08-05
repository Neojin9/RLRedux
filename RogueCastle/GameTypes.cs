namespace RogueCastle {
    public class GameTypes {
        #region ArmorType enum

        public enum ArmorType {
            NONE,
            HEAD,
            BODY,
            RING,
            FOOT,
            HAND,
            ALL
        }

        #endregion

        #region DoorType enum

        public enum DoorType {
            NULL,
            OPEN,
            LOCKED,
            BLOCKED
        }

        #endregion

        #region EnemyDifficulty enum

        public enum EnemyDifficulty {
            BASIC,
            ADVANCED,
            EXPERT,
            MINIBOSS
        }

        #endregion

        #region EquipmentType enum

        public enum EquipmentType {
            NONE,
            WEAPON,
            ARMOR
        }

        #endregion

        #region LevelType enum

        public enum LevelType {
            NONE,
            CASTLE,
            GARDEN,
            DUNGEON,
            TOWER
        }

        #endregion

        #region SkillType enum

        public enum SkillType {
            STRENGTH,
            HEALTH,
            DEFENSE
        }

        #endregion

        #region StatType enum

        public enum StatType {
            STRENGTH,
            HEALTH,
            ENDURANCE,
            EQUIPLOAD
        }

        #endregion

        #region WeaponType enum

        public enum WeaponType {
            NONE,
            DAGGER,
            SWORD,
            SPEAR,
            AXE
        }

        #endregion

        public const int CollisionType_NULL = 0;
        public const int CollisionType_WALL = 1;
        public const int CollisionType_PLAYER = 2;
        public const int CollisionType_ENEMY = 3;
        public const int CollisionType_ENEMYWALL = 4;
        public const int CollisionType_WALL_FOR_PLAYER = 5;
        public const int CollisionType_WALL_FOR_ENEMY = 6;
        public const int CollisionType_PLAYER_TRIGGER = 7;
        public const int CollisionType_ENEMY_TRIGGER = 8;
        public const int CollisionType_GLOBAL_TRIGGER = 9;
        public const int CollisionType_GLOBAL_DAMAGE_WALL = 10;
        public const int LogicSetType_NULL = 0;
        public const int LogicSetType_NONATTACK = 1;
        public const int LogicSetType_ATTACK = 2;
        public const int LogicSetType_CD = 3;
    }
}
