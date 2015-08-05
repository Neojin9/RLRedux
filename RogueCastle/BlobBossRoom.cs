using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {

    public class BlobBossRoom : BossRoomObj {

        private List<ObjContainer> _blobArray;
        private EnemyObj_Blob _boss1;
        private float _desiredBossScale;
        private const int NumIntroBlobs = 10;

        public override bool BossKilled {
            get { return ActiveEnemies == 0; }
        }

        public int NumActiveBlobs {
            
            get {
                
                int num = 0;

                for (int index = 0; index < EnemyList.Count; index++) {
                    EnemyObj current = EnemyList[index];
                    if (current.Type == 2 && !current.IsKilled)
                        num++;
                }

                for (int index = 0; index < TempEnemyList.Count; index++) {
                    EnemyObj current2 = TempEnemyList[index];
                    if (current2.Type == 2 && !current2.IsKilled)
                        num++;
                }

                return num;

            }

        }

        public override void Initialize() {
            
            _boss1 = (EnemyList[0] as EnemyObj_Blob);
            _boss1.PauseEnemy(true);
            _boss1.DisableAllWeight = false;
            
            _desiredBossScale = _boss1.Scale.X;
            _blobArray = new List<ObjContainer>();
            
            for (int i = 0; i < NumIntroBlobs; i++) {

                ObjContainer objContainer = new ObjContainer("EnemyBlobBossAir_Character");
                objContainer.Position = _boss1.Position;
                objContainer.Scale = new Vector2(0.4f, 0.4f);
                objContainer.GetChildAt(0).TextureColor = Color.White;
                objContainer.GetChildAt(2).TextureColor = Color.LightSkyBlue;
                objContainer.GetChildAt(2).Opacity = 0.8f;
                (objContainer.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
                objContainer.Y -= 1000f;
                
                _blobArray.Add(objContainer);
                GameObjList.Add(objContainer);

            }

            base.Initialize();

        }

        public override void OnEnter() {
            
            _boss1.Name = "Herodotus";
            _boss1.GetChildAt(0).TextureColor = Color.White;
            _boss1.GetChildAt(2).TextureColor = Color.LightSkyBlue;
            _boss1.GetChildAt(2).Opacity = 0.8f;
            (_boss1.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
            _boss1.GetChildAt(1).TextureColor = Color.Black;
            
            SoundManager.StopMusic(0.5f);
            
            Player.LockControls();
            Player.AttachedLevel.UpdateCamera();
            Player.AttachedLevel.CameraLockedToPlayer = false;
            
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, new[] {
                "X",
                (Bounds.Left + 700).ToString(),
                "Y",
                _boss1.Y.ToString()
            });

            Tween.By(_blobArray[0], 1f, Quad.EaseIn, new[] {
                "delay",
                "0.5",
                "Y",
                "1150"
            });

            Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[] {
                _blobArray[0]
            });

            Tween.By(_blobArray[1], 1f, Quad.EaseIn, new[] {
                "delay",
                "1.5",
                "Y",
                "1150"
            });

            Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[] {
                _blobArray[1]
            });

            Tween.RunFunction(1f, this, "DropBlobs", new object[0]);
            _boss1.Scale = new Vector2(0.5f, 0.5f);
            Player.AttachedLevel.RunCinematicBorders(9f);
            base.OnEnter();

        }

        public void DropBlobs() {
            
            float num = 1f;
            
            for (int i = 2; i < _blobArray.Count; i++) {
                
                Tween.By(_blobArray[i], 1f, Quad.EaseIn, new[] {
                    "delay",
                    num.ToString(),
                    "Y",
                    "1150"
                });

                Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[] {
                    _blobArray[i]
                });

                num += 0.5f * (_blobArray.Count - i) / _blobArray.Count;

            }

            Tween.RunFunction(num + 1f, _boss1, "PlayAnimation", new object[] {
                true
            });

            Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", new object[] {
                "Boss_Blob_Idle_Loop"
            });

            Tween.RunFunction(num + 1f, this, "DisplayBossTitle", new object[] {
                "The Infinite",
                _boss1.Name,
                "Intro2"
            });

            Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", new object[] {
                "Boss_Blob_Spawn"
            });

        }

        public void GrowBlob(GameObj blob) {
            
            float num = (_desiredBossScale - 0.5f) / NumIntroBlobs;
            blob.Visible = false;
            _boss1.PlayAnimation(false);
            _boss1.ScaleX += num;
            _boss1.ScaleY += num;
            SoundManager.PlaySound(new[] {
                "Boss_Blob_Spawn_01",
                "Boss_Blob_Spawn_02",
                "Boss_Blob_Spawn_03"
            });

        }

        public void Intro2() {
            
            _boss1.PlayAnimation();
            
            Tween.To(Player.AttachedLevel.Camera, 0.5f, Quad.EaseInOut, new[] {
                "delay",
                "0.5",
                "X",
                (Player.X + GlobalEV.Camera_XOffset).ToString(),
                "Y",
                (Bounds.Bottom - (Player.AttachedLevel.Camera.Bounds.Bottom - Player.AttachedLevel.Camera.Y)).ToString()
            });

            Tween.AddEndHandlerToLastTween(this, "BeginBattle", new object[0]);

        }

        public void BeginBattle() {
            
            SoundManager.PlayMusic("DungeonBoss", true, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            _boss1.UnpauseEnemy(true);
            _boss1.PlayAnimation();

        }

        public override void Update(GameTime gameTime) {
            
            Rectangle bounds = Bounds;

            for (int index = 0; index < EnemyList.Count; index++) {
                EnemyObj current = EnemyList[index];
                if (current.Type == 2 && !current.IsKilled && (current.X > (float)(this.Bounds.Right - 20) || current.X < (float)(this.Bounds.Left + 20) || current.Y > (float)(this.Bounds.Bottom - 20) || current.Y < (float)(this.Bounds.Top + 20)))
                    current.Position = new Vector2(bounds.Center.X, bounds.Center.Y);
            }

            for (int index = 0; index < TempEnemyList.Count; index++) {
                EnemyObj current2 = TempEnemyList[index];
                if (current2.Type == 2 && !current2.IsKilled && (current2.X > (float)(this.Bounds.Right - 20) || current2.X < (float)(this.Bounds.Left + 20) || current2.Y > (float)(this.Bounds.Bottom - 20) || current2.Y < (float)(this.Bounds.Top + 20)))
                    current2.Position = new Vector2(bounds.Center.X, bounds.Center.Y);
            }

            base.Update(gameTime);

        }

        public override void Dispose() {

            if (!IsDisposed) {
                _blobArray.Clear();
                _blobArray = null;
                _boss1 = null;
                base.Dispose();
            }

        }

    }

}
