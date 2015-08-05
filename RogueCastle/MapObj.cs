using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public class MapObj : GameObj {
        public Vector2 CameraOffset;
        private List<RoomObj> m_addedRooms;
        private RenderTarget2D m_alphaMaskRT;
        private Rectangle m_alphaMaskRect;
        private List<SpriteObj> m_doorSpriteList;
        private List<Vector2> m_doorSpritePosList;
        private List<SpriteObj> m_iconSpriteList;
        private List<Vector2> m_iconSpritePosList;
        private ProceduralLevelScreen m_level;
        private RenderTarget2D m_mapScreenRT;
        private PlayerObj m_player;
        private SpriteObj m_playerSprite;
        private List<SpriteObj> m_roomSpriteList;
        private List<Vector2> m_roomSpritePosList;
        private Vector2 m_spriteScale;
        private List<SpriteObj> m_teleporterList;
        private List<Vector2> m_teleporterPosList;
        private TweenObject m_xOffsetTween;
        private TweenObject m_yOffsetTween;

        public MapObj(bool followPlayer, ProceduralLevelScreen level) {
            m_level = level;
            FollowPlayer = followPlayer;
            base.Opacity = 0.3f;
            m_roomSpriteList = new List<SpriteObj>();
            m_doorSpriteList = new List<SpriteObj>();
            m_iconSpriteList = new List<SpriteObj>();
            m_roomSpritePosList = new List<Vector2>();
            m_doorSpritePosList = new List<Vector2>();
            m_iconSpritePosList = new List<Vector2>();
            CameraOffset = new Vector2(20f, 560f);
            m_playerSprite = new SpriteObj("MapPlayerIcon_Sprite");
            m_playerSprite.AnimationDelay = 0.0333333351f;
            m_playerSprite.ForceDraw = true;
            m_playerSprite.PlayAnimation(true);
            m_spriteScale = new Vector2(22f, 22.5f);
            m_addedRooms = new List<RoomObj>();
            m_teleporterList = new List<SpriteObj>();
            m_teleporterPosList = new List<Vector2>();
        }

        public bool DrawTeleportersOnly { get; set; }
        public bool DrawNothing { get; set; }
        public bool FollowPlayer { get; set; }

        public List<RoomObj> AddedRoomsList {
            get { return m_addedRooms; }
        }

        public float CameraOffsetX {
            get { return CameraOffset.X; }
            set { CameraOffset.X = value; }
        }

        public float CameraOffsetY {
            get { return CameraOffset.Y; }
            set { CameraOffset.Y = value; }
        }

        public void InitializeAlphaMap(Rectangle mapSize, Camera2D camera) {
            m_alphaMaskRect = mapSize;
            m_mapScreenRT = new RenderTarget2D(camera.GraphicsDevice, 1320, 720);
            m_alphaMaskRT = new RenderTarget2D(camera.GraphicsDevice, 1320, 720);
            CameraOffset = new Vector2(mapSize.X, mapSize.Y);
            SpriteObj spriteObj = new SpriteObj("MapMask_Sprite");
            spriteObj.ForceDraw = true;
            spriteObj.Position = new Vector2(mapSize.X, mapSize.Y);
            spriteObj.Scale = new Vector2(mapSize.Width / (float)spriteObj.Width, mapSize.Height / (float)spriteObj.Height);
            camera.GraphicsDevice.SetRenderTarget(m_alphaMaskRT);
            camera.GraphicsDevice.Clear(Color.White);
            camera.Begin();
            spriteObj.Draw(camera);
            camera.End();
            camera.GraphicsDevice.SetRenderTarget(Game.ScreenManager.RenderTarget);
        }

        public void InitializeAlphaMap(RenderTarget2D mapScreenRT, RenderTarget2D alphaMaskRT, Rectangle mapSize) {
            m_mapScreenRT = mapScreenRT;
            m_alphaMaskRT = alphaMaskRT;
            m_alphaMaskRect = mapSize;
            CameraOffset = new Vector2(mapSize.X, mapSize.Y);
        }

        public void DisposeRTs() {
            m_mapScreenRT.Dispose();
            m_mapScreenRT = null;
            m_alphaMaskRT.Dispose();
            m_alphaMaskRT = null;
        }

        public void SetPlayer(PlayerObj player) {
            m_player = player;
        }

        public void AddRoom(RoomObj room) {
            if (!m_addedRooms.Contains(room) && room.Width / 1320 < 5) {
                SpriteObj spriteObj = new SpriteObj(string.Concat(new object[] {
                    "MapRoom",
                    room.Width / 1320,
                    "x",
                    room.Height / 720,
                    "_Sprite"
                }));
                spriteObj.Position = new Vector2(room.X / m_spriteScale.X, room.Y / m_spriteScale.Y);
                spriteObj.Scale = new Vector2(((float)spriteObj.Width - 3f) / (float)spriteObj.Width, ((float)spriteObj.Height - 3f) / (float)spriteObj.Height);
                spriteObj.ForceDraw = true;
                spriteObj.TextureColor = room.TextureColor;
                m_roomSpriteList.Add(spriteObj);
                m_roomSpritePosList.Add(spriteObj.Position);
                foreach (DoorObj current in room.DoorList) {
                    if (!(room.Name == "CastleEntrance") || !(current.DoorPosition == "Left")) {
                        bool flag = false;
                        SpriteObj spriteObj2 = new SpriteObj("MapDoor_Sprite");
                        spriteObj2.ForceDraw = true;
                        string doorPosition;
                        if ((doorPosition = current.DoorPosition) != null) {
                            if (!(doorPosition == "Left")) {
                                if (!(doorPosition == "Right")) {
                                    if (!(doorPosition == "Bottom")) {
                                        if (doorPosition == "Top") {
                                            spriteObj2.Rotation = -90f;
                                            spriteObj2.Position = new Vector2(current.X / m_spriteScale.X, current.Y / m_spriteScale.Y + 2f);
                                            flag = true;
                                        }
                                    }
                                    else {
                                        spriteObj2.Rotation = -90f;
                                        spriteObj2.Position = new Vector2(current.X / m_spriteScale.X, (current.Y + (float)current.Height) / m_spriteScale.Y + 2f);
                                        flag = true;
                                    }
                                }
                                else {
                                    spriteObj2.Position = new Vector2((float)room.Bounds.Right / m_spriteScale.X - 5f, current.Y / m_spriteScale.Y - 2f);
                                    flag = true;
                                }
                            }
                            else {
                                spriteObj2.Position = new Vector2((float)room.Bounds.Left / m_spriteScale.X - (float)spriteObj2.Width + 2f, current.Y / m_spriteScale.Y - 2f);
                                flag = true;
                            }
                        }
                        if (flag) {
                            m_doorSpritePosList.Add(spriteObj2.Position);
                            m_doorSpriteList.Add(spriteObj2);
                        }
                    }
                }
                if (room.Name != "Bonus" && Game.PlayerStats.Class != 13) {
                    foreach (GameObj current2 in room.GameObjList) {
                        ChestObj chestObj = current2 as ChestObj;
                        if (chestObj != null) {
                            SpriteObj spriteObj3;
                            if (chestObj.IsOpen)
                                spriteObj3 = new SpriteObj("MapChestUnlocked_Sprite");
                            else if (chestObj is FairyChestObj) {
                                spriteObj3 = new SpriteObj("MapFairyChestIcon_Sprite");
                                if ((chestObj as FairyChestObj).ConditionType == 10)
                                    spriteObj3.Opacity = 0.2f;
                            }
                            else
                                spriteObj3 = new SpriteObj("MapLockedChestIcon_Sprite");
                            m_iconSpriteList.Add(spriteObj3);
                            spriteObj3.AnimationDelay = 0.0333333351f;
                            spriteObj3.PlayAnimation(true);
                            spriteObj3.ForceDraw = true;
                            spriteObj3.Position = new Vector2(current2.X / m_spriteScale.X - 8f, current2.Y / m_spriteScale.Y - 12f);
                            if (room.IsReversed)
                                spriteObj3.X -= (float)current2.Width / m_spriteScale.X;
                            m_iconSpritePosList.Add(spriteObj3.Position);
                        }
                    }
                }
                if (room.Name == "EntranceBoss") {
                    SpriteObj spriteObj4 = new SpriteObj("MapBossIcon_Sprite");
                    spriteObj4.AnimationDelay = 0.0333333351f;
                    spriteObj4.ForceDraw = true;
                    spriteObj4.PlayAnimation(true);
                    spriteObj4.Position = new Vector2((room.X + (float)room.Width / 2f) / m_spriteScale.X - (spriteObj4.Width / 2) - 1f, (room.Y + (float)room.Height / 2f) / m_spriteScale.Y - (spriteObj4.Height / 2) - 2f);
                    m_iconSpriteList.Add(spriteObj4);
                    m_iconSpritePosList.Add(spriteObj4.Position);
                    m_teleporterList.Add(spriteObj4);
                    m_teleporterPosList.Add(spriteObj4.Position);
                }
                else if (room.Name == "Linker") {
                    SpriteObj spriteObj5 = new SpriteObj("MapTeleporterIcon_Sprite");
                    spriteObj5.AnimationDelay = 0.0333333351f;
                    spriteObj5.ForceDraw = true;
                    spriteObj5.PlayAnimation(true);
                    spriteObj5.Position = new Vector2((room.X + (float)room.Width / 2f) / m_spriteScale.X - (spriteObj5.Width / 2) - 1f, (room.Y + (float)room.Height / 2f) / m_spriteScale.Y - (spriteObj5.Height / 2) - 2f);
                    m_iconSpriteList.Add(spriteObj5);
                    m_iconSpritePosList.Add(spriteObj5.Position);
                    m_teleporterList.Add(spriteObj5);
                    m_teleporterPosList.Add(spriteObj5.Position);
                }
                else if (room.Name == "CastleEntrance") {
                    SpriteObj spriteObj6 = new SpriteObj("MapTeleporterIcon_Sprite");
                    spriteObj6.AnimationDelay = 0.0333333351f;
                    spriteObj6.ForceDraw = true;
                    spriteObj6.PlayAnimation(true);
                    spriteObj6.Position = new Vector2((room.X + (float)room.Width / 2f) / m_spriteScale.X - (spriteObj6.Width / 2) - 1f, (room.Y + (float)room.Height / 2f) / m_spriteScale.Y - (spriteObj6.Height / 2) - 2f);
                    m_iconSpriteList.Add(spriteObj6);
                    m_iconSpritePosList.Add(spriteObj6.Position);
                    m_teleporterList.Add(spriteObj6);
                    m_teleporterPosList.Add(spriteObj6.Position);
                }
                if (Game.PlayerStats.Class != 13 && room.Name == "Bonus") {
                    SpriteObj spriteObj7 = new SpriteObj("MapBonusIcon_Sprite");
                    spriteObj7.PlayAnimation(true);
                    spriteObj7.AnimationDelay = 0.0333333351f;
                    spriteObj7.ForceDraw = true;
                    spriteObj7.Position = new Vector2((room.X + (float)room.Width / 2f) / m_spriteScale.X - (spriteObj7.Width / 2) - 1f, (room.Y + (float)room.Height / 2f) / m_spriteScale.Y - (spriteObj7.Height / 2) - 2f);
                    m_iconSpriteList.Add(spriteObj7);
                    m_iconSpritePosList.Add(spriteObj7.Position);
                }
                m_addedRooms.Add(room);
            }
        }

        public void AddAllRooms(List<RoomObj> roomList) {
            foreach (RoomObj current in roomList)
                AddRoom(current);
        }

        public void AddAllIcons(List<RoomObj> roomList) {
            foreach (RoomObj current in roomList) {
                if (!m_addedRooms.Contains(current)) {
                    if (current.Name != "Bonus") {
                        using (List<GameObj>.Enumerator enumerator2 = current.GameObjList.GetEnumerator()) {
                            while (enumerator2.MoveNext()) {
                                GameObj current2 = enumerator2.Current;
                                ChestObj chestObj = current2 as ChestObj;
                                if (chestObj != null) {
                                    SpriteObj spriteObj;
                                    if (chestObj.IsOpen)
                                        spriteObj = new SpriteObj("MapChestUnlocked_Sprite");
                                    else if (chestObj is FairyChestObj) {
                                        spriteObj = new SpriteObj("MapFairyChestIcon_Sprite");
                                        if ((chestObj as FairyChestObj).ConditionType == 10)
                                            spriteObj.Opacity = 0.2f;
                                    }
                                    else
                                        spriteObj = new SpriteObj("MapLockedChestIcon_Sprite");
                                    m_iconSpriteList.Add(spriteObj);
                                    spriteObj.AnimationDelay = 0.0333333351f;
                                    spriteObj.PlayAnimation(true);
                                    spriteObj.ForceDraw = true;
                                    spriteObj.Position = new Vector2(current2.X / m_spriteScale.X - 8f, current2.Y / m_spriteScale.Y - 12f);
                                    if (current.IsReversed)
                                        spriteObj.X -= (float)current2.Width / m_spriteScale.X;
                                    m_iconSpritePosList.Add(spriteObj.Position);
                                }
                            }
                            continue;
                        }
                    }
                    if (current.Name == "Bonus") {
                        SpriteObj spriteObj2 = new SpriteObj("MapBonusIcon_Sprite");
                        spriteObj2.PlayAnimation(true);
                        spriteObj2.AnimationDelay = 0.0333333351f;
                        spriteObj2.ForceDraw = true;
                        spriteObj2.Position = new Vector2((current.X + (float)current.Width / 2f) / m_spriteScale.X - (spriteObj2.Width / 2) - 1f, (current.Y + (float)current.Height / 2f) / m_spriteScale.Y - (spriteObj2.Height / 2) - 2f);
                        m_iconSpriteList.Add(spriteObj2);
                        m_iconSpritePosList.Add(spriteObj2.Position);
                    }
                }
            }
        }

        public void AddLinkerRoom(GameTypes.LevelType levelType, List<RoomObj> roomList) {
            foreach (RoomObj current in roomList) {
                if (current.Name == "Linker" && current.LevelType == levelType)
                    AddRoom(current);
            }
        }

        public void RefreshChestIcons(RoomObj room) {
            foreach (GameObj current in room.GameObjList) {
                ChestObj chestObj = current as ChestObj;
                if (chestObj != null && chestObj.IsOpen) {
                    Vector2 pt = new Vector2(chestObj.X / m_spriteScale.X - 8f, chestObj.Y / m_spriteScale.Y - 12f);
                    for (int i = 0; i < m_iconSpritePosList.Count; i++) {
                        if (CDGMath.DistanceBetweenPts(pt, m_iconSpritePosList[i]) < 15f) {
                            m_iconSpriteList[i].ChangeSprite("MapChestUnlocked_Sprite");
                            m_iconSpriteList[i].Opacity = 1f;
                            break;
                        }
                    }
                }
            }
        }

        public void CentreAroundPos(Vector2 pos, bool tween = false) {
            if (!tween) {
                CameraOffset.X = m_alphaMaskRect.X + m_alphaMaskRect.Width / 2f - pos.X / 1320f * 60f;
                CameraOffset.Y = m_alphaMaskRect.Y + m_alphaMaskRect.Height / 2f - pos.Y / 720f * 32f;
                return;
            }
            if (m_xOffsetTween != null && m_xOffsetTween.TweenedObject == this)
                m_xOffsetTween.StopTween(false);
            if (m_yOffsetTween != null && m_yOffsetTween.TweenedObject == this)
                m_yOffsetTween.StopTween(false);
            m_xOffsetTween = Tween.To(this, 0.3f, new Easing(Quad.EaseOut), new[] {
                "CameraOffsetX",
                (m_alphaMaskRect.X + m_alphaMaskRect.Width / 2f - pos.X / 1320f * 60f).ToString()
            });
            m_yOffsetTween = Tween.To(this, 0.3f, new Easing(Quad.EaseOut), new[] {
                "CameraOffsetY",
                (m_alphaMaskRect.Y + m_alphaMaskRect.Height / 2f - pos.Y / 720f * 32f).ToString()
            });
        }

        public void CentreAroundObj(GameObj obj) {
            CentreAroundPos(obj.Position, false);
        }

        public void CentreAroundPlayer() {
            CentreAroundObj(m_player);
        }

        public void TeleportPlayer(int index) {
            if (m_teleporterList.Count > 0) {
                Vector2 position = m_teleporterPosList[index];
                position.X += 10f;
                position.Y += 10f;
                position.X *= m_spriteScale.X;
                position.Y *= m_spriteScale.Y;
                position.X += 30f;
                if (index == 0) {
                    position.X -= 610f;
                    position.Y += 230f;
                }
                else
                    position.Y += 290f;
                m_player.TeleportPlayer(position, null);
            }
        }

        public void CentreAroundTeleporter(int index, bool tween = false) {
            Vector2 pos = m_teleporterPosList[index];
            pos.X *= m_spriteScale.X;
            pos.Y *= m_spriteScale.Y;
            CentreAroundPos(pos, tween);
        }

        public void DrawRenderTargets(Camera2D camera) {
            if (FollowPlayer) {
                CameraOffset.X = ((int)(m_alphaMaskRect.X + m_alphaMaskRect.Width / 2f - m_player.X / 1320f * 60f));
                CameraOffset.Y = m_alphaMaskRect.Y + m_alphaMaskRect.Height / 2f - ((int)m_player.Y) / 720f * 32f;
            }
            camera.GraphicsDevice.SetRenderTarget(m_mapScreenRT);
            camera.GraphicsDevice.Clear(Color.Transparent);
            for (int i = 0; i < m_roomSpriteList.Count; i++) {
                m_roomSpriteList[i].Position = CameraOffset + m_roomSpritePosList[i];
                m_roomSpriteList[i].Draw(camera);
            }
            for (int j = 0; j < m_doorSpriteList.Count; j++) {
                m_doorSpriteList[j].Position = CameraOffset + m_doorSpritePosList[j];
                m_doorSpriteList[j].Draw(camera);
            }
            if (!DrawTeleportersOnly) {
                for (int k = 0; k < m_iconSpriteList.Count; k++) {
                    m_iconSpriteList[k].Position = CameraOffset + m_iconSpritePosList[k];
                    m_iconSpriteList[k].Draw(camera);
                }
            }
            else {
                for (int l = 0; l < m_teleporterList.Count; l++) {
                    m_teleporterList[l].Position = CameraOffset + m_teleporterPosList[l];
                    m_teleporterList[l].Draw(camera);
                }
            }
            if (Game.PlayerStats.Traits.X == 28f || Game.PlayerStats.Traits.Y == 28f) {
                m_playerSprite.TextureColor = Color.Red;
                foreach (RoomObj current in m_addedRooms) {
                    foreach (EnemyObj current2 in current.EnemyList) {
                        if (!current2.IsKilled && !current2.IsDemented && current2.SaveToFile && current2.Type != 21 && current2.Type != 27 && current2.Type != 17) {
                            m_playerSprite.Position = new Vector2(current2.X / m_spriteScale.X - 9f, current2.Y / m_spriteScale.Y - 10f) + CameraOffset;
                            m_playerSprite.Draw(camera);
                        }
                    }
                }
            }
            m_playerSprite.TextureColor = Color.White;
            m_playerSprite.Position = new Vector2(m_level.Player.X / m_spriteScale.X - 9f, m_level.Player.Y / m_spriteScale.Y - 10f) + CameraOffset;
            m_playerSprite.Draw(camera);
        }

        public override void Draw(Camera2D camera) {
            if (base.Visible) {
                camera.End();
                camera.GraphicsDevice.Textures[1] = m_alphaMaskRT;
                camera.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
                camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.MaskEffect);
                if (!DrawNothing)
                    camera.Draw(m_mapScreenRT, Vector2.Zero, Color.White);
                camera.End();
                camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
                if (DrawNothing)
                    m_playerSprite.Draw(camera);
            }
        }

        public void ClearRoomsAdded() {
            m_addedRooms.Clear();
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_player = null;
                m_level = null;
                if (m_alphaMaskRT != null && !m_alphaMaskRT.IsDisposed)
                    m_alphaMaskRT.Dispose();
                m_alphaMaskRT = null;
                if (m_mapScreenRT != null && !m_mapScreenRT.IsDisposed)
                    m_mapScreenRT.Dispose();
                m_mapScreenRT = null;
                foreach (SpriteObj current in m_roomSpriteList)
                    current.Dispose();
                m_roomSpriteList.Clear();
                m_roomSpriteList = null;
                foreach (SpriteObj current2 in m_doorSpriteList)
                    current2.Dispose();
                m_doorSpriteList.Clear();
                m_doorSpriteList = null;
                foreach (SpriteObj current3 in m_iconSpriteList)
                    current3.Dispose();
                m_iconSpriteList.Clear();
                m_iconSpriteList = null;
                m_addedRooms.Clear();
                m_addedRooms = null;
                m_roomSpritePosList.Clear();
                m_roomSpritePosList = null;
                m_doorSpritePosList.Clear();
                m_doorSpritePosList = null;
                m_iconSpritePosList.Clear();
                m_iconSpritePosList = null;
                m_playerSprite.Dispose();
                m_playerSprite = null;
                foreach (SpriteObj current4 in m_teleporterList)
                    current4.Dispose();
                m_teleporterList.Clear();
                m_teleporterList = null;
                m_teleporterPosList.Clear();
                m_teleporterPosList = null;
                m_xOffsetTween = null;
                m_yOffsetTween = null;
                base.Dispose();
            }
        }

        protected override GameObj CreateCloneInstance() {
            return new MapObj(FollowPlayer, m_level);
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            MapObj mapObj = obj as MapObj;
            mapObj.DrawTeleportersOnly = DrawTeleportersOnly;
            mapObj.CameraOffsetX = CameraOffsetX;
            mapObj.CameraOffsetY = CameraOffsetY;
            mapObj.InitializeAlphaMap(m_mapScreenRT, m_alphaMaskRT, m_alphaMaskRect);
            mapObj.SetPlayer(m_player);
            mapObj.AddAllRooms(m_addedRooms);
        }

        public SpriteObj[] TeleporterList() {
            return m_teleporterList.ToArray();
        }
    }
}
