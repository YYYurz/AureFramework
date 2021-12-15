//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;
using UnityEngine;

namespace AureFramework.Resource {
	public class LoadAssetSuccessEventArgs : AureEventArgs {
		public int TaskId
		{
			private set;
			get;
		}

		public string AssetName
		{
			private set;
			get;
		}

		public Object Asset
		{
			private set;
			get;
		}

		public static LoadAssetSuccessEventArgs Create(int taskId, string assetName, Object asset) {
			var loadSuccessEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<LoadAssetSuccessEventArgs>();
			loadSuccessEventArgs.TaskId = taskId;
			loadSuccessEventArgs.AssetName = assetName;
			loadSuccessEventArgs.Asset = asset;
			
			return loadSuccessEventArgs;
		}

		public override void Clear() {
			TaskId = -1;
			AssetName = null;
			Asset = null;
		}
	}
}