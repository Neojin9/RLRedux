using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyHUDObj : SpriteObj {
        private int m_blinkCounter;
        private float m_blinkDuration = 0.05f;
        private float m_blinkDurationCounter;
        private int m_blinkNumber = 13;
        private SpriteObj m_enemyHPBar;
        private int m_enemyHPBarLength;
        private float m_enemyHPPercent;
        private TextObj m_enemyLevelText;
        private TextObj m_enemyNameText;
        private float m_opacity = 1f;

        public EnemyHUDObj() : base("EnemyHUD_Sprite") {
            base.ForceDraw = true;
            m_enemyNameText = new TextObj(null);
            m_enemyNameText.Font = Game.JunicodeFont;
            m_enemyNameText.FontSize = 10f;
            m_enemyNameText.Align = Types.TextAlign.Right;
            m_enemyLevelText = new TextObj(null);
            m_enemyLevelText.Font = Game.EnemyLevelFont;
            m_enemyHPBar = new SpriteObj("EnemyHPBar_Sprite");
            m_enemyHPBar.ForceDraw = true;
            m_enemyHPBarLength = m_enemyHPBar.SpriteRect.Width;
        }

        public void UpdateEnemyInfo(string enemyName, int enemyLevel, float enemyHPPercent) {
            m_blinkDurationCounter = 0f;
            m_blinkCounter = 0;
            m_enemyHPBar.Opacity = 1f;
            m_enemyLevelText.Opacity = 1f;
            m_enemyNameText.Opacity = 1f;
            base.Opacity = 1f;
            if (enemyName == null)
                enemyName = "Default Enemy";
            if (enemyName.Length > 17)
                enemyName = enemyName.Substring(0, 14) + "..";
            m_enemyNameText.Text = enemyName;
            m_enemyLevelText.Text = ((int)(enemyLevel * 2.75f)).ToString();
            m_enemyHPPercent = enemyHPPercent;
            if (enemyHPPercent <= 0f) {
                m_blinkCounter = m_blinkNumber;
                m_blinkDurationCounter = m_blinkDuration;
                m_opacity = 0.5f;
                m_enemyHPBar.Opacity = 0.5f;
                m_enemyLevelText.Opacity = 0.5f;
                m_enemyNameText.Opacity = 0.5f;
                base.Opacity = 0.5f;
            }
        }

        public void Update(GameTime gameTime) {
            if (m_blinkDurationCounter > 0f)
                m_blinkDurationCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (m_blinkCounter > 0 && m_blinkDurationCounter <= 0f) {
                if (m_opacity > 0f)
                    m_opacity = 0f;
                else
                    m_opacity = 0.5f;
                m_enemyHPBar.Opacity = m_opacity;
                m_enemyLevelText.Opacity = m_opacity;
                m_enemyNameText.Opacity = m_opacity;
                base.Opacity = m_opacity;
                m_blinkCounter--;
                m_blinkDurationCounter = m_blinkDuration;
            }
        }

        public override void Draw(Camera2D camera) {
            base.Draw(camera);
            m_enemyHPBar.Position = new Vector2(base.X + 8f, base.Y + 17f);
            m_enemyHPBar.SpriteRect = new Rectangle(m_enemyHPBar.SpriteRect.X, m_enemyHPBar.SpriteRect.Y, (int)(m_enemyHPBarLength * m_enemyHPPercent), m_enemyHPBar.SpriteRect.Height);
            m_enemyHPBar.Draw(camera);
            m_enemyNameText.Position = new Vector2(base.X + (float)this.Width - 5f, base.Y - 10f);
            m_enemyNameText.Draw(camera);
            m_enemyLevelText.Position = new Vector2(base.X + 22f, base.Y - 8f);
            m_enemyLevelText.Draw(camera);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_enemyNameText.Dispose();
                m_enemyNameText = null;
                m_enemyLevelText.Dispose();
                m_enemyLevelText = null;
                m_enemyHPBar.Dispose();
                m_enemyHPBar = null;
                base.Dispose();
            }
        }
    }
}
