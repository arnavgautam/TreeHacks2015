namespace UsingQueues.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CustomMessage
    {
        private DateTime date;
        private string body;

        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public string Body
        {
            get { return this.body; }
            set { this.body = value; }
        }

        public Dictionary<string, object> ToDictinary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("body", this.body);
            dictionary.Add("date", this.date);
            return dictionary;
        }
    }

}