using System;
using System.Globalization;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class LoadingScreen : Screen {
        private SpriteObj m_blackScreen;
        private SpriteObj m_blackTransitionIn;
        private SpriteObj m_blackTransitionOut;
        private ImpactEffectPool m_effectPool;
        private bool m_gameCrashed;
        private ObjContainer m_gateSprite;
        private bool m_horizontalShake;
        private bool m_isLoading;
        private Screen m_levelToLoad;
        private bool m_loadingComplete;
        private TextObj m_loadingText;
        private float m_screenShakeMagnitude;
        private byte m_screenTypeToLoad;
        private bool m_shakeScreen;
        private bool m_verticalShake;
        private bool m_wipeTransition;

        public LoadingScreen(byte screenType, bool wipeTransition) {
            m_screenTypeToLoad = screenType;
            m_effectPool = new ImpactEffectPool(50);
            m_wipeTransition = wipeTransition;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent() {
            m_loadingText = new TextObj(null);
            m_loadingText.Font = Game.JunicodeLargeFont;
            m_loadingText.Text = "Building";
            m_loadingText.Align = Types.TextAlign.Centre;
            m_loadingText.FontSize = 40f;
            m_loadingText.OutlineWidth = 4;
            m_loadingText.ForceDraw = true;
            m_gateSprite = new ObjContainer("LoadingScreenGate_Character");
            m_gateSprite.ForceDraw = true;
            m_gateSprite.Scale = new Vector2(2f, 2f);
            m_gateSprite.Y -= (float)m_gateSprite.Height;
            m_effectPool.Initialize();
            m_blackTransitionIn = new SpriteObj("Blank_Sprite");
            m_blackTransitionIn.Rotation = 15f;
            m_blackTransitionIn.Scale = new Vector2((1320 / m_blackTransitionIn.Width), (2000 / m_blackTransitionIn.Height));
            m_blackTransitionIn.TextureColor = Color.Black;
            m_blackTransitionIn.ForceDraw = true;
            m_blackScreen = new SpriteObj("Blank_Sprite");
            m_blackScreen.Scale = new Vector2((1320 / m_blackScreen.Width), (720 / m_blackScreen.Height));
            m_blackScreen.TextureColor = Color.Black;
            m_blackScreen.ForceDraw = true;
            m_blackTransitionOut = new SpriteObj("Blank_Sprite");
            m_blackTransitionOut.Rotation = 15f;
            m_blackTransitionOut.Scale = new Vector2((1320 / m_blackTransitionOut.Width), (2000 / m_blackTransitionOut.Height));
            m_blackTransitionOut.TextureColor = Color.Black;
            m_blackTransitionOut.ForceDraw = true;
            base.LoadContent();
        }

        public override void OnEnter() {
            BackBufferOpacity = 0f;
            m_gameCrashed = false;
            if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
                m_loadingText.Text = "Jacking In";
            else if (Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f)
                m_loadingText.Text = "Reminiscing";
            else if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
                m_loadingText.Text = "Balding";
            else
                m_loadingText.Text = "Building";
            if (!m_loadingComplete) {
                if (m_screenTypeToLoad == 27) {
                    Tween.To(this, 0.05f, new Easing(Tween.EaseNone), new[] {
                        "BackBufferOpacity",
                        "1"
                    });
                    Tween.RunFunction(1f, this, "BeginThreading", new object[0]);
                }
                else {
                    m_blackTransitionIn.X = 0f;
                    m_blackTransitionIn.X = (float)(1320 - m_blackTransitionIn.Bounds.Left);
                    m_blackScreen.X = m_blackTransitionIn.X;
                    m_blackTransitionOut.X = m_blackScreen.X + (float)m_blackScreen.Width;
                    if (!m_wipeTransition) {
                        SoundManager.PlaySound("GateDrop");
                        Tween.To(m_gateSprite, 0.5f, new Easing(Tween.EaseNone), new[] {
                            "Y",
                            "0"
                        });
                        Tween.RunFunction(0.3f, m_effectPool, "LoadingGateSmokeEffect", new object[] {
                            40
                        });
                        Tween.RunFunction(0.3f, typeof(SoundManager), "PlaySound", new object[] {
                            "GateSlam"
                        });
                        Tween.RunFunction(0.55f, this, "ShakeScreen", new object[] {
                            4,
                            true,
                            true
                        });
                        Tween.RunFunction(0.65f, this, "StopScreenShake", new object[0]);
                        Tween.RunFunction(1.5f, this, "BeginThreading", new object[0]);
                    }
                    else {
                        Tween.By(m_blackTransitionIn, 0.15f, new Easing(Quad.EaseIn), new[] {
                            "X",
                            (-m_blackTransitionIn.X).ToString()
                        });
                        Tween.By(m_blackScreen, 0.15f, new Easing(Quad.EaseIn), new[] {
                            "X",
                            (-m_blackTransitionIn.X).ToString()
                        });
                        Tween.By(m_blackTransitionOut, 0.2f, new Easing(Quad.EaseIn), new[] {
                            "X",
                            (-m_blackTransitionIn.X).ToString()
                        });
                        Tween.AddEndHandlerToLastTween(this, "BeginThreading", new object[0]);
                    }
                }
                base.OnEnter();
            }
        }

        public void BeginThreading() {
            Tween.StopAll(false);
            Thread thread = new Thread(BeginLoading);
            if (thread.CurrentCulture.Name != "en-US") {
                thread.CurrentCulture = new CultureInfo("en-US", false);
                thread.CurrentUICulture = new CultureInfo("en-US", false);
            }
            thread.Start();
        }

        private void BeginLoading() {
            m_isLoading = true;
            m_loadingComplete = false;
            byte screenTypeToLoad = m_screenTypeToLoad;
            if (screenTypeToLoad <= 9) {
                switch (screenTypeToLoad) {
                    case 1:
                        m_levelToLoad = new CDGSplashScreen();
                        lock (m_levelToLoad) {
                            m_loadingComplete = true;
                            return;
                        }
                        break;
                    case 2:
                    case 4:
                        return;
                    case 3:
                        goto IL_199;
                    case 5:
                        goto IL_205;
                    default:
                        if (screenTypeToLoad != 9)
                            return;
                        goto IL_1CF;
                }
            }
            else {
                if (screenTypeToLoad == 15)
                    goto IL_205;
                if (screenTypeToLoad == 18)
                    goto IL_11E;
                switch (screenTypeToLoad) {
                    case 23:
                    case 24:
                        goto IL_205;
                    case 25:
                    case 26:
                        return;
                    case 27:
                        goto IL_199;
                    case 28:
                        goto IL_E8;
                    case 29:
                        break;
                    default:
                        return;
                }
            }
            m_levelToLoad = new DemoEndScreen();
            lock (m_levelToLoad) {
                m_loadingComplete = true;
                return;
            }
            IL_E8:
            m_levelToLoad = new DemoStartScreen();
            lock (m_levelToLoad) {
                m_loadingComplete = true;
                return;
            }
            IL_11E:
            m_levelToLoad = new CreditsScreen();
            bool isEnding = true;
            Screen[] screens = base.ScreenManager.GetScreens();
            for (int i = 0; i < screens.Length; i++) {
                Screen screen = screens[i];
                if (screen is TitleScreen) {
                    isEnding = false;
                    break;
                }
            }
            (m_levelToLoad as CreditsScreen).IsEnding = isEnding;
            lock (m_levelToLoad) {
                m_loadingComplete = true;
                return;
            }
            IL_199:
            m_levelToLoad = new TitleScreen();
            lock (m_levelToLoad) {
                m_loadingComplete = true;
                return;
            }
            IL_1CF:
            m_levelToLoad = new LineageScreen();
            lock (m_levelToLoad) {
                m_loadingComplete = true;
                return;
            }
            IL_205:
            RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
            AreaStruct[] area1List = Game.Area1List;
            m_levelToLoad = null;
            if (m_screenTypeToLoad == 15)
                m_levelToLoad = LevelBuilder2.CreateStartingRoom();
            else if (m_screenTypeToLoad == 23)
                m_levelToLoad = LevelBuilder2.CreateTutorialRoom();
            else if (m_screenTypeToLoad == 24)
                m_levelToLoad = LevelBuilder2.CreateEndingRoom();
            else {
                ProceduralLevelScreen levelScreen = (base.ScreenManager as RCScreenManager).GetLevelScreen();
                if (levelScreen != null) {
                    if (Game.PlayerStats.LockCastle) {
                        try {
                            m_levelToLoad = (base.ScreenManager.Game as Game).SaveManager.LoadMap();
                        } catch {
                            m_gameCrashed = true;
                        }
                        if (!m_gameCrashed) {
                            (base.ScreenManager.Game as Game).SaveManager.LoadFiles(m_levelToLoad as ProceduralLevelScreen, new[] {
                                SaveType.MapData
                            });
                            Game.PlayerStats.LockCastle = false;
                        }
                    }
                    else
                        m_levelToLoad = LevelBuilder2.CreateLevel(levelScreen.RoomList[0], area1List);
                }
                else if (Game.PlayerStats.LoadStartingRoom) {
                    Console.WriteLine("This should only be used for debug purposes");
                    m_levelToLoad = LevelBuilder2.CreateLevel(null, area1List);
                    (base.ScreenManager.Game as Game).SaveManager.SaveFiles(new[] {
                        SaveType.Map,
                        SaveType.MapData
                    });
                }
                else {
                    try {
                        m_levelToLoad = (base.ScreenManager.Game as Game).SaveManager.LoadMap();
                        (base.ScreenManager.Game as Game).SaveManager.LoadFiles(m_levelToLoad as ProceduralLevelScreen, new[] {
                            SaveType.MapData
                        });
                    } catch {
                        m_gameCrashed = true;
                    }
                    if (!m_gameCrashed)
                        Game.ScreenManager.Player.Position = new Vector2((m_levelToLoad as ProceduralLevelScreen).RoomList[1].X, 420f);
                }
            }
            if (!m_gameCrashed) {
                lock (m_levelToLoad) {
                    ProceduralLevelScreen proceduralLevelScreen = m_levelToLoad as ProceduralLevelScreen;
                    proceduralLevelScreen.Player = rCScreenManager.Player;
                    rCScreenManager.Player.AttachLevel(proceduralLevelScreen);
                    for (int j = 0; j < proceduralLevelScreen.RoomList.Count; j++)
                        proceduralLevelScreen.RoomList[j].RoomNumber = j + 1;
                    rCScreenManager.AttachMap(proceduralLevelScreen);
                    if (!m_wipeTransition)
                        Thread.Sleep(100);
                    m_loadingComplete = true;
                }
            }
        }

        public override void Update(GameTime gameTime) {
            if (m_gameCrashed)
                (base.ScreenManager.Game as Game).SaveManager.ForceBackup();
            if (m_isLoading && m_loadingComplete && !m_gameCrashed)
                EndLoading();
            float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_gateSprite.GetChildAt(1).Rotation += 120f * num;
            m_gateSprite.GetChildAt(2).Rotation -= 120f * num;
            if (m_shakeScreen)
                UpdateShake();
            base.Update(gameTime);
        }

        public void EndLoading() {
            m_isLoading = false;
            ScreenManager screenManager = base.ScreenManager;
            Screen[] screens = base.ScreenManager.GetScreens();
            for (int i = 0; i < screens.Length; i++) {
                Screen screen = screens[i];
                if (screen != this)
                    screenManager.RemoveScreen(screen, true);
                else
                    screenManager.RemoveScreen(screen, false);
            }
            base.ScreenManager = screenManager;
            m_levelToLoad.DrawIfCovered = true;
            if (m_screenTypeToLoad == 15) {
                if (Game.PlayerStats.IsDead)
                    (m_levelToLoad as ProceduralLevelScreen).DisableRoomOnEnter = true;
                base.ScreenManager.AddScreen(m_levelToLoad, new PlayerIndex?(PlayerIndex.One));
                if (Game.PlayerStats.IsDead) {
                    base.ScreenManager.AddScreen((base.ScreenManager as RCScreenManager).SkillScreen, new PlayerIndex?(PlayerIndex.One));
                    (m_levelToLoad as ProceduralLevelScreen).DisableRoomOnEnter = false;
                }
                m_levelToLoad.UpdateIfCovered = false;
            }
            else {
                base.ScreenManager.AddScreen(m_levelToLoad, new PlayerIndex?(PlayerIndex.One));
                m_levelToLoad.UpdateIfCovered = true;
            }
            base.ScreenManager.AddScreen(this, new PlayerIndex?(PlayerIndex.One));
            AddFinalTransition();
        }

        public void AddFinalTransition() {
            if (m_screenTypeToLoad == 27) {
                BackBufferOpacity = 1f;
                Tween.To(this, 2f, new Easing(Tween.EaseNone), new[] {
                    "BackBufferOpacity",
                    "0"
                });
                Tween.AddEndHandlerToLastTween(base.ScreenManager, "RemoveScreen", new object[] {
                    this,
                    true
                });
                return;
            }
            if (!m_wipeTransition) {
                SoundManager.PlaySound("GateRise");
                Tween.To(m_gateSprite, 1f, new Easing(Tween.EaseNone), new[] {
                    "Y",
                    (-m_gateSprite.Height).ToString()
                });
                Tween.AddEndHandlerToLastTween(base.ScreenManager, "RemoveScreen", new object[] {
                    this,
                    true
                });
                return;
            }
            m_blackTransitionOut.Y = -500f;
            Tween.By(m_blackTransitionIn, 0.2f, new Easing(Tween.EaseNone), new[] {
                "X",
                (-m_blackTransitionIn.Bounds.Width).ToString()
            });
            Tween.By(m_blackScreen, 0.2f, new Easing(Tween.EaseNone), new[] {
                "X",
                (-m_blackTransitionIn.Bounds.Width).ToString()
            });
            Tween.By(m_blackTransitionOut, 0.2f, new Easing(Tween.EaseNone), new[] {
                "X",
                (-m_blackTransitionIn.Bounds.Width).ToString()
            });
            Tween.AddEndHandlerToLastTween(base.ScreenManager, "RemoveScreen", new object[] {
                this,
                true
            });
        }

        public override void Draw(GameTime gameTime) {
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            if (m_screenTypeToLoad != 27) {
                if (!m_wipeTransition) {
                    m_gateSprite.Draw(base.Camera);
                    m_effectPool.Draw(base.Camera);
                    m_loadingText.Position = new Vector2(m_gateSprite.X + 995f, m_gateSprite.Y + 540f);
                    m_loadingText.Draw(base.Camera);
                }
                else {
                    m_blackTransitionIn.Draw(base.Camera);
                    m_blackTransitionOut.Draw(base.Camera);
                    m_blackScreen.Draw(base.Camera);
                }
            }
            base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.White * BackBufferOpacity);
            base.Camera.End();
            base.Draw(gameTime);
        }

        public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true) {
            m_screenShakeMagnitude = magnitude;
            m_horizontalShake = horizontalShake;
            m_verticalShake = verticalShake;
            m_shakeScreen = true;
        }

        public void UpdateShake() {
            if (m_horizontalShake)
                m_gateSprite.X = (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * m_screenShakeMagnitude);
            if (m_verticalShake)
                m_gateSprite.Y = (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * m_screenShakeMagnitude);
        }

        public void StopScreenShake() {
            m_shakeScreen = false;
            m_gateSprite.X = 0f;
            m_gateSprite.Y = 0f;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                Console.WriteLine("Disposing Loading Screen");
                m_loadingText.Dispose();
                m_loadingText = null;
                m_levelToLoad = null;
                m_gateSprite.Dispose();
                m_gateSprite = null;
                m_effectPool.Dispose();
                m_effectPool = null;
                m_blackTransitionIn.Dispose();
                m_blackTransitionIn = null;
                m_blackScreen.Dispose();
                m_blackScreen = null;
                m_blackTransitionOut.Dispose();
                m_blackTransitionOut = null;
                base.Dispose();
            }
        }
    }
}
