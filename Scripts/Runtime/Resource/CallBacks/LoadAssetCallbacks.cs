//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework.Resource
{
	/// <summary>
	/// 加载资源开始委托
	/// </summary>
	/// <param name="assetName"> 资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	public delegate void LoadAssetBeginCallback(string assetName, int taskId);

	/// <summary>
	/// 加载资源成功委托
	/// </summary>
	/// <param name="assetName"> 资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="asset"> 加载完成的资源 </param>
	/// <param name="userData"> 用户数据 </param>
	public delegate void LoadAssetSuccessCallback(string assetName, int taskId, Object asset, object userData);

	/// <summary>
	/// 加载资源更新委托
	/// </summary>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="progress"> 进度 </param>
	public delegate void LoadAssetUpdateCallback(int taskId, float progress);

	/// <summary>
	/// 加载资源失败委托
	/// </summary>
	/// <param name="assetName"> 资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="errorMessage"> 错误信息 </param>
	/// <param name="userData"> 用户数据 </param>
	public delegate void LoadAssetFailedCallback(string assetName, int taskId, string errorMessage, object userData);

	/// <summary>
	/// 加载资源回调
	/// </summary>
	public sealed class LoadAssetCallbacks
	{
		private readonly LoadAssetBeginCallback loadAssetBeginCallback;
		private readonly LoadAssetSuccessCallback loadAssetSuccessCallback;
		private readonly LoadAssetUpdateCallback loadAssetUpdateCallback;
		private readonly LoadAssetFailedCallback loadAssetFailedCallback;

		public LoadAssetCallbacks(LoadAssetBeginCallback loadAssetBeginCallback, LoadAssetSuccessCallback loadAssetSuccessCallback, LoadAssetUpdateCallback loadAssetUpdateCallback, LoadAssetFailedCallback loadAssetFailedCallback)
		{
			this.loadAssetBeginCallback = loadAssetBeginCallback;
			this.loadAssetSuccessCallback = loadAssetSuccessCallback;
			this.loadAssetUpdateCallback = loadAssetUpdateCallback;
			this.loadAssetFailedCallback = loadAssetFailedCallback;
		}

		/// <summary>
		/// 加载资源开始函数
		/// </summary>
		public LoadAssetBeginCallback LoadAssetBeginCallback
		{
			get
			{
				return loadAssetBeginCallback;
			}
		}

		/// <summary>
		/// 加载资源成功函数
		/// </summary>
		public LoadAssetSuccessCallback LoadAssetSuccessCallback
		{
			get
			{
				return loadAssetSuccessCallback;
			}
		}

		/// <summary>
		/// 加载资源更新函数
		/// </summary>
		public LoadAssetUpdateCallback LoadAssetUpdateCallback
		{
			get
			{
				return loadAssetUpdateCallback;
			}
		}

		/// <summary>
		/// 加载资源失败函数
		/// </summary>
		public LoadAssetFailedCallback LoadAssetFailedCallback
		{
			get
			{
				return loadAssetFailedCallback;
			}
		}
	}
}