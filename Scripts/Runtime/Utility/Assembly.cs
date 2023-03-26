//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AureFramework.Utility
{
	/// <summary>
	/// 运行时程序集工具
	/// </summary>
	public static class Assembly
	{
		private static readonly List<System.Reflection.Assembly> Assemblies;
		private static readonly Dictionary<string, Type> CachedTypes = new Dictionary<string, Type>(StringComparer.Ordinal);

		static Assembly()
		{
			Assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
		}

		/// <summary>
		/// 获取已加载的程序集。
		/// </summary>
		/// <returns>已加载的程序集。</returns>
		public static List<System.Reflection.Assembly> GetAssemblies()
		{
			return Assemblies;
		}

		/// <summary>
		/// 获取已加载的程序集中的所有类型。
		/// </summary>
		/// <returns> 已加载的程序集中的所有类型 </returns>
		public static List<Type> GetTypes()
		{
			var results = new List<Type>();
			foreach (var assembly in Assemblies)
			{
				results.AddRange(assembly.GetTypes());
			}

			return results;
		}

		/// <summary>
		/// 获取已加载的程序集中的指定类型。
		/// </summary>
		/// <param name="typeName">要获取的类型名。</param>
		/// <returns>已加载的程序集中的指定类型。</returns>
		public static Type GetType(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				Debug.LogError("Utility.Assembly : Type name is invalid.");
				return null;
			}

			if (CachedTypes.TryGetValue(typeName, out var type))
			{
				return type;
			}

			type = Type.GetType(typeName);
			if (type != null)
			{
				CachedTypes.Add(typeName, type);
				return type;
			}

			foreach (var assembly in Assemblies)
			{
				type = Type.GetType($"{typeName}, {assembly.FullName}");
				if (type != null)
				{
					CachedTypes.Add(typeName, type);
					return type;
				}
			}

			return null;
		}
	}
}