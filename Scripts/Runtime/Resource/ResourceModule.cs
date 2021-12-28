//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AureFramework.Resource {
	/// <summary>
	/// 资源加载模块
	/// </summary>
	public sealed class ResourceModule : AureFrameworkModule, IResourceModule {
		private readonly Dictionary<int, AsyncOperationHandle> loadingAssetDic = new Dictionary<int, AsyncOperationHandle>();
		private readonly Dictionary<int, LoadAssetCallbacks> assetCallbackDic = new Dictionary<int, LoadAssetCallbacks>();
		private readonly Dictionary<int, InstantiateGameObjectCallbacks> instantiateCallbackDic = new Dictionary<int, InstantiateGameObjectCallbacks>();
		private readonly Dictionary<int, LoadSceneCallbacks> sceneCallbackDic = new Dictionary<int, LoadSceneCallbacks>();
		private readonly Dictionary<SceneInstance, string> loadedSceneDic = new Dictionary<SceneInstance, string>();
		private int taskIdAccumulator;

		/// <summary>
		/// 框架优先级，最小的优先初始化以及轮询
		/// </summary>
		public override int Priority => 2;
		
		public override void Init() {
			Addressables.InitializeAsync();
		}
		
		public override void Tick(float elapseTime, float realElapseTime) {
			foreach (var assetCallback in assetCallbackDic) {
				var handle = loadingAssetDic[assetCallback.Key];
				assetCallback.Value?.LoadAssetUpdateCallback?.Invoke(assetCallback.Key, handle.PercentComplete);
			}
			
			foreach (var instantiateCallback in instantiateCallbackDic) {
				var handle = loadingAssetDic[instantiateCallback.Key];
				instantiateCallback.Value?.InstantiateGameObjectUpdateCallback?.Invoke(instantiateCallback.Key, handle.PercentComplete);
			}
			
			foreach (var sceneCallback in sceneCallbackDic) {
				var handle = loadingAssetDic[sceneCallback.Key];
				sceneCallback.Value?.LoadSceneUpdateCallback?.Invoke(sceneCallback.Key, handle.PercentComplete);
			}
		}
		
		public override void Clear() {
			foreach (var loadingTask in loadingAssetDic) {
				ReleaseTask(loadingTask.Key);
			}
		}

		/// <summary>
		/// 同步克隆
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		public GameObject InstantiateSync(string assetName) {
			if (!InternalCreateInstantiateAsyncHandle(assetName, out var handle)) {
				return null;
			}

			handle.WaitForCompletion();

			if (handle.Result != null) {
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
		public T LoadAssetSync<T>(string assetName) where T : Object {
			if (!InternalCreateLoadAsyncHandle<T>(assetName, out var handle)) {
				return null;
			}
			
			handle.WaitForCompletion();

			if (handle.Result != null) {
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
		/// <typeparam name="T"></typeparam>
		public async void LoadAssetAsync<T>(string assetName, LoadAssetCallbacks loadAssetCallbacks = null) where T : Object{
			if (!InternalCreateLoadAsyncHandle<T>(assetName, out var handle)) {
				return;
			}
			var taskId = GetTaskId();
			loadingAssetDic.Add(taskId, handle);
			assetCallbackDic.Add(taskId, loadAssetCallbacks);
			loadAssetCallbacks?.LoadAssetBeginCallback?.Invoke(assetName, taskId);

			await handle.Task;

			if (loadingAssetDic.ContainsKey(taskId)) {
				if (handle.Status == AsyncOperationStatus.Succeeded) {
					loadAssetCallbacks?.LoadAssetSuccessCallback?.Invoke(assetName, taskId, handle.Result);
				} else {
					loadAssetCallbacks?.LoadAssetFailedCallback?.Invoke(assetName, taskId, handle.OperationException.Message);
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
		public async void InstantiateAsync(string assetName, InstantiateGameObjectCallbacks instantiateGameObjectCallbacks = null) {
			if (!InternalCreateInstantiateAsyncHandle(assetName, out var handle)) {
				return;
			}
			var taskId = GetTaskId(); 
			loadingAssetDic.Add(taskId, handle);
			instantiateCallbackDic.Add(taskId, instantiateGameObjectCallbacks);
			instantiateGameObjectCallbacks?.InstantiateGameObjectBeginCallback?.Invoke(assetName, taskId);

			await handle.Task;

			if (loadingAssetDic.ContainsKey(taskId)) {
				if (handle.Status == AsyncOperationStatus.Succeeded) {
					instantiateGameObjectCallbacks?.InstantiateGameObjectSuccessCallback?.Invoke(assetName, taskId, handle.Result);
				} else {
					instantiateGameObjectCallbacks?.InstantiateGameObjectFailedCallback?.Invoke(assetName, taskId, handle.OperationException.Message);
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
		/// <returns></returns>
		public async void LoadSceneAsync(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks = null) {
			if (!InternalCreateSceneAsyncHandle(sceneAssetName, out var handle)) {
				return;
			}
			var taskId = GetTaskId();
			loadingAssetDic.Add(taskId, handle);
			sceneCallbackDic.Add(taskId, loadSceneCallbacks);
			loadSceneCallbacks?.LoadSceneBeginCallback?.Invoke(sceneAssetName, taskId);

			await handle.Task;
			
			if (loadingAssetDic.ContainsKey(taskId)) {
				if (handle.Status == AsyncOperationStatus.Succeeded) {
					loadedSceneDic.Add(handle.Result, sceneAssetName);
					loadSceneCallbacks?.LoadSceneSuccessCallback?.Invoke(sceneAssetName, taskId, handle.Result);
				} else {
					loadSceneCallbacks?.LoadSceneFailedCallback?.Invoke(sceneAssetName, taskId, handle.OperationException.Message);
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
		public void ReleaseAsset(Object asset) {
			Addressables.Release(asset);
		}
		
		/// <summary>
		///	终止正在加载的任务 
		/// </summary>
		/// <param name="taskId"> 加载任务Id </param>
		public void ReleaseTask(int taskId) {
			if (!loadingAssetDic.TryGetValue(taskId, out var loadingHandle)) {
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
		public async void UnloadSceneAsync(SceneInstance scene, Action<string> callBack = null) {
			var handle = Addressables.UnloadSceneAsync(scene);
				
			await handle.Task;

			if (handle.Status == AsyncOperationStatus.Succeeded) {
				callBack?.Invoke(loadedSceneDic[scene]);
				loadedSceneDic.Remove(scene);
			} else {
				Debug.LogError($"ResourceModule : Unload scene failed, error message :{handle.OperationException.Message}");
			}
		}
		
		private static bool InternalCreateLoadAsyncHandle<T>(string assetName, out AsyncOperationHandle<T> handle) where T : Object {
			if (string.IsNullOrEmpty(assetName)) {
				Debug.LogError("ResourceModule : Load asset name is null.");
				handle = default;
				return false;
			}

			handle = Addressables.LoadAssetAsync<T>(assetName);
			return true;
		}

		private static bool InternalCreateInstantiateAsyncHandle(string assetName, out AsyncOperationHandle<GameObject> handle) {
			if (string.IsNullOrEmpty(assetName)) {
				Debug.LogError("ResourceModule : Load asset name is null.");
				handle = default;
				return false;
			}

			handle = Addressables.InstantiateAsync(assetName);
			return true;
		}

		private static bool InternalCreateSceneAsyncHandle(string sceneName, out AsyncOperationHandle<SceneInstance> handle) {
			if (string.IsNullOrEmpty(sceneName)) {
				Debug.LogError("ResourceModule : Load asset name is null.");
				handle = default;
				return false;
			}

			handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			return true;
		}
		
		private int GetTaskId() {
			while (true) {
				++taskIdAccumulator;
				if (taskIdAccumulator == int.MaxValue) {
					taskIdAccumulator = 1;
				}

				if (loadingAssetDic.ContainsKey(taskIdAccumulator)) continue;
				return taskIdAccumulator;
			}
		}
	}
}