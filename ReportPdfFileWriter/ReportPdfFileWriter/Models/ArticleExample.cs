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

        public void Test
                (
                Boolean Debug,
                String FileName
                )
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
            //DrawFrameAndBackgroundWaterMark(Contents);
            //DrawTwoLinesOfHeading(Contents);
            //DrawHappyFace(Contents);
            //DrawBrickPattern(Document, Contents);
            DrawLogo(Document, Contents);
            //DrawHeart(Contents);
            //DrawStar(Contents);
            //DrawTextBox(Contents);
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

        private void DefineFontResources
                (
                PdfDocument Document
                )
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
            return;
        }

        private void DrawLogo
                (
                PdfDocument Document,
                PdfContents Contents
                )
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
      
        private void DrawForm
                (
                PdfContents Contents
                )
        {
            const Double Width = 6.25;
            const Double CenterWidth = 3.15;
            const Double Heigh = 11.75 - 2.25;
            Double HeightTableData = 0.2 + (weeklyreport.Months.Count * 0.2);
            const Double Margin = 0.04;
            const Double FontSize = 9.0;
            const Double FontSizebig = 14.0;
            Double LineSpacing = ArialNormal.LineSpacing(FontSize);
            Double Descent = ArialNormal.Descent(FontSize);
            
            // save graphics state
            Contents.SaveGraphicsState();

            // form line width 0.01"
            Contents.SetLineWidth(0.01);
            //Contents.SetColorStroking(Color.Black);
            // Initial vertical position for contents
            //Double PosY1 = Height - LineSpacing - 2 * Margin;

            // bottom of the contents area of the form
            //Double PosY2 = 2 * Margin + 3 * LineSpacing;

            // shift origin, bottom left of the form to X=4.35" and Y=1.1"กำหนดจุด X Y เริ่มต้น
            Contents.Translate(1, Heigh-1.5);
            //Contents.Translate(1, 8);

            Contents.DrawText(ArialBold, FontSizebig, CenterWidth, 1.5, TextJustify.Center, weeklyreport.ReportName);

            // draw outline rectangle
            Contents.DrawRectangle(0, 0, Width, HeightTableData, PaintOp.CloseStroke);

            // draw two horizontal lines. under table heading and above total
            Contents.SetLineWidth(0.02);
            Contents.DrawLine(0, HeightTableData - 0.2, Width, HeightTableData - 0.2);

            // draw three vertical lines separating the columns
            Contents.SetLineWidth(0.01);
            Contents.DrawLine(0 + 3.25, HeightTableData, 0 + 3.25, 0);
            Contents.DrawLine(0 + 3.25 + 1.5, HeightTableData, 0 + 3.25 + 1.5, 0);

            // draw table heading
            Contents.DrawText(ArialBold, FontSize, (double)(0 + 3.25) / 2 - Margin, HeightTableData - 0.2 + Margin, TextJustify.Center, weeklyreport.LabelMonth);
            Contents.DrawText(ArialBold, FontSize, 0 + 3.25 + 1.5 - Margin, HeightTableData - 0.2 + Margin, TextJustify.Right, weeklyreport.LabelRevenue);
            Contents.DrawText(ArialBold, FontSize, 0 + 3.25 + 1.5 + 1.5 - Margin, HeightTableData - 0.2 + Margin, TextJustify.Right, weeklyreport.LabelForecast);

            Double PosY = HeightTableData - 0.2;

             int i = 1;

            foreach (MonthData item in weeklyreport.Months)
            {
               PosY -= 0.2;
               if (weeklyreport.Months.Count != i)
               {
                   Contents.DrawLine(0, PosY, Width, PosY);
               }

               Contents.DrawText(ArialBold, FontSize, (double)(0 + 3.25) / 2 - Margin, PosY + Margin, TextJustify.Center, item.Month);
               Contents.DrawText(ArialNormal, FontSize, 0 + 3.25 + 1.5 - Margin, PosY + Margin, TextJustify.Right,"$ "+ item.Revenue.ToString("#,###,###,###"));
               Contents.DrawText(ArialNormal, FontSize, 0 + 3.25 + 1.5 + 1.5 - Margin, PosY + Margin, TextJustify.Right, "$ " + item.Forecast.ToString("#,###,###,###"));

               i += 1;
            }

            PosY -= 0.5;
            Contents.DrawText(ArialNormal, FontSize, 0 + 3.25 + 1.5 + 1.5 - Margin, PosY, TextJustify.Right, weeklyreport.LabelFooter);
            PosY -= 0.2;
            Contents.DrawText(ArialNormal, FontSize, 0 + 3.25 + 1.5 + 1.5 - Margin, PosY, TextJustify.Right, weeklyreport.GeneratedOn.ToString("MMMM dd, yyyy"));
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

