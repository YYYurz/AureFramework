//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------


using AureFramework;

namespace GameTest
{
	public partial class GameMain
	{
		public static ILuaModule Lua
		{
			get;
			private set;
		}
		
		public static void InitCustomManagers() {
			Lua = Aure.GetModule<ILuaModule>();
		}
	}
}