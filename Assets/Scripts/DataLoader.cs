using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using OfficeOpenXml;

public class DataLoader : MonoBehaviour
{
    public static DataLoader instance;

    public List<List<string>> modelData;

    public Dictionary<int, GameObject> modelPrefabDictionary = new Dictionary<int, GameObject>();

    public Dictionary<string, Material> materialDictionary = new Dictionary<string, Material>();

    string value;
    //string all;

    private void Awake()
    {
        instance = this;

        modelData = LoadData("UnitModel.xlsx");
    }

    /// <summary>
    ///返回数据的集合 
    ///数据的格式为 每一行为一条数据
    ///例：赵一|党员|1年|赵一.png| 
    /// </summary>
    /// <returns></returns>
    public List<List<string>> LoadData(string fileName)
    {
        // StreamingAssets目录下的  党员信息.xlsx文件的路径：Application.streamingAssetsPath + "/党员信息.xlsx" 
        FileStream fileStream = File.Open(Application.streamingAssetsPath + "/" + fileName, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        // 表格数据全部读取到result里
        DataSet result = excelDataReader.AsDataSet();

        // 获取表格有多少列 
        int columns = result.Tables[0].Columns.Count;
        // 获取表格有多少行 
        int rows = result.Tables[0].Rows.Count;
        // 根据行列依次打印表格中的每个数据 

        List<List<string>> excelData = new List<List<string>>();

        //第一行为表头，不读取
        for (int i = 1; i < rows; i++)
        {
            value = null;
            //all = null;
            excelData.Add(new List<string>());
            for (int j = 0; j < columns; j++)
            {
                // 获取表格中指定行指定列的数据 
                value = result.Tables[0].Rows[i][j].ToString();
                excelData[i - 1].Add(value);
            }
        }
        return excelData;
    }

    /// <summary>
    /// list内容格式
    /// 赵一|党员|1年|赵一.png| 
    /// </summary>
    /// <param name="newList"></param>
    public void WriteExcel(List<string> newList, string fileName)
    {
        //自定义excel的路径

        string path = Application.streamingAssetsPath + fileName;
        // print(Application.dataPath);

        FileInfo newFile = new FileInfo(path);

        if (newFile.Exists)
        {
            //创建一个新的excel文件

            newFile.Delete();

            newFile = new FileInfo(path);
        }

        //通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            //在excel空文件添加新sheet

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("message");
            //添加列名

            worksheet.Cells[1, 1].Value = "姓名";

            worksheet.Cells[1, 2].Value = "职务";

            worksheet.Cells[1, 3].Value = "党龄";

            worksheet.Cells[1, 4].Value = "图片名";

            for (int i = 0; i < newList.Count; i++)
            {
                string[] messages = newList[i].Split('|'); //赵一|党员|1年|赵一.png| 
                string itemName = messages[0];
                string itemWork = messages[1];
                string itemYear = messages[2];
                string imageName = messages[3];
                //添加一行数据

                int num = i + 2;

                worksheet.Cells["A" + num].Value = itemName;

                worksheet.Cells["B" + num].Value = itemWork;

                worksheet.Cells["C" + num].Value = itemYear;

                worksheet.Cells["D" + num].Value = imageName;
            }

            //保存excel
            package.Save();
            print("重写完成");
        }
    }

    /// <summary>
    /// 获取对应ID的mpdelPrefab
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetModelPrefab(int id)
    {
        if (modelPrefabDictionary.ContainsKey(id))
        {
            return modelPrefabDictionary[id];
        }
        else
        {
            GameObject temp = Resources.Load(GetModelPath(id)) as GameObject;
            modelPrefabDictionary.Add(id, temp);
            return temp;
        
        }
    }
    
    private string GetModelPath(int id)
    {
        int dataIndex = -1;
        for (int i = 0; i < modelData.Count; i++)
        {
            if (Tools.String2Int(modelData[i][0]).Equals(id))
            {
                dataIndex = i;
                break;
            }
        }
        if (dataIndex != -1)
        {
            return modelData[dataIndex][1];
        }
        else return "UnitModel/Default";
    }

    /// <summary>
    /// 获取材质
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Material GetMaterial(string name)
    {
        if (materialDictionary.ContainsKey(name))
        {
            return materialDictionary[name];
        }
        else
        {
            Material temp = Resources.Load("Materials/" + name) as Material;
            materialDictionary.Add(name, temp);
            return temp;

        }
    }
}
