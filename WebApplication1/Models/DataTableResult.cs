using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class DataTableResult<T>
    {
        public int sEcho { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<T> aaData { get; set; }
    }
}
