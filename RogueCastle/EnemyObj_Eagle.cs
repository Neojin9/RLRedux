using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Eagle : EnemyObj {
        private LogicBlock m_basicAttackLB = new LogicBlock();
        private bool m_flyingLeft;
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        public EnemyObj_Eagle(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("Dummy_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 4;
        }

        protected override void InitializeEV() {
            MaxHealth = 10;
            base.Damage = 10;
            base.XPValue = 5;
            HealthGainPerLevel = 2;
            DamageGainPerLevel = 2;
            XPBonusPerLevel = 1;
            base.IsWeighted = false;
            base.Speed = 6f;
            EngageRadius = 1900;
            ProjectileRadius = 1600;
            MeleeRadius = 650;
            CooldownTime = 2f;
            base.CanBeKnockedBack = false;
            base.JumpHeight = 20.5f;
            CanFallOffLedges = true;
            this.TurnSpeed = 0.0175f;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                case GameTypes.EnemyDifficulty.ADVANCED:
                case GameTypes.EnemyDifficulty.EXPERT:
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    return;
            }
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f), -1f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
            m_basicAttackLB.AddLogicSet(new LogicSet[] {
                logicSet2,
                logicSet
            });
            logicBlocksToDispose.Add(m_basicAttackLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (m_flyingLeft) {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicAttackLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
                    }
                    else {
                        base.RunLogicBlock(false, m_basicAttackLB, new[] {
                            0,
                            100
                        });
                    }
                    if (base.X > (float)m_levelScreen.CurrentRoom.Bounds.Right) {
                        base.Y = m_levelScreen.CurrentRoom.Y + (float)CDGMath.RandomInt(100, m_levelScreen.CurrentRoom.Height - 100);
                        m_flyingLeft = true;
                        return;
                    }
                    if (base.X < (float)m_levelScreen.CurrentRoom.Bounds.Left) {
                        base.Y = m_levelScreen.CurrentRoom.Y + (float)CDGMath.RandomInt(100, m_levelScreen.CurrentRoom.Height - 100);
                        m_flyingLeft = false;
                    }
                    return;
                default:
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

        public override void Update(GameTime gameTime) {
            if (!base.IsAnimating)
                base.PlayAnimation(true);
            base.Update(gameTime);
        }
    }
}
