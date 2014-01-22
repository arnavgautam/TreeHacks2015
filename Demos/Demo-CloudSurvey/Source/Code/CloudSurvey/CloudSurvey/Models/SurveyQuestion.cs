namespace CloudSurvey.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SurveyQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int Index { get; set; }

        public string Description { get; set; }

        public int TypeValue { get; set; }

        public virtual QuestionType Type
        {
            get 
            { 
                return (QuestionType)this.TypeValue; 
            }

            set 
            { 
                this.TypeValue = (int)value; 
            }
        }
    }
}