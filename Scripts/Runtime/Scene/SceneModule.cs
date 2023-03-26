//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Event;
using AureFramework.Resource;
using UnityEngine.ResourceManagement.ResourceProviders;
using Debug = UnityEngine.Debug;

namespace AureFramework.Scene
{
	/// <summary>
	/// 场景模块
	/// </summary>
	public class SceneModule : AureFrameworkModule, ISceneModule
	{
		private readonly Dictionary<int, string> loadingSceneDic = new Dictionary<int, string>();
		private readonly Dictionary<string, SceneInstance> loadedSceneDic = new Dictionary<string, SceneInstance>();
		private readonly List<string> unLoadingSceneList = new List<string>();
		private IResourceModule resourceModule;
		private IEventModule eventModule;
		private LoadSceneCallbacks loadSceneAssetCallbacks;

		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public override int Priority => 2;

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
			resourceModule = Aure.GetModule<IResourceModule>();
			eventModule = Aure.GetModule<IEventModule>();
			loadSceneAssetCallbacks = new LoadSceneCallbacks(OnLoadSceneBegin, OnLoadSceneSuccess, OnLoadSceneUpdate, OnLoadSceneFailed);
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			// foreach (var loadedScene in loadedSceneDic)
			// {
			// 	if (SceneIsUnloading(loadedScene.Key))
			// 	{
			// 		continue;
			// 	}
			//
			// 	UnloadScene(loadedScene.Key);
			// }

			loadingSceneDic.Clear();
			loadedSceneDic.Clear();
			unLoadingSceneList.Clear();
		}

		/// <summary>
		/// 场景是否正在加载
		/// </summary>
		/// <param name="sceneName"> 场景名称 </param>
		/// <returns></returns>
		public bool SceneIsLoading(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				Debug.LogError("SceneModule : Scene name is invalid.");
				return false;
			}

			return loadingSceneDic.ContainsValue(sceneName);
		}

		/// <summary>
		/// 场景是否正在卸载
		/// </summary>
		/// <param name="sceneName">场景名称</param>
		/// <returns></returns>
		public bool SceneIsUnloading(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				Debug.LogError("SceneModule : Scene name is invalid.");
				return false;
			}

			return unLoadingSceneList.Contains(sceneName);
		}

		/// <summary>
		/// 场景是否已经加载完成
		/// </summary>
		/// <param name="sceneName"> 场景名称 </param>
		/// <returns></returns>
		public bool SceneIsLoaded(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				Debug.LogError("SceneModule : Scene name is invalid.");
				return false;
			}

			return loadedSceneDic.ContainsKey(sceneName);
		}

		/// <summary>
		/// 加载场景
		/// </summary>
		/// <param name="sceneName"> 场景名称 </param>
		public void LoadScene(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				Debug.LogError("SceneModule : Scene name is invalid.");
				return;
			}

			if (SceneIsUnloading(sceneName))
			{
				Debug.LogError($"SceneModule : Scene is unloading, Name :{sceneName}");
				return;
			}

			if (SceneIsLoading(sceneName))
			{
				Debug.LogError($"SceneModule : Scene is loading, Name :{sceneName}");
				return;
			}

			if (SceneIsLoaded(sceneName))
			{
				Debug.LogError($"SceneModule : Scene is already loaded, Name :{sceneName}");
				return;
			}

			resourceModule.LoadSceneAsync(sceneName, loadSceneAssetCallbacks, null);
		}

		/// <summary>
		/// 卸载场景
		/// </summary>
		/// <param name="sceneName"> 场景名称 </param>
		public void UnloadScene(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				Debug.LogError("SceneModule : Scene name is invalid.");
				return;
			}

			if (SceneIsUnloading(sceneName))
			{
				Debug.LogError($"SceneModule : Scene is unloading, Name :{sceneName}");
				return;
			}

			if (SceneIsLoading(sceneName))
			{
				Debug.LogError($"SceneModule : Scene is loading, Name :{sceneName}");
				return;
			}

			if (!SceneIsLoaded(sceneName))
			{
				Debug.LogError($"SceneModule : Scene is not loaded, Name :{sceneName}");
				return;
			}

			unLoadingSceneList.Add(sceneName);
			resourceModule.UnloadSceneAsync(loadedSceneDic[sceneName], OnUnloadSceneOver);
		}

		private void OnLoadSceneBegin(string sceneAssetName, int taskId)
		{
			loadingSceneDic.Add(taskId, sceneAssetName);
		}

		private void OnLoadSceneSuccess(string sceneAssetName, int taskId, SceneInstance sceneAsset, object userData)
		{
			if (loadingSceneDic.ContainsKey(taskId))
			{
				loadedSceneDic.Add(sceneAssetName, sceneAsset);
				loadingSceneDic.Remove(taskId);
				eventModule.Fire(this, LoadSceneSuccessEventArgs.Create(sceneAssetName));
			}
		}

		private void OnLoadSceneUpdate(int taskId, float progress)
		{
			if (loadingSceneDic.ContainsKey(taskId))
			{
				eventModule.Fire(this, LoadSceneUpdateEventArgs.Create(loadingSceneDic[taskId], progress));
			}
		}

		private void OnLoadSceneFailed(string sceneAssetName, int taskId, string errorMessage, object userData)
		{
			if (loadingSceneDic.ContainsKey(taskId))
			{
				eventModule.Fire(this, LoadSceneFailedEventArgs.Create(loadingSceneDic[taskId], errorMessage));
				Debug.LogError($"SceneModule : Load scene Failed, error message :{errorMessage}.");
				loadingSceneDic.Remove(taskId);
			}
		}

		private void OnUnloadSceneOver(string sceneName)
		{
			if (unLoadingSceneList.Contains(sceneName) && loadedSceneDic.ContainsKey(sceneName))
			{
				eventModule.Fire(this, UnloadSceneSuccessEventArgs.Create(sceneName));
				unLoadingSceneList.Remove(sceneName);
				loadedSceneDic.Remove(sceneName);
			}
		}
	}
}