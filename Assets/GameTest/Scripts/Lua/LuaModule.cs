//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework;
using UnityEngine;
using XLua;

namespace GameTest {
	public class LuaModule : AureFrameworkModule, ILuaModule {
		private LuaEnv luaEnv;
		private float tickRecord;
		
		public override int Priority => 10;

		public override void Init() {
			luaEnv = new LuaEnv();

			InitLuaExternalApi();
		}

		public override void Tick(float elapseTime, float realElapseTime) {
			tickRecord += elapseTime;
			if (tickRecord < 1) {
				return;
			}
			
			luaEnv.Tick();
		}

		public override void Clear() {
			
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
		public LuaTable GetLuaTable(string className) {
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
				Debug.LogError("AureFramework LuaModule :" + e.Message);
				return null;
			}
		}
		
		/// <summary>
		/// 调用LuaTable函数
		/// </summary>
		/// <param name="luaTable"> 类名 </param>
		/// <param name="funcName"> 函数名 </param>
		/// <param name="args"> 函数调用参数 </param>
		public void CallLuaFunction(LuaTable luaTable, string funcName, params object[] args)
		{
			if (luaTable != null)
			{
				try
				{
					var luaFunction = luaTable.Get<LuaFunction>(funcName);
					luaFunction.Call(args, null);
					luaFunction.Dispose();
				}
				catch (Exception exception)
				{
					Debug.Log(exception.Message);
				}
			}
			else
			{
				Debug.LogError("AureFramework LuaModule: LuaTable is invalid.");
			}
		}
		
		private void InitLuaExternalApi() {
			if (luaEnv != null) {
				luaEnv.AddLoader(CustomLoader);
			}
		}

		private static byte[] CustomLoader(ref string filePath) {
			if (filePath.Contains("emmy_core")) {
				return null;
			}

			return LuaAsset.Require(filePath);
		}

	}
}