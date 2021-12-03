//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.UI {
	public interface IUIGroup {
		
		
		/// <summary>
		/// 界面是否存在
		/// </summary>
		/// <param name="uiName"> 界面资源 </param>
		/// <returns></returns>
		bool HasUIForm(string uiName);

		/// <summary>
		/// 获取界面
		/// </summary>
		/// <param name="uiName"></param>
		/// <returns></returns>
		IUIForm GetUIForm(string uiName);

		/// <summary>
		/// 获取所有界面
		/// </summary>
		/// <returns></returns>
		IUIForm GetAllUIForm();

		/// <summary>
		/// 
		/// </summary>
		void OpenUIForm();
	}
}