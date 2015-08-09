using System.Collections.Generic;
using System.Xml;


namespace RogueCastle {

    public class XMLCompiler {

        public static void CompileEnemies(List<EnemyEditorData> enemyDataList, string filePath) {

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;

            XmlWriter xmlWriter = XmlWriter.Create(filePath + "\\EnemyList.xml", xmlWriterSettings);
            xmlWriter.WriteStartElement("xml");

            for (int index = 0; index < enemyDataList.Count; index++) {

                EnemyEditorData current = enemyDataList[index];
                
                xmlWriter.WriteStartElement("EnemyObj");
                byte type = current.Type;
                xmlWriter.WriteAttributeString("Type"          , type.ToString());
                xmlWriter.WriteAttributeString("SpriteName"    , current.SpriteName);
                xmlWriter.WriteAttributeString("BasicScaleX"   , current.BasicScale.X.ToString());
                xmlWriter.WriteAttributeString("BasicScaleY"   , current.BasicScale.Y.ToString());
                xmlWriter.WriteAttributeString("AdvancedScaleX", current.AdvancedScale.X.ToString());
                xmlWriter.WriteAttributeString("AdvancedScaleY", current.AdvancedScale.Y.ToString());
                xmlWriter.WriteAttributeString("ExpertScaleX"  , current.ExpertScale.X.ToString());
                xmlWriter.WriteAttributeString("ExpertScaleY"  , current.ExpertScale.Y.ToString());
                xmlWriter.WriteAttributeString("MinibossScaleX", current.MinibossScale.X.ToString());
                xmlWriter.WriteAttributeString("MinibossScaleY", current.MinibossScale.Y.ToString());
                xmlWriter.WriteEndElement();

            }

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();

        }

    }

}
