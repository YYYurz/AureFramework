//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------


using AureFramework;
using AureFramework.Fsm;
using AureFramework.Resource;
using AureFramework.Event;
using AureFramework.Lua;
using AureFramework.Procedure;
using AureFramework.UI;

namespace GameTest
{
	public partial class GameEntrance
	{
		public static IFsmModule Fsm { get; private set; }
		public static IResourceModule Resource { get; private set; }
		public static IProcedureModule Procedure { get; private set; }
		public static IEventModule Event { get; private set; }
		public static IUIModule UI { get; private set; }
		public static ILuaModule Lua { get; private set; }
		
		private static void InitBuiltinManagers() {
			Fsm = Aure.GetModule<IFsmModule>();
			Resource = Aure.GetModule<IResourceModule>();
			Procedure = Aure.GetModule<IProcedureModule>();
			Event = Aure.GetModule<IEventModule>();
			UI = Aure.GetModule<IUIModule>();
			Lua = Aure.GetModule<ILuaModule>();
		}
	}
}