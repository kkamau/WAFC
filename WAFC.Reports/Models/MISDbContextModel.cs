using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace WAFC.Reports.Models
{

    //public class MISDbContext : DbContext
    //{

    //    public DbSet<ThemesViewModel> themesViewModels{ get; set; }
    //    public DbSet<GrantsViewModel> grantsViewModels { get; set; }

    //}


    public class MISDbContext : DbContext
    {
        public MISDbContext(string connString)
          : base(connString)
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 120;
        }
        public DbSet<ThemesViewModel> ThemesView{ get; set; }
        public DbSet<GrantsViewModel> GrantsView { get; set; }
        public DbSet<NaturalClassViewModel> NaturalClassView{ get; set; }
        public DbSet<AllAccntsViewModel> AllAccntsView { get; set; }
        public DbSet<DetailsViewModel> DetailsView { get; set; }
        public DbSet<DonorsViewModel> DonorsView { get; set; }
        public DbSet<GrantsActualsViewModel> GrantsActualsView { get; set; }
        public DbSet<GrantsAccountsViewModel> GrantsAccountsView { get; set; }
        public DbSet<GrantsAccountCodesViewModel> GrantsAccountCodesView { get; set; }
        public DbSet<GrantsDetailsViewModel> GrantsDetailsView { get; set; }

        public DbSet<ThemesModel> ThemesModels { get; set; }
        public DbSet<GrantsModel> GrantsModels { get; set; }
        public DbSet<DonorsModel> DonorsModel { get; set; }
        public DbSet<AccountClassViewModel> AccountClassModel { get; set; }
        public DbSet<AccountCodesViewModel> AccountCodesModel { get; set; }
        public DbSet<BudgetPeriodsViewModel> BudgetPeriodsModel { get; set; }
        public DbSet<ReportGrantsViewModel> ReportGrantsModel { get; set; }
        public DbSet<ReportThemesViewModel> ReportThemesModel { get; set; }
        public DbSet<ReportDonorsViewModel> ReportDonorsModel { get; set; }
        public DbSet<ReportNaturalClassViewModel> ReportNaturalClassModel { get; set; }
        public DbSet<StaffCostsViewModel> StaffCostsModel { get; set; }
        public DbSet<StaffCostsDonorsViewModel> StaffCostsDonorsModel { get; set; }
        public DbSet<StaffCostsGrantsViewModel> StaffCostsGrantsModel { get; set; }
        public DbSet<ReportAccntsViewModel> ReportAccntsModel { get; set; }
        public DbSet<StaffCostsDetailsViewModel> StafffCostsViewModel { get; set; }
        public DbSet<StaffViewModel> StaffViewModels { get; set; }
        public DbSet<BudgetViewModel> BudgetViewModels { get; set; }
 
        


    }

}