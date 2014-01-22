namespace CloudSurvey.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SurveyAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual SurveyQuestion Question { get; set; }

        public Guid SurveyQuestionId { get; set; }

        public string Value { get; set; }        
    }
}