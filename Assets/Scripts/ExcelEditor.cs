using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Excel;
using System.Data;

// Editor
public static class MyEditor
{
    [MenuItem("External tool/excel×ª»»")]
    public static void ExportExcelToTxt()
    {
        // _Excel path
        string assetPath = Application.dataPath + "/_Excel";
        // Get files
        string[] files = Directory.GetFiles(assetPath, "*.xlsx");
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace('\\', '/');
            
            using (FileStream fs = File.Open(files[i], FileMode.Open, FileAccess.Read))
            {
                // FileStream to excel
                var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                // Get excel data
                DataSet dataSet = excelDataReader.AsDataSet();
                DataTable table = dataSet.Tables[0];
                // Read data to txt
                readTableToTxt(files[i], table);
            }
        }
        AssetDatabase.Refresh();
    }

    private static void readTableToTxt(string filePath, DataTable table)
    {
        // Get file name
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        // Save txt
        string path = Application.dataPath + "/Resources/Data/" + fileName + ".txt";
        // Delete duplicate
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        // FileStream create txt
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    DataRow dataRow = table.Rows[row];
                    string str = "";
                    for (int col = 0; col < table.Columns.Count; col++)
                    {
                        string val = dataRow[col].ToString();
                        str = str + val + "\t"; // Splite
                    }

                    sw.Write(str);

                    if (row != table.Rows.Count - 1)
                    {
                        sw.WriteLine();
                    }
                }
            }


        }
    }
}
