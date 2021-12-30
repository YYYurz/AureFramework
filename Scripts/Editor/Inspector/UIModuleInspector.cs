//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
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
		private SerializedProperty objectPoolCapacity;
		private SerializedProperty objectPoolExpireTime;
		private SerializedProperty uiRoot;
		private SerializedProperty uiGroupList;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var t = (UIModule) target;

			var capacity = EditorGUILayout.DelayedIntField("UI Object Pool Capacity", objectPoolCapacity.intValue);
			if (capacity != objectPoolCapacity.intValue)
			{
				if (EditorApplication.isPlaying)
				{
					t.ObjectPoolCapacity = capacity;
				}
				else
				{
					objectPoolCapacity.intValue = capacity;
				}
			}

			var expireTime =
				EditorGUILayout.DelayedFloatField("UI Object Pool Expire Time", objectPoolExpireTime.floatValue);
			if (!expireTime.Equals(objectPoolExpireTime.floatValue))
			{
				if (EditorApplication.isPlaying)
				{
					t.ObjectPoolExpireTime = expireTime;
				}
				else
				{
					objectPoolExpireTime.floatValue = expireTime;
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
			objectPoolCapacity = serializedObject.FindProperty("objectPoolCapacity");
			objectPoolExpireTime = serializedObject.FindProperty("objectPoolExpireTime");
			uiRoot = serializedObject.FindProperty("uiRoot");
			uiGroupList = serializedObject.FindProperty("uiGroupList");
		}
	}
}