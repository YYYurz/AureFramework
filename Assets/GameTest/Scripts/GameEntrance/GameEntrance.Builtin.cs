//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------


using AureFramework.Runtime;
using AureFramework.Runtime.Fsm;
using AureFramework.Runtime.Procedure;
using AureFramework.Runtime.Resource;
using AureFramework.Runtime.UI;

namespace GameTest
{
	public partial class GameEntrance
	{
		public static FsmManager Fsm { get; private set; }
		public static ProcedureManager Procedure { get; private set; }
		public static ResourceManager Resource { get; private set; }
		public static UIManager UI { get; private set; }
		
		public static void InitBuiltinManagers() {
			Fsm = Aure.GetManager<FsmManager>();
			Procedure = Aure.GetManager<ProcedureManager>();
			Resource = Aure.GetManager<ResourceManager>();
			UI = Aure.GetManager<UIManager>();
		}
	}
}