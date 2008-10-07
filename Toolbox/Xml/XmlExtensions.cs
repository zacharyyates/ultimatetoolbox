/* Zachary Yates
 * Copyright © 2008 YatesMorrison Software Company
 * 10/7/2008 1:27 PM
 */

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Toolbox.Xml
{
	public static class XmlExtensions
	{
		//
		// TODO: Find a better name for this method
		//
		public static XmlDocument ToXml(this Object obj)
		{
			using (StringWriter sw = new StringWriter())
			{
				Type t = obj.GetType();
				XmlSerializer xs = new XmlSerializer(t);
				xs.Serialize(sw, obj);

				XmlDocument xd = new XmlDocument();
				xd.LoadXml(sw.ToString());

				return xd;
			}
		}

		//
		// TODO: Find a better name for this method
		//
		public static String ToXmlString(this Object obj)
		{
			StringBuilder sb = new StringBuilder();
			using (XmlTextWriter xw = new XmlTextWriter(new StringWriter(sb)))
			{
				Type t = obj.GetType();
				XmlSerializer xs = new XmlSerializer(t);
				xw.Formatting = Formatting.Indented;
				xs.Serialize(xw, obj);

				return sb.ToString();
			}
		}

		//
		// TODO: Find a better name for this method
		//
		public static T ToObjectFromXml<T>(this String xmlData)
		{
			XmlSerializer s = new XmlSerializer(typeof(T));
			using (StringReader reader = new StringReader(xmlData))
			{
				Object obj = s.Deserialize(reader);
				return (T)obj;
			}
		}
	}

	//
	// TODO: Merge all this functionality into a single class, if possible
	//
	public static class XmlSerializationHelper<T>
	{
		public static string ToXml(T obj)
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Indent = true,
				Encoding = Encoding.UTF8
			};
			StringBuilder output = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(output, settings))
			{
				ToXml(obj, writer);
			}
			return output.ToString();
		}
		public static void ToXml(T obj, string xmlFilePath, Encoding encoding)
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Indent = true,
				Encoding = encoding
			};
			using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
			{
				ToXml(obj, writer);
			}
		}
		public static void ToXml(T obj, XmlWriter xmlWriter)
		{
			if (typeof(T).IsSerializable)
			{
				XmlSerializer serializer = new XmlSerializer(obj.GetType());
				serializer.Serialize(xmlWriter, obj);
			}
			else
			{
				throw new InvalidOperationException("This class is not marked serializable.");
			}
		}

		public static T FromXml(XmlReader xmlReader)
		{
			return FromXml<T>(xmlReader);
		}
		public static T FromXml(string xmlString)
		{
			return FromXml<T>(xmlString);
		}

		public static TReturnType FromXml<TReturnType>(string xmlString)
		{
			return FromXml<TReturnType>(XmlReader.Create(new StringReader(xmlString)));
		}
		public static TReturnType FromXml<TReturnType>(XmlReader xmlReader)
		{
			if (typeof(TReturnType).IsSerializable)
			{
				XmlSerializer serializer = new XmlSerializer(typeof(TReturnType));
				return (TReturnType)serializer.Deserialize(xmlReader);
			}
			else
			{
				throw new InvalidOperationException("This class is not marked serializable.");
			}
		}
	}

	public static class XmlSerializationExtensions
	{
		public static string ToXml(this object obj)
		{
			return XmlSerializationHelper<object>.ToXml(obj);
		}
		public static void ToXml(this object obj, string xmlFilePath, Encoding encoding)
		{
			XmlSerializationHelper<object>.ToXml(obj, xmlFilePath, encoding);
		}
		public static void ToXml(this object obj, XmlWriter writer)
		{
			XmlSerializationHelper<object>.ToXml(obj, writer);
		}

		public static void FromXml(this object obj, string xmlString)
		{
			obj = XmlSerializationHelper<object>.FromXml(xmlString);
		}
		public static void FromXml(this object obj, XmlReader xmlReader)
		{
			obj = XmlSerializationHelper<object>.FromXml(xmlReader);
		}
	}
}