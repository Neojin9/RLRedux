using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Tweener;
using Tweener.Ease;
using InputSystem;


namespace RogueCastle {

    public class BlacksmithScreen : Screen {

        private const int StartingCategoryIndex = 6;
        private ObjContainer[] _activeIconArray;
        private TextObj        _addPropertiesText;
        private TextObj        _addPropertiesTitleText;
        private ObjContainer   _blacksmithUI;
        private KeyIconTextObj _cancelText;
        private KeyIconTextObj _confirmText;
        private int            _currentCategoryIndex;
        private int            _currentEquipmentIndex;
        private TextObj        _equipmentDescriptionText;
        private TextObj        _equipmentTitleText;
        private SpriteObj      _equippedIcon;
        private bool           _inCategoryMenu = true;
        private bool           _lockControls;
        private List<ObjContainer[]> _masterIconArray;
        private KeyIconTextObj  _navigationText;
        private List<SpriteObj> _newIconList;
        private int             _newIconListIndex;
        private TextObj         _playerMoney;
        private Cue             _rainSound;
        private SpriteObj       _selectionIcon;
        private ObjContainer    _textInfoStatContainer;
        private ObjContainer    _textInfoStatModContainer;
        private ObjContainer    _textInfoTitleContainer;
        private ObjContainer    _unlockCostContainer;

        public BlacksmithScreen() {

            _currentCategoryIndex = 6;
            _masterIconArray = new List<ObjContainer[]>();

            for (int i = 0; i < 5; i++)
                _masterIconArray.Add(new ObjContainer[15]);

        }

        public float BackBufferOpacity { get; set; }
        public PlayerObj Player { get; set; }

        private int CurrentCategoryIndex {
            get { return _currentCategoryIndex - 6; }
        }

        public override void LoadContent() {

            _blacksmithUI = new ObjContainer("BlacksmithUI_Character");
            _blacksmithUI.Position = new Vector2(660f, 360f);
            
            _playerMoney = new TextObj(Game.GoldFont);
            _playerMoney.Align = Types.TextAlign.Left;
            _playerMoney.Text = "1000";
            _playerMoney.FontSize = 30f;
            _playerMoney.OverrideParentScale = true;
            _playerMoney.Position = new Vector2(210f, -225f);
            _playerMoney.AnchorY = 10f;
            
            _blacksmithUI.AddChild(_playerMoney);
            
            for (int i = 0; i < _blacksmithUI.NumChildren; i++)
                _blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;
            
            _selectionIcon = new SpriteObj("BlacksmithUI_SelectionIcon_Sprite");
            _selectionIcon.PlayAnimation();
            _selectionIcon.Scale = Vector2.Zero;
            _selectionIcon.AnimationDelay = 0.1f;
            _selectionIcon.ForceDraw = true;
            
            _equipmentDescriptionText = new TextObj(Game.JunicodeFont);
            _equipmentDescriptionText.Align = Types.TextAlign.Centre;
            _equipmentDescriptionText.FontSize = 12f;
            _equipmentDescriptionText.Position = new Vector2(230f, -20f);
            _equipmentDescriptionText.Text = "Select a category";
            _equipmentDescriptionText.WordWrap(190);
            _equipmentDescriptionText.Scale = Vector2.Zero;
            
            _blacksmithUI.AddChild(_equipmentDescriptionText);

            for (int index = 0; index < _masterIconArray.Count; index++) {

                ObjContainer[] icon = _masterIconArray[index];
                Vector2 absPosition = _blacksmithUI.GetChildAt(6).AbsPosition;
                absPosition.X += 85f;
                
                float x = absPosition.X;

                for (int j = 0; j < icon.Length; j++) {
                    
                    icon[j] = new ObjContainer("BlacksmithUI_QuestionMarkIcon_Character");
                    icon[j].Position = absPosition;
                    icon[j].Scale = Vector2.Zero;
                    icon[j].ForceDraw = true;
                    
                    absPosition.X += 70f;
                    
                    if (absPosition.X > x + 70f * 4f) {
                        absPosition.X = x;
                        absPosition.Y += 80f;
                    }

                }

            }

            InitializeTextObjs();

            _equippedIcon = new SpriteObj("BlacksmithUI_EquippedIcon_Sprite");
            
            _confirmText           = new KeyIconTextObj(Game.JunicodeFont);
            _confirmText.Text      = "to close map";
            _confirmText.FontSize  = 12f;
            _confirmText.Position  = new Vector2(50f, 550f);
            _confirmText.ForceDraw = true;
            
            _cancelText           = new KeyIconTextObj(Game.JunicodeFont);
            _cancelText.Text      = "to re-center on player";
            _cancelText.FontSize  = 12f;
            _cancelText.Position  = new Vector2(_confirmText.X, _confirmText.Y + 40f);
            _cancelText.ForceDraw = true;
            
            _navigationText           = new KeyIconTextObj(Game.JunicodeFont);
            _navigationText.Text      = "to move map";
            _navigationText.FontSize  = 12f;
            _navigationText.Position  = new Vector2(_confirmText.X, _confirmText.Y + 80f);
            _navigationText.ForceDraw = true;
            
            _newIconList = new List<SpriteObj>();
            
            for (int k = 0; k < 25; k++) {
                SpriteObj spriteObj = new SpriteObj("BlacksmithUI_NewIcon_Sprite");
                spriteObj.Visible = false;
                spriteObj.Scale = new Vector2(1.1f, 1.1f);
                _newIconList.Add(spriteObj);
            }

            base.LoadContent();

        }

        private void InitializeTextObjs() {

            _textInfoTitleContainer   = new ObjContainer();
            _textInfoStatContainer    = new ObjContainer();
            _textInfoStatModContainer = new ObjContainer();

            string[] array = new[] {
                "Health",
                "Mana",
                "Damage",
                "Magic",
                "Armor",
                "Weight"
            };
            
            Vector2 zero = Vector2.Zero;

            TextObj textObj   = new TextObj();
            textObj.Font      = Game.JunicodeFont;
            textObj.FontSize  = 10f;
            textObj.Text      = "0";
            textObj.ForceDraw = true;

            for (int i = 0; i < array.Length; i++) {

                textObj.Position = zero;
                _textInfoTitleContainer.AddChild(textObj.Clone() as TextObj);
                _textInfoStatContainer.AddChild(textObj.Clone() as TextObj);
                _textInfoStatModContainer.AddChild(textObj.Clone() as TextObj);
                (_textInfoTitleContainer.GetChildAt(i) as TextObj).Align = Types.TextAlign.Right;
                (_textInfoTitleContainer.GetChildAt(i) as TextObj).Text = array[i];
                zero.Y += (_textInfoTitleContainer.GetChildAt(i).Height - 5);

            }
            _addPropertiesTitleText              = new TextObj();
            _addPropertiesTitleText.Font         = Game.JunicodeFont;
            _addPropertiesTitleText.FontSize     = 8f;
            _addPropertiesTitleText.TextureColor = new Color(237, 202, 138);
            _addPropertiesTitleText.Text         = "Additional Properties:";
            
            _addPropertiesText          = new TextObj();
            _addPropertiesText.Font     = Game.JunicodeFont;
            _addPropertiesText.FontSize = 8f;
            
            _unlockCostContainer = new ObjContainer();
            
            TextObj textObj2 = new TextObj();
            textObj2.Font = Game.JunicodeFont;
            textObj2.FontSize = 10f;
            textObj2.TextureColor = Color.Yellow;
            textObj2.Position = new Vector2(50f, 9f);
            
            _unlockCostContainer.AddChild(new SpriteObj("BlacksmithUI_CoinBG_Sprite"));
            _unlockCostContainer.AddChild(textObj2);
            
            _equipmentTitleText = new TextObj(Game.JunicodeFont);
            _equipmentTitleText.ForceDraw = true;
            _equipmentTitleText.FontSize = 12f;
            _equipmentTitleText.DropShadow = new Vector2(2f, 2f);
            _equipmentTitleText.TextureColor = new Color(237, 202, 138);
            
            _textInfoTitleContainer.Position   = new Vector2(_blacksmithUI.X + 205f, _blacksmithUI.Y - _blacksmithUI.Height / 2 + 45f);
            _textInfoStatContainer.Position    = new Vector2(_textInfoTitleContainer.X + 15f, _textInfoTitleContainer.Y);
            _textInfoStatModContainer.Position = new Vector2(_textInfoStatContainer.X + 75f, _textInfoStatContainer.Y);
            _addPropertiesTitleText.Position   = new Vector2(_blacksmithUI.X + 140f, (_textInfoStatModContainer.Bounds.Bottom + 5));
            _addPropertiesText.Position        = new Vector2(_addPropertiesTitleText.X, _addPropertiesTitleText.Bounds.Bottom);
            _unlockCostContainer.Position      = new Vector2(_blacksmithUI.X + 114f, 485f);
            _equipmentTitleText.Position       = new Vector2(_blacksmithUI.X + 140f, _textInfoTitleContainer.Y - 45f);
            
            _textInfoTitleContainer.Visible   = false;
            _textInfoStatContainer.Visible    = false;
            _textInfoStatModContainer.Visible = false;
            _addPropertiesTitleText.Visible   = false;
            _addPropertiesText.Visible        = false;
            _unlockCostContainer.Visible      = false;
            _equipmentTitleText.Visible       = false;

        }

        private void DisplayCategory(int equipmentType) {

            const float duration = 0.2f;
            float num = 0f;

            if (_activeIconArray != null) {

                for (int i = 0; i < 15; i++) {

                    Tween.StopAllContaining(_activeIconArray[i], false);
                    Tween.To(_activeIconArray[i], duration, Back.EaseIn, new[] {
                        "delay",
                        num.ToString(),
                        "ScaleX",
                        "0",
                        "ScaleY",
                        "0"
                    });

                }

            }

            _activeIconArray = _masterIconArray[equipmentType];
            num = 0.2f;
            
            for (int j = 0; j < 15; j++) {

                Tween.To(_activeIconArray[j], duration, Back.EaseOut, new[] {
                    "delay",
                    num.ToString(),
                    "ScaleX",
                    "1",
                    "ScaleY",
                    "1"
                });

            }

            for (int index = 0; index < _newIconList.Count; index++) {
                
                SpriteObj spriteObj = _newIconList[index];
                Tween.StopAllContaining(spriteObj, false);
                spriteObj.Scale = Vector2.Zero;
                
                Tween.To(spriteObj, duration, Back.EaseOut, new[] {
                    "delay",
                    num.ToString(),
                    "ScaleX",
                    "1",
                    "ScaleY",
                    "1"
                });

            }

            UpdateNewIcons();
            
            _equippedIcon.Scale = Vector2.Zero;
            
            Tween.StopAllContaining(_equippedIcon, false);
            
            Tween.To(_equippedIcon, duration, Back.EaseOut, new[] {
                "delay",
                num.ToString(),
                "ScaleX",
                "1",
                "ScaleY",
                "1"
            });

        }

        public void EaseInMenu() {

            const float duration = 0.4f;
            
            Tween.To(_blacksmithUI.GetChildAt(0), duration, Back.EaseOut, new[] {
                "ScaleX",
                "1",
                "ScaleY",
                "1"
            });

            Tween.To(_selectionIcon, duration, Back.EaseOut, new[] {
                "delay",
                "0.25",
                "ScaleX",
                "1",
                "ScaleY",
                "1"
            });

            float num = 0.2f;

            for (int i = 6; i < _blacksmithUI.NumChildren - 3; i++) {
                
                num += 0.05f;
                
                Tween.To(_blacksmithUI.GetChildAt(i), duration, Back.EaseOut, new[] {
                    "delay",
                    num.ToString(),
                    "ScaleX",
                    "1",
                    "ScaleY",
                    "1"
                });

            }

            Tween.To(_blacksmithUI.GetChildAt(_blacksmithUI.NumChildren - 1), duration, Back.EaseOut, new[] {
                "delay",
                num.ToString(),
                "ScaleX",
                "1",
                "ScaleY",
                "1"
            });

            Tween.To(_blacksmithUI.GetChildAt(_blacksmithUI.NumChildren - 2), duration, Back.EaseOut, new[] {
                "delay",
                num.ToString(),
                "ScaleX",
                "1",
                "ScaleY",
                "1"
            });

            Tween.To(_blacksmithUI.GetChildAt(_blacksmithUI.NumChildren - 3), duration, Back.EaseOut, new[] {
                "delay",
                num.ToString(),
                "ScaleX",
                "1",
                "ScaleY",
                "1"
            });

            Tween.AddEndHandlerToLastTween(this, "EaseInComplete", new object[0]);

        }

        public void EaseInComplete() {
            _lockControls = false;
        }

        private void EaseOutMenu() {

            for (int index = 0; index < _newIconList.Count; index++)
                _newIconList[index].Visible = false;

            _equippedIcon.Visible = false;

            Tween.To(_confirmText, 0.2f, Linear.EaseNone, new[] {
                "Opacity",
                "0"
            });

            Tween.To(_cancelText, 0.2f, Linear.EaseNone, new[] {
                "Opacity",
                "0"
            });

            Tween.To(_navigationText, 0.2f, Linear.EaseNone, new[] {
                "Opacity",
                "0"
            });

            const float duration = 0.4f;
            float num2 = 0f;
            
            Tween.To(_blacksmithUI.GetChildAt(_blacksmithUI.NumChildren - 2), duration, Back.EaseIn, new[] {
                "ScaleX",
                "0",
                "ScaleY",
                "0"
            });

            Tween.To(_blacksmithUI.GetChildAt(_blacksmithUI.NumChildren - 3), duration, Back.EaseIn, new[] {
                "ScaleX",
                "0",
                "ScaleY",
                "0"
            });

            Tween.To(_blacksmithUI.GetChildAt(_blacksmithUI.NumChildren - 4), duration, Back.EaseIn, new[] {
                "ScaleX",
                "0",
                "ScaleY",
                "0"
            });

            for (int i = 6; i < 11; i++) {

                if (_currentCategoryIndex == i) {

                    Tween.To(_selectionIcon, duration, Back.EaseIn, new[] {
                        "delay",
                        num2.ToString(),
                        "ScaleX",
                        "0",
                        "ScaleY",
                        "0"
                    });

                }

                Tween.To(_blacksmithUI.GetChildAt(i), duration, Back.EaseIn, new[] {
                    "delay",
                    num2.ToString(),
                    "ScaleX",
                    "0",
                    "ScaleY",
                    "0"
                });

                num2 += 0.05f;

            }

            for (int j = 1; j < 6; j++)
                _blacksmithUI.GetChildAt(j).Scale = Vector2.Zero;
            
            for (int k = 0; k < _activeIconArray.Length; k++) {

                Tween.To(_activeIconArray[k], duration, Back.EaseIn, new[] {
                    "ScaleX",
                    "0",
                    "ScaleY",
                    "0"
                });

            }

            Tween.To(_blacksmithUI.GetChildAt(0), duration, Back.EaseIn, new[] {
                "delay",
                "0.3",
                "ScaleX",
                "0",
                "ScaleY",
                "0"
            });

            Tween.RunFunction(duration + 0.35f, ScreenManager, "HideCurrentScreen", new object[0]);

        }

        private void UpdateIconStates() {

            for (int i = 0; i < Game.PlayerStats.GetBlueprintArray.Count; i++) {

                for (int j = 0; j < Game.PlayerStats.GetBlueprintArray[i].Length; j++) {

                    byte blueprint = Game.PlayerStats.GetBlueprintArray[i][j];
                    
                    if (blueprint == 0)
                        _masterIconArray[i][j].ChangeSprite("BlacksmithUI_QuestionMarkIcon_Character");
                    else {
                        
                        _masterIconArray[i][j].ChangeSprite("BlacksmithUI_" + EquipmentCategoryType.ToString(i) + (j % 5 + 1).ToString() + "Icon_Character");
                        
                        for (int k = 1; k < _masterIconArray[i][j].NumChildren; k++)
                            _masterIconArray[i][j].GetChildAt(k).Opacity = 0.2f;

                    }
                    
                    if (blueprint > 2) {
                        
                        for (int l = 1; l < _masterIconArray[i][j].NumChildren; l++)
                            _masterIconArray[i][j].GetChildAt(l).Opacity = 1f;
                        
                        int num = 1;
                        
                        if (i == 0)
                            num = 2;
                        
                        EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(i, j);
                        _masterIconArray[i][j].GetChildAt(num).TextureColor = equipmentData.FirstColour;
                        
                        if (i != 4) {
                            num++;
                            _masterIconArray[i][j].GetChildAt(num).TextureColor = equipmentData.SecondColour;
                        }

                    }

                }

            }

        }

        private void UpdateNewIcons() {

            if (Player != null) {

                if (Player.CurrentMana > Player.MaxMana)
                    Player.CurrentMana = Player.MaxMana;
                
                if (Player.CurrentHealth > Player.MaxHealth)
                    Player.CurrentHealth = Player.MaxHealth;

            }

            UpdateMoneyText();
            _newIconListIndex = 0;

            for (int index = 0; index < _newIconList.Count; index++)
                _newIconList[index].Visible = false;

            for (int i = 0; i < Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex].Length; i++) {
                
                byte blueprint = Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][i];
                
                if (blueprint == 1) {

                    SpriteObj spriteObj = _newIconList[_newIconListIndex];
                    spriteObj.Visible = true;
                    spriteObj.Position = _masterIconArray[CurrentCategoryIndex][i].AbsPosition;
                    spriteObj.X -= 20f;
                    spriteObj.Y -= 30f;

                    _newIconListIndex++;

                }

            }

            sbyte b2 = Game.PlayerStats.GetEquippedArray[CurrentCategoryIndex];

            if (b2 > -1) {
                _equippedIcon.Position = new Vector2(_activeIconArray[b2].AbsPosition.X + 18f, _activeIconArray[b2].AbsPosition.Y + 18f);
                _equippedIcon.Visible = true;
                return;
            }

            _equippedIcon.Visible = false;

        }

        public override void OnEnter() {

            if (_rainSound != null)
                _rainSound.Dispose();
            
            if (DateTime.Now.Month != 12 && DateTime.Now.Month != 1)
                _rainSound = SoundManager.PlaySound("Rain1_Filtered");
            else
                _rainSound = SoundManager.PlaySound("snowloop_filtered");
            
            if (Game.PlayerStats.TotalBlueprintsFound >= 75)
                GameUtil.UnlockAchievement("FEAR_OF_THROWING_STUFF_OUT");
            
            _lockControls = true;
            SoundManager.PlaySound("ShopMenuOpen");
            _confirmText.Opacity = 0f;
            _cancelText.Opacity = 0f;
            _navigationText.Opacity = 0f;
            
            Tween.To(_confirmText, 0.2f, Linear.EaseNone, new[] {
                "Opacity",
                "1"
            });

            Tween.To(_cancelText, 0.2f, Linear.EaseNone, new[] {
                "Opacity",
                "1"
            });

            Tween.To(_navigationText, 0.2f, Linear.EaseNone, new[] {
                "Opacity",
                "1"
            });

            _confirmText.Text = "[Input:" + 0 + "]  select/equip";
            _cancelText.Text = "[Input:" + 2 + "]  cancel/close menu";
            
            if (!InputManager.GamePadIsConnected(PlayerIndex.One))
                _navigationText.Text = "Arrow keys to navigate";
            else
                _navigationText.Text = "[Button:LeftStick] to navigate";
            
            _currentEquipmentIndex = 0;
            _inCategoryMenu = true;
            _selectionIcon.Position = _blacksmithUI.GetChildAt(6).AbsPosition;
            _currentCategoryIndex = 6;
            
            UpdateIconStates();
            DisplayCategory(0);
            EaseInMenu();
            
            Tween.To(this, 0.2f, Linear.EaseNone, new[] {
                "BackBufferOpacity",
                "0.5"
            });

            UpdateIconSelectionText();
            base.OnEnter();

        }

        public override void OnExit() {

            if (_rainSound != null)
                _rainSound.Stop(AudioStopOptions.Immediate);
            
            for (int i = 0; i < _blacksmithUI.NumChildren; i++)
                _blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;

            for (int index = 0; index < _masterIconArray.Count; index++) {
                
                ObjContainer[] icon = _masterIconArray[index];
                
                for (int j = 0; j < icon.Length; j++)
                    icon[j].Scale = Vector2.Zero;

            }

            _selectionIcon.Scale = Vector2.Zero;
            Player.CurrentHealth = Player.MaxHealth;
            Player.CurrentMana = Player.MaxMana;
            
            (ScreenManager.Game as Game).SaveManager.SaveFiles(new[] {
                SaveType.PlayerData,
                SaveType.UpgradeData
            });

            bool flag = true;
            sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
            
            for (int k = 0; k < getEquippedArray.Length; k++) {
                
                sbyte b = getEquippedArray[k];
                
                if (b == -1) {
                    flag = false;
                    break;
                }

            }

            if (flag)
                GameUtil.UnlockAchievement("FEAR_OF_NUDITY");

            base.OnExit();

        }

        public override void HandleInput() {

            if (!_lockControls) {

                if (_inCategoryMenu)
                    CategorySelectionInput();
                else
                    EquipmentSelectionInput();

            }

            base.HandleInput();

        }

        private void CategorySelectionInput() {

            int currentCategoryIndex = _currentCategoryIndex;
            
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)) {
                
                _currentCategoryIndex--;
                
                if (_currentCategoryIndex < 6)
                    _currentCategoryIndex = 10;

            }
            else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19)) {
                
                _currentCategoryIndex++;
                
                if (_currentCategoryIndex > 10)
                    _currentCategoryIndex = 6;

            }

            if (currentCategoryIndex != _currentCategoryIndex) {
                
                SoundManager.PlaySound("ShopBSMenuMove");
                _selectionIcon.Position = _blacksmithUI.GetChildAt(_currentCategoryIndex).AbsPosition;
                
                for (int i = 1; i < 6; i++) {

                    if (i == 1)
                        _blacksmithUI.GetChildAt(i).Scale = new Vector2(1f, 1f);
                    else
                        _blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;

                }

                if (_currentCategoryIndex != 6)
                    _blacksmithUI.GetChildAt(_currentCategoryIndex - 5).Scale = new Vector2(1f, 1f);
                else
                    _blacksmithUI.GetChildAt(_currentCategoryIndex - 5).Scale = Vector2.Zero;
                
                DisplayCategory(_currentCategoryIndex - 6);

            }

            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)) {
                
                _lockControls = true;
                
                Tween.To(this, 0.2f, Linear.EaseNone, new[] {
                    "delay",
                    "0.5",
                    "BackBufferOpacity",
                    "0"
                });

                EaseOutMenu();
                
                Tween.RunFunction(0.13f, typeof(SoundManager), "PlaySound", new object[] {
                    "ShopMenuClose"
                });

            }

            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1)) {
                
                _inCategoryMenu = false;
                _currentEquipmentIndex = 0;
                _selectionIcon.Position = _activeIconArray[_currentEquipmentIndex].AbsPosition;
                byte b = Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][_currentEquipmentIndex];
                
                if (b == 1)
                    Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][_currentEquipmentIndex] = 2;
                
                UpdateNewIcons();
                UpdateIconSelectionText();
                SoundManager.PlaySound("ShopMenuConfirm");

            }

        }

        private void EquipmentSelectionInput() {
            
            int currentEquipmentIndex = _currentEquipmentIndex;
            
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)) {
                _currentEquipmentIndex -= 5;
                if (_currentEquipmentIndex < 0)
                    _currentEquipmentIndex += 15;
            }

            if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19)) {
                _currentEquipmentIndex += 5;
                if (_currentEquipmentIndex > 14)
                    _currentEquipmentIndex -= 15;
            }

            if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21)) {
                _currentEquipmentIndex--;
                if ((_currentEquipmentIndex + 1) % 5 == 0)
                    _currentEquipmentIndex += 5;
            }

            if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23)) {
                _currentEquipmentIndex++;
                if (_currentEquipmentIndex % 5 == 0)
                    _currentEquipmentIndex -= 5;
            }

            if (currentEquipmentIndex != _currentEquipmentIndex) {
                
                byte b = Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][_currentEquipmentIndex];
                
                if (b == 1)
                    Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][_currentEquipmentIndex] = 2;
                
                UpdateNewIcons();
                UpdateIconSelectionText();
                _selectionIcon.Position = _activeIconArray[_currentEquipmentIndex].AbsPosition;
                SoundManager.PlaySound("ShopBSMenuMove");

            }

            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)) {
                SoundManager.PlaySound("ShopMenuCancel");
                _inCategoryMenu = true;
                _selectionIcon.Position = _blacksmithUI.GetChildAt(_currentCategoryIndex).AbsPosition;
                UpdateIconSelectionText();
            }

            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1)) {
                
                int num = _currentCategoryIndex - 6;
                int num2 = Game.PlayerStats.GetBlueprintArray[num][_currentEquipmentIndex];
                int num3 = Game.PlayerStats.GetEquippedArray[num];
                
                if (num2 < 3 && num2 > 0) {

                    EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(num, _currentEquipmentIndex);
                    
                    if (Game.PlayerStats.Gold >= equipmentData.Cost) {

                        SoundManager.PlaySound("ShopMenuUnlock");
                        Game.PlayerStats.Gold -= equipmentData.Cost;
                        Game.PlayerStats.GetBlueprintArray[num][_currentEquipmentIndex] = 3;
                        ObjContainer objContainer = _masterIconArray[num][_currentEquipmentIndex];
                        objContainer.ChangeSprite("BlacksmithUI_" + EquipmentCategoryType.ToString(num) + (_currentEquipmentIndex % 5 + 1).ToString() + "Icon_Character");
                        
                        for (int i = 1; i < objContainer.NumChildren; i++)
                            objContainer.GetChildAt(i).Opacity = 1f;
                        
                        int num4 = 1;
                        
                        if (num == 0)
                            num4 = 2;
                        
                        objContainer.GetChildAt(num4).TextureColor = equipmentData.FirstColour;
                        
                        if (num != 4) {
                            num4++;
                            objContainer.GetChildAt(num4).TextureColor = equipmentData.SecondColour;
                        }

                        num2 = 3;
                        UpdateIconSelectionText();

                    }
                    else
                        SoundManager.PlaySound("ShopMenuUnlockFail");

                }

                if (num3 != _currentEquipmentIndex && num2 == 3) {

                    EquipmentData equipmentData2 = Game.EquipmentSystem.GetEquipmentData(num, _currentEquipmentIndex);
                    int num5 = Game.PlayerStats.GetEquippedArray[num];
                    int num6 = 0;
                    
                    if (num5 != -1)
                        num6 = Game.EquipmentSystem.GetEquipmentData(num, num5).Weight;
                    
                    if (equipmentData2.Weight + Player.CurrentWeight - num6 <= Player.MaxWeight) {

                        SoundManager.PlaySound("ShopBSEquip");
                        Game.PlayerStats.GetEquippedArray[num] = (sbyte)_currentEquipmentIndex;
                        UpdateIconSelectionText();
                        Vector3 partIndices = PlayerPart.GetPartIndices(num);
                        
                        if (partIndices.X != -1f)
                            Player.GetChildAt((int)partIndices.X).TextureColor = equipmentData2.FirstColour;
                        
                        if (partIndices.Y != -1f)
                            Player.GetChildAt((int)partIndices.Y).TextureColor = equipmentData2.SecondColour;
                        
                        if (partIndices.Z != -1f)
                            Player.GetChildAt((int)partIndices.Z).TextureColor = equipmentData2.SecondColour;
                        
                        if (num == 2 && partIndices.X != -1f)
                            Player.GetChildAt(5).TextureColor = equipmentData2.FirstColour;
                        
                        UpdateNewIcons();
                        return;

                    }

                    Console.WriteLine("cannot equip. too heavy. Weight:" + (equipmentData2.Weight + Player.CurrentWeight - num6));
                    return;
                }

                if (num3 == _currentEquipmentIndex) {
                    Game.PlayerStats.GetEquippedArray[num] = -1;
                    Player.UpdateEquipmentColours();
                    UpdateIconSelectionText();
                    UpdateNewIcons();
                }

            }

        }

        private void UpdateIconSelectionText() {

            _equipmentDescriptionText.Position = new Vector2(-1000f, -1000f);
            _textInfoTitleContainer.Visible    = false;
            _textInfoStatContainer.Visible     = false;
            _textInfoStatModContainer.Visible  = false;
            _addPropertiesTitleText.Visible    = false;
            _addPropertiesText.Visible         = false;
            _unlockCostContainer.Visible       = false;
            _equipmentTitleText.Visible        = false;
            
            if (_inCategoryMenu) {
                _equipmentDescriptionText.Text = "Select a category";
                return;
            }
            
            if (Game.PlayerStats.GetBlueprintArray[_currentCategoryIndex - 6][_currentEquipmentIndex] == 0) {
                _equipmentDescriptionText.Position = new Vector2(230f, -20f);
                _equipmentDescriptionText.Text = "Blueprint needed";
                return;
            }
            
            if (Game.PlayerStats.GetBlueprintArray[_currentCategoryIndex - 6][_currentEquipmentIndex] < 3) {
                
                _equipmentDescriptionText.Text    = "Purchase Info Here";
                (_unlockCostContainer.GetChildAt(1) as TextObj).Text = Game.EquipmentSystem.GetEquipmentData(_currentCategoryIndex - 6, _currentEquipmentIndex).Cost.ToString() + " to unlock";
                _unlockCostContainer.Visible      = true;
                _textInfoTitleContainer.Visible   = true;
                _textInfoStatContainer.Visible    = true;
                _textInfoStatModContainer.Visible = true;
                _addPropertiesTitleText.Visible   = true;
                _addPropertiesText.Visible        = true;
                _equipmentTitleText.Visible       = true;
                _textInfoTitleContainer.Opacity   = 0.5f;
                _textInfoStatContainer.Opacity    = 0.5f;
                _textInfoStatModContainer.Opacity = 0.5f;
                _addPropertiesTitleText.Opacity   = 0.5f;
                _addPropertiesText.Opacity        = 0.5f;
                _equipmentTitleText.Opacity       = 0.5f;
                
                UpdateEquipmentDataText();
                
                return;

            }

            _textInfoTitleContainer.Visible   = true;
            _textInfoStatContainer.Visible    = true;
            _textInfoStatModContainer.Visible = true;
            _addPropertiesTitleText.Visible   = true;
            _addPropertiesText.Visible        = true;
            _equipmentTitleText.Visible       = true;
            _textInfoTitleContainer.Opacity   = 1f;
            _textInfoStatContainer.Opacity    = 1f;
            _textInfoStatModContainer.Opacity = 1f;
            _addPropertiesTitleText.Opacity   = 1f;
            _addPropertiesText.Opacity        = 1f;
            _equipmentTitleText.Opacity       = 1f;

            UpdateEquipmentDataText();

        }

        private void UpdateEquipmentDataText() {

            (_textInfoStatContainer.GetChildAt(0) as TextObj).Text = Player.MaxHealth.ToString();
            (_textInfoStatContainer.GetChildAt(1) as TextObj).Text = Player.MaxMana.ToString();
            (_textInfoStatContainer.GetChildAt(2) as TextObj).Text = Player.Damage.ToString();
            (_textInfoStatContainer.GetChildAt(3) as TextObj).Text = Player.TotalMagicDamage.ToString();
            (_textInfoStatContainer.GetChildAt(4) as TextObj).Text = Player.TotalArmor.ToString();
            (_textInfoStatContainer.GetChildAt(5) as TextObj).Text = Player.CurrentWeight.ToString() + "/" + Player.MaxWeight.ToString();
            
            int num = _currentCategoryIndex - 6;
            EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(num, _currentEquipmentIndex);
            int num2 = Game.PlayerStats.GetEquippedArray[num];
            EquipmentData equipmentData2 = new EquipmentData();
            
            if (num2 > -1)
                equipmentData2 = Game.EquipmentSystem.GetEquipmentData(num, num2);
            
            bool flag = Game.PlayerStats.GetEquippedArray[CurrentCategoryIndex] == _currentEquipmentIndex;
            int num3 = equipmentData.BonusHealth - equipmentData2.BonusHealth;
            
            if (flag)
                num3 = -equipmentData.BonusHealth;
            
            TextObj textObj = _textInfoStatModContainer.GetChildAt(0) as TextObj;
            
            if (num3 > 0) {
                textObj.TextureColor = Color.Cyan;
                textObj.Text = "+" + num3.ToString();
            }
            else if (num3 < 0) {
                textObj.TextureColor = Color.Red;
                textObj.Text = num3.ToString();
            }
            else
                textObj.Text = "";
            
            TextObj textObj2 = _textInfoStatModContainer.GetChildAt(1) as TextObj;
            int num4 = equipmentData.BonusMana - equipmentData2.BonusMana;
            
            if (flag)
                num4 = -equipmentData.BonusMana;
            
            if (num4 > 0) {
                textObj2.TextureColor = Color.Cyan;
                textObj2.Text = "+" + num4.ToString();
            }
            else if (num4 < 0) {
                textObj2.TextureColor = Color.Red;
                textObj2.Text = num4.ToString();
            }
            else
                textObj2.Text = "";
            
            TextObj textObj3 = _textInfoStatModContainer.GetChildAt(2) as TextObj;
            int num5 = equipmentData.BonusDamage - equipmentData2.BonusDamage;
            
            if (flag)
                num5 = -equipmentData.BonusDamage;
            
            if (num5 > 0) {
                textObj3.TextureColor = Color.Cyan;
                textObj3.Text = "+" + num5.ToString();
            }
            else if (num5 < 0) {
                textObj3.TextureColor = Color.Red;
                textObj3.Text = num5.ToString();
            }
            else
                textObj3.Text = "";

            TextObj textObj4 = _textInfoStatModContainer.GetChildAt(3) as TextObj;
            int num6 = equipmentData.BonusMagic - equipmentData2.BonusMagic;
            
            if (flag)
                num6 = -equipmentData.BonusMagic;
            
            if (num6 > 0) {
                textObj4.TextureColor = Color.Cyan;
                textObj4.Text = "+" + num6.ToString();
            }
            else if (num6 < 0) {
                textObj4.TextureColor = Color.Red;
                textObj4.Text = num6.ToString();
            }
            else
                textObj4.Text = "";

            TextObj textObj5 = _textInfoStatModContainer.GetChildAt(4) as TextObj;
            int num7 = equipmentData.BonusArmor - equipmentData2.BonusArmor;
            
            if (flag)
                num7 = -equipmentData.BonusArmor;

            if (num7 > 0) {
                textObj5.TextureColor = Color.Cyan;
                textObj5.Text = "+" + num7.ToString();
            }
            else if (num7 < 0) {
                textObj5.TextureColor = Color.Red;
                textObj5.Text = num7.ToString();
            }
            else
                textObj5.Text = "";

            TextObj textObj6 = _textInfoStatModContainer.GetChildAt(5) as TextObj;
            int num8 = equipmentData.Weight - equipmentData2.Weight;
            
            if (flag)
                num8 = -equipmentData.Weight;
            
            if (num8 > 0) {
                textObj6.TextureColor = Color.Red;
                textObj6.Text = "+" + num8.ToString();
            }
            else if (num8 < 0) {
                textObj6.TextureColor = Color.Cyan;
                textObj6.Text = num8.ToString();
            }
            else
                textObj6.Text = "";

            Vector2[] secondaryAttribute = equipmentData.SecondaryAttribute;
            _addPropertiesText.Text = "";
            
            if (secondaryAttribute != null) {

                Vector2[] array = secondaryAttribute;

                for (int i = 0; i < array.Length; i++) {

                    Vector2 vector = array[i];

                    if (vector.X != 0f) {

                        if (vector.X < 7f) {

                            TextObj expr_4FE = _addPropertiesText;
                            string text = expr_4FE.Text;

                            expr_4FE.Text = string.Concat(new[] {
                                text,
                                "+",
                                (vector.Y * 100f).ToString(),
                                "% ",
                                EquipmentSecondaryDataType.ToString((int)vector.X),
                                "\n"
                            });

                        }
                        else {

                            TextObj expr_56E = _addPropertiesText;
                            string text2 = expr_56E.Text;
                            string[] array2 = new string[6];
                            array2[0] = text2;
                            array2[1] = "+";
                            string[] arg_5A0_0 = array2;
                            int arg_5A0_1 = 2;
                            float y = vector.Y;
                            arg_5A0_0[arg_5A0_1] = y.ToString();
                            array2[3] = " ";
                            array2[4] = EquipmentSecondaryDataType.ToString((int)vector.X);
                            array2[5] = "\n";
                            expr_56E.Text = string.Concat(array2);

                        }

                    }

                }

                if (secondaryAttribute.Length == 0)
                    _addPropertiesText.Text = "None";

            }
            else
                _addPropertiesText.Text = "None";

            _equipmentTitleText.Text = EquipmentBaseType.ToString(_currentEquipmentIndex) + " " + EquipmentCategoryType.ToString(num);

        }

        private void UpdateMoneyText() {

            _playerMoney.Text = Game.PlayerStats.Gold.ToString();
            ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
            
            if (levelScreen != null)
                levelScreen.UpdatePlayerHUD();

        }

        public override void Draw(GameTime gameTime) {

            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            _blacksmithUI.Draw(Camera);
            _selectionIcon.Draw(Camera);
            _textInfoTitleContainer.Draw(Camera);
            _textInfoStatContainer.Draw(Camera);
            _textInfoStatModContainer.Draw(Camera);
            _addPropertiesTitleText.Draw(Camera);
            _addPropertiesText.Draw(Camera);
            _unlockCostContainer.Draw(Camera);
            _equipmentTitleText.Draw(Camera);

            for (int index = 0; index < _masterIconArray.Count; index++) {
                ObjContainer[] icon = _masterIconArray[index];
                for (int i = 0; i < icon.Length; i++)
                    icon[i].Draw(Camera);
            }

            _navigationText.Draw(Camera);
            _cancelText.Draw(Camera);
            _confirmText.Draw(Camera);
            _equippedIcon.Draw(Camera);

            for (int index = 0; index < _newIconList.Count; index++)
                _newIconList[index].Draw(Camera);

            Camera.End();
            base.Draw(gameTime);

        }

        public override void Dispose() {

            if (!IsDisposed) {

                Console.WriteLine("Disposing Blacksmith Screen");
                
                if (_rainSound != null)
                    _rainSound.Dispose();

                _rainSound = null;
                _blacksmithUI.Dispose();
                _blacksmithUI = null;
                _equipmentDescriptionText.Dispose();
                _equipmentDescriptionText = null;
                _selectionIcon.Dispose();
                _selectionIcon = null;
                _equipmentTitleText.Dispose();
                _equipmentTitleText = null;
                _activeIconArray = null;

                for (int index = 0; index < _masterIconArray.Count; index++) {

                    ObjContainer[] icon = _masterIconArray[index];

                    for (int i = 0; i < icon.Length; i++) {
                        icon[i].Dispose();
                        icon[i] = null;
                    }

                    Array.Clear(icon, 0, icon.Length);

                }

                _masterIconArray.Clear();
                _masterIconArray = null;
                _textInfoStatContainer.Dispose();
                _textInfoStatContainer = null;
                _textInfoTitleContainer.Dispose();
                _textInfoTitleContainer = null;
                _textInfoStatModContainer.Dispose();
                _textInfoStatModContainer = null;
                _unlockCostContainer.Dispose();
                _unlockCostContainer = null;
                _addPropertiesText.Dispose();
                _addPropertiesText = null;
                _addPropertiesTitleText.Dispose();
                _addPropertiesTitleText = null;
                _equippedIcon.Dispose();
                _equippedIcon = null;
                Player = null;
                _confirmText.Dispose();
                _confirmText = null;
                _cancelText.Dispose();
                _cancelText = null;
                _navigationText.Dispose();
                _navigationText = null;
                _playerMoney = null;

                for (int index = 0; index < _newIconList.Count; index++)
                    _newIconList[index].Dispose();

                _newIconList.Clear();
                _newIconList = null;
                
                base.Dispose();

            }

        }

    }

}
