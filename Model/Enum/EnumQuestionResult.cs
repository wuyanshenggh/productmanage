using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model.Enum
{
    public enum EnumQuestionResult
    {
        /// <summary>
        /// 无结果
        /// </summary>
        None = 0,
        /// <summary>
        /// 忽略
        /// </summary>
        Ignore = 1,
        /// <summary>
        /// 重试
        /// </summary>
        ReTry = 2,
        /// <summary>
        /// 终止
        /// </summary>
        Stop = 3
    }
}
