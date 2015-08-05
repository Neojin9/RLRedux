using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class EnemyObj_Chicken : EnemyObj {
        private LogicBlock m_generalBasicLB = new LogicBlock();

        public EnemyObj_Chicken(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyChickenRun_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 26;
        }

        protected override void InitializeEV() {
            base.LockFlip = true;
            base.Name = "Kentucky";
            MaxHealth = 1;
            base.Damage = 8;
            base.XPValue = 100;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0f;
            base.Speed = 350f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 850f;
            base.JumpHeight = 950f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.0333333351f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = true;
            base.CanBeKnockedBack = true;
            base.IsWeighted = false;
            this.Scale = EnemyEV.Chicken_Basic_Scale;
            base.ProjectileScale = EnemyEV.Chicken_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Chicken_Basic_Tint;
            MeleeRadius = 10;
            ProjectileRadius = 20;
            EngageRadius = 30;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Chicken_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Fried";
                    MaxHealth = 1;
                    base.Damage = 11;
                    base.XPValue = 175;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 1;
                    MoneyDropChance = 0f;
                    base.Speed = 375f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 850f;
                    base.JumpHeight = 950f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Chicken_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Chicken_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Chicken_Advanced_Tint;
                    MeleeRadius = 10;
                    EngageRadius = 30;
                    ProjectileRadius = 20;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Chicken_Advanced_KnockBack;
                    goto IL_40A;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Chicken";
                    MaxHealth = 1;
                    base.Damage = 14;
                    base.XPValue = 350;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0f;
                    base.Speed = 400f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 850f;
                    base.JumpHeight = 950f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Chicken_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Chicken_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Chicken_Expert_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 30;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Chicken_Expert_KnockBack;
                    goto IL_40A;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Delicious";
                    MaxHealth = 1;
                    base.Damage = 35;
                    base.XPValue = 800;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 5;
                    MoneyDropChance = 0f;
                    base.Speed = 750f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 850f;
                    base.JumpHeight = 950f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Chicken_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Chicken_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Chicken_Miniboss_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 30;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Chicken_Miniboss_KnockBack;
                    goto IL_40A;
            }
            this.Scale = new Vector2(2f, 2f);
            IL_40A:
            base.IsWeighted = true;
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyChickenRun_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.FlipHorizontally), Types.Sequence.Serial);
            logicSet.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.5f, 1f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyChickenRun_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.None), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(0.5f, 1f, false), Types.Sequence.Serial);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        50,
                        50
                    });
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        50,
                        50
                    });
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        50,
                        50
                    });
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        50,
                        50
                    });
                    return;
            }
        }

        public void MakeCollideable() {
            base.IsCollidable = true;
        }

        public override void Update(GameTime gameTime) {
            if (m_levelScreen != null && m_levelScreen.CurrentRoom != null && !base.IsKilled && !CollisionMath.Intersects(this.TerrainBounds, m_levelScreen.CurrentRoom.Bounds))
                Kill(true);
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer) {
            SoundManager.Play3DSound(this, m_target, new[] {
                "Chicken_Cluck_01",
                "Chicken_Cluck_02",
                "Chicken_Cluck_03"
            });
            base.HitEnemy(damage, collisionPt, isPlayer);
        }
    }
}
