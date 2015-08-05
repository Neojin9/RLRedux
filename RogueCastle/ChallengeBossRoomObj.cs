using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public abstract class ChallengeBossRoomObj : RoomObj {
        protected ChestObj m_bossChest;
        private SpriteObj m_bossDivider;
        private TextObj m_bossTitle1;
        private TextObj m_bossTitle2;
        protected bool m_cutsceneRunning;
        private List<RaindropObj> m_rainFG;
        private float m_roomFloor;
        private float m_sparkleTimer;
        private int m_storedHealth;
        private float m_storedMana;
        private PlayerStats m_storedPlayerStats;
        private Vector2 m_storedScale;
        private bool m_teleportingOut;

        public ChallengeBossRoomObj() {
            m_storedPlayerStats = new PlayerStats();
        }

        public abstract bool BossKilled { get; }

        public int StoredHP {
            get { return m_storedHealth; }
        }

        public float StoredMP {
            get { return m_storedMana; }
        }

        public override void Initialize() {
            m_bossTitle1 = new TextObj(Game.JunicodeFont);
            m_bossTitle1.Text = "The Forsaken";
            m_bossTitle1.OutlineWidth = 2;
            m_bossTitle1.FontSize = 18f;
            m_bossTitle2 = new TextObj(Game.JunicodeLargeFont);
            m_bossTitle2.Text = "Alexander";
            m_bossTitle2.OutlineWidth = 2;
            m_bossTitle2.FontSize = 40f;
            m_bossDivider = new SpriteObj("Blank_Sprite");
            m_bossDivider.OutlineWidth = 2;
            foreach (DoorObj current in base.DoorList)
                m_roomFloor = (float)current.Bounds.Bottom;
            m_bossChest = new ChestObj(null);
            m_bossChest.Position = new Vector2((float)this.Bounds.Center.X - (float)m_bossChest.Width / 2f, (float)this.Bounds.Center.Y);
            base.GameObjList.Add(m_bossChest);
            m_rainFG = new List<RaindropObj>();
            for (int i = 0; i < 50; i++) {
                RaindropObj raindropObj = new RaindropObj(new Vector2(CDGMath.RandomFloat(base.X - (float)this.Width, base.X), CDGMath.RandomFloat(base.Y, base.Y + (float)this.Height)));
                m_rainFG.Add(raindropObj);
                raindropObj.ChangeToParticle();
            }
            base.Initialize();
        }

        public void StorePlayerData() {
            m_storedPlayerStats = Game.PlayerStats;
            Game.PlayerStats = new PlayerStats();
            Game.PlayerStats.TutorialComplete = true;
            m_storedScale = Player.Scale;
            Player.Scale = new Vector2(2f, 2f);
            SkillSystem.ResetAllTraits();
            Player.OverrideInternalScale(Player.Scale);
            m_storedHealth = Player.CurrentHealth;
            m_storedMana = Player.CurrentMana;
        }

        public void LoadPlayerData() {
            Game.PlayerStats = m_storedPlayerStats;
        }

        public override void OnEnter() {
            Player.CurrentHealth = Player.MaxHealth;
            Player.CurrentMana = Player.MaxMana;
            Player.ChangeSprite("PlayerIdle_Character");
            Player.AttachedLevel.UpdatePlayerHUD();
            Player.AttachedLevel.UpdatePlayerHUDAbilities();
            Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
            Player.AttachedLevel.UpdatePlayerSpellIcon();
            Player.UpdateEquipmentColours();
            Player.AttachedLevel.RefreshPlayerHUDPos();
            Game.ScreenManager.GetLevelScreen().JukeboxEnabled = false;
            m_bossChest.ChestType = 5;
            m_bossChest.Visible = false;
            m_bossChest.IsLocked = true;
            m_bossChest.X = Player.X;
            if (m_bossChest.PhysicsMngr == null)
                Player.PhysicsMngr.AddObject(m_bossChest);
            m_teleportingOut = false;
            m_bossTitle1.Opacity = 0f;
            m_bossTitle2.Opacity = 0f;
            m_bossDivider.ScaleX = 0f;
            m_bossDivider.Opacity = 0f;
            if (LevelEV.WEAKEN_BOSSES) {
                foreach (EnemyObj current in base.EnemyList)
                    current.CurrentHealth = 1;
            }
            base.OnEnter();
        }

        public override void OnExit() {
            LoadPlayerData();
            (Game.ScreenManager.Game as Game).SaveManager.LoadFiles(Player.AttachedLevel, new[] {
                SaveType.UpgradeData
            });
            Player.AttachedLevel.UpdatePlayerHUD();
            Player.AttachedLevel.UpdatePlayerHUDAbilities();
            Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
            Player.AttachedLevel.UpdatePlayerSpellIcon();
            Player.UpdateEquipmentColours();
            Player.AttachedLevel.RefreshPlayerHUDPos();
            Player.CurrentHealth = m_storedHealth;
            Player.CurrentMana = m_storedMana;
            if (BossKilled)
                SaveCompletionData();
            Game.PlayerStats.NewBossBeaten = true;
            if (base.LinkedRoom != null)
                Player.AttachedLevel.CloseBossDoor(base.LinkedRoom, base.LevelType);
            (Game.ScreenManager.Game as Game).SaveManager.SaveFiles(new[] {
                SaveType.PlayerData
            });
            base.OnExit();
        }

        public void DisplayBossTitle(string bossTitle1, string bossTitle2, string endHandler) {
            SoundManager.PlaySound("Boss_Title");
            m_bossTitle1.Text = bossTitle1;
            m_bossTitle2.Text = bossTitle2;
            Camera2D camera = Player.AttachedLevel.Camera;
            if (Player.AttachedLevel.CurrentRoom is LastBossRoom)
                m_bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 100f);
            else
                m_bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 50f);
            m_bossTitle2.X = m_bossTitle1.X - 0f;
            m_bossTitle2.Y = m_bossTitle1.Y + 50f;
            m_bossDivider.Position = m_bossTitle1.Position;
            m_bossDivider.Y += (float)(m_bossTitle1.Height - 5);
            m_bossTitle1.X -= 1000f;
            m_bossTitle2.X += 1500f;
            Tween.To(m_bossDivider, 0.5f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "0.3",
                "Opacity",
                "1"
            });
            Tween.To(m_bossDivider, 1f, new Easing(Quad.EaseInOut), new[] {
                "delay",
                "0",
                "ScaleX",
                ((float)(m_bossTitle2.Width / 5)).ToString()
            });
            Tween.To(m_bossTitle1, 0.5f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "0.3",
                "Opacity",
                "1"
            });
            Tween.To(m_bossTitle2, 0.5f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "0.3",
                "Opacity",
                "1"
            });
            Tween.By(m_bossTitle1, 1f, new Easing(Quad.EaseOut), new[] {
                "X",
                "1000"
            });
            Tween.By(m_bossTitle2, 1f, new Easing(Quad.EaseOut), new[] {
                "X",
                "-1500"
            });
            m_bossTitle1.X += 1000f;
            m_bossTitle2.X -= 1500f;
            Tween.By(m_bossTitle1, 2f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "1",
                "X",
                "20"
            });
            Tween.By(m_bossTitle2, 2f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "1",
                "X",
                "-20"
            });
            m_bossTitle1.X -= 1000f;
            m_bossTitle2.X += 1500f;
            Tween.AddEndHandlerToLastTween(this, endHandler, new object[0]);
            Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", new object[] {
                "Boss_Title_Exit"
            });
            m_bossTitle1.X += 1020f;
            m_bossTitle2.X -= 1520f;
            m_bossTitle1.Opacity = 1f;
            m_bossTitle2.Opacity = 1f;
            Tween.To(m_bossTitle1, 0.5f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "3",
                "Opacity",
                "0"
            });
            Tween.To(m_bossTitle2, 0.5f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "3",
                "Opacity",
                "0"
            });
            Tween.By(m_bossTitle1, 0.6f, new Easing(Quad.EaseIn), new[] {
                "delay",
                "3",
                "X",
                "1500"
            });
            Tween.By(m_bossTitle2, 0.6f, new Easing(Quad.EaseIn), new[] {
                "delay",
                "3",
                "X",
                "-1000"
            });
            m_bossTitle1.Opacity = 0f;
            m_bossTitle2.Opacity = 0f;
            m_bossDivider.Opacity = 1f;
            Tween.To(m_bossDivider, 0.5f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "2.8",
                "Opacity",
                "0"
            });
            m_bossDivider.Opacity = 0f;
        }

        public override void Update(GameTime gameTime) {
            foreach (RaindropObj current in m_rainFG)
                current.UpdateNoCollision(gameTime);
            if (!m_cutsceneRunning)
                base.Update(gameTime);
            if (BossKilled && !m_bossChest.Visible) {
                BossCleanup();
                m_bossChest.Visible = true;
                m_bossChest.Opacity = 0f;
                SoundManager.PlayMusic("TitleScreenSong", true, 1f);
                Tween.To(m_bossChest, 4f, new Easing(Tween.EaseNone), new[] {
                    "Opacity",
                    "1"
                });
                Tween.To(m_bossChest, 4f, new Easing(Quad.EaseOut), new[] {
                    "Y",
                    m_roomFloor.ToString()
                });
                Tween.AddEndHandlerToLastTween(this, "UnlockChest", new object[0]);
                m_sparkleTimer = 0.5f;
            }
            if (m_bossChest.Visible && !m_bossChest.IsOpen && BossKilled) {
                if (m_sparkleTimer > 0f) {
                    m_sparkleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_sparkleTimer <= 0f) {
                        m_sparkleTimer = 0.5f;
                        Tween.RunFunction(0f, Player.AttachedLevel.ImpactEffectPool, "DisplayChestSparkleEffect", new object[] {
                            new Vector2(m_bossChest.X, m_bossChest.Y - (float)(m_bossChest.Height / 2))
                        });
                        return;
                    }
                }
            }
            else if (m_bossChest.Visible && m_bossChest.IsOpen && BossKilled && !m_teleportingOut) {
                m_teleportingOut = true;
                if (LevelEV.RUN_DEMO_VERSION) {
                    (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(29, true, null);
                    return;
                }
                TeleportPlayer();
            }
        }

        protected abstract void SaveCompletionData();

        public virtual void BossCleanup() {
            Player.StopAllSpells();
        }

        public void TeleportPlayer() {
            Player.CurrentSpeed = 0f;
            Vector2 position = Player.Position;
            Vector2 scale = Player.Scale;
            LogicSet logicSet = new LogicSet(Player);
            logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", true), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, false), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            logicSet.AddAction(new PlaySoundLogicAction(new[] {
                "Teleport_Disappear"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleOut", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleport", new object[] {
                new Vector2(Player.X, Player.Bounds.Bottom),
                Player.Scale
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(this, "LoadPlayerData", new object[0]), Types.Sequence.Serial);
            if (base.LinkedRoom != null) {
                Player.Scale = m_storedScale;
                Player.OverrideInternalScale(m_storedScale);
                Player.UpdateCollisionBoxes();
                Player.Position = new Vector2((float)base.LinkedRoom.Bounds.Center.X, (base.LinkedRoom.Bounds.Bottom - 60) - ((float)Player.TerrainBounds.Bottom - Player.Y));
                logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", false), Types.Sequence.Serial);
                logicSet.AddAction(new ChangePropertyLogicAction(Player, "ScaleY", m_storedScale.Y), Types.Sequence.Serial);
                logicSet.AddAction(new TeleportLogicAction(null, Player.Position), Types.Sequence.Serial);
                logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
                logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
                logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleIn", new object[0]), Types.Sequence.Serial);
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleportReverse", new object[] {
                    new Vector2(Player.X, (base.LinkedRoom.Bounds.Bottom - 60)),
                    m_storedScale
                }), Types.Sequence.Serial);
                logicSet.AddAction(new PlaySoundLogicAction(new[] {
                    "Teleport_Reappear"
                }), Types.Sequence.Serial);
            }
            logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "ForceInvincible", false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(Player, "UnlockControls", new object[0]), Types.Sequence.Serial);
            Player.RunExternalLogicSet(logicSet);
            Player.Position = position;
            Player.Scale = scale;
            Player.UpdateCollisionBoxes();
        }

        public void TeleportScaleOut() {
            Tween.To(Player, 0.05f, new Easing(Tween.EaseNone), new[] {
                "ScaleX",
                "0"
            });
        }

        public void TeleportScaleIn() {
            Tween.To(Player, 0.05f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "0.15",
                "ScaleX",
                m_storedScale.X.ToString()
            });
        }

        public void KickPlayerOut() {
            Player.AttachedLevel.PauseScreen();
            SoundManager.StopMusic(0f);
            Player.LockControls();
            Player.AttachedLevel.RunWhiteSlashEffect();
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
            Player.IsWeighted = false;
            Player.IsCollidable = false;
            Player.CurrentSpeed = 0f;
            Player.AccelerationX = 0f;
            Player.AccelerationY = 0f;
            if (this is BlobChallengeRoom) {
                Tween.To(Player.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new[] {
                    "Zoom",
                    "1",
                    "X",
                    Player.X.ToString(),
                    "Y",
                    Player.Y.ToString()
                });
                Tween.AddEndHandlerToLastTween(this, "LockCamera", new object[0]);
            }
            Tween.RunFunction(1f, this, "KickPlayerOut2", new object[0]);
        }

        public void KickPlayerOut2() {
            Player.AttachedLevel.UnpauseScreen();
            Player.CurrentSpeed = 0f;
            Vector2 position = Player.Position;
            Vector2 scale = Player.Scale;
            LogicSet logicSet = new LogicSet(Player);
            logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", true), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(1.3f, false), Types.Sequence.Serial);
            logicSet.AddAction(new PlaySoundLogicAction(new[] {
                "Teleport_Disappear"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleOut", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleport", new object[] {
                new Vector2(Player.X, Player.Bounds.Bottom),
                Player.Scale
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(this, "LoadPlayerData", new object[0]), Types.Sequence.Serial);
            if (base.LinkedRoom != null) {
                Player.Scale = m_storedScale;
                Player.OverrideInternalScale(m_storedScale);
                Player.UpdateCollisionBoxes();
                Player.Position = new Vector2((float)base.LinkedRoom.Bounds.Center.X, (base.LinkedRoom.Bounds.Bottom - 60) - ((float)Player.TerrainBounds.Bottom - Player.Y));
                logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", false), Types.Sequence.Serial);
                logicSet.AddAction(new ChangePropertyLogicAction(Player, "ScaleY", m_storedScale.Y), Types.Sequence.Serial);
                logicSet.AddAction(new TeleportLogicAction(null, Player.Position), Types.Sequence.Serial);
                logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
                logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
                logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleIn", new object[0]), Types.Sequence.Serial);
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleportReverse", new object[] {
                    new Vector2(Player.X, (base.LinkedRoom.Bounds.Bottom - 60)),
                    m_storedScale
                }), Types.Sequence.Serial);
                logicSet.AddAction(new PlaySoundLogicAction(new[] {
                    "Teleport_Reappear"
                }), Types.Sequence.Serial);
            }
            logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsWeighted", true), Types.Sequence.Serial);
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsCollidable", true), Types.Sequence.Serial);
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "ForceInvincible", false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(Player, "UnlockControls", new object[0]), Types.Sequence.Serial);
            Player.RunExternalLogicSet(logicSet);
            Player.Position = position;
            Player.Scale = scale;
            Player.UpdateCollisionBoxes();
        }

        public void LockCamera() {
            Player.AttachedLevel.CameraLockedToPlayer = true;
        }

        public void UnlockChest() {
            m_bossChest.IsLocked = false;
        }

        public override void Draw(Camera2D camera) {
            foreach (RaindropObj current in m_rainFG)
                current.Draw(camera);
            base.Draw(camera);
            m_bossDivider.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            m_bossTitle1.Draw(camera);
            m_bossTitle2.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_bossChest = null;
                m_bossDivider.Dispose();
                m_bossDivider = null;
                m_bossTitle1.Dispose();
                m_bossTitle1 = null;
                m_bossTitle2.Dispose();
                m_bossTitle2 = null;
                foreach (RaindropObj current in m_rainFG)
                    current.Dispose();
                m_rainFG.Clear();
                m_rainFG = null;
                base.Dispose();
            }
        }
    }
}
