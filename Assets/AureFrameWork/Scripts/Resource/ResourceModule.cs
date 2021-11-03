//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace AureFramework.Resource {
	public class ResourceModule : AureFrameworkModule, IResourceModule {
		private readonly Dictionary<uint, AsyncOperationHandle> loadingAssetDic = new Dictionary<uint, AsyncOperationHandle>();
		
		private uint _counter;

		public void Init() {
			Addressables.InitializeAsync();
		}
		
		public override void Update() {
			foreach (var task in loadingAssetDic) {
				
			}
		}

		public void InstantiateAsync(string assetName) {
			
		}

		private uint GetCounter() {
			++_counter;
			if (_counter == uint.MaxValue) {
				_counter = 1;
			}

			return loadingAssetDic.ContainsKey(_counter) ? GetCounter() : _counter;
		}
		
		public async void LoadAssetAsync<T>(string assetName, Action<T> callBack = null) where T : Object{
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

		public T LoadAssetSync<T>(string assetName) where T : Object {
			var handle = Addressables.LoadAssetAsync<T>(assetName);
			handle.Task.Wait(TimeSpan.FromSeconds(3));

			var result = handle.Result;
			return result;
		}

		public override void ClearData() {
			
		}
	}
}