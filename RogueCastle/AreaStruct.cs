using Microsoft.Xna.Framework;


namespace RogueCastle {
    public struct AreaStruct {
        public Vector2 BonusRooms;
        public bool BossInArea;
        public int BossLevel;
        public byte BossType;
        public Color Color;
        public Vector2 EnemyLevel;
        public int EnemyLevelScale;
        public bool IsFinalArea;
        public GameTypes.LevelType LevelType;
        public bool LinkToCastleOnly;
        public Color MapColor;
        public string Name;
        public Vector2 SecretRooms;
        public Vector2 TotalRooms;
    }
}
