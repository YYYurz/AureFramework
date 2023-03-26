//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.UI;
using UnityEditor;

namespace AureFramework.Editor
{
	[CustomEditor(typeof(UIModule))]
	public class UIModuleInspector : AureFrameworkInspector
	{
		private SerializedProperty uiObjectPoolCapacity;
		private SerializedProperty uiObjectPoolExpireTime;
		private SerializedProperty uiRoot;
		private SerializedProperty uiGroupList;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var t = (UIModule) target;

			var capacity = EditorGUILayout.DelayedIntField("UI Object Pool Capacity", uiObjectPoolCapacity.intValue);
			if (capacity != uiObjectPoolCapacity.intValue)
			{
				if (EditorApplication.isPlaying)
				{
					t.UIObjectPoolCapacity = capacity;
				}
				else
				{
					uiObjectPoolCapacity.intValue = capacity;
				}
			}

			var expireTime = EditorGUILayout.DelayedFloatField("UI Object Pool Expire Time", uiObjectPoolExpireTime.floatValue);
			if (!expireTime.Equals(uiObjectPoolExpireTime.floatValue))
			{
				if (EditorApplication.isPlaying)
				{
					t.UIObjectPoolExpireTime = expireTime;
				}
				else
				{
					uiObjectPoolExpireTime.floatValue = expireTime;
				}
			}

			EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
			{
				EditorGUILayout.PropertyField(uiRoot);
				EditorGUILayout.PropertyField(uiGroupList, true);
			}
			EditorGUI.EndDisabledGroup();

			serializedObject.ApplyModifiedProperties();

			Repaint();
		}

		private void OnEnable()
		{
			uiObjectPoolCapacity = serializedObject.FindProperty("uiObjectPoolCapacity");
			uiObjectPoolExpireTime = serializedObject.FindProperty("uiObjectPoolExpireTime");
			uiRoot = serializedObject.FindProperty("uiRoot");
			uiGroupList = serializedObject.FindProperty("uiGroupList");
		}
	}
}