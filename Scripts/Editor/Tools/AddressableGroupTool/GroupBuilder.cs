//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------


using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AureFramework.Editor
{
	/// <summary>
	/// 创建分组类
	/// </summary>
	public static class GroupBuilder
	{
		private static readonly Dictionary<string, List<string>> AssetDic = new Dictionary<string, List<string>>();
		
		public static void ResetGroup(List<GroupSetting> groupSettingList)
		{
			GetAssets(groupSettingList);
		}

		private static void GetAssets(List<GroupSetting> groupSettingList)
		{
			AssetDic.Clear();
			
			foreach (var groupSetting in groupSettingList)
			{
				var path = groupSetting.AssetPath;
				if (File.Exists(path) && groupSetting.ErgodicLayers == 0)
				{
					AssetDic.Add(path, new List<string>() {path});
					Debug.Log(path);
				}
				else if (Directory.Exists(path))
				{
					var allFileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
					foreach (var file in allFileList)
					{
						if (!file.Contains(".meta"))
						{
							Debug.Log(file);
						}
					}
				}
				Debug.Log("================================================");
			}
		}
	}
}