using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.RepoV1.Base
{
    public class PageResultDto<T>
    {

        /// <summary>
        /// 目前頁數
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁顯示數
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 總計資料筆數
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 總計頁數
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (Limit == 0)
                {
                    return 0;
                }
                return Convert.ToInt32(Math.Ceiling((decimal)Count / Limit));
            }
        }

        /// <summary>
        /// 回傳結果
        /// </summary>
        public IEnumerable<T> Results { get; set; }

    }
}
