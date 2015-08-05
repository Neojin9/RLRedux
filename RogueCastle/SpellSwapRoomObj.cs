using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;


namespace RogueCastle {
    public class SpellSwapRoomObj : BonusRoomObj {
        private SpriteObj m_icon;
        private float m_iconYPos;
        private SpriteObj m_pedestal;
        private SpriteObj m_speechBubble;
        public byte Spell { get; set; }

        public override void Initialize() {
            m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
            m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
            m_icon = new SpriteObj("Blank_Sprite");
            foreach (GameObj current in base.GameObjList) {
                if (current.Name == "pedestal") {
                    m_pedestal = (current as SpriteObj);
                    break;
                }
            }
            m_pedestal.OutlineWidth = 2;
            m_icon.X = m_pedestal.X;
            m_icon.Y = m_pedestal.Y - (m_pedestal.Y - (float)m_pedestal.Bounds.Top) - (float)m_icon.Height / 2f - 20f;
            m_icon.OutlineWidth = 2;
            m_iconYPos = m_icon.Y;
            base.GameObjList.Add(m_icon);
            m_speechBubble.Y = m_icon.Y - 30f;
            m_speechBubble.X = m_icon.X;
            m_speechBubble.Visible = false;
            base.GameObjList.Add(m_speechBubble);
            base.Initialize();
        }

        private void RandomizeItem() {
            if (Game.PlayerStats.Class != 16 && Game.PlayerStats.Class != 17) {
                byte[] spellList = ClassType.GetSpellList(Game.PlayerStats.Class);
                do {
                    Spell = spellList[CDGMath.RandomInt(0, spellList.Length - 1)];
                } while ((Game.PlayerStats.Traits.X == 31f || Game.PlayerStats.Traits.Y == 31f) && (Spell == 6 || Spell == 4 || Spell == 11));
                Array.Clear(spellList, 0, spellList.Length);
                base.ID = (int)Spell;
            }
            else if (Game.PlayerStats.Class == 16) {
                base.ID = 13;
                Spell = 13;
            }
            else if (Game.PlayerStats.Class == 17) {
                base.ID = 14;
                Spell = 14;
            }
            m_icon.ChangeSprite(SpellType.Icon(Spell));
        }

        public override void OnEnter() {
            if (base.ID == -1 && !base.RoomCompleted) {
                while (true) {
                    RandomizeItem();
                    if ((Spell != Game.PlayerStats.Spell || Game.PlayerStats.Class == 16 || Game.PlayerStats.Class == 17) && (Spell != 13 || Game.PlayerStats.Class == 16)) {
                        if (Spell != 14)
                            break;
                        if (Game.PlayerStats.Class == 17)
                            break;
                    }
                }
            }
            else if (base.ID != -1) {
                Spell = (byte)base.ID;
                m_icon.ChangeSprite(SpellType.Icon(Spell));
                if (base.RoomCompleted) {
                    m_icon.Visible = false;
                    m_speechBubble.Visible = false;
                }
            }
            base.OnEnter();
        }

        public override void Update(GameTime gameTime) {
            m_icon.Y = m_iconYPos - 10f + (float)Math.Sin((Game.TotalGameTime * 2f)) * 5f;
            m_speechBubble.Y = m_iconYPos - 90f + (float)Math.Sin((Game.TotalGameTime * 20f)) * 2f;
            if (!base.RoomCompleted) {
                m_icon.Visible = true;
                if (CollisionMath.Intersects(Player.Bounds, m_pedestal.Bounds)) {
                    m_speechBubble.Visible = true;
                    if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                        TakeItem();
                }
                else
                    m_speechBubble.Visible = false;
            }
            else
                m_icon.Visible = false;
            base.Update(gameTime);
        }

        public void TakeItem() {
            base.RoomCompleted = true;
            Game.PlayerStats.Spell = Spell;
            if (Game.PlayerStats.Class == 9)
                Game.PlayerStats.WizardSpellList = SpellType.GetNext3Spells();
            Spell = 0;
            m_speechBubble.Visible = false;
            m_icon.Visible = false;
            (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerSpellIcon();
            List<object> list = new List<object>();
            list.Add(new Vector2(m_icon.X, m_icon.Y - (float)m_icon.Height / 2f));
            list.Add(4);
            list.Add(new Vector2(Game.PlayerStats.Spell, 0f));
            (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list);
            Tween.RunFunction(0f, Player, "RunGetItemAnimation", new object[0]);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_pedestal = null;
                m_icon = null;
                m_speechBubble = null;
                base.Dispose();
            }
        }
    }
}
