using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Randomchaos2DGodRays {

    public class CrepuscularRays : BasePostProcessingEffect {

        public LightSourceMask LightSourceMask;
        public LightRay Rays;

        public CrepuscularRays(Game game, Vector2 lightScreenSourcePos, string lightSourceImage, float lightSourceSize, float density, float decay, float weight, float exposure) : base(game) {

            LightSourceMask = new LightSourceMask(game, lightScreenSourcePos, lightSourceImage, lightSourceSize);
            Rays = new LightRay(game, lightScreenSourcePos, density, decay, weight, exposure);
            AddPostProcess(LightSourceMask);
            AddPostProcess(Rays);

        }

        public Vector2 LightSource {
            get { return Rays.LighScreenSourcePos; }
            set {
                LightSourceMask.LighScreenSourcePos = value;
                Rays.LighScreenSourcePos = value;
            }
        }

        public float X {
            get { return Rays.LighScreenSourcePos.X; }
            set {
                LightSourceMask.LighScreenSourcePos.X = value;
                Rays.LighScreenSourcePos.X = value;
            }
        }

        public float Y {
            get { return Rays.LighScreenSourcePos.Y; }
            set {
                LightSourceMask.LighScreenSourcePos.Y = value;
                Rays.LighScreenSourcePos.Y = value;
            }
        }

        public Texture LightTexture {
            get { return LightSourceMask.LishsourceTexture; }
            set { LightSourceMask.LishsourceTexture = value; }
        }

        public float LightSourceSize {
            get { return LightSourceMask.LightSize; }
            set { LightSourceMask.LightSize = value; }
        }

        public float Density {
            get { return Rays.Density; }
            set { Rays.Density = value; }
        }

        public float Decay {
            get { return Rays.Decay; }
            set { Rays.Decay = value; }
        }

        public float Weight {
            get { return Rays.Weight; }
            set { Rays.Weight = value; }
        }

        public float Exposure {
            get { return Rays.Exposure; }
            set { Rays.Exposure = value; }
        }

    }

}
