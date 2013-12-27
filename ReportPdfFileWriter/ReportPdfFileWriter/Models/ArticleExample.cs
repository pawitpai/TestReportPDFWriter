using System;
using System.Diagnostics;
using System.Drawing;
using PdfFileWriter;
using System.Web;
using ReportPdfFileWriter.Models;

namespace TestPdfFileWriter
{
    public class ArticleExample
    {
        private PdfFont ArialNormal;
        private PdfFont ArialBold;
        private PdfFont ArialItalic;
        private PdfFont ArialBoldItalic;
        private PdfFont TimesNormal;
        private PdfFont Comic;

        WeeklyReport weeklyreport = new WeeklyReport();

        public void Test(Boolean Debug, String FileName)
        {
            WeeklyreportTempList();
            // Step 1: Create empty document
            // Arguments: page width: 8.5”, page height: 11”, Unit of measure: inches
            // Return value: PdfDocument main class
            PdfDocument Document = new PdfDocument(8.25, 11.75, UnitOfMeasure.Inch);

            // Debug property
            // By default it is set to false. Use it for debugging only.
            // If this flag is set, PDF objects will not be compressed, font and images will be replaced
            // by text place holder. You can view the file with a text editor but you cannot open it with PDF reader.
            Document.Debug = Debug;

            // Step 2: create resources
            // define font resources
            DefineFontResources(Document);

            // define tiling pattern resources
            //DefineTilingPatternResource(Document);

            // Step 3: Add new page
            PdfPage Page = new PdfPage(Document);

            // Step 4:Add contents to page
            PdfContents Contents = new PdfContents(Page);

            // Step 5: add graphices and text contents to the contents object
            DrawLogo(Document, Contents);
            DrawForm(Contents);

            // Step 6: create pdf file
            // argument: PDF file name
            Document.CreateFile(FileName);

            // start default PDF reader and display the file
            Process Proc = new Process();
            Proc.StartInfo = new ProcessStartInfo(FileName);
            Proc.Start();

            // exit
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Define Font Resources
        ////////////////////////////////////////////////////////////////////

        private void DefineFontResources(PdfDocument Document)
        {
            // Define font resources
            // Arguments: PdfDocument class, font family name, font style, embed flag
            // Font style (must be: Regular, Bold, Italic or Bold | Italic) All other styles are invalid.
            // Embed font. If true, the font file will be embedded in the PDF file.
            // If false, the font will not be embedded
            ArialNormal = new PdfFont(Document, "Arial", FontStyle.Regular, true);
            ArialBold = new PdfFont(Document, "Arial", FontStyle.Bold, true);
            ArialItalic = new PdfFont(Document, "Arial", FontStyle.Italic, true);
            ArialBoldItalic = new PdfFont(Document, "Arial", FontStyle.Bold | FontStyle.Italic, true);
            TimesNormal = new PdfFont(Document, "Times New Roman", FontStyle.Regular, true);
            Comic = new PdfFont(Document, "Comic Sans MS", FontStyle.Bold, true);

            // substitute one character for another
            // this program supports characters 32 to 126 and 160 to 255
            // if a font has a character outside these ranges that is required by the application,
            // you can replace an unused character with this character
            ArialNormal.CharSubstitution(9679, 9679, 164);
            //ArialNormal.CharSubstitution(3585, 3675, 164);
            return;
        }

        private void DrawLogo(PdfDocument Document, PdfContents Contents)
        {
            // define local image resources
            PdfImage Image1 = new PdfImage(Document, weeklyreport.Logo);

            // image size will be limited to 1.4" by 1.4"
            SizeD ImageSize = Image1.ImageSize(1.3, 0.4);

            // save graphics state
            Contents.SaveGraphicsState();

            // translate coordinate origin to the center of the picture
            Contents.Translate(1.7, 11.75 - 1.25);

            // clipping path
            //Contents.DrawOval(-ImageSize.Width / 2, -ImageSize.Height / 2, ImageSize.Width, ImageSize.Height, PaintOp.ClipPathEor);

            // draw image
            Contents.DrawImage(Image1, -ImageSize.Width / 2, -ImageSize.Height / 2, ImageSize.Width, ImageSize.Height);

            // restore graphics state
            Contents.RestoreGraphicsState();
            return;
        }
      
        private void DrawForm(PdfContents Contents)
        {
            const Double Width = 6.25;
            const Double CenterWidth = 3.15;
            const Double Heigh = 11.75 - 2.25;//9.5
            double HeightReportName = 1.5;
            double HeightRow = 0.2;
            Double HeightTableData = HeightRow + (weeklyreport.Months.Count * HeightRow);
            const Double Margin = 0.04;
            const Double FontSize = 9.0;
            const Double FontSizebig = 14.0;
            
            // save graphics state
            Contents.SaveGraphicsState();

            Contents.Translate(1, Heigh);//Heigh9.5

            //ReportName
            Contents.DrawText(ArialBold, FontSizebig, CenterWidth, 0, TextJustify.Center, weeklyreport.ReportName);
            
            // draw outline rectangle//Heigh8
            Contents.SetLineWidth(0.01);
            Contents.DrawRectangle(0, 0 - HeightReportName, Width, HeightTableData, PaintOp.CloseStroke);

            Double PosY1 = HeightTableData- HeightReportName;
            Double PosY2 = 0 - HeightReportName;
            Double PosX1 = 0 + 3.25;
            Double PosX2 = 0 + 3.25 + 1.5;
            Double PosX3 = 0 + 3.25 + 1.5 + 1.5;
            Contents.SetLineWidth(0.01);

            Contents.DrawLine(PosX1, PosY1, PosX1, PosY2);
            Contents.DrawLine(PosX2, PosY1, PosX2, PosY2);

            PosY1 = PosY1 - HeightRow;

            Contents.SetLineWidth(0.02);
            Contents.DrawLine(0, PosY1, Width, HeightTableData - HeightRow - HeightReportName);

            // draw table heading
            Contents.SetLineWidth(0.01);
            Contents.DrawText(ArialBold, FontSize, PosX1 / 2 - Margin, PosY1 + Margin, TextJustify.Center, weeklyreport.LabelMonth);
            Contents.DrawText(ArialBold, FontSize, PosX2 - Margin, PosY1 + Margin, TextJustify.Right, weeklyreport.LabelRevenue);
            Contents.DrawText(ArialBold, FontSize, PosX3 - Margin, PosY1 + Margin, TextJustify.Right, weeklyreport.LabelForecast);

            int i = 1;

            foreach (MonthData item in weeklyreport.Months)
            {
               PosY1 -= 0.2;
               if (weeklyreport.Months.Count != i)
               {
                   Contents.DrawLine(0, PosY1, Width, PosY1);
               }

               Contents.DrawText(ArialBold, FontSize, PosX1 / 2 - Margin, PosY1 + Margin, TextJustify.Center, item.Month);
               Contents.DrawText(ArialNormal, FontSize, PosX2 - Margin, PosY1 + Margin, TextJustify.Right, "$ " + item.Revenue.ToString("#,###,###,###"));
               Contents.DrawText(ArialNormal, FontSize, PosX3 - Margin, PosY1 + Margin, TextJustify.Right, "$ " + item.Forecast.ToString("#,###,###,###"));

               i += 1;
            }

            PosY1 -= 0.5;
            Contents.DrawText(ArialNormal, FontSize, PosX3 - Margin, PosY1, TextJustify.Right, weeklyreport.LabelFooter);
            PosY1 -= 0.2;
            Contents.DrawText(ArialNormal, FontSize, PosX3 - Margin, PosY1, TextJustify.Right, weeklyreport.GeneratedOn.ToString("MMMM dd, yyyy"));
            // restore graphics state
            Contents.RestoreGraphicsState();
            return;
        }

        public void WeeklyreportTempList()
        {
            MonthData Temp = new MonthData();
            Temp.Month = "NOVEMBER 2013";
            Temp.Revenue = 789000;
            Temp.Forecast = 789000;
            weeklyreport.Months.Add(Temp);

            MonthData Temp2 = new MonthData();
            Temp2.Month = "DECEMBER 2013";
            Temp2.Revenue = 79000;
            Temp2.Forecast = 99000;
            weeklyreport.Months.Add(Temp2);

            MonthData Temp3 = new MonthData();
            Temp3.Month = "JANUARY 2014";
            Temp3.Revenue = 89000;
            Temp3.Forecast = 59000;
            weeklyreport.Months.Add(Temp3);

            MonthData Temp4 = new MonthData();
            Temp4.Month = "FEBRUARY 2014";
            Temp4.Revenue = 79000;
            Temp4.Forecast = 69000;
            weeklyreport.Months.Add(Temp4);

            MonthData Temp5 = new MonthData();
            Temp5.Month = "MARCH 2014";
            Temp5.Revenue = 78000;
            Temp5.Forecast = 29000;
            weeklyreport.Months.Add(Temp5);

            weeklyreport.Logo = HttpRuntime.AppDomainAppPath + "\\Pic\\logo.jpg";
            weeklyreport.ReportName = "WEEKLY REPORT";
            weeklyreport.LabelMonth = "MONTH";
            weeklyreport.LabelRevenue = "REVENUE";
            weeklyreport.LabelForecast = "FORECAST";
            weeklyreport.LabelFooter = "Generated by PRIMES©";
            weeklyreport.GeneratedOn = DateTime.UtcNow;

        }
    }
}

