//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
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
	public class UnloadSceneSuccessEventArgs : AureEventArgs
	{
		/// <summary>
		/// 场景名称
		/// </summary>
		public string SceneName
		{
			get;
			private set;
		}

		public static UnloadSceneSuccessEventArgs Create(string sceneName)
		{
			var unloadSceneUpdateEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<UnloadSceneSuccessEventArgs>();
			unloadSceneUpdateEventArgs.SceneName = sceneName;

			return unloadSceneUpdateEventArgs;
		}

		public override void Clear()
		{
			SceneName = null;
		}
	}
}