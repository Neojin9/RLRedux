using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tweener;


namespace RogueCastle {
    public class DemoStartScreen : Screen {
        private TextObj m_text;
        public float BackBufferOpacity { get; set; }

        public override void LoadContent() {
            m_text = new TextObj(Game.JunicodeLargeFont);
            m_text.FontSize = 20f;
            m_text.Text = "This is a demo of Rogue Legacy.\nThere may be bugs, and some assets are missing, but we hope you enjoy it.";
            m_text.ForceDraw = true;
            m_text.Position = new Vector2(660f - (float)m_text.Width / 2f, 360f - (float)m_text.Height / 2f - 30f);
            base.LoadContent();
        }

        public override void OnEnter() {
            BackBufferOpacity = 1f;
            Tween.RunFunction(7f, base.ScreenManager, "DisplayScreen", new object[] {
                1,
                true,
                typeof(List<object>)
            });
            base.OnEnter();
        }

        public override void Draw(GameTime gametime) {
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_text.Draw(base.Camera);
            base.Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_text.Dispose();
                m_text = null;
                base.Dispose();
            }
        }
    }
}
