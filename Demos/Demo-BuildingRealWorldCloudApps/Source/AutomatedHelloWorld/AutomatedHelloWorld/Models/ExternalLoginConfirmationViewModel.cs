namespace AutomatedHelloWorld.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        public string LoginProvider { get; set; }
    }
}