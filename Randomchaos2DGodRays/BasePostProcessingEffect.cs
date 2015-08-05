using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Randomchaos2DGodRays {

    public class BasePostProcessingEffect {

        public bool Enabled = true;
        protected Game Game;
        public Vector2 HalfPixel;
        public Texture2D LastScene;
        public Texture2D OrgScene;
        protected List<BasePostProcess> PostProcesses = new List<BasePostProcess>();

        public BasePostProcessingEffect(Game game) {
            Game = game;
        }

        public void AddPostProcess(BasePostProcess postProcess) {
            PostProcesses.Add(postProcess);
        }

        public virtual void Draw(GameTime gameTime, Texture2D scene) {

            if (!Enabled)
                return;

            OrgScene = scene;
            int count = PostProcesses.Count;
            LastScene = null;

            for (int i = 0; i < count; i++) {

                if (PostProcesses[i].Enabled) {

                    PostProcesses[i].HalfPixel = HalfPixel;
                    PostProcesses[i].OrgBuffer = OrgScene;
                    
                    if (PostProcesses[i].NewScene == null)
                        PostProcesses[i].NewScene = new RenderTarget2D(Game.GraphicsDevice, Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2, false, SurfaceFormat.Color, DepthFormat.None);
                    
                    Game.GraphicsDevice.SetRenderTarget(PostProcesses[i].NewScene);
                    
                    if (LastScene == null)
                        LastScene = OrgScene;
                    
                    PostProcesses[i].BackBuffer = LastScene;
                    Game.GraphicsDevice.Textures[0] = PostProcesses[i].BackBuffer;
                    PostProcesses[i].Draw(gameTime);
                    Game.GraphicsDevice.SetRenderTarget(null);
                    LastScene = PostProcesses[i].NewScene;

                }

            }

            if (LastScene == null)
                LastScene = scene;

        }

    }

}
