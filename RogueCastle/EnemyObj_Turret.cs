using System;
using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Turret : EnemyObj {
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_Turret(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyTurretFire_Character", target, physicsManager, levelToAttachTo, difficulty) {
            base.IsCollidable = false;
            base.ForceDraw = true;
            Type = 17;
            base.StopAnimation();
            base.PlayAnimationOnRestart = false;
            base.NonKillable = true;
        }

        protected override void InitializeEV() {
            this.Scale = new Vector2(2f, 2f);
            base.AnimationDelay = 0.1f;
            base.Speed = 0f;
            MaxHealth = 10;
            EngageRadius = 30;
            ProjectileRadius = 20;
            MeleeRadius = 10;
            CooldownTime = 2f;
            base.KnockBack = new Vector2(1f, 2f);
            base.Damage = 25;
            base.JumpHeight = 20.5f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            base.XPValue = 2;
            base.CanBeKnockedBack = false;
            base.LockFlip = true;
            base.IsWeighted = false;
            base.IsCollidable = false;
            base.Name = "Wall Turret";
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                case GameTypes.EnemyDifficulty.ADVANCED:
                case GameTypes.EnemyDifficulty.EXPERT:
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    return;
            }
        }

        protected override void InitializeLogic() {
            float rotation = base.Rotation;
            float num = base.ParseTagToFloat("delay");
            float num2 = base.ParseTagToFloat("speed");
            if (num == 0f) {
                Console.WriteLine("ERROR: Turret set with delay of 0. Shoots too fast.");
                num = 10000f;
            }
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "TurretProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(num2, num2),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(rotation, rotation),
                CollidesWithTerrain = true,
                Scale = base.ProjectileScale
            };
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Turret_Attack01",
                "Turret_Attack02",
                "Turret_Attack03"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(num, false), Types.Sequence.Serial);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet
            });
            m_generalAdvancedLB.AddLogicSet(new LogicSet[] {
                logicSet
            });
            m_generalExpertLB.AddLogicSet(new LogicSet[] {
                logicSet
            });
            m_generalMiniBossLB.AddLogicSet(new LogicSet[] {
                logicSet
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            RunLogicBlock(false, m_generalBasicLB, new[] {100});
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
