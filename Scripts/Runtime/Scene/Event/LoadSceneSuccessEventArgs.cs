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
	/// 加载场景成功事件
	/// </summary>
	public class LoadSceneSuccessEventArgs : AureEventArgs
	{
		/// <summary>
		/// 场景名称
		/// </summary>
		public string SceneName
		{
			get;
			private set;
		}

		public static LoadSceneSuccessEventArgs Create(string sceneName)
		{
			var loadSceneUpdateEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<LoadSceneSuccessEventArgs>();
			loadSceneUpdateEventArgs.SceneName = sceneName;

			return loadSceneUpdateEventArgs;
		}

		public override void Clear()
		{
			SceneName = null;
		}
	}
}