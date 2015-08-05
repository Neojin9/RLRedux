using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tweener;


namespace RogueCastle {
    public class DemoEndScreen : Screen {
        private SpriteObj m_playerShrug;
        private TextObj m_text;
        public float BackBufferOpacity { get; set; }

        public override void LoadContent() {
            m_text = new TextObj(Game.JunicodeLargeFont);
            m_text.FontSize = 20f;
            m_text.Text = "Thanks for playing the Rogue Legacy Demo. You're pretty good at games.";
            m_text.ForceDraw = true;
            m_text.Position = new Vector2(660f - (float)m_text.Width / 2f, 360f - (float)m_text.Height / 2f - 30f);
            m_playerShrug = new SpriteObj("PlayerShrug_Sprite");
            m_playerShrug.ForceDraw = true;
            m_playerShrug.Position = new Vector2(660f, (m_text.Bounds.Bottom + 100));
            m_playerShrug.Scale = new Vector2(3f, 3f);
            base.LoadContent();
        }

        public override void OnEnter() {
            BackBufferOpacity = 1f;
            Tween.RunFunction(8f, base.ScreenManager, "DisplayScreen", new object[] {
                3,
                true,
                typeof(List<object>)
            });
            base.OnEnter();
        }

        public override void Draw(GameTime gametime) {
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            m_playerShrug.Draw(base.Camera);
            base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_text.Draw(base.Camera);
            base.Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_playerShrug.Dispose();
                m_playerShrug = null;
                m_text.Dispose();
                m_text = null;
                base.Dispose();
            }
        }
    }
}
