//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;

namespace AureFrameWork.Runtime
{
	public class Aure : MonoBehaviour
	{
		private void Awake() {
		}

		private void Start() {
			var uiManager = GameMain.GetManager<UIManager>();
			var sceneManager = GameMain.GetManager<SceneManager>();
			
			uiManager.Print();
			sceneManager.Print();
		}
	}
}