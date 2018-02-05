using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CPJIT.Library.CPJ4net.DataBaseUtil
{
    /// <summary>
    /// 提供MongoDB的数据库交互工具接口。
    /// </summary>
    public interface IMongoDBAccess
    {
        /// <summary>
        /// 新增一条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="entity">数据实体对象。</param>
        void Insert<T>(string collectionName, T entity);

        /// <summary>
        /// 新增多条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="entities">数据实体对象。</param>
        void InsertMany<T>(string collectionName, IEnumerable<T> entities);

        /// <summary>
        /// 删除一条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <returns></returns>
        long Delete<T>(string collectionName, Expression<Func<T, bool>> filter);

        /// <summary>
        /// 删除一条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <returns></returns>
        long Delete<T>(string collectionName, FilterDefinition<T> filter);

        /// <summary>
        /// 删除一条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <returns></returns>
        long Delete<T>(string collectionName, IDictionary<string, object> filter);

        /// <summary>
        /// 删除多条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <returns></returns>
        long DeleteMany<T>(string collectionName, Expression<Func<T, bool>> filter);

        /// <summary>
        /// 删除多条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <returns></returns>
        long DeleteMany<T>(string collectionName, FilterDefinition<T> filter);

        /// <summary>
        /// 删除多条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <returns></returns>
        long DeleteMany<T>(string collectionName, IDictionary<string, object> filter);

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        long Update<T>(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update);

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        long Update<T>(string collectionName, Expression<Func<T, bool>> filter, IDictionary<string, object> update);

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        long Update<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update);

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        long Update<T>(string collectionName, FilterDefinition<T> filter, IDictionary<string, object> update);

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        long Update<T>(string collectionName, IDictionary<string, object> filter, UpdateDefinition<T> update);

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        long Update<T>(string collectionName, IDictionary<string, object> filter, IDictionary<string, object> update);

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        long UpdateMany<T>(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update);

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        long UpdateMany<T>(string collectionName, Expression<Func<T, bool>> filter, IDictionary<string, object> update);

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        long UpdateMany<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update);

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        long UpdateMany<T>(string collectionName, FilterDefinition<T> filter, IDictionary<string, object> update);

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        long UpdateMany<T>(string collectionName, IDictionary<string, object> filter, UpdateDefinition<T> update);

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        long UpdateMany<T>(string collectionName, IDictionary<string, object> filter, IDictionary<string, object> update);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <returns></returns>
        T Select<T>(string collectionName, Expression<Func<T, bool>> filter);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <returns></returns>
        T Select<T>(string collectionName, FilterDefinition<T> filter);

        /// <summary>
        /// 查询数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <returns></returns>
        T Select<T>(string collectionName, IDictionary<string, object> filter);

        /// <summary>
        /// 查询多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <returns></returns>
        IList<T> SelectMany<T>(string collectionName, Expression<Func<T, bool>> filter);

        /// <summary>
        /// 查询多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <returns></returns>
        IList<T> SelectMany<T>(string collectionName, FilterDefinition<T> filter);

        /// <summary>
        /// 查询多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="dictionary">筛选条件。</param>
        /// <returns></returns>
        IList<T> SelectMany<T>(string collectionName, IDictionary<string, object> dictionary);
    }
}
