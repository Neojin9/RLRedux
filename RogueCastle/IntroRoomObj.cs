using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class IntroRoomObj : RoomObj {
        private GameObj m_fern1;
        private GameObj m_fern2;
        private GameObj m_fern3;
        private bool m_inSecondPart;
        private TextObj m_introText;
        private SpriteObj m_mountain1;
        private SpriteObj m_mountain2;
        private List<RaindropObj> m_rainFG;
        private Cue m_rainSFX;
        private float m_smokeCounter;
        private GameObj m_tree1;
        private GameObj m_tree2;
        private GameObj m_tree3;

        public IntroRoomObj() {
            m_rainFG = new List<RaindropObj>();
            for (int i = 0; i < 500; i++) {
                RaindropObj item = new RaindropObj(new Vector2((float)CDGMath.RandomInt(-100, 2640), (float)CDGMath.RandomInt(-400, 720)));
                m_rainFG.Add(item);
            }
        }

        public override void Initialize() {
            TerrainObj terrainObj = null;
            TerrainObj terrainObj2 = null;
            TerrainObj terrainObj3 = null;
            TerrainObj terrainObj4 = null;
            foreach (TerrainObj current in base.TerrainObjList) {
                if (current.Name == "BlacksmithBlock")
                    terrainObj = current;
                if (current.Name == "EnchantressBlock")
                    terrainObj2 = current;
                if (current.Name == "ArchitectBlock")
                    terrainObj3 = current;
                if (current.Name == "SignBlock")
                    terrainObj4 = current;
            }
            if (terrainObj != null)
                base.TerrainObjList.Remove(terrainObj);
            if (terrainObj2 != null)
                base.TerrainObjList.Remove(terrainObj2);
            if (terrainObj3 != null)
                base.TerrainObjList.Remove(terrainObj3);
            if (terrainObj4 != null)
                base.TerrainObjList.Remove(terrainObj4);
            if (m_tree1 == null) {
                foreach (GameObj current2 in base.GameObjList) {
                    if (current2.Name == "Mountains 1")
                        m_mountain1 = (current2 as SpriteObj);
                    else if (current2.Name == "Mountains 2")
                        m_mountain2 = (current2 as SpriteObj);
                    else if (current2.Name == "Sign")
                        current2.Visible = false;
                    if (current2.Name == "Tree1")
                        m_tree1 = current2;
                    else if (current2.Name == "Tree2")
                        m_tree2 = current2;
                    else if (current2.Name == "Tree3")
                        m_tree3 = current2;
                    else if (current2.Name == "Fern1")
                        m_fern1 = current2;
                    else if (current2.Name == "Fern2")
                        m_fern2 = current2;
                    else if (current2.Name == "Fern3")
                        m_fern3 = current2;
                }
            }
            base.EnemyList.Clear();
            base.Initialize();
        }

        public override void LoadContent(GraphicsDevice graphics) {
            m_introText = new TextObj(Game.JunicodeLargeFont);
            m_introText.Text = "My duties are to my family...";
            m_introText.FontSize = 25f;
            m_introText.ForceDraw = true;
            m_introText.Align = Types.TextAlign.Centre;
            m_introText.Position = new Vector2(660f, 260f);
            m_introText.OutlineWidth = 2;
            base.LoadContent(graphics);
        }

        public override void OnEnter() {
            SoundManager.StopMusic(0f);
            Player.CurrentHealth = Player.MaxHealth;
            Player.CurrentMana = Player.MaxMana;
            Game.PlayerStats.HeadPiece = 7;
            Player.AttachedLevel.SetMapDisplayVisibility(false);
            Player.AttachedLevel.SetPlayerHUDVisibility(false);
            if (m_rainSFX != null)
                m_rainSFX.Dispose();
            m_rainSFX = SoundManager.PlaySound("Rain1");
            Player.UpdateCollisionBoxes();
            Player.Position = new Vector2(10f, 660f - ((float)Player.Bounds.Bottom - Player.Y));
            Player.State = 1;
            LogicSet logicSet = new LogicSet(Player);
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(2.5f, false), Types.Sequence.Serial);
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0), Types.Sequence.Serial);
            Player.RunExternalLogicSet(logicSet);
            Tween.RunFunction(2f, this, "Intro1", new object[0]);
            base.OnEnter();
        }

        public void Intro1() {
            List<object> list = new List<object>();
            list.Add(1f);
            list.Add(0.2f);
            list.Add(4f);
            list.Add(true);
            list.Add(m_introText);
            list.Add(false);
            Game.ScreenManager.DisplayScreen(22, false, list);
            Tween.RunFunction(3f, this, "Intro2", new object[0]);
        }

        public void Intro2() {
            m_inSecondPart = true;
            Tween.RunFunction(3f, Player.AttachedLevel, "LightningEffectTwice", new object[0]);
            LogicSet logicSet = new LogicSet(Player);
            logicSet.AddAction(new DelayLogicAction(5f, false), Types.Sequence.Serial);
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.7f, false), Types.Sequence.Serial);
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0), Types.Sequence.Serial);
            Player.RunExternalLogicSet(logicSet);
            Tween.RunFunction(5.3f, this, "Intro3", new object[0]);
        }

        public void Intro3() {
            List<object> list = new List<object>();
            m_introText.Text = "But I am loyal only to myself.";
            list.Add(1f);
            list.Add(0.2f);
            list.Add(4f);
            list.Add(true);
            list.Add(m_introText);
            list.Add(false);
            Game.ScreenManager.DisplayScreen(22, false, list);
            Tween.RunFunction(4f, this, "Intro4", new object[0]);
        }

        public void Intro4() {
            Player.Position = new Vector2((this.Bounds.Right - 450), (this.Bounds.Bottom - 300));
            Player.X += 700f;
            Player.Y = 600f;
            m_inSecondPart = false;
        }

        public override void OnExit() {
            if (m_rainSFX != null && !m_rainSFX.IsDisposed)
                m_rainSFX.Stop(AudioStopOptions.Immediate);
        }

        public override void Update(GameTime gameTime) {
            float totalGameTime = Game.TotalGameTime;
            m_tree1.Rotation = -(float)Math.Sin(totalGameTime) * 2f;
            m_tree2.Rotation = (float)Math.Sin((totalGameTime * 2f));
            m_tree3.Rotation = (float)Math.Sin((totalGameTime * 2f)) * 2f;
            m_fern1.Rotation = (float)Math.Sin((totalGameTime * 3f)) / 2f;
            m_fern2.Rotation = -(float)Math.Sin((totalGameTime * 4f));
            m_fern3.Rotation = (float)Math.Sin((totalGameTime * 4f)) / 2f;
            foreach (RaindropObj current in m_rainFG)
                current.Update(base.TerrainObjList, gameTime);
            if (m_inSecondPart && m_smokeCounter < 0.2f) {
                m_smokeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (m_smokeCounter >= 0.2f) {
                    m_smokeCounter = 0f;
                    Player.AttachedLevel.ImpactEffectPool.DisplayMassiveSmoke(new Vector2(Player.AttachedLevel.Camera.X + 300f, (float)Player.AttachedLevel.Camera.Bounds.Top));
                    Player.AttachedLevel.ImpactEffectPool.DisplayMassiveSmoke(new Vector2(Player.AttachedLevel.Camera.X + 380f, (float)Player.AttachedLevel.Camera.Bounds.Top));
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(Camera2D camera) {
            m_mountain1.X = camera.TopLeftCorner.X * 0.5f;
            m_mountain2.X = m_mountain1.X + 2640f;
            base.Draw(camera);
            camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 2640, 720), Color.Black * 0.6f);
            foreach (RaindropObj current in m_rainFG)
                current.Draw(camera);
            if (m_inSecondPart)
                camera.Draw(Game.GenericTexture, new Rectangle(1650, 0, 3000, 720), Color.Black);
        }

        public override void PauseRoom() {
            foreach (RaindropObj current in m_rainFG)
                current.PauseAnimation();
            if (m_rainSFX != null)
                m_rainSFX.Pause();
        }

        public override void UnpauseRoom() {
            foreach (RaindropObj current in m_rainFG)
                current.ResumeAnimation();
            if (m_rainSFX != null && m_rainSFX.IsPaused)
                m_rainSFX.Resume();
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                foreach (RaindropObj current in m_rainFG)
                    current.Dispose();
                m_rainFG.Clear();
                m_rainFG = null;
                m_mountain1 = null;
                m_mountain2 = null;
                if (m_rainSFX != null)
                    m_rainSFX.Dispose();
                m_rainSFX = null;
                m_tree1 = null;
                m_tree2 = null;
                m_tree3 = null;
                m_fern1 = null;
                m_fern2 = null;
                m_fern3 = null;
                m_introText.Dispose();
                m_introText = null;
                base.Dispose();
            }
        }

        protected override GameObj CreateCloneInstance() {
            return new IntroRoomObj();
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
        }
    }
}
