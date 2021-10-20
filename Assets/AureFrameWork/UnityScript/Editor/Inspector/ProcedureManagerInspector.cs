//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using AureFramework.Procedure;
using AureFramework.Runtime.Procedure;
using UnityEditor;
using UnityEngine;

namespace AureFramework.Editor {
	[CustomEditor(typeof(ProcedureManager))]
	public class ProcedureManagerInspector : UnityEditor.Editor {
		private SerializedProperty allProcedureTypeNameList;
		private SerializedProperty entranceProcedureTypeName;
		
		private List<string> procedureTypeNameList = new List<string>();

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			
			serializedObject.Update();

			var t = (ProcedureManager)target;

			if (string.IsNullOrEmpty(entranceProcedureTypeName.stringValue)) {
				EditorGUILayout.HelpBox("Entrance procedure is invalid.", MessageType.Error);
			}

			if (EditorApplication.isPlaying) {
				EditorGUILayout.LabelField("Current Procedure", t.CurrentProcedure == null ? "None" : t.CurrentProcedure.GetType().ToString());
			}
			
			
		}

		private void OnEnable() {
			allProcedureTypeNameList = serializedObject.FindProperty("procedureNameList");
			entranceProcedureTypeName = serializedObject.FindProperty("entranceProcedureName");
			
			RefreshProcedureList();
		}

		private void RefreshProcedureList() {
			procedureTypeNameList.Clear();

			var assmeblies = Assembly.Load("Assembly-CSharp");
			var types = assmeblies.GetTypes();

			foreach (var type in types) {
				if (type.IsClass && !type.IsAbstract && type.IsAssignableFrom(typeof(ProcedureBase))) {
					procedureTypeNameList.Add(type.FullName);
				}
			}
		}

		private void WriteProcedure() {
			
		}
	}
}