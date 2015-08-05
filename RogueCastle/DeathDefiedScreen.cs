using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class DeathDefiedScreen : Screen {
        private Vector2 m_cameraPos;
        private PlayerObj m_player;
        private string m_songName;
        private SpriteObj m_spotlight;
        private float m_storedMusicVolume;
        private SpriteObj m_title;
        private SpriteObj m_titlePlate;
        public float BackBufferOpacity { get; set; }

        public override void LoadContent() {
            new Vector2(2f, 2f);
            new Color(255, 254, 128);
            m_spotlight = new SpriteObj("GameOverSpotlight_Sprite");
            m_spotlight.Rotation = 90f;
            m_spotlight.ForceDraw = true;
            m_spotlight.Position = new Vector2(660f, (40 + m_spotlight.Height));
            m_titlePlate = new SpriteObj("SkillUnlockTitlePlate_Sprite");
            m_titlePlate.Position = new Vector2(660f, 160f);
            m_titlePlate.ForceDraw = true;
            m_title = new SpriteObj("DeathDefyText_Sprite");
            m_title.Position = m_titlePlate.Position;
            m_title.Y -= 40f;
            m_title.ForceDraw = true;
            base.LoadContent();
        }

        public override void OnEnter() {
            m_cameraPos = new Vector2(base.Camera.X, base.Camera.Y);
            if (m_player == null)
                m_player = (base.ScreenManager as RCScreenManager).Player;
            m_titlePlate.Scale = Vector2.Zero;
            m_title.Scale = Vector2.Zero;
            m_title.Opacity = 1f;
            m_titlePlate.Opacity = 1f;
            m_storedMusicVolume = SoundManager.GlobalMusicVolume;
            m_songName = SoundManager.GetCurrentMusicName();
            Tween.To(typeof(SoundManager), 1f, new Easing(Tween.EaseNone), new[] {
                "GlobalMusicVolume",
                (m_storedMusicVolume * 0.1f).ToString()
            });
            SoundManager.PlaySound("Player_Death_FadeToBlack");
            m_player.Visible = true;
            m_player.Opacity = 1f;
            m_spotlight.Opacity = 0f;
            Tween.To(this, 0.5f, new Easing(Linear.EaseNone), new[] {
                "BackBufferOpacity",
                "1"
            });
            Tween.To(m_spotlight, 0.1f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "1",
                "Opacity",
                "1"
            });
            Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", new object[] {
                "Player_Death_Spotlight"
            });
            Tween.To(base.Camera, 1f, new Easing(Quad.EaseInOut), new[] {
                "X",
                m_player.AbsX.ToString(),
                "Y",
                (m_player.Bounds.Bottom - 10).ToString(),
                "Zoom",
                "1"
            });
            Tween.RunFunction(2f, this, "PlayerLevelUpAnimation", new object[0]);
            base.OnEnter();
        }

        public void PlayerLevelUpAnimation() {
            m_player.ChangeSprite("PlayerLevelUp_Character");
            m_player.PlayAnimation(false);
            Tween.To(m_titlePlate, 0.5f, new Easing(Back.EaseOut), new[] {
                "ScaleX",
                "1",
                "ScaleY",
                "1"
            });
            Tween.To(m_title, 0.5f, new Easing(Back.EaseOut), new[] {
                "delay",
                "0.1",
                "ScaleX",
                "0.8",
                "ScaleY",
                "0.8"
            });
            Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", new object[] {
                "GetItemStinger3"
            });
            Tween.RunFunction(2f, this, "ExitTransition", new object[0]);
        }

        public void ExitTransition() {
            Tween.To(typeof(SoundManager), 1f, new Easing(Tween.EaseNone), new[] {
                "GlobalMusicVolume",
                m_storedMusicVolume.ToString()
            });
            Tween.To(base.Camera, 1f, new Easing(Quad.EaseInOut), new[] {
                "X",
                m_cameraPos.X.ToString(),
                "Y",
                m_cameraPos.Y.ToString()
            });
            Tween.To(m_spotlight, 0.5f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "0"
            });
            Tween.To(m_titlePlate, 0.5f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "0"
            });
            Tween.To(m_title, 0.5f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "0"
            });
            Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "1",
                "BackBufferOpacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(base.ScreenManager, "HideCurrentScreen", new object[0]);
        }

        public override void Draw(GameTime gameTime) {
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, base.Camera.GetTransformation());
            base.Camera.Draw(Game.GenericTexture, new Rectangle((int)base.Camera.TopLeftCorner.X - 10, (int)base.Camera.TopLeftCorner.Y - 10, 1340, 740), Color.Black * BackBufferOpacity);
            m_player.Draw(base.Camera);
            base.Camera.End();
            base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            m_spotlight.Draw(base.Camera);
            m_titlePlate.Draw(base.Camera);
            m_title.Draw(base.Camera);
            base.Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                Console.WriteLine("Disposing Death Defied Screen");
                m_player = null;
                m_spotlight.Dispose();
                m_spotlight = null;
                m_title.Dispose();
                m_title = null;
                m_titlePlate.Dispose();
                m_titlePlate = null;
                base.Dispose();
            }
        }
    }
}
