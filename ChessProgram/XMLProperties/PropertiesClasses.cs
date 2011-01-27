using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;

namespace XMLProperties
{
	public class WrongXMLSyntaxException : ApplicationException
	{
		public WrongXMLSyntaxException ()
			: base()
		{
		}
		
		public WrongXMLSyntaxException (string message)
			: base(message)
		{
		}
		
		public WrongXMLSyntaxException (string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
	
	public class OnlineServerInfo
	{
		private Dictionary<string, string> info;
		
		public OnlineServerInfo ()
		{
			info = new Dictionary<string, string> ();
		}
		
		public Dictionary<string, string> Info
		{
			get { return info; }
			set { info = value; }
		}
		
		public override string ToString ()
		{
			string result = "";
			foreach (KeyValuePair<string, string> pair in info)
			{
				result += pair.Key + " " + pair.Value + Environment.NewLine;
			}
			
			return result;
		}

	}
	
	public abstract class OptionsBase<T> where T : class
	{
		protected Dictionary<string, T> properties;
		
		public OptionsBase ()
		{
			properties = new Dictionary<string, T> ();
		}
		
		/// <summary>
		/// Parses XML file and saves it content to Dictionary
		/// </summary>
		/// <param name="pathToXML">Path to XML file</param>
		public abstract void Parse(string pathToXML);
		
		public Dictionary<string, T> Properties
		{
			get { return properties; }
		}
	}
	
	public abstract class StringOptions : OptionsBase<string>
	{
		public StringOptions ()
			: base()
		{
		}
		
		protected virtual void InnerParse (string pathToXML, 
			string rootName, string nodeName, string attrName, string attrValue)
		{
			XPathDocument doc = new XPathDocument (pathToXML);
			XPathNavigator nav = ((IXPathNavigable)doc).CreateNavigator ();
			
			XPathNodeIterator iter = nav.Select (string.Format("/{0}/{1}", rootName, nodeName));
			while (iter.MoveNext ())
			{
				string nameAttr = iter.Current.GetAttribute(attrName, string.Empty);
				string valueAttr = iter.Current.GetAttribute(attrValue, string.Empty);
				
				if (string.IsNullOrEmpty(nameAttr) || string.IsNullOrEmpty(valueAttr))
					throw new WrongXMLSyntaxException("Empty program properties");
				
				if (properties.ContainsKey(nameAttr))
					throw new WrongXMLSyntaxException("Two properties with same names");
				
				properties.Add(nameAttr, valueAttr);
			}
		}
	}
	
	public class ProgramOptions : StringOptions
	{
		public ProgramOptions ()
			: base()
		{
		}
		
		public override void Parse (string pathToXML)
		{
			InnerParse (pathToXML, "ProgramOptions", "Option", "name", "value");
		}
	}
	
	public class FiguresPicturesOptions : StringOptions
	{
		public FiguresPicturesOptions ()
			: base()
		{
		}
		
		public override void Parse (string pathToXML)
		{
			XPathDocument doc = new XPathDocument (pathToXML);
			XPathNavigator nav = ((IXPathNavigable)doc).CreateNavigator ();
			
			XPathNodeIterator iter = nav.Select ("/FiguresPictures");
			
			string whiteprefix = iter.Current.GetAttribute("whiteprefix", string.Empty);
			string blackprefix = iter.Current.GetAttribute("blackprefix", string.Empty);
			
			iter = nav.Select (string.Format("/{0}/{1}", "FiguresPictures", "Picture"));
			while (iter.MoveNext ())
			{
				string nameAttr = iter.Current.GetAttribute("name", string.Empty);
				string valueAttr = iter.Current.GetAttribute("value", string.Empty);
				
				if (string.IsNullOrEmpty(nameAttr) || string.IsNullOrEmpty(valueAttr))
					throw new WrongXMLSyntaxException("Empty program properties");
				
				if (properties.ContainsKey(nameAttr))
					throw new WrongXMLSyntaxException("Two properties with same names");
				
				properties.Add(whiteprefix + nameAttr, whiteprefix + valueAttr);
				properties.Add(blackprefix + nameAttr, blackprefix + valueAttr);
			}
		}

	}
	
	public class InternetServersOptions : OptionsBase<OnlineServerInfo>
	{
		public InternetServersOptions ()
			: base()
		{
		}
		
		public override void Parse (string pathToXML)
		{
			XPathDocument doc = new XPathDocument (pathToXML);
			XPathNavigator nav = ((IXPathNavigable)doc).CreateNavigator ();
			
			string rootName = "OnlineServers";
			string nodeName = "Server";
			
			XPathNodeIterator iter = nav.Select (string.Format("/{0}/{1}", rootName, nodeName));
			while (iter.MoveNext ())
			{
				OnlineServerInfo server = new OnlineServerInfo();
				
				XPathNodeIterator tempIter = iter.Current.SelectDescendants(XPathNodeType.Element, false);
				while (tempIter.MoveNext())
				{
					server.Info.Add(tempIter.Current.Name, tempIter.Current.Value);
				}
				
				if (properties.ContainsKey(server.Info["Name"]))
					throw new WrongXMLSyntaxException("Servers must have different names");
				
				properties.Add(server.Info["Name"], server);
			}
		}
	}
}