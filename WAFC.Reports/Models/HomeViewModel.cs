using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WAFC.Reports.Models
{
    public class HomeViewModel
    {

    }
    [Table("tblThemes")]
    public class ThemesModel
    {
        [Key]
        public int ThemeId { get; set; }
        [Display(Name = "Theme Description")]
        public string ThemeDescriptn { get; set; }
    }
    [Table("tblGrants")]
    public class GrantsModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Grant Id")]
        public string GrantId { get; set; }

        [Display(Name = "Grant Description")]
        public string GrantDescriptn { get; set; }
        public string GrantInfo
        {
            get {
                return GrantId + " - " + GrantDescriptn;
            }
        }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "Grant Amount")]
        public Double? Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "Approved Budget Amount")]
        public Decimal? BAmount { get; set; }
        [Display(Name = "Overhead (%)")]
        public Double? Overhead { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? sdate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? edate { get; set; }
        [Display(Name = "Donor Id")]
        public string DonorId { get; set; }
        public virtual DonorsModel Donors { get; set; }
        public int? ThemeId { get; set; }
        [Display(Name = "Theme")]
        public virtual ThemesModel ThemesModel { get; set; }

    }
    [Table("tblDonors")]
    public class DonorsModel
    {
        [Key]
        public string DonorId { get; set; }
        [Display(Name = "Donor Name")]
        public string DonorDescriptn { get; set; }
        public string DonorInfo
        {
            get
            {
                return DonorId + " - " + DonorDescriptn;
            }
        }
    }
    [Table("tblAccountsCodes")]
    public class AccountCodesViewModel
    {
        [Key]
        public string AccntId { get; set; }
        [Display(Name = "Account Description")]
        public string AccntDescriptn { get; set; }
        [Display(Name = "Natural Classification")]
        public int? AccntClass { get; set; }
        public string AccntInfo
        {
            get
            {
                return AccntId + " - " + AccntDescriptn;
            }
        }
        public virtual AccountClassViewModel AccountClass { get; set; }
    }

    [Table("tblAccountsClass")]
    public class AccountClassViewModel
    {
        [Key]
        public int? AccntClass { get; set; }
        [Display(Name = "Natural Classification")]
        public string AccntClassDescriptn { get; set; }
    }

    [Table("budget_period_tbl")]
    public class BudgetPeriodsViewModel
    {
        [Key]
        public int? periodid { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime periodname { get; set; }
        public string PerioxDateText { get { return periodname.ToShortDateString(); } }
        [NotMapped]
        public HttpPostedFileBase FileUpload { get; set; }

    }


    public class ThemesViewModel
    {
        [Key]
        public int ThemeId { get; set; }
        public string ThemeDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetFwd { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal StaffCosts { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal OverheadAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal RbudgetAmt => (BudgetAmt + BudgetFwd);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal Available => (BudgetAmt - YtdActual);
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Decimal Cpercent
        {
            get
            {
                return RbudgetAmt == 0 ? 0 : ((YtdActual) / RbudgetAmt);
            }
        }
        public List<ThemesViewModel> ThemesList { get; set; }
    }
    public class IndexReportViewModel
    {
        public List<SelectListItem> FinYears { set; get; }
        public int? Year { set; get; }

        //public List<ThemesViewModel> ThemesList { get; set; }

    }

    public class ExchangeRatesViewModel
    {
        public List<SelectListItem> Currency { set; get; }
        public string Sdate { set; get; }
        public string Edate { set; get; }
        public string CurrencyId { set; get; }


        //public List<ThemesViewModel> ThemesList { get; set; }

    }

    public class MIRatesViewModel
    {
        public List<SelectListItem> Sdate { set; get; }
        public List<SelectListItem> Countries { set; get; }
        public string StartDate { get; set; }
        public string CountryId { get; set; }

        //public List<ThemesViewModel> ThemesList { get; set; }

    }
    public class GrantsViewModel
    {
        [Key]
        public string GrantId { get; set; }
        public Int32? DonorId { get; set; }
        //public string DonorDescriptn { get; set; }
        public string GrantDescriptn { get; set; }
        public string GrantInfo
        {
            get
            {
                return GrantId + " - " + GrantDescriptn;
            }
        }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetFwd { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal RbudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal StaffCosts { get; set; }
        public Decimal Overhead { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal Available => (BudgetAmt - YtdActual);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal OverheadAmount => (Overhead / 100 * (ActualAmt + StaffCosts));
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Decimal Cpercent
        {
            get
            {
                return RbudgetAmt == 0 ? 0 : ((YtdActual) / RbudgetAmt);
            }
        }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? sdate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? edate { get; set; }
        public List<GrantsViewModel> GrantsList { get; set; }

    }
    public class GrantsActualsViewModel
    {
        [Key]
        public string GrantId { get; set; }
        public string GrantDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal Amount { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? sdate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? edate { get; set; }
        public List<GrantsActualsViewModel> GrantsActualsList { get; set; }

    }
    public class GrantsAccountsViewModel
    {
        [Key]
        public Int32 accntClass { get; set; }
        public string accntClassDescriptn { get; set; }
        public Decimal Amount { get; set; }
        public List<GrantsAccountsViewModel> GrantsAccountsList { get; set; }

    }
    public class GrantsAccountCodesViewModel
    {
        [Key]
        public Int32 accntClass { get; set; }
        public string accntId { get; set; }
        public string accntDescriptn { get; set; }
        public string accntClassDescriptn { get; set; }
        public Double Amount { get; set; }
        public List<GrantsAccountCodesViewModel> GrantsAccountCodesList { get; set; }
    }

    public class GrantsDetailsViewModel
    {
        [Key]
        public string GrantId { get; set; }
        public string GrantDescriptn { get; set; }
        public int AccntClass { get; set; }
        public string AccntClassDescriptn { get; set; }
        public string AccntDescriptn { get; set; }
        public string JnlSource { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? Date { get; set; }
        public string TransReff { get; set; }
        public string Descriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double Amount { get; set; }
        public List<GrantsDetailsViewModel> GrantsDetailsList { get; set; }
    }

    [Table ("tblStaff")]
    public class StaffViewModel
    {
        [Key]
        public int budgetid { get; set; }
        public string userid { get; set; }
        public string surname { get; set; }
    }

    [Table("vwBudgetLines")]
    public class BudgetViewModel
    {
        [Key]
        public int bid { get; set; }
        public string grantid { get; set; }
        [Display(Name = "Budget Amount")]
        public double budgetamt { get; set; }
        [Display(Name = "Committment Amount")]
        public double commamt { get; set; }
        public string accntId { get; set; }
        public string accntDescriptn { get; set; }
        [Display(Name = "Budget b/Fwd")]
        public decimal budgetfwd { get; set; }
        [Display(Name = "Natural Classification")]
        public string NaturalClass
        {
            get
            {
                return accntId + " - " + accntDescriptn;
            }
        }
        [NotMapped]
        public string grantcode { set; get; }
        [NotMapped]
        public List<SelectListItem> Grants { set; get; }
        [NotMapped]
        public string accountid { set; get; }
        [NotMapped]
        public List<SelectListItem> AccountClass { set; get; }
        [NotMapped]
        public List<BudgetViewModel> AccountsList { get; set; }
    }


    public class NaturalClassViewModel
    {
        [Key]
        //public string GrantId { get; set; }
        //public string GrantDescriptn { get; set; }
        public Int32 AccntClass { get; set; }
        public string AccntClassDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetFwd { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal RbudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal CommAmt { get; set; }
       [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal OverheadAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal Available => (BudgetAmt - YtdActual);
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Decimal Cpercent
        {
            get
            {
                return RbudgetAmt == 0 ? 0 : ((YtdActual) / RbudgetAmt);
            }
        }
        public List<NaturalClassViewModel> NaturalClassList { get; set; }

    }

    public class AllAccntsViewModel
    {
        [Key]
        public int AccntId { get; set; }
        public string AccntClassDescriptn { get; set; }
        public string AccntDescriptn { get; set; }
        public int? AccntClass { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetFwd { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal RbudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal Available => (BudgetAmt - YtdActual);
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Decimal Cpercent
        {
            get
            {
                return RbudgetAmt == 0 ? 0 : ((YtdActual) / RbudgetAmt);
            }
        }
        public List<AllAccntsViewModel> AllAccntsList { get; set; }

    }
    public class DetailsViewModel
    {
        [Key]
        public int Id { get; set; }
        //public string GrantDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? Date { get; set; }
        public string JnlType { get; set; }
        public string TransReff { get; set; }
        public string Descriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double YtdActual { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Double ActualAmt { get; set; }
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
        public List<DetailsViewModel> DetailsList { get; set; }

    }

    public class DonorsViewModel
    {
        [Key]
        public string DonorId { get; set; }
        public string DonorDescriptn { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal StaffCosts { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetFwd { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal RbudgetAmt => (BudgetAmt + BudgetFwd);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal BudgetAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal CommAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal ActualAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal OverheadAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal YtdActual => (ActualAmt + CommAmt);
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal Available => (BudgetAmt - YtdActual);
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public Decimal Cpercent
        {
            get
            {
                return RbudgetAmt == 0 ? 0 : ((YtdActual) / RbudgetAmt);
            }
        }
        public List<DonorsViewModel> DonorsList { get; set; }

    }

    
}   