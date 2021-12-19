using AureFramework.UI;
using UnityEngine;

namespace GameTest {
	public class UIForm : UIFormBase
	{
		public override void OnInit() {
			base.OnInit();
			
			Debug.Log("UIForm OnInit");
		}

		public override void OnOpen(object userData) {
			base.OnOpen(userData);
			
			Debug.Log("UIForm OnOpen");
		}

		public override void OnPause() {
			base.OnPause();
			
			Debug.Log("UIForm OnPause");
		}

		public override void OnResume() {
			base.OnResume();
			
			Debug.Log("UIForm OnResume");
		}

		public override void OnClose() {
			base.OnClose();
			
			Debug.Log("UIForm OnClose");
		}

		public override void OnDestroy() {
			base.OnDestroy();
			
			Debug.Log("UIForm OnDestroy");
		}

		public override void OnDepthChange() {
			base.OnDepthChange();
			
			Debug.Log("UIForm OnDepthChange");
		}

		public override void OnUpdate(float elapseTime) {
			base.OnUpdate(elapseTime);
			
			// Debug.Log("UIForm OnOpen");
		}
	}
}

