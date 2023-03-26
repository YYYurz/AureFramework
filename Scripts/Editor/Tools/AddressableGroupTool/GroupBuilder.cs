//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace AureFramework.Editor
{
	/// <summary>
	/// 创建分组类
	/// </summary>
	public static class GroupBuilder
	{
		private readonly struct GroupAssetInfo
		{
			public readonly string Path;
			public readonly string Address;

			public GroupAssetInfo(string path, string address)
			{
				Path = path;
				Address = address;
			}
		}
		
		private static readonly Dictionary<string, List<GroupAssetInfo>> AssetDic = new Dictionary<string, List<GroupAssetInfo>>();
		
		public static void ResetGroup(List<GroupSetting> groupSettingList)
		{
			GetAssets(groupSettingList);
			ProcessAssetToGroup();
			AssetDatabase.SaveAssets();
		}

		private static void ProcessAssetToGroup()
		{
			var length = AssetDic.Count;
			var counter = 1f;
			foreach (var assetInfo in AssetDic)
			{
				EditorUtility.DisplayProgressBar("Processing group", assetInfo.Key, counter / length);

				var group = GetGroup(assetInfo.Key);
				foreach (var groupAssetInfo in assetInfo.Value)
				{
					AddAssetEntry(group, groupAssetInfo.Path, groupAssetInfo.Address);
				}

				counter++;
			}
			
			EditorUtility.ClearProgressBar();
		}

		private static AddressableAssetGroup GetGroup(string groupName)
		{
			var name = groupName.Replace("/", "_");
			var group = AddressableAssetSettingsDefaultObject.Settings.FindGroup(name);
			if (group == null)
			{
				group = AddressableAssetSettingsDefaultObject.Settings.CreateGroup(name, false, false, false, null, typeof(Object));
				group.AddSchema<ContentUpdateGroupSchema>();
				group.AddSchema<BundledAssetGroupSchema>();
			} 
			
			var packedAssetsTemplate = AddressableAssetSettingsDefaultObject.Settings.GroupTemplateObjects[0] as AddressableAssetGroupTemplate;
			if (packedAssetsTemplate != null)
			{
				packedAssetsTemplate.ApplyToAddressableAssetGroup(group);
			}

			// AddressableAssetSettingsDefaultObject.Settings.AddLabel(groupName, false);
			EditorUtility.SetDirty(group);
			return group;
		}

		private static void AddAssetEntry(AddressableAssetGroup group, string assetPath, string address)
		{
			var guid = AssetDatabase.AssetPathToGUID(assetPath);
			var entry = group.entries.FirstOrDefault(e => e.guid == guid);

			if (entry == null)
			{
				entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group, false, false);
			}

			entry.address = address;
			// entry.SetLabel(group.Name, true, false, false);
		}

		private static void GetAssets(List<GroupSetting> groupSettingList)
		{
			AssetDic.Clear();

			var length = groupSettingList.Count;
			for (var i = 0; i < length; i++)
			{
				var groupSetting = groupSettingList[i];

				EditorUtility.DisplayProgressBar("Loading assets", groupSetting.AssetPath, (i + 1f) / length);
				
				var path = groupSetting.AssetPath;
				if (File.Exists(path) && groupSetting.ErgodicLayers == 0)
				{
					AddAsset(path, path, groupSetting);
				}
				else if (Directory.Exists(path))
				{
					switch (groupSetting.ErgodicLayers)
					{
						case 0:
						{
							SplitDirectoryAssets(path, groupSetting);
							break;
						}
						case 1:
						{
							var allDirectoryList = Directory.GetDirectories(path);
							foreach (var directory in allDirectoryList)
							{
								SplitDirectoryAssets(directory, groupSetting);
							}
							break;
						}
					}
				}
			}
			
			EditorUtility.ClearProgressBar();
		}
		
		private static void SplitDirectoryAssets(string directoryPath, GroupSetting groupSetting)
		{
			var filePathList = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
			var groupSuffixIndex = 0;
			var byteRecord = 0;
			foreach (var filePath in filePathList)
			{
				if (filePath.Contains(".meta"))
				{
					continue;
				}
				
				var byteCount= File.ReadAllBytes(filePath).Length;
				byteRecord += byteCount;
				if (byteRecord > groupSetting.MaxByte)
				{
					groupSuffixIndex++;
					byteRecord = byteCount;
				}
				
				var groupName = directoryPath + "_" + groupSuffixIndex;
				AddAsset(groupName, filePath.Replace("\\", "/"), groupSetting);
			}
		}

		private static void AddAsset(string groupName, string filePath, GroupSetting groupSetting)
		{
			if (!string.IsNullOrEmpty(groupSetting.FilterPrefix) && groupSetting.FilterPrefixMode && !filePath.StartsWith(groupSetting.FilterPrefix))
			{
				return;
			}
			
			if (!string.IsNullOrEmpty(groupSetting.FilterPrefix) && !groupSetting.FilterPrefixMode && filePath.StartsWith(groupSetting.FilterPrefix))
			{
				return;
			}
			
			if (!string.IsNullOrEmpty(groupSetting.FilterSuffix) && groupSetting.FilterSuffixMode && !filePath.EndsWith(groupSetting.FilterSuffix))
			{
				return;
			}
			
			if (!string.IsNullOrEmpty(groupSetting.FilterSuffix) && !groupSetting.FilterSuffixMode && filePath.EndsWith(groupSetting.FilterSuffix))
			{
				return;
			}
			
			if (!string.IsNullOrEmpty(groupSetting.FilterString) && groupSetting.FilterStringMode && !filePath.Contains(groupSetting.FilterString))
			{
				return;
			}
			
			if (!string.IsNullOrEmpty(groupSetting.FilterString) && !groupSetting.FilterStringMode && filePath.Contains(groupSetting.FilterString))
			{
				return;
			}
			
			if (!AssetDic.ContainsKey(groupName))
			{
				AssetDic.Add(groupName, new List<GroupAssetInfo>());
			}
			
			switch (groupSetting.AddressNaming)
			{
				case 0:
				{
					AssetDic[groupName].Add(new GroupAssetInfo(filePath, filePath));
					break;
				}
				case 1:
				{
					var fileName = Path.GetFileNameWithoutExtension(filePath);
					AssetDic[groupName].Add(new GroupAssetInfo(filePath, fileName));
					break;
				}
			}
		}
	}
}