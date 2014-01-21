namespace Expenses.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExpenseReportStatus
    {
        [Key]
        [Column("ExpenseReportStatusID", TypeName = "tinyint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required]
        [Column("StatusName")]
        [MaxLength(10)]
        public string Name { get; set; }

        [Column("StatusDescription")]
        [MaxLength(100)]
        public string Description { get; set; }
    }
}