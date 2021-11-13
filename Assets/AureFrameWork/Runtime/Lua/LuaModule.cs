//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.Resource;
using GameTest;
using UnityEngine;
using XLua;

namespace AureFramework.Lua {
	public class LuaModule : AureFrameworkModule {
		private LuaEnv luaEnv;

		private float tickRecord;
		public override int Priority => 10;

		protected override void Awake() {
			base.Awake();

			luaEnv = new LuaEnv();

			InitLuaExternalApi();
		}

		public override void Tick() {
			tickRecord += Time.deltaTime;
			if (tickRecord < 1) {
				return;
			}
			
			luaEnv.Tick();
		}

		public override void Clear() {
			
		}

		/// <summary>
		/// 初始化Lua环境第三方接口
		/// </summary>
		private void InitLuaExternalApi() {
			if (luaEnv != null) {
				luaEnv.AddLoader(CustomLoader);
			}
		}

		/// <summary>
		/// 自定义加载器
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private static byte[] CustomLoader(ref string filePath) {
			if (filePath.Contains("emmy_core")) {
				return null;
			}

			// return LuaAsset.Require(filePath);
			var bytes = GameMain.GetModule<ResourceModule>().LoadAssetSync<TextAsset>(filePath).bytes;
			return bytes;
		}

		/// <summary>
		/// Require Lua脚本
		/// </summary>
		/// <param name="scriptContent"> lua模块名称 </param>
		/// <returns></returns>
		public object[] DoString(string scriptContent) {
			if (luaEnv == null) {
				Debug.LogError("AureFramework LuaModule : LuaEnv is null.");
				return null;
			}

			try {
				return luaEnv.DoString(scriptContent);
			}
			catch (Exception e) {
				Debug.LogError("AureFramework LuaModule " + e.Message);
				return null;
			}
		}
		
		/// <summary>
		/// 获取全局LuaTable
		/// </summary>
		/// <param name="className"> 类名 </param>
		/// <returns></returns>
		public LuaTable GetLuaTable(string className)
		{
			if (luaEnv == null) {
				Debug.LogError("AureFramework LuaModule : LuaEnv is null.");
				return null;
			}
			
			var luaTable = luaEnv.Global.Get<LuaTable>(className);
			return luaTable;
		}

		/// <summary>
		/// 无返回值调用Lua全局函数
		/// </summary>
		/// <param name="className"> 类名 </param>
		/// <param name="funcName"> 函数名 </param>
		/// <param name="args"> 函数调用参数 </param>
		public void CallLuaFunction(string className, string funcName, params object[] args) {
			if (luaEnv == null) {
				Debug.LogError("AureFramework LuaModule : LuaEnv is null.");
				return;
			}
			
			try {
				var luaTable = luaEnv.Global.Get<LuaTable>(className);
				var luaFunction = luaTable.Get<LuaFunction>(funcName);
				
				luaFunction.Call(args);
				luaTable.Dispose();
				luaFunction.Dispose();
			}
			catch (Exception e) {
				Debug.LogError("AureFramework LuaModule " + e.Message);
			}
		}

		/// <summary>
		/// 有返回值调用Lua全局函数
		/// </summary>
		/// <param name="className"> 类名 </param>
		/// <param name="funcName"> 函数名 </param>
		/// <param name="typeList"> 返回值类型列表 </param>
		/// <param name="args"> 函数调用参数 </param>
		/// <returns></returns>
		public object[] CallLuaFunction(string className, string funcName, Type[] typeList, params object[] args) {
			if (luaEnv == null) {
				Debug.LogError("AureFramework LuaModule : LuaEnv is null.");
				return null;
			}
			
			try {
				var luaTable = luaEnv.Global.Get<LuaTable>(className);
				var luaFunction = luaTable.Get<LuaFunction>(funcName);
				var result = luaFunction.Call(args, typeList);
				
				luaTable.Dispose();
				luaFunction.Dispose();
				
				return result;
			}
			catch (Exception e) {
				Debug.LogError("AureFramework LuaModule " + e.Message);
				return null;
			}
		}
	}
}