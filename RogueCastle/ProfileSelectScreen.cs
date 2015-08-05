using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class ProfileSelectScreen : Screen {
        private KeyIconTextObj m_cancelText;
        private KeyIconTextObj m_confirmText;
        private KeyIconTextObj m_deleteProfileText;
        private bool m_lockControls;
        private KeyIconTextObj m_navigationText;
        private int m_selectedIndex;
        private ObjContainer m_selectedSlot;
        private ObjContainer m_slot1Container;
        private ObjContainer m_slot2Container;
        private ObjContainer m_slot3Container;
        private List<ObjContainer> m_slotArray;
        private SpriteObj m_title;

        public ProfileSelectScreen() {
            m_slotArray = new List<ObjContainer>();
            base.DrawIfCovered = true;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent() {
            m_title = new SpriteObj("ProfileSelectTitle_Sprite");
            m_title.ForceDraw = true;
            TextObj textObj = new TextObj(Game.JunicodeFont);
            textObj.Align = Types.TextAlign.Centre;
            textObj.Text = "- START NEW LEGACY - ";
            textObj.TextureColor = Color.White;
            textObj.OutlineWidth = 2;
            textObj.FontSize = 10f;
            textObj.Position = new Vector2(0f, -((float)textObj.Height / 2f));
            m_slot1Container = new ObjContainer("ProfileSlotBG_Container");
            TextObj obj = textObj.Clone() as TextObj;
            m_slot1Container.AddChild(obj);
            SpriteObj spriteObj = new SpriteObj("ProfileSlot1Text_Sprite");
            spriteObj.Position = new Vector2(-130f, -35f);
            m_slot1Container.AddChild(spriteObj);
            TextObj textObj2 = textObj.Clone() as TextObj;
            m_slot1Container.AddChild(textObj2);
            textObj2.Position = new Vector2(120f, 15f);
            TextObj textObj3 = textObj.Clone() as TextObj;
            textObj3.Position = new Vector2(-120f, 15f);
            m_slot1Container.AddChild(textObj3);
            m_slot1Container.ForceDraw = true;
            m_slot2Container = new ObjContainer("ProfileSlotBG_Container");
            TextObj obj2 = textObj.Clone() as TextObj;
            m_slot2Container.AddChild(obj2);
            SpriteObj spriteObj2 = new SpriteObj("ProfileSlot2Text_Sprite");
            spriteObj2.Position = new Vector2(-130f, -35f);
            m_slot2Container.AddChild(spriteObj2);
            TextObj textObj4 = textObj.Clone() as TextObj;
            m_slot2Container.AddChild(textObj4);
            textObj4.Position = new Vector2(120f, 15f);
            TextObj textObj5 = textObj.Clone() as TextObj;
            textObj5.Position = new Vector2(-120f, 15f);
            m_slot2Container.AddChild(textObj5);
            m_slot2Container.ForceDraw = true;
            m_slot3Container = new ObjContainer("ProfileSlotBG_Container");
            TextObj obj3 = textObj.Clone() as TextObj;
            m_slot3Container.AddChild(obj3);
            SpriteObj spriteObj3 = new SpriteObj("ProfileSlot3Text_Sprite");
            spriteObj3.Position = new Vector2(-130f, -35f);
            m_slot3Container.AddChild(spriteObj3);
            TextObj textObj6 = textObj.Clone() as TextObj;
            m_slot3Container.AddChild(textObj6);
            textObj6.Position = new Vector2(120f, 15f);
            TextObj textObj7 = textObj.Clone() as TextObj;
            textObj7.Position = new Vector2(-120f, 15f);
            m_slot3Container.AddChild(textObj7);
            m_slot3Container.ForceDraw = true;
            m_slotArray.Add(m_slot1Container);
            m_slotArray.Add(m_slot2Container);
            m_slotArray.Add(m_slot3Container);
            m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
            m_confirmText.Text = "to select profile";
            m_confirmText.DropShadow = new Vector2(2f, 2f);
            m_confirmText.FontSize = 12f;
            m_confirmText.Align = Types.TextAlign.Right;
            m_confirmText.Position = new Vector2(1290f, 570f);
            m_confirmText.ForceDraw = true;
            m_cancelText = new KeyIconTextObj(Game.JunicodeFont);
            m_cancelText.Text = "to exit screen";
            m_cancelText.Align = Types.TextAlign.Right;
            m_cancelText.DropShadow = new Vector2(2f, 2f);
            m_cancelText.FontSize = 12f;
            m_cancelText.Position = new Vector2(m_confirmText.X, m_confirmText.Y + 40f);
            m_cancelText.ForceDraw = true;
            m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
            m_navigationText.Text = "to navigate profiles";
            m_navigationText.Align = Types.TextAlign.Right;
            m_navigationText.DropShadow = new Vector2(2f, 2f);
            m_navigationText.FontSize = 12f;
            m_navigationText.Position = new Vector2(m_confirmText.X, m_confirmText.Y + 80f);
            m_navigationText.ForceDraw = true;
            m_deleteProfileText = new KeyIconTextObj(Game.JunicodeFont);
            m_deleteProfileText.Text = "to delete profile";
            m_deleteProfileText.Align = Types.TextAlign.Left;
            m_deleteProfileText.DropShadow = new Vector2(2f, 2f);
            m_deleteProfileText.FontSize = 12f;
            m_deleteProfileText.Position = new Vector2(20f, m_confirmText.Y + 80f);
            m_deleteProfileText.ForceDraw = true;
            base.LoadContent();
        }

        public override void OnEnter() {
            SoundManager.PlaySound("DialogOpen");
            m_lockControls = true;
            m_selectedIndex = (Game.GameConfig.ProfileSlot - 1);
            m_selectedSlot = m_slotArray[m_selectedIndex];
            m_selectedSlot.TextureColor = Color.Yellow;
            CheckSaveHeaders(m_slot1Container, 1);
            CheckSaveHeaders(m_slot2Container, 2);
            CheckSaveHeaders(m_slot3Container, 3);
            m_deleteProfileText.Visible = true;
            if (m_slotArray[m_selectedIndex].ID == 0)
                m_deleteProfileText.Visible = false;
            Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new[] {
                "BackBufferOpacity",
                "0.9"
            });
            m_title.Position = new Vector2(660f, 100f);
            m_slot1Container.Position = new Vector2(660f, 300f);
            m_slot2Container.Position = new Vector2(660f, 420f);
            m_slot3Container.Position = new Vector2(660f, 540f);
            TweenInText(m_title, 0f);
            TweenInText(m_slot1Container, 0.05f);
            TweenInText(m_slot2Container, 0.1f);
            TweenInText(m_slot3Container, 0.15f);
            Tween.RunFunction(0.5f, this, "UnlockControls", new object[0]);
            if (InputManager.GamePadIsConnected(PlayerIndex.One)) {
                m_confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
                m_cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
                m_navigationText.Text = "[Button:LeftStick] to navigate profiles";
            }
            else {
                m_confirmText.ForcedScale = new Vector2(1f, 1f);
                m_cancelText.ForcedScale = new Vector2(1f, 1f);
                m_navigationText.Text = "Arrow keys to navigate profiles";
            }
            m_confirmText.Text = "[Input:" + 0 + "] to select profiles";
            m_cancelText.Text = "[Input:" + 2 + "] to exit profiles";
            m_deleteProfileText.Text = "[Input:" + 26 + "] to delete profile";
            m_confirmText.Opacity = 0f;
            m_cancelText.Opacity = 0f;
            m_navigationText.Opacity = 0f;
            m_deleteProfileText.Opacity = 0f;
            Tween.To(m_confirmText, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "1"
            });
            Tween.To(m_cancelText, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "1"
            });
            Tween.To(m_navigationText, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "1"
            });
            Tween.To(m_deleteProfileText, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "1"
            });
            base.OnEnter();
        }

        private void CheckSaveHeaders(ObjContainer container, byte profile) {
            TextObj textObj = container.GetChildAt(1) as TextObj;
            TextObj textObj2 = container.GetChildAt(3) as TextObj;
            TextObj textObj3 = container.GetChildAt(4) as TextObj;
            textObj2.Text = "";
            textObj3.Text = "";
            string text = null;
            byte classType = 0;
            int num = 0;
            bool flag = false;
            int num2 = 0;
            try {
                (base.ScreenManager.Game as Game).SaveManager.GetSaveHeader(profile, out classType, out text, out num, out flag, out num2);
                if (text == null) {
                    textObj.Text = "- START NEW LEGACY -";
                    container.ID = 0;
                }
                else {
                    bool isFemale = text.Contains("Lady");
                    if (!flag)
                        textObj.Text = text + " the " + ClassType.ToString(classType, isFemale);
                    else
                        textObj.Text = text + " the Deceased";
                    textObj2.Text = "Lvl. " + num;
                    if (num2 > 0)
                        textObj3.Text = "NG+ " + num2;
                    container.ID = 1;
                }
            } catch {
                textObj.Text = "- START NEW LEGACY -";
                container.ID = 0;
            }
        }

        public void UnlockControls() {
            m_lockControls = false;
        }

        private void TweenInText(GameObj obj, float delay) {
            obj.Opacity = 0f;
            obj.Y -= 50f;
            Tween.To(obj, 0.5f, new Easing(Tween.EaseNone), new[] {
                "delay",
                delay.ToString(),
                "Opacity",
                "1"
            });
            Tween.By(obj, 0.5f, new Easing(Quad.EaseOut), new[] {
                "delay",
                delay.ToString(),
                "Y",
                "50"
            });
        }

        private void ExitTransition() {
            SoundManager.PlaySound("DialogMenuClose");
            Tween.To(m_confirmText, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "0"
            });
            Tween.To(m_cancelText, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "0"
            });
            Tween.To(m_navigationText, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "0"
            });
            Tween.To(m_deleteProfileText, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "0"
            });
            m_lockControls = true;
            TweenOutText(m_title, 0f);
            TweenOutText(m_slot1Container, 0.05f);
            TweenOutText(m_slot2Container, 0.1f);
            TweenOutText(m_slot3Container, 0.15f);
            Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "0.5",
                "BackBufferOpacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(base.ScreenManager, "HideCurrentScreen", new object[0]);
        }

        private void TweenOutText(GameObj obj, float delay) {
            Tween.To(obj, 0.5f, new Easing(Tween.EaseNone), new[] {
                "delay",
                delay.ToString(),
                "Opacity",
                "0"
            });
            Tween.By(obj, 0.5f, new Easing(Quad.EaseInOut), new[] {
                "delay",
                delay.ToString(),
                "Y",
                "-50"
            });
        }

        public override void OnExit() {
            m_slot1Container.TextureColor = Color.White;
            m_slot2Container.TextureColor = Color.White;
            m_slot3Container.TextureColor = Color.White;
            m_lockControls = false;
            base.OnExit();
        }

        public override void HandleInput() {
            if (!m_lockControls) {
                ObjContainer selectedSlot = m_selectedSlot;
                if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19)) {
                    m_selectedIndex++;
                    if (m_selectedIndex >= m_slotArray.Count)
                        m_selectedIndex = 0;
                    m_selectedSlot = m_slotArray[m_selectedIndex];
                    SoundManager.PlaySound("frame_swap");
                    m_deleteProfileText.Visible = true;
                    if (m_selectedSlot.ID == 0)
                        m_deleteProfileText.Visible = false;
                }
                if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)) {
                    m_selectedIndex--;
                    if (m_selectedIndex < 0)
                        m_selectedIndex = m_slotArray.Count - 1;
                    m_selectedSlot = m_slotArray[m_selectedIndex];
                    SoundManager.PlaySound("frame_swap");
                    m_deleteProfileText.Visible = true;
                    if (m_selectedSlot.ID == 0)
                        m_deleteProfileText.Visible = false;
                }
                if (m_selectedSlot != selectedSlot) {
                    selectedSlot.TextureColor = Color.White;
                    m_selectedSlot.TextureColor = Color.Yellow;
                }
                if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
                    ExitTransition();
                if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1)) {
                    SoundManager.PlaySound("Map_On");
                    Game.GameConfig.ProfileSlot = (byte)(m_selectedIndex + 1);
                    Game game = base.ScreenManager.Game as Game;
                    game.SaveConfig();
                    if (game.SaveManager.FileExists(SaveType.PlayerData))
                        (base.ScreenManager as RCScreenManager).DisplayScreen(3, true, null);
                    else {
                        SkillSystem.ResetAllTraits();
                        Game.PlayerStats.Dispose();
                        Game.PlayerStats = new PlayerStats();
                        (base.ScreenManager as RCScreenManager).Player.Reset();
                        Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
                        Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;
                        (base.ScreenManager as RCScreenManager).DisplayScreen(23, true, null);
                    }
                }
                if (Game.GlobalInput.JustPressed(26) && m_deleteProfileText.Visible) {
                    SoundManager.PlaySound("Map_On");
                    DeleteSaveAsk();
                }
            }
            base.HandleInput();
        }

        public override void Draw(GameTime gametime) {
            base.Camera.Begin();
            base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            m_title.Draw(base.Camera);
            m_slot1Container.Draw(base.Camera);
            m_slot2Container.Draw(base.Camera);
            m_slot3Container.Draw(base.Camera);
            m_confirmText.Draw(base.Camera);
            m_cancelText.Draw(base.Camera);
            m_navigationText.Draw(base.Camera);
            m_deleteProfileText.Draw(base.Camera);
            base.Camera.End();
            base.Draw(gametime);
        }

        public void DeleteSaveAsk() {
            RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
            rCScreenManager.DialogueScreen.SetDialogue("Delete Save");
            rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
            rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSaveAskAgain", new object[0]);
            rCScreenManager.DisplayScreen(13, false, null);
        }

        public void DeleteSaveAskAgain() {
            RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
            rCScreenManager.DialogueScreen.SetDialogue("Delete Save2");
            rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
            rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSave", new object[0]);
            rCScreenManager.DisplayScreen(13, false, null);
        }

        public void DeleteSave() {
            bool flag = false;
            byte profileSlot = Game.GameConfig.ProfileSlot;
            if (Game.GameConfig.ProfileSlot == m_selectedIndex + 1)
                flag = true;
            Game.GameConfig.ProfileSlot = (byte)(m_selectedIndex + 1);
            (base.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(false);
            (base.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(true);
            Game.GameConfig.ProfileSlot = profileSlot;
            if (flag) {
                Game.PlayerStats.Dispose();
                SkillSystem.ResetAllTraits();
                Game.PlayerStats = new PlayerStats();
                (base.ScreenManager as RCScreenManager).Player.Reset();
                SoundManager.StopMusic(1f);
                (base.ScreenManager as RCScreenManager).DisplayScreen(23, true, null);
                return;
            }
            m_deleteProfileText.Visible = false;
            CheckSaveHeaders(m_slotArray[m_selectedIndex], (byte)(m_selectedIndex + 1));
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_title.Dispose();
                m_title = null;
                m_slot1Container.Dispose();
                m_slot1Container = null;
                m_slot2Container.Dispose();
                m_slot2Container = null;
                m_slot3Container.Dispose();
                m_slot3Container = null;
                m_slotArray.Clear();
                m_slotArray = null;
                m_selectedSlot = null;
                m_confirmText.Dispose();
                m_confirmText = null;
                m_cancelText.Dispose();
                m_cancelText = null;
                m_navigationText.Dispose();
                m_navigationText = null;
                m_deleteProfileText.Dispose();
                m_deleteProfileText = null;
                base.Dispose();
            }
        }
    }
}
