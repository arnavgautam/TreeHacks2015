namespace ContactManager.Web.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Contact
    {        
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]        
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Must have a minimum length of 3.")]
        public string Company { get; set; }

        [DisplayName("Business Phone")]
        [DataType(DataType.PhoneNumber)]        
        public string BusinessPhone { get; set; }

        [DisplayName("Mobile Phone")]
        [DataType(DataType.PhoneNumber)]        
        public string MobilePhone { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Must have a minimum length of 3.")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Must have a minimum length of 3.")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [StringLength(10)]
        [DataType(DataType.Text)]        
        public string Zip { get; set; }
    }
}