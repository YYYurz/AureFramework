//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using XLua;

namespace GameTest {
	public interface ILuaModule {
		/// <summary>
		/// Require Lua脚本
		/// </summary>
		/// <param name="scriptContent"> lua模块名称 </param>
		/// <returns></returns>
		object[] DoString(string scriptContent);

		/// <summary>
		/// 获取全局LuaTable
		/// </summary>
		/// <param name="className"> 类名 </param>
		/// <returns></returns>
		LuaTable GetLuaTable(string className);

		/// <summary>
		/// 无返回值调用Lua全局函数
		/// </summary>
		/// <param name="className"> 类名 </param>
		/// <param name="funcName"> 函数名 </param>
		/// <param name="args"> 函数调用参数 </param>
		void CallLuaFunction(string className, string funcName, params object[] args);

		/// <summary>
		/// 有返回值调用Lua全局函数
		/// </summary>
		/// <param name="className"> 类名 </param>
		/// <param name="funcName"> 函数名 </param>
		/// <param name="typeList"> 返回值类型列表 </param>
		/// <param name="args"> 函数调用参数 </param>
		/// <returns></returns>
		object[] CallLuaFunction(string className, string funcName, Type[] typeList, params object[] args);

		/// <summary>
		/// 调用LuaTable函数
		/// </summary>
		/// <param name="luaTable"> 类名 </param>
		/// <param name="funcName"> 函数名 </param>
		/// <param name="args"> 函数调用参数 </param>
		void CallLuaFunction(LuaTable luaTable, string funcName, params object[] args);
	}
}