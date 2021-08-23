//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFrameWork.Runtime
{
	public class SceneManager : AureFrameworkManager
	{
		protected override void Awake() {
			base.Awake();
			Debug.Log("This is SceneManager");
		}

		public void Print() {
			Debug.Log("Print SceneManager");
		}
	}
}