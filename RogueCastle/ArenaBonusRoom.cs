using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {

    public class ArenaBonusRoom : BonusRoomObj {

        private ChestObj _chest;
        private bool     _chestRevealed;
        private float    _chestStartingY;

        public override void Initialize() {

            for (int index = 0; index < GameObjList.Count; index++) {

                GameObj gameObj = GameObjList[index];

                if (gameObj is ChestObj) {
                    _chest = (gameObj as ChestObj);
                    break;
                }

            }

            _chest.ChestType = 3;
            _chestStartingY = _chest.Y - 200f + _chest.Height + 6f;
            
            base.Initialize();

        }

        public override void OnEnter() {

            UpdateEnemyNames();
            _chest.Y = _chestStartingY;
            _chest.ChestType = 3;
            
            if (RoomCompleted) {

                _chest.Opacity = 1f;
                _chest.Y = _chestStartingY + 200f;
                _chest.IsEmpty = true;
                _chest.ForceOpen();
                _chestRevealed = true;
                
                using (List<EnemyObj>.Enumerator enumerator = EnemyList.GetEnumerator()) {

                    while (enumerator.MoveNext()) {

                        EnemyObj enemyObj = enumerator.Current;

                        if (enemyObj != null && !enemyObj.IsKilled)
                            enemyObj.KillSilently();

                    }

                    goto IL_137;

                }

            }

            if (ActiveEnemies == 0) {

                _chest.Opacity  = 1f;
                _chest.Y        = _chestStartingY + 200f;
                _chest.IsEmpty  = false;
                _chest.IsLocked = false;
                _chestRevealed  = true;

            }
            else {

                _chest.Opacity  = 0f;
                _chest.Y        = _chestStartingY;
                _chest.IsLocked = true;
                _chestRevealed  = false;

            }

            IL_137:
            
            if (_chest.PhysicsMngr == null)
                Player.PhysicsMngr.AddObject(_chest);
            
            base.OnEnter();

        }

        private void UpdateEnemyNames() {
            bool flag = false;

            for (int index = 0; index < EnemyList.Count; index++) {

                EnemyObj enemyObj = EnemyList[index];
                
                if (enemyObj is EnemyObj_EarthWizard) {
                    if (!flag) {
                        enemyObj.Name = "Barbatos";
                        flag = true;
                    }
                    else
                        enemyObj.Name = "Amon";
                }
                else if (enemyObj is EnemyObj_Skeleton) {
                    if (!flag) {
                        enemyObj.Name = "Berith";
                        flag = true;
                    }
                    else
                        enemyObj.Name = "Halphas";
                }
                else if (enemyObj is EnemyObj_Plant) {
                    if (!flag) {
                        enemyObj.Name = "Stolas";
                        flag = true;
                    }
                    else
                        enemyObj.Name = "Focalor";
                }

            }

        }

        public override void Update(GameTime gameTime) {

            if (!_chest.IsOpen) {
                if (ActiveEnemies == 0 && _chest.Opacity == 0f && !_chestRevealed) {
                    _chestRevealed = true;
                    DisplayChest();
                }
            }
            else if (!RoomCompleted)
                RoomCompleted = true;

            base.Update(gameTime);

        }

        public override void OnExit() {

            bool skeletonMiniBossKilled = false;
            bool plantMiniBossKilled    = false;
            bool portraitMiniBossKilled = false;
            bool knightMiniBossKilled   = false;
            bool wizardMiniBossKilled   = false;
            
            if (Game.PlayerStats.EnemiesKilledList[EnemyType.Skeleton].W > 0f)
                skeletonMiniBossKilled = true;
            
            if (Game.PlayerStats.EnemiesKilledList[EnemyType.Plant].W > 0f)
                plantMiniBossKilled = true;
            
            if (Game.PlayerStats.EnemiesKilledList[EnemyType.Portrait].W > 0f)
                portraitMiniBossKilled = true;
            
            if (Game.PlayerStats.EnemiesKilledList[EnemyType.Knight].W > 0f)
                knightMiniBossKilled = true;
            
            if (Game.PlayerStats.EnemiesKilledList[EnemyType.EarthWizard].W > 0f)
                wizardMiniBossKilled = true;
            
            if (skeletonMiniBossKilled && plantMiniBossKilled && portraitMiniBossKilled && knightMiniBossKilled && wizardMiniBossKilled)
                GameUtil.UnlockAchievement("FEAR_OF_ANIMALS");
            
            base.OnExit();

        }

        private void DisplayChest() {

            _chest.IsLocked = false;
            
            Tween.To(_chest, 2f, Tween.EaseNone, new[] {
                "Opacity",
                "1"
            });
            
            Tween.By(_chest, 2f, Quad.EaseOut, new[] {
                "Y",
                "200"
            });

        }

        public override void Dispose() {

            if (!IsDisposed) {
                _chest = null;
                base.Dispose();
            }

        }

    }

}
