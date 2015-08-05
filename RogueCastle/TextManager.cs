using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public class TextManager : IDisposableObj {
        private bool m_isDisposed;
        private int m_poolSize;
        private DS2DPool<TextObj> m_resourcePool;

        public TextManager(int poolSize) {
            m_poolSize = poolSize;
            m_resourcePool = new DS2DPool<TextObj>();
        }

        public bool IsDisposed {
            get { return m_isDisposed; }
        }

        public int ActiveTextObjs {
            get { return m_resourcePool.NumActiveObjs; }
        }

        public int TotalPoolSize {
            get { return m_resourcePool.TotalPoolSize; }
        }

        public int CurrentPoolSize {
            get { return TotalPoolSize - ActiveTextObjs; }
        }

        public void Initialize() {
            for (int i = 0; i < m_poolSize; i++) {
                TextObj textObj = new TextObj(null);
                textObj.Visible = false;
                textObj.TextureColor = Color.White;
                textObj.OutlineWidth = 2;
                m_resourcePool.AddToPool(textObj);
            }
        }

        public void DisplayNumberStringText(int amount, string text, Color color, Vector2 position) {
            TextObj textObj = m_resourcePool.CheckOut();
            textObj.Font = Game.JunicodeFont;
            textObj.FontSize = 14f;
            textObj.Text = amount + " " + text;
            int width = textObj.Width;
            m_resourcePool.CheckIn(textObj);
            TextObj textObj2 = m_resourcePool.CheckOut();
            textObj2.Font = Game.HerzogFont;
            textObj2.Text = amount.ToString();
            textObj2.Align = Types.TextAlign.Left;
            textObj2.FontSize = 18f;
            textObj2.TextureColor = color;
            textObj2.Position = new Vector2(position.X - width / 2f, position.Y - (float)textObj2.Height / 2f);
            textObj2.Visible = true;
            TextObj textObj3 = m_resourcePool.CheckOut();
            textObj3.Font = Game.JunicodeFont;
            textObj3.Text = " " + text;
            textObj3.FontSize = 14f;
            textObj3.Align = Types.TextAlign.Left;
            textObj3.TextureColor = color;
            textObj3.Position = textObj2.Position;
            textObj3.X += (float)textObj2.Width;
            textObj3.Y -= 5f;
            textObj3.Visible = true;
            Tween.By(textObj2, 0.3f, new Easing(Quad.EaseOut), new[] {
                "Y",
                "-60"
            });
            Tween.To(textObj2, 0.2f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.5",
                "Opacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(this, "DestroyText", new object[] {
                textObj2
            });
            Tween.By(textObj3, 0.3f, new Easing(Quad.EaseOut), new[] {
                "Y",
                "-60"
            });
            Tween.To(textObj3, 0.2f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.5",
                "Opacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(this, "DestroyText", new object[] {
                textObj3
            });
        }

        public void DisplayStringNumberText(string text, int amount, Color color, Vector2 position) {
            TextObj textObj = m_resourcePool.CheckOut();
            textObj.Font = Game.JunicodeFont;
            textObj.FontSize = 14f;
            textObj.Text = text + " " + amount;
            int width = textObj.Width;
            m_resourcePool.CheckIn(textObj);
            TextObj textObj2 = m_resourcePool.CheckOut();
            textObj2.Font = Game.JunicodeFont;
            textObj2.Text = text + " ";
            textObj2.FontSize = 14f;
            textObj2.TextureColor = color;
            textObj2.Position = new Vector2(position.X - width / 2f, position.Y - (float)textObj2.Height / 2f);
            textObj2.Visible = true;
            TextObj textObj3 = m_resourcePool.CheckOut();
            textObj3.Font = Game.HerzogFont;
            textObj3.Text = amount.ToString();
            textObj3.FontSize = 18f;
            textObj3.TextureColor = color;
            textObj3.Position = new Vector2(textObj2.X + (float)textObj2.Width, textObj2.Y + 5f);
            textObj3.Visible = true;
            Tween.By(textObj3, 0.3f, new Easing(Quad.EaseOut), new[] {
                "Y",
                "-60"
            });
            Tween.To(textObj3, 0.2f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.5",
                "Opacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(this, "DestroyText", new object[] {
                textObj3
            });
            Tween.By(textObj2, 0.3f, new Easing(Quad.EaseOut), new[] {
                "Y",
                "-60"
            });
            Tween.To(textObj2, 0.2f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.5",
                "Opacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(this, "DestroyText", new object[] {
                textObj2
            });
        }

        public void DisplayNumberText(int amount, Color color, Vector2 position) {
            TextObj textObj = m_resourcePool.CheckOut();
            textObj.Font = Game.HerzogFont;
            textObj.Text = amount.ToString();
            textObj.FontSize = 18f;
            textObj.TextureColor = color;
            textObj.Align = Types.TextAlign.Centre;
            textObj.Visible = true;
            textObj.Position = position;
            textObj.Y -= (float)textObj.Height / 2f;
            Tween.By(textObj, 0.3f, new Easing(Quad.EaseOut), new[] {
                "Y",
                "-60"
            });
            Tween.To(textObj, 0.2f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.5",
                "Opacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(this, "DestroyText", new object[] {
                textObj
            });
        }

        public void DisplayStringText(string text, Color color, Vector2 position) {
            TextObj textObj = m_resourcePool.CheckOut();
            textObj.Font = Game.JunicodeFont;
            textObj.Text = text;
            textObj.Align = Types.TextAlign.Centre;
            textObj.FontSize = 14f;
            textObj.TextureColor = color;
            textObj.Position = position;
            textObj.Visible = true;
            Tween.By(textObj, 0.3f, new Easing(Quad.EaseOut), new[] {
                "Y",
                "-60"
            });
            Tween.To(textObj, 0.2f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.5",
                "Opacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(this, "DestroyText", new object[] {
                textObj
            });
        }

        public void DestroyText(TextObj obj) {
            obj.Opacity = 1f;
            obj.Align = Types.TextAlign.Left;
            obj.Visible = false;
            m_resourcePool.CheckIn(obj);
        }

        public void Draw(Camera2D camera) {
            foreach (TextObj current in m_resourcePool.ActiveObjsList)
                current.Draw(camera);
        }

        public void Dispose() {
            if (!IsDisposed) {
                Console.WriteLine("Disposing Text Manager");
                m_resourcePool.Dispose();
                m_resourcePool = null;
                m_isDisposed = true;
            }
        }
    }
}
