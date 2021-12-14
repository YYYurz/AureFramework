//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework.UI {
	/// <summary>
	/// UI实体类
	/// </summary>
	public abstract class UIForm : MonoBehaviour {
		private string uiName;
		private bool isPause;
		private IUIGroup uiGroup;

		/// <summary>
		/// UI名称
		/// </summary>
		public string UIName
		{
			get
			{
				return uiName;
			}
		}

		public bool IsPause
		{
			get
			{
				return isPause;
			}
		}

		public IUIGroup UIGroup
		{
			get
			{
				return uiGroup;
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public virtual void OnInit() {
		}

		/// <summary>
		/// 打开
		/// </summary>
		/// <param name="userData"> 用户自定义数据 </param>
		public virtual void OnOpen(object userData) {
		}

		/// <summary>
		///	暂停 
		/// </summary>
		public virtual void OnPause() {
		}

		/// <summary>
		/// 暂停恢复
		/// </summary>
		public virtual void OnResume() {
		}

		/// <summary>
		/// 关闭
		/// </summary>
		public virtual void OnClose() {
		}

		/// <summary>
		/// 回收
		/// </summary>
		public virtual void OnRecycle() {
		}

		/// <summary>
		/// 深度改变
		/// </summary>
		public virtual void OnDepthChange() {
		}

		/// <summary>
		/// 轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public virtual void OnUpdate(float elapseTime) {
		}
	}
}