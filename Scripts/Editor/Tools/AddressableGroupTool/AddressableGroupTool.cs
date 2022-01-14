//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
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
			window.minSize = window.maxSize = new Vector2(580f, 400f);
		}

		private readonly List<int> selectGroupIndexList = new List<int>();
		private List<GroupSetting> settingList = new List<GroupSetting>();
		private GroupConfig groupConfig;
		private Vector2 scrollPos;
		private bool isSelectAll;

		private readonly string[] ergodicLayerArray =
		{
			"第零层",
			"第一层",
			"第二层",
			"第三层",
			"第四层",
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

			EditorGUILayout.BeginVertical("box", GUILayout.Width(200f), GUILayout.Height(20f));
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField("", GUILayout.Width(30f));
					EditorGUILayout.LabelField("目标路径", GUILayout.Width(130f));
					EditorGUILayout.LabelField("过滤前缀", GUILayout.Width(90f));
					EditorGUILayout.LabelField("过滤后缀", GUILayout.Width(90f));
					EditorGUILayout.LabelField("分组遍历目录层级", GUILayout.Width(120f));
					EditorGUILayout.LabelField("每组最大字节", GUILayout.Width(90f));
				}
				EditorGUILayout.EndHorizontal();

				if (settingList != null)
				{
					scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(600f), GUILayout.Height(300f));
					{
						for (var i = 0; i < settingList.Count; i++)
						{
							EditorGUILayout.BeginHorizontal("box", GUILayout.Width(200f), GUILayout.Height(20f));
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
								groupTarget = EditorGUILayout.ObjectField(groupTarget, typeof(Object), false, GUILayout.Width(130f), GUILayout.Height(20f));
								setting.AssetPath = AssetDatabase.GetAssetPath(groupTarget);
								setting.Prefix = EditorGUILayout.TextField("", setting.Prefix, GUILayout.Width(90f), GUILayout.Height(20f));
								setting.Suffix = EditorGUILayout.TextField("", setting.Suffix, GUILayout.Width(90f), GUILayout.Height(20f));

								var selectedIndex = EditorGUILayout.Popup("", setting.ErgodicLayers, ergodicLayerArray, GUILayout.Width(120f), GUILayout.Height(20f));
								if (selectedIndex != setting.ErgodicLayers)
								{
									setting.ErgodicLayers = selectedIndex;
								}

								setting.MaxByte = EditorGUILayout.IntField("", setting.MaxByte, GUILayout.Width(90f),
									GUILayout.Height(20f));
							}
							EditorGUILayout.EndHorizontal();
						}
					}
					EditorGUILayout.EndScrollView();
				}
			}
			EditorGUILayout.EndVertical();

			EditorGUI.BeginDisabledGroup(selectGroupIndexList.Count <= 0);
			{
				if (GUILayout.Button("分组"))
				{
					GroupBuilder.ResetGroup(settingList);
				}
			}
			EditorGUI.EndDisabledGroup();
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
				CreateDefaultGroupConfig();
				Debug.Log("由于不存在默认分组配置文件，或上一次使用的分组配置文件被删除，已在Assets/目录下自动创建AureAddressableGroupConfig.asset");
			}

			ReadGroupConfig();
		}

		private void ReadGroupConfig()
		{
			settingList = groupConfig != null ? groupConfig.GroupSettingList : null;
		}

		private void CreateDefaultGroupConfig()
		{
			groupConfig = GroupConfig.CreateDefaultConfig();
			GroupConfigGuid = AssetDatabase.AssetPathToGUID(GroupConfig.DefaultGroupConfigPath);
		}
	}
}