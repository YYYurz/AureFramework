//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AureFramework.Editor
{
	/// <summary>
	/// 分组配置
	/// </summary>
	[Serializable]
	public sealed class GroupConfig : ScriptableObject
	{
		public const string DefaultGroupConfigPath = "Assets/AureAddressableGroupConfig.asset";

		[SerializeField] private List<GroupSetting> groupSettingList = new List<GroupSetting>();

		public List<GroupSetting> GroupSettingList
		{
			get
			{
				return groupSettingList;
			}
		}

		public static GroupConfig CreateDefaultConfig()
		{
			var addressableGroupConfig = CreateInstance<GroupConfig>();
			AssetDatabase.CreateAsset(addressableGroupConfig, DefaultGroupConfigPath);

			return addressableGroupConfig;
		}

		public static GroupConfig LoadDefaultConfig(string configPath = null)
		{
			if (string.IsNullOrEmpty(configPath))
			{
				return AssetDatabase.LoadAssetAtPath<GroupConfig>(DefaultGroupConfigPath);
			}

			return AssetDatabase.LoadAssetAtPath<GroupConfig>(configPath);
		}
	}
}