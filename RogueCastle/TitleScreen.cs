using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Randomchaos2DGodRays;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {

    public class TitleScreen : Screen {

        private SpriteObj _bg;
        private SpriteObj _castle;
        private TextObj _copyrightText;
        private SpriteObj _creditsIcon;
        private KeyIconTextObj _creditsKey;
        private SpriteObj _crown;
        private SpriteObj _dlcIcon;
        private CrepuscularRays _godRay;
        private RenderTarget2D _godRayTexture;
        private float _hardCoreModeOpacity;
        private bool _heroIsDead;
        private SpriteObj _largeCloud1;
        private SpriteObj _largeCloud2;
        private SpriteObj _largeCloud3;
        private SpriteObj _largeCloud4;
        private bool m_loadStartingRoom;
        private SpriteObj m_logo;
        private bool m_optionsEntered;
        private SpriteObj m_optionsIcon;
        private KeyIconTextObj m_optionsKey;
        private PostProcessingManager m_ppm;
        private KeyIconTextObj m_pressStartText;
        private TextObj m_pressStartText2;
        private SpriteObj m_profileCard;
        private KeyIconTextObj m_profileCardKey;
        private KeyIconTextObj m_profileSelectKey;
        private float m_randomSeagullSFX;
        private Cue m_seagullCue;
        private SpriteObj m_smallCloud1;
        private SpriteObj m_smallCloud2;
        private SpriteObj m_smallCloud3;
        private SpriteObj m_smallCloud4;
        private SpriteObj m_smallCloud5;
        private bool m_startNewGamePlus;
        private bool m_startNewLegacy;
        private bool m_startPressed;
        private TextObj m_titleText;
        private TextObj m_versionNumber;
        private TextObj _rlRedux;

        public override void LoadContent() {

            m_ppm = new PostProcessingManager(ScreenManager.Game, ScreenManager.Camera);
            _godRay = new CrepuscularRays(ScreenManager.Game, Vector2.One * 0.5f, "GameSpritesheets/flare3", 2f, 0.97f, 0.97f, 0.5f, 1.25f);
            m_ppm.AddEffect(_godRay);
            _godRayTexture = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color, DepthFormat.None);
            _godRay.LightSource = new Vector2(0.495f, 0.3f);
            
            _bg = new SpriteObj("TitleBG_Sprite");
            _bg.Scale = new Vector2(1320f / _bg.Width, 720f / _bg.Height);
            _bg.TextureColor = Color.Red;
            
            _hardCoreModeOpacity = 0f;
            
            m_logo = new SpriteObj("TitleLogo_Sprite");
            m_logo.Position = new Vector2(660f, 360f);
            m_logo.DropShadow = new Vector2(0f, 5f);
            
            _castle = new SpriteObj("TitleCastle_Sprite");
            _castle.Scale = new Vector2(2f, 2f);
            _castle.Position = new Vector2(630f, (720 - _castle.Height / 2));
            
            m_smallCloud1 = new SpriteObj("TitleSmallCloud1_Sprite");
            m_smallCloud1.Position = new Vector2(660f, 0f);
            
            m_smallCloud2 = new SpriteObj("TitleSmallCloud2_Sprite");
            m_smallCloud2.Position = m_smallCloud1.Position;
            
            m_smallCloud3 = new SpriteObj("TitleSmallCloud3_Sprite");
            m_smallCloud3.Position = m_smallCloud1.Position;
            
            m_smallCloud4 = new SpriteObj("TitleSmallCloud4_Sprite");
            m_smallCloud4.Position = m_smallCloud1.Position;
            
            m_smallCloud5 = new SpriteObj("TitleSmallCloud5_Sprite");
            m_smallCloud5.Position = m_smallCloud1.Position;
            
            _largeCloud1 = new SpriteObj("TitleLargeCloud1_Sprite");
            _largeCloud1.Position = new Vector2(0f, (720 - _largeCloud1.Height));
            
            _largeCloud2 = new SpriteObj("TitleLargeCloud2_Sprite");
            _largeCloud2.Position = new Vector2(440f, (720 - _largeCloud2.Height + 130));
            
            _largeCloud3 = new SpriteObj("TitleLargeCloud1_Sprite");
            _largeCloud3.Position = new Vector2(880f, (720 - _largeCloud3.Height + 50));
            _largeCloud3.Flip = SpriteEffects.FlipHorizontally;
            
            _largeCloud4 = new SpriteObj("TitleLargeCloud2_Sprite");
            _largeCloud4.Position = new Vector2(1320f, (720 - _largeCloud4.Height));
            _largeCloud4.Flip = SpriteEffects.FlipHorizontally;
            
            m_titleText = new TextObj();
            m_titleText.Font = Game.JunicodeFont;
            m_titleText.FontSize = 45f;
            m_titleText.Text = "RL REDUX";
            m_titleText.Position = new Vector2(660f, 60f);
            m_titleText.Align = Types.TextAlign.Centre;
            
            _copyrightText = new TextObj(Game.JunicodeFont);
            _copyrightText.FontSize = 8f;
            _copyrightText.Text = " Copyright(C) 2011-2013, Cellar Door Games Inc. Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved. (Modified By: NeoCode)";
            _copyrightText.Align = Types.TextAlign.Centre;
            _copyrightText.Position = new Vector2(660f, (720 - _copyrightText.Height - 10));
            _copyrightText.DropShadow = new Vector2(1f, 2f);
            
            m_versionNumber = (_copyrightText.Clone() as TextObj);
            m_versionNumber.Align = Types.TextAlign.Right;
            m_versionNumber.FontSize = 8f;
            m_versionNumber.Position = new Vector2(1305f, 5f);
            m_versionNumber.Text = "v1.2.0b - RL Redux v0.0.1";
            
            m_pressStartText = new KeyIconTextObj(Game.JunicodeFont);
            m_pressStartText.FontSize = 20f;
            m_pressStartText.Text = "Press Enter to begin";
            m_pressStartText.Align = Types.TextAlign.Centre;
            m_pressStartText.Position = new Vector2(660f, 560f);
            m_pressStartText.DropShadow = new Vector2(2f, 2f);
            
            m_pressStartText2 = new TextObj(Game.JunicodeFont);
            m_pressStartText2.FontSize = 20f;
            m_pressStartText2.Text = "Press Enter to begin";
            m_pressStartText2.Align = Types.TextAlign.Centre;
            m_pressStartText2.Position = m_pressStartText.Position;
            m_pressStartText2.Y -= m_pressStartText.Height - 5;
            m_pressStartText2.DropShadow = new Vector2(2f, 2f);
            
            _rlRedux = new TextObj(Game.JunicodeFont);
            _rlRedux.FontSize = 40f;
            _rlRedux.Text = "Redux";
            _rlRedux.Align = Types.TextAlign.Left;
            _rlRedux.Position = new Vector2(910f, 410f);
            _rlRedux.DropShadow = new Vector2(2f, 5f);
            _rlRedux.TextureColor = Color.Yellow;
            _rlRedux.OutlineColour = Color.Black;

            m_profileCard = new SpriteObj("TitleProfileCard_Sprite");
            m_profileCard.OutlineWidth = 2;
            m_profileCard.Scale = new Vector2(2f, 2f);
            m_profileCard.Position = new Vector2(m_profileCard.Width, (720 - m_profileCard.Height));
            m_profileCard.ForceDraw = true;
            
            m_optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite");
            m_optionsIcon.Scale = new Vector2(2f, 2f);
            m_optionsIcon.OutlineWidth = m_profileCard.OutlineWidth;
            m_optionsIcon.Position = new Vector2((1320 - m_optionsIcon.Width * 2), m_profileCard.Y);
            m_optionsIcon.ForceDraw = true;
            
            _creditsIcon = new SpriteObj("TitleCreditsIcon_Sprite");
            _creditsIcon.Scale = new Vector2(2f, 2f);
            _creditsIcon.OutlineWidth = m_profileCard.OutlineWidth;
            _creditsIcon.Position = new Vector2(m_optionsIcon.X + 120f, m_profileCard.Y);
            _creditsIcon.ForceDraw = true;
            
            m_profileCardKey = new KeyIconTextObj(Game.JunicodeFont);
            m_profileCardKey.Align = Types.TextAlign.Centre;
            m_profileCardKey.FontSize = 12f;
            m_profileCardKey.Text = "[Input:" + 7 + "]";
            m_profileCardKey.Position = new Vector2(m_profileCard.X, (m_profileCard.Bounds.Top - m_profileCardKey.Height - 10));
            m_profileCardKey.ForceDraw = true;
            
            m_optionsKey = new KeyIconTextObj(Game.JunicodeFont);
            m_optionsKey.Align = Types.TextAlign.Centre;
            m_optionsKey.FontSize = 12f;
            m_optionsKey.Text = "[Input:" + 4 + "]";
            m_optionsKey.Position = new Vector2(m_optionsIcon.X, (m_optionsIcon.Bounds.Top - m_optionsKey.Height - 10));
            m_optionsKey.ForceDraw = true;
            
            _creditsKey = new KeyIconTextObj(Game.JunicodeFont);
            _creditsKey.Align = Types.TextAlign.Centre;
            _creditsKey.FontSize = 12f;
            _creditsKey.Text = "[Input:" + 6 + "]";
            _creditsKey.Position = new Vector2(_creditsIcon.X, (_creditsIcon.Bounds.Top - _creditsKey.Height - 10));
            _creditsKey.ForceDraw = true;
            
            m_profileSelectKey = new KeyIconTextObj(Game.JunicodeFont);
            m_profileSelectKey.Align = Types.TextAlign.Left;
            m_profileSelectKey.FontSize = 10f;
            m_profileSelectKey.Text = string.Concat(new object[] {
                "[Input:",
                25,
                "] to Change Profile (",
                Game.GameConfig.ProfileSlot,
                ")"
            });
            m_profileSelectKey.Position = new Vector2(30f, 15f);
            m_profileSelectKey.ForceDraw = true;
            m_profileSelectKey.DropShadow = new Vector2(2f, 2f);
            
            _crown = new SpriteObj("Crown_Sprite");
            _crown.ForceDraw = true;
            _crown.Scale = new Vector2(0.7f, 0.7f);
            _crown.Rotation = -30f;
            _crown.OutlineWidth = 2;
            
            _dlcIcon = new SpriteObj("MedallionPiece5_Sprite");
            _dlcIcon.Position = new Vector2(950f, 310f);
            _dlcIcon.ForceDraw = true;
            _dlcIcon.TextureColor = Color.Yellow;
            
            base.LoadContent();

        }

        public override void OnEnter() {
            
            Camera.Zoom = 1f;
            m_profileSelectKey.Text = string.Concat(new object[] {
                "[Input:",
                25,
                "] to Change Profile (",
                Game.GameConfig.ProfileSlot,
                ")"
            });
            SoundManager.PlayMusic("TitleScreenSong", true, 1f);
            Game.ScreenManager.Player.ForceInvincible = false;
            m_optionsEntered = false;
            m_startNewLegacy = false;
            _heroIsDead = false;
            m_startNewGamePlus = false;
            m_loadStartingRoom = false;
            _bg.TextureColor = Color.Red;
            _crown.Visible = false;
            m_randomSeagullSFX = CDGMath.RandomInt(1, 5);
            m_startPressed = false;
            Tween.By(_godRay, 5f, Quad.EaseInOut, new[] {
                "Y",
                "-0.23"
            });

            m_logo.Opacity = 0f;
            m_logo.Position = new Vector2(660f, 310f);
            Tween.To(m_logo, 2f, Linear.EaseNone, new[] {
                "Opacity",
                "1"
            });
            Tween.To(m_logo, 3f, Quad.EaseInOut, new[] {
                "Y",
                "360"
            });

            _rlRedux.Opacity = 0f;
            _rlRedux.Position = new Vector2(910, 360);
            Tween.To(_rlRedux, 2f, Linear.EaseNone, new[] {
                "Opacity",
                "1"
            });
            Tween.To(_rlRedux, 3f, Quad.EaseInOut, new[] {
                "Y",
                "410"
            });

            _crown.Opacity = 0f;
            _crown.Position = new Vector2(390f, 200f);
            Tween.To(_crown, 2f, Linear.EaseNone, new[] {
                "Opacity",
                "1"
            });
            Tween.By(_crown, 3f, Quad.EaseInOut, new[] {
                "Y",
                "50"
            });
            _dlcIcon.Opacity = 0f;
            _dlcIcon.Visible = false;
            if (Game.PlayerStats.ChallengeLastBossBeaten)
                _dlcIcon.Visible = true;
            _dlcIcon.Position = new Vector2(898f, 267f);
            Tween.To(_dlcIcon, 2f, Linear.EaseNone, new[] {
                "Opacity",
                "1"
            });
            Tween.By(_dlcIcon, 3f, Quad.EaseInOut, new[] {
                "Y",
                "50"
            });
            Camera.Position = new Vector2(660f, 360f);
            m_pressStartText.Text = "[Input:" + 0 + "]";
            LoadSaveData();
            Game.PlayerStats.TutorialComplete = true;
            m_startNewLegacy = !Game.PlayerStats.CharacterFound;
            _heroIsDead = Game.PlayerStats.IsDead;
            m_startNewGamePlus = Game.PlayerStats.LastbossBeaten;
            m_loadStartingRoom = Game.PlayerStats.LoadStartingRoom;
            if (Game.PlayerStats.TimesCastleBeaten > 0) {
                _crown.Visible = true;
                _bg.TextureColor = Color.White;
            }
            InitializeStartingText();
            base.OnEnter();
        }

        public override void OnExit() {

            if (m_seagullCue != null && m_seagullCue.IsPlaying) {
                m_seagullCue.Stop(AudioStopOptions.Immediate);
                m_seagullCue.Dispose();
            }

            base.OnExit();

        }

        public void LoadSaveData() {

            SkillSystem.ResetAllTraits();
            Game.PlayerStats.Dispose();
            Game.PlayerStats = new PlayerStats();
            
            (ScreenManager as RCScreenManager).Player.Reset();
            
            (ScreenManager.Game as Game).SaveManager.LoadFiles(null, new[] {
                SaveType.PlayerData,
                SaveType.Lineage,
                SaveType.UpgradeData
            });

            Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
            Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;

        }

        public void InitializeStartingText() {
            
            if (!m_startNewLegacy) {

                if (!_heroIsDead) {

                    if (Game.PlayerStats.TimesCastleBeaten == 1) {
                        m_pressStartText2.Text = "Continue Your Quest +";
                        return;
                    }

                    if (Game.PlayerStats.TimesCastleBeaten > 1) {
                        m_pressStartText2.Text = "Continue Your Quest +" + Game.PlayerStats.TimesCastleBeaten;
                        return;
                    }

                    m_pressStartText2.Text = "Continue Your Quest";

                }
                else {

                    if (Game.PlayerStats.TimesCastleBeaten == 1) {
                        m_pressStartText2.Text = "Choose Your Heir +";
                        return;
                    }

                    if (Game.PlayerStats.TimesCastleBeaten > 1) {
                        m_pressStartText2.Text = "Choose Your Heir +" + Game.PlayerStats.TimesCastleBeaten;
                        return;
                    }

                    m_pressStartText2.Text = "Choose Your Heir";

                }

            }
            else {

                if (!m_startNewGamePlus) {
                    m_pressStartText2.Text = "Start Your Legacy";
                    return;
                }

                if (Game.PlayerStats.TimesCastleBeaten == 1) {
                    m_pressStartText2.Text = "Start Your Legacy +";
                    return;
                }

                m_pressStartText2.Text = "Start Your Legacy +" + Game.PlayerStats.TimesCastleBeaten;

            }

        }

        public void StartPressed() {

            SoundManager.PlaySound("Game_Start");
            
            if (!m_startNewLegacy) {

                if (!_heroIsDead) {

                    if (m_loadStartingRoom)
                        (ScreenManager as RCScreenManager).DisplayScreen(15, true);
                    else
                        (ScreenManager as RCScreenManager).DisplayScreen(5, true);

                }
                else
                    (ScreenManager as RCScreenManager).DisplayScreen(9, true);

            }
            else {

                Game.PlayerStats.CharacterFound = true;
                
                if (m_startNewGamePlus) {

                    Game.PlayerStats.LastbossBeaten = false;
                    Game.PlayerStats.BlobBossBeaten = false;
                    Game.PlayerStats.EyeballBossBeaten = false;
                    Game.PlayerStats.FairyBossBeaten = false;
                    Game.PlayerStats.FireballBossBeaten = false;
                    Game.PlayerStats.FinalDoorOpened = false;
                    
                    if ((ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map)) {
                        
                        (ScreenManager.Game as Game).SaveManager.ClearFiles(new[] {
                            SaveType.Map,
                            SaveType.MapData
                        });
                        
                        (ScreenManager.Game as Game).SaveManager.ClearBackupFiles(new[] {
                            SaveType.Map,
                            SaveType.MapData
                        });

                    }

                }
                else
                    Game.PlayerStats.Gold = 0;

                Game.PlayerStats.HeadPiece = (byte)CDGMath.RandomInt(1, 5);
                Game.PlayerStats.EnemiesKilledInRun.Clear();
                
                (ScreenManager.Game as Game).SaveManager.SaveFiles(new[] {
                    SaveType.PlayerData,
                    SaveType.Lineage,
                    SaveType.UpgradeData
                });

                (ScreenManager as RCScreenManager).DisplayScreen(15, true, null);

            }

            SoundManager.StopMusic(0.2f);

        }

        public override void Update(GameTime gameTime) {

            if (m_randomSeagullSFX > 0f) {

                m_randomSeagullSFX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (m_randomSeagullSFX <= 0f) {

                    if (m_seagullCue != null && m_seagullCue.IsPlaying) {
                        m_seagullCue.Stop(AudioStopOptions.Immediate);
                        m_seagullCue.Dispose();
                    }

                    m_seagullCue = SoundManager.PlaySound("Wind1");
                    m_randomSeagullSFX = CDGMath.RandomInt(10, 15);

                }

            }

            float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_smallCloud1.Rotation += 1.8f * num;
            m_smallCloud2.Rotation += 1.2f * num;
            m_smallCloud3.Rotation += 3f * num;
            m_smallCloud4.Rotation -= 0.6f * num;
            m_smallCloud5.Rotation -= 1.8f * num;
            _largeCloud2.X += 2.4f * num;
            
            if (_largeCloud2.Bounds.Left > 1320)
                _largeCloud2.X = (-(float)(_largeCloud2.Width / 2));
            
            _largeCloud3.X -= 3f * num;
            
            if (_largeCloud3.Bounds.Right < 0)
                _largeCloud3.X = (float)(1320 + _largeCloud3.Width / 2);
            
            if (!m_startPressed)
                m_pressStartText.Opacity = (float)Math.Abs(Math.Sin((Game.TotalGameTime * 1f)));
            
            _godRay.LightSourceSize = 1f + (float)Math.Abs(Math.Sin((Game.TotalGameTime * 0.5f))) * 0.5f;
            
            if (m_optionsEntered && Game.ScreenManager.CurrentScreen == this) {

                m_optionsEntered = false;
                m_optionsKey.Text = "[Input:" + 4 + "]";
                m_profileCardKey.Text = "[Input:" + 7 + "]";
                _creditsKey.Text = "[Input:" + 6 + "]";
                m_profileSelectKey.Text = string.Concat(new object[] {
                    "[Input:",
                    25,
                    "] to Change Profile (",
                    Game.GameConfig.ProfileSlot,
                    ")"
                });

            }

            ChangeRay();

            base.Update(gameTime);

        }

        public override void HandleInput() {

            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                StartPressed();
            
            if (!m_startNewLegacy && Game.GlobalInput.JustPressed(7))
                (ScreenManager as RCScreenManager).DisplayScreen(17, false);
            
            if (Game.GlobalInput.JustPressed(4)) {
                m_optionsEntered = true;
                List<object> list = new List<object>();
                list.Add(true);
                (ScreenManager as RCScreenManager).DisplayScreen(4, false, list);
            }
            
            if (Game.GlobalInput.JustPressed(6))
                (ScreenManager as RCScreenManager).DisplayScreen(18, false);
            
            if (Game.GlobalInput.JustPressed(25))
                (ScreenManager as RCScreenManager).DisplayScreen(30, false);
            
            base.HandleInput();

        }

        public void ChangeRay() {

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                _godRay.LightSource = new Vector2(_godRay.LightSource.X, _godRay.LightSource.Y - 0.01f);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                _godRay.LightSource = new Vector2(_godRay.LightSource.X, _godRay.LightSource.Y + 0.01f);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                _godRay.LightSource = new Vector2(_godRay.LightSource.X - 0.01f, _godRay.LightSource.Y);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                _godRay.LightSource = new Vector2(_godRay.LightSource.X + 0.01f, _godRay.LightSource.Y);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                _godRay.Exposure += 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.H))
                _godRay.Exposure -= 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.U))
                _godRay.LightSourceSize += 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                _godRay.LightSourceSize -= 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.I))
                _godRay.Density += 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                _godRay.Density -= 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.O))
                _godRay.Decay += 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.L))
                _godRay.Decay -= 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.P))
                _godRay.Weight += 0.01f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
                _godRay.Weight -= 0.01f;

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8)) {
                Vector2 position = _rlRedux.Position;
                position.Y += 10f;
                Console.WriteLine(position);
                _rlRedux.Position = position;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2)) {
                Vector2 position = _rlRedux.Position;
                position.Y -= 10f;
                Console.WriteLine(position);
                _rlRedux.Position = position;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4)) {
                Vector2 position = _rlRedux.Position;
                position.X -= 10f;
                Console.WriteLine(position);
                _rlRedux.Position = position;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6)) {
                Vector2 position = _rlRedux.Position;
                position.X += 10f;
                Console.WriteLine(position);
                _rlRedux.Position = position;
            }

        }

        public override void Draw(GameTime gameTime) {

            Camera.GraphicsDevice.SetRenderTarget(_godRayTexture);
            Camera.GraphicsDevice.Clear(Color.White);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null);
            m_smallCloud1.DrawOutline(Camera);
            m_smallCloud3.DrawOutline(Camera);
            m_smallCloud4.DrawOutline(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _castle.DrawOutline(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_smallCloud2.DrawOutline(Camera);
            m_smallCloud5.DrawOutline(Camera);
            m_logo.DrawOutline(Camera);
            _dlcIcon.DrawOutline(Camera);
            _crown.DrawOutline(Camera);
            Camera.End();
            m_ppm.Draw(gameTime, _godRayTexture);
            Camera.GraphicsDevice.SetRenderTarget(_godRayTexture);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            _bg.Draw(Camera);
            m_smallCloud1.Draw(Camera);
            m_smallCloud3.Draw(Camera);
            m_smallCloud4.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _castle.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_smallCloud2.Draw(Camera);
            m_smallCloud5.Draw(Camera);
            _largeCloud1.Draw(Camera);
            _largeCloud2.Draw(Camera);
            _largeCloud3.Draw(Camera);
            _largeCloud4.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            Camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.Black * _hardCoreModeOpacity);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_logo.Draw(Camera);
            _crown.Draw(Camera);
            _copyrightText.Draw(Camera);
            m_versionNumber.Draw(Camera);
            m_pressStartText2.Opacity = m_pressStartText.Opacity;
            m_pressStartText.Draw(Camera);
            m_pressStartText2.Draw(Camera);

            _rlRedux.Draw(Camera);

            if (!m_startNewLegacy)
                m_profileCardKey.Draw(Camera);
            _creditsKey.Draw(Camera);
            m_optionsKey.Draw(Camera);
            m_profileSelectKey.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            if (!m_startNewLegacy)
                m_profileCard.Draw(Camera);
            _dlcIcon.Draw(Camera);
            m_optionsIcon.Draw(Camera);
            _creditsIcon.Draw(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Camera.Draw(m_ppm.Scene, new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height), Color.White);
            Camera.Draw(_godRayTexture, new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height), Color.White);
            Camera.End();
            base.Draw(gameTime);

        }

        public override void Dispose() {

            if (!IsDisposed) {

                Console.WriteLine("Disposing Title Screen");
                _godRayTexture.Dispose();
                _godRayTexture = null;
                _bg.Dispose();
                _bg = null;
                m_logo.Dispose();
                m_logo = null;
                _castle.Dispose();
                _castle = null;
                m_smallCloud1.Dispose();
                m_smallCloud2.Dispose();
                m_smallCloud3.Dispose();
                m_smallCloud4.Dispose();
                m_smallCloud5.Dispose();
                m_smallCloud1 = null;
                m_smallCloud2 = null;
                m_smallCloud3 = null;
                m_smallCloud4 = null;
                m_smallCloud5 = null;
                _largeCloud1.Dispose();
                _largeCloud1 = null;
                _largeCloud2.Dispose();
                _largeCloud2 = null;
                _largeCloud3.Dispose();
                _largeCloud3 = null;
                _largeCloud4.Dispose();
                _largeCloud4 = null;
                m_pressStartText.Dispose();
                m_pressStartText = null;
                m_pressStartText2.Dispose();
                m_pressStartText2 = null;
                _copyrightText.Dispose();
                _copyrightText = null;
                m_versionNumber.Dispose();
                m_versionNumber = null;
                m_titleText.Dispose();
                m_titleText = null;
                m_profileCard.Dispose();
                m_profileCard = null;
                m_optionsIcon.Dispose();
                m_optionsIcon = null;
                _creditsIcon.Dispose();
                _creditsIcon = null;
                m_profileCardKey.Dispose();
                m_profileCardKey = null;
                m_optionsKey.Dispose();
                m_optionsKey = null;
                _creditsKey.Dispose();
                _creditsKey = null;
                _crown.Dispose();
                _crown = null;
                m_profileSelectKey.Dispose();
                m_profileSelectKey = null;
                _dlcIcon.Dispose();
                _dlcIcon = null;
                m_seagullCue = null;
                base.Dispose();

            }

        }

    }

}
