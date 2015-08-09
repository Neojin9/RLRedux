using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {

    public class VitaChamberRoomObj : BonusRoomObj {

        private GameObj   _fountain;
        private SpriteObj _speechBubble;

        public override void Initialize() {

            _speechBubble = new SpriteObj("UpArrowSquare_Sprite");
            _speechBubble.Flip = SpriteEffects.FlipHorizontally;
            GameObjList.Add(_speechBubble);

            for (int index = 0; index < GameObjList.Count; index++) {

                GameObj current = GameObjList[index];
                
                if (current.Name == "fountain") {
                    _fountain = current;
                    break;
                }

            }

            (_fountain as SpriteObj).OutlineWidth = 2;
            _speechBubble.X = _fountain.X;
            base.Initialize();

        }

        public override void OnEnter() {

            if (RoomCompleted) {
                _speechBubble.Visible = false;
                _fountain.TextureColor = new Color(100, 100, 100);
            }
            else
                _fountain.TextureColor = Color.White;

            base.OnEnter();

        }

        public override void Update(GameTime gameTime) {

            if (!RoomCompleted) {

                Rectangle bounds = _fountain.Bounds;
                bounds.X -= 50;
                bounds.Width += 100;
                
                if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround) {
                    _speechBubble.Y = _fountain.Y - 150f + (float)Math.Sin((Game.TotalGameTime * 20f)) * 2f;
                    _speechBubble.Visible = true;
                }
                else
                    _speechBubble.Visible = false;

                if (_speechBubble.Visible && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))) {

                    int num = (int)(Player.MaxHealth * 0.3f);
                    int num2 = (int)(Player.MaxMana * 0.3f);
                    Player.CurrentHealth += num;
                    Player.CurrentMana += num2;
                    Console.WriteLine("Healed");
                    SoundManager.PlaySound("Collect_Mana");
                    Player.AttachedLevel.TextManager.DisplayNumberStringText(num, "hp recovered", Color.LawnGreen, new Vector2(Player.X, (Player.Bounds.Top - 30)));
                    Player.AttachedLevel.TextManager.DisplayNumberStringText(num2, "mp recovered", Color.CornflowerBlue, new Vector2(Player.X, Player.Bounds.Top));
                    RoomCompleted = true;
                    _fountain.TextureColor = new Color(100, 100, 100);
                    _speechBubble.Visible = false;

                }

            }

            base.Update(gameTime);

        }

        public override void Dispose() {

            if (!IsDisposed) {
                _fountain = null;
                _speechBubble = null;
                base.Dispose();
            }

        }

    }

}
