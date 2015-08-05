using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class SkillObj : SpriteObj {
        private TextObj LevelText;
        private SpriteObj m_coinIcon;
        private int m_currentLevel;

        public SkillObj(string spriteName) : base(spriteName) {
            StatType = 0;
            DisplayStat = false;
            base.Visible = false;
            base.ForceDraw = true;
            LevelText = new TextObj(Game.JunicodeFont);
            LevelText.FontSize = 10f;
            LevelText.Align = Types.TextAlign.Centre;
            LevelText.OutlineWidth = 2;
            InputDescription = "";
            base.OutlineWidth = 2;
            m_coinIcon = new SpriteObj("UpgradeIcon_Sprite");
        }

        public string Description { get; set; }
        public string InputDescription { get; set; }
        public float PerLevelModifier { get; set; }
        public int BaseCost { get; set; }
        public int Appreciation { get; set; }
        public int MaxLevel { get; set; }
        public SkillType TraitType { get; set; }
        public string IconName { get; set; }
        public string UnitOfMeasurement { get; set; }
        public byte StatType { get; set; }
        public bool DisplayStat { get; set; }

        public int CurrentLevel {
            get { return m_currentLevel; }
            set {
                if (value > MaxLevel) {
                    m_currentLevel = MaxLevel;
                    return;
                }
                m_currentLevel = value;
            }
        }

        public int TotalCost {
            get { return BaseCost + CurrentLevel * Appreciation + 10 * Game.PlayerStats.CurrentLevel; }
        }

        public float ModifierAmount {
            get { return CurrentLevel * PerLevelModifier; }
        }

        public override void Draw(Camera2D camera) {
            if (base.Opacity > 0f) {
                float opacity = base.Opacity;
                base.TextureColor = Color.Black;
                base.Opacity = 0.5f;
                base.X += 8f;
                base.Y += 8f;
                base.Draw(camera);
                base.X -= 8f;
                base.Y -= 8f;
                base.TextureColor = Color.White;
                base.Opacity = opacity;
            }
            base.Draw(camera);
            LevelText.Position = new Vector2(base.X, (this.Bounds.Bottom - LevelText.Height / 2));
            LevelText.Text = CurrentLevel + "/" + MaxLevel;
            LevelText.Opacity = base.Opacity;
            if (CurrentLevel >= MaxLevel) {
                LevelText.TextureColor = Color.Yellow;
                LevelText.Text = "Max";
            }
            else
                LevelText.TextureColor = Color.White;
            LevelText.Draw(camera);
            if (Game.PlayerStats.Gold >= TotalCost && CurrentLevel < MaxLevel) {
                m_coinIcon.Opacity = base.Opacity;
                m_coinIcon.Position = new Vector2(base.X + 18f, base.Y - 40f);
                m_coinIcon.Draw(camera);
            }
        }

        protected override GameObj CreateCloneInstance() {
            return new SkillObj(this._spriteName);
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            SkillObj skillObj = obj as SkillObj;
            skillObj.Description = Description;
            skillObj.PerLevelModifier = PerLevelModifier;
            skillObj.BaseCost = BaseCost;
            skillObj.Appreciation = Appreciation;
            skillObj.MaxLevel = MaxLevel;
            skillObj.CurrentLevel = CurrentLevel;
            skillObj.TraitType = TraitType;
            skillObj.InputDescription = InputDescription;
            skillObj.UnitOfMeasurement = UnitOfMeasurement;
            skillObj.StatType = StatType;
            skillObj.DisplayStat = DisplayStat;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                LevelText.Dispose();
                LevelText = null;
                m_coinIcon.Dispose();
                m_coinIcon = null;
                base.Dispose();
            }
        }
    }
}
