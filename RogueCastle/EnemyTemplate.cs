using DS2DEngine;


namespace RogueCastle {
    public class EnemyTemplate : EnemyObj {
        public EnemyTemplate(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemySpriteNameGoesHere", target, physicsManager, levelToAttachTo, difficulty) {}

        protected override void InitializeEV() {
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                case GameTypes.EnemyDifficulty.ADVANCED:
                case GameTypes.EnemyDifficulty.EXPERT:
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    return;
            }
        }

        protected override void InitializeLogic() {
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }
    }
}
