//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework.Runtime
{
	public class UIManager : AureFrameworkManager
	{
		protected override void Awake() {
			base.Awake();
			Debug.Log("This is UIManager");
		}

		public void Print() {
			Debug.Log("Print UIManger");
		}
	}
}