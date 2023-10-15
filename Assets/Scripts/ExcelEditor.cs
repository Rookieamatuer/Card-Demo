using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;
using ExcelDataReader;

public static class ExcelEditor
{
    [UnityEditor.MenuItem("ExternalTool/Excel×ª»»")]
    public static void ExportExcelToText()
    {
        // Get path of files
        string assetPath = UnityEngine.Application.dataPath + "/Resources/_Excel";
        // Get files
        string[] files = Directory.GetFiles(assetPath, "*.xlsx");

        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace("\\", "/");

            Debug.Log(files[i]);
            using (FileStream fileStream = File.Open(files[i], FileMode.Open, FileAccess.Read))
            {
                var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

                DataSet dataSet = excelDataReader.AsDataSet();

                DataTable dataTable = dataSet.Tables[0];

                ReadTableToText(files[i], dataTable);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("switch files");
    }

    private static void ReadTableToText(string filePath, DataTable dataTable)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);

        string path = Application.dataPath + "/Resources/Data/" + fileName + ".txt";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];

                    string str = "";

                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        string val = row[col].ToString();

                        str = str + val + "\t";
                    }

                    writer.Write(str);

                    if (i != dataTable.Rows.Count - 1)
                    {
                        writer.WriteLine();
                    }
                }
            }
        }
    }
}
