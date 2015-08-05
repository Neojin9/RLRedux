using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public class CarnivalShoot2BonusRoom : BonusRoomObj {
        private ObjContainer m_axeIcons;
        private int m_axesThrown;
        private List<ObjContainer> m_balloonList;
        private NpcObj m_elf;
        private PhysicsObj m_gate;
        private bool m_gateClosed;
        private bool m_isPlayingGame;
        private int m_numTries = 5;
        private ChestObj m_rewardChest;
        private float m_storedPlayerMana;
        private byte m_storedPlayerSpell;
        private Rectangle m_targetBounds;
        private List<TextObj> m_targetDataText;
        private List<BreakableObj> m_targetList;
        private List<TextObj> m_targetText;

        public CarnivalShoot2BonusRoom() {
            m_targetList = new List<BreakableObj>();
            m_elf = new NpcObj("Clown_Character");
            m_elf.Scale = new Vector2(2f, 2f);
            m_targetText = new List<TextObj>();
            m_targetDataText = new List<TextObj>();
            m_balloonList = new List<ObjContainer>();
        }

        public int TargetsDestroyed {
            get {
                int num = 0;
                foreach (BreakableObj current in m_targetList) {
                    if (current.Broken)
                        num++;
                }
                return num;
            }
        }

        public int TargetsRemaining {
            get {
                int num = 0;
                foreach (BreakableObj current in m_targetList) {
                    if (!current.Broken)
                        num++;
                }
                return num;
            }
        }

        public override void LoadContent(GraphicsDevice graphics) {
            TextObj textObj = new TextObj(Game.JunicodeFont);
            textObj.FontSize = 25f;
            textObj.Text = "test text";
            textObj.DropShadow = new Vector2(2f, 2f);
            if (!base.IsReversed)
                textObj.Position = new Vector2((this.Bounds.Right - 1000), (this.Bounds.Top + 200));
            else
                textObj.Position = new Vector2((this.Bounds.Left + 300), (this.Bounds.Top + 200));
            for (int i = 0; i < 3; i++) {
                TextObj textObj2 = textObj.Clone() as TextObj;
                textObj2.Y += (float)(i * 100);
                m_targetText.Add(textObj2);
                TextObj textObj3 = textObj.Clone() as TextObj;
                textObj3.Y += (float)(i * 100);
                textObj3.X = textObj2.X + 500f;
                m_targetDataText.Add(textObj3);
            }
            m_axeIcons = new ObjContainer();
            int num = 0;
            int num2 = 10;
            for (int j = 0; j < m_numTries; j++) {
                SpriteObj spriteObj = new SpriteObj("SpellAxe_Sprite");
                spriteObj.Scale = new Vector2(2f, 2f);
                spriteObj.X = (float)(num + 10);
                spriteObj.Y = (float)num2;
                num += spriteObj.Width + 5;
                m_axeIcons.AddChild(spriteObj);
            }
            m_axeIcons.OutlineWidth = 2;
            base.GameObjList.Add(m_axeIcons);
            base.LoadContent(graphics);
        }

        public override void Initialize() {
            m_gate = new PhysicsObj("CastleEntranceGate_Sprite", null);
            m_gate.IsWeighted = false;
            m_gate.IsCollidable = true;
            m_gate.CollisionTypeTag = 1;
            m_gate.Layer = -1f;
            m_gate.OutlineWidth = 2;
            base.GameObjList.Add(m_gate);
            Rectangle rectangle = default(Rectangle);
            Color[] array = new[] {
                Color.Red,
                Color.Blue,
                Color.Green,
                Color.Yellow,
                Color.Orange,
                Color.Purple,
                Color.Pink,
                Color.MediumTurquoise,
                Color.CornflowerBlue
            };
            foreach (GameObj current in base.GameObjList) {
                if (current is WaypointObj)
                    m_elf.X = current.X;
                if (current.Name == "Balloon") {
                    m_balloonList.Add(current as ObjContainer);
                    (current as ObjContainer).GetChildAt(1).TextureColor = array[CDGMath.RandomInt(0, array.Length - 1)];
                }
            }
            foreach (TerrainObj current2 in base.TerrainObjList) {
                if (current2.Name == "Floor")
                    m_elf.Y = current2.Y - ((float)m_elf.Bounds.Bottom - m_elf.Y) - 2f;
                if (current2.Name == "GatePosition")
                    rectangle = current2.Bounds;
            }
            m_gate.X = (float)rectangle.X;
            m_gate.Y = (float)rectangle.Bottom;
            if (!base.IsReversed)
                m_elf.Flip = SpriteEffects.FlipHorizontally;
            base.GameObjList.Add(m_elf);
            foreach (TerrainObj current3 in base.TerrainObjList) {
                if (current3.Name == "Boundary") {
                    current3.Visible = false;
                    m_targetBounds = current3.Bounds;
                    break;
                }
            }
            float num = 10f;
            float num2 = (m_targetBounds.X + 40);
            float num3 = m_targetBounds.Y;
            float num4 = m_targetBounds.Width / num;
            float num5 = m_targetBounds.Height / num;
            int num6 = 0;
            while (num6 < num * num) {
                BreakableObj breakableObj = new BreakableObj("Target2_Character");
                breakableObj.X = num2;
                breakableObj.Y = num3;
                breakableObj.Scale = new Vector2(1.6f, 1.6f);
                breakableObj.OutlineWidth = 2;
                breakableObj.HitBySpellsOnly = true;
                breakableObj.IsWeighted = false;
                m_targetList.Add(breakableObj);
                breakableObj.SameTypesCollide = false;
                breakableObj.DropItem = false;
                base.GameObjList.Add(breakableObj);
                num2 += num4;
                if ((num6 + 1) % num == 0f) {
                    num2 = (m_targetBounds.X + 40);
                    num3 += num5;
                }
                num6++;
            }
            m_rewardChest = new ChestObj(null);
            m_rewardChest.ChestType = 3;
            if (!base.IsReversed) {
                m_rewardChest.Flip = SpriteEffects.FlipHorizontally;
                m_rewardChest.Position = new Vector2(m_elf.X + 100f, (m_elf.Bounds.Bottom - m_rewardChest.Height - 8));
            }
            else
                m_rewardChest.Position = new Vector2(m_elf.X - 150f, (m_elf.Bounds.Bottom - m_rewardChest.Height - 8));
            m_rewardChest.Visible = false;
            base.GameObjList.Add(m_rewardChest);
            base.Initialize();
        }

        public override void OnEnter() {
            m_rewardChest.ChestType = 3;
            if (!base.IsReversed)
                m_axeIcons.Position = new Vector2((this.Bounds.Right - 200 - m_axeIcons.Width), (this.Bounds.Bottom - 60));
            else
                m_axeIcons.Position = new Vector2((this.Bounds.Left + 900), (this.Bounds.Bottom - 60));
            m_targetText[0].Text = "Targets Destroyed:";
            m_targetText[1].Text = "Targets Remaining:";
            m_targetText[2].Text = "Reward:";
            m_targetDataText[0].Text = "50";
            m_targetDataText[1].Text = "10";
            m_targetDataText[2].Text = "100 gold";
            for (int i = 0; i < m_targetText.Count; i++) {
                m_targetText[i].Opacity = 0f;
                m_targetDataText[i].Opacity = 0f;
            }
            foreach (BreakableObj current in m_targetList) {
                current.Opacity = 0f;
                current.Visible = false;
            }
            m_gateClosed = true;
            m_storedPlayerSpell = Game.PlayerStats.Spell;
            m_storedPlayerMana = Player.CurrentMana;
            ReflipPosters();
            base.OnEnter();
        }

        private void ReflipPosters() {
            foreach (GameObj current in base.GameObjList) {
                SpriteObj spriteObj = current as SpriteObj;
                if (spriteObj != null && spriteObj.Flip == SpriteEffects.FlipHorizontally && (spriteObj.SpriteName == "CarnivalPoster1_Sprite" || spriteObj.SpriteName == "CarnivalPoster2_Sprite" || spriteObj.SpriteName == "CarnivalPoster3_Sprite" || spriteObj.SpriteName == "CarnivalTent_Sprite"))
                    spriteObj.Flip = SpriteEffects.None;
            }
        }

        public override void OnExit() {
            Game.PlayerStats.Spell = m_storedPlayerSpell;
            Player.AttachedLevel.UpdatePlayerSpellIcon();
            Player.CurrentMana = m_storedPlayerMana;
            base.OnExit();
        }

        public void BeginGame() {
            Player.AttachedLevel.ProjectileManager.DestroyAllProjectiles(true);
            Player.StopAllSpells();
            m_gateClosed = false;
            Tween.By(m_gate, 0.5f, new Easing(Quad.EaseInOut), new[] {
                "Y",
                (-m_gate.Height).ToString()
            });
            m_isPlayingGame = true;
            EquipPlayer();
            float num = 0f;
            foreach (BreakableObj current in m_targetList) {
                current.Visible = true;
                Tween.To(current, 0.5f, new Easing(Tween.EaseNone), new[] {
                    "delay",
                    num.ToString(),
                    "Opacity",
                    "1"
                });
                num += 0.01f;
            }
        }

        private void EndGame() {
            Player.LockControls();
            Player.CurrentSpeed = 0f;
            foreach (BreakableObj current in m_targetList) {
                Tween.To(current, 0.5f, new Easing(Tween.EaseNone), new[] {
                    "Opacity",
                    "0"
                });
            }
            Tween.AddEndHandlerToLastTween(this, "EndGame2", new object[0]);
            m_isPlayingGame = false;
        }

        public void EndGame2() {
            m_targetDataText[0].Text = TargetsDestroyed.ToString();
            m_targetDataText[1].Text = TargetsRemaining.ToString();
            int num = (int)(TargetsDestroyed / 2f) * 10;
            m_targetDataText[2].Text = num + " gold";
            float num2 = 0f;
            for (int i = 0; i < m_targetDataText.Count; i++) {
                Tween.To(m_targetText[i], 0.5f, new Easing(Tween.EaseNone), new[] {
                    "delay",
                    num2.ToString(),
                    "Opacity",
                    "1"
                });
                Tween.To(m_targetDataText[i], 0.5f, new Easing(Tween.EaseNone), new[] {
                    "delay",
                    num2.ToString(),
                    "Opacity",
                    "1"
                });
                num2 += 0.5f;
            }
            Tween.AddEndHandlerToLastTween(this, "GiveReward", new object[] {
                num
            });
        }

        public void GiveReward(int gold) {
            if ((!base.IsReversed && Player.X < (float)this.Bounds.Right - (float)Player.AttachedLevel.Camera.Width / 2f) || (base.IsReversed && Player.X > (float)this.Bounds.Left + (float)Player.AttachedLevel.Camera.Width / 2f)) {
                Tween.To(Player.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new[] {
                    "X",
                    Player.X.ToString()
                });
                Tween.AddEndHandlerToLastTween(this, "ResetCamera", new object[0]);
            }
            else
                ResetCamera();
            Player.AttachedLevel.TextManager.DisplayNumberStringText(gold, " gold", Color.Yellow, Player.Position);
            Game.PlayerStats.Gold += gold;
            Tween.By(m_gate, 0.5f, new Easing(Quad.EaseInOut), new[] {
                "Y",
                (-m_gate.Height).ToString()
            });
            m_gateClosed = false;
            base.RoomCompleted = true;
            Tween.AddEndHandlerToLastTween(this, "CheckPlayerReward", new object[0]);
        }

        public void CheckPlayerReward() {
            if (TargetsRemaining <= 10) {
                RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom2-Reward");
                (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
                RevealChest();
                GameUtil.UnlockAchievement("LOVE_OF_CLOWNS");
                return;
            }
            RCScreenManager rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
            rCScreenManager2.DialogueScreen.SetDialogue("CarnivalRoom2-Fail");
            (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
        }

        public void RevealChest() {
            Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(m_rewardChest.Position);
            m_rewardChest.Visible = true;
        }

        public void ResetCamera() {
            Player.UnlockControls();
            Player.AttachedLevel.CameraLockedToPlayer = true;
        }

        private void EquipPlayer() {
            Game.PlayerStats.Spell = 2;
            Player.AttachedLevel.UpdatePlayerSpellIcon();
            Player.CurrentMana = Player.MaxMana;
        }

        public void UnequipPlayer() {
            Game.PlayerStats.Spell = m_storedPlayerSpell;
            Player.AttachedLevel.UpdatePlayerSpellIcon();
            Player.CurrentMana = m_storedPlayerMana;
        }

        public override void Update(GameTime gameTime) {
            if ((m_axesThrown >= m_numTries && m_isPlayingGame && Player.AttachedLevel.ProjectileManager.ActiveProjectiles < 1) || (m_isPlayingGame && TargetsDestroyed >= 100))
                EndGame();
            if (m_isPlayingGame && !m_gateClosed && ((!base.IsReversed && Player.X > (float)m_gate.Bounds.Right) || (base.IsReversed && Player.X < (float)m_gate.Bounds.Left))) {
                Player.LockControls();
                Player.CurrentSpeed = 0f;
                Player.AccelerationX = 0f;
                Tween.By(m_gate, 0.5f, new Easing(Quad.EaseInOut), new[] {
                    "Y",
                    m_gate.Height.ToString()
                });
                Tween.AddEndHandlerToLastTween(Player, "UnlockControls", new object[0]);
                m_gateClosed = true;
                Player.AttachedLevel.CameraLockedToPlayer = false;
                if (!base.IsReversed) {
                    Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new[] {
                        "X",
                        ((float)this.Bounds.Right - (float)Player.AttachedLevel.Camera.Width / 2f).ToString()
                    });
                }
                else {
                    Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new[] {
                        "X",
                        ((float)this.Bounds.Left + (float)Player.AttachedLevel.Camera.Width / 2f).ToString()
                    });
                }
            }
            m_elf.Update(gameTime, Player);
            if (m_isPlayingGame)
                m_elf.CanTalk = false;
            else
                m_elf.CanTalk = true;
            if (m_elf.IsTouching && !base.RoomCompleted && !m_isPlayingGame) {
                if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)) {
                    RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                    rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom2-Start");
                    rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "BeginGame", new object[0]);
                    rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[] {
                        "Canceling Selection"
                    });
                    (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
                }
            }
            else if (m_elf.IsTouching && base.RoomCompleted && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))) {
                RCScreenManager rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
                rCScreenManager2.DialogueScreen.SetDialogue("CarnivalRoom1-End");
                (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
            }
            if (!base.IsReversed && m_isPlayingGame && Player.X < (float)(this.Bounds.Left + 10))
                Player.X = (float)(this.Bounds.Left + 10);
            else if (base.IsReversed && m_isPlayingGame && Player.X > (float)(this.Bounds.Right - 10))
                Player.X = (float)(this.Bounds.Right - 10);
            float totalGameTime = Game.TotalGameTime;
            float num = 2f;
            foreach (ObjContainer current in m_balloonList) {
                current.Rotation = (float)Math.Sin((totalGameTime * num)) * num;
                num += 0.2f;
            }
            HandleInput();
            base.Update(gameTime);
        }

        private void HandleInput() {
            if (m_isPlayingGame && (Game.GlobalInput.JustPressed(24) || (Game.GlobalInput.JustPressed(12) && Game.PlayerStats.Class == 16)) && Player.SpellCastDelay <= 0f && m_gateClosed) {
                m_axesThrown++;
                Player.CurrentMana = Player.MaxMana;
                if (m_axesThrown <= m_numTries)
                    m_axeIcons.GetChildAt(m_numTries - m_axesThrown).Visible = false;
                if (m_axesThrown > m_numTries)
                    Game.PlayerStats.Spell = 0;
            }
        }

        public override void Draw(Camera2D camera) {
            base.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            for (int i = 0; i < m_targetText.Count; i++) {
                m_targetText[i].Draw(camera);
                m_targetDataText[i].Draw(camera);
            }
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_targetList.Clear();
                m_targetList = null;
                m_elf = null;
                m_axeIcons = null;
                m_gate = null;
                for (int i = 0; i < m_targetText.Count; i++) {
                    m_targetText[i].Dispose();
                    m_targetDataText[i].Dispose();
                }
                m_targetText.Clear();
                m_targetText = null;
                m_targetDataText.Clear();
                m_targetDataText = null;
                m_balloonList.Clear();
                m_balloonList = null;
                m_rewardChest = null;
                base.Dispose();
            }
        }
    }
}
