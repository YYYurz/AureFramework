//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.ObjectPool;
using AureFramework.Resource;
using AureFramework.Utility;
using UnityEngine;

namespace AureFramework.UI
{
	/// <summary>
	/// UI模块
	/// </summary>
	public sealed partial class UIModule : AureFrameworkModule, IUIModule
	{
		private readonly Dictionary<string, UIGroup> uiGroupDic = new Dictionary<string, UIGroup>();
		private readonly Dictionary<int, string> loadingUIDic = new Dictionary<int, string>();
		private InstantiateGameObjectCallbacks instantiateGameObjectCallbacks;
		private IObjectPool<UIObject> uiObjectPool;
		private IResourceModule resourceModule;

		[SerializeField] private Transform uiRoot;
		[SerializeField] private int objectPoolCapacity;
		[SerializeField] private float objectPoolExpireTime;
		[SerializeField] private string[] uiGroupList;

		/// <summary>
		/// 获取或设置UI对象池容量
		/// </summary>
		public int ObjectPoolCapacity
		{
			get
			{
				return uiObjectPool.Capacity;
			}
			set
			{
				uiObjectPool.Capacity = objectPoolCapacity = value;
			}
		}

		/// <summary>
		/// 获取或设置UI对象池过期时间
		/// </summary>
		public float ObjectPoolExpireTime
		{
			get
			{
				return objectPoolExpireTime;
			}
			set
			{
				uiObjectPool.ExpireTime = objectPoolExpireTime = value;
			}
		}

		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public override int Priority => 10;

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
			uiRoot.gameObject.layer = LayerMask.NameToLayer("UI");
			instantiateGameObjectCallbacks = new InstantiateGameObjectCallbacks(OnInstantiateUIBegin, OnInstantiateUISuccess, null, OnInstantiateUIFailed);
			resourceModule = Aure.GetModule<IResourceModule>();

			uiObjectPool = Aure.GetModule<IObjectPoolModule>().CreateObjectPool<UIObject>("UI Pool", 100, 240);
			uiObjectPool.Capacity = objectPoolCapacity;
			uiObjectPool.ExpireTime = objectPoolExpireTime;

			var tempGroupDepth = 0;
			foreach (var groupName in uiGroupList)
			{
				AddUIGroup(groupName, tempGroupDepth);
				tempGroupDepth += 3000;
			}
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
			foreach (var uiGroup in uiGroupDic)
			{
				uiGroup.Value.InternalUpdate(realElapseTime);
			}
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			CancelAllProcessingUI();
			uiGroupDic.Clear();
			loadingUIDic.Clear();
		}

		/// <summary>
		/// UI是否已经打开
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		/// <returns></returns>
		public bool IsUIOpen(string uiName)
		{
			foreach (var uiGroup in uiGroupDic)
			{
				if (uiGroup.Value.IsHasUI(uiName))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 打开UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		/// <param name="uiGroupName"> UI组名称 </param>
		/// <param name="userData"> 用户数据 </param>
		public void OpenUI(string uiName, string uiGroupName, object userData)
		{
			if (string.IsNullOrEmpty(uiName))
			{
				Debug.LogError("UIModule : UI name is null.");
				return;
			}

			if (string.IsNullOrEmpty(uiGroupName))
			{
				Debug.LogError("UIModule : UI group name is null.");
				return;
			}

			if (!uiGroupDic.ContainsKey(uiGroupName))
			{
				Debug.LogError("UIModule : UI group is not exist.");
			}

			if (!uiObjectPool.IsHasObject(uiName) && !loadingUIDic.ContainsValue(uiName))
			{
				resourceModule.InstantiateAsync(uiName, instantiateGameObjectCallbacks);
			}

			uiGroupDic[uiGroupName].OpenUI(uiName, userData);
		}

		/// <summary>
		/// 关闭UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		public void CloseUI(string uiName)
		{
			if (string.IsNullOrEmpty(uiName))
			{
				Debug.LogError("UIModule : UI name is null.");
				return;
			}

			foreach (var uiGroup in uiGroupDic)
			{
				uiGroup.Value.CloseUI(uiName);
			}
		}

		/// <summary>
		/// 关闭所有UI
		/// </summary>
		public void CloseAllUI()
		{
			foreach (var uiGroup in uiGroupDic)
			{
				uiGroup.Value.CloseAllUI();
			}
		}

		/// <summary>
		/// 除了传入UI，关闭所有UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		public void CloseAllUIExcept(string uiName)
		{
			if (string.IsNullOrEmpty(uiName))
			{
				Debug.LogError("UIModule : UI name is null.");
				return;
			}

			foreach (var uiGroup in uiGroupDic)
			{
				uiGroup.Value.CloseAllExcept(uiName);
			}
		}

		/// <summary>
		/// 除了传入UI组，关闭所有UI
		/// </summary>
		/// <param name="groupName"> UI组名称 </param>
		public void CloseAllUIExceptGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Debug.LogError("UIModule : UI group name is null.");
				return;
			}

			foreach (var uiGroup in uiGroupDic)
			{
				if (!uiGroup.Value.GroupName.Equals(groupName))
				{
					uiGroup.Value.CloseAllUI();
				}
			}
		}

		/// <summary>
		/// 关闭一个UI组的所有UI
		/// </summary>
		/// <param name="groupName"> UI组名称 </param>
		public void CloseGroupUI(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Debug.LogError("UIModule : UI group name is null.");
				return;
			}

			var uiGroup = InternalGetUIGroup(groupName);
			uiGroup?.CloseAllUI();
		}

		/// <summary>
		/// 取消所有处理中、加载中的UI
		/// </summary>
		public void CancelAllProcessingUI()
		{
			foreach (var loadingTask in loadingUIDic)
			{
				resourceModule.ReleaseTask(loadingTask.Key);
			}

			foreach (var uiGroup in uiGroupDic)
			{
				uiGroup.Value.ClearAllUITask();
			}

			loadingUIDic.Clear();
		}

		/// <summary>
		/// UI对象加锁
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		public void LockUIObject(string uiName)
		{
			if (string.IsNullOrEmpty(uiName))
			{
				Debug.LogError("UIModule : UI name is null.");
				return;
			}

			uiObjectPool.Lock(uiName);
		}

		/// <summary>
		/// UI对象解锁
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		public void UnlockUIObject(string uiName)
		{
			if (string.IsNullOrEmpty(uiName))
			{
				Debug.LogError("UIModule : UI name is null.");
				return;
			}

			uiObjectPool.Unlock(uiName);
		}

		/// <summary>
		/// 所有UI对象加锁
		/// </summary>
		public void LockAllUIObject()
		{
			uiObjectPool.LockAll();
		}

		/// <summary>
		/// 所有UI对象解锁
		/// </summary>
		public void UnlockAllUIObject()
		{
			uiObjectPool.UnlockAll();
		}

		/// <summary>
		/// 添加UI组
		/// </summary>
		/// <param name="groupName">  </param>
		/// <param name="groupDepth"></param>
		public void AddUIGroup(string groupName, int groupDepth)
		{
			if (uiGroupDic.ContainsKey(groupName))
			{
				Debug.LogError("UIModule : UI group is already exist.");
				return;
			}

			var groupGameObject = new GameObject(groupName);
			groupGameObject.transform.SetParent(uiRoot.transform);

			var uiGroupAdapter = groupGameObject.GetOrAddComponent<UIGroupAdapter>();
			uiGroupAdapter.SetDepth(groupDepth);

			var uiGroup = new UIGroup(uiObjectPool, groupName, groupDepth, groupGameObject.transform, uiGroupAdapter);
			uiGroupDic.Add(groupName, uiGroup);
		}

		/// <summary>
		/// 获取UI组
		/// </summary>
		/// <param name="groupName"> UI组名称 </param>
		/// <returns></returns>
		public IUIGroup GetUIGroup(string groupName)
		{
			return InternalGetUIGroup(groupName);
		}

		private UIGroup InternalGetUIGroup(string groupName)
		{
			foreach (var uiGroup in uiGroupDic)
			{
				if (uiGroup.Value.GroupName.Equals(groupName))
				{
					return uiGroup.Value;
				}
			}

			return null;
		}

		private void OnInstantiateUIBegin(string uiName, int taskId)
		{
			if (!loadingUIDic.ContainsKey(taskId))
			{
				loadingUIDic.Add(taskId, uiName);
			}
		}

		private void OnInstantiateUISuccess(string uiName, int taskId, GameObject uiGameObject)
		{
			var uiForm = uiGameObject.GetComponent<UIFormBase>();
			if (uiForm == null)
			{
				foreach (var uiGroup in uiGroupDic)
				{
					uiGroup.Value.DiscardUITask(uiName);
				}

				loadingUIDic.Remove(taskId);
				resourceModule.ReleaseAsset(uiGameObject);
				Debug.LogError("UIModule : Can not find UIForm.");
			}

			var uiObject = UIObject.Create(uiName, uiGameObject);
			uiObjectPool.Register(uiObject, false, uiName);
			loadingUIDic.Remove(taskId);
		}

		private void OnInstantiateUIFailed(string uiName, int taskId, string errorMessage)
		{
			foreach (var uiGroup in uiGroupDic)
			{
				uiGroup.Value.DiscardUITask(uiName);
			}

			loadingUIDic.Remove(taskId);
			Debug.LogError($"UIModule : Load ui Failed, error message :{errorMessage}.");
		}
	}
}