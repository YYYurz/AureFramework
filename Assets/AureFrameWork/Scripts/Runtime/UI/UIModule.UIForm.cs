﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework.UI {
	public sealed partial class UIModule : AureFrameworkModule, IUIModule{
		/// <summary>
		/// UI实体类
		/// </summary>
		public abstract class UIForm : MonoBehaviour {
			private string uiFormName;
			private int depthInGroup;
			private bool isPause;
			private UIGroup uiGroup;

			/// <summary>
			/// UI名称
			/// </summary>
			public string UIFormName
			{
				get
				{
					return uiFormName;
				}
			}
			
			/// <summary>
			/// UI在界面组里面的深度
			/// </summary>
			public int DepthInGroup
			{
				get
				{
					return depthInGroup;
				}
			}

			public bool IsPause
			{
				get
				{
					return isPause;
				}
			}

			public UIGroup UIGroup
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
}