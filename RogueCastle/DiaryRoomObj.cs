using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class DiaryRoomObj : BonusRoomObj {
        private SpriteObj m_diary;
        private int m_diaryIndex;
        private SpriteObj m_speechBubble;

        public override void Initialize() {
            m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
            m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
            base.GameObjList.Add(m_speechBubble);
            foreach (GameObj current in base.GameObjList) {
                if (current.Name == "diary") {
                    m_diary = (current as SpriteObj);
                    break;
                }
            }
            m_diary.OutlineWidth = 2;
            m_speechBubble.Position = new Vector2(m_diary.X, m_diary.Y - (float)m_speechBubble.Height - 20f);
            base.Initialize();
        }

        public override void OnEnter() {
            if (!base.RoomCompleted)
                m_speechBubble.Visible = true;
            else
                m_speechBubble.Visible = false;
            if (!base.RoomCompleted)
                m_diaryIndex = Game.PlayerStats.DiaryEntry;
            if (m_diaryIndex >= 24)
                m_speechBubble.Visible = false;
            base.OnEnter();
        }

        public override void Update(GameTime gameTime) {
            m_speechBubble.Y = m_diary.Y - (float)m_speechBubble.Height - 20f - 30f + (float)Math.Sin((Game.TotalGameTime * 20f)) * 2f;
            Rectangle bounds = m_diary.Bounds;
            bounds.X -= 50;
            bounds.Width += 100;
            if (!CollisionMath.Intersects(Player.Bounds, bounds) && m_speechBubble.SpriteName == "UpArrowSquare_Sprite")
                m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");
            if (!base.RoomCompleted || CollisionMath.Intersects(Player.Bounds, bounds))
                m_speechBubble.Visible = true;
            else if (base.RoomCompleted && !CollisionMath.Intersects(Player.Bounds, bounds))
                m_speechBubble.Visible = false;
            if (m_diaryIndex >= 24)
                m_speechBubble.Visible = false;
            if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround) {
                if (m_speechBubble.SpriteName == "ExclamationSquare_Sprite")
                    m_speechBubble.ChangeSprite("UpArrowSquare_Sprite");
                if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)) {
                    if (!base.RoomCompleted && Game.PlayerStats.DiaryEntry < 24) {
                        RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                        rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + m_diaryIndex);
                        rCScreenManager.DisplayScreen(13, true, null);
                        PlayerStats expr_1DB = Game.PlayerStats;
                        expr_1DB.DiaryEntry += 1;
                        base.RoomCompleted = true;
                    }
                    else {
                        base.RoomCompleted = true;
                        RCScreenManager rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
                        rCScreenManager2.DisplayScreen(20, true, null);
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_diary = null;
                m_speechBubble = null;
                base.Dispose();
            }
        }
    }
}
