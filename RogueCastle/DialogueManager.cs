using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;


namespace RogueCastle {
    public class DialogueManager : IDisposable {
        private static bool m_isDisposed;
        private static Dictionary<string, Dictionary<string, DialogueObj>> m_languageArray;
        private static string m_currentLanguage;

        public static bool IsDisposed {
            get { return DialogueManager.m_isDisposed; }
        }

        #region IDisposable Members

        public void Dispose() {
            if (!DialogueManager.IsDisposed) {
                Console.WriteLine("Disposing Dialogue Manager");
                foreach (KeyValuePair<string, Dictionary<string, DialogueObj>> current in DialogueManager.m_languageArray) {
                    foreach (KeyValuePair<string, DialogueObj> current2 in current.Value)
                        current2.Value.Dispose();
                    current.Value.Clear();
                }
                DialogueManager.m_languageArray.Clear();
                DialogueManager.m_languageArray = null;
                DialogueManager.m_isDisposed = true;
            }
        }

        #endregion

        public static void Initialize() {
            DialogueManager.m_languageArray = new Dictionary<string, Dictionary<string, DialogueObj>>();
        }

        public static void LoadLanguageDocument(ContentManager content, string fileName) {
            DialogueManager.LoadLanguageDocument(content.RootDirectory + "\\" + fileName + ".txt");
        }

        public static void LoadLanguageDocument(string fullFilePath) {
            FileInfo fileInfo = new FileInfo(fullFilePath);
            using (StreamReader streamReader = fileInfo.OpenText())
                DialogueManager.ParseDocument(streamReader);
        }

        private static void ParseDocument(StreamReader reader) {
            int num = 0;
            string key = "";
            string item = "";
            string text = null;
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            bool flag = true;
            string text2;
            while ((text2 = reader.ReadLine()) != null) {
                if (text2 != "" && text2.IndexOf("//") != 0) {
                    string text3 = text2.Substring(text2.IndexOf(" ") + 1);
                    if (num == 0 && !text2.Contains("@language"))
                        throw new Exception("Cannot create text dictionary from file. Unspecified language type.");
                    if (text2.Contains("@language"))
                        DialogueManager.SetLanguage(text3);
                    else if (text2.Contains("@label")) {
                        if (!flag) {
                            if (text != null) {
                                list2.Add(text);
                                text = null;
                            }
                            DialogueManager.AddText(key, list.ToArray(), list2.ToArray());
                            flag = true;
                        }
                        if (flag) {
                            flag = false;
                            key = text3;
                            list.Clear();
                            list2.Clear();
                            text = null;
                            item = "";
                        }
                    }
                    else if (text2.Contains("@title"))
                        item = text3;
                    else if (text2.Contains("@text")) {
                        list.Add(item);
                        if (text != null)
                            list2.Add(text);
                        text = text3;
                    }
                    else
                        text = text + "\n" + text2;
                }
                num++;
            }
            if (text != null)
                list2.Add(text);
            DialogueManager.AddText(key, list.ToArray(), list2.ToArray());
        }

        public static void LoadLanguageBinFile(string filePath) {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                using (BinaryReader binaryReader = new BinaryReader(fileStream)) {
                    int num = 0;
                    string key = "";
                    string item = "";
                    string text = null;
                    List<string> list = new List<string>();
                    List<string> list2 = new List<string>();
                    bool flag = true;
                    while (true) {
                        string text2 = binaryReader.ReadString();
                        if (text2 != "" && text2.IndexOf("//") != 0) {
                            string text3 = text2.Substring(text2.IndexOf(" ") + 1);
                            if (num == 0 && !text2.Contains("@language"))
                                break;
                            if (text2.Contains("@language"))
                                DialogueManager.SetLanguage(text3);
                            else if (text2.Contains("@label")) {
                                if (!flag) {
                                    if (text != null) {
                                        list2.Add(text);
                                        text = null;
                                    }
                                    DialogueManager.AddText(key, list.ToArray(), list2.ToArray());
                                    flag = true;
                                }
                                if (flag) {
                                    flag = false;
                                    key = text3;
                                    list.Clear();
                                    list2.Clear();
                                    text = null;
                                    item = "";
                                }
                            }
                            else if (text2.Contains("@title"))
                                item = text3;
                            else if (text2.Contains("@text")) {
                                list.Add(item);
                                if (text != null)
                                    list2.Add(text);
                                text = text3;
                            }
                            else if (text2 != "eof")
                                text = text + "\n" + text2;
                        }
                        num++;
                        if (!(text2 != "eof"))
                            goto Block_18;
                    }
                    throw new Exception("Cannot create text dictionary from file. Unspecified language type.");
                    Block_18:
                    if (!flag) {
                        if (text != null)
                            list2.Add(text);
                        DialogueManager.AddText(key, list.ToArray(), list2.ToArray());
                    }
                }
            }
        }

        public static void SetLanguage(string language) {
            DialogueManager.m_currentLanguage = language;
            if (!DialogueManager.m_languageArray.ContainsKey(DialogueManager.m_currentLanguage)) {
                Console.WriteLine("Adding language dictionary for language: " + language);
                DialogueManager.m_languageArray.Add(language, new Dictionary<string, DialogueObj>());
            }
        }

        public static void AddText(string key, string[] speakers, string[] text) {
            if (DialogueManager.m_currentLanguage == null) {
                Console.WriteLine("Call SetLanguage() before attempting to add text to a specified language.");
                return;
            }
            if (DialogueManager.m_languageArray[DialogueManager.m_currentLanguage].ContainsKey(key)) {
                Console.WriteLine("Cannot add text. Text with title already specified.");
                return;
            }
            DialogueObj value = new DialogueObj(speakers, text);
            DialogueManager.m_languageArray[DialogueManager.m_currentLanguage].Add(key, value);
        }

        public static DialogueObj GetText(string key) {
            return DialogueManager.m_languageArray[DialogueManager.m_currentLanguage][key];
        }

        public static string GetCurrentLanguage() {
            return DialogueManager.m_currentLanguage;
        }
    }
}
