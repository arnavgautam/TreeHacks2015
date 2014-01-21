namespace Expenses.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExpenseReport
    {
        public ExpenseReport()
        {
            this.Created = DateTime.Now;
            this.StatusId = 1;
        }

        [Key]
        [Column("ExpenseReportID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("ExpenseReportName")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("ExpenseReportPurpose")]
        public string Purpose { get; set; }

        [Required]
        [Column("ExpenseReportCreateDate")]
        public DateTime Created { get; set; }

        [Column("ExpenseReportSubmitDate")]
        public DateTime? Submitted { get; set; }

        [Column("Comments")]
        public string Comments { get; set; }

        [Column("ApproverUserName")]
        [Display(Name = "Approver Name")]
        public string ApproverName { get; set; }

        [Timestamp]
        public byte[] LastModified { get; set; }

        [Required]
        [Column("ExpenseReportStatusID", TypeName = "tinyint")]
        public byte StatusId { get; set; }

        [ForeignKey("StatusId")]
        public virtual ExpenseReportStatus Status { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }

        public virtual ICollection<ExpenseReportDetail> Details { get; set; }

        public void CopyFrom(ExpenseReport report)
        {
            this.Name = report.Name;
            this.Purpose = report.Purpose;
            this.Created = report.Created;
            this.Submitted = report.Submitted;
            this.Comments = report.Comments;
            this.ApproverName = report.ApproverName;
            this.StatusId = report.StatusId;
            this.UserId = report.UserId;            
        }

        public override bool Equals(object obj)
        {
            return (obj is ExpenseReport) && ((ExpenseReport)obj).Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}