//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace NoFrameWork.Runtime
{
	public partial class BossManager
	{
		public static UIManager UI
		{
			get;
			private set;
		}
		
		public static SceneManager Scene
		{
			get;
			private set;
		}

		private static void InitBuiltinManager() {
			UI = GameMain.GetManager<UIManager>();
			Scene = GameMain.GetManager<SceneManager>();
		}
	}
}