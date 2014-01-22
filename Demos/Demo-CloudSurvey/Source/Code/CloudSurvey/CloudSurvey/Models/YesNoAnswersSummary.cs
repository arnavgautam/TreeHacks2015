namespace CloudSurvey.Models
{
    using System;

    public class YesNoAnswersSummary : AnswersSummary
    {
        public int Yes { get; set; }

        public int No { get; set; }
    }
}