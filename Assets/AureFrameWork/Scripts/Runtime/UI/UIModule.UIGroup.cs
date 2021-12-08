//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AureFramework.UI {
	public sealed partial class UIModule : AureFrameworkModule, IUIModule {
		/// <summary>
		/// UI组
		/// </summary>
		public sealed class UIGroup {
			private LinkedList<UIForm> uiFormLinked = new LinkedList<UIForm>();
			private string groupName;
			private int depth;

			public void Update(float elapseTime) {
				var curNode = uiFormLinked.First;
				while (curNode != null) {
					if (!curNode.Value.IsPause) {
						curNode.Value.OnUpdate(elapseTime);
					}
					curNode = curNode.Next;
				}
			}

			// public bool HasUIForm(string uiName) {
			// 	
			// }
			
			// public UIForm GetUIForm(string uiName) {
			// 	
			// }

			public UIForm[] GetAllUIForm() {
				return uiFormLinked.ToArray();
			}

			internal void OpenUIForm() {
				
			}
			
			
		}
	}
}