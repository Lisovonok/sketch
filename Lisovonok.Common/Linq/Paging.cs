using System.Runtime.Serialization;
using System.Web.UI.WebControls;

namespace Lisovonok.Common.Linq
{
    [DataContract]
    public class Paging
    {
        public Paging()
            : this(1, 10)
        {
        }

        public Paging(int page, int pageSize)
        {
            SortDirection = SortDirection.Ascending;
            PageIndex = page > 0 ? page - 1 : 0;
            PageSize = pageSize;
        }

        public Paging(int page, int pageSize, string sort)
            : this(page, pageSize)
        {
            SortColumn = sort;
        }

        public Paging(int page, int pageSize, string sort, SortDirection sortDir)
            : this(page, pageSize, sort)
        {
            SortDirection = sortDir;
        }

        [DataMember]
        public SortDirection SortDirection { get; set; }

        [DataMember]
        public string SortColumn { get; set; }

        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        public override string ToString()
        {
            return "SortColumn: " + SortColumn + ", PageIndex: " + PageIndex + ", PageSize: " + PageSize;
        }
    }
}
