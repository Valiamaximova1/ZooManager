using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using BusinessLayer.DTOs;
    using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ExcelExporter
    {
        public async Task ExportEventsToExcel(List<EventDto> filteredEvents, string filePath)
        {
           
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Events");

            // Заглавия на колоните
            worksheet.Cell(1, 1).Value = "Заглавие";
            worksheet.Cell(1, 2).Value = "Тип";
            worksheet.Cell(1, 3).Value = "Дата";
            worksheet.Cell(1, 4).Value = "Описание";
            worksheet.Cell(1, 5).Value = "Животни";
            worksheet.Cell(1, 6).Value = "Брой билети";

            int row = 2;

            foreach (var ev in filteredEvents)
            {
                worksheet.Cell(row, 1).Value = ev.Title;
                worksheet.Cell(row, 2).Value = ev.Type.ToString();
                worksheet.Cell(row, 3).Value = ev.Date.ToShortDateString();
                worksheet.Cell(row, 4).Value = ev.Description;
                worksheet.Cell(row, 5).Value = string.Join(", ", ev.AnimalNamesCollection);
                worksheet.Cell(row, 6).Value = ev.TicketCount;

                row++;
            }

            // Автоматично оразмеряване на колоните
            worksheet.Columns().AdjustToContents();

            // Записване на файла
            try
            {
                workbook.SaveAs(filePath);
            }
            catch (IOException)
            {
                // Опит за автоматично затваряне на отворен файл чрез Excel (само ако се казва така)
                CloseExcelIfFileOpen(filePath);

                //чакаме да се освободи
                await Task.Delay(1000);

                // Опит за запис
                workbook.SaveAs(filePath);
            }
        }
        private void CloseExcelIfFileOpen(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            var processes = System.Diagnostics.Process.GetProcessesByName("EXCEL");
            foreach (var process in processes)
            {
                try
                {
                    if (process.MainWindowTitle.Contains(fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                catch
                {
                    
                }
            }
        }

    }
}
