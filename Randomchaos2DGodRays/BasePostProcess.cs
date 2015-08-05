using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Randomchaos2DGodRays {

    public class BasePostProcess {

        public Texture2D BackBuffer;
        public bool Enabled = true;
        protected Game Game;
        public Vector2 HalfPixel;
        public bool UsesVertexShader;
        protected Effect Effect;
        public RenderTarget2D NewScene;
        public Texture2D OrgBuffer;
        private ScreenQuad _screenQuad;

        public BasePostProcess(Game game) {
            Game = game;
        }

        protected SpriteBatch spriteBatch {
            get { return (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); }
        }

        public virtual void Draw(GameTime gameTime) {

            if (Enabled) {

                if (_screenQuad == null) {
                    _screenQuad = new ScreenQuad(Game);
                    _screenQuad.Initialize();
                }

                Effect.CurrentTechnique.Passes[0].Apply();
                _screenQuad.Draw();

            }

        }

    }

}
