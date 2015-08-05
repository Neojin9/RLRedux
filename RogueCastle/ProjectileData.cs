using System;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class ProjectileData : IDisposable {
        public Vector2 Angle;
        public float AngleOffset;
        public bool CanBeFusRohDahed = true;
        public bool ChaseTarget;
        public bool CollidesWith1Ways;
        public bool CollidesWithTerrain = true;
        public int Damage;
        public bool DestroyOnRoomTransition = true;
        public bool DestroysWithEnemy = true;
        public bool DestroysWithTerrain = true;
        public bool FollowArc;
        public bool IgnoreInvincibleCounter;
        public bool IsCollidable = true;
        public bool IsWeighted;
        public float Lifespan = 10f;
        public bool LockPosition;
        public float RotationSpeed;
        public Vector2 Scale = new Vector2(1f, 1f);
        public bool ShowIcon = true;
        public GameObj Source;
        public Vector2 SourceAnchor;
        public Vector2 Speed;
        public string SpriteName;
        public float StartingRotation;
        public GameObj Target;
        public float TurnSpeed = 10f;
        public bool WrapProjectile;
        private bool m_isDisposed;

        public ProjectileData(GameObj source) {
            if (source == null)
                throw new Exception("Cannot create a projectile without a source");
            Source = source;
        }

        public bool IsDisposed {
            get { return m_isDisposed; }
        }

        #region IDisposable Members

        public void Dispose() {
            if (!IsDisposed) {
                Source = null;
                Target = null;
                m_isDisposed = true;
            }
        }

        #endregion

        public ProjectileData Clone() {
            return new ProjectileData(Source) {
                IsWeighted = IsWeighted,
                SpriteName = SpriteName,
                Source = Source,
                Target = Target,
                SourceAnchor = SourceAnchor,
                AngleOffset = AngleOffset,
                Angle = Angle,
                RotationSpeed = RotationSpeed,
                Damage = Damage,
                Speed = Speed,
                Scale = Scale,
                CollidesWithTerrain = CollidesWithTerrain,
                Lifespan = Lifespan,
                ChaseTarget = ChaseTarget,
                TurnSpeed = TurnSpeed,
                FollowArc = FollowArc,
                StartingRotation = StartingRotation,
                ShowIcon = ShowIcon,
                DestroysWithTerrain = DestroysWithTerrain,
                IsCollidable = IsCollidable,
                DestroysWithEnemy = DestroysWithEnemy,
                LockPosition = LockPosition,
                CollidesWith1Ways = CollidesWith1Ways,
                DestroyOnRoomTransition = DestroyOnRoomTransition,
                CanBeFusRohDahed = CanBeFusRohDahed,
                IgnoreInvincibleCounter = IgnoreInvincibleCounter,
                WrapProjectile = WrapProjectile
            };
        }
    }
}
