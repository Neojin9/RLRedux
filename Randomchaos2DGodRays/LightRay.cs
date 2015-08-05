using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Randomchaos2DGodRays {

    public class LightRay : BasePostProcess {

        public float Decay    = 0.95f;
        public float Density  = 0.5f;
        public float Exposure = 0.15f;
        public float Weight   = 1f;
        public Vector2 LighScreenSourcePos;

        public LightRay(Game game, Vector2 sourcePos, float density, float decay, float weight, float exposure) : base(game) {

            LighScreenSourcePos = sourcePos;
            Density             = density;
            Decay               = decay;
            Weight              = weight;
            Exposure            = exposure;
            UsesVertexShader    = true;

        }

        public override void Draw(GameTime gameTime) {

            if (Effect == null)
                Effect = Game.Content.Load<Effect>("Shaders/LightRays");
            
            Effect.CurrentTechnique = Effect.Techniques["LightRayFX"];
            Effect.Parameters["halfPixel"].SetValue(HalfPixel);
            Effect.Parameters["Density"].SetValue(Density);
            Effect.Parameters["Decay"].SetValue(Decay);
            Effect.Parameters["Weight"].SetValue(Weight);
            Effect.Parameters["Exposure"].SetValue(Exposure);
            Effect.Parameters["lightScreenPosition"].SetValue(LighScreenSourcePos);
            
            base.Draw(gameTime);

        }

    }

}
