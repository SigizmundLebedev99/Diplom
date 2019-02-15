using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Xls
{
    static class XlsBuilder
    {
        public static byte[] GenereateXls<T>(IList<T> objects)
        {
            if (objects == null)
                throw new InvalidOperationException("rows list can not be null");
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("List");

            var props = typeof(T).GetType().GetProperties().Where(e => Attribute.IsDefined(e, typeof(XlsHeaderAttribute))).ToArray();

            for (int i = 0; i < objects.Count(); i++)
            {
                var row = sheet.CreateRow(i + 2);
                for (var y = 0; y < props.Length; y++)
                {
                    row.CreateCell(y).SetCellValue(props[y].GetValue(objects[i])?.ToString());
                }
            }

            var headers = sheet.CreateRow(0);
            headers.RowStyle.Alignment = HorizontalAlignment.Center;
            headers.RowStyle.SetFont(new XSSFFont() { IsBold = true });
            for (var i = 0; i < props.Count(); i++)
            {
                var attr = (XlsHeaderAttribute)Attribute.GetCustomAttribute(props[i], typeof(XlsHeaderAttribute));
                headers.CreateCell(i).SetCellValue(attr.Header);
                sheet.AutoSizeColumn(i);
            }

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            stream.Position = 0;
            workbook.Close();
            return stream.ToArray();
        }
    }
}
