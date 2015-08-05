using System;
using System.Collections.Generic;
using System.IO;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Tweener;


namespace RogueCastle {

    public class SaveGameManager {

        private bool _autosaveLoaded;

        private string _fileNameLineage  = "RogueLegacyLineage.rcdat";
        private string _fileNameMap      = "RogueLegacyMap.rcdat";
        private string _fileNameMapData  = "RogueLegacyMapDat.rcdat";
        private string _fileNamePlayer   = "RogueLegacyPlayer.rcdat";
        private string _fileNameUpgrades = "RogueLegacyBP.rcdat";

        private Game _game;

        private int _saveFailCounter;

        private StorageContainer _storageContainer;
        private StorageContainer _modStorageContainer;

        public SaveGameManager(Game game) {

            _saveFailCounter = 0;
            _autosaveLoaded  = false;
            _game            = game;

        }

        public void Initialize() {

            if (LevelEV.RUN_DEMO_VERSION) {
                _fileNamePlayer   = "RogueLegacyDemoPlayer.rcdat";
                _fileNameUpgrades = "RogueLegacyDemoBP.rcdat";
                _fileNameMap      = "RogueLegacyDemoMap.rcdat";
                _fileNameMapData  = "RogueLegacyDemoMapDat.rcdat";
                _fileNameLineage  = "RogueLegacyDemoLineage.rcdat";
            }

            if (_storageContainer != null) {

                _storageContainer.Dispose();
                _storageContainer = null;

            }

            if (_modStorageContainer != null) {

                _modStorageContainer.Dispose();
                _modStorageContainer = null;

            }

            PerformDirectoryCheck();

        }

        private void GetStorageContainer() {

            if (_storageContainer == null || _storageContainer.IsDisposed || _modStorageContainer == null || _modStorageContainer.IsDisposed) {

                IAsyncResult asyncResult = StorageDevice.BeginShowSelector(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                StorageDevice storageDevice = StorageDevice.EndShowSelector(asyncResult);
                asyncResult.AsyncWaitHandle.Close();
                asyncResult = storageDevice.BeginOpenContainer("RogueLegacyStorageContainer", null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                _storageContainer = storageDevice.EndOpenContainer(asyncResult);
                asyncResult.AsyncWaitHandle.Close();

                IAsyncResult modAsyncResult = StorageDevice.BeginShowSelector(null, null);
                modAsyncResult.AsyncWaitHandle.WaitOne();
                StorageDevice modStorageDevice = StorageDevice.EndShowSelector(modAsyncResult);
                modAsyncResult.AsyncWaitHandle.Close();
                modAsyncResult = modStorageDevice.BeginOpenContainer("RLReduxStorageContainer", null, null);
                modAsyncResult.AsyncWaitHandle.WaitOne();
                _modStorageContainer = modStorageDevice.EndOpenContainer(modAsyncResult);
                modAsyncResult.AsyncWaitHandle.Close();

            }

        }

        private void PerformDirectoryCheck() {

            GetStorageContainer();

            if (!_storageContainer.DirectoryExists("Profile1")) {

                _storageContainer.CreateDirectory("Profile1");
                CopyFile(_storageContainer, _fileNamePlayer, "Profile1");
                CopyFile(_storageContainer, "AutoSave_" + _fileNamePlayer, "Profile1");
                CopyFile(_storageContainer, _fileNameUpgrades, "Profile1");
                CopyFile(_storageContainer, "AutoSave_" + _fileNameUpgrades, "Profile1");
                CopyFile(_storageContainer, _fileNameMap, "Profile1");
                CopyFile(_storageContainer, "AutoSave_" + _fileNameMap, "Profile1");
                CopyFile(_storageContainer, _fileNameMapData, "Profile1");
                CopyFile(_storageContainer, "AutoSave_" + _fileNameMapData, "Profile1");
                CopyFile(_storageContainer, _fileNameLineage, "Profile1");
                CopyFile(_storageContainer, "AutoSave_" + _fileNameLineage, "Profile1");
                
            }

            if (!_storageContainer.DirectoryExists("Profile2"))
                _storageContainer.CreateDirectory("Profile2");

            if (!_storageContainer.DirectoryExists("Profile3"))
                _storageContainer.CreateDirectory("Profile3");

            for (int i = 1; i < 4; i++) {

                string profile = "Profile" + i.ToString();

                if (!_modStorageContainer.DirectoryExists(profile)) {

                    _modStorageContainer.CreateDirectory(profile);

                    if (_storageContainer.DirectoryExists(profile)) {

                        CopyFileToModFile(_storageContainer, _modStorageContainer, _fileNamePlayer, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, "AutoSave_" + _fileNamePlayer, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, _fileNameUpgrades, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, "AutoSave_" + _fileNameUpgrades, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, _fileNameMap, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, "AutoSave_" + _fileNameMap, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, _fileNameMapData, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, "AutoSave_" + _fileNameMapData, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, _fileNameLineage, profile);
                        CopyFileToModFile(_storageContainer, _modStorageContainer, "AutoSave_" + _fileNameLineage, profile);

                    }

                }

            }

            _storageContainer.Dispose();
            _storageContainer = null;

            _modStorageContainer.Dispose();
            _modStorageContainer = null;

        }

        private void CopyFileToModFile(StorageContainer originalContainer, StorageContainer modContainer, string fileName, string profileName) {
            
            if (originalContainer.FileExists(fileName)) {
                
                Stream originalFile = originalContainer.OpenFile(profileName + "/" + fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                Stream modFile = modContainer.CreateFile(profileName + "/" + fileName);

                originalFile.CopyTo(modFile);
                originalFile.Close();
                modFile.Close();

            }

        }

        private void CopyFile(StorageContainer storageContainer, string fileName, string profileName) {
            
            if (storageContainer.FileExists(fileName)) {

                Stream stream = storageContainer.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                Stream stream2 = storageContainer.CreateFile(profileName + "/" + fileName);
                stream.CopyTo(stream2);
                stream.Close();
                stream2.Close();

            }

        }

        public void SaveFiles(params SaveType[] saveList) {
            
            if (!LevelEV.DISABLE_SAVING) {

                GetStorageContainer();
                
                try {
                    
                    for (int i = 0; i < saveList.Length; i++) {
                        SaveType saveType = saveList[i];
                        SaveData(saveType, false);
                    }

                    _saveFailCounter = 0;

                } catch {
                    
                    if (_saveFailCounter > 2) {

                        RCScreenManager screenManager = Game.ScreenManager;
                        screenManager.DialogueScreen.SetDialogue("Save File Error Antivirus");
                        Tween.RunFunction(0.25f, screenManager, "DisplayScreen", new object[] {
                            13,
                            true,
                            typeof(List<object>)
                        });

                        _saveFailCounter = 0;

                    }
                    else
                        _saveFailCounter++;

                } finally {

                    if (_modStorageContainer != null && !_modStorageContainer.IsDisposed)
                        _modStorageContainer.Dispose();

                    _modStorageContainer = null;

                }

            }

        }

        public void SaveBackupFiles(params SaveType[] saveList) {

            if (!LevelEV.DISABLE_SAVING) {

                GetStorageContainer();

                for (int i = 0; i < saveList.Length; i++) {
                    SaveType saveType = saveList[i];
                    SaveData(saveType, true);
                }

                _modStorageContainer.Dispose();
                _modStorageContainer = null;

            }

        }

        public void SaveAllFileTypes(bool saveBackup) {

            if (!saveBackup) {

                SaveFiles(new[] {
                    SaveType.PlayerData,
                    SaveType.UpgradeData,
                    SaveType.Map,
                    SaveType.MapData,
                    SaveType.Lineage
                });

                return;

            }

            SaveBackupFiles(new[] {
                SaveType.PlayerData,
                SaveType.UpgradeData,
                SaveType.Map,
                SaveType.MapData,
                SaveType.Lineage
            });

        }

        public void LoadFiles(ProceduralLevelScreen level, params SaveType[] loadList) {

            if (LevelEV.ENABLE_BACKUP_SAVING) {

                GetStorageContainer();
                
                SaveType saveType = SaveType.None;
                
                try {

                    try {

                        if (!LevelEV.DISABLE_SAVING) {

                            for (int i = 0; i < loadList.Length; i++) {
                                SaveType saveType2 = loadList[i];
                                saveType = saveType2;
                                LoadData(saveType2, level);
                            }

                        }

                    } catch {

                        if (saveType == SaveType.Map || saveType == SaveType.MapData || saveType == SaveType.None)
                            throw new Exception();
                        
                        if (!_autosaveLoaded) {
                            RCScreenManager screenManager = Game.ScreenManager;
                            screenManager.DialogueScreen.SetDialogue("Save File Error");
                            screenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave", new object[0]);
                            screenManager.DisplayScreen(13, false, null);
                            Game.PlayerStats.HeadPiece = 0;
                        }
                        else {
                            _autosaveLoaded = false;
                            RCScreenManager screenManager2 = Game.ScreenManager;
                            screenManager2.DialogueScreen.SetDialogue("Save File Error 2");
                            screenManager2.DialogueScreen.SetConfirmEndHandler(this, "StartNewGame", new object[0]);
                            screenManager2.DisplayScreen(13, false, null);
                            Game.PlayerStats.HeadPiece = 0;
                        }

                    }

                    return;

                } finally {

                    if (_modStorageContainer != null && !_modStorageContainer.IsDisposed)
                        _modStorageContainer.Dispose();

                }

            }

            if (!LevelEV.DISABLE_SAVING) {

                GetStorageContainer();
                
                for (int j = 0; j < loadList.Length; j++) {
                    SaveType loadType = loadList[j];
                    LoadData(loadType, level);
                }

                _modStorageContainer.Dispose();
                _modStorageContainer = null;

            }

        }

        public void ForceBackup() {

            if (_modStorageContainer != null && !_modStorageContainer.IsDisposed)
                _modStorageContainer.Dispose();
            
            RCScreenManager screenManager = Game.ScreenManager;
            screenManager.DialogueScreen.SetDialogue("Save File Error");
            screenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave", new object[0]);
            screenManager.DisplayScreen(13, false);

        }

        public void LoadAutosave() {

            Console.WriteLine("Save file corrupted");
            SkillSystem.ResetAllTraits();
            Game.PlayerStats.Dispose();
            Game.PlayerStats = new PlayerStats();
            Game.ScreenManager.Player.Reset();
            LoadBackups();
            Game.ScreenManager.DisplayScreen(3, true);

        }

        public void StartNewGame() {

            ClearAllFileTypes(false);
            ClearAllFileTypes(true);
            SkillSystem.ResetAllTraits();
            Game.PlayerStats.Dispose();
            Game.PlayerStats = new PlayerStats();
            Game.ScreenManager.Player.Reset();
            Game.ScreenManager.DisplayScreen(23, true);

        }

        public void ResetAutosave() {
            _autosaveLoaded = false;
        }

        public void LoadAllFileTypes(ProceduralLevelScreen level) {
            
            LoadFiles(level, new[] {
                SaveType.PlayerData,
                SaveType.UpgradeData,
                SaveType.Map,
                SaveType.MapData,
                SaveType.Lineage
            });

        }

        public void ClearFiles(params SaveType[] deleteList) {
            
            GetStorageContainer();
            
            for (int i = 0; i < deleteList.Length; i++) {
                SaveType deleteType = deleteList[i];
                DeleteData(deleteType);
            }

            _modStorageContainer.Dispose();
            _modStorageContainer = null;

        }

        public void ClearBackupFiles(params SaveType[] deleteList) {
            
            GetStorageContainer();
            
            for (int i = 0; i < deleteList.Length; i++) {
                SaveType deleteType = deleteList[i];
                DeleteBackupData(deleteType);
            }

            _modStorageContainer.Dispose();
            _modStorageContainer = null;

        }

        public void ClearAllFileTypes(bool deleteBackups) {
            
            if (!deleteBackups) {

                ClearFiles(new[] {
                    SaveType.PlayerData,
                    SaveType.UpgradeData,
                    SaveType.Map,
                    SaveType.MapData,
                    SaveType.Lineage
                });

                return;

            }

            ClearBackupFiles(new[] {
                SaveType.PlayerData,
                SaveType.UpgradeData,
                SaveType.Map,
                SaveType.MapData,
                SaveType.Lineage
            });

        }

        private void DeleteData(SaveType deleteType) {

            switch (deleteType) {

                case SaveType.PlayerData:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNamePlayer
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/",
                            _fileNamePlayer
                        }));
                    }

                    break;

                case SaveType.UpgradeData:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNameUpgrades
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/",
                            _fileNameUpgrades
                        }));
                    }

                    break;

                case SaveType.Map:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNameMap
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/",
                            _fileNameMap
                        }));
                    }

                    break;

                case SaveType.MapData:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNameMapData
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/",
                            _fileNameMapData
                        }));
                    }

                    break;

                case SaveType.Lineage:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNameLineage
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/",
                            _fileNameLineage
                        }));
                    }

                    break;

            }

            Console.WriteLine("Save file type " + deleteType + " deleted.");

        }

        private void DeleteBackupData(SaveType deleteType) {

            switch (deleteType) {

                case SaveType.PlayerData:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/AutoSave_",
                        _fileNamePlayer
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/AutoSave_",
                            _fileNamePlayer
                        }));
                    }

                    break;

                case SaveType.UpgradeData:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/AutoSave_",
                        _fileNameUpgrades
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/AutoSave_",
                            _fileNameUpgrades
                        }));
                    }

                    break;

                case SaveType.Map:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/AutoSave_",
                        _fileNameMap
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/AutoSave_",
                            _fileNameMap
                        }));
                    }

                    break;

                case SaveType.MapData:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/AutoSave_",
                        _fileNameMapData
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/AutoSave_",
                            _fileNameMapData
                        }));
                    }

                    break;

                case SaveType.Lineage:
                    if (_modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/AutoSave_",
                        _fileNameLineage
                    }))) {
                        _modStorageContainer.DeleteFile(string.Concat(new object[] {
                            "Profile",
                            Game.GameConfig.ProfileSlot,
                            "/AutoSave_",
                            _fileNameLineage
                        }));
                    }

                    break;

            }

            Console.WriteLine("Backup save file type " + deleteType + " deleted.");

        }

        private void LoadBackups() {

            Console.WriteLine("Replacing save file with back up saves");
            
            GetStorageContainer();
            
            if (_modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/AutoSave_",
                _fileNamePlayer
            })) && _modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNamePlayer
            }))) {
                Stream stream = _modStorageContainer.OpenFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/AutoSave_",
                    _fileNamePlayer
                }), FileMode.Open);
                Stream stream2 = _modStorageContainer.CreateFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/",
                    _fileNamePlayer
                }));
                stream.CopyTo(stream2);
                stream.Close();
                stream2.Close();
            }

            if (_modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/AutoSave_",
                _fileNameUpgrades
            })) && _modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNameUpgrades
            }))) {
                Stream stream3 = _modStorageContainer.OpenFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/AutoSave_",
                    _fileNameUpgrades
                }), FileMode.Open);
                Stream stream4 = _modStorageContainer.CreateFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/",
                    _fileNameUpgrades
                }));
                stream3.CopyTo(stream4);
                stream3.Close();
                stream4.Close();
            }

            if (_modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/AutoSave_",
                _fileNameMap
            })) && _modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNameMap
            }))) {
                Stream stream5 = _modStorageContainer.OpenFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/AutoSave_",
                    _fileNameMap
                }), FileMode.Open);
                Stream stream6 = _modStorageContainer.CreateFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/",
                    _fileNameMap
                }));
                stream5.CopyTo(stream6);
                stream5.Close();
                stream6.Close();
            }

            if (_modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/AutoSave_",
                _fileNameMapData
            })) && _modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNameMapData
            }))) {
                Stream stream7 = _modStorageContainer.OpenFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/AutoSave_",
                    _fileNameMapData
                }), FileMode.Open);
                Stream stream8 = _modStorageContainer.CreateFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/",
                    _fileNameMapData
                }));
                stream7.CopyTo(stream8);
                stream7.Close();
                stream8.Close();
            }

            if (_modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/AutoSave_",
                _fileNameLineage
            })) && _modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNameLineage
            }))) {
                Stream stream9 = _modStorageContainer.OpenFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/AutoSave_",
                    _fileNameLineage
                }), FileMode.Open);
                Stream stream10 = _modStorageContainer.CreateFile(string.Concat(new object[] {
                    "Profile",
                    Game.GameConfig.ProfileSlot,
                    "/",
                    _fileNameLineage
                }));
                stream9.CopyTo(stream10);
                stream9.Close();
                stream10.Close();
            }

            _autosaveLoaded      = true;
            _modStorageContainer.Dispose();
            _modStorageContainer = null;

        }

        private void SaveData(SaveType saveType, bool saveBackup) {
            
            switch (saveType) {
                case SaveType.PlayerData:
                    SavePlayerData(saveBackup);
                    break;
                case SaveType.UpgradeData:
                    SaveUpgradeData(saveBackup);
                    break;
                case SaveType.Map:
                    SaveMap(saveBackup);
                    break;
                case SaveType.MapData:
                    SaveMapData(saveBackup);
                    break;
                case SaveType.Lineage:
                    SaveLineageData(saveBackup);
                    break;
            }

            Console.WriteLine("\nData type " + saveType + " saved!");

        }

        private void SavePlayerData(bool saveBackup) {
            
            string text = _fileNamePlayer;
            
            if (saveBackup)
                text = text.Insert(0, "AutoSave_");
            
            text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

            using (Stream stream = _modStorageContainer.CreateFile(text)) {

                using (BinaryWriter binaryWriter = new BinaryWriter(stream)) {

                    binaryWriter.Write(Game.PlayerStats.Gold);
                    Game.PlayerStats.CurrentHealth = Game.ScreenManager.Player.CurrentHealth;
                    binaryWriter.Write(Game.PlayerStats.CurrentHealth);
                    Game.PlayerStats.CurrentMana = (int)Game.ScreenManager.Player.CurrentMana;
                    binaryWriter.Write(Game.PlayerStats.CurrentMana);
                    binaryWriter.Write(Game.PlayerStats.Age);
                    binaryWriter.Write(Game.PlayerStats.ChildAge);
                    binaryWriter.Write(Game.PlayerStats.Spell);
                    binaryWriter.Write(Game.PlayerStats.Class);
                    binaryWriter.Write(Game.PlayerStats.SpecialItem);
                    binaryWriter.Write((byte)Game.PlayerStats.Traits.X);
                    binaryWriter.Write((byte)Game.PlayerStats.Traits.Y);
                    binaryWriter.Write(Game.PlayerStats.PlayerName);
                    binaryWriter.Write(Game.PlayerStats.HeadPiece);
                    binaryWriter.Write(Game.PlayerStats.ShoulderPiece);
                    binaryWriter.Write(Game.PlayerStats.ChestPiece);
                    binaryWriter.Write(Game.PlayerStats.DiaryEntry);
                    binaryWriter.Write(Game.PlayerStats.BonusHealth);
                    binaryWriter.Write(Game.PlayerStats.BonusStrength);
                    binaryWriter.Write(Game.PlayerStats.BonusMana);
                    binaryWriter.Write(Game.PlayerStats.BonusDefense);
                    binaryWriter.Write(Game.PlayerStats.BonusWeight);
                    binaryWriter.Write(Game.PlayerStats.BonusMagic);
                    binaryWriter.Write(Game.PlayerStats.LichHealth);
                    binaryWriter.Write(Game.PlayerStats.LichMana);
                    binaryWriter.Write(Game.PlayerStats.LichHealthMod);
                    binaryWriter.Write(Game.PlayerStats.NewBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.EyeballBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.FairyBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.FireballBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.BlobBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.LastbossBeaten);
                    binaryWriter.Write(Game.PlayerStats.TimesCastleBeaten);
                    binaryWriter.Write(Game.PlayerStats.NumEnemiesBeaten);
                    binaryWriter.Write(Game.PlayerStats.TutorialComplete);
                    binaryWriter.Write(Game.PlayerStats.CharacterFound);
                    binaryWriter.Write(Game.PlayerStats.LoadStartingRoom);
                    binaryWriter.Write(Game.PlayerStats.LockCastle);
                    binaryWriter.Write(Game.PlayerStats.SpokeToBlacksmith);
                    binaryWriter.Write(Game.PlayerStats.SpokeToEnchantress);
                    binaryWriter.Write(Game.PlayerStats.SpokeToArchitect);
                    binaryWriter.Write(Game.PlayerStats.SpokeToTollCollector);
                    binaryWriter.Write(Game.PlayerStats.IsDead);
                    binaryWriter.Write(Game.PlayerStats.FinalDoorOpened);
                    binaryWriter.Write(Game.PlayerStats.RerolledChildren);
                    binaryWriter.Write(Game.PlayerStats.IsFemale);
                    binaryWriter.Write(Game.PlayerStats.TimesDead);
                    binaryWriter.Write(Game.PlayerStats.HasArchitectFee);
                    binaryWriter.Write(Game.PlayerStats.ReadLastDiary);
                    binaryWriter.Write(Game.PlayerStats.SpokenToLastBoss);
                    binaryWriter.Write(Game.PlayerStats.HardcoreMode);
                    float value = Game.PlayerStats.TotalHoursPlayed + Game.PlaySessionLength;
                    binaryWriter.Write(value);
                    binaryWriter.Write((byte)Game.PlayerStats.WizardSpellList.X);
                    binaryWriter.Write((byte)Game.PlayerStats.WizardSpellList.Y);
                    binaryWriter.Write((byte)Game.PlayerStats.WizardSpellList.Z);
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        Console.WriteLine("\nSaving Player Stats");
                        Console.WriteLine("Gold: " + Game.PlayerStats.Gold);
                        Console.WriteLine("Current Health: " + Game.PlayerStats.CurrentHealth);
                        Console.WriteLine("Current Mana: " + Game.PlayerStats.CurrentMana);
                        Console.WriteLine("Age: " + Game.PlayerStats.Age);
                        Console.WriteLine("Child Age: " + Game.PlayerStats.ChildAge);
                        Console.WriteLine("Spell: " + Game.PlayerStats.Spell);
                        Console.WriteLine("Class: " + Game.PlayerStats.Class);
                        Console.WriteLine("Special Item: " + Game.PlayerStats.SpecialItem);
                        Console.WriteLine(string.Concat(new object[] {
                            "Traits: ",
                            Game.PlayerStats.Traits.X,
                            ", ",
                            Game.PlayerStats.Traits.Y
                        }));
                        Console.WriteLine("Name: " + Game.PlayerStats.PlayerName);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Head Piece: " + Game.PlayerStats.HeadPiece);
                        Console.WriteLine("Shoulder Piece: " + Game.PlayerStats.ShoulderPiece);
                        Console.WriteLine("Chest Piece: " + Game.PlayerStats.ChestPiece);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Diary Entry: " + Game.PlayerStats.DiaryEntry);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Bonus Health: " + Game.PlayerStats.BonusHealth);
                        Console.WriteLine("Bonus Strength: " + Game.PlayerStats.BonusStrength);
                        Console.WriteLine("Bonus Mana: " + Game.PlayerStats.BonusMana);
                        Console.WriteLine("Bonus Armor: " + Game.PlayerStats.BonusDefense);
                        Console.WriteLine("Bonus Weight: " + Game.PlayerStats.BonusWeight);
                        Console.WriteLine("Bonus Magic: " + Game.PlayerStats.BonusMagic);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Lich Health: " + Game.PlayerStats.LichHealth);
                        Console.WriteLine("Lich Mana: " + Game.PlayerStats.LichMana);
                        Console.WriteLine("Lich Health Mod: " + Game.PlayerStats.LichHealthMod);
                        Console.WriteLine("---------------");
                        Console.WriteLine("New Boss Beaten: " + Game.PlayerStats.NewBossBeaten);
                        Console.WriteLine("Eyeball Boss Beaten: " + Game.PlayerStats.EyeballBossBeaten);
                        Console.WriteLine("Fairy Boss Beaten: " + Game.PlayerStats.FairyBossBeaten);
                        Console.WriteLine("Fireball Boss Beaten: " + Game.PlayerStats.FireballBossBeaten);
                        Console.WriteLine("Blob Boss Beaten: " + Game.PlayerStats.BlobBossBeaten);
                        Console.WriteLine("Last Boss Beaten: " + Game.PlayerStats.LastbossBeaten);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Times Castle Beaten: " + Game.PlayerStats.TimesCastleBeaten);
                        Console.WriteLine("Number of Enemies Beaten: " + Game.PlayerStats.NumEnemiesBeaten);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Tutorial Complete: " + Game.PlayerStats.TutorialComplete);
                        Console.WriteLine("Character Found: " + Game.PlayerStats.CharacterFound);
                        Console.WriteLine("Load Starting Room: " + Game.PlayerStats.LoadStartingRoom);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Spoke to Blacksmith: " + Game.PlayerStats.SpokeToBlacksmith);
                        Console.WriteLine("Spoke to Enchantress: " + Game.PlayerStats.SpokeToEnchantress);
                        Console.WriteLine("Spoke to Architect: " + Game.PlayerStats.SpokeToArchitect);
                        Console.WriteLine("Spoke to Toll Collector: " + Game.PlayerStats.SpokeToTollCollector);
                        Console.WriteLine("Player Is Dead: " + Game.PlayerStats.IsDead);
                        Console.WriteLine("Final Door Opened: " + Game.PlayerStats.FinalDoorOpened);
                        Console.WriteLine("Rerolled Children: " + Game.PlayerStats.RerolledChildren);
                        Console.WriteLine("Is Female: " + Game.PlayerStats.IsFemale);
                        Console.WriteLine("Times Dead: " + Game.PlayerStats.TimesDead);
                        Console.WriteLine("Has Architect Fee: " + Game.PlayerStats.HasArchitectFee);
                        Console.WriteLine("Player read last diary: " + Game.PlayerStats.ReadLastDiary);
                        Console.WriteLine("Player has spoken to last boss: " + Game.PlayerStats.SpokenToLastBoss);
                        Console.WriteLine("Is Hardcore mode: " + Game.PlayerStats.HardcoreMode);
                        Console.WriteLine("Total Hours Played " + Game.PlayerStats.TotalHoursPlayed);
                        Console.WriteLine("Wizard Spell 1: " + Game.PlayerStats.WizardSpellList.X);
                        Console.WriteLine("Wizard Spell 2: " + Game.PlayerStats.WizardSpellList.Y);
                        Console.WriteLine("Wizard Spell 3: " + Game.PlayerStats.WizardSpellList.Z);
                    }

                    Console.WriteLine("///// ENEMY LIST DATA - BEGIN SAVING /////");
                    List<Vector4> enemiesKilledList = Game.PlayerStats.EnemiesKilledList;

                    for (int index = 0; index < enemiesKilledList.Count; index++) {
                        Vector4 current = enemiesKilledList[index];
                        binaryWriter.Write((byte)current.X);
                        binaryWriter.Write((byte)current.Y);
                        binaryWriter.Write((byte)current.Z);
                        binaryWriter.Write((byte)current.W);
                    }

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        
                        Console.WriteLine("Saving Enemy List Data");
                        int num = 0;

                        for (int index = 0; index < enemiesKilledList.Count; index++) {
                            Vector4 current2 = enemiesKilledList[index];
                            Console.WriteLine(string.Concat(new object[] {
                                "Enemy Type: ",
                                num,
                                ", Difficulty: Basic, Killed: ",
                                current2.X
                            }));
                            Console.WriteLine(string.Concat(new object[] {
                                "Enemy Type: ",
                                num,
                                ", Difficulty: Advanced, Killed: ",
                                current2.Y
                            }));
                            Console.WriteLine(string.Concat(new object[] {
                                "Enemy Type: ",
                                num,
                                ", Difficulty: Expert, Killed: ",
                                current2.Z
                            }));
                            Console.WriteLine(string.Concat(new object[] {
                                "Enemy Type: ",
                                num,
                                ", Difficulty: Miniboss, Killed: ",
                                current2.W
                            }));
                            num++;
                        }
                    }

                    int count = Game.PlayerStats.EnemiesKilledInRun.Count;
                    List<Vector2> enemiesKilledInRun = Game.PlayerStats.EnemiesKilledInRun;
                    binaryWriter.Write(count);

                    for (int index = 0; index < enemiesKilledInRun.Count; index++) {
                        Vector2 current3 = enemiesKilledInRun[index];
                        binaryWriter.Write((int)current3.X);
                        binaryWriter.Write((int)current3.Y);
                    }

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        Console.WriteLine("Saving num enemies killed");
                        Console.WriteLine("Number of enemies killed in run: " + count);
                        for (int index = 0; index < enemiesKilledInRun.Count; index++) {
                            Vector2 current4 = enemiesKilledInRun[index];
                            Console.WriteLine("Enemy Room Index: " + current4.X);
                            Console.WriteLine("Enemy Index in EnemyList: " + current4.Y);
                        }
                    }

                    Console.WriteLine("///// ENEMY LIST DATA - SAVE COMPLETE /////");
                    Console.WriteLine("///// DLC DATA - BEGIN SAVING /////");
                    binaryWriter.Write(Game.PlayerStats.ChallengeEyeballUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeSkullUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeFireballUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeBlobUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeLastBossUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeEyeballBeaten);
                    binaryWriter.Write(Game.PlayerStats.ChallengeSkullBeaten);
                    binaryWriter.Write(Game.PlayerStats.ChallengeFireballBeaten);
                    binaryWriter.Write(Game.PlayerStats.ChallengeBlobBeaten);
                    binaryWriter.Write(Game.PlayerStats.ChallengeLastBossBeaten);
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        Console.WriteLine("Eyeball Challenge Unlocked: " + Game.PlayerStats.ChallengeEyeballUnlocked);
                        Console.WriteLine("Skull Challenge Unlocked: " + Game.PlayerStats.ChallengeSkullUnlocked);
                        Console.WriteLine("Fireball Challenge Unlocked: " + Game.PlayerStats.ChallengeFireballUnlocked);
                        Console.WriteLine("Blob Challenge Unlocked: " + Game.PlayerStats.ChallengeBlobUnlocked);
                        Console.WriteLine("Last Boss Challenge Unlocked: " + Game.PlayerStats.ChallengeLastBossUnlocked);
                        Console.WriteLine("Eyeball Challenge Beaten: " + Game.PlayerStats.ChallengeEyeballBeaten);
                        Console.WriteLine("Skull Challenge Beaten: " + Game.PlayerStats.ChallengeSkullBeaten);
                        Console.WriteLine("Fireball Challenge Beaten: " + Game.PlayerStats.ChallengeFireballBeaten);
                        Console.WriteLine("Blob Challenge Beaten: " + Game.PlayerStats.ChallengeBlobBeaten);
                        Console.WriteLine("Last Boss Challenge Beaten: " + Game.PlayerStats.ChallengeLastBossBeaten);
                    }

                    Console.WriteLine("///// DLC DATA - SAVE COMPLETE /////");
                    
                    if (saveBackup) {
                        FileStream fileStream = stream as FileStream;
                        if (fileStream != null)
                            fileStream.Flush(true);
                    }

                    binaryWriter.Close();

                }

                stream.Close();

            }

        }

        private void SaveUpgradeData(bool saveBackup) {
            
            string text = _fileNameUpgrades;
            
            if (saveBackup)
                text = text.Insert(0, "AutoSave_");
            
            text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

            using (Stream stream = _modStorageContainer.CreateFile(text)) {

                using (BinaryWriter binaryWriter = new BinaryWriter(stream)) {

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nSaving Equipment States");
                    
                    List<byte[]> getBlueprintArray = Game.PlayerStats.GetBlueprintArray;
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("Standard Blueprints");

                    for (int index = 0; index < getBlueprintArray.Count; index++) {
                        
                        byte[] current = getBlueprintArray[index];
                        byte[] array = current;

                        for (int i = 0; i < array.Length; i++) {
                            byte b = array[i];
                            binaryWriter.Write(b);
                            if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                                Console.Write(" " + b);
                        }

                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write("\n");

                    }

                    List<byte[]> getRuneArray = Game.PlayerStats.GetRuneArray;
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nRune Blueprints");

                    for (int index = 0; index < getRuneArray.Count; index++) {
                        
                        byte[] current2 = getRuneArray[index];
                        byte[] array2 = current2;
                        
                        for (int j = 0; j < array2.Length; j++) {
                            byte b2 = array2[j];
                            binaryWriter.Write(b2);
                            if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                                Console.Write(" " + b2);
                        }

                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write("\n");

                    }

                    sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nEquipped Standard Item");
                    
                    sbyte[] array3 = getEquippedArray;
                    
                    for (int k = 0; k < array3.Length; k++) {
                        sbyte b3 = array3[k];
                        binaryWriter.Write(b3);
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write(" " + b3);
                    }

                    sbyte[] getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nEquipped Abilities");
                    
                    sbyte[] array4 = getEquippedRuneArray;
                    
                    for (int l = 0; l < array4.Length; l++) {
                        sbyte b4 = array4[l];
                        binaryWriter.Write(b4);
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write(" " + b4);
                    }

                    SkillObj[] skillArray = SkillSystem.GetSkillArray();
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nskills");
                    
                    SkillObj[] array5 = skillArray;
                    
                    for (int m = 0; m < array5.Length; m++) {
                        SkillObj skillObj = array5[m];
                        binaryWriter.Write(skillObj.CurrentLevel);
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write(" " + skillObj.CurrentLevel);
                    }

                    if (saveBackup) {
                        FileStream fileStream = stream as FileStream;
                        if (fileStream != null)
                            fileStream.Flush(true);
                    }

                    binaryWriter.Close();

                }

                stream.Close();

            }

        }

        private void SaveMap(bool saveBackup) {

            string text = _fileNameMap;
            
            if (saveBackup)
                text = text.Insert(0, "AutoSave_");
            
            text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

            using (Stream stream = _modStorageContainer.CreateFile(text)) {

                using (BinaryWriter binaryWriter = new BinaryWriter(stream)) {

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nSaving Map");
                    
                    int num = 0;
                    
                    ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
                    
                    if (levelScreen != null) {
                        
                        if (LevelEV.RUN_DEMO_VERSION)
                            binaryWriter.Write(levelScreen.RoomList.Count - 4);
                        else
                            binaryWriter.Write(levelScreen.RoomList.Count - 12);
                        
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.WriteLine("Map size: " + (levelScreen.RoomList.Count - 12));
                        
                        List<byte> list = new List<byte>();
                        List<byte> list2 = new List<byte>();

                        for (int index = 0; index < levelScreen.RoomList.Count; index++) {
                            
                            RoomObj current = levelScreen.RoomList[index];
                            
                            if (current.Name != "Boss" && current.Name != "Tutorial" && current.Name != "Ending" && current.Name != "Compass" && current.Name != "ChallengeBoss") {
                                
                                binaryWriter.Write(current.PoolIndex);
                                binaryWriter.Write((byte)current.LevelType);
                                binaryWriter.Write((int)current.X);
                                binaryWriter.Write((int)current.Y);
                                binaryWriter.Write(current.TextureColor.R);
                                binaryWriter.Write(current.TextureColor.G);
                                binaryWriter.Write(current.TextureColor.B);
                                
                                if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                                    Console.Write(string.Concat(new object[] {
                                        "I:",
                                        current.PoolIndex,
                                        " T:",
                                        (int)current.LevelType,
                                        ", "
                                    }));
                                }

                                num++;
                                
                                if (num > 5) {
                                    num = 0;
                                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                                        Console.Write("\n");
                                }

                                for (int i = 0; i < current.EnemyList.Count; i++) {
                                    EnemyObj current2 = current.EnemyList[i];
                                    if (current2.IsProcedural) {
                                        list.Add(current2.Type);
                                        list2.Add((byte)current2.Difficulty);
                                    }
                                }

                            }

                        }

                        int count = list.Count;
                        binaryWriter.Write(count);

                        for (int index = 0; index < list.Count; index++) {
                            byte current3 = list[index];
                            binaryWriter.Write(current3);
                        }

                        using (List<byte>.Enumerator enumerator4 = list2.GetEnumerator()) {
                            
                            while (enumerator4.MoveNext()) {
                                byte current4 = enumerator4.Current;
                                binaryWriter.Write(current4);
                            }

                            goto IL_34E;

                        }

                    }

                    Console.WriteLine("WARNING: Attempting to save LEVEL screen but it was null. Make sure it exists in the screen manager before saving it.");
                    
                IL_34E:
                
                    if (saveBackup) {
                        FileStream fileStream = stream as FileStream;
                        if (fileStream != null)
                            fileStream.Flush(true);
                    }

                    binaryWriter.Close();

                }

                stream.Close();

            }

        }

        private void SaveMapData(bool saveBackup) {
            
            string text = _fileNameMapData;
            
            if (saveBackup)
                text = text.Insert(0, "AutoSave_");
            
            text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

            using (Stream stream = _modStorageContainer.CreateFile(text)) {
                
                using (BinaryWriter binaryWriter = new BinaryWriter(stream)) {

                    ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
                    
                    if (levelScreen != null) {

                        List<RoomObj> mapRoomsAdded = levelScreen.MapRoomsAdded;
                        List<bool> list = new List<bool>();
                        List<bool> list2 = new List<bool>();
                        List<int> list3 = new List<int>();
                        List<bool> list4 = new List<bool>();
                        List<byte> list5 = new List<byte>();
                        List<bool> list6 = new List<bool>();
                        List<bool> list7 = new List<bool>();
                        List<bool> list8 = new List<bool>();

                        for (int index = 0; index < levelScreen.RoomList.Count; index++) {
                            
                            RoomObj current = levelScreen.RoomList[index];
                            
                            if (mapRoomsAdded.Contains(current))
                                list.Add(true);
                            else
                                list.Add(false);
                            
                            BonusRoomObj bonusRoomObj = current as BonusRoomObj;
                            
                            if (bonusRoomObj != null) {
                                if (bonusRoomObj.RoomCompleted)
                                    list2.Add(true);
                                else
                                    list2.Add(false);
                                list3.Add(bonusRoomObj.ID);
                            }

                            if (current.Name != "Boss" && current.Name != "ChallengeBoss") {
                                foreach (EnemyObj current2 in current.EnemyList) {
                                    if (current2.IsKilled)
                                        list7.Add(true);
                                    else
                                        list7.Add(false);
                                }
                            }

                            if (current.Name != "Bonus" && current.Name != "Boss" && current.Name != "Compass" && current.Name != "ChallengeBoss") {
                                
                                for (int i = 0; i < current.GameObjList.Count; i++) {
                                    
                                    GameObj current3 = current.GameObjList[i];
                                    BreakableObj breakableObj = current3 as BreakableObj;
                                    
                                    if (breakableObj != null) {
                                        if (breakableObj.Broken)
                                            list8.Add(true);
                                        else
                                            list8.Add(false);
                                    }

                                    ChestObj chestObj = current3 as ChestObj;
                                    
                                    if (chestObj != null) {
                                        
                                        list5.Add(chestObj.ChestType);
                                        
                                        if (chestObj.IsOpen)
                                            list4.Add(true);
                                        else
                                            list4.Add(false);
                                        
                                        FairyChestObj fairyChestObj = chestObj as FairyChestObj;
                                        
                                        if (fairyChestObj != null) {
                                            if (fairyChestObj.State == 2)
                                                list6.Add(true);
                                            else
                                                list6.Add(false);
                                        }

                                    }

                                }

                            }

                        }

                        binaryWriter.Write(list.Count);

                        for (int index = 0; index < list.Count; index++) {
                            bool current4 = list[index];
                            binaryWriter.Write(current4);
                        }

                        binaryWriter.Write(list2.Count);

                        for (int index = 0; index < list2.Count; index++) {
                            bool current5 = list2[index];
                            binaryWriter.Write(current5);
                        }

                        for (int index = 0; index < list3.Count; index++) {
                            int current6 = list3[index];
                            binaryWriter.Write(current6);
                        }

                        binaryWriter.Write(list5.Count);

                        for (int index = 0; index < list5.Count; index++) {
                            byte current7 = list5[index];
                            binaryWriter.Write(current7);
                        }

                        binaryWriter.Write(list4.Count);

                        for (int index = 0; index < list4.Count; index++) {
                            bool current8 = list4[index];
                            binaryWriter.Write(current8);
                        }

                        binaryWriter.Write(list6.Count);

                        for (int index = 0; index < list6.Count; index++) {
                            bool current9 = list6[index];
                            binaryWriter.Write(current9);
                        }

                        binaryWriter.Write(list7.Count);

                        for (int index = 0; index < list7.Count; index++) {
                            bool current10 = list7[index];
                            binaryWriter.Write(current10);
                        }

                        binaryWriter.Write(list8.Count);
                        
                        using (List<bool>.Enumerator enumerator11 = list8.GetEnumerator()) {
                            
                            while (enumerator11.MoveNext()) {
                                bool current11 = enumerator11.Current;
                                binaryWriter.Write(current11);
                            }

                            goto IL_4C3;

                        }

                    }

                    Console.WriteLine("WARNING: Attempting to save level screen MAP data but level was null. Make sure it exists in the screen manager before saving it.");
                    
                IL_4C3:
                
                    if (saveBackup) {
                        FileStream fileStream = stream as FileStream;
                        if (fileStream != null)
                            fileStream.Flush(true);
                    }

                    binaryWriter.Close();

                }

                stream.Close();

            }

        }

        private void SaveLineageData(bool saveBackup) {
            
            string text = _fileNameLineage;
            
            if (saveBackup)
                text = text.Insert(0, "AutoSave_");
            
            text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

            using (Stream stream = _modStorageContainer.CreateFile(text)) {

                using (BinaryWriter binaryWriter = new BinaryWriter(stream)) {

                    Console.WriteLine("///// PLAYER LINEAGE DATA - BEGIN SAVING /////");
                    List<PlayerLineageData> currentBranches = Game.PlayerStats.CurrentBranches;
                    int num = 0;
                    
                    if (currentBranches != null) {

                        num = currentBranches.Count;
                        binaryWriter.Write(num);
                        
                        for (int i = 0; i < num; i++) {
                            binaryWriter.Write(currentBranches[i].Name);
                            binaryWriter.Write(currentBranches[i].Spell);
                            binaryWriter.Write(currentBranches[i].Class);
                            binaryWriter.Write(currentBranches[i].HeadPiece);
                            binaryWriter.Write(currentBranches[i].ChestPiece);
                            binaryWriter.Write(currentBranches[i].ShoulderPiece);
                            binaryWriter.Write(currentBranches[i].Age);
                            binaryWriter.Write(currentBranches[i].ChildAge);
                            binaryWriter.Write((byte)currentBranches[i].Traits.X);
                            binaryWriter.Write((byte)currentBranches[i].Traits.Y);
                            binaryWriter.Write(currentBranches[i].IsFemale);
                        }

                    }
                    else
                        binaryWriter.Write(num);

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        
                        Console.WriteLine("Saving Current Branch Lineage Data");
                        
                        for (int j = 0; j < num; j++) {
                            Console.WriteLine("Player Name: " + currentBranches[j].Name);
                            Console.WriteLine("Spell: " + currentBranches[j].Name);
                            Console.WriteLine("Class: " + currentBranches[j].Name);
                            Console.WriteLine("Head Piece: " + currentBranches[j].HeadPiece);
                            Console.WriteLine("Chest Piece: " + currentBranches[j].ChestPiece);
                            Console.WriteLine("Shoulder Piece: " + currentBranches[j].ShoulderPiece);
                            Console.WriteLine("Player Age: " + currentBranches[j].Age);
                            Console.WriteLine("Player Child Age: " + currentBranches[j].ChildAge);
                            Console.WriteLine(string.Concat(new object[] {
                                "Traits: ",
                                currentBranches[j].Traits.X,
                                ", ",
                                currentBranches[j].Traits.Y
                            }));
                            Console.WriteLine("Is Female: " + currentBranches[j].IsFemale);
                        }

                    }

                    List<FamilyTreeNode> familyTreeArray = Game.PlayerStats.FamilyTreeArray;
                    int num2 = 0;
                    
                    if (familyTreeArray != null) {
                        
                        num2 = familyTreeArray.Count;
                        binaryWriter.Write(num2);
                        
                        for (int k = 0; k < num2; k++) {
                            binaryWriter.Write(familyTreeArray[k].Name);
                            binaryWriter.Write(familyTreeArray[k].Age);
                            binaryWriter.Write(familyTreeArray[k].Class);
                            binaryWriter.Write(familyTreeArray[k].HeadPiece);
                            binaryWriter.Write(familyTreeArray[k].ChestPiece);
                            binaryWriter.Write(familyTreeArray[k].ShoulderPiece);
                            binaryWriter.Write(familyTreeArray[k].NumEnemiesBeaten);
                            binaryWriter.Write(familyTreeArray[k].BeatenABoss);
                            binaryWriter.Write((byte)familyTreeArray[k].Traits.X);
                            binaryWriter.Write((byte)familyTreeArray[k].Traits.Y);
                            binaryWriter.Write(familyTreeArray[k].IsFemale);
                        }

                    }
                    else
                        binaryWriter.Write(num2);

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        
                        Console.WriteLine("Saving Family Tree Data");
                        Console.WriteLine("Number of Branches: " + num2);
                        
                        for (int l = 0; l < num2; l++) {
                            Console.WriteLine("/// Saving branch");
                            Console.WriteLine("Name: " + familyTreeArray[l].Name);
                            Console.WriteLine("Age: " + familyTreeArray[l].Age);
                            Console.WriteLine("Class: " + familyTreeArray[l].Class);
                            Console.WriteLine("Head Piece: " + familyTreeArray[l].HeadPiece);
                            Console.WriteLine("Chest Piece: " + familyTreeArray[l].ChestPiece);
                            Console.WriteLine("Shoulder Piece: " + familyTreeArray[l].ShoulderPiece);
                            Console.WriteLine("Number of Enemies Beaten: " + familyTreeArray[l].NumEnemiesBeaten);
                            Console.WriteLine("Beaten a Boss: " + familyTreeArray[l].BeatenABoss);
                            Console.WriteLine(string.Concat(new object[] {
                                "Traits: ",
                                familyTreeArray[l].Traits.X,
                                ", ",
                                familyTreeArray[l].Traits.Y
                            }));
                            Console.WriteLine("Is Female: " + familyTreeArray[l].IsFemale);
                        }

                    }

                    Console.WriteLine("///// PLAYER LINEAGE DATA - SAVE COMPLETE /////");
                    
                    if (saveBackup) {
                        FileStream fileStream = stream as FileStream;
                        if (fileStream != null)
                            fileStream.Flush(true);
                    }

                    binaryWriter.Close();

                }

                stream.Close();

            }

        }

        private void LoadData(SaveType loadType, ProceduralLevelScreen level) {
            
            if (FileExists(loadType)) {

                switch (loadType) {

                    case SaveType.PlayerData:
                        LoadPlayerData();
                        break;
                    case SaveType.UpgradeData:
                        LoadUpgradeData();
                        break;
                    case SaveType.Map:
                        Console.WriteLine("Cannot load Map directly from LoadData. Call LoadMap() instead.");
                        break;
                    case SaveType.MapData:
                        if (level != null)
                            LoadMapData(level);
                        else
                            Console.WriteLine("Could not load Map data. Level was null.");
                        break;
                    case SaveType.Lineage:
                        LoadLineageData();
                        break;

                }

                Console.WriteLine("\nData of type " + loadType + " Loaded.");
                return;

            }

            Console.WriteLine("Could not load data of type " + loadType + " because data did not exist.");

        }

        private void LoadPlayerData() {

            using (Stream stream = _modStorageContainer.OpenFile(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNamePlayer
            }), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                
                using (BinaryReader binaryReader = new BinaryReader(stream)) {
                    
                    Game.PlayerStats.Gold = binaryReader.ReadInt32();
                    Game.PlayerStats.CurrentHealth = binaryReader.ReadInt32();
                    Game.PlayerStats.CurrentMana = binaryReader.ReadInt32();
                    Game.PlayerStats.Age = binaryReader.ReadByte();
                    Game.PlayerStats.ChildAge = binaryReader.ReadByte();
                    Game.PlayerStats.Spell = binaryReader.ReadByte();
                    Game.PlayerStats.Class = binaryReader.ReadByte();
                    Game.PlayerStats.SpecialItem = binaryReader.ReadByte();
                    Game.PlayerStats.Traits = new Vector2(binaryReader.ReadByte(), binaryReader.ReadByte());
                    Game.PlayerStats.PlayerName = binaryReader.ReadString();
                    Game.PlayerStats.HeadPiece = binaryReader.ReadByte();
                    Game.PlayerStats.ShoulderPiece = binaryReader.ReadByte();
                    Game.PlayerStats.ChestPiece = binaryReader.ReadByte();
                    
                    if (Game.PlayerStats.HeadPiece == 0 || Game.PlayerStats.ShoulderPiece == 0 || Game.PlayerStats.ChestPiece == 0)
                        throw new Exception("Corrupted Save File");
                    
                    Game.PlayerStats.DiaryEntry = binaryReader.ReadByte();
                    Game.PlayerStats.BonusHealth = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusStrength = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusMana = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusDefense = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusWeight = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusMagic = binaryReader.ReadInt32();
                    Game.PlayerStats.LichHealth = binaryReader.ReadInt32();
                    Game.PlayerStats.LichMana = binaryReader.ReadInt32();
                    Game.PlayerStats.LichHealthMod = binaryReader.ReadSingle();
                    Game.PlayerStats.NewBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.EyeballBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.FairyBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.FireballBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.BlobBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.LastbossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.TimesCastleBeaten = binaryReader.ReadInt32();
                    Game.PlayerStats.NumEnemiesBeaten = binaryReader.ReadInt32();
                    Game.PlayerStats.TutorialComplete = binaryReader.ReadBoolean();
                    Game.PlayerStats.CharacterFound = binaryReader.ReadBoolean();
                    Game.PlayerStats.LoadStartingRoom = binaryReader.ReadBoolean();
                    Game.PlayerStats.LockCastle = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokeToBlacksmith = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokeToEnchantress = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokeToArchitect = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokeToTollCollector = binaryReader.ReadBoolean();
                    Game.PlayerStats.IsDead = binaryReader.ReadBoolean();
                    Game.PlayerStats.FinalDoorOpened = binaryReader.ReadBoolean();
                    Game.PlayerStats.RerolledChildren = binaryReader.ReadBoolean();
                    Game.PlayerStats.IsFemale = binaryReader.ReadBoolean();
                    Game.PlayerStats.TimesDead = binaryReader.ReadInt32();
                    Game.PlayerStats.HasArchitectFee = binaryReader.ReadBoolean();
                    Game.PlayerStats.ReadLastDiary = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokenToLastBoss = binaryReader.ReadBoolean();
                    Game.PlayerStats.HardcoreMode = binaryReader.ReadBoolean();
                    Game.PlayerStats.TotalHoursPlayed = binaryReader.ReadSingle();
                    byte b = binaryReader.ReadByte();
                    byte b2 = binaryReader.ReadByte();
                    byte b3 = binaryReader.ReadByte();
                    Game.PlayerStats.WizardSpellList = new Vector3(b, b2, b3);
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {

                        Console.WriteLine("\nLoading Player Stats");
                        Console.WriteLine("Gold: " + Game.PlayerStats.Gold);
                        Console.WriteLine("Current Health: " + Game.PlayerStats.CurrentHealth);
                        Console.WriteLine("Current Mana: " + Game.PlayerStats.CurrentMana);
                        Console.WriteLine("Age: " + Game.PlayerStats.Age);
                        Console.WriteLine("Child Age: " + Game.PlayerStats.ChildAge);
                        Console.WriteLine("Spell: " + Game.PlayerStats.Spell);
                        Console.WriteLine("Class: " + Game.PlayerStats.Class);
                        Console.WriteLine("Special Item: " + Game.PlayerStats.SpecialItem);
                        Console.WriteLine(string.Concat(new object[] {
                            "Traits: ",
                            Game.PlayerStats.Traits.X,
                            ", ",
                            Game.PlayerStats.Traits.Y
                        }));
                        Console.WriteLine("Name: " + Game.PlayerStats.PlayerName);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Head Piece: " + Game.PlayerStats.HeadPiece);
                        Console.WriteLine("Shoulder Piece: " + Game.PlayerStats.ShoulderPiece);
                        Console.WriteLine("Chest Piece: " + Game.PlayerStats.ChestPiece);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Diary Entry: " + Game.PlayerStats.DiaryEntry);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Bonus Health: " + Game.PlayerStats.BonusHealth);
                        Console.WriteLine("Bonus Strength: " + Game.PlayerStats.BonusStrength);
                        Console.WriteLine("Bonus Mana: " + Game.PlayerStats.BonusMana);
                        Console.WriteLine("Bonus Armor: " + Game.PlayerStats.BonusDefense);
                        Console.WriteLine("Bonus Weight: " + Game.PlayerStats.BonusWeight);
                        Console.WriteLine("Bonus Magic: " + Game.PlayerStats.BonusMagic);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Lich Health: " + Game.PlayerStats.LichHealth);
                        Console.WriteLine("Lich Mana: " + Game.PlayerStats.LichMana);
                        Console.WriteLine("Lich Health Mod: " + Game.PlayerStats.LichHealthMod);
                        Console.WriteLine("---------------");
                        Console.WriteLine("New Boss Beaten: " + Game.PlayerStats.NewBossBeaten);
                        Console.WriteLine("Eyeball Boss Beaten: " + Game.PlayerStats.EyeballBossBeaten);
                        Console.WriteLine("Fairy Boss Beaten: " + Game.PlayerStats.FairyBossBeaten);
                        Console.WriteLine("Fireball Boss Beaten: " + Game.PlayerStats.FireballBossBeaten);
                        Console.WriteLine("Blob Boss Beaten: " + Game.PlayerStats.BlobBossBeaten);
                        Console.WriteLine("Last Boss Beaten: " + Game.PlayerStats.LastbossBeaten);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Times Castle Beaten: " + Game.PlayerStats.TimesCastleBeaten);
                        Console.WriteLine("Number of Enemies Beaten: " + Game.PlayerStats.NumEnemiesBeaten);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Tutorial Complete: " + Game.PlayerStats.TutorialComplete);
                        Console.WriteLine("Character Found: " + Game.PlayerStats.CharacterFound);
                        Console.WriteLine("Load Starting Room: " + Game.PlayerStats.LoadStartingRoom);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Castle Locked: " + Game.PlayerStats.LockCastle);
                        Console.WriteLine("Spoke to Blacksmith: " + Game.PlayerStats.SpokeToBlacksmith);
                        Console.WriteLine("Spoke to Enchantress: " + Game.PlayerStats.SpokeToEnchantress);
                        Console.WriteLine("Spoke to Architect: " + Game.PlayerStats.SpokeToArchitect);
                        Console.WriteLine("Spoke to Toll Collector: " + Game.PlayerStats.SpokeToTollCollector);
                        Console.WriteLine("Player Is Dead: " + Game.PlayerStats.IsDead);
                        Console.WriteLine("Final Door Opened: " + Game.PlayerStats.FinalDoorOpened);
                        Console.WriteLine("Rerolled Children: " + Game.PlayerStats.RerolledChildren);
                        Console.WriteLine("Is Female: " + Game.PlayerStats.IsFemale);
                        Console.WriteLine("Times Dead: " + Game.PlayerStats.TimesDead);
                        Console.WriteLine("Has Architect Fee: " + Game.PlayerStats.HasArchitectFee);
                        Console.WriteLine("Player read last diary: " + Game.PlayerStats.ReadLastDiary);
                        Console.WriteLine("Player has spoken to last boss: " + Game.PlayerStats.SpokenToLastBoss);
                        Console.WriteLine("Is Hardcore mode: " + Game.PlayerStats.HardcoreMode);
                        Console.WriteLine("Total Hours Played " + Game.PlayerStats.TotalHoursPlayed);
                        Console.WriteLine("Wizard Spell 1: " + Game.PlayerStats.WizardSpellList.X);
                        Console.WriteLine("Wizard Spell 2: " + Game.PlayerStats.WizardSpellList.Y);
                        Console.WriteLine("Wizard Spell 3: " + Game.PlayerStats.WizardSpellList.Z);

                    }

                    Console.WriteLine("///// ENEMY LIST DATA - BEGIN LOADING /////");
                    
                    for (int i = 0; i < 34; i++) {
                        Vector4 value = new Vector4(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
                        Game.PlayerStats.EnemiesKilledList[i] = value;
                    }

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        
                        Console.WriteLine("Loading Enemy List Data");
                        int num = 0;

                        for (int index = 0; index < Game.PlayerStats.EnemiesKilledList.Count; index++) {
                            Vector4 current = Game.PlayerStats.EnemiesKilledList[index];
                            Console.WriteLine(string.Concat(new object[] {
                                "Enemy Type: ",
                                num,
                                ", Difficulty: Basic, Killed: ",
                                current.X
                            }));
                            Console.WriteLine(string.Concat(new object[] {
                                "Enemy Type: ",
                                num,
                                ", Difficulty: Advanced, Killed: ",
                                current.Y
                            }));
                            Console.WriteLine(string.Concat(new object[] {
                                "Enemy Type: ",
                                num,
                                ", Difficulty: Expert, Killed: ",
                                current.Z
                            }));
                            Console.WriteLine(string.Concat(new object[] {
                                "Enemy Type: ",
                                num,
                                ", Difficulty: Miniboss, Killed: ",
                                current.W
                            }));
                            num++;
                        }

                    }

                    int num2 = binaryReader.ReadInt32();
                    
                    for (int j = 0; j < num2; j++) {
                        Vector2 item = new Vector2(binaryReader.ReadInt32(), binaryReader.ReadInt32());
                        Game.PlayerStats.EnemiesKilledInRun.Add(item);
                    }

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        Console.WriteLine("Loading num enemies killed");
                        Console.WriteLine("Number of enemies killed in run: " + num2);
                        for (int index = 0; index < Game.PlayerStats.EnemiesKilledInRun.Count; index++) {
                            Vector2 current2 = Game.PlayerStats.EnemiesKilledInRun[index];
                            Console.WriteLine("Enemy Room Index: " + current2.X);
                            Console.WriteLine("Enemy Index in EnemyList: " + current2.Y);
                        }
                    }

                    Console.WriteLine("///// ENEMY LIST DATA - LOAD COMPLETE /////");
                    
                    if (binaryReader.PeekChar() != -1) {
                        
                        Console.WriteLine("///// DLC DATA FOUND - BEGIN LOADING /////");
                        Game.PlayerStats.ChallengeEyeballUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeSkullUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeFireballUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeBlobUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeLastBossUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeEyeballBeaten = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeSkullBeaten = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeFireballBeaten = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeBlobBeaten = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeLastBossBeaten = binaryReader.ReadBoolean();
                        
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                            Console.WriteLine("Eyeball Challenge Unlocked: " + Game.PlayerStats.ChallengeEyeballUnlocked);
                            Console.WriteLine("Skull Challenge Unlocked: " + Game.PlayerStats.ChallengeSkullUnlocked);
                            Console.WriteLine("Fireball Challenge Unlocked: " + Game.PlayerStats.ChallengeFireballUnlocked);
                            Console.WriteLine("Blob Challenge Unlocked: " + Game.PlayerStats.ChallengeBlobUnlocked);
                            Console.WriteLine("Last Boss Challenge Unlocked: " + Game.PlayerStats.ChallengeLastBossUnlocked);
                            Console.WriteLine("Eyeball Challenge Beaten: " + Game.PlayerStats.ChallengeEyeballBeaten);
                            Console.WriteLine("Skull Challenge Beaten: " + Game.PlayerStats.ChallengeSkullBeaten);
                            Console.WriteLine("Fireball Challenge Beaten: " + Game.PlayerStats.ChallengeFireballBeaten);
                            Console.WriteLine("Blob Challenge Beaten: " + Game.PlayerStats.ChallengeBlobBeaten);
                            Console.WriteLine("Last Boss Challenge Beaten: " + Game.PlayerStats.ChallengeLastBossBeaten);
                        }

                        Console.WriteLine("///// DLC DATA - LOADING COMPLETE /////");

                    }
                    else
                        Console.WriteLine("///// NO DLC DATA FOUND - SKIPPED LOADING /////");
                    
                    binaryReader.Close();

                }

                stream.Close();

            }

        }

        private void LoadUpgradeData() {

            using (Stream stream = _modStorageContainer.OpenFile(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNameUpgrades
            }), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                
                using (BinaryReader binaryReader = new BinaryReader(stream)) {
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                        Console.WriteLine("\nLoading Equipment States");
                        Console.WriteLine("\nLoading Standard Blueprints");
                    }

                    List<byte[]> getBlueprintArray = Game.PlayerStats.GetBlueprintArray;
                    
                    for (int i = 0; i < 5; i++) {
                        
                        for (int j = 0; j < 15; j++) {
                            getBlueprintArray[i][j] = binaryReader.ReadByte();
                            if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                                Console.Write(" " + getBlueprintArray[i][j]);
                        }

                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write("\n");

                    }

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nLoading Ability Blueprints");
                    
                    List<byte[]> getRuneArray = Game.PlayerStats.GetRuneArray;
                    
                    for (int k = 0; k < 5; k++) {
                        
                        for (int l = 0; l < 11; l++) {
                            getRuneArray[k][l] = binaryReader.ReadByte();
                            if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                                Console.Write(" " + getRuneArray[k][l]);
                        }
                        
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write("\n");

                    }

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nLoading Equipped Standard Items");
                    
                    sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
                    
                    for (int m = 0; m < 5; m++) {
                        getEquippedArray[m] = binaryReader.ReadSByte();
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write(" " + getEquippedArray[m]);
                    }

                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nLoading Equipped Abilities");
                    
                    sbyte[] getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
                    
                    for (int n = 0; n < 5; n++) {
                        getEquippedRuneArray[n] = binaryReader.ReadSByte();
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write(" " + getEquippedRuneArray[n]);
                    }

                    SkillObj[] skillArray = SkillSystem.GetSkillArray();
                    
                    if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                        Console.WriteLine("\nLoading Traits");
                    
                    SkillSystem.ResetAllTraits();
                    Game.PlayerStats.CurrentLevel = 0;
                    
                    for (int num = 0; num < 32; num++) {
                        int num2 = binaryReader.ReadInt32();
                        for (int num3 = 0; num3 < num2; num3++)
                            SkillSystem.LevelUpTrait(skillArray[num], false);
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
                            Console.Write(" " + skillArray[num].CurrentLevel);
                    }

                    binaryReader.Close();
                    Game.ScreenManager.Player.UpdateEquipmentColours();

                }

                stream.Close();
                
                if (Game.PlayerStats.GetNumberOfEquippedRunes(0) > 0 && SkillSystem.GetSkill(SkillType.Enchanter).CurrentLevel < 1 && LevelEV.CREATE_RETAIL_VERSION)
                    throw new Exception("Corrupted Save file");
                
                bool flag = false;
                List<FamilyTreeNode> familyTreeArray = Game.PlayerStats.FamilyTreeArray;

                for (int index = 0; index < familyTreeArray.Count; index++) {
                    FamilyTreeNode current = familyTreeArray[index];
                    if (current.Class > 3) {
                        flag = true;
                        break;
                    }
                }

                if (flag && SkillSystem.GetSkill(SkillType.Smithy).CurrentLevel < 1 && LevelEV.CREATE_RETAIL_VERSION)
                    throw new Exception("Corrupted Save file");

            }

        }

        public ProceduralLevelScreen LoadMap() {

            GetStorageContainer();
            ProceduralLevelScreen proceduralLevelScreen;

            using (Stream stream = _modStorageContainer.OpenFile(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNameMap
            }), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                
                using (BinaryReader binaryReader = new BinaryReader(stream)) {
                    
                    int num = binaryReader.ReadInt32();
                    Vector4[] array = new Vector4[num];
                    Vector3[] array2 = new Vector3[num];
                    
                    for (int i = 0; i < num; i++) {
                        array[i].W = binaryReader.ReadInt32();
                        array[i].X = binaryReader.ReadByte();
                        array[i].Y = binaryReader.ReadInt32();
                        array[i].Z = binaryReader.ReadInt32();
                        array2[i].X = binaryReader.ReadByte();
                        array2[i].Y = binaryReader.ReadByte();
                        array2[i].Z = binaryReader.ReadByte();
                    }

                    proceduralLevelScreen = LevelBuilder2.CreateLevel(array, array2);
                    int num2 = binaryReader.ReadInt32();
                    List<byte> list = new List<byte>();
                    
                    for (int j = 0; j < num2; j++)
                        list.Add(binaryReader.ReadByte());
                    
                    List<byte> list2 = new List<byte>();
                    
                    for (int k = 0; k < num2; k++)
                        list2.Add(binaryReader.ReadByte());
                    
                    LevelBuilder2.OverrideProceduralEnemies(proceduralLevelScreen, list.ToArray(), list2.ToArray());
                    binaryReader.Close();

                }

                stream.Close();

            }

            _modStorageContainer.Dispose();
            return proceduralLevelScreen;

        }

        private void LoadMapData(ProceduralLevelScreen createdLevel) {

            using (Stream stream = _modStorageContainer.OpenFile(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNameMapData
            }), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                
                using (BinaryReader binaryReader = new BinaryReader(stream)) {
                    
                    int num = binaryReader.ReadInt32();
                    List<bool> list = new List<bool>();
                    
                    for (int i = 0; i < num; i++)
                        list.Add(binaryReader.ReadBoolean());
                    
                    int num2 = binaryReader.ReadInt32();
                    List<bool> list2 = new List<bool>();
                    
                    for (int j = 0; j < num2; j++)
                        list2.Add(binaryReader.ReadBoolean());
                    
                    List<int> list3 = new List<int>();
                    
                    for (int k = 0; k < num2; k++)
                        list3.Add(binaryReader.ReadInt32());
                    
                    int num3 = binaryReader.ReadInt32();
                    List<byte> list4 = new List<byte>();
                    
                    for (int l = 0; l < num3; l++)
                        list4.Add(binaryReader.ReadByte());
                    
                    num3 = binaryReader.ReadInt32();
                    List<bool> list5 = new List<bool>();
                    
                    for (int m = 0; m < num3; m++)
                        list5.Add(binaryReader.ReadBoolean());
                    
                    num3 = binaryReader.ReadInt32();
                    List<bool> list6 = new List<bool>();
                    
                    for (int n = 0; n < num3; n++)
                        list6.Add(binaryReader.ReadBoolean());
                    
                    int num4 = binaryReader.ReadInt32();
                    List<bool> list7 = new List<bool>();
                    
                    for (int num5 = 0; num5 < num4; num5++)
                        list7.Add(binaryReader.ReadBoolean());
                    
                    int num6 = binaryReader.ReadInt32();
                    List<bool> list8 = new List<bool>();
                    
                    for (int num7 = 0; num7 < num6; num7++)
                        list8.Add(binaryReader.ReadBoolean());
                    
                    int num8 = 0;
                    int num9 = 0;
                    int num10 = 0;
                    int num11 = 0;
                    int num12 = 0;
                    int num13 = 0;

                    for (int index = 0; index < createdLevel.RoomList.Count; index++) {
                        
                        RoomObj current = createdLevel.RoomList[index];
                        
                        if (num2 > 0) {
                            BonusRoomObj bonusRoomObj = current as BonusRoomObj;
                            if (bonusRoomObj != null) {
                                bool flag = list2[num8];
                                int iD = list3[num8];
                                num8++;
                                if (flag)
                                    bonusRoomObj.RoomCompleted = true;
                                bonusRoomObj.ID = iD;
                            }
                        }

                        if (num4 > 0 && !Game.PlayerStats.LockCastle && current.Name != "Boss" && current.Name != "ChallengeBoss") {
                            for (int i = 0; i < current.EnemyList.Count; i++) {
                                EnemyObj current2 = current.EnemyList[i];
                                bool flag2 = list7[num12];
                                num12++;
                                if (flag2)
                                    current2.KillSilently();
                            }
                        }

                        if (current.Name != "Bonus" && current.Name != "Boss" && current.Name != "Compass" && current.Name != "ChallengeBoss") {
                            
                            for (int i = 0; i < current.GameObjList.Count; i++) {
                                
                                GameObj current3 = current.GameObjList[i];
                                
                                if (!Game.PlayerStats.LockCastle && num6 > 0) {
                                    
                                    BreakableObj breakableObj = current3 as BreakableObj;
                                    
                                    if (breakableObj != null) {
                                        bool flag3 = list8[num13];
                                        num13++;
                                        if (flag3)
                                            breakableObj.ForceBreak();
                                    }

                                }

                                ChestObj chestObj = current3 as ChestObj;
                                
                                if (chestObj != null) {
                                    
                                    chestObj.IsProcedural = false;
                                    byte chestType = list4[num9];
                                    num9++;
                                    chestObj.ChestType = chestType;
                                    bool flag4 = list5[num10];
                                    num10++;
                                    
                                    if (flag4)
                                        chestObj.ForceOpen();
                                    
                                    if (!Game.PlayerStats.LockCastle) {
                                        
                                        FairyChestObj fairyChestObj = chestObj as FairyChestObj;
                                        
                                        if (fairyChestObj != null) {
                                            bool flag5 = list6[num11];
                                            num11++;
                                            if (flag5)
                                                fairyChestObj.SetChestFailed(true);
                                        }

                                    }

                                }

                            }

                        }

                    }

                    if (num > 0) {

                        List<RoomObj> list9 = new List<RoomObj>();
                        int count = list.Count;
                        
                        for (int num14 = 0; num14 < count; num14++) {
                            if (list[num14])
                                list9.Add(createdLevel.RoomList[num14]);
                        }

                        createdLevel.MapRoomsUnveiled = list9;

                    }

                    binaryReader.Close();

                }

                stream.Close();

            }

        }

        private void LoadLineageData() {

            using (Stream stream = _modStorageContainer.OpenFile(string.Concat(new object[] {
                "Profile",
                Game.GameConfig.ProfileSlot,
                "/",
                _fileNameLineage
            }), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                
                using (BinaryReader binaryReader = new BinaryReader(stream)) {
                    
                    Console.WriteLine("///// PLAYER LINEAGE DATA - BEGIN LOADING /////");
                    List<PlayerLineageData> list = new List<PlayerLineageData>();
                    int num = binaryReader.ReadInt32();
                    
                    for (int i = 0; i < num; i++) {
                        list.Add(new PlayerLineageData {
                            Name = binaryReader.ReadString(),
                            Spell = binaryReader.ReadByte(),
                            Class = binaryReader.ReadByte(),
                            HeadPiece = binaryReader.ReadByte(),
                            ChestPiece = binaryReader.ReadByte(),
                            ShoulderPiece = binaryReader.ReadByte(),
                            Age = binaryReader.ReadByte(),
                            ChildAge = binaryReader.ReadByte(),
                            Traits = new Vector2(binaryReader.ReadByte(), binaryReader.ReadByte()),
                            IsFemale = binaryReader.ReadBoolean()
                        });
                    }

                    if (list.Count > 0) {
                        
                        Game.PlayerStats.CurrentBranches = list;
                        
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                            
                            Console.WriteLine("Loading Current Branch Lineage Data");
                            List<PlayerLineageData> currentBranches = Game.PlayerStats.CurrentBranches;
                            
                            for (int j = 0; j < num; j++) {
                                Console.WriteLine("Player Name: " + currentBranches[j].Name);
                                Console.WriteLine("Spell: " + currentBranches[j].Name);
                                Console.WriteLine("Class: " + currentBranches[j].Name);
                                Console.WriteLine("Head Piece: " + currentBranches[j].HeadPiece);
                                Console.WriteLine("Chest Piece: " + currentBranches[j].ChestPiece);
                                Console.WriteLine("Shoulder Piece: " + currentBranches[j].ShoulderPiece);
                                Console.WriteLine("Player Age: " + currentBranches[j].Age);
                                Console.WriteLine("Player Child Age: " + currentBranches[j].ChildAge);
                                Console.WriteLine(string.Concat(new object[] {
                                    "Traits: ",
                                    currentBranches[j].Traits.X,
                                    ", ",
                                    currentBranches[j].Traits.Y
                                }));
                                Console.WriteLine("Is Female: " + currentBranches[j].IsFemale);
                            }

                        }

                    }

                    List<FamilyTreeNode> list2 = new List<FamilyTreeNode>();
                    int num2 = binaryReader.ReadInt32();
                    
                    for (int k = 0; k < num2; k++) {
                        FamilyTreeNode item = default(FamilyTreeNode);
                        item.Name = binaryReader.ReadString();
                        item.Age = binaryReader.ReadByte();
                        item.Class = binaryReader.ReadByte();
                        item.HeadPiece = binaryReader.ReadByte();
                        item.ChestPiece = binaryReader.ReadByte();
                        item.ShoulderPiece = binaryReader.ReadByte();
                        item.NumEnemiesBeaten = binaryReader.ReadInt32();
                        item.BeatenABoss = binaryReader.ReadBoolean();
                        item.Traits.X = binaryReader.ReadByte();
                        item.Traits.Y = binaryReader.ReadByte();
                        item.IsFemale = binaryReader.ReadBoolean();
                        list2.Add(item);
                    }

                    if (list2.Count > 0) {
                        
                        Game.PlayerStats.FamilyTreeArray = list2;
                        
                        if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT) {
                            
                            List<FamilyTreeNode> familyTreeArray = Game.PlayerStats.FamilyTreeArray;
                            Console.WriteLine("Loading Family Tree Data");
                            Console.WriteLine("Number of Branches: " + num2);
                            
                            for (int l = 0; l < num2; l++) {
                                Console.WriteLine("/// Saving branch");
                                Console.WriteLine("Name: " + familyTreeArray[l].Name);
                                Console.WriteLine("Age: " + familyTreeArray[l].Age);
                                Console.WriteLine("Class: " + familyTreeArray[l].Class);
                                Console.WriteLine("Head Piece: " + familyTreeArray[l].HeadPiece);
                                Console.WriteLine("Chest Piece: " + familyTreeArray[l].ChestPiece);
                                Console.WriteLine("Shoulder Piece: " + familyTreeArray[l].ShoulderPiece);
                                Console.WriteLine("Number of Enemies Beaten: " + familyTreeArray[l].NumEnemiesBeaten);
                                Console.WriteLine("Beaten a Boss: " + familyTreeArray[l].BeatenABoss);
                                Console.WriteLine(string.Concat(new object[] {
                                    "Traits: ",
                                    familyTreeArray[l].Traits.X,
                                    ", ",
                                    familyTreeArray[l].Traits.Y
                                }));
                                Console.WriteLine("Is Female: " + familyTreeArray[l].IsFemale);
                            }

                        }

                    }

                    Console.WriteLine("///// PLAYER LINEAGE DATA - LOAD COMPLETE /////");
                    binaryReader.Close();

                }

                stream.Close();

            }

        }

        public bool FileExists(SaveType saveType) {
            
            bool flag = !(_modStorageContainer != null && !_modStorageContainer.IsDisposed);
            GetStorageContainer();
            bool result = false;
            
            switch (saveType) {

                case SaveType.PlayerData:
                    result = _modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNamePlayer
                    }));
                    break;

                case SaveType.UpgradeData:
                    result = _modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNameUpgrades
                    }));
                    break;

                case SaveType.Map:
                    result = _modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNameMap
                    }));
                    break;

                case SaveType.MapData:
                    result = _modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNameMapData
                    }));
                    break;

                case SaveType.Lineage:
                    result = _modStorageContainer.FileExists(string.Concat(new object[] {
                        "Profile",
                        Game.GameConfig.ProfileSlot,
                        "/",
                        _fileNameLineage
                    }));
                    break;

            }

            if (flag) {
                _modStorageContainer.Dispose();
                _modStorageContainer = null;
            }

            return result;

        }

        public StorageContainer GetContainer() {
            return _modStorageContainer;
        }

        public void GetSaveHeader(byte profile, out byte playerClass, out string playerName, out int playerLevel, out bool playerIsDead, out int castlesBeaten) {
            
            playerName = null;
            playerClass = 0;
            playerLevel = 0;
            playerIsDead = false;
            castlesBeaten = 0;
            GetStorageContainer();

            if (_modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                profile,
                "/",
                _fileNamePlayer
            }))) {

                using (Stream stream = _modStorageContainer.OpenFile(string.Concat(new object[] {
                    "Profile",
                    profile,
                    "/",
                    _fileNamePlayer
                }), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    
                    using (BinaryReader binaryReader = new BinaryReader(stream)) {
                        
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        playerClass = binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        playerName = binaryReader.ReadString();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadSingle();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        castlesBeaten = binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        playerIsDead = binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.Close();

                    }

                    stream.Close();

                }

            }

            if (_modStorageContainer.FileExists(string.Concat(new object[] {
                "Profile",
                profile,
                "/",
                _fileNameUpgrades
            }))) {

                using (Stream stream2 = _modStorageContainer.OpenFile(string.Concat(new object[] {
                    "Profile",
                    profile,
                    "/",
                    _fileNameUpgrades
                }), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    
                    using (BinaryReader binaryReader2 = new BinaryReader(stream2)) {
                        
                        for (int i = 0; i < 5; i++) {
                            for (int j = 0; j < 15; j++)
                                binaryReader2.ReadByte();
                        }

                        for (int k = 0; k < 5; k++) {
                            for (int l = 0; l < 11; l++)
                                binaryReader2.ReadByte();
                        }

                        for (int m = 0; m < 5; m++)
                            binaryReader2.ReadSByte();
                        
                        for (int n = 0; n < 5; n++)
                            binaryReader2.ReadSByte();
                        
                        int num = 0;
                        
                        for (int num2 = 0; num2 < 32; num2++) {
                            int num3 = binaryReader2.ReadInt32();
                            for (int num4 = 0; num4 < num3; num4++)
                                num++;
                        }

                        playerLevel = num;
                        binaryReader2.Close();

                    }

                    stream2.Close();

                }

            }

            _modStorageContainer.Dispose();
            _modStorageContainer = null;

        }
        
    }

}
