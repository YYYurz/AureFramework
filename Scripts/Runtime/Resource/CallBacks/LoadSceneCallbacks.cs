//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine.ResourceManagement.ResourceProviders;

namespace AureFramework.Resource
{
	/// <summary>
	/// 加载场景资源开始委托
	/// </summary>
	/// <param name="sceneAssetName"> 场景资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	public delegate void LoadSceneBeginCallback(string sceneAssetName, int taskId);

	/// <summary>
	/// 加载场景资源成功委托
	/// </summary>
	/// <param name="sceneAssetName"> 场景资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="sceneAsset"> 加载完成的场景资源 </param>
	/// <param name="userData"> 用户数据 </param>
	public delegate void LoadSceneSuccessCallback(string sceneAssetName, int taskId, SceneInstance sceneAsset, object userData);

	/// <summary>
	/// 加载场景资源更新委托
	/// </summary>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="progress"> 进度 </param>
	public delegate void LoadSceneUpdateCallback(int taskId, float progress);

	/// <summary>
	/// 加载场景资源失败委托
	/// </summary>
	/// <param name="sceneAssetName"> 场景资源名称 </param>
	/// <param name="taskId"> 任务Id </param>
	/// <param name="errorMessage"> 错误信息 </param>
	/// <param name="userData"> 用户数据 </param>
	public delegate void LoadSceneFailedCallback(string sceneAssetName, int taskId, string errorMessage, object userData);

	/// <summary>
	/// 加载场景资源回调
	/// </summary>
	public sealed class LoadSceneCallbacks
	{
		private readonly LoadSceneBeginCallback loadSceneBeginCallback;
		private readonly LoadSceneSuccessCallback loadSceneSuccessCallback;
		private readonly LoadSceneUpdateCallback loadSceneUpdateCallback;
		private readonly LoadSceneFailedCallback loadSceneFailedCallback;

		public LoadSceneCallbacks(LoadSceneBeginCallback loadSceneBeginCallback, LoadSceneSuccessCallback loadSceneSuccessCallback, LoadSceneUpdateCallback loadSceneUpdateCallback, LoadSceneFailedCallback loadSceneFailedCallback)
		{
			this.loadSceneBeginCallback = loadSceneBeginCallback;
			this.loadSceneSuccessCallback = loadSceneSuccessCallback;
			this.loadSceneUpdateCallback = loadSceneUpdateCallback;
			this.loadSceneFailedCallback = loadSceneFailedCallback;
		}

		/// <summary>
		/// 加载场景资源开始函数
		/// </summary>
		public LoadSceneBeginCallback LoadSceneBeginCallback
		{
			get
			{
				return loadSceneBeginCallback;
			}
		}

		/// <summary>
		/// 加载场景资源成功函数
		/// </summary>
		public LoadSceneSuccessCallback LoadSceneSuccessCallback
		{
			get
			{
				return loadSceneSuccessCallback;
			}
		}

		/// <summary>
		/// 加载场景资源更新函数
		/// </summary>
		public LoadSceneUpdateCallback LoadSceneUpdateCallback
		{
			get
			{
				return loadSceneUpdateCallback;
			}
		}

		/// <summary>
		/// 加载场景资源失败函数
		/// </summary>
		public LoadSceneFailedCallback LoadSceneFailedCallback
		{
			get
			{
				return loadSceneFailedCallback;
			}
		}
	}
}