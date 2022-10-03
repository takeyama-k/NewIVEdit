using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using ExcelDataReader;
using System.Data;

namespace NewIVEdit {
	public static class HSCode
	{
		public static SortedList<string, HSCodeElement> HSCodeDictionary = new SortedList<string, HSCodeElement>();
        private static Tuple<int,string>[] HSCodeDataTitle;

		static HSCode()
		{
            HSCodeDataTitle = new Tuple<int,string>[4] { new Tuple<int,string>(0,"HSCode"), new Tuple<int, string>(1, "Checkthumb"), new Tuple<int, string>(3, "Unit1"), new Tuple<int, string>(4, "Unit2") };
			string path = Path.Combine(Application.StartupPath, ConstValue.HSCodeFolder, ConstValue.HSCodeFile);
            int ttlRow = 0;
            Dictionary<string, string[]> rawData = new Dictionary<string, string[]>();
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet().Tables["HSCode"];
                    ttlRow = result.Rows.Count;
                    foreach (var colDef in HSCodeDataTitle)
                    {
                        if (rawData.ContainsKey(colDef.Item2)) continue;
                        rawData.Add(colDef.Item2, new string[ttlRow]);
                    }
                    foreach (var colDef in HSCodeDataTitle) {
                        int rowNo = 0;
                        foreach (var row in result.Rows)
                        {
                            rawData[colDef.Item2][rowNo] = (((DataRow)row)[colDef.Item1]).ToString();
                            rowNo++;
                        }
                    }
                    
                }
            }
            int cntr = 0;
            foreach(var hsItem in rawData["HSCode"])
            {
                HSCodeDictionary.Add(hsItem, new HSCodeElement() { HSCode = rawData["HSCode"][cntr], Checkthumb = rawData["Checkthumb"][cntr], Unit1 = rawData["Unit1"][cntr], Unit2 = rawData["Unit2"][cntr] });
                cntr++;
            }
        }

		public class HSCodeElement{
            public string HSCode { set; get; }
            public string Checkthumb { set; get; }
            public string Unit1 { set; get; }
            public string Unit2 { set; get; }

        }
    }
}
