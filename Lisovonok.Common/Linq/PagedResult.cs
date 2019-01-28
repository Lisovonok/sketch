using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lisovonok.Common.Linq
{
    [DataContract]
    public class PagedResult<T> : IEnumerable<T>
    {
        public PagedResult()
        {
        }

        public PagedResult(IEnumerable<T> items, int totalNumberOfItems, Paging paging)
        {
            Items = items;
            TotalNumberOfItems = totalNumberOfItems;
            Paging = paging;
        }

        [DataMember]
        public IEnumerable<T> Items { get; private set; }

        [DataMember]
        public int TotalNumberOfItems { get; private set; }

        [IgnoreDataMember]
        public int TotalNumberOfPages
        {
            get
            {
                if (Paging == null || Paging.PageSize <= 0)
                {
                    return 0;
                }

                return (int)Math.Ceiling((double)TotalNumberOfItems / Paging.PageSize);
            }
        }

        [DataMember]
        public Paging Paging { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
