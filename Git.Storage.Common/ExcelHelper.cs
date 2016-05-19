using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
//using Microsoft.Office.Interop.Excel;

namespace Storage.Common
{
    /// <summary>
    /// EXcel帮助类
    /// </summary>
    public class ExcelHelper
    {
        private static  string GetConnString(string path, string excelVersion)
        {
            string strConn = "";
            if (excelVersion == "2003")
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
            else //if (excelVersion == "2007")
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;";
                strConn += @"Data Source=" + path + ";";
                strConn += "Extended Properties=\"Excel 12.0 Xml;HDR=NO\";";
            }
            return strConn;
        }
        //加载Excel    
        public static DataSet LoadDataFromExcel(string filePath, string SheetName, string top = "", string excelVersion = "2003")
        {
            try
            {
                string strConn = GetConnString(filePath, excelVersion);
                OleDbConnection OleConn = new OleDbConnection(strConn);
                OleConn.Open();
                System.Data.DataTable dt = GetTableName(OleConn);
                if (dt.Rows.Count <= 0)
                    throw new Exception("Excel中没有工作表！");               
                if (string.IsNullOrEmpty(SheetName))
                    throw new Exception("工作表不能为空");
                bool isexistTable = false;
                for (var rowIndex = 0; rowIndex < dt.Rows.Count;rowIndex++ )
                {
                    if (SheetName == Convert.ToString(dt.Rows[rowIndex][2]))
                    {
                        isexistTable = true;
                        break;
                    }
                }
                if (!isexistTable) {
                    throw new Exception("工作表"+SheetName+"不存在");
                }
                String sql = "SELECT "+top+" * FROM  [" + SheetName + "]";//可是更改Sheet名称，比如sheet2，等等    

                OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);
                DataSet OleDsExcle = new DataSet();
                OleDaExcel.Fill(OleDsExcle, SheetName);
                OleConn.Close();
                return OleDsExcle;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public static System.Data.DataTable GetTableName(OleDbConnection oledbconn1)
        {
            System.Data.DataTable dt = oledbconn1.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            return dt;

        }
        /// <summary>
        /// 获得所有的Excel工作表名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static System.Data.DataTable GetTableName(string path,string excelVersion="2003")
        {
            string strConn = GetConnString(path, excelVersion);
            OleDbConnection OleConn = new OleDbConnection(strConn);
            OleConn.Open();
            System.Data.DataTable dt = OleConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            OleConn.Close();
            return dt;

        }
        public static bool SaveDataTableToExcel(System.Data.DataTable excelTable, string filePath)
        {
            //    Microsoft.Office.Interop.Excel.Application app =
            //        new Microsoft.Office.Interop.Excel.ApplicationClass();
            //    try
            //    {
            //        app.Visible = false;
            //        Workbook wBook = app.Workbooks.Add(true);
            //        Worksheet wSheet = wBook.Worksheets[1] as Worksheet;
            //        if (excelTable.Rows.Count > 0)
            //        {
            //            int row = 0;
            //            row = excelTable.Rows.Count;
            //            int col = excelTable.Columns.Count;
            //            for (int i = 0; i < row; i++)
            //            {
            //                for (int j = 0; j < col; j++)
            //                {
            //                    string str = excelTable.Rows[i][j].ToString();
            //                    wSheet.Cells[i + 2, j + 1] = str;
            //                }
            //            }
            //        }

            //        int size = excelTable.Columns.Count;
            //        for (int i = 0; i < size; i++)
            //        {
            //            wSheet.Cells[1, 1 + i] = excelTable.Columns[i].ColumnName;
            //        }
            //        //设置禁止弹出保存和覆盖的询问提示框    
            //        app.DisplayAlerts = false;
            //        app.AlertBeforeOverwriting = false;
            //        //保存工作簿    
            //        wBook.Save();
            //        //保存excel文件    
            //        app.Save(filePath);
            //        app.SaveWorkspace(filePath);
            //        app.Quit();
            //        app = null;
            return true;
            //    }
            //    catch (Exception err)
            //    {
            //        throw err;
            //    }
            //    finally
            //    {
            //    }
        }
    }
}
