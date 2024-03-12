using OfficeOpenXml;
class Program
{
    static void Main()
    {
        var file_list = Directory.GetFiles(".", "街乡*.xlsx");
        var files = Directory.GetFiles(".", "小区*.xlsx");
        var xls_file = files[0];
        if (file_list.Length == 0 || files.Length == 0)
        {
            Console.WriteLine("缺少文件.");
            Environment.Exit(1);
        }

        var sum = new Dictionary<string, int>();
        var online = new Dictionary<string, int>();
        var not_online = new Dictionary<string, int>();

        foreach (var file_name in file_list)
        {
            using (var package = new ExcelPackage(new FileInfo(file_name)))
            {
                var workbook = package.Workbook;
                var sheet = workbook.Worksheets[1];
                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var category = sheet.Cells[row, 2].Text.Split('/').Last();
                    var status = sheet.Cells[row, 4].Text;
                    if (!sum.ContainsKey(category))
                    {
                        sum[category] = 0;
                    }
                    sum[category] += 1;
                    if (!online.ContainsKey(category))
                    {
                        online[category] = 0;
                    }
                    if (!not_online.ContainsKey(category))
                    {
                        not_online[category] = 0;
                    }
                    if (status == "在线" || status == "未检测")
                    {
                        online[category] = online.GetValueOrDefault(category, 0) + 1;
                    }
                    else if (status == "离线" && sheet.Cells[row, 7].Text != "0")
                    {
                        not_online[category] = not_online.GetValueOrDefault(category, 0) + 1;
                    }
                }
            }
        }
        Console.WriteLine("统计完成..");

        using (var file = new ExcelPackage(new FileInfo(xls_file)))
        {
            var sheet = file.Workbook.Worksheets[0];

            for (int row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                var value = sheet.Cells[row, 5].Text;
                if (online.ContainsKey(value))
                {
                    sheet.Cells[row, 11].Value = online[value];
                    sheet.Cells[row, 12].Value = not_online[value];
                    sheet.Cells[row, 13].Value = sum[value];
                }
                else
                {
                    sheet.Cells[row, 11].Value = "平台无设备";
                }
            }
            try
            {
                file.Save();
            }
            catch (IOException)
            {
                Console.WriteLine("结果写入失败,请关闭文件后重试");
                Environment.Exit(1);
            }
        }
        Console.WriteLine("结果写入成功,按enter退出...");
        Console.ReadLine();
    }
}

