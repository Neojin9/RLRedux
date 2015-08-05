using System;
using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class HoverObj : SpriteObj {
        public float Amplitude = 1f;
        public float HoverSpeed = 1f;
        private Vector2 m_startPos;

        public HoverObj(string spriteName) : base(spriteName) {}

        public void SetStartingPos(Vector2 pos) {
            m_startPos = pos;
        }

        public override void Draw(Camera2D camera) {
            base.Y = m_startPos.Y + (float)Math.Sin((Game.TotalGameTime * HoverSpeed)) * Amplitude;
            base.Draw(camera);
        }

        protected override GameObj CreateCloneInstance() {
            return new HoverObj(base.SpriteName);
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            HoverObj hoverObj = obj as HoverObj;
            hoverObj.HoverSpeed = HoverSpeed;
            hoverObj.Amplitude = Amplitude;
            hoverObj.SetStartingPos(m_startPos);
        }
    }
}
