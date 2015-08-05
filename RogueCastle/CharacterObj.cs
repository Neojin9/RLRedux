using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public abstract class CharacterObj : PhysicsObjContainer, IStateObj, IKillableObj {
        protected float CurrentAirSpeed;
        public SpriteEffects InternalFlip;
        public float SlopeClimbRotation = 45f;
        protected int StepUp;
        private Color m_blinkColour = Color.White;
        protected float m_blinkTimer;
        private int m_currentHealth;
        protected float m_internalAnimationDelay = 0.1f;
        protected bool m_internalIsWeighted = true;
        protected bool m_internalLockFlip;
        protected float m_internalRotation;
        protected Vector2 m_internalScale = new Vector2(1f, 1f);
        protected bool m_isKilled;
        protected bool m_isTouchingGround;
        protected ProceduralLevelScreen m_levelScreen;

        public CharacterObj(string spriteName, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo) : base(spriteName, physicsManager) {
            m_levelScreen = levelToAttachTo;
            CanBeKnockedBack = true;
        }

        public float JumpHeight { get; set; }
        public virtual int MaxHealth { get; internal set; }
        public Vector2 KnockBack { get; internal set; }
        public float DoubleJumpHeight { get; internal set; }
        public bool CanBeKnockedBack { get; set; }
        public int State { get; set; }

        public int CurrentHealth {
            get { return m_currentHealth; }
            set {
                m_currentHealth = value;
                if (m_currentHealth > MaxHealth)
                    m_currentHealth = MaxHealth;
                if (m_currentHealth < 0)
                    m_currentHealth = 0;
            }
        }

        public bool IsKilled {
            get { return m_isKilled; }
        }

        public bool IsTouchingGround {
            get { return m_isTouchingGround; }
        }

        public Vector2 InternalScale {
            get { return m_internalScale; }
        }

        protected abstract void InitializeEV();

        protected abstract void InitializeLogic();

        public virtual void HandleInput() {}

        public virtual void Update(GameTime gameTime) {
            if (m_blinkTimer > 0f) {
                m_blinkTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                base.TextureColor = m_blinkColour;
                return;
            }
            if (base.TextureColor == m_blinkColour)
                base.TextureColor = Color.White;
        }

        public void Blink(Color blinkColour, float duration) {
            m_blinkColour = blinkColour;
            m_blinkTimer = duration;
        }

        public virtual void Kill(bool giveXP = true) {
            base.AccelerationX = 0f;
            base.AccelerationY = 0f;
            base.Opacity = 1f;
            base.CurrentSpeed = 0f;
            base.StopAnimation();
            base.Visible = false;
            m_isKilled = true;
            base.IsCollidable = false;
            base.IsWeighted = false;
            m_blinkTimer = 0f;
        }

        public virtual void Reset() {
            base.AccelerationX = 0f;
            base.AccelerationY = 0f;
            base.CurrentSpeed = 0f;
            CurrentHealth = MaxHealth;
            base.Opacity = 1f;
            base.IsCollidable = true;
            base.IsWeighted = m_internalIsWeighted;
            base.LockFlip = m_internalLockFlip;
            base.Rotation = m_internalRotation;
            base.AnimationDelay = m_internalAnimationDelay;
            this.Scale = m_internalScale;
            this.Flip = InternalFlip;
            m_isKilled = false;
            base.Visible = true;
            base.IsTriggered = false;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_levelScreen = null;
                base.Dispose();
            }
        }
    }
}
