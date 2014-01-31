namespace MyTodo.WebUx.Models
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }
}