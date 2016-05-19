/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-02-19 9:21:41
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-02-19 9:21:41       情缘
*********************************************************************************/

using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Git.Storage.Common.Excel
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    public class AsposeExcel
    {

        private string outFileName = "";
        private string fullFilename = "";
        private Workbook book = null;
        private Worksheet sheet = null;

        private Action<Cell> action = (Cell cellItem) =>
            {
                cellItem.Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                cellItem.Style.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.Black;

                cellItem.Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                cellItem.Style.Borders[BorderType.TopBorder].Color = System.Drawing.Color.Black;

                cellItem.Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                cellItem.Style.Borders[BorderType.LeftBorder].Color = System.Drawing.Color.Black;

                cellItem.Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                cellItem.Style.Borders[BorderType.RightBorder].Color = System.Drawing.Color.Black;
            };

        /// <summary>
        /// //导出构造数
        /// </summary>
        /// <param name="outfilename"></param>
        /// <param name="tempfilename"></param>
        public AsposeExcel(string outfilename, string tempfilename)
        {
            outFileName = outfilename;
            book = new Workbook();
            // book.Open(tempfilename);这里我们暂时不用模板
            sheet = book.Worksheets[0];

        }
        /// <summary>
        /// //导入构造数
        /// </summary>
        /// <param name="fullfilename"></param>
        public AsposeExcel(string fullfilename)
        {
            fullFilename = fullfilename;
        }

        private void AddTitle(string title, int columnCount)
        {
            sheet.Cells.Merge(0, 0, 2, columnCount);
            Cell cell1 = sheet.Cells[0, 0];
            cell1.PutValue(title);
            cell1.Style.HorizontalAlignment = TextAlignmentType.Center;
            //cell1.Style.Font.Color = System.Drawing.Color.Blue;
            cell1.Style.Font.Size = 20;
            cell1.Style.Font.IsBold = true;

        }

        private void AddHeader(DataTable dt)
        {

            for (int col = 0; col < dt.Columns.Count; col++)
            {
                action(sheet.Cells[2, col]);
                sheet.Cells[2, col].PutValue(dt.Columns[col].ColumnName);
                sheet.Cells[2, col].Style.Font.IsBold = true;
                // sheet.Cells[2, col].Style.Font.Size = 13;
            }
        }

        private void AddBody(DataTable dt)
        {
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    action(sheet.Cells[r + 3, c]);
                    sheet.Cells[r + 3, c].PutValue(dt.Rows[r][c].ToString());

                }
            }
        }
        /// <summary>
        /// //导出------------
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Boolean DatatableToExcel(DataTable dt, string sheetName, string title)
        {
            Boolean yn = false;
            try
            {
                sheet.Name = sheetName;

                AddTitle(title, dt.Columns.Count);
                AddHeader(dt);
                AddBody(dt);
                sheet.AutoFitColumns();
                book.Save(outFileName);
                yn = true;
                return yn;
            }
            catch (Exception e)
            {
                return yn;
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public DataTable ExcelToDatatalbe(string sheetName)
        {
            Workbook book = new Workbook();
            book.Open(fullFilename);
            Worksheet sheet = book.Worksheets[sheetName];
            Cells cells = sheet.Cells;
            DataTable dt_Import = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            return dt_Import;
        }
    }
}
