//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework;
using AureFramework.Procedure;
using AureFramework.Resource;
using UnityEngine;

namespace GameTest {
	public class ProcedureMain : ProcedureBase {
		private LoadAssetCallbacks loadAssetCallbacks;

		
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);

			loadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetBegin, OnLoadAssetSuccess, OnLoadAssetUpdate, OnLoadAssetFailed);
			
			Aure.GetModule<IResourceModule>().LoadAssetAsync<GameObject>("Ball", loadAssetCallbacks);
			Debug.Log($"OnEnter");
		}

		public override void OnUpdate() {
			base.OnUpdate();

		}
		
		private static void OnLoadAssetBegin(string assetName, int taskId) {
			Debug.Log($"OnLoadAssetBegin  assetName:{assetName}  taskId:{taskId}");
		}			

		private static void OnLoadAssetSuccess(string assetName, int taskId, Object asset) {
			Debug.Log($"OnLoadAssetSuccess  assetName:{assetName}  taskId:{taskId}");
		}

		private static void OnLoadAssetUpdate(int taskId, float progress) {
			Debug.Log($"OnLoadAssetUpdate  taskId:{taskId}  progress:{progress}");
		}
		
		private static void OnLoadAssetFailed(string assetName, int taskId, string errorMessage) {
			Debug.Log($"OnLoadAssetFailed  assetName:{assetName}  taskId:{taskId}  errorMessage:{errorMessage}");
		}
	}
} 