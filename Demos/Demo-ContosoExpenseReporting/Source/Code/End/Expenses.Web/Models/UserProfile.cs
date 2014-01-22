namespace Expenses.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class UserProfile
    {
        [Key]        
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Column("ModifyDateTime")]
        public DateTime? Modified { get; set; }

        [Display(Name = "Manager")]
        public Guid? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public virtual UserProfile Manager { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }
    }
}