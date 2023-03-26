//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.ObjectPool
{
	/// <summary>
	/// 对象池模块接口
	/// </summary>
	public interface IObjectPoolModule
	{
		/// <summary>
		/// 获取对象池数量
		/// </summary>
		int Count
		{
			get;
		}

		/// <summary>
		/// 获取对象池
		/// </summary>
		/// <param name="poolName"> 对象池名称 </param>
		/// <typeparam name="T"> 对象类型 </typeparam>
		/// <returns></returns>
		IObjectPool<T> GetObjectPool<T>(string poolName) where T : ObjectBase;

		/// <summary>
		/// 创建对象池
		/// </summary>
		/// <param name="poolName"> 对象池名称 </param>
		/// <param name="capacity"> 容量 </param>
		/// <param name="expireTime"> 自动释放时间 </param>
		/// <typeparam name="T"> 对象类型 </typeparam>
		/// <returns></returns>
		IObjectPool<T> CreateObjectPool<T>(string poolName, int capacity, float expireTime) where T : ObjectBase;

		/// <summary>
		/// 销毁对象池
		/// <param name="poolName"> 对象池名称 </param>
		/// </summary>
		void DestroyObjectPool<T>(string poolName, IObjectPool<T> objPool) where T : ObjectBase;

		/// <summary>
		/// 释放所有对象池中未使用的对象
		/// </summary>
		void ReleaseAllUnused();
	}
}