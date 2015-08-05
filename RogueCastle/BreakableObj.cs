using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {

    public class BreakableObj : PhysicsObjContainer {

        private bool _internalIsWeighted;

        public BreakableObj(string spriteName) : base(spriteName, null) {

            DisableCollisionBoxRotations = true;
            Broken = false;
            OutlineWidth = 2;
            SameTypesCollide = true;
            CollisionTypeTag = 5;
            CollidesLeft = false;
            CollidesRight = false;
            CollidesBottom = false;
            
            for (int index = 0; index < _objectList.Count; index++)
                _objectList[index].Visible = false;
            
            _objectList[0].Visible = true;
            DropItem = true;

        }

        public bool Broken          { get; internal set; }
        public bool DropItem        { get; set; }
        public bool HitBySpellsOnly { get; set; }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            
            PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
            
            if (playerObj != null && otherBox.Type == 1 && !HitBySpellsOnly && !Broken)
                Break();
            
            ProjectileObj projectileObj = otherBox.AbsParent as ProjectileObj;
            
            if (projectileObj != null && (projectileObj.CollisionTypeTag == 2 || projectileObj.CollisionTypeTag == 10) && otherBox.Type == 1) {
                
                if (!Broken)
                    Break();
                
                if (projectileObj.DestroysWithTerrain && SpriteName == "Target1_Character")
                    projectileObj.RunDestroyAnimation(false);

            }

            if ((otherBox.AbsRect.Y > thisBox.AbsRect.Y || otherBox.AbsRotation != 0f) && (otherBox.Parent is TerrainObj || otherBox.AbsParent is BreakableObj))
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);

        }

        public void Break() {

            PlayerObj player = Game.ScreenManager.Player;

            for (int index = 0; index < _objectList.Count; index++)
                _objectList[index].Visible = true;

            GoToFrame(2);
            Broken = true;
            _internalIsWeighted = IsWeighted;
            IsWeighted = false;
            IsCollidable = false;

            if (DropItem) {

                bool flag = false;
                
                if (Name == "Health") {
                    player.AttachedLevel.ItemDropManager.DropItem(Position, 2, 0.1f);
                    flag = true;
                }
                else if (Name == "Mana") {
                    player.AttachedLevel.ItemDropManager.DropItem(Position, 3, 0.1f);
                    flag = true;
                }

                if (flag) {

                    for (int i = 0; i < NumChildren; i++) {
                        
                        Tween.By(GetChildAt(i), 0.3f, Linear.EaseNone, new[] {
                            "X",
                            CDGMath.RandomInt(-50, 50).ToString(),
                            "Y",
                            "50",
                            "Rotation",
                            CDGMath.RandomInt(-360, 360).ToString()
                        });

                        Tween.To(GetChildAt(i), 0.1f, Linear.EaseNone, new[] {
                            "delay",
                            "0.2",
                            "Opacity",
                            "0"
                        });

                    }

                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                        "EnemyHit1",
                        "EnemyHit2",
                        "EnemyHit3",
                        "EnemyHit4",
                        "EnemyHit5",
                        "EnemyHit6"
                    });

                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                        "Break1",
                        "Break2",
                        "Break3"
                    });

                    if (Game.PlayerStats.Traits.X == 15f || Game.PlayerStats.Traits.Y == 15f) {
                        player.CurrentMana += 1f;
                        player.AttachedLevel.TextManager.DisplayNumberStringText(1, "mp", Color.RoyalBlue, new Vector2(player.X, (player.Bounds.Top - 30)));
                    }

                    return;

                }

                int num = CDGMath.RandomInt(1, 100);
                int num2 = 0;
                int j = 0;
                
                while (j < GameEV.BREAKABLE_ITEMDROP_CHANCE.Length) {
                    
                    num2 += GameEV.BREAKABLE_ITEMDROP_CHANCE[j];
                    
                    if (num <= num2) {
                        
                        if (j == 0) {

                            if (Game.PlayerStats.Traits.X != 24f && Game.PlayerStats.Traits.Y != 24f) {
                                player.AttachedLevel.ItemDropManager.DropItem(Position, 2, 0.1f);
                                break;
                            }

                            EnemyObj_Chicken enemyObj_Chicken = new EnemyObj_Chicken(null, null, null, GameTypes.EnemyDifficulty.BASIC);
                            enemyObj_Chicken.AccelerationY = -500f;
                            enemyObj_Chicken.Position = Position;
                            enemyObj_Chicken.Y -= 50f;
                            enemyObj_Chicken.SaveToFile = false;
                            
                            player.AttachedLevel.AddEnemyToCurrentRoom(enemyObj_Chicken);
                            enemyObj_Chicken.IsCollidable = false;
                            Tween.RunFunction(0.2f, enemyObj_Chicken, "MakeCollideable", new object[0]);
                            
                            SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                                "Chicken_Cluck_01",
                                "Chicken_Cluck_02",
                                "Chicken_Cluck_03"
                            });

                            break;

                        }

                        if (j == 1) {
                            player.AttachedLevel.ItemDropManager.DropItem(Position, 3, 0.1f);
                            break;
                        }

                        if (j == 2) {
                            player.AttachedLevel.ItemDropManager.DropItem(Position, 1, 10f);
                            break;
                        }

                        if (j == 3) {
                            player.AttachedLevel.ItemDropManager.DropItem(Position, 10, 100f);
                        }

                        break;

                    }

                    j++;

                }

            }

            for (int k = 0; k < NumChildren; k++) {
                
                Tween.By(GetChildAt(k), 0.3f, Linear.EaseNone, new[] {
                    "X",
                    CDGMath.RandomInt(-50, 50).ToString(),
                    "Y",
                    "50",
                    "Rotation",
                    CDGMath.RandomInt(-360, 360).ToString()
                });

                Tween.To(GetChildAt(k), 0.1f, Linear.EaseNone, new[] {
                    "delay",
                    "0.2",
                    "Opacity",
                    "0"
                });

            }

            SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                "EnemyHit1",
                "EnemyHit2",
                "EnemyHit3",
                "EnemyHit4",
                "EnemyHit5",
                "EnemyHit6"
            });

            SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                "Break1",
                "Break2",
                "Break3"
            });

            if (Game.PlayerStats.Traits.X == 15f || Game.PlayerStats.Traits.Y == 15f) {
                player.CurrentMana += 1f;
                player.AttachedLevel.TextManager.DisplayNumberStringText(1, "mp", Color.RoyalBlue, new Vector2(player.X, (player.Bounds.Top - 30)));
            }

        }

        public void ForceBreak() {
            
            for (int index = 0; index < _objectList.Count; index++) {
                GameObj current = _objectList[index];
                current.Visible = true;
                current.Opacity = 0f;
            }

            GoToFrame(2);
            Broken = true;
            _internalIsWeighted = IsWeighted;
            IsWeighted = false;
            IsCollidable = false;

        }

        public void Reset() {
            
            GoToFrame(1);
            Broken = false;
            IsWeighted = _internalIsWeighted;
            IsCollidable = true;
            ChangeSprite(_spriteName);
            
            for (int i = 0; i < NumChildren; i++) {
                GetChildAt(i).Opacity = 1f;
                GetChildAt(i).Rotation = 0f;
            }

            for (int index = 0; index < _objectList.Count; index++)
                _objectList[index].Visible = false;
            
            _objectList[0].Visible = true;

        }

        public void UpdateTerrainBox() {
            
            for (int index = 0; index < CollisionBoxes.Count; index++) {
                
                CollisionBox current = CollisionBoxes[index];
                
                if (current.Type == 0) {
                    m_terrainBounds = current.AbsRect;
                    break;
                }

            }

        }

        protected override GameObj CreateCloneInstance() {
            return new BreakableObj(_spriteName);
        }

        protected override void FillCloneInstance(object obj) {

            base.FillCloneInstance(obj);
            BreakableObj breakableObj = obj as BreakableObj;
            breakableObj.HitBySpellsOnly = HitBySpellsOnly;
            breakableObj.DropItem = DropItem;

        }

    }

}
