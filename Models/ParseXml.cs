using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace OpenRSS_1.Models
{
    public class ParseXml
    {
       
        private string xmlString;
        public string XmlString 
        {
            get { return xmlString; }
            set { xmlString = value.Trim(); }
        }
        public string parseXmlString() {
            string htmlString = "";
           

            if (xmlString == null) { return ""; }

            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(xmlString));

            /*            while (reader.ReadToFollowing("element"))
                        {
                            // Print the name and value of the element
                            Debug.WriteLine("Element: {0}", reader.Name);
                            Debug.WriteLine("Value: {0}", reader.ReadElementContentAsString());
                        }*/



            while (reader.Read())
            {
                string? description = null;
                string? contentEncoded = null;
                string? content = null;


                // Print the node type and name
                Debug.WriteLine("NodeType: {0}, Name: {1}", reader.NodeType, reader.Name);

                // If the node is an element, print its attributes
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "description")
                    {
                        description = reader.ReadString();                        
                    }

                    if (reader.Name == "content:encoded")
                    {   
                        contentEncoded = reader.ReadString();
                                                
                    }
                    
                    if (reader.Name == "content")
                    {
                        contentEncoded = reader.ReadString();

                    }

                    if (description == null && contentEncoded != null)
                    {
                        htmlString += contentEncoded;
                        htmlString += "<br><hr><br>";
                    }

                    if (contentEncoded == null && description != null)
                    {
                        htmlString += description;
                        htmlString += "<br><hr><br>";
                    }

                    if (contentEncoded == null && description == null && content != null)
                    {
                        htmlString += content;
                        htmlString += "<br><hr><br>";
                    }

                    //Debug.WriteLine(reader.ReadString());
                    //Debug.WriteLine("Attributes:");
                    if (reader.HasAttributes)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToAttribute(i);
                      //      Debug.WriteLine("{0}: {1}", reader.Name, reader.Value);                            
                        }
                    }
                }
            }
            return htmlString;
        }

        public string feedName(string url)
        {
            XmlDocument rssXmlDoc = new XmlDocument();

            // Load the RSS file from the RSS URL
            rssXmlDoc.Load(url);

            XmlNode channelNode = rssXmlDoc.SelectSingleNode("rss/channel");
            XmlNode rssTitle = channelNode.SelectSingleNode("title");
            string name = rssTitle.InnerText;

            return name;
        }


        public string newParseXml(string url) 
        {
            XmlDocument rssXmlDoc = new XmlDocument();

            // Load the RSS file from the RSS URL
            rssXmlDoc.Load(url);

            XmlNode channelNode = rssXmlDoc.SelectSingleNode("rss/channel");
            XmlNode rssTitle = channelNode.SelectSingleNode("title");
            string Name = rssTitle.InnerText;


            // Parse the Items in the RSS file
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            StringBuilder rssContent = new StringBuilder();

            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                string description = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("pubDate");
                string pubDate = rssSubNode != null ? "Published date: "+ rssSubNode.InnerText : "";

                if (pubDate.Length > 0) rssContent.Append(pubDate).Append("<br>");
                rssContent.Append(title+"<br>"+description+"<br><a href='" + link + "'>"+link+"</a>");
                rssContent.Append("\n<hr>\n");                
            }

            // Return the string that contain the RSS items
           // Debug.WriteLine(rssContent.ToString());
            return rssContent.ToString();
        }


    }
}
