using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class FrameRateCounter : DrawableGameComponent {
        private ContentManager content;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private int frameCounter;
        private int frameRate;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public FrameRateCounter(Game game) : base(game) {
            content = base.Game.Content;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(base.GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("Fonts\\FpsFont");
        }

        protected override void UnloadContent() {
            content.Unload();
        }

        public override void Update(GameTime gameTime) {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromSeconds(1.0)) {
                elapsedTime -= TimeSpan.FromSeconds(1.0);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime) {
            frameCounter++;
            string text = string.Format("fps: {0}", frameRate);
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, text, new Vector2(1220f, 33f), Color.Black);
            spriteBatch.DrawString(spriteFont, text, new Vector2(1221f, 32f), Color.White);
            spriteBatch.End();
        }
    }
}
