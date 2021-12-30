//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.Scene
{
	/// <summary>
	/// 加载场景进度事件
	/// </summary>
	public class LoadSceneUpdateEventArgs : AureEventArgs
	{
		/// <summary>
		/// 场景名称
		/// </summary>
		public string SceneName
		{
			get;
			private set;
		}

		/// <summary>
		/// 加载进度
		/// </summary>
		public float Progress
		{
			get;
			private set;
		}

		public static LoadSceneUpdateEventArgs Create(string sceneName, float progress)
		{
			var loadSceneUpdateEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<LoadSceneUpdateEventArgs>();
			loadSceneUpdateEventArgs.SceneName = sceneName;
			loadSceneUpdateEventArgs.Progress = progress;

			return loadSceneUpdateEventArgs;
		}

		public override void Clear()
		{
			SceneName = null;
			Progress = 0f;
		}
	}
}