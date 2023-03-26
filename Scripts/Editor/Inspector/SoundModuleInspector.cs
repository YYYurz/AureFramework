//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Sound;
using UnityEditor;

namespace AureFramework.Editor
{
	[CustomEditor(typeof(SoundModule))]
	public class SoundModuleInspector : AureFrameworkInspector
	{
		private SerializedProperty soundGroupList;
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var t = (SoundModule) target;
			
			EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
			{
				EditorGUILayout.PropertyField(soundGroupList, true);
			}
			EditorGUI.EndDisabledGroup();

			serializedObject.ApplyModifiedProperties();

			Repaint();
		}

		private void OnEnable()
		{
			soundGroupList = serializedObject.FindProperty("soundGroupList");
		}
	}
}