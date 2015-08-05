using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Randomchaos2DGodRays {

    public class LightSourceMask : BasePostProcess {

        public Vector2 LighScreenSourcePos;
        public float LightSize = 1500f;
        public string LightSourceasset;
        public Texture LishsourceTexture;

        public LightSourceMask(Game game, Vector2 sourcePos, string lightSourceasset, float lightSize) : base(game) {

            UsesVertexShader = true;
            LighScreenSourcePos = sourcePos;
            LightSourceasset = lightSourceasset;
            LightSize = lightSize;

        }

        public override void Draw(GameTime gameTime) {

            if (Effect == null) {
                Effect = Game.Content.Load<Effect>("Shaders/LightSourceMask");
                LishsourceTexture = Game.Content.Load<Texture2D>(LightSourceasset);
            }
            
            Effect.Parameters["screenRes"].SetValue(new Vector2(16f, 9f));
            Effect.Parameters["halfPixel"].SetValue(HalfPixel);
            Effect.CurrentTechnique = Effect.Techniques["LightSourceMask"];
            Effect.Parameters["flare"].SetValue(LishsourceTexture);
            Effect.Parameters["SunSize"].SetValue(LightSize);
            Effect.Parameters["lightScreenPosition"].SetValue(LighScreenSourcePos);
            
            base.Draw(gameTime);

        }

    }

}
