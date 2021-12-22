//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework;
using AureFramework.Procedure;
using AureFramework.Resource;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameTest {
	public class ProcedureMain : ProcedureBase {
		private LoadAssetCallbacks loadAssetCallbacks;
		private InstantiateGameObjectCallbacks instantiateGameObjectCallbacks;

		public override void OnEnter(params object[] args) {
			base.OnEnter(args);
			loadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetBegin, OnLoadAssetSuccess, OnLoadAssetUpdate, OnLoadAssetFailed);
			instantiateGameObjectCallbacks = new InstantiateGameObjectCallbacks(OnLoadAssetBegin, OnLoadAssetSuccess, OnLoadAssetUpdate, OnLoadAssetFailed);

			GameMain.UI.ObjectPoolExpireTime = 5;
			
			// resourceModule.LoadAssetAsync<GameObject>("Boom", loadAssetCallbacks);
			// resourceModule.LoadAssetAsync<GameObject>("Boom", loadAssetCallbacks);
			// resourceModule.InstantiateAsync("Boom", instantiateGameObjectCallbacks);
			// resourceModule.InstantiateAsync("Boom", instantiateGameObjectCallbacks);
			// resourceModule.InstantiateAsync("TestWindow", instantiateGameObjectCallbacks);
			// resourceModule.LoadSceneAsync("TestScene");
			GameMain.UI.OpenUI("TestWindow", "Normal", null);
			GameMain.UI.CloseUI("TestWindow");
			// GameMain.UI.OpenUI("TestWindow", "Normal", null);
			// GameMain.UI.CloseUI("TestWindow");
			// GameMain.UI.OpenUI("TestWindow", "Normal", null);
			// GameMain.UI.CloseUI("TestWindow");

			var utcDate = DateTime.UtcNow;
			var tempData = utcDate.AddSeconds(1);
			var b = DateTime.Compare(utcDate, tempData);
		}

		public override void OnUpdate() {
			base.OnUpdate();

		}
		
		private void OnLoadAssetBegin(string assetName, int taskId) {
			Debug.Log($"OnLoadAssetBegin  assetName:{assetName}  taskId:{taskId}");
		}			

		private void OnLoadAssetSuccess(string assetName, int taskId, Object asset) {
			Debug.Log($"OnLoadAssetSuccess  assetName:{assetName}  taskId:{taskId}");
		}

		private void OnLoadAssetUpdate(int taskId, float progress) {
			Debug.Log($"OnLoadAssetUpdate  taskId:{taskId}  progress:{progress}");
		}
		
		private void OnLoadAssetFailed(string assetName, int taskId, string errorMessage) {
			Debug.Log($"OnLoadAssetFailed  assetName:{assetName}  taskId:{taskId}  errorMessage:{errorMessage}");
		}
	}
} 