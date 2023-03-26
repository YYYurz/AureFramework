//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework.Resource
{
	/// <summary>
	/// 克隆游戏物体开始委托
	/// </summary>
	/// <param name="assetName"> 游戏物体游戏物体资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	public delegate void InstantiateGameObjectBeginCallback(string assetName, int taskId);

	/// <summary>
	/// 克隆游戏物体成功委托
	/// </summary>
	/// <param name="assetName"> 游戏物体资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="gameObject"> 游戏物体资源 </param>
	/// <param name="userData"> 用户数据 </param>
	public delegate void InstantiateGameObjectSuccessCallback(string assetName, int taskId, GameObject gameObject, object userData);

	/// <summary>
	/// 克隆游戏物体更新委托
	/// </summary>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="progress"> 进度 </param>
	public delegate void InstantiateGameObjectUpdateCallback(int taskId, float progress);

	/// <summary>
	/// 克隆游戏物体失败委托
	/// </summary>
	/// <param name="assetName"> 游戏物体资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="errorMessage"> 错误信息 </param>
	/// <param name="userData"> 用户数据 </param>
	public delegate void InstantiateGameObjectFailedCallback(string assetName, int taskId, string errorMessage, object userData);

	/// <summary>
	/// 克隆游戏物体回调
	/// </summary>
	public sealed class InstantiateGameObjectCallbacks
	{
		private readonly InstantiateGameObjectBeginCallback instantiateGameObjectBeginCallback;
		private readonly InstantiateGameObjectSuccessCallback instantiateGameObjectSuccessCallback;
		private readonly InstantiateGameObjectUpdateCallback instantiateGameObjectUpdateCallback;
		private readonly InstantiateGameObjectFailedCallback instantiateGameObjectFailedCallback;

		public InstantiateGameObjectCallbacks(InstantiateGameObjectBeginCallback instantiateGameObjectBeginCallback, InstantiateGameObjectSuccessCallback instantiateGameObjectSuccessCallback, InstantiateGameObjectUpdateCallback instantiateGameObjectUpdateCallback, InstantiateGameObjectFailedCallback instantiateGameObjectFailedCallback)
		{
			this.instantiateGameObjectBeginCallback = instantiateGameObjectBeginCallback;
			this.instantiateGameObjectSuccessCallback = instantiateGameObjectSuccessCallback;
			this.instantiateGameObjectUpdateCallback = instantiateGameObjectUpdateCallback;
			this.instantiateGameObjectFailedCallback = instantiateGameObjectFailedCallback;
		}

		/// <summary>
		/// 克隆游戏物体开始函数
		/// </summary>
		public InstantiateGameObjectBeginCallback InstantiateGameObjectBeginCallback
		{
			get
			{
				return instantiateGameObjectBeginCallback;
			}
		}

		/// <summary>
		/// 克隆游戏物体成功函数
		/// </summary>
		public InstantiateGameObjectSuccessCallback InstantiateGameObjectSuccessCallback
		{
			get
			{
				return instantiateGameObjectSuccessCallback;
			}
		}

		/// <summary>
		/// 克隆游戏物体更新函数
		/// </summary>
		public InstantiateGameObjectUpdateCallback InstantiateGameObjectUpdateCallback
		{
			get
			{
				return instantiateGameObjectUpdateCallback;
			}
		}

		/// <summary>
		/// 克隆游戏物体失败函数
		/// </summary>
		public InstantiateGameObjectFailedCallback InstantiateGameObjectFailedCallback
		{
			get
			{
				return instantiateGameObjectFailedCallback;
			}
		}
	}
}