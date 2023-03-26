//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace AureFramework.Editor
{
	/// <summary>
	/// Addressable分组工具
	/// </summary>
	public sealed class AddressableGroupTool : EditorWindow
	{
		[MenuItem("Aure/Addressable分组工具", false, 0)]
		private static void OpenWindow()
		{
			var window = GetWindow<AddressableGroupTool>(true, "Addressable分组工具", true);
			window.minSize = window.maxSize = new Vector2(1000f, 400f);
		}

		private readonly List<int> selectGroupIndexList = new List<int>();
		private List<GroupSetting> settingList = new List<GroupSetting>();
		private GroupConfig groupConfig;
		private Vector2 scrollPos;
		private bool isSelectAll;

		private readonly string[] ergodicLayerArray =
		{
			"遍历所有资源为一个Group",
			"第一层文件夹为一个Group",
		};
		
		private readonly string[] addressNamingArray =
		{
			"资源路径",
			"文件名",
		};

		/// <summary>
		/// 设置是否全选
		/// </summary>
		private bool IsSelectAll
		{
			get
			{
				return isSelectAll;
			}
			set
			{
				if (isSelectAll.Equals(value))
				{
					return;
				}

				selectGroupIndexList.Clear();
				if (value)
				{
					for (var i = 0; i < settingList.Count; i++)
					{
						selectGroupIndexList.Add(i);
					}
				}

				isSelectAll = value;
			}
		}

		/// <summary>
		/// 当前编辑的分组配置
		/// </summary>
		private GroupConfig CurrentGroupConfig
		{
			get
			{
				return groupConfig;
			}
			set
			{
				if (value != null)
				{
					GroupConfigGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(value));
				}

				groupConfig = value;
				ReadGroupConfig();
			}
		}

		/// <summary>
		/// 分组配置缓存文件Guid
		/// </summary>
		private string GroupConfigGuid
		{
			get
			{
				return EditorPrefs.GetString("AureAddressableGroupConfigGuid");
			}
			set
			{
				EditorPrefs.SetString("AureAddressableGroupConfigGuid", value);
			}
		}

		private void OnEnable()
		{
			LoadGroupConfig();
			IsSelectAll = true;
		}

		private void OnGUI()
		{
			CurrentGroupConfig = (GroupConfig) EditorGUILayout.ObjectField("分组配置文件", CurrentGroupConfig, typeof(GroupConfig), false);

			EditorGUILayout.BeginHorizontal();
			{
				IsSelectAll = EditorGUILayout.ToggleLeft("全选", IsSelectAll);
				
				if (GUILayout.Button("清除所有Group"))
				{
					RemoveAllGroup();
				}
				
				if (GUILayout.Button("清除所有空Group"))
				{
					RemoveEmptyGroup();
				}

				EditorGUI.BeginDisabledGroup(groupConfig == null);
				{
					if (GUILayout.Button("+"))
					{
						AddSetting();
					}
				}
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup(selectGroupIndexList.Count <= 0);
				{
					if (GUILayout.Button("-"))
					{
						DeleteSetting();
					}
				}
				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginVertical("box", GUILayout.Width(800f), GUILayout.Height(20f));
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField("", GUILayout.Width(30f));
					EditorGUILayout.LabelField("目标路径", GUILayout.Width(160f));
					EditorGUILayout.LabelField("过滤前缀(筛/排)", GUILayout.Width(125f));
					EditorGUILayout.LabelField("过滤后缀(筛/排)", GUILayout.Width(125f));
					EditorGUILayout.LabelField("过滤字符串(筛/排)", GUILayout.Width(120f));
					EditorGUILayout.LabelField("分组资源收集方式", GUILayout.Width(190f));
					EditorGUILayout.LabelField("资源Key命名方式", GUILayout.Width(110f));
					EditorGUILayout.LabelField("每组最大字节", GUILayout.Width(90f));
				}
				EditorGUILayout.EndHorizontal();

				if (settingList != null)
				{
					scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(990f), GUILayout.Height(300f));
					{
						for (var i = 0; i < settingList.Count; i++)
						{
							EditorGUILayout.BeginHorizontal("box", GUILayout.Width(960f), GUILayout.Height(20f));
							{
								var setting = settingList[i];
								var isSelect = selectGroupIndexList.Contains(i);
								if (isSelect != EditorGUILayout.Toggle("", isSelect, GUILayout.Width(20f),
									GUILayout.Height(20f)))
								{
									if (!isSelect)
									{
										selectGroupIndexList.Add(i);
										if (selectGroupIndexList.Count == settingList.Count)
										{
											isSelectAll = true;
										}
									}
									else
									{
										selectGroupIndexList.Remove(i);
										isSelectAll = false;
									}
								}

								var groupTarget = AssetDatabase.LoadAssetAtPath<Object>(setting.AssetPath);
								groupTarget = EditorGUILayout.ObjectField(groupTarget, typeof(Object), false, GUILayout.Width(150f), GUILayout.Height(20f));
								setting.AssetPath = AssetDatabase.GetAssetPath(groupTarget);
								EditorGUILayout.LabelField("", GUILayout.Width(10f));
								setting.FilterPrefixMode = EditorGUILayout.Toggle("", setting.FilterPrefixMode, GUILayout.Width(20f));
								setting.FilterPrefix = EditorGUILayout.TextField("", setting.FilterPrefix, GUILayout.Width(90f), GUILayout.Height(20f));
								EditorGUILayout.LabelField("", GUILayout.Width(10f));
								setting.FilterSuffixMode = EditorGUILayout.Toggle("", setting.FilterSuffixMode, GUILayout.Width(20f));
								setting.FilterSuffix = EditorGUILayout.TextField("", setting.FilterSuffix, GUILayout.Width(90f), GUILayout.Height(20f));
								EditorGUILayout.LabelField("", GUILayout.Width(10f));
								setting.FilterStringMode = EditorGUILayout.Toggle("", setting.FilterStringMode, GUILayout.Width(20f));
								setting.FilterString = EditorGUILayout.TextField("", setting.FilterString, GUILayout.Width(90f), GUILayout.Height(20f));

								var selectErgodicIndex = EditorGUILayout.Popup("", setting.ErgodicLayers, ergodicLayerArray, GUILayout.Width(190f), GUILayout.Height(20f));
								if (selectErgodicIndex != setting.ErgodicLayers)
								{
									setting.ErgodicLayers = selectErgodicIndex;
								}
								
								var selectAddressIndex = EditorGUILayout.Popup("", setting.AddressNaming, addressNamingArray, GUILayout.Width(110f), GUILayout.Height(20f));
								if (selectAddressIndex != setting.AddressNaming)
								{
									setting.AddressNaming = selectAddressIndex;
								}

								setting.MaxByte = EditorGUILayout.IntField("", setting.MaxByte, GUILayout.Width(90f), GUILayout.Height(20f));
							}
							EditorGUILayout.EndHorizontal();
						}
					}
					EditorGUILayout.EndScrollView();
				}
			}
			EditorGUILayout.EndVertical();

			EditorGUI.BeginDisabledGroup(selectGroupIndexList.Count <= 0 || settingList == null);
			{
				if (GUILayout.Button("分组"))
				{
					var selectGroupList = new List<GroupSetting>();
					foreach (var selectIndex in selectGroupIndexList)
					{
						selectGroupList.Add(settingList[selectIndex]);
					}
					
					GroupBuilder.ResetGroup(selectGroupList);
					RemoveEmptyGroup();
				}
			}
			EditorGUI.EndDisabledGroup();
		}

		private void RemoveAllGroup()
		{
			var groupList = AddressableAssetSettingsDefaultObject.Settings.groups.ToArray();
			foreach (var needRemoveGroup in groupList)
			{
				AddressableAssetSettingsDefaultObject.Settings.RemoveGroup(needRemoveGroup);
			}
		}
		
		private void RemoveEmptyGroup()
		{
			var groupList = AddressableAssetSettingsDefaultObject.Settings.groups;
			var needRemoveGroupList = new List<AddressableAssetGroup>();
			foreach (var group in groupList)
			{
				if (group.entries.Count == 0)
				{
					needRemoveGroupList.Add(group);
				}
			}

			foreach (var needRemoveGroup in needRemoveGroupList)
			{
				AddressableAssetSettingsDefaultObject.Settings.RemoveGroup(needRemoveGroup);
			}
		}

		private void AddSetting()
		{
			groupConfig.GroupSettingList.Add(new GroupSetting());
		}

		private void DeleteSetting()
		{
			selectGroupIndexList.Sort();
			for (var i = selectGroupIndexList.Count - 1; i >= 0; i--)
			{
				groupConfig.GroupSettingList.RemoveAt(selectGroupIndexList[i]);
			}

			selectGroupIndexList.Clear();
		}

		private void LoadGroupConfig()
		{
			groupConfig = null;
			var groupConfigPath = AssetDatabase.GUIDToAssetPath(GroupConfigGuid);
			groupConfig = GroupConfig.LoadDefaultConfig(groupConfigPath);

			if (groupConfig == null)
			{
				groupConfig = GroupConfig.CreateDefaultConfig();
				GroupConfigGuid = AssetDatabase.AssetPathToGUID(GroupConfig.DefaultGroupConfigPath);
				Debug.Log("由于不存在默认分组配置文件，或上一次使用的分组配置文件被删除，已在Assets/目录下自动创建AureAddressableGroupConfig.asset");
			}
			
			ReadGroupConfig();
		}

		private void ReadGroupConfig()
		{
			if (groupConfig != null)
			{
				settingList = groupConfig.GroupSettingList;
				EditorUtility.SetDirty(groupConfig);
			}
			else
			{
				settingList = null;
			}
		}
	}
}