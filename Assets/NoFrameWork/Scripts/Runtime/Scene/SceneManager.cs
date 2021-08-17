//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace NoFrameWork.Runtime
{
	public class SceneManager : NoFrameWorkManager
	{
		public override void Awake() {
			base.Awake();
			Debug.Log("This is SceneManager");
		}
	}
}