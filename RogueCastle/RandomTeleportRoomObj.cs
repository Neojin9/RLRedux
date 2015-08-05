using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class RandomTeleportRoomObj : BonusRoomObj {
        private TeleporterObj m_teleporter;

        public override void Initialize() {
            m_teleporter = new TeleporterObj();
            SpriteObj item = null;
            foreach (GameObj current in base.GameObjList) {
                if (current.Name == "teleporter") {
                    m_teleporter.Position = current.Position;
                    item = (current as SpriteObj);
                    break;
                }
            }
            base.GameObjList.Remove(item);
            base.GameObjList.Add(m_teleporter);
            m_teleporter.OutlineWidth = 2;
            m_teleporter.TextureColor = Color.PaleVioletRed;
            base.Initialize();
        }

        public override void Update(GameTime gameTime) {
            if (CollisionMath.Intersects(Player.Bounds, m_teleporter.Bounds) && Player.IsTouchingGround && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
                TeleportPlayer();
        }

        private void TeleportPlayer() {
            ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
            PlayerObj player = Game.ScreenManager.Player;
            player.UpdateCollisionBoxes();
            if (levelScreen != null) {
                Vector2 position = default(Vector2);
                int num = levelScreen.RoomList.Count - 1;
                if (num < 1)
                    num = 1;
                int index = CDGMath.RandomInt(0, num);
                RoomObj roomObj = levelScreen.RoomList[index];
                while (roomObj.Name == "Boss" || roomObj.Name == "Start" || roomObj.Name == "Ending" || roomObj.Name == "Compass" || roomObj.Name == "ChallengeBoss" || roomObj.Name == "Throne" || roomObj.Name == "Tutorial" || roomObj.Name == "EntranceBoss" || roomObj.Name == "Bonus" || roomObj.Name == "Linker" || roomObj.Name == "Secret" || roomObj.Name == "CastleEntrance" || roomObj.DoorList.Count < 2) {
                    index = CDGMath.RandomInt(0, num);
                    roomObj = levelScreen.RoomList[index];
                }
                foreach (DoorObj current in roomObj.DoorList) {
                    bool flag = false;
                    position.X = (float)current.Bounds.Center.X;
                    string doorPosition;
                    if ((doorPosition = current.DoorPosition) != null) {
                        if (!(doorPosition == "Left") && !(doorPosition == "Right")) {
                            if (doorPosition == "Bottom") {
                                position.Y = (float)current.Bounds.Top - ((float)player.Bounds.Bottom - player.Y);
                                flag = true;
                            }
                        }
                        else {
                            flag = true;
                            position.Y = (float)current.Bounds.Bottom - ((float)player.Bounds.Bottom - player.Y);
                        }
                    }
                    if (flag)
                        break;
                }
                player.TeleportPlayer(position, m_teleporter);
            }
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_teleporter = null;
                base.Dispose();
            }
        }
    }
}
