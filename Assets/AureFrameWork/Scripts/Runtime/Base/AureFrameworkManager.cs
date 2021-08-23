//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFrameWork.Runtime
{
	public interface IAureFrameworkManager{}
	
	public abstract class AureFrameworkManager : MonoBehaviourSingleton<AureFrameworkManager>, IAureFrameworkManager
	{
		protected virtual void Awake() {
			GameMain.RegisterManager(this);
		}
	}	
}
