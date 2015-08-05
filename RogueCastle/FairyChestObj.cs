using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class FairyChestObj : ChestObj {
        private const float SparkleDelay = 1f;
        private int m_conditionType;
        private SpriteObj m_errorSprite;
        private SpriteObj m_lockSprite;
        private PlayerObj m_player;
        private float m_sparkleCounter = 1f;
        private TextObj m_timerText;

        public FairyChestObj(PhysicsManager physicsManager) : base(physicsManager) {
            m_lockSprite = new SpriteObj("Chest4Unlock_Sprite");
            m_errorSprite = new SpriteObj("CancelIcon_Sprite");
            m_errorSprite.Visible = false;
            m_timerText = new TextObj(Game.JunicodeFont);
            m_timerText.FontSize = 18f;
            m_timerText.DropShadow = new Vector2(2f, 2f);
            m_timerText.Align = Types.TextAlign.Centre;
            m_player = Game.ScreenManager.Player;
        }

        public int State { get; internal set; }
        public float Timer { get; set; }

        public int ConditionType {
            get { return m_conditionType; }
        }

        public void SetConditionType(int conditionType = 0) {
            if (conditionType != 0)
                m_conditionType = conditionType;
            else
                int.TryParse(this.Tag, out m_conditionType);
            if (m_conditionType == 8)
                Timer = 5f;
        }

        public void SetChestUnlocked() {
            if (ConditionType != 10 && ConditionType != 0)
                m_player.AttachedLevel.ObjectiveComplete();
            State = 1;
            m_lockSprite.PlayAnimation(false);
            Tween.By(m_lockSprite, 0.2f, new Easing(Linear.EaseNone), new[] {
                "Y",
                "40"
            });
            Tween.To(m_lockSprite, 0.2f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.1",
                "Opacity",
                "0"
            });
        }

        public void SetChestFailed(bool skipTween = false) {
            if (!skipTween)
                m_player.AttachedLevel.ObjectiveFailed();
            State = 2;
            m_errorSprite.Visible = true;
            m_errorSprite.Opacity = 0f;
            m_errorSprite.Scale = Vector2.One;
            m_errorSprite.Position = new Vector2(base.X, base.Y - (float)(this.Height / 2));
            if (!skipTween) {
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "FairyChest_Fail");
                Tween.To(m_errorSprite, 0.5f, new Easing(Quad.EaseIn), new[] {
                    "ScaleX",
                    "0.5",
                    "ScaleY",
                    "0.5",
                    "Opacity",
                    "1"
                });
                return;
            }
            m_errorSprite.Scale = new Vector2(0.5f, 0.5f);
            m_errorSprite.Opacity = 1f;
        }

        public override void OpenChest(ItemDropManager itemDropManager, PlayerObj player) {
            if (State == 1 && !base.IsOpen && !base.IsLocked) {
                base.GoToFrame(2);
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Chest_Open_Large");
                if (Game.PlayerStats.TotalRunesFound >= 55) {
                    base.GiveStatDrop(itemDropManager, m_player, 1, 0);
                    player.AttachedLevel.RefreshMapChestIcons();
                    return;
                }
                List<byte[]> getRuneArray = Game.PlayerStats.GetRuneArray;
                List<Vector2> list = new List<Vector2>();
                int num = 0;
                foreach (byte[] current in getRuneArray) {
                    int num2 = 0;
                    byte[] array = current;
                    for (int i = 0; i < array.Length; i++) {
                        if (array[i] == 0)
                            list.Add(new Vector2(num, num2));
                        num2++;
                    }
                    num++;
                }
                if (list.Count > 0) {
                    Vector2 vector = list[CDGMath.RandomInt(0, list.Count - 1)];
                    Game.PlayerStats.GetRuneArray[(int)vector.X][(int)vector.Y] = 1;
                    List<object> list2 = new List<object>();
                    list2.Add(new Vector2(base.X, base.Y - (float)this.Height / 2f));
                    list2.Add(2);
                    list2.Add(new Vector2(vector.X, vector.Y));
                    (player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list2);
                    player.RunGetItemAnimation();
                    Console.WriteLine(string.Concat(new object[] {
                        "Unlocked item index ",
                        vector.X,
                        " of type ",
                        vector.Y
                    }));
                }
                else
                    base.GiveGold(itemDropManager, 0);
                player.AttachedLevel.RefreshMapChestIcons();
            }
        }

        public override void Draw(Camera2D camera) {
            if (State == 0)
                ChestConditionChecker.SetConditionState(this, m_player);
            if (!base.IsOpen) {
                if (Game.ScreenManager.CurrentScreen is ProceduralLevelScreen && m_sparkleCounter > 0f) {
                    m_sparkleCounter -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
                    if (m_sparkleCounter <= 0f) {
                        m_sparkleCounter = 1f;
                        float num = 0f;
                        for (int i = 0; i < 2; i++) {
                            Tween.To(this, num, new Easing(Linear.EaseNone), new string[0]);
                            Tween.AddEndHandlerToLastTween(m_player.AttachedLevel.ImpactEffectPool, "DisplayChestSparkleEffect", new object[] {
                                new Vector2(base.X, base.Y - (float)(this.Height / 2))
                            });
                            num += 0.5f;
                        }
                    }
                }
                if (ConditionType == 8 && State == 0) {
                    if (!m_player.AttachedLevel.IsPaused)
                        Timer -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
                    m_timerText.Position = new Vector2(base.Position.X, base.Y - 50f);
                    m_timerText.Text = ((int)Timer + 1).ToString();
                    m_timerText.Draw(camera);
                    m_player.AttachedLevel.UpdateObjectiveProgress((DialogueManager.GetText("Chest_Locked " + ConditionType).Dialogue[0] + (int)(Timer + 1f)));
                }
            }
            if (ConditionType != 10 || base.IsOpen) {
                base.Draw(camera);
                m_lockSprite.Flip = this.Flip;
                if (this.Flip == SpriteEffects.None)
                    m_lockSprite.Position = new Vector2(base.X - 10f, base.Y - (float)(this.Height / 2));
                else
                    m_lockSprite.Position = new Vector2(base.X + 10f, base.Y - (float)(this.Height / 2));
                m_lockSprite.Draw(camera);
                m_errorSprite.Position = new Vector2(base.X, base.Y - (float)(this.Height / 2));
                m_errorSprite.Draw(camera);
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            if (State == 1)
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void ForceOpen() {
            State = 1;
            m_errorSprite.Visible = false;
            m_lockSprite.Visible = false;
            base.ForceOpen();
        }

        public override void ResetChest() {
            State = 0;
            m_errorSprite.Visible = false;
            m_lockSprite.Visible = true;
            m_lockSprite.Opacity = 1f;
            base.Opacity = 1f;
            m_lockSprite.PlayAnimation(1, 1, false);
            base.TextureColor = Color.White;
            if (ConditionType == 8)
                Timer = 5f;
            base.ResetChest();
        }

        protected override GameObj CreateCloneInstance() {
            return new FairyChestObj(base.PhysicsMngr);
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            FairyChestObj fairyChestObj = obj as FairyChestObj;
            fairyChestObj.State = State;
            SetConditionType(0);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_player = null;
                m_lockSprite.Dispose();
                m_lockSprite = null;
                m_errorSprite.Dispose();
                m_errorSprite = null;
                m_timerText.Dispose();
                m_timerText = null;
                base.Dispose();
            }
        }
    }
}
