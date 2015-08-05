using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public class ChestBonusRoomObj : BonusRoomObj {
        private const int TOTAL_UNLOCKED_CHESTS = 1;
        private List<ChestObj> m_chestList;
        private NpcObj m_elf;
        private PhysicsObj m_gate;
        private bool m_paid;

        public ChestBonusRoomObj() {
            m_chestList = new List<ChestObj>();
            m_elf = new NpcObj("Elf_Character");
            m_elf.Scale = new Vector2(2f, 2f);
            m_gate = new PhysicsObj("CastleEntranceGate_Sprite", null);
            m_gate.IsWeighted = false;
            m_gate.IsCollidable = true;
            m_gate.CollisionTypeTag = 1;
            m_gate.Layer = -1f;
        }

        private int NumberOfChestsOpen {
            get {
                int num = 0;
                foreach (ChestObj current in m_chestList) {
                    if (current.IsOpen)
                        num++;
                }
                return num;
            }
        }

        public override void Initialize() {
            Vector2 vector = Vector2.Zero;
            Vector2 zero = Vector2.Zero;
            foreach (GameObj current in base.GameObjList) {
                if (current is WaypointObj)
                    zero.X = current.X;
            }
            foreach (TerrainObj current2 in base.TerrainObjList) {
                if (current2.Name == "GatePosition")
                    vector = new Vector2(current2.X, (float)current2.Bounds.Bottom);
                if (current2.Name == "Floor")
                    zero.Y = current2.Y;
            }
            m_gate.Position = new Vector2(vector.X, vector.Y);
            if (!base.IsReversed)
                m_elf.Flip = SpriteEffects.FlipHorizontally;
            m_elf.Position = new Vector2(zero.X, zero.Y - ((float)m_elf.Bounds.Bottom - m_elf.AnchorY) - 2f);
            base.GameObjList.Add(m_elf);
            base.GameObjList.Add(m_gate);
            base.Initialize();
        }

        public override void OnEnter() {
            base.ID = 1;
            foreach (GameObj current in base.GameObjList) {
                ChestObj chestObj = current as ChestObj;
                if (chestObj != null) {
                    chestObj.ChestType = 2;
                    chestObj.IsEmpty = true;
                    chestObj.IsLocked = true;
                }
            }
            (m_elf.GetChildAt(2) as SpriteObj).StopAnimation();
            base.OnEnter();
        }

        private void ShuffleChests(int goldPaid) {
            int[] array = new[] {
                1,
                2,
                3
            };
            CDGMath.Shuffle<int>(array);
            int num = 0;
            foreach (GameObj current in base.GameObjList) {
                ChestObj chestObj = current as ChestObj;
                if (chestObj != null) {
                    chestObj.ForcedItemType = 1;
                    int num2 = array[num];
                    if (num2 == 1)
                        chestObj.IsEmpty = true;
                    else if (num2 == 2)
                        chestObj.IsEmpty = true;
                    else {
                        chestObj.IsEmpty = false;
                        chestObj.ForcedAmount = goldPaid * 3f;
                    }
                    num++;
                    m_chestList.Add(chestObj);
                    chestObj.IsLocked = false;
                    chestObj.TextureColor = Color.White;
                    if (num2 == 3 && Game.PlayerStats.SpecialItem == 8)
                        chestObj.TextureColor = Color.Gold;
                }
            }
        }

        public override void Update(GameTime gameTime) {
            m_elf.Update(gameTime, Player);
            if (!base.RoomCompleted) {
                if (m_paid) {
                    if (!base.IsReversed && Player.X < base.X + 50f)
                        Player.X = base.X + 50f;
                    else if (base.IsReversed && Player.X > base.X + (float)this.Width - 50f)
                        Player.X = base.X + (float)this.Width - 50f;
                }
                if (NumberOfChestsOpen >= 1) {
                    bool flag = false;
                    foreach (ChestObj current in m_chestList) {
                        if (current.IsEmpty && current.IsOpen)
                            flag = true;
                        current.IsLocked = true;
                    }
                    base.RoomCompleted = true;
                    RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                    if (!flag)
                        rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom1-Won");
                    else
                        rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom1-Lost");
                    Game.ScreenManager.DisplayScreen(13, true, null);
                }
            }
            HandleInput();
            base.Update(gameTime);
        }

        private void HandleInput() {
            if (m_elf.IsTouching && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))) {
                RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                if (!base.RoomCompleted) {
                    if (!m_paid) {
                        rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom" + base.ID + "-Start");
                        rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                        rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "PlayChestGame", new object[0]);
                        rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[] {
                            "Canceling Selection"
                        });
                    }
                    else
                        rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom1-Choose");
                }
                else
                    rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom1-End");
                Game.ScreenManager.DisplayScreen(13, true, null);
            }
        }

        public void PlayChestGame() {
            if (Game.PlayerStats.Gold >= 4) {
                m_paid = true;
                float num;
                if (base.ID == 1)
                    num = 0.25f;
                else if (base.ID == 2)
                    num = 0.5f;
                else
                    num = 0.75f;
                int num2 = (int)(Game.PlayerStats.Gold * num);
                Game.PlayerStats.Gold -= num2;
                ShuffleChests(num2);
                Player.AttachedLevel.TextManager.DisplayNumberStringText(-num2, "gold", Color.Yellow, new Vector2(Player.X, Player.Bounds.Top));
                Tween.By(m_gate, 1f, new Easing(Quad.EaseInOut), new[] {
                    "Y",
                    (-m_gate.Height).ToString()
                });
                return;
            }
            (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen.SetDialogue("ChestBonusRoom1-NoMoney");
            Tween.To(this, 0f, new Easing(Linear.EaseNone), new string[0]);
            Tween.AddEndHandlerToLastTween(Player.AttachedLevel.ScreenManager, "DisplayScreen", new object[] {
                13,
                true,
                typeof(List<object>)
            });
        }

        public override void Reset() {
            foreach (ChestObj current in m_chestList)
                current.ResetChest();
            if (m_paid) {
                m_gate.Y += (float)m_gate.Height;
                m_paid = false;
            }
            base.Reset();
        }

        protected override GameObj CreateCloneInstance() {
            return new ChestBonusRoomObj();
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_elf = null;
                m_gate = null;
                m_chestList.Clear();
                m_chestList = null;
                base.Dispose();
            }
        }
    }
}
