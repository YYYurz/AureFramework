//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AureFramework.Resource
{
	/// <summary>
	/// 资源模块
	/// </summary>
	public sealed class ResourceModule : AureFrameworkModule, IResourceModule
	{
		private readonly Dictionary<int, AsyncOperationHandle> loadingAssetDic = new Dictionary<int, AsyncOperationHandle>();
		private readonly Dictionary<int, LoadAssetCallbacks> assetCallbackDic = new Dictionary<int, LoadAssetCallbacks>();
		private readonly Dictionary<int, InstantiateGameObjectCallbacks> instantiateCallbackDic = new Dictionary<int, InstantiateGameObjectCallbacks>();
		private readonly Dictionary<int, LoadSceneCallbacks> sceneCallbackDic = new Dictionary<int, LoadSceneCallbacks>();
		private readonly Dictionary<SceneInstance, string> loadedSceneDic = new Dictionary<SceneInstance, string>();
		private int taskIdAccumulator;

		/// <summary>
		/// 框架优先级，最小的优先初始化以及轮询
		/// </summary>
		public override int Priority => 3;

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
			Addressables.InitializeAsync();
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
			foreach (var assetCallback in assetCallbackDic)
			{
				var handle = loadingAssetDic[assetCallback.Key];
				assetCallback.Value?.LoadAssetUpdateCallback?.Invoke(assetCallback.Key, handle.PercentComplete);
			}

			foreach (var instantiateCallback in instantiateCallbackDic)
			{
				var handle = loadingAssetDic[instantiateCallback.Key];
				instantiateCallback.Value?.InstantiateGameObjectUpdateCallback?.Invoke(instantiateCallback.Key, handle.PercentComplete);
			}

			foreach (var sceneCallback in sceneCallbackDic)
			{
				var handle = loadingAssetDic[sceneCallback.Key];
				sceneCallback.Value?.LoadSceneUpdateCallback?.Invoke(sceneCallback.Key, handle.PercentComplete);
			}
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			var loadingTaskIdList = loadingAssetDic.Keys.ToList();
			foreach (var loadingTaskId in loadingTaskIdList)
			{
				ReleaseTask(loadingTaskId);
			}
			
			loadingAssetDic.Clear();
			assetCallbackDic.Clear();
			instantiateCallbackDic.Clear();
			sceneCallbackDic.Clear();
			loadedSceneDic.Clear();
		}

		/// <summary>
		/// 同步克隆
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		public GameObject InstantiateSync(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Debug.LogError("ResourceModule : Load asset name is invalid.");
				return null;
			}

			var handle = Addressables.InstantiateAsync(assetName);
			handle.WaitForCompletion();

			if (handle.Result != null)
			{
				return handle.Result;
			}

			Addressables.Release(handle);
			return null;
		}

		/// <summary>
		/// 同步加载资源
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T LoadAssetSync<T>(string assetName) where T : Object
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Debug.LogError("ResourceModule : Load asset name is invalid.");
				return null;
			}

			var handle = Addressables.LoadAssetAsync<T>(assetName);
			handle.WaitForCompletion();

			if (handle.Result != null)
			{
				return handle.Result;
			}

			Addressables.Release(handle);
			return null;
		}

		/// <summary>
		/// 异步加载资源
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		/// <param name="loadAssetCallbacks"> 加载资源回调 </param>
		/// <param name="userData"> 用户数据 </param>
		/// <typeparam name="T"></typeparam>
		public async void LoadAssetAsync<T>(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData) where T : Object
		{
			if (string.IsNullOrEmpty(assetName))
			{
				loadAssetCallbacks?.LoadAssetFailedCallback?.Invoke(assetName, 0, "Load asset name is invalid.", userData);
				return;
			}

			var handle = Addressables.LoadAssetAsync<T>(assetName);
			var taskId = GetTaskId();
			loadingAssetDic.Add(taskId, handle);
			assetCallbackDic.Add(taskId, loadAssetCallbacks);
			loadAssetCallbacks?.LoadAssetBeginCallback?.Invoke(assetName, taskId);

			await handle.Task;

			if (loadingAssetDic.ContainsKey(taskId))
			{
				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					loadAssetCallbacks?.LoadAssetSuccessCallback?.Invoke(assetName, taskId, handle.Result, userData);
				}
				else
				{
					loadAssetCallbacks?.LoadAssetFailedCallback?.Invoke(assetName, taskId, handle.OperationException.Message, userData);
					Addressables.Release(handle);
				}

				loadingAssetDic.Remove(taskId);
				assetCallbackDic.Remove(taskId);
			}
		}

		/// <summary>
		/// 异步克隆
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		/// <param name="instantiateGameObjectCallbacks"> 克隆游戏物体回调 </param>
		/// <param name="userData"> 用户数据 </param>
		public async void InstantiateAsync(string assetName, InstantiateGameObjectCallbacks instantiateGameObjectCallbacks, object userData)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				instantiateGameObjectCallbacks?.InstantiateGameObjectFailedCallback?.Invoke(assetName, 0, "Load asset name is invalid.", userData);
				return;
			}

			var handle = Addressables.InstantiateAsync(assetName);
			var taskId = GetTaskId();
			loadingAssetDic.Add(taskId, handle);
			instantiateCallbackDic.Add(taskId, instantiateGameObjectCallbacks);
			instantiateGameObjectCallbacks?.InstantiateGameObjectBeginCallback?.Invoke(assetName, taskId);

			await handle.Task;

			if (loadingAssetDic.ContainsKey(taskId))
			{
				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					instantiateGameObjectCallbacks?.InstantiateGameObjectSuccessCallback?.Invoke(assetName, taskId, handle.Result, userData);
				}
				else
				{
					instantiateGameObjectCallbacks?.InstantiateGameObjectFailedCallback?.Invoke(assetName, taskId, handle.OperationException.Message, userData);
					Addressables.Release(handle);
				}

				loadingAssetDic.Remove(taskId);
				instantiateCallbackDic.Remove(taskId);
			}
		}

		/// <summary>
		/// 异步加载场景
		/// </summary>
		/// <param name="sceneAssetName"> 场景资源Key </param>
		/// <param name="loadSceneCallbacks"> 加载场景资源回调 </param>
		/// <param name="userData"> 用户数据 </param>
		/// <returns></returns>
		public async void LoadSceneAsync(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks, object userData)
		{
			if (string.IsNullOrEmpty(sceneAssetName))
			{
				loadSceneCallbacks?.LoadSceneFailedCallback?.Invoke(sceneAssetName, 0, "Load asset name is invalid.", userData);
				return;
			}

			var handle = Addressables.LoadSceneAsync(sceneAssetName, LoadSceneMode.Additive);
			var taskId = GetTaskId();
			loadingAssetDic.Add(taskId, handle);
			sceneCallbackDic.Add(taskId, loadSceneCallbacks);
			loadSceneCallbacks?.LoadSceneBeginCallback?.Invoke(sceneAssetName, taskId);

			await handle.Task;

			if (loadingAssetDic.ContainsKey(taskId))
			{
				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					loadedSceneDic.Add(handle.Result, sceneAssetName);
					loadSceneCallbacks?.LoadSceneSuccessCallback?.Invoke(sceneAssetName, taskId, handle.Result, userData);
				}
				else
				{
					loadSceneCallbacks?.LoadSceneFailedCallback?.Invoke(sceneAssetName, taskId, handle.OperationException.Message, userData);
					Addressables.Release(handle);
				}

				sceneCallbackDic.Remove(taskId);
				instantiateCallbackDic.Remove(taskId);
			}
		}

		/// <summary>
		/// 卸载资源
		/// </summary>
		/// <param name="asset"> 要卸载的资源 </param>
		public void ReleaseAsset(Object asset)
		{
			Addressables.Release(asset);
		}

		/// <summary>
		///	终止正在加载的任务 
		/// </summary>
		/// <param name="taskId"> 加载任务Id </param>
		public void ReleaseTask(int taskId)
		{
			if (!loadingAssetDic.TryGetValue(taskId, out var loadingHandle))
			{
				return;
			}

			Addressables.Release(loadingHandle);
			loadingAssetDic.Remove(taskId);
			assetCallbackDic.Remove(taskId);
			instantiateCallbackDic.Remove(taskId);
			sceneCallbackDic.Remove(taskId);
		}

		/// <summary>
		/// 异步卸载场景
		/// </summary>
		/// <param name="scene"> 场景Instance引用 </param>
		/// <param name="callBack"> 卸载完成回调 </param>
		public async void UnloadSceneAsync(SceneInstance scene, Action<string> callBack = null)
		{
			var handle = Addressables.UnloadSceneAsync(scene, false);

			await handle.Task;

			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				callBack?.Invoke(loadedSceneDic[scene]);
				loadedSceneDic.Remove(scene);
			}
			else
			{
				Debug.LogError($"ResourceModule : Unload scene failed, error message :{handle.OperationException.Message}");
			}
			
			Addressables.Release(handle);
		}

		private int GetTaskId()
		{
			while (true)
			{
				++taskIdAccumulator;
				if (taskIdAccumulator == int.MaxValue)
				{
					taskIdAccumulator = 1;
				}

				if (loadingAssetDic.ContainsKey(taskIdAccumulator))
				{
					continue;
				}
				
				return taskIdAccumulator;
			}
		}
	}
}