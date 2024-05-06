using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using KaibaSystem_Back_End.Extensions.Helpers;
using KaibaSystem_Back_End.Services.DevBoard.Interface;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace KaibaSystem_Back_End.Services.DevBoard
{
    public class DevBoardService : IDevBoardService
    {
        public String ConvertExcelToJSON(String Path)
        {
            var dt = new DataTable();
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(Path, false))
            {
                //Read the first Sheet from Excel file.
                Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

                //Get the Worksheet instance.
                Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                //Fetch all the rows present in the Worksheet.
                IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                //Loop through the Worksheet rows.
                foreach (Row row in rows)
                {
                    //Use the first row to add columns to DataTable.
                    if (row.RowIndex.Value == 1)
                    {
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            dt.Columns.Add(GetCellValue(doc, cell));
                        }
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = GetCellValue(doc, cell);
                            i++;
                        }
                    }
                }
            }

            return dt.DataTableToJSON();
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue?.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }

        public static int GetIndex(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return -1;
            }

            int index = 0;
            foreach (var ch in name)
            {
                if (char.IsLetter(ch))
                {
                    int value = ch - 'A' + 1;
                    index = value + index * 26;
                }
                else
                {
                    break;
                }
            }

            return index - 1;
        }
    }
}