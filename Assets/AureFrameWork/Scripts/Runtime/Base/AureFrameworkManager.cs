﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework.Runtime
{
	public abstract class AureFrameworkManager : MonoBehaviour
	{
		protected virtual void Awake() {
			GameMain.RegisterManager(this);
		}
	}	
}