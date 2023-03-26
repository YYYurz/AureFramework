//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.UI
{
	/// <summary>
	/// UI组接口
	/// </summary>
	public interface IUIGroup
	{
		/// <summary>
		/// 获取UI组名称
		/// </summary>
		string GroupName
		{
			get;
		}
		
		/// <summary>
		/// 获取或设置UI组深度
		/// </summary>
		int GroupDepth
		{
			get;
			set;
		}
		
		/// <summary>
		/// 是否存在已打开UI
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		/// <returns></returns>
		bool IsHasUI(string uiName);

		/// <summary>
		/// 获取已打开的UIForm
		/// </summary>
		/// <param name="uiName"> UI名称 </param>
		/// <returns></returns>
		UIFormBase GetUIForm(string uiName);

		/// <summary>
		/// 获取所有已打开UIForm
		/// </summary>
		/// <returns></returns>
		UIFormBase[] GetAllUIForm();
	}
}