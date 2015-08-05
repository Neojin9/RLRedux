using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class ProjectileObj : PhysicsObj, IDealsDamageObj {
        public float LifeSpan;
        private Color m_blinkColour = Color.White;
        private float m_blinkTimer;
        private float m_elapsedLifeSpan;

        public ProjectileObj(string spriteName) : base(spriteName, null) {
            base.CollisionTypeTag = 3;
            CollidesWithTerrain = true;
            ChaseTarget = false;
            IsDying = false;
            DestroysWithEnemy = true;
            DestroyOnRoomTransition = true;
        }

        public float RotationSpeed { get; set; }
        public bool IsAlive { get; internal set; }
        public bool CollidesWithTerrain { get; set; }
        public bool DestroysWithTerrain { get; set; }
        public bool DestroysWithEnemy { get; set; }
        public GameObj Target { get; set; }
        public bool ChaseTarget { get; set; }
        public bool FollowArc { get; set; }
        public ProjectileIconObj AttachedIcon { get; set; }
        public bool ShowIcon { get; set; }
        public bool IgnoreBoundsCheck { get; set; }
        public bool CollidesWith1Ways { get; set; }
        public bool GamePaused { get; set; }
        public bool DestroyOnRoomTransition { get; set; }
        public bool CanBeFusRohDahed { get; set; }
        public bool IgnoreInvincibleCounter { get; set; }
        public bool IsDying { get; internal set; }
        public int Spell { get; set; }
        public float AltX { get; set; }
        public float AltY { get; set; }
        public float BlinkTime { get; set; }
        public GameObj Source { get; set; }
        public bool WrapProjectile { get; set; }

        public bool IsDemented {
            get {
                EnemyObj enemyObj = Source as EnemyObj;
                return enemyObj != null && enemyObj.IsDemented;
            }
        }

        #region IDealsDamageObj Members

        public int Damage { get; set; }

        #endregion

        public void Reset() {
            Source = null;
            CollidesWithTerrain = true;
            DestroysWithTerrain = true;
            DestroysWithEnemy = true;
            base.IsCollidable = true;
            base.IsWeighted = true;
            IsDying = false;
            IsAlive = true;
            m_elapsedLifeSpan = 0f;
            base.Rotation = 0f;
            base.TextureColor = Color.White;
            Spell = 0;
            AltY = 0f;
            AltX = 0f;
            BlinkTime = 0f;
            IgnoreBoundsCheck = false;
            this.Scale = Vector2.One;
            base.DisableHitboxUpdating = false;
            m_blinkColour = Color.White;
            m_blinkTimer = 0f;
            base.AccelerationYEnabled = true;
            base.AccelerationXEnabled = true;
            GamePaused = false;
            DestroyOnRoomTransition = true;
            CanBeFusRohDahed = true;
            this.Flip = SpriteEffects.None;
            IgnoreInvincibleCounter = false;
            WrapProjectile = false;
            base.DisableCollisionBoxRotations = true;
            Tween.StopAllContaining(this, false);
        }

        public void UpdateHeading() {
            if (ChaseTarget && Target != null) {
                Vector2 position = Target.Position;
                TurnToFace(position, this.TurnSpeed, 0.0166666675f);
                base.HeadingX = (float)Math.Cos((double)base.Orientation);
                base.HeadingY = (float)Math.Sin((double)base.Orientation);
            }
        }

        public void Update(GameTime gameTime) {
            if (!base.IsPaused) {
                float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
                int spell = Spell;
                switch (spell) {
                    case 3:
                        if (BlinkTime >= AltX && AltX != 0f) {
                            Blink(Color.Red, 0.1f);
                            BlinkTime = AltX / 1.5f;
                        }
                        if (AltX > 0f) {
                            AltX -= num;
                            if (AltX <= 0f)
                                ActivateEffect();
                        }
                        break;
                    case 4:
                        break;
                    case 5:
                        if (AltY > 0f) {
                            AltY -= num;
                            if (AltY <= 0f) {
                                ProceduralLevelScreen proceduralLevelScreen = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
                                if (proceduralLevelScreen != null) {
                                    proceduralLevelScreen.ImpactEffectPool.CrowSmokeEffect(base.Position);
                                    AltY = 0.05f;
                                }
                            }
                        }
                        if (AltX <= 0f) {
                            Vector2 position = Target.Position;
                            TurnToFace(position, this.TurnSpeed, num);
                        }
                        else {
                            AltX -= num;
                            base.Orientation = MathHelper.WrapAngle(base.Orientation);
                        }
                        base.HeadingX = (float)Math.Cos((double)base.Orientation);
                        base.HeadingY = (float)Math.Sin((double)base.Orientation);
                        base.AccelerationX = 0f;
                        base.AccelerationY = 0f;
                        base.Position += base.Heading * (base.CurrentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        if (base.HeadingX > 0f) {
                            this.Flip = SpriteEffects.None;
                            base.Rotation = MathHelper.ToDegrees(base.Orientation);
                        }
                        else {
                            this.Flip = SpriteEffects.FlipHorizontally;
                            float num2 = MathHelper.ToDegrees(base.Orientation);
                            if (num2 < 0f)
                                base.Rotation = (180f + num2) * 60f * num;
                            else
                                base.Rotation = (-180f + num2) * 60f * num;
                        }
                        base.Rotation = MathHelper.Clamp(base.Rotation, -90f, 90f);
                        if (Target != null) {
                            EnemyObj enemyObj = Target as EnemyObj;
                            if (enemyObj != null && enemyObj.IsKilled)
                                RunDestroyAnimation(false);
                        }
                        else
                            RunDestroyAnimation(false);
                        break;
                    default:
                        switch (spell) {
                            case 8:
                                base.AccelerationX -= AltX * 60f * num;
                                if (AltY > 0f) {
                                    AltY -= num;
                                    if (AltY <= 0f)
                                        ActivateEffect();
                                }
                                break;
                            case 9:
                            case 10:
                                break;
                            case 11: {
                                PlayerObj player = Game.ScreenManager.Player;
                                if (player.CastingDamageShield || Source is EnemyObj) {
                                    AltX += base.CurrentSpeed * 60f * num;
                                    base.Position = CDGMath.GetCirclePosition(AltX, AltY, Target.Position);
                                }
                                else
                                    KillProjectile();
                                break;
                            }
                            case 12:
                                base.AccelerationX = 0f;
                                base.AccelerationY = 0f;
                                base.HeadingX = (float)Math.Cos((double)base.Orientation);
                                base.HeadingY = (float)Math.Sin((double)base.Orientation);
                                base.Position += base.Heading * (base.CurrentSpeed * num);
                                if (AltY > 0f) {
                                    AltY -= num;
                                    if (AltY <= 0f)
                                        ActivateEffect();
                                }
                                break;
                            default:
                                if (spell == 100) {
                                    if (AltX > 0f) {
                                        AltX -= num;
                                        base.Opacity = 0.9f - AltX;
                                        this.ScaleY = 1f - AltX;
                                        if (AltX <= 0f)
                                            ActivateEffect();
                                    }
                                }
                                break;
                        }
                        break;
                }
                if (ChaseTarget && Target != null) {
                    Vector2 position2 = Target.Position;
                    TurnToFace(position2, this.TurnSpeed, num);
                    base.HeadingX = (float)Math.Cos((double)base.Orientation);
                    base.HeadingY = (float)Math.Sin((double)base.Orientation);
                    base.AccelerationX = 0f;
                    base.AccelerationY = 0f;
                    base.Position += base.Heading * (base.CurrentSpeed * num);
                    base.Rotation = MathHelper.ToDegrees(base.Orientation);
                }
                if (FollowArc && !ChaseTarget && !IsDying) {
                    float radians = (float)Math.Atan2((double)base.AccelerationY, (double)base.AccelerationX);
                    base.Rotation = MathHelper.ToDegrees(radians);
                }
                else if (!ChaseTarget)
                    base.Rotation += RotationSpeed * 60f * num;
                m_elapsedLifeSpan += num;
                if (m_elapsedLifeSpan >= LifeSpan)
                    IsAlive = false;
                if (m_blinkTimer > 0f) {
                    m_blinkTimer -= num;
                    base.TextureColor = m_blinkColour;
                    return;
                }
                if (base.TextureColor == m_blinkColour)
                    base.TextureColor = Color.White;
            }
        }

        public void Blink(Color blinkColour, float duration) {
            m_blinkColour = blinkColour;
            m_blinkTimer = duration;
        }

        private void TurnToFace(Vector2 facePosition, float turnSpeed, float elapsedSeconds) {
            float num = facePosition.X - base.Position.X;
            float num2 = facePosition.Y - base.Position.Y;
            float num3 = (float)Math.Atan2(num2, num);
            float num4 = MathHelper.WrapAngle(num3 - base.Orientation);
            float num5 = turnSpeed * 60f * elapsedSeconds;
            num4 = MathHelper.Clamp(num4, -num5, num5);
            base.Orientation = MathHelper.WrapAngle(base.Orientation + num4);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            if (Spell == 20) {
                ProjectileObj projectileObj = otherBox.AbsParent as ProjectileObj;
                if (projectileObj != null && projectileObj.CollisionTypeTag != 2 && projectileObj.CanBeFusRohDahed)
                    projectileObj.RunDestroyAnimation(false);
            }
            TerrainObj terrainObj = otherBox.Parent as TerrainObj;
            if (CollidesWithTerrain && !(otherBox.Parent is DoorObj) && terrainObj != null && ((terrainObj.CollidesTop && terrainObj.CollidesBottom && terrainObj.CollidesLeft && terrainObj.CollidesRight) || CollidesWith1Ways)) {
                int spell = Spell;
                if (spell != 3) {
                    if (spell == 7) {
                        base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                        base.IsWeighted = false;
                        ActivateEffect();
                        return;
                    }
                    if (spell != 12) {
                        if (DestroysWithTerrain) {
                            RunDestroyAnimation(false);
                            return;
                        }
                        base.AccelerationY = 0f;
                        base.AccelerationX = 0f;
                        base.IsWeighted = false;
                        return;
                    }
                    else {
                        Vector2 value = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                        if (value != Vector2.Zero) {
                            SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                                "Spike_Bounce_01",
                                "Spike_Bounce_02",
                                "Spike_Bounce_03"
                            });
                            Vector2 heading = base.Heading;
                            Vector2 vector = new Vector2(value.Y, value.X * -1f);
                            Vector2 pt = 2f * (CDGMath.DotProduct(heading, vector) / CDGMath.DotProduct(vector, vector)) * vector - heading;
                            base.X += value.X;
                            base.Y += value.Y;
                            base.Orientation = MathHelper.ToRadians(CDGMath.VectorToAngle(pt));
                            return;
                        }
                    }
                }
                else if (terrainObj.CollidesBottom && terrainObj.CollidesTop && terrainObj.CollidesLeft && terrainObj.CollidesRight) {
                    Vector2 vector2 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                    base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                    if ((vector2.Y <= 0f && vector2.X == 0f) || otherBox.AbsRotation != 0f) {
                        base.AccelerationY = 0f;
                        base.AccelerationX = 0f;
                        base.IsWeighted = false;
                        return;
                    }
                }
                else if (!terrainObj.CollidesBottom && terrainObj.CollidesTop && !terrainObj.CollidesLeft && !terrainObj.CollidesRight) {
                    Vector2 vector3 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                    if (vector3.Y <= 0f && base.AccelerationY > 0f) {
                        base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                        if ((vector3.Y <= 0f && vector3.X == 0f) || otherBox.AbsRotation != 0f) {
                            base.AccelerationY = 0f;
                            base.AccelerationX = 0f;
                            base.IsWeighted = false;
                            return;
                        }
                    }
                }
            }
            else if (otherBox.Type != 0) {
                int spell2 = Spell;
                if (spell2 == 5) {
                    if (otherBox.AbsParent == Target) {
                        base.CollisionTypeTag = 2;
                        return;
                    }
                }
                else
                    base.CollisionResponse(thisBox, otherBox, collisionResponseType);
            }
        }

        public void RunDisplacerEffect(RoomObj room, PlayerObj player) {
            int num = 2147483647;
            TerrainObj terrainObj = null;
            Vector2 value = Vector2.Zero;
            foreach (TerrainObj current in room.TerrainObjList) {
                value = Vector2.Zero;
                float num2 = 3.40282347E+38f;
                if (player.Flip == SpriteEffects.None) {
                    if (current.X > base.X && current.Bounds.Top < this.Bounds.Bottom && current.Bounds.Bottom > this.Bounds.Top) {
                        if (current.Rotation < 0f)
                            value = CollisionMath.LineToLineIntersect(base.Position, new Vector2(base.X + 6600f, base.Y), CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                        else if (current.Rotation > 0f)
                            value = CollisionMath.LineToLineIntersect(base.Position, new Vector2(base.X + 6600f, base.Y), CollisionMath.LowerLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                        if (value != Vector2.Zero)
                            num2 = value.X - base.X;
                        else
                            num2 = (float)(current.Bounds.Left - this.Bounds.Right);
                    }
                }
                else if (current.X < base.X && current.Bounds.Top < this.Bounds.Bottom && current.Bounds.Bottom > this.Bounds.Top) {
                    if (current.Rotation < 0f)
                        value = CollisionMath.LineToLineIntersect(new Vector2(base.X - 6600f, base.Y), base.Position, CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.LowerRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                    else if (current.Rotation > 0f)
                        value = CollisionMath.LineToLineIntersect(new Vector2(base.X - 6600f, base.Y), base.Position, CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                    if (value != Vector2.Zero)
                        num2 = base.X - value.X;
                    else
                        num2 = (float)(this.Bounds.Left - current.Bounds.Right);
                }
                if (num2 < num) {
                    num = (int)num2;
                    terrainObj = current;
                }
            }
            if (terrainObj != null) {
                if (player.Flip == SpriteEffects.None) {
                    if (terrainObj.Rotation == 0f) {
                        player.X += (float)num - (float)player.TerrainBounds.Width / 2f;
                        return;
                    }
                    player.X += (float)num - (float)player.Width / 2f;
                    return;
                }
                else {
                    if (terrainObj.Rotation == 0f) {
                        player.X -= (float)num - (float)player.TerrainBounds.Width / 2f;
                        return;
                    }
                    player.X -= (float)num - (float)player.Width / 2f;
                }
            }
        }

        public void RunDestroyAnimation(bool hitPlayer) {
            if (!IsDying && !IsDemented) {
                base.CurrentSpeed = 0f;
                base.AccelerationX = 0f;
                base.AccelerationY = 0f;
                IsDying = true;
                string spriteName;
                switch (spriteName = base.SpriteName) {
                    case "ArrowProjectile_Sprite":
                    case "SpellClose_Sprite":
                    case "SpellDagger_Sprite":
                        if (hitPlayer) {
                            Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                                "Rotation",
                                "270"
                            });
                            int num2 = CDGMath.RandomInt(-50, 50);
                            int num3 = CDGMath.RandomInt(-100, -50);
                            Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                                "X",
                                num2.ToString(),
                                "Y",
                                num3.ToString()
                            });
                            Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                                "Opacity",
                                "0"
                            });
                            Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
                            return;
                        }
                        base.IsWeighted = false;
                        base.IsCollidable = false;
                        Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                            "delay",
                            "0.3",
                            "Opacity",
                            "0"
                        });
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
                        return;
                    case "ShurikenProjectile1_Sprite":
                    case "BoneProjectile_Sprite":
                    case "SpellBounce_Sprite":
                    case "LastBossSwordVerticalProjectile_Sprite": {
                        Tween.StopAllContaining(this, false);
                        base.IsCollidable = false;
                        int num4 = CDGMath.RandomInt(-50, 50);
                        int num5 = CDGMath.RandomInt(-100, 100);
                        Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                            "X",
                            num4.ToString(),
                            "Y",
                            num5.ToString()
                        });
                        Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                            "Opacity",
                            "0"
                        });
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
                        return;
                    }
                    case "HomingProjectile_Sprite": {
                        ProceduralLevelScreen proceduralLevelScreen = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
                        if (proceduralLevelScreen != null)
                            proceduralLevelScreen.ImpactEffectPool.DisplayExplosionEffect(base.Position);
                        SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                            "MissileExplosion_01",
                            "MissileExplosion_02"
                        });
                        KillProjectile();
                        return;
                    }
                    case "SpellAxe_Sprite":
                    case "SpellDualBlades_Sprite":
                        base.IsCollidable = false;
                        base.AccelerationX = 0f;
                        base.AccelerationY = 0f;
                        Tween.To(this, 0.3f, new Easing(Tween.EaseNone), new[] {
                            "Opacity",
                            "0"
                        });
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
                        return;
                    case "SpellDamageShield_Sprite":
                    case "SpellDisplacer_Sprite":
                        Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new[] {
                            "Opacity",
                            "0"
                        });
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
                        return;
                    case "LastBossSwordProjectile_Sprite": {
                        base.IsCollidable = false;
                        Tween.StopAllContaining(this, false);
                        Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                            "Rotation",
                            "270"
                        });
                        int num6 = CDGMath.RandomInt(-100, -50);
                        Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                            "Y",
                            num6.ToString()
                        });
                        Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new[] {
                            "Opacity",
                            "0"
                        });
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
                        return;
                    }
                    case "SpellNuke_Sprite": {
                        ProceduralLevelScreen proceduralLevelScreen2 = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
                        if (proceduralLevelScreen2 != null)
                            proceduralLevelScreen2.ImpactEffectPool.CrowDestructionEffect(base.Position);
                        KillProjectile();
                        return;
                    }
                    case "EnemyFlailKnightBall_Sprite":
                    case "WizardIceSpell_Sprite":
                    case "WizardEarthSpell_Sprite":
                    case "SpellTimeBomb_Sprite":
                    case "SpellLaser_Sprite":
                    case "SpellBoomerang_Sprite":
                        KillProjectile();
                        return;
                }
                if (base.SpriteName == "WizardIceProjectile_Sprite") {
                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                        "Ice_Wizard_Break_01",
                        "Ice_Wizard_Break_02",
                        "Ice_Wizard_Break_03"
                    });
                }
                string text = base.SpriteName.Replace("_", "Explosion_");
                this.ChangeSprite(text);
                base.AnimationDelay = 0.0333333351f;
                base.PlayAnimation(false);
                base.IsWeighted = false;
                base.IsCollidable = false;
                if (text != "EnemySpearKnightWaveExplosion_Sprite" && text != "WizardIceProjectileExplosion_Sprite")
                    base.Rotation = 0f;
                Tween.RunFunction(0.5f, this, "KillProjectile", new object[0]);
            }
        }

        public void ActivateEffect() {
            int spell = Spell;
            switch (spell) {
                case 3:
                    base.IsWeighted = false;
                    this.ChangeSprite("SpellTimeBombExplosion_Sprite");
                    base.PlayAnimation(false);
                    IsDying = true;
                    base.CollisionTypeTag = 10;
                    base.AnimationDelay = 0.0333333351f;
                    this.Scale = new Vector2(4f, 4f);
                    Tween.RunFunction(0.5f, this, "KillProjectile", new object[0]);
                    (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).ImpactEffectPool.DisplayExplosionEffect(base.Position);
                    return;
                case 4:
                case 6:
                    return;
                case 5:
                    RunDestroyAnimation(false);
                    (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).DamageAllEnemies(Damage);
                    (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).ImpactEffectPool.DisplayDeathEffect(base.Position);
                    return;
                case 7: {
                    RunDestroyAnimation(false);
                    PlayerObj player = (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).Player;
                    player.Translocate(base.Position);
                    return;
                }
                case 8:
                    break;
                default:
                    if (spell != 12) {
                        if (spell != 100)
                            return;
                        base.CollisionTypeTag = 10;
                        LifeSpan = AltY;
                        m_elapsedLifeSpan = 0f;
                        base.IsCollidable = true;
                        base.Opacity = 1f;
                        return;
                    }
                    break;
            }
            base.CollisionTypeTag = 10;
        }

        public void KillProjectile() {
            IsAlive = false;
            IsDying = false;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                Target = null;
                AttachedIcon = null;
                Source = null;
                base.Dispose();
            }
        }

        protected override GameObj CreateCloneInstance() {
            return new ProjectileObj(this._spriteName);
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            ProjectileObj projectileObj = obj as ProjectileObj;
            projectileObj.Target = Target;
            projectileObj.CollidesWithTerrain = CollidesWithTerrain;
            projectileObj.ChaseTarget = ChaseTarget;
            projectileObj.FollowArc = FollowArc;
            projectileObj.ShowIcon = ShowIcon;
            projectileObj.DestroysWithTerrain = DestroysWithTerrain;
            projectileObj.AltX = AltX;
            projectileObj.AltY = AltY;
            projectileObj.Spell = Spell;
            projectileObj.IgnoreBoundsCheck = IgnoreBoundsCheck;
            projectileObj.DestroysWithEnemy = DestroysWithEnemy;
            projectileObj.DestroyOnRoomTransition = DestroyOnRoomTransition;
            projectileObj.Source = Source;
            projectileObj.CanBeFusRohDahed = CanBeFusRohDahed;
            projectileObj.WrapProjectile = WrapProjectile;
            projectileObj.IgnoreInvincibleCounter = IgnoreInvincibleCounter;
        }
    }
}
