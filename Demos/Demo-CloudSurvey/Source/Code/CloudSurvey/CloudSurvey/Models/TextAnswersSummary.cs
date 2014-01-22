namespace CloudSurvey.Models
{
    using System.Collections.Generic;

    public class TextAnswersSummary : AnswersSummary
    {
        public List<string> Answers { get; set; }
    }
}