//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ReferencePool;
using UnityEditor;

namespace AureFramework.Editor
{
	[CustomEditor(typeof(ReferencePoolModule))]
	public class ReferencePoolModuleInspector : AureFrameworkInspector
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			if (!EditorApplication.isPlaying)
			{
				EditorGUILayout.HelpBox("Show in runtime.", MessageType.Info);
				return;
			}

			var referenceInfoList = ReferencePoolModule.GetReferenceInfoList();
			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("Class", "Unused\tUsing\tAcquire\tRelease");
			foreach (var referenceInfo in referenceInfoList)
			{
				EditorGUILayout.LabelField(referenceInfo.TypeName,
					$"{referenceInfo.UnusedReferenceCount}\t{referenceInfo.UsingReferenceCount}\t{referenceInfo.AcquireReferenceCount}\t{referenceInfo.ReleaseReferenceCount}");
			}

			EditorGUILayout.EndVertical();
		}
	}
}