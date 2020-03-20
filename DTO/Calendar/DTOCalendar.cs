using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO
{
    /// <summary>
    /// 保存日历
    /// </summary>
    public class DTOHolidayData : BaseRequest
    {
        public List<HolidayItem> Holidays { get; set; }

        /// <summary>
        /// 预约提前时间（小时）
        /// </summary>
        public int ReserveHour { get; set; }

        /// <summary>
        /// 预约最大时间(天)
        /// </summary>
        public int ReserveDay { get; set; }
    }

    public class HolidayItem
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string StrTime { get; set; }
        /// <summary>
        /// 日期类型
        /// </summary>
        public int Type { get; set; }
    }

    /// <summary>
    /// 查询日历数据
    /// </summary>
    public class DTOGetHolidayData : BaseRequest
    {
        public int Year { get; set; }
    }

    /// <summary>
    /// 返回日历数据
    /// </summary>
    public class DTOBackHolidayData: ReturnResponse
    {
        /// <summary>
        /// 日历数据
        /// </summary>
        public string StrJson { get; set; }

        /// <summary>
        /// 预约提前时间（小时）
        /// </summary>
        public int ReserveHour { get; set; }

        /// <summary>
        /// 预约最大时间(天)
        /// </summary>
        public int ReserveDay { get; set; }
    }

    /// <summary>
    /// 获取节假日
    /// </summary>
    public class DTOLoadHoliday : BaseRequest
    {
        public int Year { get; set; }
    }

    /// <summary>
    /// 节假日平台，获取节假日
    /// </summary>
    public class DTOHDLoadHoliday : DTOHolidayBaseRequest
    {
        public string Year { get; set; }
    }

    public class DTOHDBackHoliday : ReturnResponse
    {
        public string HolidayData { get; set; }
    }
}
