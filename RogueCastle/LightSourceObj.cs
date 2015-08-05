namespace RogueCastle {
    public class LightSourceObj : SpriteObj {
        private float m_growthDifference;
        private float m_growthRate;

        public LightSourceObj() : base("LightSource_Sprite") {
            m_growthRate = 0.7f + CDGMath.RandomFloat(-0.1f, 0.1f);
            m_growthDifference = 0.05f + CDGMath.RandomFloat(0f, 0.05f);
            base.Opacity = 1f;
        }

        public override void Draw(Camera2D camera) {
            base.Draw(camera);
        }

        protected override GameObj CreateCloneInstance() {
            return new LightSourceObj();
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
        }
    }
}
