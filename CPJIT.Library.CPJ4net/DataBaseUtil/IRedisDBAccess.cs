using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.DataBaseUtil
{
    /// <summary>
    /// 提供Redis客户端交互接口。
    /// </summary>
    public interface IRedisDBAccess : IDisposable
    {
        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        bool Add<T>(string key, T value);

        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, DateTime expiresAt);

        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param> 
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, TimeSpan expiresIn);

        /// <summary>
        /// 按给定的数值递增指定键的值。操作是原子的, 并在服务器上发生。 一个不存在的键值从0开始。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="amount">递增的数量</param>
        /// <returns></returns>
        long Increment(string key, uint amount);

        /// <summary>
        /// 按给定的数值递减指定键的值。操作是原子的, 并在服务器上发生。 一个不存在的键值从0开始。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="amount">递减的数量</param>
        /// <returns></returns>
        long Decrement(string key, uint amount);

        /// <summary>
        /// 使高速缓存中的所有数据作废。
        /// </summary>
        void FlushAll();

        /// <summary>
        /// 获取指定键的值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取多个键的值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="keys">表示多个键的集合。</param>
        /// <returns></returns>
        IDictionary<string, T> GetAll<T>(IEnumerable<string> keys);

        /// <summary>
        /// 移除一个键/值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>如果移除成功，则返回true。如果指定键在Redis中不存在或移除失败，则返回false。</returns>
        bool Remove(string key);

        /// <summary>
        /// 移除多个键/值。
        /// </summary>
        /// <param name="keys">表示多个键的集合。</param>
        void RemoveAll(IEnumerable<string> keys);

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        bool Replace<T>(string key, T value);

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        bool Replace<T>(string key, T value, DateTime expiresAt);

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        bool Replace<T>(string key, T value, TimeSpan expiresIn);

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        bool Set<T>(string key, T value);

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, DateTime expiresAt);

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, TimeSpan expiresIn);

        /// <summary>
        /// 将多个值设置到缓存中。
        /// </summary>
        /// <typeparam name="T">值得数据类型。</typeparam>
        /// <param name="values">以“键-值”形式表示的多个数据的集合。</param>
        void SetAll<T>(IDictionary<string, T> values);
    }
}
