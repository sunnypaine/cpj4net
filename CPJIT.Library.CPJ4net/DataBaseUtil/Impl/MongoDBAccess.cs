using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CPJIT.Library.CPJ4net.DataBaseUtil.Impl
{
    /// <summary>
    /// 提供MongoDB的数据库交互工具。
    /// </summary>
    public class MongoDBAccess : IMongoDBAccess
    {
        #region 私有变量
        /// <summary>
        /// 表示MongoDB中的一个数据库对象。
        /// </summary>
        private IMongoDatabase dataBase;
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="ipports">MongoDB服务主机地址。</param>
        /// <param name="dbName">数据库名称。区分大小写。</param>
        public MongoDBAccess(string[] ipports, string dbName)
            : this(ipports, dbName, 30) { }

        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="ipports">MongoDB服务主机地址。</param>
        /// <param name="dbName">数据库名称。区分大小写。</param>
        /// <param name="connectTimeout">连接服务的超时时长。单位：秒。</param>
        public MongoDBAccess(string[] ipports, string dbName, int connectTimeout)
        {
            if (ipports == null || ipports.Length <= 0)
            {
                throw new ArgumentNullException("指定的参数ipports为null或不合法。");
            }
            if (string.IsNullOrWhiteSpace(dbName))
            {
                throw new ArgumentNullException("指定的参数dbName为null或不合法。");
            }

            IList<MongoServerAddress> addresses = new List<MongoServerAddress>();
            foreach (string item in ipports)
            {
                string[] ipport = item.Split(':');
                addresses.Add(new MongoServerAddress(ipport[0], int.Parse(ipport[1])));
            }

            MongoClientSettings setting = new MongoClientSettings();
            setting.Servers = addresses;
            setting.ConnectTimeout = new TimeSpan(connectTimeout * TimeSpan.TicksPerSecond);

            IMongoClient client = new MongoClient(setting);
            this.dataBase = client.GetDatabase(dbName);
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 新增一条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="entity">数据实体对象。</param>
        public void Insert<T>(string collectionName, T entity)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                collection.InsertOne(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 新增多条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="entities">数据实体对象。</param>
        public void InsertMany<T>(string collectionName, IEnumerable<T> entities)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                collection.InsertMany(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除一条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <returns></returns>
        public long Delete<T>(string collectionName, Expression<Func<T, bool>> filter)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                DeleteResult dr = collection.DeleteOne(filter);
                return dr.DeletedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除一条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <returns></returns>
        public long Delete<T>(string collectionName, FilterDefinition<T> filter)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                DeleteResult dr = collection.DeleteOne(filter);
                return dr.DeletedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除一条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <returns></returns>
        public long Delete<T>(string collectionName, IDictionary<string, object> filter)
        {
            try
            {
                BsonDocument bsonDoc = new BsonDocument(filter);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                DeleteResult dr = collection.DeleteOne(bsonDoc);
                return dr.DeletedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除多条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <returns></returns>
        public long DeleteMany<T>(string collectionName, Expression<Func<T, bool>> filter)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                DeleteResult dr = collection.DeleteMany(filter);
                return dr.DeletedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除多条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <returns></returns>
        public long DeleteMany<T>(string collectionName, FilterDefinition<T> filter)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                DeleteResult dr = collection.DeleteMany(filter);
                return dr.DeletedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除多条数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <returns></returns>
        public long DeleteMany<T>(string collectionName, IDictionary<string, object> filter)
        {
            try
            {
                BsonDocument bsonDocFilter = new BsonDocument(filter);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                DeleteResult dr = collection.DeleteMany(bsonDocFilter);
                return dr.DeletedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        public long Update<T>(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateOne(filter, update);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        public long Update<T>(string collectionName, Expression<Func<T, bool>> filter, IDictionary<string, object> update)
        {
            try
            {
                BsonDocument bsonDoc = new BsonDocument(update);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateOne(filter, bsonDoc);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        public long Update<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateOne(filter, update);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        public long Update<T>(string collectionName, FilterDefinition<T> filter, IDictionary<string, object> update)
        {
            try
            {
                BsonDocument bsonDocUpdate = new BsonDocument(update);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateOne(filter, bsonDocUpdate);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        public long Update<T>(string collectionName, IDictionary<string, object> filter, UpdateDefinition<T> update)
        {
            try
            {
                BsonDocument bsonDocFilter = new BsonDocument(filter);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateOne(bsonDocFilter, update);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB集合的名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        public long Update<T>(string collectionName, IDictionary<string, object> filter, IDictionary<string, object> update)
        {
            try
            {
                BsonDocument bsonDocFilter = new BsonDocument(filter);
                BsonDocument bsonDocUpdate = new BsonDocument(update);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateOne(bsonDocFilter, bsonDocUpdate);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        public long UpdateMany<T>(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateMany(filter, update);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        public long UpdateMany<T>(string collectionName, Expression<Func<T, bool>> filter, IDictionary<string, object> update)
        {
            try
            {
                BsonDocument bsonDocUpdate = new BsonDocument(update);
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateMany(filter, bsonDocUpdate);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        public long UpdateMany<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateMany(filter, update);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        public long UpdateMany<T>(string collectionName, FilterDefinition<T> filter, IDictionary<string, object> update)
        {
            try
            {
                BsonDocument bsonUpdate = new BsonDocument(update);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateMany(filter, bsonUpdate);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <param name="update">将要更新的内容。表达式示例：Builders&lt;T&gt;.Update.Set(string key, object value)。</param>
        /// <returns></returns>
        public long UpdateMany<T>(string collectionName, IDictionary<string, object> filter, UpdateDefinition<T> update)
        {
            try
            {
                BsonDocument bsonDocFilter = new BsonDocument(filter);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateMany(bsonDocFilter, update);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <param name="update">将要更新的内容。</param>
        /// <returns></returns>
        public long UpdateMany<T>(string collectionName, IDictionary<string, object> filter, IDictionary<string, object> update)
        {
            try
            {
                BsonDocument bsonDocFilter = new BsonDocument(filter);
                BsonDocument bsonDocUpdate = new BsonDocument(update);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                UpdateResult ur = collection.UpdateMany(bsonDocFilter, bsonDocUpdate);
                return ur.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <returns></returns>
        public T Select<T>(string collectionName, Expression<Func<T, bool>> filter)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                return collection.Find(filter).First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <returns></returns>
        public T Select<T>(string collectionName, FilterDefinition<T> filter)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                return collection.Find(filter).First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 查询数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。</param>
        /// <returns></returns>
        public T Select<T>(string collectionName, IDictionary<string, object> filter)
        {
            try
            {
                BsonDocument bsonDoc = new BsonDocument(filter);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                return collection.Find(bsonDoc).First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 查询多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：p => p.UserId == "admin"。</param>
        /// <returns></returns>
        public IList<T> SelectMany<T>(string collectionName, Expression<Func<T, bool>> filter)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                return collection.Find(filter).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 查询多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="filter">筛选条件。表达式示例：Builders&lt;T&gt;.Filter.Eq(string key, object value)。key区分大小写。</param>
        /// <returns></returns>
        public IList<T> SelectMany<T>(string collectionName, FilterDefinition<T> filter)
        {
            try
            {
                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                return collection.Find(filter).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 查询多条数据。
        /// </summary>
        /// <typeparam name="T">数据的类型。</typeparam>
        /// <param name="collectionName">MongoDB的集合名称。</param>
        /// <param name="dictionary">筛选条件。</param>
        /// <returns></returns>
        public IList<T> SelectMany<T>(string collectionName, IDictionary<string, object> dictionary)
        {
            try
            {
                BsonDocument bsonDoc = new BsonDocument(dictionary);

                IMongoCollection<T> collection = this.dataBase.GetCollection<T>(collectionName);
                return collection.Find(bsonDoc).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion
    }
}
