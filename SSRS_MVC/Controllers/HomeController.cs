using Microsoft.Reporting.WebForms;
using SSRS_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SSRS_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Local report
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportCustomers()
        {
            Model1 db = new Model1();
            ReportViewer reportViewer = new ReportViewer();
            ReportDataSource reportDataSource;
            DataTable dataTable = ToDataTables(db.Customers.ToList());
            reportDataSource = new ReportDataSource("dsCustomer", dataTable);
            reportViewer.ShowToolBar = true;
            reportViewer.ZoomMode = ZoomMode.PageWidth;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Pixel(1000);
            reportViewer.ShowExportControls = true;
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\RptCustomers.rdlc";
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            ViewBag.ReportViewer = reportViewer;

            return View();
        }


        /// <summary>
        /// Local Report
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductReports()
        {
            Model1 db = new Model1();
            ReportViewer reportViewer = new ReportViewer();
            ReportParameter reportParameter;
            ReportDataSource reportDataSource;
            DataTable dataTable = ToDataTables(db.Products.Where(x=>x.ProductID == 680).ToList());
            reportDataSource = new ReportDataSource("dsProduct", dataTable);

            reportParameter = new ReportParameter("ProducID", "680",true);
            

            reportViewer.ShowPageNavigationControls = false;
            reportViewer.ShowRefreshButton = false;
            reportViewer.ShowBackButton = false;
            reportViewer.ShowZoomControl = false;

            reportViewer.ShowPrintButton = true;
            reportViewer.ShowExportControls = true;
            reportViewer.ShowToolBar = true;

            reportViewer.ZoomMode = ZoomMode.PageWidth;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Pixel(1000);
            reportViewer.ShowExportControls = true;
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\rptProducts.rdlc";
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { reportParameter });
            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        /// <summary>
        /// Remote report
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportProducts()
        {
            ReportViewer rptViewer = new ReportViewer();

            ReportParameter parmProductCategoryID;
            ReportParameter parmProductModelID;
            List<ReportParameter> reportParameters;

            parmProductCategoryID = new ReportParameter("ProductCategoryID", "18");
            parmProductModelID = new ReportParameter("ProductModelID", "9");
            reportParameters = new List<ReportParameter>();
            reportParameters.Add(parmProductCategoryID);
            reportParameters.Add(parmProductModelID);


            //rptViewer.ShowPageNavigationControls = false;
            //rptViewer.ShowRefreshButton = false;
            //rptViewer.ShowBackButton = false;
            //rptViewer.ShowZoomControl = false;
            //rptViewer.ShowToolBar = true;
            //rptViewer.ShowPrintButton = true;
            //rptViewer.ShowExportControls = true;
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            rptViewer.SizeToReportContent = true;
            rptViewer.ZoomMode = ZoomMode.FullPage;
            rptViewer.Width = Unit.Percentage(100);
            rptViewer.Height = Unit.Percentage(100);
            rptViewer.AsyncRendering = false;
            rptViewer.ServerReport.ReportServerUrl = new Uri("http://vnbid0579:8082/ReportServer/");
            rptViewer.ServerReport.ReportPath = "/Reports/RptProducts";
            rptViewer.ServerReport.SetParameters(reportParameters);
            ViewBag.ReportViewer = rptViewer;
            return View();
        }

        public static DataTable ToDataTables<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }
}