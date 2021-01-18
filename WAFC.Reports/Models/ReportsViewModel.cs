using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace WAFC.Reports.Models
{
    public class ReportsViewModel
    {

    }
    public class ViewModel
    {
        public StaffCostsViewModel staff { get; set; }
        public ReportThemesViewModel themes { get; set; }
        public string ThemesValues { get; set; }
        public string BudgetValues { get; set; }
        public string ActualsValues { get; set; }
        public string AvailableValues { get; set; }
        public string DataValues { get; set; }
        public int ThemeId { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double StaffCosts { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount { get; set; }
        public string ThemeDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        //public Decimal Overhead { get; set; }
        //[DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdActual { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdTotal
        {
            get { return YtdActual + OverheadAmount; }
            set => value = YtdActual + OverheadAmount;
        }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Available
        {
            get { return BudgetAmt- YtdTotal; }
            set => value = BudgetAmt - YtdTotal;
        }
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Double Cpercent
        {
            get { return BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt); }
            set => value = BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt);
        }
        [Display(Name = "From: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
        public DateTime? fromdate { get; set; }
        [Display(Name = "To: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
        public DateTime? todate { get; set; } 

        public List<ViewModel> ThemeList { get; set; }

    }
    public class StaffCostsViewModel
    {
       [Key]
        public int ThemeId { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double StaffCosts { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount { get; set; }
    }


    public class ReportThemesViewModel
    {
        
        //[ForeignKey("ThemeId")]
        [Key]
        public int ThemeId { get; set; }
        public string ThemeDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        //public Decimal Overhead { get; set; }
        //[DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [NotMapped]
        public Double YtdTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Available => (BudgetAmt - YtdActual);

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Double Cpercent => BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt);

    }

    public class ViewGrantsModel
    {
        public string GrantId { get; set; }
        public string GrantDescriptn { get; set; }
        public string GrantInfo { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Overhead { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? edate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? sdate { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdActual { get; set; }// => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdTotal
        {
            get { return YtdActual + OverheadAmount; }
            set => value = YtdActual + OverheadAmount;
        }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Available
        {
            get { return BudgetAmt - YtdTotal; }
            set => value = BudgetAmt - YtdTotal;
        }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount
        {
            get { return Overhead / 100 * (ActualAmt+ StaffCosts); }
            set => value = Overhead / 100 * (ActualAmt + StaffCosts);
        }
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Double Cpercent
        {
            get { return BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt); }
            set => value = BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt);
        }
        [Display(Name = "From: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? fromdate { get; set; } //=> Convert.ToDateTime("01-Jan-" + System.Web.HttpContext.Current.Session["syear"]);
        [Display(Name = "To: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? todate { get; set; } //=> int.Parse(System.Web.HttpContext.Current.Session["syear"].ToString()) < DateTime.Now.Year ? Convert.ToDateTime("31-Dec" + "-" + System.Web.HttpContext.Current.Session["syear"]) : DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double StaffCosts { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double SalaryOverheadAmount { get; set; }
        public List<ViewGrantsModel> GrantsList { get; set; }

    }
    public class StaffCostsGrantsViewModel
    {
        [Key]
        public string GrantId { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double StaffCosts { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double? OverheadAmount { get; set; }
    }
    public class ReportGrantsViewModel
    {
        [Key]
        public string GrantId { get; set; }
        public string GrantDescriptn { get; set; }
        public string GrantInfo
        {
            get
            {
                return GrantId + ":" + GrantDescriptn;
            }
        }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Overhead { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? edate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? sdate { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [NotMapped]
        public Double YtdTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount => (Overhead/100*ActualAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Available => (BudgetAmt - YtdActual);

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Double Cpercent => BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt);
        [Display(Name = "From: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? fromdate => Convert.ToDateTime("01-Jan-" + System.Web.HttpContext.Current.Session["syear"]);
        [Display(Name = "To: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? todate => int.Parse(System.Web.HttpContext.Current.Session["syear"].ToString()) < DateTime.Now.Year ? Convert.ToDateTime("31-Dec" + "-" + System.Web.HttpContext.Current.Session["syear"]) : DateTime.Now;

    }

    public class ReportNaturalClassViewModel
    {
        [Key]
        //public string GrantId { get; set; }
        //public string GrantDescriptn { get; set; }
        public int AccntClass { get; set; }
        public string AccntClassDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        //public Decimal Overhead { get; set; }
        //[DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
     
        public Double YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Available => (BudgetAmt - YtdTotal);

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Double Cpercent => BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [NotMapped]
        public Double YtdTotal
        {
            get { return YtdActual + OverheadAmount; }
            set => value = YtdActual + OverheadAmount;
        }

        [Display(Name = "From: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? fromdate => Convert.ToDateTime("01-Jan-" + System.Web.HttpContext.Current.Session["syear"]);
        [Display(Name = "To: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? todate => int.Parse(System.Web.HttpContext.Current.Session["syear"].ToString()) < DateTime.Now.Year ? Convert.ToDateTime("31-Dec" + "-" + System.Web.HttpContext.Current.Session["syear"]) : DateTime.Now;
        public List<ReportNaturalClassViewModel> NaturalClassList { get; set; }

    }

    public class ReportDetailsViewModel
    {
        [Key]
        //public string GrantId { get; set; }
        //public string GrantDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? Date { get; set; }
        public string JnlType { get; set; }
        public string TransReff { get; set; }
        public string Descriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double YtdActual { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double ActualAmt { get; set; }
        public List<ReportDetailsViewModel> DetailsList { get; set; }

    }
    public class ViewDonorModel
    {
        public string DonorId { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double StaffCosts { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount { get; set; }
        public string DonorDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdActual { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [NotMapped]
        public Double YtdTotal
        {
            get { return YtdActual + OverheadAmount; }
            set => value = YtdActual + OverheadAmount;
        }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [NotMapped]
        public Double Available
        {
            get { return BudgetAmt - YtdTotal; }
            set => value = BudgetAmt - YtdTotal;
        }
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Double Cpercent
        {
            get { return BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt); }
            set => value = BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt);
        }
        [Display(Name = "From: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? fromdate { get; set; } //=> Convert.ToDateTime("01-Jan-" + System.Web.HttpContext.Current.Session["syear"]);
        [Display(Name = "To: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? todate { get; set; } //=> int.Parse(System.Web.HttpContext.Current.Session["syear"].ToString()) < DateTime.Now.Year ? Convert.ToDateTime("31-Dec" + "-" + System.Web.HttpContext.Current.Session["syear"]) : DateTime.Now;
        public List<ViewDonorModel> ThemeList { get; set; }

    }
    public class StaffCostsDonorsViewModel
    {
        [Key]
        public string DonorId { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double StaffCosts { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount { get; set; }
    }

    public class ReportDonorsViewModel
    {
        [Key]
        public string DonorId { get; set; }
        public string DonorDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        //public Decimal Overhead { get; set; }
        //[DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [NotMapped]
        public Double YtdTotal
        {
            get { return YtdActual + OverheadAmount; }
            set => value = YtdActual + OverheadAmount;
        }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [NotMapped]
        public Double Available
        {
            get { return BudgetAmt - YtdTotal; }
            set => value = BudgetAmt - YtdTotal;
        }
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [NotMapped]
        public Double Cpercent
        {
            get { return BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt); }
            set => value = BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt);
        }
    }

    public class ReportAccntsViewModel
    {
        [Key]
        public string AccntId { get; set; }
        public string AccntClassDescriptn { get; set; }
        public string AccntDescriptn { get; set; }
        public int? AccntClass { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdTotal => (ActualAmt + CommAmt+ OverheadAmount);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double OverheadAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double Available => (BudgetAmt - YtdTotal);

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Double Cpercent => BudgetAmt == 0 ? 0 : ((YtdActual) / BudgetAmt);
        [Display(Name = "From: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? fromdate => Convert.ToDateTime(System.Web.HttpContext.Current.Session["sdate"]);
        [Display(Name = "To: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? todate => Convert.ToDateTime(System.Web.HttpContext.Current.Session["edate"]);
        public List<ReportAccntsViewModel> AllAccntsList { get; set;
        }

    }

    public class StaffCostsDetailsViewModel
    {
        [Key]
        public int staffId { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double StaffCosts { get; set; }
        public string Surname { get; set; }
        [Display(Name = "From: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? fromdate => Convert.ToDateTime(System.Web.HttpContext.Current.Session["sdate"]);
        [Display(Name = "To: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? todate => Convert.ToDateTime(System.Web.HttpContext.Current.Session["edate"]);
        public List<StaffCostsDetailsViewModel> StaffList
        {
            get; set;
        }
    }


}