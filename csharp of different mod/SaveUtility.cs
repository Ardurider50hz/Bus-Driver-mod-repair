using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MSCLoader;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000030 RID: 48
	public class SaveUtility
	{
		// Token: 0x060000DD RID: 221 RVA: 0x0000B5D8 File Offset: 0x000097D8
		public static void Save<T>(T saveData)
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
				xmlSerializerNamespaces.Add("", "");
				TextWriter output = new StreamWriter(SaveUtility.path);
				XmlWriterSettings settings = new XmlWriterSettings
				{
					Indent = true,
					IndentChars = "    ",
					NewLineOnAttributes = false,
					OmitXmlDeclaration = true
				};
				XmlWriter xmlWriter = XmlWriter.Create(output, settings);
				xmlSerializer.Serialize(xmlWriter, saveData, xmlSerializerNamespaces);
				xmlWriter.Close();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				ModConsole.Error(SaveUtility.modName + ": " + ex.ToString());
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000B6A4 File Offset: 0x000098A4
		public static SaveData Load<T>()
		{
			SaveData result;
			try
			{
				bool flag = File.Exists(SaveUtility.path);
				if (flag)
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
					XmlReader xmlReader = XmlReader.Create(new StreamReader(SaveUtility.path));
					result = (xmlSerializer.Deserialize(xmlReader) as SaveData);
				}
				else
				{
					result = new SaveData();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				ModConsole.Error(SaveUtility.modName + ": " + ex.ToString());
				result = new SaveData();
			}
			return result;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000B744 File Offset: 0x00009944
		public static void Remove()
		{
			bool flag = File.Exists(SaveUtility.path);
			if (flag)
			{
				File.Delete(SaveUtility.path);
				ModConsole.Print(SaveUtility.modName + ": Savefile found and deleted, mod is reset.");
			}
			else
			{
				ModConsole.Print(SaveUtility.modName + ": Savefile not found, mod is already reset.");
			}
		}

		// Token: 0x040001AB RID: 427
		private static string modName = typeof(SaveUtility).Namespace;

		// Token: 0x040001AC RID: 428
		private static string path = Path.Combine(Application.persistentDataPath, SaveUtility.modName + ".xml");
	}
}
