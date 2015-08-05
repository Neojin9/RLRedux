using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class SFXVolOptionsObj : OptionsObj {
        private SpriteObj m_volumeBar;
        private SpriteObj m_volumeBarBG;

        public SFXVolOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "SFX Volume") {
            m_volumeBarBG = new SpriteObj("OptionsScreenVolumeBG_Sprite");
            m_volumeBarBG.X = (float)m_optionsTextOffset;
            m_volumeBarBG.Y = (float)m_volumeBarBG.Height / 2f - 2f;
            this.AddChild(m_volumeBarBG);
            m_volumeBar = new SpriteObj("OptionsScreenVolumeBar_Sprite");
            m_volumeBar.X = m_volumeBarBG.X + 6f;
            m_volumeBar.Y = m_volumeBarBG.Y + 5f;
            this.AddChild(m_volumeBar);
        }

        public override bool IsActive {
            get { return base.IsActive; }
            set {
                base.IsActive = value;
                if (value) {
                    m_volumeBar.TextureColor = Color.Yellow;
                    return;
                }
                m_volumeBar.TextureColor = Color.White;
            }
        }

        public override void Initialize() {
            m_volumeBar.ScaleX = SoundManager.GlobalSFXVolume;
            base.Initialize();
        }

        public override void HandleInput() {
            if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21)) {
                SoundManager.GlobalSFXVolume -= 0.01f;
                SetVolumeLevel();
            }
            else if (Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23)) {
                SoundManager.GlobalSFXVolume += 0.01f;
                SetVolumeLevel();
            }
            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
                IsActive = false;
            base.HandleInput();
        }

        public void SetVolumeLevel() {
            m_volumeBar.ScaleX = SoundManager.GlobalSFXVolume;
            Game.GameConfig.SFXVolume = SoundManager.GlobalSFXVolume;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_volumeBar = null;
                m_volumeBarBG = null;
                base.Dispose();
            }
        }
    }
}
