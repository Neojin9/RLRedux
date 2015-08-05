using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class GroundCheckLogicAction : LogicAction {
        private CharacterObj m_obj;

        public override void Execute() {
            if (this.ParentLogicSet != null && this.ParentLogicSet.IsActive) {
                m_obj = (this.ParentLogicSet.ParentGameObj as CharacterObj);
                this.SequenceType = Types.Sequence.Serial;
                base.Execute();
            }
        }

        public override void Update(GameTime gameTime) {
            ExecuteNext();
            base.Update(gameTime);
        }

        public override void ExecuteNext() {
            if (m_obj.IsTouchingGround)
                base.ExecuteNext();
        }

        public override object Clone() {
            return new GroundCheckLogicAction();
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_obj = null;
                base.Dispose();
            }
        }
    }
}
