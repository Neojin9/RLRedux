namespace RogueCastle {

    public class BonusRoomObj : RoomObj {

        public BonusRoomObj() {
            ID = -1;
        }

        public bool RoomCompleted { get; set; }

        public override void Reset() {
            RoomCompleted = false;
            base.Reset();
        }

    }

}
