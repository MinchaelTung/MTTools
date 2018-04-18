// DailyRemindersStorage.cs by Charles Petzold, March 2009
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;

namespace DailyReminders
{
    public class DailyRemindersStorage
    {
        const string filename = "reminders.xml";
        Dictionary<DateTime, string> reminders = new Dictionary<DateTime, string>();

        public DailyRemindersStorage()
        {
            IsolatedStorageFileStream stream;

            try
            {
                stream = new IsolatedStorageFileStream(filename, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return;
            }

            // Read XML 
            XmlReader reader = XmlReader.Create(stream);
            reader.ReadStartElement("Reminders");

            while (reader.IsStartElement("Reminder"))
            {
                string str = reader.GetAttribute("DateTime");
                DateTime dateTime = DateTime.Parse(str);
                string text = reader.GetAttribute("Text");
                reminders.Add(dateTime, text);
                reader.ReadToNextSibling("Reminder");
            }
            reader.Close();
        }

        public string GetReminderText(DateTime key)
        {
            string text;

            if (!reminders.TryGetValue(key, out text))
                text = "";

            return text;
        }

        public void Update(DateTime key, string text)
        {
            if (text != null)
                text = text.Trim();

            if (String.IsNullOrEmpty(text))
            {
                if (reminders.ContainsKey(key))
                    reminders.Remove(key);
            }
            else
            {
                if (reminders.ContainsKey(key))
                    reminders[key] = text;
                else
                    reminders.Add(key, text);
            }
        }

        public void Save()
        {
            IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Create, FileAccess.Write);

            // Write XML
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(stream, settings);
            writer.WriteStartElement("Reminders");

            foreach (DateTime key in reminders.Keys)
            {
                writer.WriteStartElement("Reminder");
                writer.WriteAttributeString("DateTime", key.ToString("O"));
                writer.WriteAttributeString("Text", reminders[key]);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Close();
        }
    }
}
