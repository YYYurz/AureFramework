//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace AureFramework.Resource {
	public interface IResourceModule {
		void LoadAssetAsync<T>(string assetName, Action<T> callBack = null) where T : Object;
		
		IEnumerator LoadSceneAsync(string assetName, Action<float> percentCallBack = null,
			Action<SceneInstance> endCallBack = null);
	}
}