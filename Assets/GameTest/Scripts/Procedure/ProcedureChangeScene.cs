//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.Procedure;
using AureFramework.Scene;
using UnityEngine;

namespace GameTest {
	public class ProcedureChangeScene : ProcedureBase {
		private string loadingSceneName;
		private bool isLoadSceneComplete;
		
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);
			
			GameMain.Event.Subscribe<LoadSceneUpdateEventArgs>(OnLoadSceneUpdateCallback);
			GameMain.Event.Subscribe<LoadSceneSuccessEventArgs>(OnLoadSceneSuccessCallback);

			isLoadSceneComplete = false;
			loadingSceneName = (string) args[0];
			GameMain.Scene.LoadScene(loadingSceneName);			
		}

		public override void OnUpdate() {
			base.OnUpdate();

			if (!isLoadSceneComplete) {
				return;
			}
			
			ChangeState<ProcedureMain>();
		}

		public override void OnExit() {
			base.OnExit();
			
			GameMain.Event.Unsubscribe<LoadSceneUpdateEventArgs>(OnLoadSceneUpdateCallback);
			GameMain.Event.Unsubscribe<LoadSceneSuccessEventArgs>(OnLoadSceneSuccessCallback);
		}

		private void OnLoadSceneUpdateCallback(object sender, AureEventArgs e) {
			var args = (LoadSceneUpdateEventArgs) e;

			Debug.Log($"Loading Scene, SceneName :{args.SceneName}, Progress :{args.Progress}");
		}
		
		private void OnLoadSceneSuccessCallback(object sender, AureEventArgs e) {
			var args = (LoadSceneSuccessEventArgs) e;
			
			if (args.SceneName.Equals(loadingSceneName)) {
				isLoadSceneComplete = true;
			}
		}
	}
}