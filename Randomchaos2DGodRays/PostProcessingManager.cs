using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Randomchaos2DGodRays {

    public class PostProcessingManager {

        protected Game Game;
        public Vector2 HalfPixel;
        public Texture2D Scene;
        private readonly SpriteBatch m_spriteBatch;
        public RenderTarget2D NewScene;
        protected List<BasePostProcessingEffect> PostProcessingEffects = new List<BasePostProcessingEffect>();

        public PostProcessingManager(Game game, SpriteBatch spriteBatch) {
            Game = game;
            m_spriteBatch = spriteBatch;
        }

        public SpriteBatch SpriteBatch {
            get { return m_spriteBatch; }
        }

        public void AddEffect(BasePostProcessingEffect ppEfect) {
            PostProcessingEffects.Add(ppEfect);
        }

        public virtual void Draw(GameTime gameTime, Texture2D scene) {

            HalfPixel = -new Vector2(0.5f / Game.GraphicsDevice.Viewport.Width, 0.5f / Game.GraphicsDevice.Viewport.Height);
            int count = PostProcessingEffects.Count;
            Scene = scene;
            
            for (int i = 0; i < count; i++) {

                if (!PostProcessingEffects[i].Enabled)
                    continue;

                PostProcessingEffects[i].HalfPixel = HalfPixel;
                PostProcessingEffects[i].OrgScene = scene;
                PostProcessingEffects[i].Draw(gameTime, Scene);
                Scene = PostProcessingEffects[i].LastScene;

            }

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            SpriteBatch.Draw(Scene, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);
            SpriteBatch.End();

        }

        protected void SaveTexture(Texture2D texture, string name) {

            FileStream fileStream = new FileStream(name, FileMode.Create);
            texture.SaveAsJpeg(fileStream, texture.Width, texture.Height);
            fileStream.Close();

        }

    }

}
