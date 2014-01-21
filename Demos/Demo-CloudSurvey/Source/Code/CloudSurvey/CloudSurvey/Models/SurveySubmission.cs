namespace CloudSurvey.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SurveySubmission
    {
        public SurveySubmission()
        {
            this.Answers = new List<SurveyAnswer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual Survey Survey { get; set; }

        public Guid SurveyId { get; set; }

        public virtual ICollection<SurveyAnswer> Answers { get; set; }        
    }
}