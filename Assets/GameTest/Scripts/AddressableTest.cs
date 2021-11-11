//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace AureFramework {
	public class AddressableManager : MonoBehaviour {
		private readonly Dictionary<uint, AsyncOperationHandle> _loadingAssetMap =
			new Dictionary<uint, AsyncOperationHandle>();

		private uint _counter;

		private uint GetCounter() {
			++_counter;
			if (_counter == uint.MaxValue) {
				_counter = 1;
			}

			return _loadingAssetMap.ContainsKey(_counter) ? GetCounter() : _counter;
		}

		public static IEnumerator InitAsync() {
			// Addressables.ResourceManager.ResourceUrl = ServerUrl;
			// Addressables.ResourceManager.ResourceVersion = ResourceVersion;
			var handle = Addressables.InitializeAsync();
			yield return handle;
		}

		private static long GetDownLoadSize(IEnumerable<IResourceLocation> list) {
			long size = 0;
			foreach (var location in list) {
				if (location.Data is ILocationSizeData sizeData) {
					// size += sizeData.ComputeSize(location, Addressables.ResourceManager, true);
					size += 1;
				}
			}

			return size;
		}

		#region Release asset

		/// 取消加载中的任务
		public void ReleaseTask(uint taskId) {
			if (!_loadingAssetMap.TryGetValue(taskId, out var asset)) {
				return;
			}

			Addressables.Release(asset);
			_loadingAssetMap.Remove(taskId);
		}

		public static void Release<T>(T asset) where T : Object => Addressables.Release(asset);

		public static bool ReleaseGameObject(GameObject instance) => Addressables.ReleaseInstance(instance);

		public static void ReleaseScene(SceneInstance scene, Action callback) => UnLoadSceneAsync(scene, callback);

		#endregion

		#region Task

		public uint LoadAssetAsync<T>(string assetName, Action<T> callback) where T : Object {
			if (string.IsNullOrEmpty(assetName)) {
				callback?.Invoke(null);
				return 0u;
			}

			var task = 0u;
			LoadAsync(assetName, index => task = index, callback);
			return task;
		}

		public static T LoadAssetSync<T>(string assetName) where T : Object =>
			string.IsNullOrEmpty(assetName) ? null : LoadSync<T>(assetName);

		public uint LoadGameObjectAsync(string goName, Action<GameObject> callback) {
			if (string.IsNullOrEmpty(goName)) {
				callback?.Invoke(null);
				return 0;
			}

			var task = 0u;
			InstantiateAsync(goName, index => task = index, callback);
			return task;
		}

		public static GameObject LoadGameObjectSync(string goName) =>
			string.IsNullOrEmpty(goName) ? null : InstantiateSync(goName);

		public void LoadScene(string sceneName, Action<float> perCallBack, Action<SceneInstance> callback) =>
			StartCoroutine(LoadSceneAsync(sceneName, perCallBack, callback));

		#endregion

		#region addressable load

		private async void LoadAsync<T>(string asset, Action<uint> beginCall, Action<T> callback = null)
			where T : Object {
			var handle = Addressables.LoadAssetAsync<T>(asset);
			var index = GetCounter();
			
			_loadingAssetMap.Add(index, handle);
			beginCall?.Invoke(index);
			
			await handle.Task;
			
			if (_loadingAssetMap.ContainsKey(index)) {
				_loadingAssetMap.Remove(index);
				callback?.Invoke(handle.Result);
				if (handle.Result == null ) {
					Addressables.Release(handle);
				}
			}
		}

		private static T LoadSync<T>(string asset) where T : Object {
			var handle = Addressables.LoadAssetAsync<T>(asset);
			if (handle.Result != null) {
				return handle.Result;
			}

			Addressables.Release(handle);
			return null;
		}

		private async void InstantiateAsync(string asset, Action<uint> beginCall,
			Action<GameObject> callback = null) {
			var handle = Addressables.InstantiateAsync(asset);
			var index = GetCounter();
			_loadingAssetMap.Add(index, handle);
			beginCall?.Invoke(index);
			await handle.Task;
			if (_loadingAssetMap.ContainsKey(index)) {
				_loadingAssetMap.Remove(index);
				callback?.Invoke(handle.Result);
				if (handle.Result == null) {
					Addressables.Release(handle);
				}
			}
		}

		private static GameObject InstantiateSync(string asset) {
			var handle = Addressables.InstantiateAsync(asset);
			if (handle.Result != null) {
				return handle.Result;
			}

			Addressables.Release(handle);
			return null;
		}

		private static IEnumerator LoadSceneAsync(string asset, Action<float> perCallBack = null,
			Action<SceneInstance> callback = null) {
			var handle = Addressables.LoadSceneAsync(asset);
			while (!handle.IsDone) {
				perCallBack?.Invoke(handle.PercentComplete);
				yield return null;
			}

			callback?.Invoke(handle.Result);
		}

		private static async void UnLoadSceneAsync(SceneInstance scene, Action callback) {
			var handle = Addressables.UnloadSceneAsync(scene);
			await handle.Task;
			callback?.Invoke();
		}

		#endregion
	}
}