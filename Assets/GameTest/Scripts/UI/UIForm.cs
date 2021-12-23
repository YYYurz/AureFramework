using AureFramework.UI;
using UnityEngine;

namespace GameTest {
	public class UIForm : UIFormBase
	{
		public override void OnInit(object userData) {
			base.OnInit(userData);
			
			Debug.Log("UIForm OnInit" + "  " + name);
		}

		public override void OnOpen(object userData) {
			base.OnOpen(userData);
			
			Debug.Log("UIForm OnOpen" + "  " + name);
		}

		public override void OnPause() {
			base.OnPause();
			
			Debug.Log("UIForm OnPause" + "  " + name);
		}

		public override void OnResume() {
			base.OnResume();
			
			Debug.Log("UIForm OnResume" + "  " + name);
		}

		public override void OnClose() {
			base.OnClose();
			
			Debug.Log("UIForm OnClose" + "  " + name);
		}

		public override void OnDestroy() {
			base.OnDestroy();
			
			Debug.Log("UIForm OnDestroy" + "  " + name);
		}

		public override void OnDepthChange(int depth) {
			base.OnDepthChange(depth);
			
			Debug.Log("UIForm OnDepthChange" + "  " + name);
		}

		public override void OnUpdate(float elapseTime) {
			base.OnUpdate(elapseTime);
			
		}
	}
}

