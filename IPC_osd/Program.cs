using System.IO;
using OfficeOpenXml;

var directory = new DirectoryInfo(".");
var files = directory.GetFiles("./../一机*.xlsx");
Console.WriteLine(Environment.CurrentDirectory);
var file = files[0];
var package = new ExcelPackage(file);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var newPackage = new ExcelPackage();
var newSheet = newPackage.Workbook.Worksheets.Add("Sheet1");
var row_sum = 3 + 2;    //新表从第三行开始写入
foreach (var sheet in package.Workbook.Worksheets)
{
    Console.WriteLine(sheet.Name);
    for (int row = 3; row <= sheet.Dimension.End.Row; row++)
    {
        if (sheet.Cells[row, 2].Value != null) // Check if the second column in the current row is not empty
        {
            for (int col = 2; col <= 39; col++)
            {
                var cellValue = sheet.Cells[row, col].Value;
                newSheet.Cells[row_sum - 2, col].Value = cellValue;
            }
            row_sum++;
        }
    }
}
Console.WriteLine(row_sum);
newPackage.SaveAs(new FileInfo("./../数据表.xlsx"));

var firstSheet = package.Workbook.Worksheets[1];
for (int row = 1; row <= 2; row++)
{
    for (int col = 1; col <= firstSheet.Dimension.End.Column; col++)
    {
        var cellValue = firstSheet.Cells[row, col].Value;
        newSheet.Cells[row, col].Value = cellValue;
    }
}

newPackage.SaveAs(new FileInfo("./../数据表.xlsx"));

