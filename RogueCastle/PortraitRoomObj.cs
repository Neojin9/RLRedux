using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class PortraitRoomObj : BonusRoomObj {
        private SpriteObj m_portrait;
        private SpriteObj m_portraitFrame;
        private int m_portraitIndex;

        public override void Initialize() {
            foreach (GameObj current in base.GameObjList) {
                if (current.Name == "portrait") {
                    m_portraitFrame = (current as SpriteObj);
                    break;
                }
            }
            m_portraitFrame.ChangeSprite("GiantPortrait_Sprite");
            m_portraitFrame.Scale = new Vector2(2f, 2f);
            m_portrait = new SpriteObj("Blank_Sprite");
            m_portrait.Position = m_portraitFrame.Position;
            m_portrait.Scale = new Vector2(0.95f, 0.95f);
            base.GameObjList.Add(m_portrait);
            base.Initialize();
        }

        public override void OnEnter() {
            if (!base.RoomCompleted && base.ID == -1) {
                m_portraitIndex = CDGMath.RandomInt(0, 7);
                m_portrait.ChangeSprite("Portrait" + m_portraitIndex + "_Sprite");
                base.ID = m_portraitIndex;
                base.OnEnter();
                return;
            }
            if (base.ID != -1) {
                m_portraitIndex = base.ID;
                m_portrait.ChangeSprite("Portrait" + m_portraitIndex + "_Sprite");
            }
        }

        public override void Update(GameTime gameTime) {
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)) {
                Rectangle b = new Rectangle(this.Bounds.Center.X - 100, this.Bounds.Bottom - 300, 200, 200);
                if (CollisionMath.Intersects(Player.Bounds, b) && Player.IsTouchingGround && base.ID > -1) {
                    RCScreenManager screenManager = Game.ScreenManager;
                    screenManager.DialogueScreen.SetDialogue("PortraitRoomText" + base.ID);
                    screenManager.DisplayScreen(13, true, null);
                }
            }
            base.Update(gameTime);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_portraitFrame = null;
                m_portrait = null;
                base.Dispose();
            }
        }
    }
}
