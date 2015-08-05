using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class BlacksmithObj : ObjContainer {
        private const int PART_ASSET1 = 0;
        private const int PART_ASSET2 = 1;
        private const int PART_BODY = 2;
        private const int PART_HEAD = 3;
        private const int PART_HEADTRIM = 4;
        private const int PART_ARM = 5;
        private float m_hammerAnimCounter;
        private SpriteObj m_hammerSprite;
        private SpriteObj m_headSprite;

        public BlacksmithObj() : base("Blacksmith_Character") {
            m_hammerSprite = (this._objectList[5] as SpriteObj);
            m_headSprite = (this._objectList[3] as SpriteObj);
            base.AnimationDelay = 0.1f;
        }

        public void Update(GameTime gameTime) {
            if (m_hammerAnimCounter <= 0f && !m_hammerSprite.IsAnimating) {
                m_hammerSprite.PlayAnimation(false);
                m_hammerAnimCounter = CDGMath.RandomFloat(0.5f, 3f);
                return;
            }
            m_hammerAnimCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_hammerSprite = null;
                m_headSprite = null;
                base.Dispose();
            }
        }
    }
}
