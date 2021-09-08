//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Resources;
using AureFramework.Runtime;
using UnityEngine;

namespace GameTest
{
	public partial class GameEntrance
	{
		public static UIManager UI { get; private set; }
		public static SceneManager Scene { get; private set; }
		public static ResourcesManager Resources { get; private set; }
		
		public static void InitBuiltinManagers() {
			UI = GameMain.GetManager<UIManager>();
			Scene = GameMain.GetManager<SceneManager>();
			Resources = GameMain.GetManager<ResourcesManager>();
		}
	}
}