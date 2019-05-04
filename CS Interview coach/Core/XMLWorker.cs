using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace CS_Interview_coach
{
    /// <summary>
    /// Class for working with XML file.
    /// </summary>
    public sealed class XMLWorker
    {
        /// <summary>
        /// Path to XML file
        /// </summary>
        private string filePath { get; set; }

        /// <summary>
        /// Basic ctor for initializing worker.
        /// </summary>
        /// <param name="XMLFullPath"></param>
        public XMLWorker(string XMLFullPath)
        {
            if (string.IsNullOrEmpty(XMLFullPath))
                throw new Exception("Path is null or empty");
            else
                this.filePath = XMLFullPath;
        }

        /// <summary>
        /// Reading XML file and adding it to the dictionary.
        /// </summary>
        public void Read() 
        {
            try
            {
                XDocument xdoc = XDocument.Load(filePath);
                foreach (XElement qaelem in xdoc.Element("Questions").Elements("question"))
                {
                    XAttribute qattribute = qaelem.Attribute("qtext");
                    XAttribute answattribute = qaelem.Attribute("qanswer");

                    if (qattribute != null && answattribute != null)
                    {
                        QADictionary.questionAndAnswers.Add(
                            QADictionary.questionAndAnswers.Count, new QAInstance(
                                qattribute.Value, answattribute.Value));
                    }
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Windows.MessageBox.Show("XML file not found.", "New file will be created.", System.Windows.MessageBoxButton.OK);
                CreateNew();
            }
            catch (Exception E)
            {
                System.Windows.MessageBox.Show(E.ToString());
            }
        }

        /// <summary>
        /// Adding new record to XML.
        /// </summary>
        /// <param name="instance"></param>
        public void Increment(QAInstance instance) 
        {
            if (String.IsNullOrWhiteSpace(instance.Answer) || String.IsNullOrWhiteSpace(instance.Question) ||
                instance.Question.Length < 5 || instance.Answer.Length < 5)
            {
                System.Windows.MessageBox.Show("Args is empty or shorter then 5 symbols");
            }
            else
            {
                XDocument xdoc = XDocument.Load(filePath);
                xdoc.Elements("Questions").First().Add(
                    new XElement("question", new XAttribute("qtext", instance.Question),
                    new XAttribute("qanswer", instance.Answer)));
                xdoc.Save(filePath);
            }
        }

        /// <summary>
        /// Creating new XML file.
        /// </summary>
        public void CreateNew() 
        {
            new XDocument(new XElement("Questions")).Save($@"{filePath}");
        }

        /// <summary>
        /// Updating existing record in XML.
        /// </summary>
        /// <param name="instanceOld"></param>
        /// <param name="instanceUpdated"></param>
        public void UpdateExisting(QAInstance instanceOld, QAInstance instanceUpdated) 
        {
            XDocument xdoc = XDocument.Load(filePath);
            XElement root = xdoc.Element("Questions");

            foreach (XElement xe in root.Elements("question").ToList())
            {
                if (xe.Attribute("qtext").Value == instanceOld.Question)
                {
                    xe.Attribute("qtext").Value = instanceUpdated.Question;
                    xe.Attribute("qanswer").Value = instanceUpdated.Answer;
                }
            }
            xdoc.Save(filePath);
        }
    }
}