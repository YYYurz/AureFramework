//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using AureFramework.Event;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace AureFramework.Resource {
	public sealed class ResourceModule : AureFrameworkModule, IResourceModule {
		private readonly Dictionary<int, AsyncOperationHandle> loadingAssetDic = new Dictionary<int, AsyncOperationHandle>();
		private int taskIdAccumulator;
		private IEventModule eventModule;

		/// <summary>
		/// 框架优先级，最小的优先初始化以及轮询
		/// </summary>
		public override int Priority => 2;
		
		public override void Init() {
			Addressables.InitializeAsync();
			eventModule = Aure.GetModule<IEventModule>();
		}
		
		public override void Tick(float elapseTime, float realElapseTime) {
			
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
		/// 异步克隆
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		/// <param name="beginCallBack"> 克隆开始回调，返回异步任务Id </param>
		/// <param name="endCallBack"> 克隆完成回调，返回结果 </param>
		public async void InstantiateAsync(string assetName, Action<int> beginCallBack = null, Action<GameObject> endCallBack = null) {
			if (!InternalCreateInstantiateAsyncHandle(assetName, out var handle)) {
				endCallBack?.Invoke(null);
				return;
			}
			var taskId = GetTaskId();
			beginCallBack?.Invoke(taskId);
			loadingAssetDic.Add(taskId, handle);

			await handle.Task;

			if (loadingAssetDic.ContainsKey(taskId)) {
				loadingAssetDic.Remove(taskId);
				endCallBack?.Invoke(handle.Result);
				Addressables.Release(handle);
				eventModule.Fire(this, LoadAssetSuccessEventArgs.Create(taskId, assetName, handle.Result));
				if (handle.Result == null) {
					Addressables.Release(handle);
					eventModule.Fire(this, LoadAssetFailedEventArgs.Create(taskId, assetName));
				}
			}
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
		/// <param name="beginCallBack"> 克隆开始回调，返回异步任务Id </param>
		/// <param name="endCallBack"> 克隆完成回调，返回结果 </param>
		/// <typeparam name="T"></typeparam>
		public async void LoadAssetAsync<T>(string assetName, Action<int> beginCallBack = null, Action<T> endCallBack = null) where T : Object{
			if (!InternalCreateLoadAsyncHandle<T>(assetName, out var handle)) {
				endCallBack?.Invoke(null);
				return;
			}
			var taskId = GetTaskId();
			beginCallBack?.Invoke(taskId);
			loadingAssetDic.Add(taskId, handle);
			
			await handle.Task;

			if (loadingAssetDic.ContainsKey(taskId)) {
				loadingAssetDic.Remove(taskId);
				endCallBack?.Invoke(handle.Result);
				Addressables.Release(handle);
				eventModule.Fire(this, LoadAssetSuccessEventArgs.Create(taskId, assetName, handle.Result));
				if (handle.Result == null) {
					Addressables.Release(handle);
					eventModule.Fire(this, LoadAssetFailedEventArgs.Create(taskId, assetName));
				}
			}
		}

		/// <summary>
		/// 异步加载场景
		/// </summary>
		/// <param name="sceneName"> 场景资源Key </param>
		/// <param name="percentCallBack"> 加载百分比回调 </param>
		/// <param name="endCallBack"> 加载完成回调 </param>
		/// <returns></returns>
		public void LoadSceneAsync(string sceneName, Action<float> percentCallBack, Action<SceneInstance> endCallBack) {
			StartCoroutine(InternalLoadSceneAsync(sceneName, percentCallBack, endCallBack));
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
		}

		/// <summary>
		/// 异步卸载场景
		/// </summary>
		/// <param name="scene"> 场景Instance引用 </param>
		/// <param name="callBack"> 卸载完成回调 </param>
		public async void UnloadSceneAsync(SceneInstance scene, Action callBack = null) {
			var handle = Addressables.UnloadSceneAsync(scene);
				
			await handle.Task;
			
			callBack?.Invoke();
		}
		
		private static bool InternalCreateLoadAsyncHandle<T>(string assetName, out AsyncOperationHandle<T> handle) where T : Object {
			if (string.IsNullOrEmpty(assetName)) {
				Debug.LogError("AureFramework ResourceModule : Load asset name is null.");
				handle = default;
				return false;
			}

			handle = Addressables.LoadAssetAsync<T>(assetName);
			return true;
		}

		private static bool InternalCreateInstantiateAsyncHandle(string assetName, out AsyncOperationHandle<GameObject> handle) {
			if (string.IsNullOrEmpty(assetName)) {
				Debug.LogError("AureFramework ResourceModule : Load asset name is null.");
				handle = default;
				return false;
			}

			handle = Addressables.InstantiateAsync(assetName);
			return true;
		}

		private static bool InternalCreateSceneAsyncHandle(string sceneName, out AsyncOperationHandle<SceneInstance> handle) {
			if (string.IsNullOrEmpty(sceneName)) {
				Debug.LogError("AureFramework ResourceModule : Load asset name is null.");
				handle = default;
				return false;
			}

			handle = Addressables.LoadSceneAsync(sceneName);
			return true;
		}
		
		private static IEnumerator InternalLoadSceneAsync(string sceneName, Action<float> percentCallBack = null, Action<SceneInstance> endCallBack = null) {
			if (!InternalCreateSceneAsyncHandle(sceneName, out var handle)) {
				endCallBack?.Invoke(default);
				yield break;
			}

			while (!handle.IsDone) {
				percentCallBack?.Invoke(handle.PercentComplete);
				yield return null;
			}
			
			endCallBack?.Invoke(handle.Result);
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