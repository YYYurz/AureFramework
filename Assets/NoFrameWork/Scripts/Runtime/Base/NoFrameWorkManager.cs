//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;

namespace NoFrameWork.Runtime
{
	/// <summary>
	/// 框架Manager抽象类
	/// </summary>
	public abstract class NoFrameWorkManager : MonoBehaviour
	{
		protected void Awake() {
			GameMain.RegisterManager(this);
		}
	}	
}
