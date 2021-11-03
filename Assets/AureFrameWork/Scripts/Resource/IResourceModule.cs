//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using Object = UnityEngine.Object;

namespace AureFramework.Resource {
	public interface IResourceModule {
		void LoadAssetAsync<T>(string assetName, Action<T> callBack = null) where T : Object;
		
		T LoadAssetSync<T>(string assetName) where T : Object;
	}
}