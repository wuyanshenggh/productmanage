using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductMange.DTO;
using ProductMange.Model;
using ProductMange.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Controllers
{
    /// <summary>
    /// 日历管理
    /// </summary>
    [CustomActionFilter(ModelCode = "1004")]
    public class CalendarController : BaseControllerLogin
    {

        public ProductManageDbContext DbContext { get; }


        public CalendarController(ProductManageDbContext _DbContext)
        {
            DbContext = _DbContext;
        }

        /// <summary>
        /// 保存更新日历
        /// </summary>
        [HttpPost]
        [Route("/Calendar/SaveHoliday")]
        public ReturnResponse SaveHoliday([FromForm]DTOHolidayData dto)
        {
            if (dto.Holidays == null)
            {
                dto.Holidays = new List<HolidayItem>();
            }
            Dictionary<int, List<HolidayItem>> yearSplit = new Dictionary<int, List<HolidayItem>>();
            foreach (HolidayItem holiday in dto.Holidays)
            {
                DateTime dt = Convert.ToDateTime(holiday.StrTime);
                if (!yearSplit.ContainsKey(dt.Year))
                {
                    yearSplit.Add(dt.Year, new List<HolidayItem>());
                }
                yearSplit[dt.Year].Add(holiday);
            }
            Repository<Prc_ConfigSet> repositoryCfgSet = new Repository<Prc_ConfigSet>(DbContext);
            Repository<Prc_Holiday> repositoryHoliday = new Repository<Prc_Holiday>(DbContext);

            //保存节假日历
            foreach (int year in yearSplit.Keys)
            {
                Prc_Holiday model = repositoryHoliday.Get(a => !a.IsDelete && a.Year == year);
                List<HolidayItem> yearData = yearSplit[year];
                if (model == null)
                {
                    //如果没有当年，则新加
                    model = new Prc_Holiday();
                    model.Year = year;
                    model.Data = JsonConvert.SerializeObject(new Dictionary<string, Dictionary<string, int>>());
                    repositoryHoliday.Add(model);
                    DbContext.SaveChanges();
                }

                Dictionary<string, Dictionary<string, int>> monthData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(model.Data);
                foreach (HolidayItem holiday in yearData)
                {
                    DateTime dt = DateTime.Parse(holiday.StrTime);
                    string yearMonthStr = dt.ToString("yyyyMM");
                    string dayStr = dt.ToString("dd");
                    if (!monthData.ContainsKey(yearMonthStr))
                    {
                        monthData.Add(yearMonthStr, new Dictionary<string, int>());
                    }
                    if (monthData[yearMonthStr].ContainsKey(dayStr))
                    {
                        if (holiday.Type == 0)
                        {
                            monthData[yearMonthStr].Remove(dayStr);
                        }
                        else
                        {
                            monthData[yearMonthStr][dayStr] = holiday.Type;
                        }
                    }
                    else
                    {
                        if (holiday.Type != 0)
                        {
                            monthData[yearMonthStr].Add(dayStr, holiday.Type);
                        }
                    }
                }
                model.Data = JsonConvert.SerializeObject(monthData);
                repositoryHoliday.Update(model);
            }
            //保存期限配置
            var revDay = repositoryCfgSet.Get(a => a.Code == PrcConfigSet.ReserveDay);
            var revHour = repositoryCfgSet.Get(a => a.Code == PrcConfigSet.ReserveHour);
            if (revDay == null)
            {
                repositoryCfgSet.Add(new Prc_ConfigSet() { Code = PrcConfigSet.ReserveDay, Value = dto.ReserveDay.ToString() });
            }
            else
            {
                revDay.Value = dto.ReserveDay.ToString();
                repositoryCfgSet.Update(revDay);
            }

            if (revHour == null)
            {
                repositoryCfgSet.Add(new Prc_ConfigSet() { Code = PrcConfigSet.ReserveHour, Value = dto.ReserveHour.ToString() });
            }
            else
            {
                revHour.Value = dto.ReserveHour.ToString();
                repositoryCfgSet.Update(revHour);
            }
            DbContext.SaveChanges();

            return new ReturnResponse();
        }

        /// <summary>
        /// 获取更新日历
        /// </summary>
        [HttpPost]
        [Route("/Calendar/GetHoliday")]
        public DTOBackHolidayData GetHoliday([FromForm]DTOGetHolidayData dto)
        {

            Repository<Prc_Holiday> repository = new Repository<Prc_Holiday>(DbContext);
            Prc_Holiday model = repository.Get(a => !a.IsDelete && a.Year == dto.Year);
            string str = string.Empty;
            if (model != null)
            {
                str = model.Data;
            }
            Repository<Prc_ConfigSet> repositoryCfgSet = new Repository<Prc_ConfigSet>(DbContext);
            var revDay = repositoryCfgSet.Get(a => a.Code == PrcConfigSet.ReserveDay);
            var revHour = repositoryCfgSet.Get(a => a.Code == PrcConfigSet.ReserveHour);
            int revDayValue = revDay == null || string.IsNullOrWhiteSpace(revDay.Value) ? 7 : int.Parse(revDay.Value);
            int revHourValue = revHour == null || string.IsNullOrWhiteSpace(revHour.Value) ? 24 : int.Parse(revHour.Value);

            return new DTOBackHolidayData() { StrJson = str, ReserveDay = revDayValue, ReserveHour = revHourValue };

        }

        /// <summary>
        /// 拉取法定日历
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/Calendar/LoadHoliday")]
        public ReturnResponse LoadHoliday(DTOLoadHoliday dto)
        {
            string holidayUrl = Environment.GetEnvironmentVariable("HolidayURL");
            if (string.IsNullOrWhiteSpace(holidayUrl))
            {
                holidayUrl = "http://holiday.youcaihua.net:9955";
            }
            holidayUrl += "/YCH/1.0/DTOGetHoliday";
            DTOHDLoadHoliday hddto = new DTOHDLoadHoliday()
            {
                Year = dto.Year.ToString(),
                BusinessId = "0D842379-F14F-4078-A79C-1E2AD005C381",
                BusinessName = "产品管理平台"
            };
            hddto.SetSign(hddto);
            string jsonstr = "";
            try
            {
                var result = PostHelper.Post<DTOHDLoadHoliday, DTOHDBackHoliday> (hddto, holidayUrl);
                if (result.ResponseStatus.ErrorCode != "0")
                {
                    throw new Exception(result.ResponseStatus.Message);
                }
                else
                {
                    jsonstr = result.HolidayData;
                }
            }
            catch (System.Net.WebException e)
            {
                throw new Exception($"同步失败，请检查服务器网络连接,原因:{e.Message}");
            }

            if (!string.IsNullOrWhiteSpace(jsonstr) && jsonstr.Length > 2)
            {
                Repository<Prc_Holiday> repository = new Repository<Prc_Holiday>(DbContext);
                Prc_Holiday model = repository.Get(a => !a.IsDelete && a.Year == dto.Year);
                if (model == null)
                {
                    model = new Prc_Holiday();
                    model.Year = dto.Year;
                    model.Data = jsonstr;
                    repository.Add(model);
                }
                else
                {
                    model.Data = jsonstr;
                    repository.Update(model);
                }
                DbContext.SaveChanges();

            }
            else
            {
                throw new Exception($"同步失败，{dto.Year}年尚未有节假日数据!");
            }

            return new ReturnResponse();
        }
    }
}
