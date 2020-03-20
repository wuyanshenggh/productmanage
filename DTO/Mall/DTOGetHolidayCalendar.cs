using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    public class DTOGetHolidayCalendar: BaseRequestMall
    {
        /// <summary>
        /// 年份
        /// </summary>
        public List<int> Years { get; set; }
    }

    /// <summary>
    /// 返回日历数据
    /// </summary>
    public class DTOBackHolidayCalendar : ReturnResponse
    {
        /// <summary>
        /// 日历数据
        /// </summary>
        public Dictionary<int,string> HolidayJsons { get; set; }

        /// <summary>
        /// 预约提前时间（小时）
        /// </summary>
        public int ReserveHour { get; set; }

        /// <summary>
        /// 预约最大时间(天)
        /// </summary>
        public int ReserveDay { get; set; }
    }
}
