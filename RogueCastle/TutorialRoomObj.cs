using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;


namespace RogueCastle {

    public class TutorialRoomObj : RoomObj {

        private int _creditsIndex;
        private Vector2 _creditsPosition;
        private TextObj _creditsText;
        private string[] _creditsTextList;
        private string[] _creditsTextTitleList;
        private TextObj _creditsTitleText;
        private SpriteObj _diary;
        private DoorObj _door;
        private SpriteObj _doorSprite;
        private SpriteObj _speechBubble;
        private string[] _tutorialControllerTextList;
        private KeyIconTextObj _tutorialText;
        private string[] _tutorialTextList;
        private int _waypointIndex;
        private List<GameObj> _waypointList;

        public TutorialRoomObj() {
            _waypointList = new List<GameObj>();
        }

        public override void Initialize() {

            for (int index = 0; index < GameObjList.Count; index++) {

                GameObj current = GameObjList[index];

                if (current.Name == "diary")
                    _diary = (current as SpriteObj);

                if (current.Name == "doorsprite")
                    _doorSprite = (current as SpriteObj);

            }

            _door = DoorList[0];
            
            _speechBubble = new SpriteObj("ExclamationSquare_Sprite");
            _speechBubble.Flip = SpriteEffects.FlipHorizontally;
            _speechBubble.Scale = new Vector2(1.2f, 1.2f);
            
            GameObjList.Add(_speechBubble);
            _diary.OutlineWidth = 2;
            _speechBubble.Position = new Vector2(_diary.X, _diary.Y - _speechBubble.Height - 20f);
            
            _tutorialTextList = new[] {
                "Tap [Input:" + 11 + "] to Jump",
                "Hold [Input:" + 11 + "] to Jump Higher",
                "Tap [Input:" + 12 + "] to Attack",
                string.Concat(new object[] {
                    "Hold [Input:",
                    19,
                    "] and Tap [Input:",
                    11,
                    "] to Drop Ledges"
                }),
                string.Concat(new object[] {
                    "(Air) Hold [Input:",
                    19,
                    "] and Tap [Input:",
                    12,
                    "] to Attack Down"
                })
            };

            _tutorialControllerTextList = new[] {
                "Tap [Input:" + 10 + "] to Jump",
                "Hold [Input:" + 10 + "] to Jump Higher",
                "Tap [Input:" + 12 + "] to Attack",
                string.Concat(new object[] {
                    "Hold [Input:",
                    18,
                    "] and Tap [Input:",
                    10,
                    "] to Drop Ledges"
                }),
                string.Concat(new object[] {
                    "(Air) Hold [Input:",
                    18,
                    "] and Tap [Input:",
                    12,
                    "] to Attack Down"
                })
            };

            _creditsTextTitleList = new[] {
                "Developed by",
                "Design",
                "Programming",
                "Art",
                "Sound Design & Music",
                "Music",
                ""
            };

            _creditsTextList = new[] {
                "Cellar Door Games",
                "Teddy Lee",
                "Kenny Lee",
                "Glauber Kotaki",
                "Gordon McGladdery",
                "Judson Cowan",
                "Rogue Legacy"
            };

            _creditsPosition = new Vector2(50f, 580f);

            for (int index = 0; index < GameObjList.Count; index++) {

                GameObj current2 = GameObjList[index];

                if (current2.Name == "waypoint1")
                    _waypointList.Add(current2);
                
                if (current2.Name == "waypoint2")
                    _waypointList.Add(current2);
                
                if (current2.Name == "waypoint3")
                    _waypointList.Add(current2);
                
                if (current2.Name == "waypoint4")
                    _waypointList.Add(current2);
                
                if (current2.Name == "waypoint5")
                    _waypointList.Add(current2);

            }

            base.Initialize();

        }

        public override void LoadContent(GraphicsDevice graphics) {

            _tutorialText = new KeyIconTextObj(Game.JunicodeLargeFont);
            _tutorialText.FontSize = 28f;
            _tutorialText.Text = "[Input:" + 10 + "] to Jump";
            _tutorialText.Align = Types.TextAlign.Centre;
            _tutorialText.OutlineWidth = 2;
            _tutorialText.ForcedScale = new Vector2(0.8f, 0.8f);
            
            _creditsText = new TextObj(Game.JunicodeFont);
            _creditsText.FontSize = 20f;
            _creditsText.Text = "Cellar Door Games";
            _creditsText.DropShadow = new Vector2(2f, 2f);
            
            _creditsTitleText = (_creditsText.Clone() as TextObj);
            _creditsTitleText.FontSize = 14f;
            
            TextObj textObj = new TextObj(Game.JunicodeFont);
            textObj.FontSize = 12f;
            textObj.Text = "Down Attack this";
            textObj.OutlineWidth = 2;
            textObj.Align = Types.TextAlign.Centre;
            textObj.Position = _waypointList[_waypointList.Count - 1].Position;
            textObj.X -= 25f;
            textObj.Y -= 70f;
            
            GameObjList.Add(textObj);
            base.LoadContent(graphics);

        }

        public override void OnEnter() {

            _speechBubble.Visible = false;
            _diary.Visible = false;
            _doorSprite.ChangeSprite("CastleDoorOpen_Sprite");
            
            if (Game.PlayerStats.TutorialComplete) {

                if (!Game.PlayerStats.ReadLastDiary) {
                    _door.Locked = true;
                    _doorSprite.ChangeSprite("CastleDoor_Sprite");
                }
                else
                    _door.Locked = false;

                _diary.Visible = true;
                Player.UpdateCollisionBoxes();
                Player.Position = new Vector2(X + 240f + Player.Width, (Bounds.Bottom - 120) - (Player.Bounds.Bottom - Player.Y));
            }
            _creditsTitleText.Opacity = 0f;
            _creditsText.Opacity = 0f;
            
            for (int index = 0; index < EnemyList.Count; index++) {
                EnemyObj current = EnemyList[index];
                current.Damage = 0;
            }

            _tutorialText.Opacity = 0f;
            Player.UnlockControls();
            
            if (!Game.PlayerStats.TutorialComplete)
                SoundManager.PlayMusic("EndSong", true, 4f);
            else
                SoundManager.StopMusic(4f);

            Tween.RunFunction(2f, Player.AttachedLevel, "DisplayCreditsText", new object[] {
                true
            });
            
            base.OnEnter();

        }

        public void DisplayCreditsText() {

            if (_creditsIndex < _creditsTextList.Length) {

                _creditsTitleText.Opacity = 0f;
                _creditsText.Opacity = 0f;
                _creditsTitleText.Text = _creditsTextTitleList[_creditsIndex];
                _creditsText.Text = _creditsTextList[_creditsIndex];
                
                Tween.To(_creditsTitleText, 0.5f, Tween.EaseNone, new[] {
                    "Opacity",
                    "1"
                });

                Tween.To(_creditsText, 0.5f, Tween.EaseNone, new[] {
                    "delay",
                    "0.2",
                    "Opacity",
                    "1"
                });

                _creditsTitleText.Opacity = 1f;
                _creditsText.Opacity = 1f;
                
                Tween.To(_creditsTitleText, 0.5f, Tween.EaseNone, new[] {
                    "delay",
                    "4",
                    "Opacity",
                    "0"
                });

                Tween.To(_creditsText, 0.5f, Tween.EaseNone, new[] {
                    "delay",
                    "4.2",
                    "Opacity",
                    "0"
                });

                _creditsTitleText.Opacity = 0f;
                _creditsText.Opacity = 0f;
                _creditsIndex++;

                Tween.RunFunction(8f, this, "DisplayCreditsText", new object[0]);

            }

        }

        private int PlayerNearWaypoint() {

            for (int i = 0; i < _waypointList.Count; i++) {
                if (CDGMath.DistanceBetweenPts(Player.Position, _waypointList[i].Position) < 500f)
                    return i;
            }

            return -1;

        }

        public override void Update(GameTime gameTime) {

            if (!Game.PlayerStats.TutorialComplete) {

                int waypointIndex = _waypointIndex;
                _waypointIndex = PlayerNearWaypoint();
                
                if (_waypointIndex != waypointIndex) {

                    Tween.StopAllContaining(_tutorialText, false);
                    
                    if (_waypointIndex != -1) {

                        if (!InputManager.GamePadIsConnected(PlayerIndex.One))
                            _tutorialText.Text = _tutorialTextList[_waypointIndex];
                        else
                            _tutorialText.Text = _tutorialControllerTextList[_waypointIndex];

                        Tween.To(_tutorialText, 0.25f, Tween.EaseNone, new[] {
                            "Opacity",
                            "1"
                        });
                    }
                    else {
                        Tween.To(_tutorialText, 0.25f, Tween.EaseNone, new[] {
                            "Opacity",
                            "0"
                        });
                    }

                }

            }
            else {

                Rectangle bounds = _diary.Bounds;
                bounds.X -= 50;
                bounds.Width += 100;
                _speechBubble.Y = _diary.Y - _speechBubble.Height - 20f - 30f + (float)Math.Sin((Game.TotalGameTime * 20f)) * 2f;
                
                if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround) {
                    
                    if (_speechBubble.SpriteName == "ExclamationSquare_Sprite")
                        _speechBubble.ChangeSprite("UpArrowSquare_Sprite");
                    
                    if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)) {

                        if (!Game.PlayerStats.ReadLastDiary) {
                            RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                            rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + 24);
                            rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "RunFlashback", new object[0]);
                            rCScreenManager.DisplayScreen(13, true);
                        }
                        else {
                            RCScreenManager rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
                            rCScreenManager2.DisplayScreen(20, true);
                        }

                    }

                }
                else if (_speechBubble.SpriteName == "UpArrowSquare_Sprite")
                    _speechBubble.ChangeSprite("ExclamationSquare_Sprite");

                if (!Game.PlayerStats.ReadLastDiary || CollisionMath.Intersects(Player.Bounds, bounds))
                    _speechBubble.Visible = true;
                else if (Game.PlayerStats.ReadLastDiary && !CollisionMath.Intersects(Player.Bounds, bounds))
                    _speechBubble.Visible = false;

            }

            base.Update(gameTime);

        }

        public void RunFlashback() {
            Player.LockControls();
            (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(25, true);
            Tween.RunFunction(0.5f, this, "OpenDoor", new object[0]);
        }

        public void OpenDoor() {

            Player.UnlockControls();
            _doorSprite.ChangeSprite("CastleDoorOpen_Sprite");
            _door.Locked = false;
            Game.PlayerStats.ReadLastDiary = true;
            Game.PlayerStats.DiaryEntry = 25;

            (Player.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(new[] {
                SaveType.PlayerData
            });

        }

        public override void Draw(Camera2D camera) {
            
            Vector2 topLeftCorner = Game.ScreenManager.Camera.TopLeftCorner;
            _creditsTitleText.Position = new Vector2(topLeftCorner.X + _creditsPosition.X, topLeftCorner.Y + _creditsPosition.Y);
            _creditsText.Position = _creditsTitleText.Position;
            _creditsText.Y += 35f;
            _creditsTitleText.X += 5f;
            base.Draw(camera);
            _tutorialText.Position = Game.ScreenManager.Camera.Position;
            _tutorialText.Y -= 200f;
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _tutorialText.Draw(camera);
            _creditsText.Draw(camera);
            _creditsTitleText.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

        }

        public override void Dispose() {

            if (!IsDisposed) {

                _tutorialText.Dispose();
                _tutorialText = null;
                _waypointList.Clear();
                _waypointList = null;
                _creditsText.Dispose();
                _creditsText = null;
                _creditsTitleText.Dispose();
                _creditsTitleText = null;
                Array.Clear(_tutorialTextList, 0, _tutorialTextList.Length);
                Array.Clear(_tutorialControllerTextList, 0, _tutorialControllerTextList.Length);
                Array.Clear(_creditsTextTitleList, 0, _creditsTextTitleList.Length);
                Array.Clear(_creditsTextList, 0, _creditsTextList.Length);
                _tutorialTextList = null;
                _creditsTextTitleList = null;
                _creditsTextList = null;
                _tutorialControllerTextList = null;
                _door = null;
                _doorSprite = null;
                _diary = null;
                _speechBubble = null;
                base.Dispose();

            }

        }

        protected override GameObj CreateCloneInstance() {
            return new TutorialRoomObj();
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
        }

    }

}
