//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

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
		private readonly Dictionary<string, int> loadingUIDic = new Dictionary<string, int>();
		private IObjectPool<GameObject> uiObjectPool;
		private IResourceModule resourceModule;

		public override int Priority => 10;

		public override void Init() {
			Aure.GetModule<IEventModule>().Subscribe<LoadAssetSuccessEventArgs>(OnLoadAssetSuccess);
			Aure.GetModule<IEventModule>().Subscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
			uiObjectPool = Aure.GetModule<IObjectPoolModule>().CreateObjectPool<GameObject>("UI Pool", 100, 240);
			resourceModule = Aure.GetModule<IResourceModule>();
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

			if (!uiObjectPool.IsHasObject(uiName)) {
				resourceModule.LoadAssetAsync<GameObject>(uiName);
			}
			
			
		}
		
		/// <summary>
		/// 关闭UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		public void CloseUI(string uiName) {
			if (string.IsNullOrEmpty(uiName)) {
				Debug.LogError("AureFramework UIModule : UI name is null.");
				return;
			}

			foreach (var uiGroup in uiGroupDic) {
				uiGroup.Value.CloseUI(uiName);
			}
		}

		/// <summary>
		/// 关闭所有UI
		/// </summary>
		public void CloseAllUI() {
			foreach (var uiGroup in uiGroupDic) {
				uiGroup.Value.CloseAllUI();
			}
		}

		/// <summary>
		/// 除了传入UI，关闭所有UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		public void CloseAllUIExcept(string uiName) {
			if (string.IsNullOrEmpty(uiName)) {
				Debug.LogError("AureFramework UIModule : UI name is null.");
				return;
			}

			foreach (var uiGroup in uiGroupDic) {
				uiGroup.Value.CloseAllExcept(uiName);
			}
		}

		/// <summary>
		/// 除了传入UI组，关闭所有UI
		/// </summary>
		/// <param name="groupName"> UI组名称 </param>
		public void CloseAllUIExceptGroup(string groupName) {
			if (string.IsNullOrEmpty(groupName)) {
				Debug.LogError("AureFramework UIModule : UI group name is null.");
				return;
			}
			
			foreach (var uiGroup in uiGroupDic) {
				if (!uiGroup.Value.GroupName.Equals(groupName)){
					uiGroup.Value.CloseAllUI();
				}
			}
		}

		/// <summary>
		/// 关闭一个UI组的所有UI
		/// </summary>
		/// <param name="groupName"> UI组名称 </param>
		public void CloseGroupUI(string groupName) {
			if (string.IsNullOrEmpty(groupName)) {
				Debug.LogError("AureFramework UIModule : UI group name is null.");
				return;
			}
			
			foreach (var uiGroup in uiGroupDic) {
				if (uiGroup.Value.GroupName.Equals(groupName)){
					uiGroup.Value.CloseAllUI();
					break;
				}
			}
		}

		/// <summary>
		/// 取消所有处理中的UI（如加载中未打开的界面，由于前者未加载完成排在后面未能关闭的）
		/// </summary>
		public void CancelAllProcessingUI() {
			
		}

		public UIForm GetAlreadyOpenUI(string uiName) {
			return null;
		}

		public void SetUIObjectLock(bool isLock) {
			
		}

		public IUIGroup GetUIGroup() {
			return null;
		}
		
		private void OnLoadAssetSuccess(object sender, AureEventArgs e) {
			var loadAssetSuccessEventArgs = (LoadAssetSuccessEventArgs) e;
			uiObjectPool.Register((GameObject) loadAssetSuccessEventArgs.Asset, false, loadAssetSuccessEventArgs.AssetName);
		}
		
		private void OnLoadAssetFailed(object sender, AureEventArgs e) {
			
		}

		private void OnUIObjectRelease(IObject<GameObject> uiObject) {
			
		}
	}
}