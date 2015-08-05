namespace RogueCastle {
    public class FireProjectileLogicAction : LogicAction {
        private ProjectileData m_data;
        private ProjectileManager m_projectileManager;

        public FireProjectileLogicAction(ProjectileManager projectileManager, ProjectileData data) {
            m_projectileManager = projectileManager;
            m_data = data.Clone();
        }

        public override void Execute() {
            if (this.ParentLogicSet != null && this.ParentLogicSet.IsActive) {
                GameObj arg_20_0 = this.ParentLogicSet.ParentGameObj;
                m_projectileManager.FireProjectile(m_data);
                base.Execute();
            }
        }

        public override object Clone() {
            return new FireProjectileLogicAction(m_projectileManager, m_data);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_projectileManager = null;
                m_data = null;
                base.Dispose();
            }
        }
    }
}
