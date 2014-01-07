namespace FabrikamInsurance.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public class QuoteViewModel
    {
        // render support
        public IEnumerable<KeyValuePair<string, string>> Makes { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Models { get; set; }

        public IEnumerable<Factor> BodyStyles { get; set; }

        public IEnumerable<Factor> BrakeTypes { get; set; }

        public IEnumerable<Factor> SafetyEquipment { get; set; }

        public IEnumerable<Factor> AntiTheftDevices { get; set; }

        public IEnumerable<int> YearList { get; set; }

        public string Message { get; set; }

        public decimal MonthlyPremium { get; set; }

        public decimal YearlyPremium { get; set; }

        // view model data
        [Required]
        [DisplayName("Make")]
        public string MakeId { get; set; }
        [Required]
        [DisplayName("Model")]
        public string ModelId { get; set; }
        [Required]
        [DisplayName("Year")]
        public int ManufacturedYear { get; set; }
        [Required]
        [DisplayName("Body Style")]
        public string BodyStyleId { get; set; }
        [Required]
        [DisplayName("Anti-Lock Brakes")]
        public string BrakeTypeId { get; set; }
        [Required]
        [DisplayName("Automatic Safety Equipment")]
        public string SafetyEquipmentId { get; set; }
        [Required]
        [DisplayName("Anti-Theft Device")]
        public string AntiTheftDeviceId { get; set; }
    }
}