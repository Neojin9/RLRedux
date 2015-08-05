using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_BallAndChain : EnemyObj {
        private float m_BallSpeedDivider = 1.5f;
        private float m_actualChainRadius;
        private ProjectileObj m_ballAndChain;
        private ProjectileObj m_ballAndChain2;
        private float m_ballAngle;
        private SpriteObj m_chain;
        private float m_chainLinkDistance;
        private List<Vector2> m_chainLinks2List;
        private List<Vector2> m_chainLinksList;
        private float m_chainRadius;
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private int m_numChainLinks = 10;
        private FrameSoundObj m_walkSound;
        private FrameSoundObj m_walkSound2;

        public EnemyObj_BallAndChain(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyFlailKnight_Character", target, physicsManager, levelToAttachTo, difficulty) {
            m_ballAndChain = new ProjectileObj("EnemyFlailKnightBall_Sprite");
            m_ballAndChain.IsWeighted = false;
            m_ballAndChain.CollidesWithTerrain = false;
            m_ballAndChain.IgnoreBoundsCheck = true;
            m_ballAndChain.OutlineWidth = 2;
            m_ballAndChain2 = (m_ballAndChain.Clone() as ProjectileObj);
            m_chain = new SpriteObj("EnemyFlailKnightLink_Sprite");
            m_chainLinksList = new List<Vector2>();
            m_chainLinks2List = new List<Vector2>();
            for (int i = 0; i < m_numChainLinks; i++)
                m_chainLinksList.Add(default(Vector2));
            for (int j = 0; j < m_numChainLinks / 2; j++)
                m_chainLinks2List.Add(default(Vector2));
            Type = 1;
            TintablePart = this._objectList[3];
            m_walkSound = new FrameSoundObj(this, m_target, 1, new[] {
                "KnightWalk1",
                "KnightWalk2"
            });
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, new[] {
                "KnightWalk1",
                "KnightWalk2"
            });
        }

        public float ChainSpeed { get; set; }
        public float ChainSpeed2Modifier { get; set; }

        private float ChainRadius {
            get { return m_chainRadius; }
            set { m_chainRadius = value; }
        }

        public ProjectileObj BallAndChain {
            get { return m_ballAndChain; }
        }

        public ProjectileObj BallAndChain2 {
            get { return m_ballAndChain2; }
        }

        protected override void InitializeEV() {
            ChainSpeed = 2.5f;
            ChainRadius = 260f;
            ChainSpeed2Modifier = 1.5f;
            base.Name = "Chaintor";
            MaxHealth = 40;
            base.Damage = 27;
            base.XPValue = 125;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 100f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 1020f;
            base.JumpHeight = 600f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.BallAndChain_Basic_Scale;
            base.ProjectileScale = EnemyEV.BallAndChain_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.BallAndChain_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 500;
            EngageRadius = 800;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.BallAndChain_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.ADVANCED:
                    ChainRadius = 275f;
                    base.Name = "Chaintex";
                    MaxHealth = 58;
                    base.Damage = 32;
                    base.XPValue = 150;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 150f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 1020f;
                    base.JumpHeight = 600f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.BallAndChain_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.BallAndChain_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChain_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 800;
                    ProjectileRadius = 500;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.BallAndChain_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    ChainRadius = 350f;
                    ChainSpeed2Modifier = 1.5f;
                    base.Name = "Chaintus";
                    MaxHealth = 79;
                    base.Damage = 36;
                    base.XPValue = 200;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    base.Speed = 175f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 1020f;
                    base.JumpHeight = 600f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.BallAndChain_Expert_Scale;
                    base.ProjectileScale = EnemyEV.BallAndChain_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChain_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 800;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.BallAndChain_Expert_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Pantheon";
                    MaxHealth = 300;
                    base.Damage = 60;
                    base.XPValue = 1250;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    base.Speed = 100f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 1020f;
                    base.JumpHeight = 600f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.BallAndChain_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.BallAndChain_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChain_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 800;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.BallAndChain_Miniboss_KnockBack;
                    break;
            }
            this._objectList[1].TextureColor = TintablePart.TextureColor;
            m_ballAndChain.Damage = base.Damage;
            m_ballAndChain.Scale = base.ProjectileScale;
            m_ballAndChain2.Damage = base.Damage;
            m_ballAndChain2.Scale = base.ProjectileScale;
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(1.25f, 2.75f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(1.25f, 2.75f, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(1f, 1.5f, false), Types.Sequence.Serial);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3
            });
            m_generalCooldownLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            base.SetCooldownLogicBlock(m_generalCooldownLB, new[] {
                40,
                40,
                20
            });
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        0,
                        0,
                        100
                    });
                    return;
                case 1:
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        60,
                        20,
                        20
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        0,
                        0,
                        100
                    });
                    return;
                case 1:
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        60,
                        20,
                        20
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        0,
                        0,
                        100
                    });
                    return;
                case 1:
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        60,
                        20,
                        20
                    });
                    return;
                default:
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
            if (!base.IsPaused) {
                if (!base.IsKilled && m_initialDelayCounter <= 0f) {
                    float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_actualChainRadius < ChainRadius) {
                        m_actualChainRadius += num * 200f;
                        m_chainLinkDistance = m_actualChainRadius / m_numChainLinks;
                    }
                    float num2 = 0f;
                    m_ballAndChain.Position = CDGMath.GetCirclePosition(m_ballAngle, m_actualChainRadius, new Vector2(base.X, Bounds.Top));
                    for (int i = 0; i < m_chainLinksList.Count; i++) {
                        m_chainLinksList[i] = CDGMath.GetCirclePosition(m_ballAngle, num2, new Vector2(base.X, Bounds.Top));
                        num2 += m_chainLinkDistance;
                    }
                    num2 = 0f;
                    if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
                        m_ballAndChain2.Position = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, m_actualChainRadius / 2f, new Vector2(base.X, Bounds.Top));
                    else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
                        m_ballAndChain2.Position = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -m_actualChainRadius / 2f, new Vector2(base.X, Bounds.Top));
                    for (int j = 0; j < m_chainLinks2List.Count; j++) {
                        if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
                            m_chainLinks2List[j] = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, num2, new Vector2(base.X, Bounds.Top));
                        else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
                            m_chainLinks2List[j] = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -num2, new Vector2(base.X, Bounds.Top));
                        num2 += m_chainLinkDistance;
                    }
                    m_ballAngle += ChainSpeed * 60f * num;
                    if (!base.IsAnimating && base.CurrentSpeed != 0f)
                        base.PlayAnimation(true);
                }
                if (base.SpriteName == "EnemyFlailKnight_Character") {
                    m_walkSound.Update();
                    m_walkSound2.Update();
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(Camera2D camera) {
            if (!base.IsKilled) {
                foreach (Vector2 current in m_chainLinksList) {
                    m_chain.Position = current;
                    m_chain.Draw(camera);
                }
                m_ballAndChain.Draw(camera);
                if (base.Difficulty > GameTypes.EnemyDifficulty.BASIC) {
                    foreach (Vector2 current2 in m_chainLinks2List) {
                        m_chain.Position = current2;
                        m_chain.Draw(camera);
                    }
                    m_ballAndChain2.Draw(camera);
                }
            }
            base.Draw(camera);
        }

        public override void Kill(bool giveXP = true) {
            m_levelScreen.PhysicsManager.RemoveObject(m_ballAndChain);
            EnemyObj_BouncySpike enemyObj_BouncySpike = new EnemyObj_BouncySpike(m_target, null, m_levelScreen, base.Difficulty);
            enemyObj_BouncySpike.SavedStartingPos = base.Position;
            enemyObj_BouncySpike.Position = base.Position;
            m_levelScreen.AddEnemyToCurrentRoom(enemyObj_BouncySpike);
            enemyObj_BouncySpike.Position = m_ballAndChain.Position;
            enemyObj_BouncySpike.Speed = ChainSpeed * 200f / m_BallSpeedDivider;
            enemyObj_BouncySpike.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle + 90f)));
            enemyObj_BouncySpike.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle + 90f)));
            if (base.Difficulty > GameTypes.EnemyDifficulty.BASIC) {
                m_levelScreen.PhysicsManager.RemoveObject(m_ballAndChain2);
                EnemyObj_BouncySpike enemyObj_BouncySpike2 = new EnemyObj_BouncySpike(m_target, null, m_levelScreen, base.Difficulty);
                enemyObj_BouncySpike2.SavedStartingPos = base.Position;
                enemyObj_BouncySpike2.Position = base.Position;
                m_levelScreen.AddEnemyToCurrentRoom(enemyObj_BouncySpike2);
                enemyObj_BouncySpike2.Position = m_ballAndChain2.Position;
                enemyObj_BouncySpike2.Speed = ChainSpeed * 200f * ChainSpeed2Modifier / m_BallSpeedDivider;
                if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED) {
                    enemyObj_BouncySpike2.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle * ChainSpeed2Modifier + 90f)));
                    enemyObj_BouncySpike2.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle * ChainSpeed2Modifier + 90f)));
                }
                else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT) {
                    enemyObj_BouncySpike2.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(-m_ballAngle * ChainSpeed2Modifier + 90f)));
                    enemyObj_BouncySpike2.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(-m_ballAngle * ChainSpeed2Modifier + 90f)));
                }
                enemyObj_BouncySpike2.SpawnRoom = m_levelScreen.CurrentRoom;
                enemyObj_BouncySpike2.SaveToFile = false;
                if (base.IsPaused)
                    enemyObj_BouncySpike2.PauseEnemy(false);
            }
            enemyObj_BouncySpike.SpawnRoom = m_levelScreen.CurrentRoom;
            enemyObj_BouncySpike.SaveToFile = false;
            if (base.IsPaused)
                enemyObj_BouncySpike.PauseEnemy(false);
            base.Kill(giveXP);
        }

        public override void ResetState() {
            base.ResetState();
            m_actualChainRadius = 0f;
            m_chainLinkDistance = m_actualChainRadius / m_numChainLinks;
            float num = 0f;
            m_ballAndChain.Position = CDGMath.GetCirclePosition(m_ballAngle, m_actualChainRadius, new Vector2(base.X, Bounds.Top));
            for (int i = 0; i < m_chainLinksList.Count; i++) {
                m_chainLinksList[i] = CDGMath.GetCirclePosition(m_ballAngle, num, new Vector2(base.X, Bounds.Top));
                num += m_chainLinkDistance;
            }
            num = 0f;
            if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
                m_ballAndChain2.Position = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, m_actualChainRadius / 2f, new Vector2(base.X, Bounds.Top));
            else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
                m_ballAndChain2.Position = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -m_actualChainRadius / 2f, new Vector2(base.X, Bounds.Top));
            for (int j = 0; j < m_chainLinks2List.Count; j++) {
                if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
                    m_chainLinks2List[j] = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, num, new Vector2(base.X, Bounds.Top));
                else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
                    m_chainLinks2List[j] = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -num, new Vector2(base.X, Bounds.Top));
                num += m_chainLinkDistance;
            }
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer) {
            SoundManager.Play3DSound(this, m_target, new[] {
                "Knight_Hit01",
                "Knight_Hit02",
                "Knight_Hit03"
            });
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_chain.Dispose();
                m_chain = null;
                m_ballAndChain.Dispose();
                m_ballAndChain = null;
                m_ballAndChain2.Dispose();
                m_ballAndChain2 = null;
                m_chainLinksList.Clear();
                m_chainLinksList = null;
                m_chainLinks2List.Clear();
                m_chainLinks2List = null;
                m_walkSound.Dispose();
                m_walkSound = null;
                m_walkSound2.Dispose();
                m_walkSound2 = null;
                base.Dispose();
            }
        }
    }
}
