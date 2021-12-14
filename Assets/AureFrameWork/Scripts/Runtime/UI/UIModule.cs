//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.Event;
using AureFramework.ObjectPool;
using AureFramework.Resource;
using UnityEngine;

namespace AureFramework.UI
{
	/// <summary>
	/// UI模块
	/// </summary>
	public sealed partial class UIModule : AureFrameworkModule, IUIModule
	{
		private readonly Dictionary<string, UIGroup> uiGroupDic = new Dictionary<string, UIGroup>();
		private readonly Dictionary<string, IObject<GameObject>> usingUIObject = new Dictionary<string, IObject<GameObject>>(); 
		private readonly List<string> loadingUINameList = new List<string>();
		private IObjectPool<GameObject> uiObjectPool;

		public override int Priority => 10;

		public override void Init() {
			Aure.GetModule<IEventModule>().Subscribe<LoadAssetSuccessEventArgs>(OnLoadAssetSuccess);
			Aure.GetModule<IEventModule>().Subscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
			uiObjectPool = Aure.GetModule<IObjectPoolModule>().CreateObjectPool<GameObject>("UI Pool", 100, 240);
		}

		public override void Tick(float elapseTime, float realElapseTime) {
			
		}

		public override void Clear() {
			Aure.GetModule<IEventModule>().Unsubscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
			Aure.GetModule<IEventModule>().Unsubscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
		}
		
		public void OpenUI(string uiName, string uiGroupName, object userData) {
			if (string.IsNullOrEmpty(uiName)) {
				Debug.LogError("AureFramework UIModule : UI name is null.");
				return;
			}

			if (string.IsNullOrEmpty(uiGroupName)) {
				Debug.LogError("AureFramework UIModule : UI group name is null.");
				return;
			}

			
		}
		
		public void CloseUI(string uiName) {
			
		}

		public void CloseAllLoadingUI() {
			
		}

		public UIForm GetAlreadyOpenUI(string uiName) {
			return null;
		}

		public void SetUIObjectLock(bool isLock) {
			
		}

		public IObject<GameObject> GetUIObject(string uiName) {
			
		}
		
		private void OnLoadAssetSuccess(object sender, AureEventArgs e) {
			
		}
		
		private void OnLoadAssetFailed(object sender, AureEventArgs e) {
			
		}
	}
}