using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MyCode.ObjectSerializer
{
	public class ObjectSerializer
	{
		public static string Serialize<T>(T @object)
		{
			if (@object == null)
				return null;

			using (var stream = new MemoryStream())
			{
				XmlWriter writer = null;
				if (_writerSetters != null)
				{
					writer = XmlWriter.Create(stream, _writerSetters);
				}
				else
				{
					writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8);
				}

				var serializer = new DataContractSerializer(typeof(T));

				serializer.WriteObject(writer, @object);
				writer.Flush();

				stream.Seek(0, SeekOrigin.Begin);

				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					return reader.ReadToEnd();
				}
			}
		}

		public static T Deserialize<T>(string xml)
		{
			if (string.IsNullOrEmpty(xml))
				return default(T);

			using (var stringReader = new StringReader(xml))
			{
				XmlReader reader = null;
				if (_readerSetters != null)
				{
					reader = XmlReader.Create(stringReader, _readerSetters);
				}
				else
				{
					reader = XmlDictionaryReader.Create(stringReader);
				}

				var serializer = new DataContractSerializer(typeof(T));

				return (T)serializer.ReadObject(reader, false);
			}
		}

		#region Setter

		private static XmlWriterSettings _writerSetters { get; set; }

		private static XmlReaderSettings _readerSetters { get; set; }

		private static void SetXmlWriterSettings(XmlWriterSettings setter)
		{
			if (setter != null)
			{
				_writerSetters = setter;
			}
			else
			{
				_writerSetters = new XmlWriterSettings()
				{
					Encoding = Encoding.UTF8,
					Indent = true,
					IndentChars = "\t",
					NewLineChars = Environment.NewLine,
					NewLineHandling = NewLineHandling.Replace,
					NewLineOnAttributes = false,
					ConformanceLevel = ConformanceLevel.Auto,
				};
			}
		}

		private static void SetXmlReaderSettings(XmlReaderSettings setter)
		{
			if (setter != null)
			{
				_readerSetters = setter;
			}
			else
			{
				_readerSetters = new XmlReaderSettings()
				{
					ConformanceLevel = ConformanceLevel.Auto,
				};
			}
		}

		public static string Serialize<T>(T @object, XmlWriterSettings setter)
		{
			SetXmlWriterSettings(setter);
			return Serialize(@object);
		}

		public static T Deserialize<T>(string xml, XmlReaderSettings setter)
		{
			SetXmlReaderSettings(setter);
			return Deserialize<T>(xml);
		}

		#endregion
	}
}
