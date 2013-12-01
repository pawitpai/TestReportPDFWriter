using ReportPdfFileWriter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfFileWriter;
using System.Diagnostics;
using System.Drawing;
using TestPdfFileWriter;

namespace ReportPdfFileWriter.Controllers
{
    public class HomeController : Controller
    {
        
        ////
        // GET: /Home/

        public ActionResult Index()
        {
            //ViewData["Message"] = "Press Button to download report (pdf)";
            //return View();
            ArticleExample AE = new ArticleExample();
            AE.Test(false, "D:\\Rsc\\ReportPdfFileWriter\\ReportPdfFileWriter\\ReportPdfFileWriter\\ReportPdfFileWriterWeeklyReportpdf.pdf");
            return View();
        }

        public ActionResult GenPDF()
        {
            //ArticleExample AE = new ArticleExample();
            //AE.Test(false, "D:\\Rsc\\ReportPdfFileWriter\\ReportPdfFileWriter\\ReportPdfFileWriter\\WeeklyReportpdf.pdf");
            return null;
            //return File(filename, "WeeklyReportpdf");
        }

        



    }
}
