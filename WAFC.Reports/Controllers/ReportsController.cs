using WAFC.Reports.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WAFC.Reports.org.cgiar.ocs.icraf;

namespace WAFC.Reports.Controllers
{
    [Authorize(Roles = "Users")]
    public class ReportsController : Controller
    {
        // GET: Reports
        private MISDbContext db;
        static string constr;

        public ActionResult Index(string id, string syear)
        {

            List<SelectListItem> finyears = new List<SelectListItem>();


            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                finyears.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }



            var model = new IndexReportViewModel
            {

                FinYears = finyears
            };

            return View(model);
        }

        public void CRP(string cc, string et, string crp, string cname, string period)
        {
            QueryEngineV201101 QueryEngineCIP;
            string jscript = "";

            WSCredentials credentials = (WSCredentials)Session["WSCredentials"];
            TemplateList templateList = new TemplateList();
            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult inputForTemplateResult = new InputForTemplateResult();

            //bind data
            string text = "";
            int num = (!(crp != "")) ? 1966 : 2232;
            string text2 = "";
            string text3 = "";
            bool flag = false;
            if (!string.IsNullOrWhiteSpace(et))
            {
                text2 = et;
                if (text2.IndexOf(",") > 0)
                {
                    num = 2102;
                    flag = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(cc))
            {
                text3 = cc;
                if (text3.IndexOf(",") > 0)
                {
                    num = 2014;
                    flag = true;
                }
            }
            if (string.IsNullOrWhiteSpace(et) && string.IsNullOrWhiteSpace(cc) && string.IsNullOrWhiteSpace(crp))
            {
                ViewBag.PageTitle += " - Main Projects: All";
            }
            QueryEngineV201101 queryEngineV200606DotNet = new QueryEngineV201101();
            SearchCriteria searchCriteria = queryEngineV200606DotNet.GetSearchCriteria(num, true, credentials);
            InputForTemplateResult inputForTemplateResult2 = new InputForTemplateResult();
            TemplateResultOptions templateResultOptions = queryEngineV200606DotNet.GetTemplateResultOptions(credentials);
            templateResultOptions.RemoveHiddenColumns = true;
            inputForTemplateResult2.TemplateResultOptions = templateResultOptions;
            inputForTemplateResult2.TemplateId = num;
            inputForTemplateResult2.SearchCriteriaPropertiesList = searchCriteria.SearchCriteriaPropertiesList;
            SearchCriteriaProperties[] searchCriteriaPropertiesList = inputForTemplateResult2.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties searchCriteriaProperties in searchCriteriaPropertiesList)
            {
                if (searchCriteriaProperties.Description == "Costc" && !string.IsNullOrWhiteSpace(cc))
                {
                    searchCriteriaProperties.FromValue = cc;
                    ViewBag.PageTitle += " - Budget Holder: " + cc + " - " + cname;
                }
                if (searchCriteriaProperties.Description == "Entity" && !string.IsNullOrWhiteSpace(et))
                {
                    searchCriteriaProperties.FromValue = et;
                    ViewBag.PageTitle += " - Entity: " + et + " - " + cname;
                }
                if (searchCriteriaProperties.Description == "Mainproj" && !string.IsNullOrWhiteSpace(crp))
                {
                    if (crp != "0")
                    {
                        searchCriteriaProperties.FromValue = crp + "*";
                    }

                    ViewBag.PageTitle += " - Main Project: CRP " + cname + "-" + crp;
                }
                if (searchCriteriaProperties.Description == "Mainproj" && !string.IsNullOrWhiteSpace(crp) && crp != "0")
                {
                    searchCriteriaProperties.FromValue = "cso" + crp + "*";
                }
                if (searchCriteriaProperties.Description == "Period")
                {
                    searchCriteriaProperties.FromValue = Session["speriod"].ToString();
                    searchCriteriaProperties.ToValue = Session["eperiod"].ToString();
                }
            }
            TemplateResultAsDataSet templateResultAsDataSet = queryEngineV200606DotNet.GetTemplateResultAsDataSet(inputForTemplateResult2, credentials);
            DataSet templateResult = templateResultAsDataSet.TemplateResult;
            DataTable source = templateResult.Tables[0];
            double num2 = 0.0;
            string text4 = period;
            string text5 = "";
            double num3 = 0.0;
            double num4 = 0.0;
            string text6 = "";
            string text7 = "";
            double num5 = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;
            DataTable dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("Project");
            dataTable.Columns.Add("CRP Output/Units");
            dataTable.Columns.Add("Budget");
            dataTable.Columns.Add("Commitments");
            dataTable.Columns.Add("Actuals");
            dataTable.Columns.Add("Total Expenditure");
            dataTable.Columns.Add("Available");
            dataTable.Columns.Add("% Utilized");
            dynamic obj = null;
            obj = ((!flag) ? ((object)(from row in source.AsEnumerable()
                                       where row.Field<string>("xr0dim3") != ""
                                       select new
                                       {
                                           r0dim3 = row.Field<string>("r0dim3"),
                                           xr0dim3 = row.Field<string>("xr0dim3"),
                                           ple_amount = row.Field<double>("ple_amount"),
                                           amount = row.Field<double>("amount"),
                                           co_rest_amount = row.Field<double>("co_rest_amount"),
                                           actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                                           available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                                       })) : ((object)(from row in source.AsEnumerable()
                                                       where row.Field<string>("xr0dim3") != ""
                                                       select new
                                                       {
                                                           r0dim5 = row.Field<string>("dim5"),
                                                           r0dim3 = row.Field<string>("r0dim3"),
                                                           xr0dim3 = row.Field<string>("xr0dim3"),
                                                           ple_amount = row.Field<double>("ple_amount"),
                                                           amount = row.Field<double>("amount"),
                                                           co_rest_amount = row.Field<double>("co_rest_amount"),
                                                           actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                                                           available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                                                       })));
            foreach (dynamic item in obj)
            {
                if (item.xr0dim3 != null)
                {
                    num5 = (double)(num5 + item.actuals);
                    num6 = (double)(num6 + item.ple_amount);
                    num7 = (double)(num7 + item.co_rest_amount);
                }
                num3 = item.ple_amount;
                if (num3 > 0.0)
                {
                    num4 = item.actuals / num3 * 100;
                    num4 = Math.Round(num4, 2);
                }
                else
                {
                    num4 = 0.0;
                }
                text7 = ((num4 <= 0.0 || num4 > 100.0) ? "danger" : ((!(num4 > 0.0) || !(num4 < 50.0)) ? "success" : "warning"));
                string text8 = "text-right";
                string text9 = "text-right";
                string text10 = "text-right";
                text8 = ((!((item.amount < 0) ? true : false)) ? "text-right" : "danger");
                text9 = ((!((item.actuals < 0) ? true : false)) ? "text-right" : "danger");
                text10 = ((!((item.available < 0) ? true : false)) ? "text-right" : "danger");
                num2 = (double)(num2 + item.amount);
                text6 = item.r0dim3;
                if (!string.IsNullOrWhiteSpace(text6) && text6.Length > 3 && text6.Substring(0, 3) == "CSO")
                {
                    text6 = text6.Substring(3, text6.Length - 3);
                }
                if (!string.IsNullOrWhiteSpace(item.r0dim3) && item.r0dim3.Length > 4 && item.r0dim3.Substring(0, 4) == "NCSO")
                {
                    text6 = item.r0dim3.Substring(4, item.r0dim3.Length - 4);
                }
                string text11 = "";
                text11 = ((!((item.xr0dim3 != null) ? true : false)) ? "Total" : ((string)item.xr0dim3));
                text = ((!flag) ? ((string)(text + ("<tr><td class='numeric'><a href='ccode?cname=" + text11 + "&entity=" + base.Request.QueryString["entity"] + "&cc=" + cc + "&et=" + et + "&projectcode=" + item.r0dim3 + "'>" + text11 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text8 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text7 + "'>" + num4 + "%</td></tr>"))) : ((string)(text + ("<tr><td class='numeric'><a href='ccode?cname=" + text11 + "&entity=" + base.Request.QueryString["entity"] + "&cc=" + cc + "&et=" + et + "&projectcode=" + item.r0dim3 + "'>" + text11 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text8 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text7 + "'>" + num4 + "%</td></tr>"))));
                DataRow dataRow = dataTable.NewRow();
                dataRow["Project"] = (object)item.xr0dim3;
                dataRow["CRP Output/Units"] = text6;
                dataRow["Budget"] = (object)string.Format("{0:n0}", item.ple_amount);
                dataRow["Actuals"] = (object)string.Format("{0:n0}", item.amount);
                dataRow["Commitments"] = (object)string.Format("{0:n0}", item.co_rest_amount);
                dataRow["Total Expenditure"] = (object)string.Format("{0:n0}", item.actuals);
                dataRow["Available"] = (object)string.Format("{0:n0}", item.available);
                dataRow["% Utilized"] = num4;
                dataTable.Rows.Add(dataRow);
            }
            Session["dtl"] = dataTable;
            ViewBag.tdstr = text;
            jscript = jscript + " { y: 'Budget (A)', a: " + num6 + " },";
            jscript = jscript + " { y: 'Expenditures (B)', a: " + num5 + " },";
            jscript = jscript + " { y: 'Commitments (C)', a: " + num7 + " },";
            jscript = jscript + " { y: 'Total Expenditures (B+C)', a: " + (num5 + num7) + " },";
            jscript = jscript + " { y: 'Available Balance (A-B-C)', a: " + (num6 - num5 - num7) + " },";
            ViewBag.jscript = jscript;

            //end bind data

            //bind grants
            num = (!(crp != "")) ? 1966 : 1994;
            flag = false;
            if (!string.IsNullOrWhiteSpace(et))
            {
                text2 = et;
                if (text2.IndexOf(",") > 0)
                {
                    num = 2102;
                    flag = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(cc))
            {
                text3 = cc;
                if (text3.IndexOf(",") > 0)
                {
                    num = 2014;
                    flag = true;
                }
            }
            if (base.Request.QueryString["projectcode"] == "NCSOBOT1")
            {
                num = 2483;
            }
            if (string.IsNullOrWhiteSpace(et) && string.IsNullOrWhiteSpace(cc) && string.IsNullOrWhiteSpace(crp))
            {
                ViewBag.PageTitle += " - Main Projects: All";
            }
            queryEngineV200606DotNet = new QueryEngineV201101();
            searchCriteria = queryEngineV200606DotNet.GetSearchCriteria(num, true, credentials);
            templateResultOptions = queryEngineV200606DotNet.GetTemplateResultOptions(credentials);
            templateResultOptions.RemoveHiddenColumns = true;
            inputForTemplateResult2.TemplateResultOptions = templateResultOptions;
            inputForTemplateResult2.TemplateId = num;
            inputForTemplateResult2.SearchCriteriaPropertiesList = searchCriteria.SearchCriteriaPropertiesList;
            searchCriteriaPropertiesList = inputForTemplateResult2.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties searchCriteriaProperties in searchCriteriaPropertiesList)
            {
                if (searchCriteriaProperties.Description == "Costc" && !string.IsNullOrWhiteSpace(cc))
                {
                    searchCriteriaProperties.FromValue = cc;
                    ViewBag.PageTitle += " - Budget Holder: " + cc + " - " + cname;
                }
                if (searchCriteriaProperties.Description == "Entity" && !string.IsNullOrWhiteSpace(et))
                {
                    searchCriteriaProperties.FromValue = et;
                    ViewBag.PageTitle += " - Entity: " + et + " - " + cname;
                }
                if (searchCriteriaProperties.Description == "Mainproj" && !string.IsNullOrWhiteSpace(crp))
                {
                    if (crp != "0")
                    {
                        searchCriteriaProperties.FromValue = crp + "*";
                    }
                    ViewBag.PageTitle += " - Main Project: CRP " + cname + "-" + crp;
                }
                if (searchCriteriaProperties.Description == "Mainproj" && !string.IsNullOrWhiteSpace(crp) && crp != "0")
                {
                    searchCriteriaProperties.FromValue = "cso" + crp + "*";
                }
                if (searchCriteriaProperties.Description == "Period")
                {
                    searchCriteriaProperties.FromValue = Session["speriod"].ToString();
                    searchCriteriaProperties.ToValue = Session["eperiod"].ToString();
                }
            }
            templateResultAsDataSet = queryEngineV200606DotNet.GetTemplateResultAsDataSet(inputForTemplateResult2, credentials);
            templateResult = templateResultAsDataSet.TemplateResult;
            source = templateResult.Tables[0];
            num2 = 0.0;
            text4 = period;
            text5 = "";
            num3 = 0.0;
            num4 = 0.0;
            text6 = "";
            text7 = "";
            num5 = 0.0;
            num6 = 0.0;
            num7 = 0.0;
            dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("Grant Agreement");
            dataTable.Columns.Add("Budget");
            dataTable.Columns.Add("Commitments");
            dataTable.Columns.Add("Actuals");
            dataTable.Columns.Add("Total Expenditure");
            dataTable.Columns.Add("Available");
            dataTable.Columns.Add("% Utilized");
            obj = null;
            obj = ((!flag) ? ((object)(from row in source.AsEnumerable()
                                       where row.Field<string>("xr0dim4") != ""
                                       select new
                                       {
                                           r0dim4 = row.Field<string>("r0dim4"),
                                           xr0dim4 = row.Field<string>("xr0dim4"),
                                           ple_amount = row.Field<double>("ple_amount"),
                                           amount = row.Field<double>("amount"),
                                           co_rest_amount = row.Field<double>("co_rest_amount"),
                                           actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                                           available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                                       })) : ((object)(from row in source.AsEnumerable()
                                                       where row.Field<string>("xr0dim3") != ""
                                                       select new
                                                       {
                                                           r0dim5 = row.Field<string>("dim5"),
                                                           xdim5 = row.Field<string>("xdim5"),
                                                           r0dim4 = row.Field<string>("r0dim4"),
                                                           xr0dim4 = row.Field<string>("xr0dim3"),
                                                           ple_amount = row.Field<double>("ple_amount"),
                                                           amount = row.Field<double>("amount"),
                                                           co_rest_amount = row.Field<double>("co_rest_amount"),
                                                           actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                                                           available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                                                       })));
            foreach (dynamic item in obj)
            {
                if (item.xr0dim4 != null)
                {
                    num5 = (double)(num5 + item.actuals);
                    num6 = (double)(num6 + item.ple_amount);
                    num7 = (double)(num7 + item.co_rest_amount);
                }
                num3 = item.ple_amount;
                if (num3 > 0.0)
                {
                    num4 = item.actuals / num3 * 100;
                    num4 = Math.Round(num4, 2);
                }
                else
                {
                    num4 = 0.0;
                }
                text7 = ((!(num4 <= 0.0) && !(num4 > 100.0)) ? ((!(num4 > 0.0) || !(num4 < 50.0)) ? "success" : "warning") : "danger");
                string text8 = "text-right";
                string text9 = "text-right";
                string text10 = "text-right";
                text8 = ((!((item.amount < 0) ? true : false)) ? "text-right" : "danger");
                text9 = ((!((item.actuals < 0) ? true : false)) ? "text-right" : "danger");
                text10 = ((!((item.available < 0) ? true : false)) ? "text-right" : "danger");
                num2 = (double)(num2 + item.amount);
                string text11 = "";
                text11 = ((!((item.xr0dim4 != null) ? true : false)) ? "Total" : ((string)item.xr0dim4));
                text = ((!flag) ? ((string)(text + ("<tr><td class='numeric'><a href='ccode?cname=" + text11 + "&entity=" + base.Request.QueryString["entity"] + "&cc=" + cc + "&et=" + et + "&grantcode=" + item.r0dim4 + "'>" + text11 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text8 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text7 + "'>" + num4 + "%</td></tr>"))) : ((string)(text + ("<tr><td class='numeric'><a href='ccode?multi=yes&cname=" + text11 + "&entity=" + item.xdim5 + "&cc=" + cc + "&et=" + item.r0dim5 + "&grantcode=" + item.r0dim4 + "'>" + text11 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text8 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text7 + "'>" + num4 + "%</td></tr>"))));
                DataRow dataRow = dataTable.NewRow();
                dataRow["Grant Agreement"] = (object)item.xr0dim4;
                dataRow["Budget"] = (object)string.Format("{0:n0}", item.ple_amount);
                dataRow["Actuals"] = (object)string.Format("{0:n0}", item.amount);
                dataRow["Commitments"] = (object)string.Format("{0:n0}", item.co_rest_amount);
                dataRow["Total Expenditure"] = (object)string.Format("{0:n0}", item.actuals);
                dataRow["Available"] = (object)string.Format("{0:n0}", item.available);
                dataRow["% Utilized"] = num4;
                dataTable.Rows.Add(dataRow);
            }
            Session["gtl"] = dataTable;
            Session["gdstr"] = text;

            //end bind grants
            //return View();
        }

        public void Mreports(string cc, string projectcode, string cname)
        {

        }
        public void Ccode(string cc, string projectcode, string cname)
        {

        }
        public void Regions(string rid)
        {

        }
    }
}