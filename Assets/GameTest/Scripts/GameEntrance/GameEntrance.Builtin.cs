//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------


using AureFramework;
using AureFramework.Event;
using AureFramework.Fsm;
using AureFramework.Lua;
using AureFramework.Procedure;
using AureFramework.Resource;
using AureFramework.UI;

namespace GameTest
{
	public partial class GameEntrance
	{
		public static FsmModule Fsm { get; private set; }
		public static ProcedureModule Procedure { get; private set; }
		public static EventModule Event { get; private set; }
		public static ResourceModule Resource { get; private set; }
		public static UIModule UI { get; private set; }
		public static LuaModule Lua { get; private set; }
		
		private static void InitBuiltinManagers() {
			Fsm = GameMain.GetModule<FsmModule>();
			Procedure = GameMain.GetModule<ProcedureModule>();
			Event = GameMain.GetModule<EventModule>();
			Resource = GameMain.GetModule<ResourceModule>();
			UI = GameMain.GetModule<UIModule>();
			Lua = GameMain.GetModule<LuaModule>();
		}
	}
}