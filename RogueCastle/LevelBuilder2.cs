using System;
using System.Collections.Generic;
using System.Globalization;
using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    internal class LevelBuilder2 {
        private const int MAX_ROOM_SIZE = 4;
        private static List<RoomObj>[,] m_castleRoomArray = new List<RoomObj>[4,4];
        private static List<RoomObj>[,] m_dungeonRoomArray = new List<RoomObj>[4,4];
        private static List<RoomObj>[,] m_towerRoomArray = new List<RoomObj>[4,4];
        private static List<RoomObj>[,] m_gardenRoomArray = new List<RoomObj>[4,4];
        private static List<RoomObj> m_bossRoomArray;
        private static RoomObj m_startingRoom;
        private static RoomObj m_testRoom;
        private static RoomObj m_castleEntranceRoom;
        private static RoomObj m_linkerCastleRoom;
        private static RoomObj m_linkerDungeonRoom;
        private static RoomObj m_linkerGardenRoom;
        private static RoomObj m_linkerTowerRoom;
        private static RoomObj m_bossCastleEntranceRoom;
        private static RoomObj m_bossDungeonEntranceRoom;
        private static RoomObj m_bossGardenEntranceRoom;
        private static RoomObj m_bossTowerEntranceRoom;
        private static List<RoomObj> m_secretCastleRoomArray;
        private static List<RoomObj> m_secretGardenRoomArray;
        private static List<RoomObj> m_secretTowerRoomArray;
        private static List<RoomObj> m_secretDungeonRoomArray;
        private static List<RoomObj> m_bonusCastleRoomArray;
        private static List<RoomObj> m_bonusGardenRoomArray;
        private static List<RoomObj> m_bonusTowerRoomArray;
        private static List<RoomObj> m_bonusDungeonRoomArray;
        private static List<RoomObj> m_dlcCastleRoomArray;
        private static List<RoomObj> m_dlcGardenRoomArray;
        private static List<RoomObj> m_dlcTowerRoomArray;
        private static List<RoomObj> m_dlcDungeonRoomArray;
        private static RoomObj m_tutorialRoom;
        private static RoomObj m_throneRoom;
        private static RoomObj m_endingRoom;
        private static CompassRoomObj m_compassRoom;
        private static List<RoomObj> m_challengeRoomArray;
        private static bool hasTopDoor;
        private static bool hasBottomDoor;
        private static bool hasLeftDoor;
        private static bool hasRightDoor;
        private static bool hasTopLeftDoor;
        private static bool hasTopRightDoor;
        private static bool hasBottomLeftDoor;
        private static bool hasBottomRightDoor;
        private static bool hasRightTopDoor;
        private static bool hasRightBottomDoor;
        private static bool hasLeftTopDoor;
        private static bool hasLeftBottomDoor;

        public static RoomObj StartingRoom {
            get { return LevelBuilder2.m_startingRoom; }
        }

        public static List<RoomObj> SequencedRoomList {
            get {
                List<RoomObj> list = new List<RoomObj>();
                list.Add(LevelBuilder2.m_startingRoom);
                list.Add(LevelBuilder2.m_linkerCastleRoom);
                list.Add(LevelBuilder2.m_linkerTowerRoom);
                list.Add(LevelBuilder2.m_linkerDungeonRoom);
                list.Add(LevelBuilder2.m_linkerGardenRoom);
                list.Add(LevelBuilder2.m_bossCastleEntranceRoom);
                list.Add(LevelBuilder2.m_bossTowerEntranceRoom);
                list.Add(LevelBuilder2.m_bossDungeonEntranceRoom);
                list.Add(LevelBuilder2.m_bossGardenEntranceRoom);
                list.Add(LevelBuilder2.m_castleEntranceRoom);
                List<RoomObj>[,] castleRoomArray = LevelBuilder2.m_castleRoomArray;
                int upperBound = castleRoomArray.GetUpperBound(0);
                int upperBound2 = castleRoomArray.GetUpperBound(1);
                for (int i = castleRoomArray.GetLowerBound(0); i <= upperBound; i++) {
                    for (int j = castleRoomArray.GetLowerBound(1); j <= upperBound2; j++) {
                        List<RoomObj> collection = castleRoomArray[i, j];
                        list.AddRange(collection);
                    }
                }
                List<RoomObj>[,] dungeonRoomArray = LevelBuilder2.m_dungeonRoomArray;
                int upperBound3 = dungeonRoomArray.GetUpperBound(0);
                int upperBound4 = dungeonRoomArray.GetUpperBound(1);
                for (int k = dungeonRoomArray.GetLowerBound(0); k <= upperBound3; k++) {
                    for (int l = dungeonRoomArray.GetLowerBound(1); l <= upperBound4; l++) {
                        List<RoomObj> collection2 = dungeonRoomArray[k, l];
                        list.AddRange(collection2);
                    }
                }
                List<RoomObj>[,] towerRoomArray = LevelBuilder2.m_towerRoomArray;
                int upperBound5 = towerRoomArray.GetUpperBound(0);
                int upperBound6 = towerRoomArray.GetUpperBound(1);
                for (int m = towerRoomArray.GetLowerBound(0); m <= upperBound5; m++) {
                    for (int n = towerRoomArray.GetLowerBound(1); n <= upperBound6; n++) {
                        List<RoomObj> collection3 = towerRoomArray[m, n];
                        list.AddRange(collection3);
                    }
                }
                List<RoomObj>[,] gardenRoomArray = LevelBuilder2.m_gardenRoomArray;
                int upperBound7 = gardenRoomArray.GetUpperBound(0);
                int upperBound8 = gardenRoomArray.GetUpperBound(1);
                for (int num = gardenRoomArray.GetLowerBound(0); num <= upperBound7; num++) {
                    for (int num2 = gardenRoomArray.GetLowerBound(1); num2 <= upperBound8; num2++) {
                        List<RoomObj> collection4 = gardenRoomArray[num, num2];
                        list.AddRange(collection4);
                    }
                }
                list.AddRange(LevelBuilder2.m_secretCastleRoomArray);
                list.AddRange(LevelBuilder2.m_secretTowerRoomArray);
                list.AddRange(LevelBuilder2.m_secretDungeonRoomArray);
                list.AddRange(LevelBuilder2.m_secretGardenRoomArray);
                list.AddRange(LevelBuilder2.m_bonusCastleRoomArray);
                list.AddRange(LevelBuilder2.m_bonusTowerRoomArray);
                list.AddRange(LevelBuilder2.m_bonusDungeonRoomArray);
                list.AddRange(LevelBuilder2.m_bonusGardenRoomArray);
                list.AddRange(LevelBuilder2.m_bossRoomArray);
                list.AddRange(LevelBuilder2.m_challengeRoomArray);
                list.Add(LevelBuilder2.m_compassRoom);
                return list;
            }
        }

        public static void Initialize() {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    LevelBuilder2.m_castleRoomArray[i, j] = new List<RoomObj>();
                    LevelBuilder2.m_dungeonRoomArray[i, j] = new List<RoomObj>();
                    LevelBuilder2.m_towerRoomArray[i, j] = new List<RoomObj>();
                    LevelBuilder2.m_gardenRoomArray[i, j] = new List<RoomObj>();
                }
            }
            LevelBuilder2.m_secretCastleRoomArray = new List<RoomObj>();
            LevelBuilder2.m_secretGardenRoomArray = new List<RoomObj>();
            LevelBuilder2.m_secretTowerRoomArray = new List<RoomObj>();
            LevelBuilder2.m_secretDungeonRoomArray = new List<RoomObj>();
            LevelBuilder2.m_bonusCastleRoomArray = new List<RoomObj>();
            LevelBuilder2.m_bonusGardenRoomArray = new List<RoomObj>();
            LevelBuilder2.m_bonusTowerRoomArray = new List<RoomObj>();
            LevelBuilder2.m_bonusDungeonRoomArray = new List<RoomObj>();
            LevelBuilder2.m_bossRoomArray = new List<RoomObj>();
            LevelBuilder2.m_challengeRoomArray = new List<RoomObj>();
            LevelBuilder2.m_dlcCastleRoomArray = new List<RoomObj>();
            LevelBuilder2.m_dlcDungeonRoomArray = new List<RoomObj>();
            LevelBuilder2.m_dlcGardenRoomArray = new List<RoomObj>();
            LevelBuilder2.m_dlcTowerRoomArray = new List<RoomObj>();
        }

        public static void StoreRoom(RoomObj room, GameTypes.LevelType levelType) {
            if (room.Name != "Start" && room.Name != "Linker" && room.Name != "Boss" && room.Name != "EntranceBoss" && room.Name != "Secret" && room.Name != "Bonus" && room.Name != "CastleEntrance" && room.Name != "Throne" && room.Name != "Tutorial" && room.Name != "Ending" && room.Name != "Compass" && room.Name != "DEBUG_ROOM" && room.Name != "ChallengeBoss") {
                if (room.Width % 1320 != 0) {
                    throw new Exception(string.Concat(new object[] {
                        "Room Name: ",
                        room.Name,
                        " is not a width divisible by ",
                        1320,
                        ". Cannot parse the file."
                    }));
                }
                if (room.Height % 720 != 0) {
                    throw new Exception(string.Concat(new object[] {
                        "Room Name: ",
                        room.Name,
                        " is not a height divisible by ",
                        720,
                        ". Cannot parse the file."
                    }));
                }
                int num = room.Width / 1320;
                int num2 = room.Height / 720;
                if (!room.IsDLCMap) {
                    List<RoomObj>[,] array = null;
                    switch (levelType) {
                        case GameTypes.LevelType.CASTLE:
                            array = LevelBuilder2.m_castleRoomArray;
                            break;
                        case GameTypes.LevelType.GARDEN:
                            array = LevelBuilder2.m_gardenRoomArray;
                            break;
                        case GameTypes.LevelType.DUNGEON:
                            array = LevelBuilder2.m_dungeonRoomArray;
                            break;
                        case GameTypes.LevelType.TOWER:
                            array = LevelBuilder2.m_towerRoomArray;
                            break;
                    }
                    array[num - 1, num2 - 1].Add(room.Clone() as RoomObj);
                    RoomObj roomObj = room.Clone() as RoomObj;
                    roomObj.Reverse();
                    array[num - 1, num2 - 1].Add(roomObj);
                    return;
                }
                List<RoomObj> sequencedDLCRoomList = LevelBuilder2.GetSequencedDLCRoomList(levelType);
                sequencedDLCRoomList.Add(room.Clone() as RoomObj);
                RoomObj roomObj2 = room.Clone() as RoomObj;
                roomObj2.Reverse();
                sequencedDLCRoomList.Add(roomObj2);
            }
        }

        public static void StoreSpecialRoom(RoomObj room, GameTypes.LevelType levelType, bool storeDebug = false) {
            if (storeDebug) {
                LevelBuilder2.m_testRoom = (room.Clone() as RoomObj);
                LevelBuilder2.m_testRoom.LevelType = LevelEV.TESTROOM_LEVELTYPE;
            }
            string name;
            switch (name = room.Name) {
                case "Start":
                    if (LevelBuilder2.m_startingRoom == null) {
                        LevelBuilder2.m_startingRoom = new StartingRoomObj();
                        LevelBuilder2.m_startingRoom.CopyRoomProperties(room);
                        LevelBuilder2.m_startingRoom.CopyRoomObjects(room);
                        return;
                    }
                    break;
                case "Linker": {
                    RoomObj roomObj = room.Clone() as RoomObj;
                    switch (levelType) {
                        case GameTypes.LevelType.CASTLE:
                            LevelBuilder2.m_linkerCastleRoom = roomObj;
                            break;
                        case GameTypes.LevelType.GARDEN:
                            LevelBuilder2.m_linkerGardenRoom = roomObj;
                            break;
                        case GameTypes.LevelType.DUNGEON:
                            LevelBuilder2.m_linkerDungeonRoom = roomObj;
                            break;
                        case GameTypes.LevelType.TOWER:
                            LevelBuilder2.m_linkerTowerRoom = roomObj;
                            break;
                    }
                    TeleporterObj teleporterObj = new TeleporterObj();
                    teleporterObj.Position = new Vector2(roomObj.X + (float)roomObj.Width / 2f - ((float)teleporterObj.Bounds.Right - teleporterObj.AnchorX), roomObj.Y + (float)roomObj.Height - 60f);
                    roomObj.GameObjList.Add(teleporterObj);
                    return;
                }
                case "Boss":
                    foreach (DoorObj current in room.DoorList) {
                        if (current.IsBossDoor)
                            current.Locked = true;
                    }
                    LevelBuilder2.m_bossRoomArray.Add(room.Clone() as RoomObj);
                    return;
                case "EntranceBoss": {
                    RoomObj roomObj2 = room.Clone() as RoomObj;
                    TeleporterObj teleporterObj2 = new TeleporterObj();
                    teleporterObj2.Position = new Vector2(roomObj2.X + (float)roomObj2.Width / 2f - ((float)teleporterObj2.Bounds.Right - teleporterObj2.AnchorX), roomObj2.Y + (float)roomObj2.Height - 60f);
                    roomObj2.GameObjList.Add(teleporterObj2);
                    switch (levelType) {
                        case GameTypes.LevelType.CASTLE:
                            LevelBuilder2.m_bossCastleEntranceRoom = roomObj2;
                            return;
                        case GameTypes.LevelType.GARDEN:
                            LevelBuilder2.m_bossGardenEntranceRoom = roomObj2;
                            return;
                        case GameTypes.LevelType.DUNGEON:
                            LevelBuilder2.m_bossDungeonEntranceRoom = roomObj2;
                            return;
                        case GameTypes.LevelType.TOWER:
                            LevelBuilder2.m_bossTowerEntranceRoom = roomObj2;
                            return;
                        default:
                            return;
                    }
                    break;
                }
                case "CastleEntrance":
                    if (LevelBuilder2.m_castleEntranceRoom == null) {
                        LevelBuilder2.m_castleEntranceRoom = new CastleEntranceRoomObj();
                        LevelBuilder2.m_castleEntranceRoom.CopyRoomProperties(room);
                        LevelBuilder2.m_castleEntranceRoom.CopyRoomObjects(room);
                        LevelBuilder2.m_castleEntranceRoom.LevelType = GameTypes.LevelType.CASTLE;
                        return;
                    }
                    break;
                case "Compass":
                    if (LevelBuilder2.m_compassRoom == null) {
                        LevelBuilder2.m_compassRoom = new CompassRoomObj();
                        LevelBuilder2.m_compassRoom.CopyRoomProperties(room);
                        LevelBuilder2.m_compassRoom.CopyRoomObjects(room);
                        return;
                    }
                    break;
                case "Secret": {
                    List<RoomObj> list = null;
                    switch (levelType) {
                        case GameTypes.LevelType.CASTLE:
                            list = LevelBuilder2.m_secretCastleRoomArray;
                            break;
                        case GameTypes.LevelType.GARDEN:
                            list = LevelBuilder2.m_secretGardenRoomArray;
                            break;
                        case GameTypes.LevelType.DUNGEON:
                            list = LevelBuilder2.m_secretDungeonRoomArray;
                            break;
                        case GameTypes.LevelType.TOWER:
                            list = LevelBuilder2.m_secretTowerRoomArray;
                            break;
                    }
                    list.Add(room.Clone() as RoomObj);
                    RoomObj roomObj3 = room.Clone() as RoomObj;
                    roomObj3.Reverse();
                    list.Add(roomObj3);
                    return;
                }
                case "Bonus": {
                    List<RoomObj> list2 = null;
                    switch (levelType) {
                        case GameTypes.LevelType.CASTLE:
                            list2 = LevelBuilder2.m_bonusCastleRoomArray;
                            break;
                        case GameTypes.LevelType.GARDEN:
                            list2 = LevelBuilder2.m_bonusGardenRoomArray;
                            break;
                        case GameTypes.LevelType.DUNGEON:
                            list2 = LevelBuilder2.m_bonusDungeonRoomArray;
                            break;
                        case GameTypes.LevelType.TOWER:
                            list2 = LevelBuilder2.m_bonusTowerRoomArray;
                            break;
                    }
                    list2.Add(room.Clone() as RoomObj);
                    RoomObj roomObj4 = room.Clone() as RoomObj;
                    roomObj4.Reverse();
                    list2.Add(roomObj4);
                    return;
                }
                case "Tutorial":
                    if (LevelBuilder2.m_tutorialRoom == null) {
                        LevelBuilder2.m_tutorialRoom = new TutorialRoomObj();
                        LevelBuilder2.m_tutorialRoom.CopyRoomProperties(room);
                        LevelBuilder2.m_tutorialRoom.CopyRoomObjects(room);
                        return;
                    }
                    break;
                case "Throne":
                    if (LevelBuilder2.m_throneRoom == null) {
                        LevelBuilder2.m_throneRoom = new ThroneRoomObj();
                        LevelBuilder2.m_throneRoom.CopyRoomProperties(room);
                        LevelBuilder2.m_throneRoom.CopyRoomObjects(room);
                        return;
                    }
                    break;
                case "Ending":
                    if (LevelBuilder2.m_endingRoom == null) {
                        LevelBuilder2.m_endingRoom = new EndingRoomObj();
                        LevelBuilder2.m_endingRoom.CopyRoomProperties(room);
                        LevelBuilder2.m_endingRoom.CopyRoomObjects(room);
                        return;
                    }
                    break;
                case "ChallengeBoss":
                    foreach (DoorObj current2 in room.DoorList) {
                        if (current2.IsBossDoor)
                            current2.Locked = true;
                    }
                    LevelBuilder2.m_challengeRoomArray.Add(room.Clone() as RoomObj);
                    break;

                    return;
            }
        }

        public static List<RoomObj> CreateArea(int areaSize, AreaStruct areaInfo, List<RoomObj> roomsToCheckCollisionsList, RoomObj startingRoom, bool firstRoom) {
            bool flag = false;
            float num = -100f;
            float num2 = 100f / areaSize;
            if (areaInfo.BossInArea)
                num = 0f;
            else
                flag = true;
            int num3 = CDGMath.RandomInt((int)areaInfo.SecretRooms.X, (int)areaInfo.SecretRooms.Y);
            int num4 = num3;
            int num5 = areaSize / (num3 + 1);
            int num6 = num5;
            List<RoomObj> list = new List<RoomObj>();
            List<RoomObj> list2 = null;
            List<RoomObj> list3 = null;
            switch (areaInfo.LevelType) {
                case GameTypes.LevelType.CASTLE:
                    list2 = LevelBuilder2.m_secretCastleRoomArray;
                    list3 = LevelBuilder2.m_bonusCastleRoomArray;
                    break;
                case GameTypes.LevelType.GARDEN:
                    list2 = LevelBuilder2.m_secretGardenRoomArray;
                    list3 = LevelBuilder2.m_bonusGardenRoomArray;
                    break;
                case GameTypes.LevelType.DUNGEON:
                    list2 = LevelBuilder2.m_secretDungeonRoomArray;
                    list3 = LevelBuilder2.m_bonusDungeonRoomArray;
                    break;
                case GameTypes.LevelType.TOWER:
                    list2 = LevelBuilder2.m_secretTowerRoomArray;
                    list3 = LevelBuilder2.m_bonusTowerRoomArray;
                    break;
            }
            list.AddRange(list2);
            int num7 = CDGMath.RandomInt((int)areaInfo.BonusRooms.X, (int)areaInfo.BonusRooms.Y);
            int num8 = num7;
            int num9 = areaSize / (num7 + 1);
            int num10 = num9;
            List<RoomObj> list4 = new List<RoomObj>();
            list4.AddRange(list3);
            if (areaInfo.SecretRooms.Y > list2.Count) {
                throw new Exception(string.Concat(new object[] {
                    "Cannot add ",
                    (int)areaInfo.SecretRooms.Y,
                    " secret rooms from pool of ",
                    list2.Count,
                    " secret rooms."
                }));
            }
            if (areaInfo.BonusRooms.Y > list3.Count) {
                throw new Exception(string.Concat(new object[] {
                    "Cannot add ",
                    (int)areaInfo.BonusRooms.Y,
                    " bonus rooms from pool of ",
                    list3.Count,
                    " bonus rooms."
                }));
            }
            GameTypes.LevelType levelType = areaInfo.LevelType;
            List<RoomObj> list5 = new List<RoomObj>();
            list5.AddRange(roomsToCheckCollisionsList);
            List<DoorObj> list6 = new List<DoorObj>();
            List<RoomObj> list7 = new List<RoomObj>();
            int i = areaSize;
            int num11 = 0;
            int num12 = 0;
            int num13 = 0;
            int num14 = 0;
            string text = "NONE";
            switch (levelType) {
                case GameTypes.LevelType.CASTLE:
                    num11 = 90;
                    num12 = 90;
                    num13 = 90;
                    num14 = 90;
                    text = "Right";
                    break;
                case GameTypes.LevelType.GARDEN:
                    num11 = 70;
                    num12 = 100;
                    num13 = 45;
                    num14 = 45;
                    text = "Right";
                    break;
                case GameTypes.LevelType.DUNGEON:
                    num11 = 55;
                    num12 = 55;
                    num13 = 45;
                    num14 = 100;
                    text = "Bottom";
                    break;
                case GameTypes.LevelType.TOWER:
                    num11 = 45;
                    num12 = 45;
                    num13 = 100;
                    num14 = 60;
                    text = "Top";
                    break;
            }
            DoorObj doorObj = null;
            if (firstRoom) {
                list7.Add(startingRoom);
                list5.Add(startingRoom);
                startingRoom.LevelType = GameTypes.LevelType.NONE;
                i--;
                LevelBuilder2.MoveRoom(startingRoom, Vector2.Zero);
                RoomObj roomObj = LevelBuilder2.m_castleEntranceRoom.Clone() as RoomObj;
                list7.Add(roomObj);
                list5.Add(roomObj);
                i--;
                LevelBuilder2.MoveRoom(roomObj, new Vector2(startingRoom.X + (float)startingRoom.Width, (float)(startingRoom.Bounds.Bottom - roomObj.Height)));
                startingRoom = roomObj;
            }
            foreach (DoorObj current in startingRoom.DoorList) {
                if (current.DoorPosition == text) {
                    list6.Add(current);
                    doorObj = current;
                    break;
                }
            }
            if (list6.Count == 0)
                throw new Exception("The starting room does not have a " + text + " positioned door. Cannot create level.");
            while (i > 0) {
                if (list6.Count <= 0) {
                    Console.WriteLine("ERROR: Ran out of available rooms to make.");
                    break;
                }
                bool flag2 = false;
                DoorObj doorObj2 = list6[0];
                if ((list6.Count <= 5 && doorObj2 != doorObj && i > 0) || doorObj2 == doorObj)
                    flag2 = true;
                else {
                    int num15 = 100;
                    string doorPosition;
                    if ((doorPosition = doorObj2.DoorPosition) != null) {
                        if (!(doorPosition == "Left")) {
                            if (!(doorPosition == "Right")) {
                                if (!(doorPosition == "Top")) {
                                    if (doorPosition == "Bottom")
                                        num15 = num14;
                                }
                                else
                                    num15 = num13;
                            }
                            else
                                num15 = num12;
                        }
                        else
                            num15 = num11;
                    }
                    if (num15 - CDGMath.RandomInt(1, 100) >= 0)
                        flag2 = true;
                }
                if (flag2) {
                    List<DoorObj> list8 = null;
                    bool flag3 = false;
                    if (num >= (float)CDGMath.RandomInt(50, 100) && !flag) {
                        RoomObj roomObj2 = null;
                        switch (areaInfo.LevelType) {
                            case GameTypes.LevelType.CASTLE:
                                roomObj2 = LevelBuilder2.m_bossCastleEntranceRoom;
                                break;
                            case GameTypes.LevelType.GARDEN:
                                roomObj2 = LevelBuilder2.m_bossGardenEntranceRoom;
                                break;
                            case GameTypes.LevelType.DUNGEON:
                                roomObj2 = LevelBuilder2.m_bossDungeonEntranceRoom;
                                break;
                            case GameTypes.LevelType.TOWER:
                                roomObj2 = LevelBuilder2.m_bossTowerEntranceRoom;
                                break;
                        }
                        flag3 = true;
                        string oppositeDoorPosition = LevelBuilder2.GetOppositeDoorPosition(doorObj2.DoorPosition);
                        list8 = new List<DoorObj>();
                        using (List<DoorObj>.Enumerator enumerator2 = roomObj2.DoorList.GetEnumerator()) {
                            while (enumerator2.MoveNext()) {
                                DoorObj current2 = enumerator2.Current;
                                if (current2.DoorPosition == oppositeDoorPosition && !LevelBuilder2.CheckForRoomCollision(doorObj2, list5, current2) && !list8.Contains(current2)) {
                                    flag = true;
                                    list8.Add(current2);
                                    break;
                                }
                            }
                            goto IL_556;
                        }
                        goto IL_552;
                    }
                    goto IL_552;
                    IL_556:
                    bool flag4 = false;
                    bool flag5 = false;
                    if ((flag3 && !flag) || !flag3) {
                        if (list7.Count >= num6 && num3 > 0) {
                            flag4 = true;
                            flag5 = true;
                            list8 = LevelBuilder2.FindSuitableDoors(doorObj2, list, list5);
                        }
                        else if (list7.Count >= num10 && num7 > 0) {
                            flag4 = true;
                            list8 = LevelBuilder2.FindSuitableDoors(doorObj2, list4, list5);
                        }
                        if (!flag4 || (flag4 && list8.Count == 0)) {
                            if (list7.Count < 5)
                                list8 = LevelBuilder2.FindSuitableDoors(doorObj2, 4, 4, list5, levelType, true);
                            else
                                list8 = LevelBuilder2.FindSuitableDoors(doorObj2, 4, 4, list5, levelType, false);
                        }
                        else if (flag5) {
                            num6 = list7.Count + num5;
                            num3--;
                        }
                        else if (!flag5) {
                            num10 = list7.Count + num9;
                            num7--;
                        }
                    }
                    if (list8.Count == 0) {
                        list6.Remove(doorObj2);
                        continue;
                    }
                    int index = CDGMath.RandomInt(0, list8.Count - 1);
                    CDGMath.Shuffle<DoorObj>(list8);
                    DoorObj doorObj3 = list8[index];
                    if (flag4) {
                        if (flag5)
                            list.Remove(doorObj3.Room);
                        else if (!flag5)
                            list4.Remove(doorObj3.Room);
                    }
                    RoomObj roomObj3 = doorObj3.Room.Clone() as RoomObj;
                    foreach (DoorObj current3 in roomObj3.DoorList) {
                        if (current3.Position == doorObj3.Position) {
                            doorObj3 = current3;
                            break;
                        }
                    }
                    roomObj3.LevelType = levelType;
                    roomObj3.TextureColor = areaInfo.Color;
                    list7.Add(roomObj3);
                    list5.Add(roomObj3);
                    Vector2 zero = Vector2.Zero;
                    string doorPosition2;
                    if ((doorPosition2 = doorObj2.DoorPosition) != null) {
                        if (!(doorPosition2 == "Left")) {
                            if (!(doorPosition2 == "Right")) {
                                if (!(doorPosition2 == "Top")) {
                                    if (doorPosition2 == "Bottom")
                                        zero = new Vector2(doorObj2.X - (doorObj3.X - doorObj3.Room.X), doorObj2.Y + (float)doorObj2.Height);
                                }
                                else
                                    zero = new Vector2(doorObj2.X - (doorObj3.X - doorObj3.Room.X), doorObj2.Y - (float)doorObj3.Room.Height);
                            }
                            else
                                zero = new Vector2(doorObj2.X + (float)doorObj2.Width, doorObj2.Y - (doorObj3.Y - doorObj3.Room.Y));
                        }
                        else
                            zero = new Vector2(doorObj2.X - (float)doorObj3.Room.Width, doorObj2.Y - (doorObj3.Y - doorObj3.Room.Y));
                    }
                    LevelBuilder2.MoveRoom(roomObj3, zero);
                    i--;
                    list6.Remove(doorObj2);
                    foreach (DoorObj current4 in roomObj3.DoorList) {
                        if (current4 != doorObj3 && current4.Room != startingRoom && current4.X >= (float)LevelBuilder2.m_startingRoom.Width)
                            list6.Add(current4);
                    }
                    doorObj3.Attached = true;
                    doorObj2.Attached = true;
                    continue;
                    IL_552:
                    num += num2;
                    goto IL_556;
                }
                list6.Remove(doorObj2);
            }
            if (num3 != 0) {
                Console.WriteLine(string.Concat(new object[] {
                    "WARNING: Only ",
                    num4 - num3,
                    " secret rooms of ",
                    num4,
                    " creation attempts were successful"
                }));
            }
            if (num7 != 0) {
                Console.WriteLine(string.Concat(new object[] {
                    "WARNING: Only ",
                    num8 - num7,
                    " secret rooms of ",
                    num8,
                    " creation attempts were successful"
                }));
            }
            return list7;
        }

        private static List<DoorObj> FindSuitableDoors(DoorObj doorToCheck, int roomWidth, int roomHeight, List<RoomObj> roomList, GameTypes.LevelType levelType, bool findRoomsWithMoreDoors) {
            List<DoorObj> list = new List<DoorObj>();
            string oppositeDoorPosition = LevelBuilder2.GetOppositeDoorPosition(doorToCheck.DoorPosition);
            for (int i = 1; i <= roomWidth; i++) {
                for (int j = 1; j <= roomHeight; j++) {
                    List<RoomObj> roomList2 = LevelBuilder2.GetRoomList(i, j, levelType);
                    foreach (RoomObj current in roomList2) {
                        bool flag = false;
                        if (!findRoomsWithMoreDoors || (findRoomsWithMoreDoors && current.DoorList.Count > 1))
                            flag = true;
                        if (flag) {
                            foreach (DoorObj current2 in current.DoorList) {
                                if (current2.DoorPosition == oppositeDoorPosition && !LevelBuilder2.CheckForRoomCollision(doorToCheck, roomList, current2) && !list.Contains(current2))
                                    list.Add(current2);
                            }
                        }
                    }
                }
            }
            List<RoomObj> sequencedDLCRoomList = LevelBuilder2.GetSequencedDLCRoomList(levelType);
            foreach (RoomObj current3 in sequencedDLCRoomList) {
                bool flag2 = false;
                if (!findRoomsWithMoreDoors || (findRoomsWithMoreDoors && current3.DoorList.Count > 1))
                    flag2 = true;
                if (flag2) {
                    foreach (DoorObj current4 in current3.DoorList) {
                        if (current4.DoorPosition == oppositeDoorPosition && !LevelBuilder2.CheckForRoomCollision(doorToCheck, roomList, current4) && !list.Contains(current4))
                            list.Add(current4);
                    }
                }
            }
            return list;
        }

        private static List<DoorObj> FindSuitableDoors(DoorObj doorToCheck, List<RoomObj> roomList, List<RoomObj> roomCollisionList) {
            List<DoorObj> list = new List<DoorObj>();
            string oppositeDoorPosition = LevelBuilder2.GetOppositeDoorPosition(doorToCheck.DoorPosition);
            foreach (RoomObj current in roomList) {
                if (Game.PlayerStats.DiaryEntry < 24 || !(current.Name == "Bonus") || !(current.Tag == 6.ToString())) {
                    foreach (DoorObj current2 in current.DoorList) {
                        if (current2.DoorPosition == oppositeDoorPosition && !LevelBuilder2.CheckForRoomCollision(doorToCheck, roomCollisionList, current2) && !list.Contains(current2))
                            list.Add(current2);
                    }
                }
            }
            return list;
        }

        private static void RemoveDoorFromRoom(DoorObj doorToRemove) {
            RoomObj room = doorToRemove.Room;
            TerrainObj terrainObj = new TerrainObj(doorToRemove.Width, doorToRemove.Height);
            terrainObj.AddCollisionBox(0, 0, terrainObj.Width, terrainObj.Height, 0);
            terrainObj.AddCollisionBox(0, 0, terrainObj.Width, terrainObj.Height, 2);
            terrainObj.Position = doorToRemove.Position;
            room.TerrainObjList.Add(terrainObj);
            BorderObj borderObj = new BorderObj();
            borderObj.Position = terrainObj.Position;
            borderObj.SetWidth(terrainObj.Width);
            borderObj.SetHeight(terrainObj.Height);
            string doorPosition;
            if ((doorPosition = doorToRemove.DoorPosition) != null) {
                if (!(doorPosition == "Left")) {
                    if (!(doorPosition == "Right")) {
                        if (!(doorPosition == "Top")) {
                            if (doorPosition == "Bottom")
                                borderObj.BorderTop = true;
                        }
                        else
                            borderObj.BorderBottom = true;
                    }
                    else
                        borderObj.BorderLeft = true;
                }
                else
                    borderObj.BorderRight = true;
            }
            room.BorderList.Add(borderObj);
            room.DoorList.Remove(doorToRemove);
            doorToRemove.Dispose();
        }

        public static void CloseRemainingDoors(List<RoomObj> roomList) {
            List<DoorObj> list = new List<DoorObj>();
            List<DoorObj> list2 = new List<DoorObj>();
            foreach (RoomObj current in roomList) {
                foreach (DoorObj current2 in current.DoorList) {
                    if (current2.DoorPosition != "None") {
                        list2.Add(current2);
                        if (!current2.Attached)
                            list.Add(current2);
                    }
                }
            }
            foreach (DoorObj current3 in list) {
                bool flag = true;
                foreach (DoorObj current4 in list2) {
                    string doorPosition;
                    if (current3 != current4 && (doorPosition = current3.DoorPosition) != null) {
                        if (!(doorPosition == "Left")) {
                            if (!(doorPosition == "Right")) {
                                if (!(doorPosition == "Top")) {
                                    if (!(doorPosition == "Bottom")) {
                                        if (doorPosition == "None")
                                            flag = false;
                                    }
                                    else if (current4.Y > current3.Y && CollisionMath.Intersects(new Rectangle((int)(current3.X - 5f), (int)(current3.Y + 5f), current3.Width, current3.Height), current4.Bounds))
                                        flag = false;
                                }
                                else if (current4.Y < current3.Y && CollisionMath.Intersects(new Rectangle((int)current3.X, (int)(current3.Y - 5f), current3.Width, current3.Height), current4.Bounds))
                                    flag = false;
                            }
                            else if (current4.X > current3.X && CollisionMath.Intersects(new Rectangle((int)(current3.X + 5f), (int)current3.Y, current3.Width, current3.Height), current4.Bounds))
                                flag = false;
                        }
                        else if (current4.X < current3.X && CollisionMath.Intersects(new Rectangle((int)(current3.X - 5f), (int)current3.Y, current3.Width, current3.Height), current4.Bounds))
                            flag = false;
                    }
                }
                if (flag)
                    LevelBuilder2.RemoveDoorFromRoom(current3);
                else
                    current3.Attached = true;
            }
        }

        public static void AddDoorBorders(List<RoomObj> roomList) {
            foreach (RoomObj current in roomList) {
                foreach (DoorObj current2 in current.DoorList) {
                    string doorPosition;
                    if ((doorPosition = current2.DoorPosition) != null) {
                        if (!(doorPosition == "Left") && !(doorPosition == "Right")) {
                            if (doorPosition == "Top" || doorPosition == "Bottom") {
                                int num = 0;
                                BorderObj borderObj = new BorderObj();
                                borderObj.Position = new Vector2(current2.Room.X + (current2.X - current2.Room.X) + (float)current2.Width, current2.Room.Y + (current2.Y - current2.Room.Y) + (float)num);
                                borderObj.SetWidth(60);
                                borderObj.SetHeight(current2.Height);
                                borderObj.BorderLeft = true;
                                current2.Room.BorderList.Add(borderObj);
                                BorderObj borderObj2 = new BorderObj();
                                borderObj2.Position = new Vector2(current2.Room.X + (current2.X - current2.Room.X) - 60f, current2.Room.Y + (current2.Y - current2.Room.Y) + (float)num);
                                borderObj2.SetWidth(60);
                                borderObj2.SetHeight(current2.Height);
                                borderObj2.BorderRight = true;
                                current2.Room.BorderList.Add(borderObj2);
                            }
                        }
                        else {
                            BorderObj borderObj3 = new BorderObj();
                            borderObj3.Position = new Vector2(current2.Room.X + (current2.X - current2.Room.X), current2.Room.Y + (current2.Y - current2.Room.Y) - 60f);
                            borderObj3.SetWidth(current2.Width);
                            borderObj3.SetHeight(60);
                            borderObj3.BorderBottom = true;
                            current2.Room.BorderList.Add(borderObj3);
                            BorderObj borderObj4 = new BorderObj();
                            borderObj4.Position = new Vector2(current2.Room.X + (current2.X - current2.Room.X), current2.Room.Y + (current2.Y - current2.Room.Y) + (float)current2.Height);
                            borderObj4.SetWidth(current2.Width);
                            borderObj4.SetHeight(60);
                            borderObj4.BorderTop = true;
                            current2.Room.BorderList.Add(borderObj4);
                        }
                    }
                }
            }
        }

        public static DoorObj FindFurthestDoor(List<RoomObj> roomList, string furthestRoomDirection, string doorPositionWanted, bool addLinkerRoom, bool castleOnly) {
            string oppositeDoorPosition = LevelBuilder2.GetOppositeDoorPosition(doorPositionWanted);
            RoomObj roomObj = roomList[0];
            float num = -10f;
            DoorObj doorObj = null;
            DoorObj doorObj2 = null;
            foreach (RoomObj current in roomList) {
                if (current != roomObj && ((current.LevelType == GameTypes.LevelType.CASTLE && castleOnly) || !castleOnly)) {
                    float num2 = 0f;
                    if (furthestRoomDirection != null) {
                        if (!(furthestRoomDirection == "Right")) {
                            if (!(furthestRoomDirection == "Left")) {
                                if (!(furthestRoomDirection == "Top")) {
                                    if (furthestRoomDirection == "Bottom")
                                        num2 = current.Y - roomObj.Y;
                                }
                                else
                                    num2 = roomObj.Y - current.Y;
                            }
                            else
                                num2 = roomObj.X - current.X;
                        }
                        else
                            num2 = current.X - roomObj.X;
                    }
                    if (num2 >= num && (doorObj == null || num2 > num)) {
                        doorObj = null;
                        foreach (DoorObj current2 in current.DoorList) {
                            if (current2.DoorPosition != "None") {
                                if (current2.DoorPosition == doorPositionWanted) {
                                    bool flag = true;
                                    foreach (RoomObj current3 in roomList) {
                                        if (current3 != current2.Room && CollisionMath.Intersects(new Rectangle((int)current3.X - 10, (int)current3.Y - 10, current3.Width + 20, current3.Height + 20), current2.Bounds)) {
                                            flag = false;
                                            break;
                                        }
                                    }
                                    if (flag) {
                                        doorObj2 = current2;
                                        num = num2;
                                        break;
                                    }
                                }
                                else if (current2.DoorPosition != oppositeDoorPosition) {
                                    bool flag2 = true;
                                    foreach (RoomObj current4 in roomList) {
                                        if (current4 != current2.Room && CollisionMath.Intersects(new Rectangle((int)current4.X - 10, (int)current4.Y - 10, current4.Width + 20, current4.Height + 20), current2.Bounds)) {
                                            flag2 = false;
                                            break;
                                        }
                                    }
                                    if (flag2) {
                                        num = num2;
                                        doorObj2 = current2;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (doorObj2 == null) {
                Console.WriteLine("Could not find suitable furthest door. That's a problem");
                return null;
            }
            if (addLinkerRoom)
                return LevelBuilder2.AddLinkerToRoom(roomList, doorObj2, doorPositionWanted);
            return doorObj2;
        }

        public static DoorObj AddLinkerToRoom(List<RoomObj> roomList, DoorObj needsLinking, string doorPositionWanted) {
            DoorObj result = null;
            RoomObj roomObj = null;
            switch (needsLinking.Room.LevelType) {
                case GameTypes.LevelType.CASTLE:
                    roomObj = (LevelBuilder2.m_linkerCastleRoom.Clone() as RoomObj);
                    break;
                case GameTypes.LevelType.GARDEN:
                    roomObj = (LevelBuilder2.m_linkerGardenRoom.Clone() as RoomObj);
                    break;
                case GameTypes.LevelType.DUNGEON:
                    roomObj = (LevelBuilder2.m_linkerDungeonRoom.Clone() as RoomObj);
                    break;
                case GameTypes.LevelType.TOWER:
                    roomObj = (LevelBuilder2.m_linkerTowerRoom.Clone() as RoomObj);
                    break;
            }
            roomObj.TextureColor = needsLinking.Room.TextureColor;
            DoorObj doorObj = null;
            string oppositeDoorPosition = LevelBuilder2.GetOppositeDoorPosition(needsLinking.DoorPosition);
            foreach (DoorObj current in roomObj.DoorList) {
                if (current.DoorPosition == oppositeDoorPosition) {
                    doorObj = current;
                    break;
                }
            }
            Vector2 zero = Vector2.Zero;
            string doorPosition;
            if ((doorPosition = needsLinking.DoorPosition) != null) {
                if (!(doorPosition == "Left")) {
                    if (!(doorPosition == "Right")) {
                        if (!(doorPosition == "Top")) {
                            if (doorPosition == "Bottom")
                                zero = new Vector2(needsLinking.X - (doorObj.X - doorObj.Room.X), needsLinking.Y + (float)needsLinking.Height);
                        }
                        else
                            zero = new Vector2(needsLinking.X - (doorObj.X - doorObj.Room.X), needsLinking.Y - (float)doorObj.Room.Height);
                    }
                    else
                        zero = new Vector2(needsLinking.X + (float)needsLinking.Width, needsLinking.Y - (doorObj.Y - doorObj.Room.Y));
                }
                else
                    zero = new Vector2(needsLinking.X - (float)doorObj.Room.Width, needsLinking.Y - (doorObj.Y - doorObj.Room.Y));
            }
            LevelBuilder2.MoveRoom(roomObj, zero);
            needsLinking.Attached = true;
            doorObj.Attached = true;
            for (int i = 0; i < roomObj.DoorList.Count; i++) {
                DoorObj doorObj2 = roomObj.DoorList[i];
                if (doorObj2.DoorPosition == doorPositionWanted)
                    result = doorObj2;
            }
            roomObj.LevelType = needsLinking.Room.LevelType;
            roomList.Add(roomObj);
            return result;
        }

        public static void AddRemoveExtraObjects(List<RoomObj> roomList) {
            foreach (RoomObj current in roomList) {
                LevelBuilder2.hasTopDoor = false;
                LevelBuilder2.hasBottomDoor = false;
                LevelBuilder2.hasLeftDoor = false;
                LevelBuilder2.hasRightDoor = false;
                LevelBuilder2.hasTopLeftDoor = false;
                LevelBuilder2.hasTopRightDoor = false;
                LevelBuilder2.hasBottomLeftDoor = false;
                LevelBuilder2.hasBottomRightDoor = false;
                LevelBuilder2.hasRightTopDoor = false;
                LevelBuilder2.hasRightBottomDoor = false;
                LevelBuilder2.hasLeftTopDoor = false;
                LevelBuilder2.hasLeftBottomDoor = false;
                foreach (DoorObj current2 in current.DoorList) {
                    string doorPosition;
                    if ((doorPosition = current2.DoorPosition) != null) {
                        if (!(doorPosition == "Top")) {
                            if (!(doorPosition == "Bottom")) {
                                if (!(doorPosition == "Left")) {
                                    if (doorPosition == "Right") {
                                        LevelBuilder2.hasRightDoor = true;
                                        if (current2.Y - current.Y == 240f)
                                            LevelBuilder2.hasRightTopDoor = true;
                                        if ((float)current.Bounds.Bottom - current2.Y == 480f)
                                            LevelBuilder2.hasRightBottomDoor = true;
                                    }
                                }
                                else {
                                    LevelBuilder2.hasLeftDoor = true;
                                    if (current2.Y - current.Y == 240f)
                                        LevelBuilder2.hasLeftTopDoor = true;
                                    if ((float)current.Bounds.Bottom - current2.Y == 480f)
                                        LevelBuilder2.hasLeftBottomDoor = true;
                                }
                            }
                            else {
                                LevelBuilder2.hasBottomDoor = true;
                                if (current2.X - current.X == 540f)
                                    LevelBuilder2.hasBottomLeftDoor = true;
                                if ((float)current.Bounds.Right - current2.X == 780f)
                                    LevelBuilder2.hasBottomRightDoor = true;
                            }
                        }
                        else {
                            LevelBuilder2.hasTopDoor = true;
                            if (current2.X - current.X == 540f)
                                LevelBuilder2.hasTopLeftDoor = true;
                            if ((float)current.Bounds.Right - current2.X == 780f)
                                LevelBuilder2.hasTopRightDoor = true;
                        }
                    }
                }
                LevelBuilder2.RemoveFromListHelper(current.TerrainObjList);
                LevelBuilder2.RemoveFromListHelper(current.GameObjList);
                LevelBuilder2.RemoveFromListHelper(current.EnemyList);
                LevelBuilder2.RemoveFromListHelper(current.BorderList);
            }
        }

        private static void RemoveFromListHelper<T>(List<T> list) {
            for (int i = 0; i < list.Count; i++) {
                string name = (list[i] as GameObj).Name;
                if (name != null) {
                    if ((!LevelBuilder2.hasTopLeftDoor && name.IndexOf("TopLeft") != -1 && name.IndexOf("!TopLeft") == -1) || (LevelBuilder2.hasTopLeftDoor && name.IndexOf("!TopLeft") != -1) || (!LevelBuilder2.hasTopRightDoor && name.IndexOf("TopRight") != -1 && name.IndexOf("!TopRight") == -1) || (LevelBuilder2.hasTopRightDoor && name.IndexOf("!TopRight") != -1) || (!LevelBuilder2.hasTopDoor && name.IndexOf("Top") != -1 && name.IndexOf("!Top") == -1 && name.Length == 3) || (LevelBuilder2.hasTopDoor && name.IndexOf("!Top") != -1 && name.Length == 4)) {
                        list.Remove(list[i]);
                        i--;
                    }
                    if ((!LevelBuilder2.hasBottomLeftDoor && name.IndexOf("BottomLeft") != -1 && name.IndexOf("!BottomLeft") == -1) || (LevelBuilder2.hasBottomLeftDoor && name.IndexOf("!BottomLeft") != -1) || (!LevelBuilder2.hasBottomRightDoor && name.IndexOf("BottomRight") != -1 && name.IndexOf("!BottomRight") == -1) || (LevelBuilder2.hasBottomRightDoor && name.IndexOf("!BottomRight") != -1) || (!LevelBuilder2.hasBottomDoor && name.IndexOf("Bottom") != -1 && name.IndexOf("!Bottom") == -1 && name.Length == 6) || (LevelBuilder2.hasBottomDoor && name.IndexOf("!Bottom") != -1 && name.Length == 7)) {
                        list.Remove(list[i]);
                        i--;
                    }
                    if ((!LevelBuilder2.hasLeftTopDoor && name.IndexOf("LeftTop") != -1 && name.IndexOf("!LeftTop") == -1) || (LevelBuilder2.hasLeftTopDoor && name.IndexOf("!LeftTop") != -1) || (!LevelBuilder2.hasLeftBottomDoor && name.IndexOf("LeftBottom") != -1 && name.IndexOf("!LeftBottom") == -1) || (LevelBuilder2.hasLeftBottomDoor && name.IndexOf("!LeftBottom") != -1) || (!LevelBuilder2.hasLeftDoor && name.IndexOf("Left") != -1 && name.IndexOf("!Left") == -1 && name.Length == 4) || (LevelBuilder2.hasLeftDoor && name.IndexOf("!Left") != -1 && name.Length == 5)) {
                        list.Remove(list[i]);
                        i--;
                    }
                    if ((!LevelBuilder2.hasRightTopDoor && name.IndexOf("RightTop") != -1 && name.IndexOf("!RightTop") == -1) || (LevelBuilder2.hasRightTopDoor && name.IndexOf("!RightTop") != -1) || (!LevelBuilder2.hasRightBottomDoor && name.IndexOf("RightBottom") != -1 && name.IndexOf("!RightBottom") == -1) || (LevelBuilder2.hasRightBottomDoor && name.IndexOf("!RightBottom") != -1) || (!LevelBuilder2.hasRightDoor && name.IndexOf("Right") != -1 && name.IndexOf("!Right") == -1 && name.Length == 5) || (LevelBuilder2.hasRightDoor && name.IndexOf("!Right") != -1 && name.Length == 6)) {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }
        }

        public static void AddProceduralEnemies(List<RoomObj> roomList) {
            Vector2 arg_0C_0 = roomList[0].Position;
            foreach (RoomObj current in roomList) {
                byte[] array;
                byte[] array2;
                switch (current.LevelType) {
                    case GameTypes.LevelType.GARDEN:
                        array = LevelEV.GARDEN_ENEMY_LIST;
                        array2 = LevelEV.GARDEN_ENEMY_DIFFICULTY_LIST;
                        goto IL_7D;
                    case GameTypes.LevelType.DUNGEON:
                        array = LevelEV.DUNGEON_ENEMY_LIST;
                        array2 = LevelEV.DUNGEON_ENEMY_DIFFICULTY_LIST;
                        goto IL_7D;
                    case GameTypes.LevelType.TOWER:
                        array = LevelEV.TOWER_ENEMY_LIST;
                        array2 = LevelEV.TOWER_ENEMY_DIFFICULTY_LIST;
                        goto IL_7D;
                    default:
                        array = LevelEV.CASTLE_ENEMY_LIST;
                        array2 = LevelEV.CASTLE_ENEMY_DIFFICULTY_LIST;
                        goto IL_7D;
                }
                IL_7D:
                if (array.Length != array2.Length)
                    throw new Exception("Cannot create enemy. Enemy pool != enemy difficulty pool - LevelBuilder2.cs - AddProceduralEnemies()");
                int num = CDGMath.RandomInt(0, array.Length - 1);
                int num2 = num;
                byte enemyType = array[num];
                byte difficulty = array2[num];
                while (num == num2)
                    num = CDGMath.RandomInt(0, array.Length - 1);
                num2 = num;
                byte enemyType2 = array[num];
                byte difficulty2 = array2[num];
                while (num == num2)
                    num = CDGMath.RandomInt(0, array.Length - 1);
                num2 = num;
                byte enemyType3 = array[num];
                byte difficulty3 = array2[num];
                while (num == num2)
                    num = CDGMath.RandomInt(0, array.Length - 1);
                num2 = num;
                byte enemyType4 = array[num];
                byte difficulty4 = array2[num];
                while (num == num2)
                    num = CDGMath.RandomInt(0, array.Length - 1);
                byte enemyType5 = array[num];
                byte difficulty5 = array2[num];
                num = CDGMath.RandomInt(0, array.Length - 1);
                byte b;
                for (b = array[num]; b == 3; b = array[num])
                    num = CDGMath.RandomInt(0, array.Length - 1);
                for (int i = 0; i < current.GameObjList.Count; i++) {
                    EnemyOrbObj enemyOrbObj = current.GameObjList[i] as EnemyOrbObj;
                    if (enemyOrbObj != null) {
                        EnemyObj enemyObj;
                        if (enemyOrbObj.OrbType == 0)
                            enemyObj = EnemyBuilder.BuildEnemy((int)enemyType, null, null, null, (GameTypes.EnemyDifficulty)difficulty, false);
                        else if (enemyOrbObj.OrbType == 1)
                            enemyObj = EnemyBuilder.BuildEnemy((int)enemyType2, null, null, null, (GameTypes.EnemyDifficulty)difficulty2, false);
                        else if (enemyOrbObj.OrbType == 2)
                            enemyObj = EnemyBuilder.BuildEnemy((int)enemyType3, null, null, null, (GameTypes.EnemyDifficulty)difficulty3, false);
                        else if (enemyOrbObj.OrbType == 3)
                            enemyObj = EnemyBuilder.BuildEnemy((int)enemyType4, null, null, null, (GameTypes.EnemyDifficulty)difficulty4, false);
                        else if (enemyOrbObj.OrbType == 4)
                            enemyObj = EnemyBuilder.BuildEnemy((int)enemyType5, null, null, null, (GameTypes.EnemyDifficulty)difficulty5, false);
                        else
                            enemyObj = EnemyBuilder.BuildEnemy((int)b, null, null, null, GameTypes.EnemyDifficulty.EXPERT, false);
                        while (enemyOrbObj.ForceFlying && enemyObj.IsWeighted) {
                            if (enemyObj != null)
                                enemyObj.Dispose();
                            if (enemyOrbObj.OrbType == 0) {
                                num = CDGMath.RandomInt(0, array.Length - 1);
                                enemyType = array[num];
                                difficulty = array2[num];
                                enemyObj = EnemyBuilder.BuildEnemy((int)enemyType, null, null, null, (GameTypes.EnemyDifficulty)difficulty, false);
                            }
                            else if (enemyOrbObj.OrbType == 1) {
                                num = CDGMath.RandomInt(0, array.Length - 1);
                                enemyType2 = array[num];
                                difficulty2 = array2[num];
                                enemyObj = EnemyBuilder.BuildEnemy((int)enemyType2, null, null, null, (GameTypes.EnemyDifficulty)difficulty2, false);
                            }
                            else if (enemyOrbObj.OrbType == 2) {
                                num = CDGMath.RandomInt(0, array.Length - 1);
                                enemyType3 = array[num];
                                difficulty3 = array2[num];
                                enemyObj = EnemyBuilder.BuildEnemy((int)enemyType3, null, null, null, (GameTypes.EnemyDifficulty)difficulty3, false);
                            }
                            else if (enemyOrbObj.OrbType == 3) {
                                num = CDGMath.RandomInt(0, array.Length - 1);
                                enemyType4 = array[num];
                                difficulty4 = array2[num];
                                enemyObj = EnemyBuilder.BuildEnemy((int)enemyType4, null, null, null, (GameTypes.EnemyDifficulty)difficulty4, false);
                            }
                            else if (enemyOrbObj.OrbType == 4) {
                                num = CDGMath.RandomInt(0, array.Length - 1);
                                enemyType5 = array[num];
                                difficulty5 = array2[num];
                                enemyObj = EnemyBuilder.BuildEnemy((int)enemyType5, null, null, null, (GameTypes.EnemyDifficulty)difficulty5, false);
                            }
                            else {
                                num = CDGMath.RandomInt(0, array.Length - 1);
                                b = array[num];
                                enemyObj = EnemyBuilder.BuildEnemy((int)b, null, null, null, GameTypes.EnemyDifficulty.EXPERT, false);
                            }
                        }
                        enemyObj.Position = enemyOrbObj.Position;
                        enemyObj.IsProcedural = true;
                        current.EnemyList.Add(enemyObj);
                        current.GameObjList.Remove(enemyOrbObj);
                        enemyOrbObj.Dispose();
                        i--;
                    }
                    else {
                        EnemyTagObj enemyTagObj = current.GameObjList[i] as EnemyTagObj;
                        if (enemyTagObj != null) {
                            int num3 = CDGMath.RandomInt(0, array.Length - 1);
                            EnemyObj enemyObj2 = EnemyBuilder.BuildEnemy((int)array[num3], null, null, null, GameTypes.EnemyDifficulty.BASIC, false);
                            enemyObj2.Position = enemyTagObj.Position;
                            enemyObj2.IsProcedural = true;
                            current.EnemyList.Add(enemyObj2);
                            current.GameObjList.Remove(enemyTagObj);
                            enemyTagObj.Dispose();
                            i--;
                        }
                    }
                }
            }
        }

        public static void OverrideProceduralEnemies(ProceduralLevelScreen level, byte[] enemyTypeData, byte[] enemyDifficultyData) {
            Console.WriteLine("////////////////// OVERRIDING CREATED ENEMIES. LOADING PRE-CONSTRUCTED ENEMY LIST ////////");
            int num = 0;
            foreach (RoomObj current in level.RoomList) {
                for (int i = 0; i < current.EnemyList.Count; i++) {
                    EnemyObj enemyObj = current.EnemyList[i];
                    if (enemyObj.IsProcedural) {
                        EnemyObj enemyObj2 = EnemyBuilder.BuildEnemy((int)enemyTypeData[num], level.Player, null, level, GameTypes.EnemyDifficulty.BASIC, true);
                        enemyObj2.IsProcedural = true;
                        enemyObj2.Position = enemyObj.Position;
                        enemyObj2.Level = enemyObj.Level;
                        enemyObj2.SetDifficulty((GameTypes.EnemyDifficulty)enemyDifficultyData[num], false);
                        current.EnemyList[i].Dispose();
                        current.EnemyList[i] = enemyObj2;
                        num++;
                    }
                }
            }
            Console.WriteLine("//////////////// PRE-CONSTRUCTED ENEMY LIST LOADED ////////////////");
        }

        public static void AddBottomPlatforms(List<RoomObj> roomList) {
            foreach (RoomObj current in roomList) {
                foreach (DoorObj current2 in current.DoorList) {
                    if (current2.DoorPosition == "Bottom") {
                        TerrainObj terrainObj = new TerrainObj(current2.Width, current2.Height);
                        terrainObj.AddCollisionBox(0, 0, terrainObj.Width, terrainObj.Height, 0);
                        terrainObj.AddCollisionBox(0, 0, terrainObj.Width, terrainObj.Height, 2);
                        terrainObj.Position = current2.Position;
                        terrainObj.CollidesBottom = false;
                        terrainObj.CollidesLeft = false;
                        terrainObj.CollidesRight = false;
                        terrainObj.SetHeight(30);
                        current.TerrainObjList.Add(terrainObj);
                        BorderObj borderObj = new BorderObj();
                        borderObj.Position = terrainObj.Position;
                        borderObj.SetWidth(terrainObj.Width);
                        borderObj.SetHeight(terrainObj.Height);
                        borderObj.BorderTop = true;
                        current.BorderList.Add(borderObj);
                    }
                }
            }
        }

        public static void AddCompassRoom(List<RoomObj> roomList) {
            CompassRoomObj compassRoomObj = LevelBuilder2.m_compassRoom.Clone() as CompassRoomObj;
            LevelBuilder2.MoveRoom(compassRoomObj, new Vector2(-999999f, -999999f));
            roomList.Add(compassRoomObj);
        }

        public static ProceduralLevelScreen CreateEndingRoom() {
            ProceduralLevelScreen proceduralLevelScreen = new ProceduralLevelScreen();
            RoomObj room = LevelBuilder2.m_endingRoom.Clone() as RoomObj;
            LevelBuilder2.MoveRoom(room, Vector2.Zero);
            proceduralLevelScreen.AddRoom(room);
            LevelBuilder2.AddDoorBorders(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddBottomPlatforms(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddRemoveExtraObjects(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddProceduralEnemies(proceduralLevelScreen.RoomList);
            LevelBuilder2.LinkAllBossEntrances(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertBonusRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertBossRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertChallengeBossRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.InitializeRooms(proceduralLevelScreen.RoomList);
            return proceduralLevelScreen;
        }

        public static ProceduralLevelScreen CreateStartingRoom() {
            ProceduralLevelScreen proceduralLevelScreen = new ProceduralLevelScreen();
            RoomObj room = LevelBuilder2.m_startingRoom.Clone() as RoomObj;
            LevelBuilder2.MoveRoom(room, Vector2.Zero);
            proceduralLevelScreen.AddRoom(room);
            LevelBuilder2.AddDoorBorders(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddBottomPlatforms(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddRemoveExtraObjects(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddProceduralEnemies(proceduralLevelScreen.RoomList);
            LevelBuilder2.LinkAllBossEntrances(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertBonusRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertBossRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertChallengeBossRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.InitializeRooms(proceduralLevelScreen.RoomList);
            return proceduralLevelScreen;
        }

        public static ProceduralLevelScreen CreateTutorialRoom() {
            ProceduralLevelScreen proceduralLevelScreen = new ProceduralLevelScreen();
            IntroRoomObj introRoomObj = new IntroRoomObj();
            introRoomObj.CopyRoomProperties(LevelBuilder2.m_startingRoom);
            introRoomObj.CopyRoomObjects(LevelBuilder2.m_startingRoom);
            LevelBuilder2.MoveRoom(introRoomObj, Vector2.Zero);
            proceduralLevelScreen.AddRoom(introRoomObj);
            Game.ScreenManager.Player.Position = new Vector2(150f, 150f);
            TutorialRoomObj tutorialRoomObj = LevelBuilder2.m_tutorialRoom.Clone() as TutorialRoomObj;
            LevelBuilder2.MoveRoom(tutorialRoomObj, new Vector2((float)introRoomObj.Width, (-(float)tutorialRoomObj.Height + introRoomObj.Height)));
            proceduralLevelScreen.AddRoom(tutorialRoomObj);
            ThroneRoomObj throneRoomObj = LevelBuilder2.m_throneRoom.Clone() as ThroneRoomObj;
            LevelBuilder2.MoveRoom(throneRoomObj, new Vector2(-10000f, -10000f));
            proceduralLevelScreen.AddRoom(throneRoomObj);
            tutorialRoomObj.LinkedRoom = throneRoomObj;
            LevelBuilder2.AddDoorBorders(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddBottomPlatforms(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddRemoveExtraObjects(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddProceduralEnemies(proceduralLevelScreen.RoomList);
            LevelBuilder2.LinkAllBossEntrances(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertBonusRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertBossRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertChallengeBossRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.InitializeRooms(proceduralLevelScreen.RoomList);
            return proceduralLevelScreen;
        }

        public static ProceduralLevelScreen CreateLevel(Vector4[] roomInfoList, Vector3[] roomColorList) {
            Console.WriteLine("///////////// LOADING PRE-CONSTRUCTED LEVEL //////");
            List<RoomObj> sequencedRoomList = LevelBuilder2.SequencedRoomList;
            List<RoomObj> sequencedDLCRoomList = LevelBuilder2.GetSequencedDLCRoomList(GameTypes.LevelType.CASTLE);
            List<RoomObj> sequencedDLCRoomList2 = LevelBuilder2.GetSequencedDLCRoomList(GameTypes.LevelType.GARDEN);
            List<RoomObj> sequencedDLCRoomList3 = LevelBuilder2.GetSequencedDLCRoomList(GameTypes.LevelType.TOWER);
            List<RoomObj> sequencedDLCRoomList4 = LevelBuilder2.GetSequencedDLCRoomList(GameTypes.LevelType.DUNGEON);
            ProceduralLevelScreen proceduralLevelScreen = new ProceduralLevelScreen();
            List<RoomObj> list = new List<RoomObj>();
            int num = 0;
            for (int i = 0; i < roomInfoList.Length; i++) {
                Vector4 vector = roomInfoList[i];
                int num2 = (int)vector.W;
                RoomObj roomObj;
                if (num2 < 10000)
                    roomObj = (sequencedRoomList[num2].Clone() as RoomObj);
                else if (num2 >= 10000 && num2 < 20000)
                    roomObj = (sequencedDLCRoomList[num2 - 10000].Clone() as RoomObj);
                else if (num2 >= 20000 && num2 < 30000)
                    roomObj = (sequencedDLCRoomList2[num2 - 20000].Clone() as RoomObj);
                else if (num2 >= 30000 && num2 < 40000)
                    roomObj = (sequencedDLCRoomList3[num2 - 30000].Clone() as RoomObj);
                else
                    roomObj = (sequencedDLCRoomList4[num2 - 40000].Clone() as RoomObj);
                roomObj.LevelType = (GameTypes.LevelType)vector.X;
                LevelBuilder2.MoveRoom(roomObj, new Vector2(vector.Y, vector.Z));
                list.Add(roomObj);
                roomObj.TextureColor = new Color(((byte)roomColorList[num].X), ((byte)roomColorList[num].Y), ((byte)roomColorList[num].Z));
                num++;
            }
            proceduralLevelScreen.AddRooms(list);
            LevelBuilder2.CloseRemainingDoors(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddDoorBorders(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddBottomPlatforms(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddRemoveExtraObjects(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddProceduralEnemies(proceduralLevelScreen.RoomList);
            LevelBuilder2.LinkAllBossEntrances(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertBonusRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertBossRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.ConvertChallengeBossRooms(proceduralLevelScreen.RoomList);
            LevelBuilder2.AddCompassRoom(proceduralLevelScreen.RoomList);
            LevelBuilder2.InitializeRooms(proceduralLevelScreen.RoomList);
            Console.WriteLine("///////////// PRE-CONSTRUCTED LEVEL LOADED //////");
            return proceduralLevelScreen;
        }

        public static ProceduralLevelScreen CreateLevel(RoomObj startingRoom = null, params AreaStruct[] areaStructs) {
            if (LevelBuilder2.m_testRoom != null && LevelEV.RUN_TESTROOM) {
                Console.WriteLine("OVERRIDING ROOM CREATION. RUNNING TEST ROOM");
                ProceduralLevelScreen proceduralLevelScreen = new ProceduralLevelScreen();
                RoomObj roomObj = LevelBuilder2.m_testRoom.Clone() as RoomObj;
                if (LevelEV.TESTROOM_REVERSE)
                    roomObj.Reverse();
                LevelBuilder2.MoveRoom(roomObj, Vector2.Zero);
                proceduralLevelScreen.AddRoom(roomObj);
                if (LevelEV.CLOSE_TESTROOM_DOORS)
                    LevelBuilder2.CloseRemainingDoors(proceduralLevelScreen.RoomList);
                LevelBuilder2.AddDoorBorders(proceduralLevelScreen.RoomList);
                LevelBuilder2.AddBottomPlatforms(proceduralLevelScreen.RoomList);
                LevelBuilder2.AddRemoveExtraObjects(proceduralLevelScreen.RoomList);
                LevelBuilder2.AddProceduralEnemies(proceduralLevelScreen.RoomList);
                LevelBuilder2.LinkAllBossEntrances(proceduralLevelScreen.RoomList);
                LevelBuilder2.ConvertBonusRooms(proceduralLevelScreen.RoomList);
                LevelBuilder2.ConvertBossRooms(proceduralLevelScreen.RoomList);
                LevelBuilder2.ConvertChallengeBossRooms(proceduralLevelScreen.RoomList);
                LevelBuilder2.InitializeRooms(proceduralLevelScreen.RoomList);
                proceduralLevelScreen.RoomList[0].LevelType = LevelEV.TESTROOM_LEVELTYPE;
                return proceduralLevelScreen;
            }
            ProceduralLevelScreen proceduralLevelScreen2 = new ProceduralLevelScreen();
            List<RoomObj> list = new List<RoomObj>();
            List<AreaStruct> list2 = new List<AreaStruct>();
            List<AreaStruct> list3 = new List<AreaStruct>();
            for (int i = 0; i < areaStructs.Length; i++) {
                AreaStruct item = areaStructs[i];
                if (item.LevelType == GameTypes.LevelType.CASTLE || item.LevelType == GameTypes.LevelType.GARDEN)
                    list2.Add(item);
                else
                    list3.Add(item);
            }
            int count = list2.Count;
            int count2 = list3.Count;
            List<RoomObj>[] array = new List<RoomObj>[count];
            List<RoomObj>[] array2 = new List<RoomObj>[count2];
            AreaStruct areaInfo2;
            while (true) {
                IL_16C:
                list.Clear();
                int j = 0;
                while (j < list2.Count) {
                    int num = 0;
                    while (true) {
                        array[j] = null;
                        AreaStruct areaInfo = list2[j];
                        int num2 = CDGMath.RandomInt((int)areaInfo.TotalRooms.X, (int)areaInfo.TotalRooms.Y);
                        DoorObj doorObj = null;
                        bool flag = true;
                        while (array[j] == null || array[j].Count < num2 || !flag) {
                            flag = true;
                            if (areaInfo.BossInArea)
                                flag = false;
                            if (j == 0) {
                                if (startingRoom == null)
                                    array[j] = LevelBuilder2.CreateArea(num2, areaInfo, list, LevelBuilder2.StartingRoom.Clone() as StartingRoomObj, true);
                                else
                                    array[j] = LevelBuilder2.CreateArea(num2, areaInfo, list, startingRoom.Clone() as StartingRoomObj, true);
                            }
                            else {
                                List<RoomObj> list4 = new List<RoomObj>();
                                list4.AddRange(list);
                                doorObj = LevelBuilder2.FindFurthestDoor(list4, "Right", "Right", true, true);
                                if (doorObj == null)
                                    goto IL_16C;
                                array[j] = LevelBuilder2.CreateArea(num2, areaInfo, list4, doorObj.Room, false);
                            }
                            foreach (RoomObj current in array[j]) {
                                if (current.Name == "EntranceBoss") {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                                Console.WriteLine("Could not find suitable boss room for area. Recreating sequential area.");
                            else
                                Console.WriteLine("Created sequential area of size: " + array[j].Count);
                            num++;
                            if (num > 15)
                                goto Block_13;
                        }
                        if (j != 0)
                            list.Add(doorObj.Room);
                        if (LevelBuilder2.FindFurthestDoor(array[j], "Right", "Right", false, false) != null && LevelBuilder2.FindFurthestDoor(array[j], "Top", "Top", false, false) != null && LevelBuilder2.FindFurthestDoor(array[j], "Bottom", "Bottom", false, false) != null)
                            break;
                        bool flag2 = j == 0 || list.Remove(doorObj.Room);
                        Console.WriteLine("Attempting re-creation of sequential area. Linker Room removed: " + flag2);
                    }
                    list.AddRange(array[j]);
                    j++;
                    continue;
                    Block_13:
                    Console.WriteLine("Could not create non-sequential area after 15 attempts. Recreating entire level.");
                    goto IL_16C;
                }
                Console.WriteLine("////////// ALL SEQUENTIAL AREAS SUCCESSFULLY ADDED");
                int k = 0;
                while (k < list3.Count) {
                    int num3 = 0;
                    while (true) {
                        array2[k] = null;
                        areaInfo2 = list3[k];
                        int num4 = CDGMath.RandomInt((int)areaInfo2.TotalRooms.X, (int)areaInfo2.TotalRooms.Y);
                        string text = "";
                        switch (areaInfo2.LevelType) {
                            case GameTypes.LevelType.GARDEN:
                                text = "Right";
                                goto IL_457;
                            case GameTypes.LevelType.DUNGEON:
                                text = "Bottom";
                                goto IL_457;
                            case GameTypes.LevelType.TOWER:
                                text = "Top";
                                goto IL_457;
                        }
                        goto Block_22;
                        IL_457:
                        DoorObj doorObj2 = null;
                        bool flag3 = true;
                        while (array2[k] == null || array2[k].Count < num4 || !flag3) {
                            flag3 = true;
                            if (areaInfo2.BossInArea)
                                flag3 = false;
                            List<RoomObj> list5 = new List<RoomObj>();
                            list5.AddRange(list);
                            doorObj2 = LevelBuilder2.FindFurthestDoor(list5, text, text, true, true);
                            if (doorObj2 == null)
                                goto IL_16C;
                            array2[k] = LevelBuilder2.CreateArea(num4, areaInfo2, list5, doorObj2.Room, false);
                            num3++;
                            if (num3 > 15)
                                goto Block_25;
                            foreach (RoomObj current2 in array2[k]) {
                                if (current2.Name == "EntranceBoss") {
                                    flag3 = true;
                                    break;
                                }
                            }
                            if (!flag3)
                                Console.WriteLine("Could not find suitable boss room for area. Recreating non-sequential area.");
                            else
                                Console.WriteLine("Created non-sequential area of size: " + array2[k].Count);
                        }
                        list.Add(doorObj2.Room);
                        if ((areaInfo2.LevelType != GameTypes.LevelType.TOWER || (LevelBuilder2.FindFurthestDoor(array2[k], "Right", "Right", false, false) != null && LevelBuilder2.FindFurthestDoor(array2[k], "Top", "Top", false, false) != null)) && (areaInfo2.LevelType != GameTypes.LevelType.DUNGEON || (LevelBuilder2.FindFurthestDoor(array2[k], "Right", "Right", false, false) != null && LevelBuilder2.FindFurthestDoor(array2[k], "Bottom", "Bottom", false, false) != null)))
                            break;
                        bool flag4 = list.Remove(doorObj2.Room);
                        Console.WriteLine("Attempting re-creation of a non-sequential area. Linker Room removed: " + flag4);
                    }
                    list.AddRange(array2[k]);
                    k++;
                    continue;
                    Block_25:
                    Console.WriteLine("Could not create non-sequential area after 15 attempts. Recreating entire level.");
                    goto IL_16C;
                }
                goto Block_35;
            }
            Block_22:
            throw new Exception("Could not create non-sequential area of type " + areaInfo2.LevelType);
            Block_35:
            Console.WriteLine("////////// ALL NON-SEQUENTIAL AREAS SUCCESSFULLY ADDED");
            proceduralLevelScreen2.AddRooms(list);
            LevelBuilder2.CloseRemainingDoors(proceduralLevelScreen2.RoomList);
            LevelBuilder2.AddDoorBorders(proceduralLevelScreen2.RoomList);
            LevelBuilder2.AddBottomPlatforms(proceduralLevelScreen2.RoomList);
            LevelBuilder2.AddRemoveExtraObjects(proceduralLevelScreen2.RoomList);
            LevelBuilder2.AddProceduralEnemies(proceduralLevelScreen2.RoomList);
            LevelBuilder2.LinkAllBossEntrances(proceduralLevelScreen2.RoomList);
            LevelBuilder2.ConvertBonusRooms(proceduralLevelScreen2.RoomList);
            LevelBuilder2.ConvertBossRooms(proceduralLevelScreen2.RoomList);
            LevelBuilder2.ConvertChallengeBossRooms(proceduralLevelScreen2.RoomList);
            LevelBuilder2.AddCompassRoom(proceduralLevelScreen2.RoomList);
            LevelBuilder2.InitializeRooms(proceduralLevelScreen2.RoomList);
            Console.WriteLine("////////// LEVEL CREATION SUCCESSFUL");
            return proceduralLevelScreen2;
        }

        private static void ConvertBonusRooms(List<RoomObj> roomList) {
            CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            for (int i = 0; i < roomList.Count; i++) {
                RoomObj roomObj = roomList[i];
                if (roomObj.Name == "Bonus") {
                    if (roomObj.Tag == "")
                        roomObj.Tag = "0";
                    RoomObj roomObj2 = null;
                    switch (int.Parse(roomObj.Tag, NumberStyles.Any, cultureInfo)) {
                        case 1:
                            roomObj2 = new ChestBonusRoomObj();
                            break;
                        case 2:
                            roomObj2 = new SpecialItemRoomObj();
                            break;
                        case 3:
                            roomObj2 = new RandomTeleportRoomObj();
                            break;
                        case 4:
                            roomObj2 = new SpellSwapRoomObj();
                            break;
                        case 5:
                            roomObj2 = new VitaChamberRoomObj();
                            break;
                        case 6:
                            roomObj2 = new DiaryRoomObj();
                            break;
                        case 7:
                            roomObj2 = new PortraitRoomObj();
                            break;
                        case 8:
                            roomObj2 = new CarnivalShoot1BonusRoom();
                            break;
                        case 9:
                            roomObj2 = new CarnivalShoot2BonusRoom();
                            break;
                        case 10:
                            roomObj2 = new ArenaBonusRoom();
                            break;
                        case 11:
                            roomObj2 = new JukeboxBonusRoom();
                            break;
                    }
                    if (roomObj2 != null) {
                        roomObj2.CopyRoomProperties(roomObj);
                        roomObj2.CopyRoomObjects(roomObj);
                        roomList.Insert(roomList.IndexOf(roomObj), roomObj2);
                        roomList.Remove(roomObj);
                    }
                }
            }
        }

        private static void ConvertBossRooms(List<RoomObj> roomList) {
            CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            for (int i = 0; i < roomList.Count; i++) {
                RoomObj roomObj = roomList[i];
                if (roomObj.Name == "Boss") {
                    if (roomObj.Tag == "")
                        roomObj.Tag = "0";
                    RoomObj roomObj2 = null;
                    switch (int.Parse(roomObj.Tag, NumberStyles.Any, cultureInfo)) {
                        case 1:
                            roomObj2 = new EyeballBossRoom();
                            break;
                        case 2:
                            roomObj2 = new LastBossRoom();
                            break;
                        case 5:
                            roomObj2 = new FairyBossRoom();
                            break;
                        case 6:
                            roomObj2 = new FireballBossRoom();
                            break;
                        case 7:
                            roomObj2 = new BlobBossRoom();
                            break;
                    }
                    if (roomObj2 != null) {
                        roomObj2.CopyRoomProperties(roomObj);
                        roomObj2.CopyRoomObjects(roomObj);
                        if (roomObj2.LinkedRoom != null) {
                            roomObj2.LinkedRoom = roomObj.LinkedRoom;
                            roomObj2.LinkedRoom.LinkedRoom = roomObj2;
                        }
                        roomList.Insert(roomList.IndexOf(roomObj), roomObj2);
                        roomList.Remove(roomObj);
                    }
                }
            }
        }

        private static void ConvertChallengeBossRooms(List<RoomObj> roomList) {
            CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            for (int i = 0; i < roomList.Count; i++) {
                RoomObj roomObj = roomList[i];
                if (roomObj.Name == "ChallengeBoss") {
                    if (roomObj.Tag == "")
                        roomObj.Tag = "0";
                    RoomObj roomObj2;
                    switch (int.Parse(roomObj.Tag, NumberStyles.Any, cultureInfo)) {
                        case 1:
                            roomObj2 = new EyeballChallengeRoom();
                            LevelBuilder2.MoveRoom(roomObj2, new Vector2(0f, 5000000f));
                            break;
                        case 2:
                            roomObj2 = new LastBossChallengeRoom();
                            LevelBuilder2.MoveRoom(roomObj2, new Vector2(0f, -9000000f));
                            break;
                        case 3:
                        case 4:
                            goto IL_138;
                        case 5:
                            roomObj2 = new FairyChallengeRoom();
                            LevelBuilder2.MoveRoom(roomObj2, new Vector2(0f, -6000000f));
                            break;
                        case 6:
                            roomObj2 = new FireballChallengeRoom();
                            LevelBuilder2.MoveRoom(roomObj2, new Vector2(0f, -7000000f));
                            break;
                        case 7:
                            roomObj2 = new BlobChallengeRoom();
                            LevelBuilder2.MoveRoom(roomObj2, new Vector2(0f, -8000000f));
                            break;
                        default:
                            goto IL_138;
                    }
                    IL_153:
                    if (roomObj2 != null) {
                        Vector2 position = roomObj2.Position;
                        roomObj2.CopyRoomProperties(roomObj);
                        roomObj2.CopyRoomObjects(roomObj);
                        LevelBuilder2.MoveRoom(roomObj2, position);
                        if (roomObj2.LinkedRoom != null)
                            roomObj2.LinkedRoom = roomObj.LinkedRoom;
                        roomList.Insert(roomList.IndexOf(roomObj), roomObj2);
                        roomList.Remove(roomObj);
                        goto IL_19E;
                    }
                    goto IL_19E;
                    IL_138:
                    roomObj2 = new EyeballChallengeRoom();
                    LevelBuilder2.MoveRoom(roomObj2, new Vector2(0f, -5000000f));
                    goto IL_153;
                }
                IL_19E:
                ;
            }
        }

        private static void InitializeRooms(List<RoomObj> roomList) {
            foreach (RoomObj current in roomList)
                current.Initialize();
        }

        private static string GetOppositeDoorPosition(string doorPosition) {
            string result = "";
            if (doorPosition != null) {
                if (!(doorPosition == "Left")) {
                    if (!(doorPosition == "Right")) {
                        if (!(doorPosition == "Top")) {
                            if (doorPosition == "Bottom")
                                result = "Top";
                        }
                        else
                            result = "Bottom";
                    }
                    else
                        result = "Left";
                }
                else
                    result = "Right";
            }
            return result;
        }

        private static bool CheckForRoomCollision(DoorObj doorToCheck, List<RoomObj> roomList, DoorObj otherDoorToCheck) {
            Vector2 zero = Vector2.Zero;
            string doorPosition;
            if ((doorPosition = doorToCheck.DoorPosition) != null) {
                if (!(doorPosition == "Left")) {
                    if (!(doorPosition == "Right")) {
                        if (!(doorPosition == "Top")) {
                            if (doorPosition == "Bottom")
                                zero = new Vector2(doorToCheck.X - (otherDoorToCheck.X - otherDoorToCheck.Room.X), doorToCheck.Y + (float)doorToCheck.Height);
                        }
                        else
                            zero = new Vector2(doorToCheck.X - (otherDoorToCheck.X - otherDoorToCheck.Room.X), doorToCheck.Y - (float)otherDoorToCheck.Room.Height);
                    }
                    else
                        zero = new Vector2(doorToCheck.X + (float)doorToCheck.Width, doorToCheck.Y - (otherDoorToCheck.Y - otherDoorToCheck.Room.Y));
                }
                else
                    zero = new Vector2(doorToCheck.Room.X - (float)otherDoorToCheck.Room.Width, doorToCheck.Y - (otherDoorToCheck.Y - otherDoorToCheck.Room.Y));
            }
            foreach (RoomObj current in roomList) {
                if (CollisionMath.Intersects(new Rectangle((int)current.X, (int)current.Y, current.Width, current.Height), new Rectangle((int)zero.X, (int)zero.Y, otherDoorToCheck.Room.Width, otherDoorToCheck.Room.Height)) || zero.X < 0f)
                    return true;
            }
            return false;
        }

        public static void MoveRoom(RoomObj room, Vector2 newPosition) {
            Vector2 value = room.Position - newPosition;
            room.Position = newPosition;
            foreach (TerrainObj current in room.TerrainObjList)
                current.Position -= value;
            foreach (GameObj current2 in room.GameObjList)
                current2.Position -= value;
            foreach (DoorObj current3 in room.DoorList)
                current3.Position -= value;
            foreach (EnemyObj current4 in room.EnemyList)
                current4.Position -= value;
            foreach (BorderObj current5 in room.BorderList)
                current5.Position -= value;
        }

        public static void LinkAllBossEntrances(List<RoomObj> roomList) {
            Vector2 newPosition = new Vector2(-100000f, 0f);
            int max = LevelBuilder2.m_bossRoomArray.Count - 1;
            List<RoomObj> list = new List<RoomObj>();
            List<RoomObj> list2 = new List<RoomObj>();
            RoomObj roomObj2;
            foreach (RoomObj current in roomList) {
                byte bossRoomType = 0;
                switch (current.LevelType) {
                    case GameTypes.LevelType.CASTLE:
                        bossRoomType = 1;
                        break;
                    case GameTypes.LevelType.GARDEN:
                        bossRoomType = 5;
                        break;
                    case GameTypes.LevelType.DUNGEON:
                        bossRoomType = 7;
                        break;
                    case GameTypes.LevelType.TOWER:
                        bossRoomType = 6;
                        break;
                }
                if (current.Name == "EntranceBoss") {
                    RoomObj roomObj = LevelBuilder2.GetSpecificBossRoom(bossRoomType);
                    if (roomObj != null)
                        roomObj = (roomObj.Clone() as RoomObj);
                    if (roomObj == null)
                        roomObj = (LevelBuilder2.GetBossRoom(CDGMath.RandomInt(0, max)).Clone() as RoomObj);
                    roomObj.LevelType = current.LevelType;
                    LevelBuilder2.MoveRoom(roomObj, newPosition);
                    newPosition.X += (float)roomObj.Width;
                    current.LinkedRoom = roomObj;
                    roomObj.LinkedRoom = current;
                    if (roomObj == null)
                        throw new Exception("Could not find a boss room for the boss entrance. This should NOT be possible. LinkAllBossEntrances()");
                    list.Add(roomObj);
                    roomObj2 = LevelBuilder2.GetChallengeRoom(bossRoomType);
                    if (roomObj2 != null) {
                        roomObj2 = (roomObj2.Clone() as RoomObj);
                        roomObj2.LevelType = current.LevelType;
                        roomObj2.LinkedRoom = current;
                        list2.Add(roomObj2);
                    }
                }
                else if (current.Name == "CastleEntrance") {
                    TutorialRoomObj tutorialRoomObj = LevelBuilder2.m_tutorialRoom.Clone() as TutorialRoomObj;
                    RoomObj roomObj = LevelBuilder2.GetSpecificBossRoom(2).Clone() as RoomObj;
                    LevelBuilder2.MoveRoom(tutorialRoomObj, new Vector2(100000f, -100000f));
                    LevelBuilder2.MoveRoom(roomObj, new Vector2(150000f, -100000f));
                    current.LinkedRoom = tutorialRoomObj;
                    tutorialRoomObj.LinkedRoom = roomObj;
                    roomObj.LinkedRoom = tutorialRoomObj;
                    if (roomObj == null)
                        throw new Exception("Could not find a boss room for the boss entrance. This should NOT be possible. LinkAllBossEntrances()");
                    list.Add(roomObj);
                    list.Add(tutorialRoomObj);
                }
            }
            roomObj2 = LevelBuilder2.GetChallengeRoom(2);
            if (roomObj2 != null) {
                roomObj2 = (roomObj2.Clone() as RoomObj);
                roomObj2.LevelType = GameTypes.LevelType.CASTLE;
                roomObj2.LinkedRoom = null;
                list2.Add(roomObj2);
            }
            roomList.AddRange(list);
            roomList.AddRange(list2);
        }

        public static List<RoomObj> GetRoomList(int roomWidth, int roomHeight, GameTypes.LevelType levelType) {
            return LevelBuilder2.GetLevelTypeRoomArray(levelType)[roomWidth - 1, roomHeight - 1];
        }

        public static RoomObj GetBossRoom(int index) {
            return LevelBuilder2.m_bossRoomArray[index];
        }

        public static RoomObj GetSpecificBossRoom(byte bossRoomType) {
            foreach (RoomObj current in LevelBuilder2.m_bossRoomArray) {
                if (current.Tag != "" && byte.Parse(current.Tag) == bossRoomType)
                    return current;
            }
            return null;
        }

        public static RoomObj GetChallengeRoom(byte bossRoomType) {
            foreach (RoomObj current in LevelBuilder2.m_challengeRoomArray) {
                if (current.Tag != "" && byte.Parse(current.Tag) == bossRoomType)
                    return current;
            }
            return null;
        }

        public static RoomObj GetChallengeBossRoomFromRoomList(GameTypes.LevelType levelType, List<RoomObj> roomList) {
            foreach (RoomObj current in roomList) {
                if (current.Name == "ChallengeBoss" && current.LevelType == levelType)
                    return current;
            }
            return null;
        }

        public static List<RoomObj>[,] GetLevelTypeRoomArray(GameTypes.LevelType levelType) {
            switch (levelType) {
                case GameTypes.LevelType.CASTLE:
                    return LevelBuilder2.m_castleRoomArray;
                case GameTypes.LevelType.GARDEN:
                    return LevelBuilder2.m_gardenRoomArray;
                case GameTypes.LevelType.DUNGEON:
                    return LevelBuilder2.m_dungeonRoomArray;
                case GameTypes.LevelType.TOWER:
                    return LevelBuilder2.m_towerRoomArray;
                default:
                    throw new Exception("Cannot create level of type NONE");
            }
        }

        public static void IndexRoomList() {
            int num = 0;
            foreach (RoomObj current in LevelBuilder2.SequencedRoomList) {
                current.PoolIndex = num;
                num++;
            }
            num = 10000;
            List<RoomObj> sequencedDLCRoomList = LevelBuilder2.GetSequencedDLCRoomList(GameTypes.LevelType.CASTLE);
            foreach (RoomObj current2 in sequencedDLCRoomList) {
                current2.PoolIndex = num;
                num++;
            }
            num = 20000;
            sequencedDLCRoomList = LevelBuilder2.GetSequencedDLCRoomList(GameTypes.LevelType.GARDEN);
            foreach (RoomObj current3 in sequencedDLCRoomList) {
                current3.PoolIndex = num;
                num++;
            }
            num = 30000;
            sequencedDLCRoomList = LevelBuilder2.GetSequencedDLCRoomList(GameTypes.LevelType.TOWER);
            foreach (RoomObj current4 in sequencedDLCRoomList) {
                current4.PoolIndex = num;
                num++;
            }
            num = 40000;
            sequencedDLCRoomList = LevelBuilder2.GetSequencedDLCRoomList(GameTypes.LevelType.DUNGEON);
            foreach (RoomObj current5 in sequencedDLCRoomList) {
                current5.PoolIndex = num;
                num++;
            }
        }

        public static List<RoomObj> GetSequencedDLCRoomList(GameTypes.LevelType levelType) {
            switch (levelType) {
                case GameTypes.LevelType.CASTLE:
                    return LevelBuilder2.m_dlcCastleRoomArray;
                case GameTypes.LevelType.GARDEN:
                    return LevelBuilder2.m_dlcGardenRoomArray;
                case GameTypes.LevelType.DUNGEON:
                    return LevelBuilder2.m_dlcDungeonRoomArray;
                case GameTypes.LevelType.TOWER:
                    return LevelBuilder2.m_dlcTowerRoomArray;
                default:
                    return null;
            }
        }
    }
}
