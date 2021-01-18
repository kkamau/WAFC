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
using System.Web.UI.WebControls;

namespace WAFC.Reports.Controllers
{
    // [Authorize(Roles = "*")]
    public class HomeController : Controller
    {


        [HttpPost]
        public ActionResult Index(string syear)
        {

            List<SelectListItem> finyears = new List<SelectListItem>();

            string fyear = Request.Form["Year"];
            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                finyears.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            Session["speriod"] = fyear + "01";
            Session["eperiod"] = fyear + "12";
            Session["syear"] = fyear;

            var selected = finyears.Where(x => x.Value == fyear).First();
            selected.Selected = true;

            var model = new IndexReportViewModel
            {

                FinYears = finyears
            };

            return View(model);
        }

        public ActionResult Index()
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

            if (Session["speriod"] != null)
            {
                string speriod = Session["speriod"].ToString();
                speriod = speriod.Substring(0, 4);
                var selected = finyears.Where(x => x.Value == speriod).First();
                selected.Selected = true;
                Session["syear"] = speriod;
            }
            else
            {
                var selected = finyears.Where(x => x.Value == DateTime.Now.Year.ToString()).First();
                selected.Selected = true;
                Session["speriod"] = DateTime.Now.Year.ToString() + "01";
                Session["eperiod"] = DateTime.Now.Year.ToString() + "12";
            }


            var model = new IndexReportViewModel
            {

                FinYears = finyears
            };

            return View(model);
        }

        public ActionResult CRP(string cc, string et, string cname, string crp, string entity, string projectcode, string period)
        {
            QueryEngineV201101 QueryEngineCIP;
            string jscript = "";

            WSCredentials credentials = GetUserCredentials();
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
                text = ((!flag) ? ((string)(text + ("<tr><td class='numeric'><a href='CCode?cname=" + text11 + "&entity=" + entity + "&cc=" + cc + "&et=" + et + "&projectcode=" + item.r0dim3 + "'>" + text11 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text8 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text7 + "'>" + num4 + "%</td></tr>"))) : ((string)(text + ("<tr><td class='numeric'><a href='CCode?cname=" + text11 + "&entity=" + entity + "&cc=" + cc + "&et=" + et + "&projectcode=" + item.r0dim3 + "'>" + text11 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text8 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text7 + "'>" + num4 + "%</td></tr>"))));
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
            ViewBag.gdstr = text;
            jscript = jscript + " { y: 'Budget (A)', a: " + num6 + " },";
            jscript = jscript + " { y: 'Expenditures (B)', a: " + num5 + " },";
            jscript = jscript + " { y: 'Commitments (C)', a: " + num7 + " },";
            jscript = jscript + " { y: 'Total Expenditures (B+C)', a: " + (num5 + num7) + " },";
            jscript = jscript + " { y: 'Available Balance (A-B-C)', a: " + (num6 - num5 - num7) + " },";
            ViewBag.jscript = jscript;

            //end bind data

            //bind grants
            text = string.Empty;
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
            if (projectcode == "NCSOBOT1")
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
                text = ((!flag) ? ((string)(text + ("<tr><td class='numeric'><a href='CCode?cname=" + text11 + "&entity=" + entity + "&cc=" + cc + "&et=" + et + "&grantcode=" + item.r0dim4 + "'>" + text11 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text8 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text7 + "'>" + num4 + "%</td></tr>"))) : ((string)(text + ("<tr><td class='numeric'><a href='CCode?multi=yes&cname=" + text11 + "&entity=" + item.xdim5 + "&cc=" + cc + "&et=" + item.r0dim5 + "&grantcode=" + item.r0dim4 + "'>" + text11 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text8 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text7 + "'>" + num4 + "%</td></tr>"))));
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
            ViewBag.tdstr = text;

            //end bind grants
            return View();
        }

        public ActionResult Mprojects(string cc, string projectcode, string cname)
        {
            QueryEngineV201101 QueryEngineCIP;
            string trtemp = "", jscript = "";
            WSCredentials credentials;
            credentials = GetUserCredentials();
            DataSet ds;

            TemplateList cust = new TemplateList();
            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            //TemplateResultAsDataSet TemplateResultHRDATA = new TemplateResultAsDataSet();
            //TemplateResultOptions TemplateResultsOptionsHR = QueryEngineCIP.GetTemplateResultOptions(GetUserCredentials());
            //new TemplateResultOptions();
            //InputforHRDATAResult. = "p1_vHrData";

            int templateID = 2014; // MultiMainProjects_FINWEBQEWS 


            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;

            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                if (scp.Description == "Costc" && !string.IsNullOrWhiteSpace(cc))
                {
                    //scp.ToValue = cc;
                    scp.FromValue = cc;// CCode2;
                    ViewBag.PageTitle = "Budget Holder: " + cc + scp.RelDateCrit.ToString() + " - " + cname;
                }


                if (scp.Description == "Period")
                {
                    scp.FromValue = Session["speriod"].ToString();
                    scp.ToValue = Session["eperiod"].ToString(); ;
                }
                //if (scp.Description == "Mainproj" && Request.QueryString["mj"] != "")
                //{
                //    scp.ToValue = Request.QueryString["mj"];
                //    scp.FromValue = Request.QueryString["mj"];
                //}

                // Response.Write(scp.ColumnName + "=" + scp.Description + "**");
            }
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //crpgrdvw.DataSource = ds;
            //crpgrdvw.DataBind();
            DataTable dt = ds.Tables[0];
            string tempstr, lstr;
            double totals = 0;


            string fstr = "";
            double budget = 0, imprate = 0;
            string pcode = "";
            string sclass = "";
            double tactuals = 0, tbudget = 0, tcommitments = 0;

            // var result = null;
            DataTable dtl = new DataTable();

            dtl.Clear();
            dtl.Columns.Add("Project");
            dtl.Columns.Add("ProjectCode");
            dtl.Columns.Add("Budget");
            dtl.Columns.Add("Commitments");
            dtl.Columns.Add("Actuals");
            dtl.Columns.Add("Total Expenditure");
            dtl.Columns.Add("Available");
            dtl.Columns.Add("% Utilized");
            DataRow dr;//= dt.NewRow();

            var result = from row in dt.AsEnumerable()
                         where row.Field<string>("r0dim3") != ""
                         select new
                         {
                             r0dim3 = row.Field<string>("r0dim3"),
                             xr0dim3 = row.Field<string>("xr0dim3"),
                             ple_amount = row.Field<double>("ple_amount"),
                             amount = row.Field<double>("amount"),
                             co_rest_amount = row.Field<double>("co_rest_amount"),
                             actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                             available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                         };
            foreach (var t in result)
            {
                if (t.xr0dim3 != null)
                {
                    tactuals = t.actuals;
                    tbudget = t.ple_amount;
                    tcommitments = t.co_rest_amount;
                }
                if (budget > 0)
                {
                    imprate = (t.actuals / budget) * 100;
                    imprate = Math.Round(imprate, 2);
                }
                else
                    imprate = 0;

                if (imprate <= 0 || imprate > 100)
                    sclass = "danger";
                else if (imprate > 0 && imprate < 50)
                    sclass = "warning";
                else
                    sclass = "success";

                totals += t.amount;
                pcode = t.r0dim3;
                //if (!String.IsNullOrWhiteSpace(pcode) && pcode.Length > 3 && pcode.Substring(0, 3) == "CSO")
                //{
                //    pcode = pcode.Substring(3, pcode.Length - 3);
                //}

                //if (!String.IsNullOrWhiteSpace(t.r0dim3) && t.r0dim3.Length > 4 && t.r0dim3.Substring(0, 4) == "NCSO")
                //{
                //    pcode = t.r0dim3.Substring(4, t.r0dim3.Length - 4);
                //}
                string stitle = "";
                if (t.xr0dim3 != null)
                    stitle = pcode + "-" + t.xr0dim3;
                else
                    stitle = "Total";

                ViewBag.tdstr += "<tr><td class='numeric'><a href='CCode?projectcode=" + t.r0dim3 + "'>" + stitle + "</a></td><td class='text-right'>" + String.Format("{0:n0}", t.ple_amount) + "</td><td class='text-right'>" + String.Format("{0:n0}", t.amount) + "</td><td class='text-right'>" + String.Format("{0:n0}", t.co_rest_amount) + "</td><td class='text-right'>" + String.Format("{0:n0}", t.actuals) + "</td><td class='text-right'>" + String.Format("{0:n0}", t.available) + "</td><td class='" + sclass + "'>" + imprate + "%</td></tr>";
                dr = dtl.NewRow();
                dr["Project"] = t.xr0dim3;
                dr["ProjectCode"] = pcode;
                dr["Budget"] = String.Format("{0:n0}", t.ple_amount);
                dr["Actuals"] = String.Format("{0:n0}", t.amount);
                dr["Commitments"] = String.Format("{0:n0}", t.co_rest_amount);
                dr["Total Expenditure"] = String.Format("{0:n0}", t.actuals);
                dr["Available"] = String.Format("{0:n0}", t.available);
                dr["% Utilized"] = imprate;
                dtl.Rows.Add(dr);
            }
            Session["dtl"] = dtl;
            ViewBag.tdstr = ViewBag.tdstr;
            jscript = " { y: 'Actuals', a: " + tactuals + " },";
            jscript += " { y: 'Commitments', a: " + tcommitments + " },";
            jscript += " { y: 'Total Expenditures', a: " + (tactuals + tcommitments) + " },";
            jscript += " { y: 'Budget', a: " + tbudget + " },";
            jscript += " { y: 'Available Balance', a: " + (tbudget - tactuals - tcommitments) + " },";
            return View();
        }
        public ActionResult CCode(string cc, string projectcode, string cname, string grantcode, string regionid, string et, string multi, string period)
        {
            QueryEngineV201101 QueryEngineCIP;
            string trtemp = "", jscript = "";

            WSCredentials credentials = GetUserCredentials();
            TemplateList templateList = new TemplateList();
            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult inputForTemplateResult = new InputForTemplateResult();
            int num = 0;
            DataSet ds;

            string text = regionid;
            if (!string.IsNullOrWhiteSpace(projectcode))
            {
                ViewBag.PageTitle = "Region: " + text;
            }
            switch (text)
            {
                case "SEA":
                    num = 1969;
                    break;
                case "ECA":
                    num = 1981;
                    break;
                case "ESA":
                    num = 1975;
                    break;
                case "SA":
                    num = 3833;
                    break;
                case "WCA":
                    num = 1973;
                    break;
                case "LA":
                    num = 1977;
                    break;
                default:
                    num = 1965;
                    break;
            }
            if (!string.IsNullOrWhiteSpace(multi))
            {
                num = 2459;
            }
            QueryEngineV201101 queryEngineV200606DotNet = new QueryEngineV201101();
            SearchCriteria searchCriteria = queryEngineV200606DotNet.GetSearchCriteria(num, true, credentials);
            InputForTemplateResult inputForTemplateResult2 = new InputForTemplateResult();
            TemplateResultOptions templateResultOptions = queryEngineV200606DotNet.GetTemplateResultOptions(credentials);
            templateResultOptions.RemoveHiddenColumns = true;
            inputForTemplateResult2.TemplateResultOptions = templateResultOptions;
            inputForTemplateResult2.TemplateId = num;
            inputForTemplateResult2.SearchCriteriaPropertiesList = searchCriteria.SearchCriteriaPropertiesList;
            if (!string.IsNullOrWhiteSpace(projectcode))
            {
                ViewBag.PageTitle = "Main Project: " + projectcode + " - " + cname;
            }
            else if (!string.IsNullOrWhiteSpace(grantcode))
            {
                ViewBag.PageTitle = "Grant Agreement: " + grantcode + " - " + cname;
            }
            SearchCriteriaProperties[] searchCriteriaPropertiesList = inputForTemplateResult2.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties searchCriteriaProperties in searchCriteriaPropertiesList)
            {
                if (searchCriteriaProperties.Description == "Mainproj" && !string.IsNullOrWhiteSpace(projectcode))
                {
                    searchCriteriaProperties.ToValue = projectcode;
                    searchCriteriaProperties.FromValue = projectcode;
                }
                if (searchCriteriaProperties.ColumnName == "r0dim4" && !string.IsNullOrWhiteSpace(grantcode))
                {
                    searchCriteriaProperties.ToValue = grantcode;
                    searchCriteriaProperties.FromValue = grantcode;
                }
                if (searchCriteriaProperties.Description == "Costc" && !string.IsNullOrWhiteSpace(cc))
                {
                    searchCriteriaProperties.ToValue = cc;
                    searchCriteriaProperties.FromValue = cc;
                }
                if (searchCriteriaProperties.Description == "Entity" && !string.IsNullOrWhiteSpace(et))
                {
                    searchCriteriaProperties.ToValue = et;
                    searchCriteriaProperties.FromValue = et;
                }
                if (searchCriteriaProperties.Description == "Period")
                {
                    searchCriteriaProperties.FromValue = Session["speriod"].ToString();
                    searchCriteriaProperties.ToValue = Session["eperiod"].ToString();
                }
            }
            TemplateResultAsDataSet templateResultAsDataSet = queryEngineV200606DotNet.GetTemplateResultAsDataSet(inputForTemplateResult2, credentials);
            ds = templateResultAsDataSet.TemplateResult;
            Session["ds"] = ds;
            DataTable dataTable = ds.Tables[0];
            Session["dt"] = dataTable;
            double num2 = 0.0;
            string text2 = period;
            string text3 = "";
            double num3 = 0.0;
            double num4 = 0.0;
            string text4 = "";
            string text5 = "";
            double num5 = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;
            string text6 = "";
            DataTable dataTable2 = new DataTable();
            dataTable2.Clear();
            dataTable2.Columns.Add("ChargeCode");
            dataTable2.Columns.Add("Budget");
            dataTable2.Columns.Add("Commitments");
            dataTable2.Columns.Add("Actuals");
            dataTable2.Columns.Add("Total Expenditure");
            dataTable2.Columns.Add("Available");
            dataTable2.Columns.Add("% Utilized");
            var enumerableRowCollection = from row in dataTable.AsEnumerable()
                                          select new
                                          {
                                              xdim4 = row.Field<string>("xdim4"),
                                              f0_chargecodes1 = row.Field<string>("f0_chargecodes1"),
                                              dim4 = row.Field<string>("dim4"),
                                              dim5 = row.Field<string>("dim5"),
                                              ple_amount = row.Field<double>("ple_amount"),
                                              amount = row.Field<double>("amount"),
                                              co_rest_amount = row.Field<double>("co_rest_amount"),
                                              actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                                              available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                                          };
            foreach (var item in enumerableRowCollection)
            {
                if (item.f0_chargecodes1 != null)
                {
                    num5 += item.actuals;
                    num6 += item.ple_amount;
                    num7 += item.co_rest_amount;
                }
                num3 = item.ple_amount;
                if (num3 > 0.0)
                {
                    num4 = item.actuals / num3 * 100.0;
                    num4 = Math.Round(num4, 2);
                }
                else
                {
                    num4 = 0.0;
                }
                text5 = ((!(num4 <= 0.0) && !(num4 > 100.0)) ? ((!(num4 > 0.0) || !(num4 < 50.0)) ? "success" : "warning") : "danger");
                string text7 = "text-right";
                string text8 = "text-right";
                string text9 = "text-right";
                text7 = ((!(item.amount < 0.0)) ? "text-right" : "danger");
                text8 = ((!(item.actuals < 0.0)) ? "text-right" : "danger");
                text9 = ((!(item.available < 0.0)) ? "text-right" : "danger");
                string text10 = "";
                text10 = ((item.f0_chargecodes1 == null) ? "Total" : item.f0_chargecodes1);
                num2 += item.amount;
                ViewBag.tdstr = ViewBag.tdstr + "<tr><td class='numeric'><a href='AClass?worder=" + item.dim4 + "&entity=" + item.dim5 + "' class='tooltips tooltip-pink' data-toggle='tooltip' data-original-title='" + item.xdim4 + "' >" + text10 + "</a></td><td class='text-right'>" + $"{item.ple_amount:n0}" + "</td><td class='" + text7 + "'><span class='text-right'>" + $"{item.amount:n0}" + "</span></td><td class='text-right'>" + $"{item.co_rest_amount:n0}" + "</td><td class='" + text8 + "'><span class='text-right'>" + $"{item.actuals:n0}" + "</span></td><td class='" + text9 + "'><span class='text-right'>" + $"{item.available:n0}" + "</span></td><td class='" + text5 + "'>" + num4 + "%</td></tr>";
                DataRow dataRow = dataTable2.NewRow();
                dataRow["ChargeCode"] = item.f0_chargecodes1;
                dataRow["Budget"] = $"{item.ple_amount:n0}";
                dataRow["Actuals"] = $"{item.amount:n0}";
                dataRow["Commitments"] = $"{item.co_rest_amount:n0}";
                dataRow["Total Expenditure"] = $"{item.actuals:n0}";
                dataRow["Available"] = $"{item.available:n0}";
                dataRow["% Utilized"] = num4;
                dataTable2.Rows.Add(dataRow);
            }
            Session["dtl"] = dataTable2;
            ViewBag.tdstr = ViewBag.tdstr;
            text6 = text6 + " { y: 'Budget (A)', a: " + num6 + " },";
            text6 = text6 + " { y: 'Expenditures (B)', a: " + num5 + " },";
            text6 = text6 + " { y: 'Commitments (C)', a: " + num7 + " },";
            text6 = text6 + " { y: 'Total Expenditures (B+C)', a: " + (num5 + num7) + " },";
            text6 = text6 + " { y: 'Available Balance (A-B-C)', a: " + (num6 - num5 - num7) + " },";
            ViewBag.jscript = text6;
            return View();
        }
        public ActionResult AClass(string cc, string worder, string entity)
        {
            QueryEngineV201101 QueryEngineCIP;
            string trtemp = "";
            ViewBag.PageTitle = "Work Order: " + worder + " Entity: " + entity;
            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            //TemplateResultAsDataSet TemplateResultHRDATA = new TemplateResultAsDataSet();
            //TemplateResultOptions TemplateResultsOptionsHR = QueryEngineCIP.GetTemplateResultOptions(GetUserCredentials());
            //new TemplateResultOptions();
            //InputforHRDATAResult. = "p1_vHrData";

            int templateID = 1968;//PerAccountClass_FINWEBQEWS
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                if (scp.Description == "Workord")
                {
                    scp.ToValue = worder;
                    scp.FromValue = worder;

                }
                if (scp.Description == "Entity")
                {
                    scp.ToValue = entity;
                    scp.FromValue = entity;

                }
                if (scp.Description == "Period")
                {
                    scp.FromValue = Session["speriod"].ToString();
                    scp.ToValue = Session["eperiod"].ToString(); ;
                }
                //Response.Write(scp.ColumnName + "=" + scp.Description + "**");
            }
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //crpgrdvw.DataSource = ds;
            //crpgrdvw.DataBind();
            DataTable dt = ds.Tables[0];
            double totals = 0;
            double budget = 0, imprate = 0;
            string sclass = "";
            // var result = null;
            DataTable dtl = new DataTable();

            dtl.Clear();
            dtl.Columns.Add("Account");
            dtl.Columns.Add("Budget");
            dtl.Columns.Add("Commitments");
            dtl.Columns.Add("Actuals");
            dtl.Columns.Add("Total Expenditure");
            dtl.Columns.Add("Available");
            dtl.Columns.Add("% Utilized");
            DataRow dr;//= dt.NewRow();

            var result = from row in dt.AsEnumerable()
                             //where row.Field<string>("r0dim3") != ""
                         select new
                         {
                             account_gr_1 = row.Field<string>("account_grp__1"),
                             xaccount_gr_1 = row.Field<string>("xaccount_grp__1"),
                             //dim4 = row.Field<string>("dim4"),
                             //dim5 = row.Field<string>("dim5"),
                             ple_amount = row.Field<double>("ple_amount"),
                             amount = row.Field<double>("amount"),
                             co_rest_amount = row.Field<double>("co_rest_amount"),
                             actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                             available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                         };
            foreach (var t in result)
            {
                budget = t.ple_amount;
                if (budget > 0)
                {
                    imprate = (t.actuals / budget) * 100;
                    imprate = Math.Round(imprate, 2);
                }
                else
                    imprate = 0;

                if (imprate <= 0 || imprate > 100)
                    sclass = "danger";
                else if (imprate > 0 && imprate < 50)
                    sclass = "warning";
                else
                    sclass = "success";
                string aclass = "text-right";
                string tclass = "text-right";
                string vclass = "text-right";
                if (t.amount < 0)
                    aclass = "danger";
                else
                    aclass = "text-right";
                if (t.actuals < 0)
                    tclass = "danger";
                else
                    tclass = "text-right";
                if (t.available < 0)
                    vclass = "danger";
                else
                    vclass = "text-right";
                totals += t.amount;
                ViewBag.tdstr += "<tr><td class='numeric'><a href='Accounts?worder=" + worder + "&entity=" + entity + "&account=" + t.account_gr_1 + "&accountdesc=" + t.xaccount_gr_1 + "'>" + t.xaccount_gr_1 + "</a></td><td class='text-right'>" + String.Format("{0:n0}", t.ple_amount) + "</td><td class='" + aclass + "'>" + String.Format("{0:n0}", t.amount) + "</td><td class='text-right'>" + String.Format("{0:n0}", t.co_rest_amount) + "</td><td class='" + tclass + "'>" + String.Format("{0:n0}", t.actuals) + "</td><td class='" + vclass + "'>" + String.Format("{0:n0}", t.available) + "</td><td class='" + sclass + "'>" + imprate + "%</td></tr>";
                dr = dtl.NewRow();
                dr["Account"] = t.xaccount_gr_1;
                dr["Budget"] = String.Format("{0:n0}", t.ple_amount);
                dr["Actuals"] = String.Format("{0:n0}", t.amount);
                dr["Commitments"] = String.Format("{0:n0}", t.co_rest_amount);
                dr["Total Expenditure"] = String.Format("{0:n0}", t.actuals);
                dr["Available"] = String.Format("{0:n0}", t.available);
                dr["% Utilized"] = imprate;
                dtl.Rows.Add(dr);
            }
            Session["dtl"] = dtl;
            ViewBag.tdstr = ViewBag.tdstr;
            return View();

        }
        public ActionResult Regions(string rid, string et, string cc, string crp, string entity, string period)
        {
            QueryEngineV201101 QueryEngineCIP;
            string trtemp = "";
            ViewBag.PageTitle = "Region: " + rid;

            //bind grants
            WSCredentials credentials;
            credentials = GetUserCredentials();// GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            //TemplateResultAsDataSet TemplateResultHRDATA = new TemplateResultAsDataSet();
            //TemplateResultOptions TemplateResultsOptionsHR = QueryEngineCIP.GetTemplateResultOptions(GetUserCredentials());
            //new TemplateResultOptions();
            //InputforHRDATAResult. = "p1_vHrData";


            ViewBag.PageTitle = "Budget and Expenditure";
            int templateID = 1966; // MainProjects_FINWEBQEWS 
            string regionid = rid;

            switch (regionid)
            {
                case "SEA":
                    templateID = 1970;// SEA_FINWEBQEWS;
                    break;
                case "ECA":
                    templateID = 1980;// ECA_FINWEBQEWS
                    break;
                case "ESA":
                    templateID = 1974;// ESA_FINWEBQEWS
                    break;
                case "SA":
                    templateID = 1978;// SA_FINWEBQEWS
                    break;
                case "WCA":
                    templateID = 1972;// WCA_FINWEBQEWS
                    break;
                case "LA":
                    templateID = 1976;// LA_FINWEBQEWS
                    break;
                default:
                    templateID = 1966;// MainProjects_FINWEBQEWS
                    break;
            }

            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                if (scp.Description == "Period")
                {
                    scp.FromValue = Session["speriod"].ToString();
                    scp.ToValue = Session["eperiod"].ToString(); ;
                }
                //Response.Write(scp.ColumnName + "=" + scp.Description + "**");
            }
            //Response.End();
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //crpgrdvw.DataSource = ds;
            //crpgrdvw.DataBind();
            DataTable dt = ds.Tables[0];
            string tempstr, lstr;
            double totals = 0;

            string querystr = period;
            string fstr = "";
            double budget = 0, imprate = 0;
            string pcode = "";
            string sclass = "";

            // var result = null;

            DataTable dtl = new DataTable();

            dtl.Clear();
            dtl.Columns.Add("Output\\Task");
            dtl.Columns.Add("Budget");
            dtl.Columns.Add("Commitments");
            dtl.Columns.Add("Actuals");
            dtl.Columns.Add("Total Expenditure");
            dtl.Columns.Add("Available");
            dtl.Columns.Add("% Utilized");
            DataRow dr;//= dt.NewRow();
            var result = from row in dt.AsEnumerable()
                         where row.Field<string>("r0dim3") != ""
                         select new
                         {
                             r0dim3 = row.Field<string>("r0dim3"),
                             xr0dim3 = row.Field<string>("r0dim3"),
                             ple_amount = row.Field<double>("ple_amount"),
                             amount = row.Field<double>("amount"),
                             co_rest_amount = row.Field<double>("co_rest_amount"),
                             actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                             available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                         };
            foreach (var t in result)
            {
                budget = t.ple_amount;
                if (budget > 0)
                {
                    imprate = (t.actuals / budget) * 100;
                    imprate = Math.Round(imprate, 2);
                }
                else
                    imprate = 0;

                if (imprate <= 0 || imprate > 100)
                    sclass = "danger";
                else if (imprate > 0 && imprate < 50)
                    sclass = "warning";
                else
                    sclass = "success";

                totals += t.amount;
                pcode = t.r0dim3;
                if (!String.IsNullOrWhiteSpace(pcode) && pcode.Length > 3 && pcode.Substring(0, 3) == "CSO")
                {
                    pcode = pcode.Substring(3, pcode.Length - 3);
                }

                if (!String.IsNullOrWhiteSpace(t.r0dim3) && t.r0dim3.Length > 4 && t.r0dim3.Substring(0, 4) == "NCSO")
                {
                    pcode = t.r0dim3.Substring(4, t.r0dim3.Length - 4);
                }
                string aclass = "text-right";
                string tclass = "text-right";
                string vclass = "text-right";
                if (t.amount < 0)
                    aclass = "danger";
                else
                    aclass = "text-right";
                if (t.actuals < 0)
                    tclass = "danger";
                else
                    tclass = "text-right";
                if (t.available < 0)
                    vclass = "danger";
                else
                    vclass = "text-right";

                ViewBag.tdstr += "<tr><td class='numeric'><a href='CCode?regionid=" + regionid + "&projectcode=" + t.r0dim3 + "'>" + t.xr0dim3 + "</a></td><td class='text-right'>" + String.Format("{0:n0}", t.ple_amount) + "</td><td class='" + aclass + "'>" + String.Format("{0:n0}", t.amount) + "</td><td class='text-right'>" + String.Format("{0:n0}", t.co_rest_amount) + "</td><td class='" + tclass + "'>" + String.Format("{0:n0}", t.actuals) + "</td><td class='" + vclass + "'>" + String.Format("{0:n0}", t.available) + "</td><td class='" + sclass + "'>" + imprate + "%</td></tr>";
                dr = dtl.NewRow();
                dr["Output\\Task"] = t.xr0dim3;
                dr["Budget"] = String.Format("{0:n0}", t.ple_amount);
                dr["Actuals"] = String.Format("{0:n0}", t.amount);
                dr["Commitments"] = String.Format("{0:n0}", t.co_rest_amount);
                dr["Total Expenditure"] = String.Format("{0:n0}", t.actuals);
                dr["Available"] = String.Format("{0:n0}", t.available);
                dr["% Utilized"] = imprate;
                dtl.Rows.Add(dr);
            }
            Session["dtl"] = dtl;
            ViewBag.tdstr = ViewBag.tdstr;
            //end bind crp

            //bind grant
            string text = "";
            string text2 = rid;
            int num;
            switch (text2)
            {
                case "SEA":
                    num = 2000;
                    break;
                case "ECA":
                    num = 2431;
                    break;
                case "ESA":
                    num = 2432;
                    break;
                case "SA":
                    num = 2434;
                    break;
                case "WCA":
                    num = 2435;
                    break;
                case "LA":
                    num = 2433;
                    break;
                default:
                    num = 1966;
                    break;
            }
            string text3 = "";
            string text4 = "";
            bool flag = false;
            if (!string.IsNullOrWhiteSpace(et))
            {
                text3 = et;
                if (text3.IndexOf(",") > 0)
                {
                    num = 2102;
                    flag = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(cc))
            {
                text4 = cc;
                if (text4.IndexOf(",") > 0)
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
            //SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);

            InputForTemplateResult inputForTemplateResult2 = new InputForTemplateResult();
            TemplateResultOptions templateResultOptions = queryEngineV200606DotNet.GetTemplateResultOptions(credentials);
            templateResultOptions.RemoveHiddenColumns = true;
            inputForTemplateResult2.TemplateResultOptions = templateResultOptions;
            inputForTemplateResult2.TemplateId = num;
            inputForTemplateResult2.SearchCriteriaPropertiesList = searchCriteria.SearchCriteriaPropertiesList;
            SearchCriteriaProperties[] searchCriteriaPropertiesList = inputForTemplateResult2.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties searchCriteriaProperties in searchCriteriaPropertiesList)
            {
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
            string text5 = period;
            string text6 = "";
            double num3 = 0.0;
            double num4 = 0.0;
            string text7 = "";
            string text8 = "";
            double num5 = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;
            DataTable dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("Grant Agreement");
            dataTable.Columns.Add("Budget");
            dataTable.Columns.Add("Commitments");
            dataTable.Columns.Add("Actuals");
            dataTable.Columns.Add("Total Expenditure");
            dataTable.Columns.Add("Available");
            dataTable.Columns.Add("% Utilized");
            dynamic obj = null;
            obj = ((!flag) ? (from row in source.AsEnumerable()
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
                              }) : (from row in source.AsEnumerable()
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
                                    }));
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
                text8 = ((!(num4 <= 0.0) && !(num4 > 100.0)) ? ((!(num4 > 0.0) || !(num4 < 50.0)) ? "success" : "warning") : "danger");
                string text9 = "text-right";
                string text10 = "text-right";
                string text11 = "text-right";
                text9 = ((!((item.amount < 0) ? true : false)) ? "text-right" : "danger");
                text10 = ((!((item.actuals < 0) ? true : false)) ? "text-right" : "danger");
                text11 = ((!((item.available < 0) ? true : false)) ? "text-right" : "danger");
                num2 = (double)(num2 + item.amount);
                string text12 = "";
                text12 = ((!((item.xr0dim4 != null) ? true : false)) ? "Total" : ((string)item.xr0dim4));
                text = ((!flag) ? ((string)(text + ("<tr><td class='numeric'><a href='CCode?regionid=" + text2 + "&cname=" + text12 + "&entity=" + entity + "&cc=" + cc + "&et=" + et + "&grantcode=" + item.r0dim4 + "'>" + text12 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text11 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text8 + "'>" + num4 + "%</td></tr>"))) : ((string)(text + ("<tr><td class='numeric'><a href='CCode?regionid=" + text2 + "&cname=" + text12 + "&entity=" + entity + "&cc=" + cc + "&et=" + item.r0dim5 + "&grantcode=" + item.r0dim4 + "'>" + text12 + "</a></td><td class='text-right'>" + string.Format("{0:n0}", item.ple_amount) + "</td><td class='" + text9 + "'>" + string.Format("{0:n0}", item.amount) + "</td><td class='text-right'>" + string.Format("{0:n0}", item.co_rest_amount) + "</td><td class='" + text10 + "'>" + string.Format("{0:n0}", item.actuals) + "</td><td class='" + text11 + "'>" + string.Format("{0:n0}", item.available) + "</td><td class='" + text8 + "'>" + num4 + "%</td></tr>"))));
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
            ViewBag.gdstr = text;
            //end bind grant
            return View();
        }

        public ActionResult Transactions(string worder, string entity, string accountdesc, string account)
        {
            BindTransactions(worder, entity, account);
            BindLedger(worder, entity, account);
            BindCommittments(worder, entity, account);
            return View();
        }
        public ActionResult StaffSummary(string expensetype)
        {
            QueryEngineV201101 QueryEngineCIP;

            string jscript = "", trtemp = "";
            string resource_id;
            double str30days, str60days, str90days, strTotal, str6090days;
            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            ViewBag.PageTitle = "My Staff Statement (" + System.Web.HttpContext.Current.User.Identity.Name.ToUpper().ToString() + ")";
            long templateID = 2115;// StaffStatement_QEWS
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            DateTime sdate = Convert.ToDateTime("01-Jan-" + DateTime.Today.Year.ToString());
            DateTime edate = DateTime.Today;
            string test = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            resource_id = getSuppID();

            //Response.Write(resource_id + "=");
            //Response.End();
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {

                if (scp.Description == "Expense Type")
                {
                    if (expensetype != "All" && expensetype != null)
                    {
                        scp.ToValue = expensetype;
                        scp.FromValue = expensetype;
                        //Response.Write(scp.ToValue);
                    }


                }

                if (scp.Description == "SuppID")
                {
                    scp.FromValue = resource_id;// Request.QueryString["SuppID"]; //"G12482";
                    Session["resource_id"] = resource_id;
                }
                //Response.Write(scp.ColumnName + "==");
                //example.Text = example.Text + "=" + scp.ColumnName + "=" + test + "=";
            }

            //else if (<..etc.. >;
            //Retrieve result
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //crpgrdvw.DataBind();
            DataTable dt = ds.Tables[0];
            string tempstr, lstr;
            double totals = 0;
            string labelstr = "", datastr = "";
            var result = from row in dt.AsEnumerable()
                         where row.Field<string>("xdim_6") != null
                         group row by row.Field<string>("xdim_6") into grp
                         orderby grp.Key
                         let row = grp.First()
                         select new
                         {
                             Id = grp.Key,
                             expensecode = row.Field<string>("dim_6"),
                             Sum = grp.Sum(r => r.Field<double>("amount"))
                         };

            foreach (var t in result)
            {
                if (t.Id == "")
                    tempstr = "undefined";
                else
                    tempstr = t.Id;
                totals += t.Sum;
                ViewBag.tdstr += "<tr><td class='numeric'><a href='StaffStatements?period=0&expensecode=" + t.expensecode + "&desc=" + Server.UrlEncode(tempstr) + "'>" + tempstr + "</a></td><td class='text-right'>" + String.Format("{0:n2}", t.Sum) + "</td></tr>";
                if (tempstr.Length > 10)
                {
                    lstr = tempstr.Substring(0, 10) + "..";
                }
                else
                    lstr = tempstr;

                labelstr += "'" + t.expensecode + "',";
                datastr += Math.Round(t.Sum, 0) + ",";
                jscript += " { y: '" + t.expensecode + ": " + lstr + "', a: " + Math.Round(t.Sum, 0) + " },";
                tempstr = "";
            }
            ViewBag.tdstr += "<tr><td class='numeric'><strong><a href='StaffStatements?desc=All'>Total</a></strong></td><td class='text-right'><strong>" + String.Format("{0:n2}", Math.Round(totals, 0)) + "</strong></td></tr>";
            jscript += " { y: 'Total', a: " + Math.Round(totals, 0) + " },";
            ViewBag.jscript = jscript;
            labelstr += "'Total'";
            datastr += Math.Round(totals, 0);
            //labelstr = labelstr.Substring(0, labelstr.Length - 1);
            //datastr = datastr.Substring(0, datastr.Length - 1);
            Session["labelstr"] = labelstr;
            Session["datastr"] = datastr;

            str30days = dt
                        .AsEnumerable()
                        .Where(r => (r.Field<DateTime?>("voucher_date").HasValue) && (DateTime.Now - r.Field<DateTime?>("voucher_date").Value.Date).Days <= 30)
                        .Sum(r => (Double)r["amount"]);

            str60days = dt
                      .AsEnumerable()
                      .Where(r => r.Field<DateTime?>("voucher_date").HasValue && (DateTime.Now - r.Field<DateTime?>("voucher_date").Value.Date).Days >= 30 && (DateTime.Now - r.Field<DateTime?>("voucher_date").Value.Date).Days <= 60)
                      .Sum(r => (Double)r["amount"]);
            str6090days = dt
                   .AsEnumerable()
                   .Where(r => r.Field<DateTime?>("voucher_date").HasValue && (DateTime.Now - r.Field<DateTime?>("voucher_date").Value.Date).Days >= 60 && (DateTime.Now - r.Field<DateTime?>("voucher_date").Value.Date).Days <= 90)
                   .Sum(r => (Double)r["amount"]);

            str90days = dt
                      .AsEnumerable()
                      .Where(r => r.Field<DateTime?>("voucher_date").HasValue && (DateTime.Now - r.Field<DateTime?>("voucher_date").Value.Date).Days > 90)
                      .Sum(r => (Double)r["amount"]);

            strTotal = str30days + str60days + str90days + str6090days;

            //var str30days = from row in dt.AsEnumerable()
            //                //where (row.Field<DateTime>("voucher_date") != null && row.Field<DateTime>("voucher_date") >= DateTime.Now.AddDays(-30))
            //                group row by row.Field<string>("SuppID") into grp
            //                select new
            //                        {
            //                            Id = grp.Key,
            //                            Sum = grp.Sum(r => r.Field<double>("amount"))
            //                        };

            string sclass = "success";

            string totalstr = "";
            if (str30days <= 0)
                sclass = "success";
            else
                sclass = "danger";
            trtemp += "<tr><td>0-30</td><td class='" + sclass + "'><a href=StaffStatements?period=30'>" + String.Format("{0:n2}", str30days) + "</a></td></tr>";
            if (str60days <= 0)
                sclass = "success";
            else
                sclass = "danger";
            trtemp += "<tr><td>31-60</td><td class='" + sclass + "'><a href='StaffStatements?period=3060'>" + String.Format("{0:n2}", str60days) + "</a></td></tr>";
            if (str6090days <= 0)
                sclass = "success";
            else
                sclass = "danger";
            trtemp += "<tr><td>61-90</td><td class='" + sclass + "'><a href='StaffStatements?period=6090'>" + String.Format("{0:n2}", str6090days) + "</a></td></tr>";
            if (str90days <= 0)
                sclass = "success";
            else
                sclass = "danger";
            trtemp += "<tr><td>>90</td><td class='" + sclass + "'><a href='StaffStatements?period=90'>" + String.Format("{0:n2}", str90days) + "</a></td></tr>";
            if (strTotal <= 0)
            {
                sclass = "success";
                totalstr = "ICRAF Owes you: ";
            }
            else
            {
                sclass = "danger";
                totalstr = "You Owe ICRAF: ";
            }
            trtemp += "<tr><td><strong>" + totalstr + "</strong></td><td class='" + sclass + "'><a href='StaffStatements?period=total'>" + String.Format("{0:n2}", strTotal) + "</a></td></tr>";
            return View();
        }

        protected string getSuppID()
        {

            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineV201101 QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();

            long templateID = 2018;//ICT_AD_QEW
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                if (scp.ColumnName == "user_id__1")
                {
                    scp.ToValue = System.Web.HttpContext.Current.User.Identity.Name.ToUpper().ToString();
                    //Response.Write("xxxxxxxxxxxxxxxxx");
                }
            }

            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            DataTable dt = ds.Tables[0];
            string tempstr = "";
            var result = from row in dt.AsEnumerable()
                         where row.Field<string>("user_id__1") != null && row.Field<string>("user_id__1") == System.Web.HttpContext.Current.User.Identity.Name.ToUpper().ToString()
                         select new
                         {
                             resource_id = row.Field<string>("resource_id"),
                         };

            foreach (var t in result)
            {
                tempstr = t.resource_id;

            }
            if (string.IsNullOrWhiteSpace(tempstr))
            {
                tempstr = "0";
            }
            return tempstr;
        }

        public ActionResult ExchangeRate(string cboCurrency)
        {
            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineV201101 QueryEngineCIP;
            string labelstr = "", datastr;

            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            //TemplateResultAsDataSet TemplateResultHRDATA = new TemplateResultAsDataSet();
            //TemplateResultOptions TemplateResultsOptionsHR = QueryEngineCIP.GetTemplateResultOptions(GetUserCredentials());
            //new TemplateResultOptions();
            //InputforHRDATAResult. = "p1_vHrData";


            int templateID = 1888; // Exchange Rates 
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, GetUserCredentials());
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(GetUserCredentials());
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            //Response.Write(sdatetbx.Value);
            //Response.End();
          

            List<SelectListItem> currency = new List<SelectListItem>();


                 
            currency = BindCurrency("All");


            var model = new ExchangeRatesViewModel
            {
                Currency = currency,
                Sdate = null,
                Edate = null
            };
            //Session["dtl"] = dtl;
            return View(model);
        }

        [HttpPost]
        public ActionResult ExchangeRate()
        {
            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineV201101 QueryEngineCIP;
            string labelstr = "", datastr;

            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            //TemplateResultAsDataSet TemplateResultHRDATA = new TemplateResultAsDataSet();
            //TemplateResultOptions TemplateResultsOptionsHR = QueryEngineCIP.GetTemplateResultOptions(GetUserCredentials());
            //new TemplateResultOptions();
            //InputforHRDATAResult. = "p1_vHrData";


            int templateID = 1888; // Exchange Rates 
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, GetUserCredentials());
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(GetUserCredentials());
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            //Response.Write(sdatetbx.Value);
            //Response.End();
            DateTime sdate = Convert.ToDateTime(Request.Form["Sdate"]);
            //sdate = sdate.AddMonths(-3);
            DateTime edate = Convert.ToDateTime(Request.Form["Edate"]);




            // Response.Write(String.Format("{0:MM/dd/yyyy}", date));  
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {

                if (scp.ColumnName == "date_from")
                {
                    scp.RestrictionType = ">=";
                    scp.ToValue = String.Format("{0:MM-dd-yyyy}", sdate);//.Replace("-", "");// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                                                                         //scp.FromValue = String.Format("{0:MM-dd-yyyy}", sdate);//.Replace("-", "");// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                                                                         //Response.Write(scp.FromValue + "<br>");
                                                                         //Response.Write(edate);
                }
                if (scp.ColumnName == "date_to")
                {
                    scp.RestrictionType = "<=";
                    scp.ToValue = String.Format("{0:MM-dd-yyyy}", edate);//.Replace("-", "");// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                                                                         // scp.FromValue = String.Format("{0:MM-dd-yyyy}", edate);//.Replace("-", "");// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                                                                         //Response.Write(scp.ToValue + "<br>");
                                                                         //Response.Write(edate);
                }

                //if (cboCurrency.Items.Count != 0)
                if (scp.ColumnName == "currency" && Request.Form["Currency"] != "All")
                {
                    scp.RestrictionType = "=";
                    scp.FromValue = Request.Form["CurrencyId"];// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");

                }

            }
            //Response.Write(sdate + "<br>");
            //Response.Write(edate);
            //Response.End();
            //else if (<..etc.. >;
            //Retrieve result
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //crpgrdvw.DataSource = ds;
            //crpgrdvw.DataBind();
            //cboCurrency.DataSource = ds;
            //cboCurrency.DataBind();
            //ListItem currency = new ListItem();
            //currency.Text = "All";
            //currency.Value = "All";
            //cboCurrency.Items.Insert(0, currency);

            DataTable dt = ds.Tables[0];
            DataTable dtl = new DataTable();
            dtl.Clear();
            dtl.Columns.Add("currency");
            dtl.Columns.Add("xcurrency");
            dtl.Columns.Add("Date from");
            dtl.Columns.Add("Date to");
            dtl.Columns.Add("Rate");
            dtl.Columns.Add("Rate (Inverse)");
            DataRow dr;//= dt.NewRow();
            var result = from row in dt.AsEnumerable()
                         let date_from = row.Field<DateTime?>("date_from")
                         where date_from.HasValue && row.Field<DateTime>("date_from") >= sdate && row.Field<DateTime>("date_to") <= edate
                         select new
                         {
                             currency = row.Field<string>("currency"),
                             xcurrency = row.Field<string>("xcurrency"),
                             date_from = row.Field<DateTime>("date_from"),
                             date_to = row.Field<DateTime>("date_to"),
                             f0_exchange_rat = Math.Round(row.Field<Double>("f0_exchange_rat"), 2),
                             f1_newrate = Math.Round(1 / row.Field<Double>("f0_exchange_rat"), 2)
                         };
            //group row by row.Field<string>("xdim_6") into grp
            //orderby grp.Key
            //select row["xdim_6"], row.Field<string>("voucher_no"), row["ext_inv_ref"], row["voucher_no"], row["voucher_date"], row["pay_currency"], row["f0_currency_amo"], row["amount"], row["description"];
            List<SelectListItem> currency = new List<SelectListItem>();
            int i = 0;

            //cboCurrency.Items.Insert(i, currency);
            foreach (var t in result)
            {
                ++i;

                //cboCurrency.Items.Insert(i, currency);
                ViewBag.tdstr += "<tr><td class='numeric'>" + t.currency + "</td><td class='numeric'>" + t.xcurrency + "</td><td class='numeric'>" + string.Format("{0:dd-MMM-yyyy}", t.date_from) + "</td><td class='numeric'>" + string.Format("{0:dd-MMM-yyyy}", t.date_to) + "</td><td class='text-right'>" + t.f0_exchange_rat + "</td><td class='text-right'>" + t.f1_newrate + "</td></tr>";
                dr = dtl.NewRow();
                dr["currency"] = t.currency;
                dr["xcurrency"] = t.xcurrency;
                dr["Date from"] = t.date_from;
                dr["Date to"] = t.date_to;
                dr["Rate"] = t.f0_exchange_rat;
                dr["Rate (Inverse)"] = t.f1_newrate;
                dtl.Rows.Add(dr);
            }

            currency = BindCurrency(Request.Form["CurrencyId"]);
            
            var model = new ExchangeRatesViewModel
            {
                Currency = currency,
                Sdate = sdate.ToString("dd-MM-yyyy"),
                Edate = edate.ToString("dd-MM-yyyy")
            };
            Session["dtl"] = dtl;
            return View(model);
        }


        public ActionResult MIRate(string mdate)
        {
            ViewBag.PageTitle = "Meals & Incidentals Rates ";

            List<SelectListItem> Sdate = new List<SelectListItem>();
            List<SelectListItem> Edate = new List<SelectListItem>();
            string sdatetemp = string.Empty;
            int j = 0;
            string stemp = "";
            string etemp = "";

            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                j++;
                Sdate.Add(new SelectListItem
                {
                    Value = "January-June  " + i.ToString(),
                    Text = "January-June  " + i.ToString()
                });

                j++;
                Sdate.Add(new SelectListItem
                {
                    Value = "July-December " + i.ToString(),
                    Text = "July-December " + i.ToString()
                });

            };

           
            //var selected = Sdate.Where(x => x.Value == sdatetemp).First();
            //selected.Selected = true;
            ViewBag.Countries = new SelectList(Sdate, "Sdate", "Sdate", sdatetemp);


            var model = new MIRatesViewModel
            {

                Sdate = Sdate,
                Countries = BindCountries("All")
            };

           

            
            return View(model);
        }


        [HttpPost]
        public ActionResult MIRate()
        {
            List<SelectListItem> Sdate = new List<SelectListItem>();
            List<SelectListItem> Edate = new List<SelectListItem>();

            int j = 0;
            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                j++;
                Sdate.Add(new SelectListItem
                {
                    Value = "January-June  " + i.ToString(),
                    Text = "January-June  " + i.ToString()
                });

                j++;
                Sdate.Add(new SelectListItem
                {
                    Value = "July-December " + i.ToString(),
                    Text = "July-December " + i.ToString()
                });
            };


           

            var selected = Sdate.Where(x => x.Value == Request.Form["StartDate"]).First();
            selected.Selected = true;

            string country = Request.Form["CountryId"];
            var model = new MIRatesViewModel
            {

                Sdate = Sdate,
                Countries = BindCountries(country)
            };

            //return View(model);

            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineV201101 QueryEngineCIP;
            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            //TemplateResultAsDataSet TemplateResultHRDATA = new TemplateResultAsDataSet();
            //TemplateResultOptions TemplateResultsOptionsHR = QueryEngineCIP.GetTemplateResultOptions(GetUserCredentials());
            //new TemplateResultOptions();
            //InputforHRDATAResult. = "p1_vHrData";


            int templateID = 2029;// M&amp;I Rates 
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            //Response.Write(edatetbx.Value);
            //Response.End();
            string midate = Request.Form["StartDate"];
            string stemp = "";
            string etemp = "";
            string dtemp = midate.Substring(0, 4);
            string sValue = midate;
            sValue = sValue.Substring(sValue.Length - 4, 4);
            switch (dtemp)
            {
                case "Janu":
                    stemp = "01-Jan-" + sValue;
                    etemp = "30-Jun-" + sValue;
                    break;
                default:
                    stemp = "01-Jul-" + sValue;
                    etemp = "31-Dec-" + sValue;
                    break;
            }

            //Response.Write(stemp + "=" + etemp);
            //Response.End();

            DateTime sdate = Convert.ToDateTime(stemp);
            DateTime edate = Convert.ToDateTime(etemp);
            //sdate = sdate.AddMonths(-1);
            //edate = edate.AddMonths(1);

            //Response.Write(sdate + "==" + edate);
            //Response.End();

            string scountry = "";
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                //if (scp.ColumnName == "date_from")
                //{
                //    //scp.RestrictionType = "=";
                //    //scp.ToValue = String.Format("{0:MM-dd-yyyy}", sdate);//.Replace("-", "");// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                //    //scp.FromValue = String.Format("{0:MM-dd-yyyy}", sdate);//.Replace("-", "");// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                //    //Response.Write(scp.FromValue + "<br>");
                //    //Response.Write(edate);
                //}
                //if (scp.ColumnName == "date_to")
                //{
                //    //scp.RestrictionType = "=";
                //    // scp.ToValue = String.Format("{0:MM-dd-yyyy}", edate);//.Replace("-", "");// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                //    // scp.FromValue = String.Format("{0:MM-dd-yyyy}", edate);//.Replace("-", "");// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                //    //Response.Write(scp.ToValue + "<br>");
                //    //Response.Write(edate);
                //}

                if (scp.ColumnName == "xdim_value" && Request.Form["CountryId"] != "All")
                {
                    scp.FromValue = Request.Form["CountryId"];// "01012015";// String.Format("{0:yyyy-MM-dd}", sdate);// sdate.ToString("mm-dd-yyyy");
                    scp.ToValue = Request.Form["CountryId"];
                    scountry = Request.Form["CountryId"];
                    //Response.Write(cboCountry.SelectedItem.Value + "==");
                    //s = scp.FromValue;
                }
                //Response.Write(scp.ColumnName + "==");
            }
            //Response.Write(scountry);
           // Response.End();
            //else if (<..etc.. >;
            //Retrieve result

            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;

            if (scountry != "" || scountry != "All")
                ds.Tables[0].DefaultView.RowFilter = " xdim_value like '%" + scountry + "%'";
            var dv = ds.Tables[0].DefaultView;
            var newDS = new DataSet();
            var newDT = dv.ToTable();
            newDS.Tables.Add(newDT);
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //crpgrdvw.DataSource = ds;
            //crpgrdvw.DataBind();
            //cboCountry.DataSource = ds;
            //cboCountry.DataBind();
            //ListItem country = new ListItem();
            //country.Text = "All";
            //country.Value = "All";
            //cboCountry.Items.Insert(0, country);

            DataTable dt = newDS.Tables[0];
            DataTable dtl = new DataTable();
            dtl.Clear();
            dtl.Columns.Add("country");
            dtl.Columns.Add("xcountry");
            dtl.Columns.Add("Date from");
            dtl.Columns.Add("Date to");
            dtl.Columns.Add("lunch");
            dtl.Columns.Add("dinner");
            dtl.Columns.Add("incidentals");
            dtl.Columns.Add("total");
            DataRow dr;//= dt.NewRow();
            var result = from row in dt.AsEnumerable()
                         let date_from = row.Field<DateTime?>("date_from")
                         where date_from.HasValue && row.Field<DateTime>("date_from") >= sdate && row.Field<DateTime>("date_to") <= edate
                         select new
                         {
                             country = row.Field<string>("xdim_value"),
                             xcountry = row.Field<string>("xdim_value"),
                             date_from = row.Field<DateTime>("date_from"),
                             date_to = row.Field<DateTime>("date_to"),
                             total = Math.Round(row.Field<Double>("value_1"), 2),
                             breakfast = Math.Round(row.Field<Double>("f0_breakfast"), 2),
                             lunch = Math.Round(row.Field<Double>("f1_lunch"), 2),
                             dinner = Math.Round(row.Field<Double>("f2_dinner"), 2),
                             incidentals = Math.Round(row.Field<Double>("f3_incidentals"), 2)
                         };
            //group row by row.Field<string>("xdim_6") into grp
            //orderby grp.Key
            //select row["xdim_6"], row.Field<string>("voucher_no"), row["ext_inv_ref"], row["voucher_no"], row["voucher_date"], row["pay_country"], row["f0_country_amo"], row["amount"], row["description"];
            //ListItem country;
            //country = new ListItem();
            //country.Text = "All";
            //country.Value = "All";
            //cboCountry.Items.Insert(i, country);
             foreach (var t in result)
            {
                //++i;
                //country = new ListItem();
                //country.Text = t.xcountry.Substring(0, t.xcountry.IndexOf("-"));
                //country.Value = country.Text;
                //cboCountry.Items.Insert(i, country);
                ViewBag.tdstr += "<tr><td class='numeric'>" + t.country + "</td><td class='numeric'>" + String.Format("{0:dd-MMM-yyyy}", t.date_from) + "</td><td class='numeric'>" + String.Format("{0:dd-MMM-yyyy}", t.date_to) + "</td><td class='text-right'>" + t.breakfast + "</td><td class='text-right'>" + t.lunch + "</td><td class='text-right'>" + t.dinner + "</td><td class='text-right'>" + t.incidentals + "</td><td class='text-right'>" + t.total + "</td></tr>";
                dr = dtl.NewRow();
                dr["country"] = t.country;
                dr["xcountry"] = t.xcountry;
                dr["Date from"] = t.date_from;
                dr["Date to"] = t.date_to;
                dr["total"] = t.total;
                dr["lunch"] = t.lunch;
                dr["dinner"] = t.dinner;
                dr["incidentals"] = t.incidentals;
                dtl.Rows.Add(dr);
            }
            //Response.Write(ViewBag.tdstr);
           // Response.End();
            Session["dtl"] = dtl;
            return View(model);
        }


        [HttpPost]
        public ActionResult CRPBudgets(string syear)
        {

            List<SelectListItem> finyears = new List<SelectListItem>();

            string fyear = Request.Form["Year"];
            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                finyears.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            var selected = finyears.Where(x => x.Value == fyear).First();
            selected.Selected = true;

            var model = new IndexReportViewModel
            {

                FinYears = finyears
            };

            return View(model);
        }

        public ActionResult CRPBudgets(string id, string syear)
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

        private void BindTransactions(string worder, string entity, string account)
        {
            string trtemp = "", bstr = "", cstr = "";
            WSCredentials credentials;
            credentials = GetUserCredentials();
            int templateID = 2001;// BudgetTransactions_FINWEBQEWS
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(GetUserCredentials());
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                if (scp.Description == "WorkOrder")
                {
                    scp.ToValue = worder;
                    scp.FromValue = worder;

                }
                if (scp.Description == "Entity")
                {
                    scp.ToValue = entity;
                    scp.FromValue = entity;

                }
                if (scp.Description == "Account")
                {
                    scp.ToValue = account;
                    scp.FromValue = account;

                }
                if (scp.Description == "Period")
                {
                    scp.FromValue = Session["speriod"].ToString();
                    scp.ToValue = Session["eperiod"].ToString(); ;
                }
                //Response.Write(scp.ColumnName + "=" + scp.Description + "**");
            }
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            DataTable dt = ds.Tables[0];
            string sclass = "";
            // var result = null;
            string vclass = "text-right";
            int i = 0;
            DataTable dtl = new DataTable();
            dtl.Clear();
            dtl.Columns.Add("Reference");
            dtl.Columns.Add("Description");
            dtl.Columns.Add("Budget");
            DataRow dr;//= dt.NewRow();
            string contact = ConfigurationManager.AppSettings["transactions"];
            var result = from row in dt.AsEnumerable()
                             //let trans_id = row.Field<Int64?>("trans_id")
                             //where trans_id.HasValue
                         select new
                         {
                             trans_id = row.Field<Int64?>("trans_id"),
                             version = row.Field<string>("version"),
                             xversion = row.Field<string>("xversion"),
                             amount = row.Field<double>("amount")

                         };
            foreach (var t in result)
            {
                if (t.amount < 0)
                    vclass = "danger";
                else
                    vclass = "text-right";

                ++i;
                ViewBag.tdstr += "<tr><td class='numeric'>" + t.trans_id + "</td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.version + " " + t.xversion + "'>" + t.version + " " + t.xversion + "</a></td><td class='" + vclass + "'>" + String.Format("{0:n0}", t.amount) + "</td></tr>";
                dr = dtl.NewRow();
                dr["Reference"] = t.trans_id;
                dr["Description"] = t.version + " " + t.xversion;
                dr["Budget"] = String.Format("{0:n0}", t.amount);
                dtl.Rows.Add(dr);
            }
            Session["dtl"] = dtl;
            if (i == 0)
            {
                ViewBag.tdstr += "<tr><td class='text - center'  colspan=4>No Budget Transactions</td></tr>";
                //LinkButton1.Visible = false;
            }
        }
        private void BindLedger(string worder, string entity, string account)
        {
            string trtemp = "", bstr = "", cstr = "";
            WSCredentials credentials;
            credentials = GetUserCredentials();
            int templateID = 1937;// GLTransactions_FINWEBQEWS
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(GetUserCredentials());
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                if (scp.Description == "WorkOrder")
                {
                    scp.ToValue = worder;
                    scp.FromValue = worder;

                }
                if (scp.Description == "Entity")
                {
                    scp.ToValue = entity;
                    scp.FromValue = entity;

                }
                if (scp.Description == "Account")
                {
                    scp.ToValue = account;
                    scp.FromValue = account;

                }
                if (scp.Description == "Period")
                {
                    scp.FromValue = Session["speriod"].ToString();
                    scp.ToValue = Session["eperiod"].ToString(); ;
                }
                //Response.Write(scp.ColumnName + "=" + scp.Description + "**");
            }
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //legdergrd.DataSource = ds;
            //legdergrd.DataBind();
            DataTable dt = ds.Tables[0];
            string vclass = "text-right";
            // var result = null;
            int i = 0;
            DataTable dtl = new DataTable();
            dtl.Clear();
            dtl.Columns.Add("Reference");
            dtl.Columns.Add("Description");
            dtl.Columns.Add("Actuals");
            DataRow dr;//= dt.NewRow();
            string contact = ConfigurationManager.AppSettings["transactions"];
            var result = from row in dt.AsEnumerable()
                             //let voucher_no = row.Field<Int64?>("voucher_no")
                             //where voucher_no.HasValue
                         select new
                         {
                             voucher_no = row.Field<Int64?>("voucher_no"),
                             description = row.Field<string>("description"),
                             amount = row.Field<double>("amount")

                         };
            foreach (var t in result)
            {
                ++i;
                if (t.amount < 0)
                    vclass = "danger";
                else
                    vclass = "text-right";
                ViewBag.bstr += "<tr><td class='numeric'>" + t.voucher_no + "</a></td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.description + "'>" + t.description + " </a></td><td class='" + vclass + "'>" + String.Format("{0:n0}", t.amount) + "</td></tr>";

                dr = dtl.NewRow();
                dr["Reference"] = t.voucher_no;
                dr["Description"] = t.description;
                dr["Actuals"] = String.Format("{0:n0}", t.amount);
                dtl.Rows.Add(dr);
            }
            Session["gtl"] = dtl;
            if (i == 0)
            {
                ViewBag.bstr += "<tr><td class='text - center'  colspan=4>No General Ledger Transactions</td></tr>";
                //LinkButton2.Visible = false;
            }
        }
        private void BindCommittments(string worder, string entity, string account)
        {
            string trtemp = "", bstr = "", cstr = "";
            WSCredentials credentials;
            credentials = GetUserCredentials();
            int templateID = 1998;// CommTransactions_FINWEBQEWS
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(GetUserCredentials());
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                if (scp.Description == "WorkOrder")
                {
                    scp.ToValue = worder;
                    scp.FromValue = worder;

                }
                if (scp.Description == "Entity")
                {
                    scp.ToValue = entity;
                    scp.FromValue = entity;

                }
                if (scp.Description == "Account")
                {
                    scp.ToValue = account;
                    scp.FromValue = account;

                }
                if (scp.Description == "Period")
                {
                    scp.FromValue = Session["speriod"].ToString();
                    scp.ToValue = Session["eperiod"].ToString(); ;
                }
                //Response.Write(scp.ColumnName + "=" + scp.Description + "**");
            }
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //commitmentgrd.DataSource = ds;
            //commitmentgrd.DataBind();
            DataTable dt = ds.Tables[0];
            string vclass = "text-right";
            DataTable dtl = new DataTable();
            dtl.Clear();
            dtl.Columns.Add("Date");
            dtl.Columns.Add("TA/TEC/Requisition Number/PO Number");
            dtl.Columns.Add("Commitment");
            DataRow dr;//= dt.NewRow();
            string contact = ConfigurationManager.AppSettings["transactions"];
            var result = from row in dt.AsEnumerable()
                             //where row.Field<string>("original_id") != ""
                             //where row.Field<string>("original_id") != ""
                         select new
                         {
                             f0_date = row.Field<string>("f0_date"),
                             original_id = row.Field<string>("original_id"),
                             amount = row.Field<double>("rest_amount")

                         };
            int i = 0;
            foreach (var t in result)
            {
                ++i;
                if (t.amount < 0)
                    vclass = "danger";
                else
                    vclass = "text-right";
                string sdate = "";
                if (!String.IsNullOrWhiteSpace(t.f0_date))
                {
                    // DateTime tdate = DateTime.ParseExact(t.f0_date, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    sdate = string.Format("{0:dd-MMM-yyyy}", t.f0_date);//  string.Format("{0:MM-dd-yyyy}", Convert.ToDateTime(t.f0_date));
                }

                ViewBag.cstr += "<tr><td class='numeric'>" + sdate + "</a></td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.original_id + "'>" + t.original_id + " </a></td><td class='" + vclass + "'>" + String.Format("{0:n0}", t.amount) + "</td></tr>";

                dr = dtl.NewRow();
                dr["Date"] = t.f0_date;
                dr["TA/TEC/Requisition Number/PO Number"] = t.original_id;
                dr["Commitment"] = String.Format("{0:n0}", t.amount);
                dtl.Rows.Add(dr);
            }
            Session["ctl"] = dtl;
            if (i == 0)
            {
                ViewBag.cstr += "<tr><td class='text - center' colspan=4>No Commitment Transactions</td></tr>";
                //LinkButton3.Visible = false;
            }
        }
        public WSCredentials GetUserCredentials()
        {
            WSCredentials cred = new WSCredentials
            {
                Username = ConfigurationManager.AppSettings["aUsername"],
                Password = ConfigurationManager.AppSettings["aPassword"],
                Client = "GG"
            };
            return cred;
        }

        private List<SelectListItem> BindCountries(string Country)
        {
            int templateID = 2324;// Countries_QEWS
            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            DataTable dt = ds.Tables[0];
            List<SelectListItem> country = new List<SelectListItem>();
            int i = 0;
            string ctemp = "", counttemp = ""; ;

            foreach (DataRow row in dt.Rows)
            {


                if (row["description"].ToString().IndexOf("-") > 0)
                {
                    country.Add(new SelectListItem
                    {
                        Text = row["description"].ToString().Substring(0, row["description"].ToString().IndexOf("-")),
                        Value = row["description"].ToString().Substring(0, row["description"].ToString().IndexOf("-"))
                    });

                    //cboCountry.Items.Insert(i, country);
                }
                else
                {
                    country.Add(new SelectListItem
                    {
                        Text = row["description"].ToString(),
                        Value = row["description"].ToString()
                    });


                    //cboCountry.Items.Insert(i, country);
                }
                i++;

            }
            // i++;
            country.Add(new SelectListItem
            {
                Text = "All",
                Value = "All"
            });

            country = country.GroupBy(x => x.Text).Select(x => x.First()).OrderBy(a => a.Text).ToList();
            var SelectedItem = country.Where(m => m.Value == Country).First();
            SelectedItem.Selected = true;


            return country;


        }

        private List<SelectListItem> BindCurrency(string CurrencyId)
        {
            int templateID = 1888;// Countries_QEWS
            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            DataTable dt = ds.Tables[0];
            List<SelectListItem> country = new List<SelectListItem>();
            int i = 0;
            string ctemp = "", counttemp = ""; ;

            var result = from row in dt.AsEnumerable()
                             // where date_from.HasValue && row.Field<DateTime>("date_from") >= sdate && row.Field<DateTime>("date_to") <= edate
                         select new
                         {
                             currency = row.Field<string>("currency"),
                             xcurrency = row.Field<string>("xcurrency"),
                         };
            //group row by row.Field<string>("xdim_6") into grp
            //orderby grp.Key
            //select row["xdim_6"], row.Field<string>("voucher_no"), row["ext_inv_ref"], row["voucher_no"], row["voucher_date"], row["pay_currency"], row["f0_currency_amo"], row["amount"], row["description"];
            //ListItem currency;
            List<SelectListItem> currency = new List<SelectListItem>();
            currency.Add(new SelectListItem
            {
                Text = "All",
                Value = "All"
            });


            //cboCurrency.Items.Insert(i, currency);
            foreach (var t in result)
            {
                ++i;

                //currency = new List<SelectListItem>();
                currency.Add(new SelectListItem
                {
                    Text = t.xcurrency,
                    Value = t.currency
                });

            }
            // i++;
            currency.Add(new SelectListItem
            {
                Text = "All",
                Value = "All"
            });

            currency = currency.GroupBy(x => x.Text).Select(x => x.First()).OrderBy(a => a.Text).ToList();
            var SelectedItem = currency.Where(m => m.Value == CurrencyId).First();
            SelectedItem.Selected = true;


            return currency;


        }

        public ActionResult Accounts(string worder, string entity, string accountdesc, string account)
        {
            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineV201101 QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();
            //TemplateResultAsDataSet TemplateResultHRDATA = new TemplateResultAsDataSet();
            //TemplateResultOptions TemplateResultsOptionsHR = QueryEngineCIP.GetTemplateResultOptions(GetUserCredentials());
            //new TemplateResultOptions();
            //InputforHRDATAResult. = "p1_vHrData";
            ViewBag.PageTitle += " - Work Order: " + worder + " Entity: " + entity + " Account: " + accountdesc + " (" + account + ")";


            int templateID = 1967;//PerAccount_FINWEBQEWS
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {
                if (scp.Description == "Workord")
                {
                    scp.ToValue = worder;
                    scp.FromValue = worder;

                }
                if (scp.Description == "Entity")
                {
                    scp.ToValue = entity;
                    scp.FromValue = entity;

                }
                if (scp.Description == "Group")
                {
                    scp.ToValue = account;
                    scp.FromValue = account;

                }
                if (scp.Description == "Period")
                {
                    scp.FromValue = Session["speriod"].ToString();
                    scp.ToValue = Session["eperiod"].ToString(); ;
                }
                //Response.Write(scp.ColumnName + "=" + scp.Description + "**");
            }
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //crpgrdvw.DataSource = ds;
            //crpgrdvw.DataBind();
            DataTable dt = ds.Tables[0];
            double totals = 0;
            double budget = 0, imprate = 0;
            string sclass = "";
            // var result = null;

            DataTable dtl = new DataTable();

            dtl.Clear();
            dtl.Columns.Add("Natural Classification");
            dtl.Columns.Add("ChargeCode");
            dtl.Columns.Add("Budget");
            dtl.Columns.Add("Commitments");
            dtl.Columns.Add("Actuals");
            dtl.Columns.Add("Total Expenditure");
            dtl.Columns.Add("Available");
            dtl.Columns.Add("% Utilized");
            DataRow dr;//= dt.NewRow();
            var result = from row in dt.AsEnumerable()
                             //where row.Field<string>("r0dim3") != ""
                         select new
                         {
                             xdim1 = row.Field<string>("xdim1"),
                             dim1 = row.Field<string>("dim1"),
                             f0_chargecode3 = row.Field<string>("f0_chargecode3"),
                             ple_amount = row.Field<double>("ple_amount"),
                             amount = row.Field<double>("amount"),
                             co_rest_amount = row.Field<double>("co_rest_amount"),
                             actuals = row.Field<double>("amount") + row.Field<double>("co_rest_amount"),
                             available = row.Field<double>("ple_amount") - row.Field<double>("amount") - row.Field<double>("co_rest_amount")
                         };
            foreach (var t in result)
            {
                budget = t.ple_amount;
                if (budget > 0)
                {
                    imprate = (t.actuals / budget) * 100;
                    imprate = Math.Round(imprate, 2);
                }
                else
                    imprate = 0;

                if (imprate <= 0 || imprate > 100)
                    sclass = "danger";
                else if (imprate > 0 && imprate < 50)
                    sclass = "warning";
                else
                    sclass = "success";
                string aclass = "text-right";
                string tclass = "text-right";
                string vclass = "text-right";
                if (t.amount < 0)
                    aclass = "danger";
                else
                    aclass = "text-right";
                if (t.actuals < 0)
                    tclass = "danger";
                else
                    tclass = "text-right";
                if (t.available < 0)
                    vclass = "danger";
                else
                    vclass = "text-right";
                totals += t.amount;
                ViewBag.tdstr += "<tr><td class='numeric'><a href='Transactions?worder=" + worder + "&Entity=" + entity + "&account=" + t.dim1 + "&accountdesc=" + accountdesc + "'>" + t.xdim1 + "</a></td><td class='numeric'>" + t.f0_chargecode3 + "</td><td class='text-right'>" + String.Format("{0:n0}", t.ple_amount) + "</td><td class='" + aclass + "'>" + String.Format("{0:n0}", t.amount) + "</td><td class='text-right'>" + String.Format("{0:n0}", t.co_rest_amount) + "</td><td class='" + tclass + "'>" + String.Format("{0:n0}", t.actuals) + "</td><td class='" + vclass + "'>" + String.Format("{0:n0}", t.available) + "</td><td class='" + sclass + "'>" + imprate + "%</td></tr>";
                dr = dtl.NewRow();
                dr["Natural Classification"] = t.xdim1;
                dr["ChargeCode"] = t.f0_chargecode3;
                dr["Budget"] = String.Format("{0:n0}", t.ple_amount);
                dr["Actuals"] = String.Format("{0:n0}", t.amount);
                dr["Commitments"] = String.Format("{0:n0}", t.co_rest_amount);
                dr["Total Expenditure"] = String.Format("{0:n0}", t.actuals);
                dr["Available"] = String.Format("{0:n0}", t.available);
                dr["% Utilized"] = imprate;
                dtl.Rows.Add(dr);
            }
            Session["dtl"] = dtl;



            return View();
        }

        public ActionResult StaffStatements(string desc, string period, string expensecode)
        {
            QueryEngineV201101 QueryEngineCIP;
            string hstr = "";
            ViewBag.PageTitle = "My Staff Statement by Expense Type: " + desc + " (" + System.Web.HttpContext.Current.User.Identity.Name.ToUpper().ToString() + ")";

            WSCredentials credentials;
            credentials = GetUserCredentials();
            TemplateList cust = new TemplateList();
            QueryEngineCIP = new QueryEngineV201101();
            InputForTemplateResult InputforHRDATAResult = new InputForTemplateResult();

            long templateID = 2115;// StaffStatement_QEWS
            QueryEngineV201101 service = new QueryEngineV201101();
            SearchCriteria searchProp = service.GetSearchCriteria(templateID, true, credentials);
            // Create the InputForTemplateResult class and set values
            InputForTemplateResult input = new InputForTemplateResult();
            TemplateResultOptions options = service.GetTemplateResultOptions(credentials);
            options.RemoveHiddenColumns = true;
            input.TemplateResultOptions = options;
            input.TemplateId = templateID;
            // Get new values to SearchCriteria (if that’s what you want to do
            input.SearchCriteriaPropertiesList = searchProp.SearchCriteriaPropertiesList;
            DateTime sdate = Convert.ToDateTime("01-Jan-" + DateTime.Today.Year.ToString());
            DateTime edate = DateTime.Today;
            string test = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            foreach (SearchCriteriaProperties scp in input.SearchCriteriaPropertiesList)
            {

                //if (scp.Description == "Expense Type")
                //{
                //    if (expensetype != "All" && expensetype  != null)
                //    {
                //        scp.ToValue = expensetype;
                //        scp.FromValue = expensetype;
                //        //Response.Write(scp.ToValue);
                //    }


                //}

                if (scp.Description == "SuppID")
                {
                    scp.FromValue = Session["resource_id"].ToString();// Request.QueryString["SuppID"]; //
                }
                //Response.Write(scp.ColumnName + "==");
                //example.Text = example.Text + "=" + scp.ColumnName + "=" + test + "=";
            }

            //else if (<..etc.. >;
            //Retrieve result
            TemplateResultAsDataSet TemplateResultHRDATA = service.GetTemplateResultAsDataSet(input, credentials);
            DataSet ds = TemplateResultHRDATA.TemplateResult;
            //FormatInfo formats = QueryEngineCIP.GetFormatInfo(templateID, credentials);
            //SetDataGridColumns(crpgrdvw, ds, formats);
            //crpgrdvw.DataSource = ds;
            //crpgrdvw.DataBind();
            //DataTable dt = ds.Tables[0];
            DataTable dt = ds.Tables[0];
            string tempstr, lstr;
            double totals = 0;

            string querystr = period;
            string fstr = "";
            // var result = null;
            DataTable dtl = new DataTable();
            dtl.Clear();
            dtl.Columns.Add("Transaction No.");
            dtl.Columns.Add("Invoice No.");
            dtl.Columns.Add("Date");
            dtl.Columns.Add("Description");
            dtl.Columns.Add("Amount (USD)");
            dtl.Columns.Add("Currency");
            dtl.Columns.Add("Other Amount");
            string exrate = "";
            DataRow dr;//= dt.NewRow();
            string contact = ConfigurationManager.AppSettings["staffstatements"];
            switch (querystr)
            {
                case "30":
                    hstr = " 30 Days";
                    var result = from row in dt.AsEnumerable()
                                 let date_from = row.Field<DateTime?>("voucher_date")
                                 where date_from.HasValue && (DateTime.Now - row.Field<DateTime>("voucher_date")).Days <= 30
                                 select new
                                 {
                                     xdim_6 = row.Field<string>("xdim_6"),
                                     voucher_no = row.Field<Int64>("voucher_no"),
                                     ext_inv_ref = row.Field<string>("ext_inv_ref"),
                                     voucher_date = row.Field<DateTime>("voucher_date"),
                                     pay_currency = row.Field<string>("pay_currency"),
                                     f0_currency_amo = row.Field<double>("f0_currency_amo"),
                                     amount = row.Field<double>("amount"),
                                     description = row.Field<string>("description")
                                 };
                    foreach (var t in result)
                    {
                        totals += t.amount;
                        exrate = "1 USD = " + (Math.Round(t.f0_currency_amo / t.amount, 2)) + " " + t.pay_currency.ToString();
                        ViewBag.tdstr += "<tr><td class='numeric'>" + t.voucher_no + "</td><td class='numeric'>" + t.ext_inv_ref + "</td><td class='numeric'>" + string.Format("{0:dd-MMM-yyyy}", t.voucher_date) + "</td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.description + "'>" + t.description + " </a></td><td><span class='tooltips tooltip-pink' data-toggle='tooltip' data-original-title='" + exrate + "'>" + t.pay_currency + "</span></td><td class='text-right'>" + String.Format("{0:n2}", t.amount) + "</td><td  class='text-right'>" + String.Format("{0:n2}", t.f0_currency_amo) + "</td></tr>";
                        dr = dtl.NewRow();
                        dr["Transaction No."] = t.voucher_no;
                        dr["Invoice No."] = t.ext_inv_ref;
                        dr["Date"] = t.voucher_date;
                        dr["Description"] = t.description;
                        dr["Currency"] = t.pay_currency;
                        dr["Other Amount"] = t.f0_currency_amo;
                        dr["Amount (USD)"] = t.amount;
                        dtl.Rows.Add(dr);
                    }
                    Session["dtl"] = dtl;
                    ViewBag.tdstr = ViewBag.tdstr;
                    break;
                case "3060":
                    hstr = " 60 Days";
                    var str60 = from row in dt.AsEnumerable()
                                let date_from = row.Field<DateTime?>("voucher_date")
                                where date_from.HasValue && (DateTime.Now - row.Field<DateTime>("voucher_date")).Days >= 30 && (DateTime.Now - row.Field<DateTime>("voucher_date")).Days <= 60
                                select new
                                {
                                    xdim_6 = row.Field<string>("xdim_6"),
                                    voucher_no = row.Field<Int64>("voucher_no"),
                                    ext_inv_ref = row.Field<string>("ext_inv_ref"),
                                    voucher_date = row.Field<DateTime>("voucher_date"),
                                    pay_currency = row.Field<string>("pay_currency"),
                                    f0_currency_amo = row.Field<double>("f0_currency_amo"),
                                    amount = row.Field<double>("amount"),
                                    description = row.Field<string>("description")
                                };
                    foreach (var t in str60)
                    {
                        totals += t.amount;
                        exrate = "1 USD = " + (Math.Round(t.f0_currency_amo / t.amount, 2)) + " " + t.pay_currency.ToString();
                        ViewBag.tdstr += "<tr><td class='numeric'>" + t.voucher_no + "</td><td class='numeric'>" + t.ext_inv_ref + "</td><td class='numeric'>" + string.Format("{0:dd-MMM-yyyy}", t.voucher_date) + "</td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.description + "'>" + t.description + " </a></td><td><span class='tooltips tooltip-pink' data-toggle='tooltip' data-original-title='" + exrate + "'>" + t.pay_currency + "</span><td class='text-right'>" + String.Format("{0:n2}", t.amount) + "</td><td  class='text-right'>" + String.Format("{0:n2}", t.f0_currency_amo) + "</td></tr>";
                        dr = dtl.NewRow();
                        dr["Transaction No."] = t.voucher_no;
                        dr["Invoice No."] = t.ext_inv_ref;
                        dr["Date"] = t.voucher_date;
                        dr["Description"] = t.description;
                        dr["Transaction Currency"] = t.pay_currency;
                        dr["Other Amount"] = t.f0_currency_amo;
                        dr["Amount (USD)"] = t.amount;
                        dtl.Rows.Add(dr);
                    }
                    Session["dtl"] = dtl;
                    ViewBag.tdstr = ViewBag.tdstr;
                    break;
                case "6090":
                    hstr = " 60-90 Days";
                    var str6090 = from row in dt.AsEnumerable()
                                  let date_from = row.Field<DateTime?>("voucher_date")
                                  where date_from.HasValue && (DateTime.Now - row.Field<DateTime>("voucher_date")).Days >= 60 && (DateTime.Now - row.Field<DateTime>("voucher_date")).Days <= 90
                                  select new
                                  {
                                      xdim_6 = row.Field<string>("xdim_6"),
                                      voucher_no = row.Field<Int64>("voucher_no"),
                                      ext_inv_ref = row.Field<string>("ext_inv_ref"),
                                      voucher_date = row.Field<DateTime>("voucher_date"),
                                      pay_currency = row.Field<string>("pay_currency"),
                                      f0_currency_amo = row.Field<double>("f0_currency_amo"),
                                      amount = row.Field<double>("amount"),
                                      description = row.Field<string>("description")
                                  };
                    foreach (var t in str6090)
                    {
                        totals += t.amount;
                        exrate = "1 USD = " + (Math.Round(t.f0_currency_amo / t.amount, 2)) + " " + t.pay_currency.ToString();
                        ViewBag.tdstr += "<tr><td class='numeric'>" + t.voucher_no + "</td><td class='numeric'>" + t.ext_inv_ref + "</td><td class='numeric'>" + string.Format("{0:dd-MMM-yyyy}", t.voucher_date) + "</td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.description + "'>" + t.description + " </a></td><td><span class='tooltips tooltip-pink' data-toggle='tooltip' data-original-title='" + exrate + "'>" + t.pay_currency + "</span><td class='text-right'>" + String.Format("{0:n2}", t.amount) + "</td><td  class='text-right'>" + String.Format("{0:n2}", t.f0_currency_amo) + "</td></tr>";
                        dr = dtl.NewRow();
                        dr["Transaction No."] = t.voucher_no;
                        dr["Invoice No."] = t.ext_inv_ref;
                        dr["Date"] = t.voucher_date;
                        dr["Description"] = t.description;
                        dr["Currency"] = t.pay_currency;
                        dr["Other Amount"] = t.f0_currency_amo;
                        dr["Amount (USD)"] = t.amount;
                        dtl.Rows.Add(dr);
                    }
                    Session["dtl"] = dtl;
                    ViewBag.tdstr = ViewBag.tdstr;
                    break;
                case "90":
                    hstr = " 90 Days";
                    var str90 = from row in dt.AsEnumerable()
                                let date_from = row.Field<DateTime?>("voucher_date")
                                where date_from.HasValue && (DateTime.Now - row.Field<DateTime>("voucher_date")).Days > 90
                                select new
                                {
                                    xdim_6 = row.Field<string>("xdim_6"),
                                    voucher_no = row.Field<Int64>("voucher_no"),
                                    ext_inv_ref = row.Field<string>("ext_inv_ref"),
                                    voucher_date = row.Field<DateTime>("voucher_date"),
                                    pay_currency = row.Field<string>("pay_currency"),
                                    f0_currency_amo = row.Field<double>("f0_currency_amo"),
                                    amount = row.Field<double>("amount"),
                                    description = row.Field<string>("description")
                                };
                    foreach (var t in str90)
                    {
                        totals += t.amount;
                        exrate = "1 USD = " + (Math.Round(t.f0_currency_amo / t.amount, 2)) + " " + t.pay_currency.ToString();
                        ViewBag.tdstr += "<tr><td class='numeric'>" + t.voucher_no + "</td><td class='numeric'>" + t.ext_inv_ref + "</td><td class='numeric'>" + string.Format("{0:dd-MMM-yyyy}", t.voucher_date) + "</td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.description + "'>" + t.description + " </a></td><td><span class='tooltips tooltip-pink' data-toggle='tooltip' data-original-title='" + exrate + "'>" + t.pay_currency + "</span><td class='text-right'>" + String.Format("{0:n2}", t.amount) + "</td><td  class='text-right'>" + String.Format("{0:n2}", t.f0_currency_amo) + "</td></tr>";
                        dr = dtl.NewRow();
                        dr["Transaction No."] = t.voucher_no;
                        dr["Invoice No."] = t.ext_inv_ref;
                        dr["Date"] = t.voucher_date;
                        dr["Description"] = t.description;
                        dr["Currency"] = t.pay_currency;
                        dr["Other Amount"] = t.f0_currency_amo;
                        dr["Amount (USD)"] = t.amount;
                        dtl.Rows.Add(dr);
                    }
                    Session["dtl"] = dtl;
                    ViewBag.tdstr = ViewBag.tdstr;
                    break;
                case "total":
                    hstr = " Total";
                    var strtotal = from row in dt.AsEnumerable()
                                   let date_from = row.Field<DateTime?>("voucher_date")
                                   where date_from.HasValue
                                   select new
                                   {
                                       xdim_6 = row.Field<string>("xdim_6"),
                                       voucher_no = row.Field<Int64>("voucher_no"),
                                       ext_inv_ref = row.Field<string>("ext_inv_ref"),
                                       voucher_date = row.Field<DateTime>("voucher_date"),
                                       pay_currency = row.Field<string>("pay_currency"),
                                       f0_currency_amo = row.Field<double>("f0_currency_amo"),
                                       amount = row.Field<double>("amount"),
                                       description = row.Field<string>("description")
                                   };
                    foreach (var t in strtotal)
                    {
                        totals += t.amount;
                        exrate = "1 USD = " + (Math.Round(t.f0_currency_amo / t.amount, 2)) + " " + t.pay_currency.ToString();
                        ViewBag.tdstr += "<tr><td class='numeric'>" + t.voucher_no + "</td><td class='numeric'>" + t.ext_inv_ref + "</td><td class='numeric'>" + string.Format("{0:dd-MMM-yyyy}", t.voucher_date) + "</td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.description + "'>" + t.description + " </a></td><td><span class='tooltips tooltip-pink' data-toggle='tooltip' data-original-title='" + exrate + "'>" + t.pay_currency + "</span><td class='text-right'>" + String.Format("{0:n2}", t.amount) + "</td><td  class='text-right'>" + String.Format("{0:n2}", t.f0_currency_amo) + "</td></tr>";
                        dr = dtl.NewRow();
                        dr["Transaction No."] = t.voucher_no;
                        dr["Invoice No."] = t.ext_inv_ref;
                        dr["Date"] = t.voucher_date;
                        dr["Description"] = t.description;
                        dr["Currency"] = t.pay_currency;
                        dr["Other Amount"] = t.f0_currency_amo;
                        dr["Amount (USD)"] = t.amount;
                        dtl.Rows.Add(dr);
                    }
                    Session["dtl"] = dtl;
                    ViewBag.tdstr = ViewBag.tdstr;
                    break;
                case "0":
                    string squery = expensecode;
                    squery = squery.Trim().ToLower();
                    var straccount = from row in dt.AsEnumerable()
                                     let date_from = row.Field<DateTime?>("voucher_date")
                                     where date_from.HasValue && (row.Field<string>("dim_6").ToLower().Trim() == squery)

                                     select new
                                     {
                                         xdim_6 = row.Field<string>("xdim_6"),
                                         voucher_no = row.Field<Int64>("voucher_no"),
                                         ext_inv_ref = row.Field<string>("ext_inv_ref"),
                                         voucher_date = row.Field<DateTime>("voucher_date"),
                                         pay_currency = row.Field<string>("pay_currency"),
                                         f0_currency_amo = row.Field<double>("f0_currency_amo"),
                                         amount = row.Field<double>("amount"),
                                         description = row.Field<string>("description")
                                     };

                    foreach (var t in straccount)
                    {
                        totals += t.amount;
                        exrate = "1 USD = " + (Math.Round(t.f0_currency_amo / t.amount, 2)) + " " + t.pay_currency.ToString();
                        ViewBag.tdstr += "<tr><td class='numeric'>" + t.voucher_no + "</td><td class='numeric'>" + t.ext_inv_ref + "</td><td class='numeric'>" + string.Format("{0:dd-MMM-yyyy}", t.voucher_date) + "</td><td class='numeric'><a href='mailto:" + contact + "?subject= " + t.description + "'>" + t.description + " </a></td><td><span class='tooltips tooltip-pink' data-toggle='tooltip' data-original-title='" + exrate + "'>" + t.pay_currency + "</span><td class='text-right'>" + String.Format("{0:n2}", t.amount) + "</td><td  class='text-right'>" + String.Format("{0:n2}", t.f0_currency_amo) + "</td></tr>";
                        dr = dtl.NewRow();
                        dr["Transaction No."] = t.voucher_no;
                        dr["Invoice No."] = t.ext_inv_ref;
                        dr["Date"] = t.voucher_date;
                        dr["Description"] = t.description;
                        dr["Currency"] = t.pay_currency;
                        dr["Other Amount"] = t.f0_currency_amo;
                        dr["Amount (USD)"] = t.amount;
                        dtl.Rows.Add(dr);
                    }
                    Session["dtl"] = dtl;
                    ViewBag.tdstr = ViewBag.tdstr;
                    break;
                default:
                    break;
            }


            //group row by row.Field<string>("xdim_6") into grp
            //orderby grp.Key
            //select row["xdim_6"], row.Field<string>("voucher_no"), row["ext_inv_ref"], row["voucher_no"], row["voucher_date"], row["pay_currency"], row["f0_currency_amo"], row["amount"], row["description"];

            //foreach (var t in result)
            //{
            //    totals += t.amount;
            //    ViewBag.tdstr += "<tr><td class='numeric'>" + t.voucher_no + "</td><td class='numeric'>" + t.ext_inv_ref + "</td><td class='numeric'>" + t.voucher_date + "</td><td class='numeric'>" + t.xdim_6 + "</td><td class='numeric'>" + t.pay_currency + "</td><td class='numeric'>" + t.f0_currency_amo + "</td><td>" + String.Format("{0:n2}", t.amount) + "</td><td class='numeric'>" + t.description + "</td></tr>";
            //}
            ViewBag.tdstr += "<tr><td colspan=4 class='numeric'><strong>Total</strong></td><td>&nbsp;</td><td  class='text-right'><strong>" + String.Format("{0:n2}", totals) + "</strong></td><td>&nbsp;</td></tr>";

            return View();
        }
    }
}

