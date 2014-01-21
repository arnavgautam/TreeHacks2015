namespace Expenses.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExpenseReportDetail
    {
        [Key]
        [Column("ExpenseReportDetailID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("ExpenseDate")]
        public DateTime? Date { get; set; }

        [MaxLength(50)]
        [Column("ExpenseCategory")]        
        public string Category { get; set; }
        
        [Required]
        [MaxLength(200)]
        [Column("ExpenseDescription")]        
        public string Description { get; set; }

        [Column("MerchantName")]
        [MaxLength(100)]
        public string Merchant { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [Display(Name = "Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [Display(Name = "Billed Amount")]
        public decimal BilledAmount { get; set; }

        [MaxLength(10)]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [MaxLength(10)]
        [Display(Name = "Cost Center")]
        public string CostCenter { get; set; }

        [MaxLength(250)]
        public string ReceiptUrl { get; set; }

        [Required]
        [Column("ExpenseReportID")]
        public int ReportId { get; set; }

        [Required]
        [ForeignKey("ReportId")]
        public virtual ExpenseReport Report { get; set; }

        public void CopyFrom(ExpenseReportDetail detail)
        {
            this.Date = detail.Date;
            this.Category = detail.Category;
            this.Description = detail.Description;
            this.Merchant = detail.Merchant;
            this.TransactionAmount = detail.TransactionAmount;
            this.BilledAmount = detail.BilledAmount;
            this.AccountNumber = detail.AccountNumber;
            this.CostCenter = detail.CostCenter;
            this.ReceiptUrl = detail.ReceiptUrl;
            this.ReportId = detail.ReportId;
        }

        public override bool Equals(object obj)
        {
            return (obj is ExpenseReportDetail) ? ((ExpenseReportDetail)obj).Id.Equals(this.Id) : false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}