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
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace AureFramework.Resource {
	public class ResourceModule : AureFrameworkModule, IResourceModule {
		private readonly Dictionary<uint, AsyncOperationHandle> loadingAssetDic = new Dictionary<uint, AsyncOperationHandle>();
		
		private uint _counter;

		public override int Priority => 1;

		protected override void Awake() {
			base.Awake();
			
			Addressables.InitializeAsync();
		}
		
		public override void Tick() {
			foreach (var task in loadingAssetDic) {

			}
		}
		
		public override void Clear() {
			
		}

		private uint GetCounter() {
			++_counter;
			if (_counter == uint.MaxValue) {
				_counter = 1;
			}

			return loadingAssetDic.ContainsKey(_counter) ? GetCounter() : _counter;
		}

		public void InstantiateSync(string assetName, Action<GameObject> callBack) {
			if (string.IsNullOrEmpty(assetName)) {
				callBack?.Invoke(null);
				return;
			}

			var handle = Addressables.InstantiateAsync(assetName);

			handle.WaitForCompletion();

			if (handle.Result != null) {
				callBack?.Invoke(handle.Result);
				return;
			}
			
			Addressables.Release(handle);
		}
		
		public async void InstantiateAsync(string assetName, Action<GameObject> callBack = null) {
			if (string.IsNullOrEmpty(assetName)) {
				callBack?.Invoke(null);
				return;
			}
			
			var handle = Addressables.InstantiateAsync(assetName);
			var index = GetCounter();
			loadingAssetDic.Add(index, handle);
			
			await handle.Task;
			
			if (loadingAssetDic.ContainsKey(index)) {
				loadingAssetDic.Remove(index);
				callBack?.Invoke(handle.Result);
				if (handle.Result == null) {
					Addressables.Release(handle);
				}
			}
		}

		public T LoadAssetSync<T>(string assetName) where T : Object {
			if (string.IsNullOrEmpty(assetName)) {
				return null;
			}
			
			var handle = Addressables.LoadAssetAsync<T>(assetName);
			
			handle.WaitForCompletion();

			if (handle.Result != null) {
				return handle.Result;
			}
			
			Addressables.Release(handle);
			return null;
		}
		
		public async void LoadAssetAsync<T>(string assetName, Action<T> callBack = null) where T : Object{
			if (string.IsNullOrEmpty(assetName)) {
				callBack?.Invoke(null);
				return;
			}
			
			var handle = Addressables.LoadAssetAsync<T>(assetName);
			var index = GetCounter();
			
			loadingAssetDic.Add(index, handle);
			
			await handle.Task;
			
			if (loadingAssetDic.ContainsKey(index)) {
				loadingAssetDic.Remove(index);
				callBack?.Invoke(handle.Result);
				if (handle.Result == null ) {
					Addressables.Release(handle);
				}
			}
		}

		public IEnumerator LoadSceneAsync(string assetName, Action<float> percentCallBack = null,
			Action<SceneInstance> endCallBack = null) {
			if (string.IsNullOrEmpty(assetName)) {
				endCallBack?.Invoke(default);
				yield break;
			}
			
			var handle = Addressables.LoadSceneAsync(assetName);

			while (!handle.IsDone) {
				percentCallBack?.Invoke(handle.PercentComplete);
				yield return null;
			}
			
			endCallBack?.Invoke(handle.Result);
		}

		public async void UnloadSceneAsync(SceneInstance scene, Action callBack) {
			var handle = Addressables.UnloadSceneAsync(scene);
			
			await handle.Task;
			
			callBack?.Invoke();
		}

		public void ReleaseAsset(Object asset) {
			Addressables.Release(asset);
		} 
	}
}