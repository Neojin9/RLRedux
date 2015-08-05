using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using System.Xml;


namespace RogueCastle {
    public class BorderObj : GameObj {
        public bool BorderBottom;
        public bool BorderLeft;
        public bool BorderRight;
        public bool BorderTop;
        public GameTypes.LevelType LevelType = GameTypes.LevelType.CASTLE;

        public BorderObj() {
            TextureScale = new Vector2(1f, 1f);
            CornerTexture = new SpriteObj("Blank_Sprite");
            CornerTexture.Scale = new Vector2(2f, 2f);
            CornerLTexture = new SpriteObj("Blank_Sprite");
            CornerLTexture.Scale = new Vector2(2f, 2f);
        }

        public Texture2D BorderTexture { get; internal set; }
        public SpriteObj CornerTexture { get; internal set; }
        public SpriteObj CornerLTexture { get; internal set; }
        public Texture2D NeoTexture { get; set; }
        public Vector2 TextureScale { get; set; }
        public Vector2 TextureOffset { get; set; }

        public void SetBorderTextures(Texture2D borderTexture, string cornerTextureString, string cornerLTextureString) {
            BorderTexture = borderTexture;
            CornerTexture.ChangeSprite(cornerTextureString);
            CornerLTexture.ChangeSprite(cornerLTextureString);
        }

        public void SetWidth(int width) {
            this._width = width;
        }

        public void SetHeight(int height) {
            this._height = height;
            if (height < 60) {
                BorderBottom = false;
                BorderLeft = false;
                BorderRight = false;
                base.TextureColor = new Color(150, 150, 150);
            }
        }

        public override void Draw(Camera2D camera) {
            Texture2D texture2D = BorderTexture;
            if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f) {
                TextureOffset = Vector2.Zero;
                texture2D = NeoTexture;
            }
            if (BorderBottom)
                camera.Draw(texture2D, new Vector2((float)(this.Bounds.Right - CornerTexture.Width) + TextureOffset.X, (float)this.Bounds.Bottom - TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Width / TextureScale.X) - CornerTexture.Width * 2, texture2D.Height)), base.TextureColor, MathHelper.ToRadians(180f), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
            if (BorderLeft)
                camera.Draw(texture2D, new Vector2(base.X + TextureOffset.Y, (float)(this.Bounds.Bottom - CornerTexture.Width) - TextureOffset.X), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Height / TextureScale.Y) - CornerTexture.Width * 2, texture2D.Height)), base.TextureColor, MathHelper.ToRadians(-90f), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
            if (BorderRight)
                camera.Draw(texture2D, new Vector2((float)this.Bounds.Right - TextureOffset.Y, base.Y + (float)CornerTexture.Width + TextureOffset.X), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Height / TextureScale.Y) - CornerTexture.Width * 2, texture2D.Height)), base.TextureColor, MathHelper.ToRadians(90f), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
            if (BorderTop) {
                if (base.Rotation == 0f)
                    camera.Draw(texture2D, new Vector2(base.X + (float)CornerTexture.Width + TextureOffset.X, base.Y + TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Width / TextureScale.X) - CornerTexture.Width * 2, texture2D.Height)), base.TextureColor, MathHelper.ToRadians(base.Rotation), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
                else {
                    Vector2 position = CollisionMath.UpperLeftCorner(new Rectangle((int)base.X, (int)base.Y, this._width, this._height), base.Rotation, Vector2.Zero);
                    Vector2 position2 = CollisionMath.UpperRightCorner(new Rectangle((int)base.X, (int)base.Y, this._width, this._height), base.Rotation, Vector2.Zero);
                    if (base.Rotation > 0f && base.Rotation < 80f) {
                        CornerTexture.Flip = SpriteEffects.FlipHorizontally;
                        CornerTexture.Position = position;
                        CornerTexture.Rotation = 0f;
                        CornerTexture.Draw(camera);
                        CornerTexture.Flip = SpriteEffects.None;
                        CornerTexture.Position = new Vector2(position2.X - (float)CornerTexture.Width / 2f, position2.Y);
                        CornerTexture.Rotation = 0f;
                        CornerTexture.Draw(camera);
                    }
                    if (base.Rotation < 0f && base.Rotation > -80f) {
                        CornerTexture.Flip = SpriteEffects.FlipHorizontally;
                        CornerTexture.Position = position;
                        CornerTexture.X += (float)CornerTexture.Width / 2f;
                        CornerTexture.Rotation = 0f;
                        CornerTexture.Draw(camera);
                        CornerTexture.Flip = SpriteEffects.None;
                        CornerTexture.Position = position2;
                        CornerTexture.Rotation = 0f;
                        CornerTexture.Draw(camera);
                    }
                    camera.Draw(texture2D, new Vector2(base.X + TextureOffset.X - (float)Math.Sin(MathHelper.ToRadians(base.Rotation)) * TextureOffset.Y, base.Y + (float)Math.Cos(MathHelper.ToRadians(base.Rotation)) * TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Width / TextureScale.X), texture2D.Height)), base.TextureColor, MathHelper.ToRadians(base.Rotation), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
                }
            }
            base.Draw(camera);
        }

        public void DrawCorners(Camera2D camera) {
            CornerTexture.TextureColor = base.TextureColor;
            CornerLTexture.TextureColor = base.TextureColor;
            CornerLTexture.Flip = SpriteEffects.None;
            CornerTexture.Flip = SpriteEffects.None;
            CornerLTexture.Rotation = 0f;
            if (BorderTop) {
                if (BorderRight) {
                    CornerLTexture.Position = new Vector2((float)(this.Bounds.Right - CornerLTexture.Width), (float)this.Bounds.Top);
                    CornerLTexture.Draw(camera);
                }
                else {
                    CornerTexture.Position = new Vector2((float)(this.Bounds.Right - CornerTexture.Width), (float)this.Bounds.Top);
                    CornerTexture.Draw(camera);
                }
                CornerLTexture.Flip = SpriteEffects.FlipHorizontally;
                CornerTexture.Flip = SpriteEffects.FlipHorizontally;
                if (BorderLeft) {
                    CornerLTexture.Position = new Vector2((float)(this.Bounds.Left + CornerLTexture.Width), (float)this.Bounds.Top);
                    CornerLTexture.Draw(camera);
                }
                else {
                    CornerTexture.Position = new Vector2((float)(this.Bounds.Left + CornerTexture.Width), (float)this.Bounds.Top);
                    CornerTexture.Draw(camera);
                }
            }
            if (BorderBottom) {
                CornerTexture.Flip = SpriteEffects.FlipVertically;
                CornerLTexture.Flip = SpriteEffects.FlipVertically;
                if (BorderRight) {
                    CornerLTexture.Position = new Vector2((float)(this.Bounds.Right - CornerLTexture.Width), (float)(this.Bounds.Bottom - CornerLTexture.Height));
                    CornerLTexture.Draw(camera);
                }
                else {
                    CornerTexture.Flip = SpriteEffects.FlipVertically;
                    CornerTexture.Position = new Vector2((float)(this.Bounds.Right - CornerTexture.Width), (float)(this.Bounds.Bottom - CornerTexture.Height));
                    CornerTexture.Draw(camera);
                }
                if (BorderLeft) {
                    CornerLTexture.Position = new Vector2((float)(this.Bounds.Left + CornerLTexture.Width), (float)(this.Bounds.Bottom - CornerLTexture.Height));
                    CornerLTexture.Rotation = 90f;
                    CornerLTexture.Draw(camera);
                    CornerLTexture.Rotation = 0f;
                }
                else {
                    CornerTexture.Flip = SpriteEffects.None;
                    CornerTexture.Position = new Vector2((float)(this.Bounds.Left + CornerTexture.Width), (float)this.Bounds.Bottom);
                    CornerTexture.Rotation = 180f;
                    CornerTexture.Draw(camera);
                    CornerTexture.Rotation = 0f;
                }
            }
            if (BorderLeft) {
                CornerTexture.Flip = SpriteEffects.None;
                CornerLTexture.Flip = SpriteEffects.None;
                if (!BorderBottom) {
                    CornerTexture.Position = new Vector2((float)this.Bounds.Left, (float)(this.Bounds.Bottom - CornerTexture.Width));
                    CornerTexture.Flip = SpriteEffects.FlipHorizontally;
                    CornerTexture.Rotation = -90f;
                    CornerTexture.Draw(camera);
                    CornerTexture.Rotation = 0f;
                }
                if (!BorderTop) {
                    CornerTexture.Position = new Vector2((float)this.Bounds.Left, (float)(this.Bounds.Top + CornerTexture.Width));
                    CornerTexture.Flip = SpriteEffects.None;
                    CornerTexture.Rotation = -90f;
                    CornerTexture.Draw(camera);
                    CornerTexture.Rotation = 0f;
                }
            }
            if (BorderRight) {
                CornerTexture.Flip = SpriteEffects.None;
                CornerLTexture.Flip = SpriteEffects.None;
                if (!BorderBottom) {
                    CornerTexture.Position = new Vector2((float)this.Bounds.Right, (float)(this.Bounds.Bottom - CornerTexture.Width));
                    CornerTexture.Rotation = 90f;
                    CornerTexture.Draw(camera);
                    CornerTexture.Rotation = 0f;
                }
                if (!BorderTop) {
                    CornerTexture.Position = new Vector2((float)this.Bounds.Right, (float)(this.Bounds.Top + CornerTexture.Width));
                    CornerTexture.Flip = SpriteEffects.FlipHorizontally;
                    CornerTexture.Rotation = 90f;
                    CornerTexture.Draw(camera);
                    CornerTexture.Rotation = 0f;
                }
            }
        }

        protected override GameObj CreateCloneInstance() {
            return new BorderObj();
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            BorderObj borderObj = obj as BorderObj;
            borderObj.LevelType = LevelType;
            borderObj.BorderTop = BorderTop;
            borderObj.BorderBottom = BorderBottom;
            borderObj.BorderLeft = BorderLeft;
            borderObj.BorderRight = BorderRight;
            borderObj.SetHeight(this._height);
            borderObj.SetWidth(this._width);
            borderObj.NeoTexture = NeoTexture;
            borderObj.SetBorderTextures(BorderTexture, CornerTexture.SpriteName, CornerLTexture.SpriteName);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                BorderTexture = null;
                CornerTexture.Dispose();
                CornerTexture = null;
                CornerLTexture.Dispose();
                CornerLTexture = null;
                NeoTexture = null;
                base.Dispose();
            }
        }

        public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci) {
            base.PopulateFromXMLReader(reader, ci);
            SetWidth(this._width);
            SetHeight(this._height);
            if (reader.MoveToAttribute("CollidesTop"))
                BorderTop = bool.Parse(reader.Value);
            if (reader.MoveToAttribute("CollidesBottom"))
                BorderBottom = bool.Parse(reader.Value);
            if (reader.MoveToAttribute("CollidesLeft"))
                BorderLeft = bool.Parse(reader.Value);
            if (reader.MoveToAttribute("CollidesRight"))
                BorderRight = bool.Parse(reader.Value);
        }
    }
}
