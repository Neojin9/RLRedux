using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public abstract class OptionsObj : ObjContainer {
        protected bool m_isActive;
        protected bool m_isSelected;
        protected TextObj m_nameText;
        protected int m_optionsTextOffset = 250;
        protected OptionsScreen m_parentScreen;

        public OptionsObj(OptionsScreen parentScreen, string name) {
            m_parentScreen = parentScreen;
            m_nameText = new TextObj(Game.JunicodeFont);
            m_nameText.FontSize = 12f;
            m_nameText.Text = name;
            m_nameText.DropShadow = new Vector2(2f, 2f);
            this.AddChild(m_nameText);
            base.ForceDraw = true;
        }

        public virtual bool IsActive {
            get { return m_isActive; }
            set {
                if (value)
                    IsSelected = false;
                else
                    IsSelected = true;
                m_isActive = value;
                if (!value)
                    (m_parentScreen.ScreenManager.Game as Game).SaveConfig();
            }
        }

        public bool IsSelected {
            get { return m_isSelected; }
            set {
                m_isSelected = value;
                if (value) {
                    m_nameText.TextureColor = Color.Yellow;
                    return;
                }
                m_nameText.TextureColor = Color.White;
            }
        }

        public virtual void Initialize() {}

        public virtual void HandleInput() {
            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
                SoundManager.PlaySound("Options_Menu_Deselect");
        }

        public virtual void Update(GameTime gameTime) {}

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_parentScreen = null;
                m_nameText = null;
                base.Dispose();
            }
        }
    }
}
