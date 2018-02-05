using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.DataBaseUtil.Model
{
    /// <summary>
    /// 表示MongoDB基础属性的实体基类。
    /// </summary>
    public abstract class BaseMongoEntity
    {
        /// <summary>
        /// _id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// 数据状态。
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 数据创建时间。
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 数据更新时间。
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
