//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.ObjectPool;
using UnityEditor;

namespace AureFramework.Editor
{
	[CustomEditor(typeof(ObjectPoolModule))]
	public class ObjectPoolModuleInspector : AureFrameworkInspector
	{
		private readonly HashSet<string> m_OpenedItems = new HashSet<string>();

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			if (!EditorApplication.isPlaying)
			{
				EditorGUILayout.HelpBox("Show in runtime.", MessageType.Info);
				return;
			}

			var objectPoolInfoList = ObjectPoolModule.GetAllObjectPoolInfo();
			EditorGUILayout.LabelField("Object Pool Count", objectPoolInfoList.Length.ToString());

			foreach (var objectPool in objectPoolInfoList)
			{
				DrawObjectPoolInfo(objectPool);
			}

			Repaint();
		}

		private void DrawObjectPoolInfo(ObjectPoolInfo objectPoolInfo)
		{
			var recordName = objectPoolInfo.Name + objectPoolInfo.ObjectType.FullName;
			var lastState = m_OpenedItems.Contains(recordName);
			var currentState = EditorGUILayout.Foldout(lastState, objectPoolInfo.Name);
			if (currentState != lastState)
			{
				if (currentState)
				{
					m_OpenedItems.Add(recordName);
				}
				else
				{
					m_OpenedItems.Remove(recordName);
				}
			}

			if (currentState)
			{
				EditorGUILayout.BeginVertical("box");
				{
					EditorGUILayout.LabelField("Pool Name", objectPoolInfo.Name);
					EditorGUILayout.LabelField("Object Type", objectPoolInfo.ObjectType.FullName);
					EditorGUILayout.LabelField("Capacity", objectPoolInfo.Capacity.ToString());
					EditorGUILayout.LabelField("Using Count", objectPoolInfo.UsingCount.ToString());
					EditorGUILayout.LabelField("Unused Count", objectPoolInfo.UnusedCount.ToString());
					EditorGUILayout.LabelField("Expire Time", objectPoolInfo.ExpireTime.ToString());
					var objectInfos = objectPoolInfo.ObjectInfoList;
					if (objectInfos.Length > 0)
					{
						EditorGUILayout.LabelField("Name", "Is Using\tIs Lock\tLast Use Time");
						foreach (var objectInfo in objectInfos)
						{
							EditorGUILayout.LabelField(
								string.IsNullOrEmpty(objectInfo.Name) ? "<None>" : objectInfo.Name,
								$"{objectInfo.IsInUse}\t{objectInfo.IsLock}\t{objectInfo.LastUseTime.ToLocalTime()}");
						}
					}
					else
					{
						EditorGUILayout.LabelField("Nothing");
					}
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.Separator();
			}
		}
	}
}