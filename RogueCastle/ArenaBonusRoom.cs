using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class ArenaBonusRoom : BonusRoomObj {
        private ChestObj m_chest;
        private bool m_chestRevealed;
        private float m_chestStartingY;

        public override void Initialize() {
            foreach (GameObj current in base.GameObjList) {
                if (current is ChestObj) {
                    m_chest = (current as ChestObj);
                    break;
                }
            }
            m_chest.ChestType = 3;
            m_chestStartingY = m_chest.Y - 200f + (float)m_chest.Height + 6f;
            base.Initialize();
        }

        public override void OnEnter() {
            UpdateEnemyNames();
            m_chest.Y = m_chestStartingY;
            m_chest.ChestType = 3;
            if (base.RoomCompleted) {
                m_chest.Opacity = 1f;
                m_chest.Y = m_chestStartingY + 200f;
                m_chest.IsEmpty = true;
                m_chest.ForceOpen();
                m_chestRevealed = true;
                using (List<EnemyObj>.Enumerator enumerator = base.EnemyList.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        EnemyObj current = enumerator.Current;
                        if (!current.IsKilled)
                            current.KillSilently();
                    }
                    goto IL_137;
                }
            }
            if (base.ActiveEnemies == 0) {
                m_chest.Opacity = 1f;
                m_chest.Y = m_chestStartingY + 200f;
                m_chest.IsEmpty = false;
                m_chest.IsLocked = false;
                m_chestRevealed = true;
            }
            else {
                m_chest.Opacity = 0f;
                m_chest.Y = m_chestStartingY;
                m_chest.IsLocked = true;
                m_chestRevealed = false;
            }
            IL_137:
            if (m_chest.PhysicsMngr == null)
                Player.PhysicsMngr.AddObject(m_chest);
            base.OnEnter();
        }

        private void UpdateEnemyNames() {
            bool flag = false;
            foreach (EnemyObj current in base.EnemyList) {
                if (current is EnemyObj_EarthWizard) {
                    if (!flag) {
                        current.Name = "Barbatos";
                        flag = true;
                    }
                    else
                        current.Name = "Amon";
                }
                else if (current is EnemyObj_Skeleton) {
                    if (!flag) {
                        current.Name = "Berith";
                        flag = true;
                    }
                    else
                        current.Name = "Halphas";
                }
                else if (current is EnemyObj_Plant) {
                    if (!flag) {
                        current.Name = "Stolas";
                        flag = true;
                    }
                    else
                        current.Name = "Focalor";
                }
            }
        }

        public override void Update(GameTime gameTime) {
            if (!m_chest.IsOpen) {
                if (base.ActiveEnemies == 0 && m_chest.Opacity == 0f && !m_chestRevealed) {
                    m_chestRevealed = true;
                    DisplayChest();
                }
            }
            else if (!base.RoomCompleted)
                base.RoomCompleted = true;
            base.Update(gameTime);
        }

        public override void OnExit() {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            if (Game.PlayerStats.EnemiesKilledList[15].W > 0f)
                flag = true;
            if (Game.PlayerStats.EnemiesKilledList[22].W > 0f)
                flag2 = true;
            if (Game.PlayerStats.EnemiesKilledList[32].W > 0f)
                flag3 = true;
            if (Game.PlayerStats.EnemiesKilledList[12].W > 0f)
                flag4 = true;
            if (Game.PlayerStats.EnemiesKilledList[5].W > 0f)
                flag5 = true;
            if (flag && flag2 && flag3 && flag4 && flag5)
                GameUtil.UnlockAchievement("FEAR_OF_ANIMALS");
            base.OnExit();
        }

        private void DisplayChest() {
            m_chest.IsLocked = false;
            Tween.To(m_chest, 2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "1"
            });
            Tween.By(m_chest, 2f, new Easing(Quad.EaseOut), new[] {
                "Y",
                "200"
            });
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_chest = null;
                base.Dispose();
            }
        }
    }
}
