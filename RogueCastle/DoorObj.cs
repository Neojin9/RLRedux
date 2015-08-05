using System;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class DoorObj : TerrainObj {
        public bool Attached;
        public string DoorPosition = "NONE";
        public bool IsBossDoor;
        public bool Locked;
        private SpriteObj m_arrowIcon;
        private GameTypes.DoorType m_doorType = GameTypes.DoorType.OPEN;

        public DoorObj(RoomObj roomRef, int width, int height, GameTypes.DoorType doorType) : base(width, height) {
            m_doorType = doorType;
            Room = roomRef;
            base.CollisionTypeTag = 0;
            base.DisableHitboxUpdating = true;
            m_arrowIcon = new SpriteObj("UpArrowSquare_Sprite");
            m_arrowIcon.OutlineWidth = 2;
            m_arrowIcon.Visible = false;
        }

        public RoomObj Room { get; set; }

        public GameTypes.DoorType DoorType {
            get { return m_doorType; }
        }

        public override void Draw(Camera2D camera) {
            if (m_arrowIcon.Visible) {
                m_arrowIcon.Position = new Vector2((float)this.Bounds.Center.X, (this.Bounds.Top - 10) + (float)Math.Sin((Game.TotalGameTime * 20f)) * 3f);
                m_arrowIcon.Draw(camera);
                m_arrowIcon.Visible = false;
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
            if (!Locked && playerObj != null && playerObj.IsTouchingGround && DoorPosition == "None")
                m_arrowIcon.Visible = true;
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        protected override GameObj CreateCloneInstance() {
            return new DoorObj(Room, this._width, this._height, m_doorType);
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            DoorObj doorObj = obj as DoorObj;
            doorObj.Attached = Attached;
            doorObj.IsBossDoor = IsBossDoor;
            doorObj.Locked = Locked;
            doorObj.DoorPosition = DoorPosition;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                Room = null;
                m_arrowIcon.Dispose();
                m_arrowIcon = null;
                base.Dispose();
            }
        }

        public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci) {
            base.PopulateFromXMLReader(reader, ci);
            if (reader.MoveToAttribute("BossDoor"))
                IsBossDoor = bool.Parse(reader.Value);
            if (reader.MoveToAttribute("DoorPos"))
                DoorPosition = reader.Value;
        }
    }
}
