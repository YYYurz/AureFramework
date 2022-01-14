//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEditor;

namespace AureFramework.Editor
{
	[CustomEditor(typeof(GroupConfig))]
	public sealed class GroupConfigInspector : AureFrameworkInspector
	{
		private SerializedProperty groupSettingList;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUI.BeginDisabledGroup(true);
			{
				EditorGUILayout.PropertyField(groupSettingList);
			}
			EditorGUI.EndDisabledGroup();
		}

		private void OnEnable()
		{
			groupSettingList = serializedObject.FindProperty("groupSettingList");
		}
	}
}