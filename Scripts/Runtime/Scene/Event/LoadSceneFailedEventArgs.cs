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
	/// 加载场景失败事件
	/// </summary>
	public class LoadSceneFailedEventArgs : AureEventArgs
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
		/// 错误信息
		/// </summary>
		public string ErrorMessage
		{
			get;
			private set;
		}

		public static LoadSceneFailedEventArgs Create(string sceneName, string errorMessage)
		{
			var loadSceneUpdateEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<LoadSceneFailedEventArgs>();
			loadSceneUpdateEventArgs.SceneName = sceneName;
			loadSceneUpdateEventArgs.ErrorMessage = errorMessage;

			return loadSceneUpdateEventArgs;
		}

		public override void Clear()
		{
			SceneName = null;
			ErrorMessage = null;
		}
	}
}