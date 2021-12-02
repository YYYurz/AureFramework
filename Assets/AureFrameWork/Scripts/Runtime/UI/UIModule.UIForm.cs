//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework.UI {
	public sealed partial class UIModule : AureFrameworkModule{
		public abstract class UIForm : MonoBehaviour{
			
			/// <summary>
			/// 初始化
			/// </summary>
			protected internal virtual void OnInit() {
				
			}

			/// <summary>
			/// 打开
			/// </summary>
			/// <param name="userData"> 用户自定义数据 </param>
			protected internal virtual void OnOpen(object userData) {
				
			}
			
			/// <summary>
			///	暂停 
			/// </summary>
			protected internal virtual void OnPause() {
				
			}
			
			/// <summary>
			/// 暂停恢复
			/// </summary>
			protected internal virtual void OnResume() {
				
			}

			/// <summary>
			/// 关闭
			/// </summary>
			protected internal virtual void OnClose() {
				
			}
			
			/// <summary>
			/// 回收
			/// </summary>
			protected internal virtual void OnRecycle() {
				
			}
			
			/// <summary>
			/// 深度改变
			/// </summary>
			protected internal virtual void OnDepthChange() {
				
			}
		}
	}
}