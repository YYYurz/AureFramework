//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using AureFramework.Procedure;
using UnityEditor;

namespace AureFramework.Editor
{
	[CustomEditor(typeof(ProcedureModule))]
	public class ProcedureModuleInspector : AureFrameworkInspector
	{
		private SerializedProperty allProcedureTypeNameList;
		private SerializedProperty entranceProcedureTypeName;

		private readonly List<string> procedureTypeNameList = new List<string>();
		private int entranceProcedureIndex;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			var t = (ProcedureModule) target;

			if (EditorApplication.isPlaying)
			{
				EditorGUILayout.LabelField("Current Procedure", t.CurrentProcedure == null ? "None" : t.CurrentProcedure.GetType().ToString());
			}

			EditorGUILayout.LabelField("All Procedure");
			if (procedureTypeNameList.Count > 0)
			{
				EditorGUILayout.BeginVertical("box");
				foreach (var procedureName in procedureTypeNameList)
				{
					EditorGUILayout.LabelField(procedureName);
				}

				EditorGUILayout.EndVertical();
			}
			else
			{
				EditorGUILayout.HelpBox("There is no procedure exists.", MessageType.Warning);
			}

			if (procedureTypeNameList.Count > 0)
			{
				EditorGUILayout.Separator();

				var selectedIndex = EditorGUILayout.Popup("Entrance Procedure", entranceProcedureIndex, procedureTypeNameList.ToArray());
				if (selectedIndex != entranceProcedureIndex)
				{
					entranceProcedureIndex = selectedIndex;
					entranceProcedureTypeName.stringValue = procedureTypeNameList[selectedIndex];
				}

				if (string.IsNullOrEmpty(entranceProcedureTypeName.stringValue))
				{
					EditorGUILayout.HelpBox("Entrance procedure is invalid.", MessageType.Error);
				}
			}

			serializedObject.ApplyModifiedProperties();

			Repaint();
		}

		private void OnEnable()
		{
			allProcedureTypeNameList = serializedObject.FindProperty("allProcedureTypeNameList");
			entranceProcedureTypeName = serializedObject.FindProperty("entranceProcedureTypeName");

			RefreshProcedureList();
		}

		private void RefreshProcedureList()
		{
			procedureTypeNameList.Clear();

			var assmeblies = Assembly.Load("Assembly-CSharp");
			var types = assmeblies.GetTypes();
			var procedureBaseType = typeof(ProcedureBase);

			foreach (var type in types)
			{
				if (type.IsClass && !type.IsAbstract && procedureBaseType.IsAssignableFrom(type))
				{
					procedureTypeNameList.Add(type.FullName);
				}
			}

			if (!string.IsNullOrEmpty(entranceProcedureTypeName.stringValue))
			{
				entranceProcedureIndex = procedureTypeNameList.IndexOf(entranceProcedureTypeName.stringValue);
				if (entranceProcedureIndex < 0)
				{
					entranceProcedureTypeName.stringValue = null;
				}
			}

			WriteAllProcedureType();
		}

		private void WriteAllProcedureType()
		{
			allProcedureTypeNameList.ClearArray();
			if (procedureTypeNameList.Count == 0)
			{
				return;
			}

			var length = procedureTypeNameList.Count;
			for (var i = 0; i < length; i++)
			{
				allProcedureTypeNameList.InsertArrayElementAtIndex(i);
				allProcedureTypeNameList.GetArrayElementAtIndex(i).stringValue = procedureTypeNameList[i];
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}