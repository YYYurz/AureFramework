//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.UI
{
	/// <summary>
	/// UI模块接口
	/// </summary>
	public interface IUIModule
	{
		/// <summary>
		/// 获取或设置UI对象池容量
		/// </summary>
		int UIObjectPoolCapacity
		{
			get;
			set;
		}

		/// <summary>
		/// 获取或设置UI对象池过期时间
		/// </summary>
		float UIObjectPoolExpireTime
		{
			get;
			set;
		}

		/// <summary>
		/// UI是否已经打开
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		/// <returns></returns>
		bool IsUIOpen(string uiName);

		/// <summary>
		/// 打开UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		/// <param name="uiAssetName"> UI资源名称 </param>
		/// <param name="uiGroupName"> UI组名称 </param>
		/// <param name="userData"> 用户数据 </param>
		void OpenUI(string uiName, string uiAssetName, string uiGroupName, object userData);

		/// <summary>
		/// 关闭UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		void CloseUI(string uiName);

		/// <summary>
		/// 关闭所有UI
		/// </summary>
		void CloseAllUI();

		/// <summary>
		/// 除了传入UI，关闭所有UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		void CloseAllUIExcept(string uiName);

		/// <summary>
		/// 除了传入UI组，关闭所有UI
		/// </summary>
		/// <param name="groupName"> UI组名称 </param>
		void CloseAllUIExceptGroup(string groupName);

		/// <summary>
		/// 关闭一个UI组的所有UI
		/// </summary>
		/// <param name="groupName"> UI组名称 </param>
		void CloseGroupUI(string groupName);

		/// <summary>
		/// 取消所有处理中、加载中的UI
		/// </summary>
		void CancelAllProcessingUI();

		/// <summary>
		/// UI对象加锁
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		void LockUIObject(string uiName);

		/// <summary>
		/// UI对象解锁
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		void UnlockUIObject(string uiName);

		/// <summary>
		/// 所有UI对象加锁
		/// </summary>
		void LockAllUIObject();

		/// <summary>
		/// 所有UI对象解锁
		/// </summary>
		void UnlockAllUIObject();

		/// <summary>
		/// 获取UI组
		/// </summary>
		/// <param name="groupName"> UI组名称 </param>
		/// <returns></returns>
		IUIGroup GetUIGroup(string groupName);
	}
}