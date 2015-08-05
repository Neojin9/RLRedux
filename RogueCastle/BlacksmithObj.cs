using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {

    public class BlacksmithObj : ObjContainer {

        private const int PartAsset1   = 0;
        private const int PartAsset2   = 1;
        private const int PartBody     = 2;
        private const int PartHead     = 3;
        private const int PartHeadtrim = 4;
        private const int PartArm      = 5;

        private float _hammerAnimCounter;
        private SpriteObj _hammerSprite;
        private SpriteObj _headSprite;

        //TODO Change String to NPC name ENUM
        public BlacksmithObj() : base("Blacksmith_Character") {
            _hammerSprite  = (_objectList[5] as SpriteObj);
            _headSprite    = (_objectList[3] as SpriteObj);
            AnimationDelay = 0.1f;
        }

        public void Update(GameTime gameTime) {

            if (_hammerAnimCounter <= 0f && !_hammerSprite.IsAnimating) {
                _hammerSprite.PlayAnimation(false);
                _hammerAnimCounter = CDGMath.RandomFloat(0.5f, 3f);
                return;
            }

            _hammerAnimCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        public override void Dispose() {

            if (!IsDisposed) {
                _hammerSprite = null;
                _headSprite = null;
                base.Dispose();
            }

        }

    }

}
