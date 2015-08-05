using System.Collections.Generic;
using System.Xml;


namespace RogueCastle {
    public class XMLCompiler {
        public static void CompileEnemies(List<EnemyEditorData> enemyDataList, string filePath) {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlWriter xmlWriter = XmlWriter.Create(filePath + "\\EnemyList.xml", xmlWriterSettings);
            string str = "<xml>";
            xmlWriter.WriteStartElement("xml");
            foreach (EnemyEditorData current in enemyDataList) {
                str += "<EnemyObj>\n";
                xmlWriter.WriteStartElement("EnemyObj");
                XmlWriter arg_79_0 = xmlWriter;
                string arg_79_1 = "Type";
                byte type = current.Type;
                arg_79_0.WriteAttributeString(arg_79_1, type.ToString());
                xmlWriter.WriteAttributeString("SpriteName", current.SpriteName);
                XmlWriter arg_AB_0 = xmlWriter;
                string arg_AB_1 = "BasicScaleX";
                float x = current.BasicScale.X;
                arg_AB_0.WriteAttributeString(arg_AB_1, x.ToString());
                XmlWriter arg_CB_0 = xmlWriter;
                string arg_CB_1 = "BasicScaleY";
                float y = current.BasicScale.Y;
                arg_CB_0.WriteAttributeString(arg_CB_1, y.ToString());
                XmlWriter arg_EB_0 = xmlWriter;
                string arg_EB_1 = "AdvancedScaleX";
                float x2 = current.AdvancedScale.X;
                arg_EB_0.WriteAttributeString(arg_EB_1, x2.ToString());
                XmlWriter arg_10B_0 = xmlWriter;
                string arg_10B_1 = "AdvancedScaleY";
                float y2 = current.AdvancedScale.Y;
                arg_10B_0.WriteAttributeString(arg_10B_1, y2.ToString());
                XmlWriter arg_12B_0 = xmlWriter;
                string arg_12B_1 = "ExpertScaleX";
                float x3 = current.ExpertScale.X;
                arg_12B_0.WriteAttributeString(arg_12B_1, x3.ToString());
                XmlWriter arg_14B_0 = xmlWriter;
                string arg_14B_1 = "ExpertScaleY";
                float y3 = current.ExpertScale.Y;
                arg_14B_0.WriteAttributeString(arg_14B_1, y3.ToString());
                XmlWriter arg_16B_0 = xmlWriter;
                string arg_16B_1 = "MinibossScaleX";
                float x4 = current.MinibossScale.X;
                arg_16B_0.WriteAttributeString(arg_16B_1, x4.ToString());
                XmlWriter arg_18B_0 = xmlWriter;
                string arg_18B_1 = "MinibossScaleY";
                float y4 = current.MinibossScale.Y;
                arg_18B_0.WriteAttributeString(arg_18B_1, y4.ToString());
                xmlWriter.WriteEndElement();
                str += "</EnemyObj>\n";
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();
        }
    }
}
