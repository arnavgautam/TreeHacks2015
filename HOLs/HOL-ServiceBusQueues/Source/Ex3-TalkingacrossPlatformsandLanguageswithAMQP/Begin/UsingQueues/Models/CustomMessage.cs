namespace UsingQueues.Models
{
    using System;

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
    }

}