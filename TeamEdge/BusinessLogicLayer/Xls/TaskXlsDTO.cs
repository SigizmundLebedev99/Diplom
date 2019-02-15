using System;
using TeamEdge.BusinessLogicLayer.Xls;

namespace TeamEdge.Models
{
    public class TaskXlsDTO
    {
        [XlsHeader("Идентификатор")]
        public string Number { get; set; }
        [XlsHeader("Название")]
        public string Name { get; set; }
        [XlsHeader("Дата начала")]
        public DateTime DateStart { get; set; }
        [XlsHeader("Дата окончания")]
        public DateTime DateFinish { get; set; }
        
    }
}
