//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Event;
using UnityEditor;

namespace AureFramework.Editor
{
	[CustomEditor(typeof(EventModule))]
	public class EventModuleInspector : AureFrameworkInspector
	{
		private readonly HashSet<string> openedItems = new HashSet<string>();

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			if (!EditorApplication.isPlaying)
			{
				EditorGUILayout.HelpBox("Show in runtime.", MessageType.Info);
				return;
			}

			var eventInfoList = EventModule.GetEventInfoList();
			foreach (var eventInfo in eventInfoList)
			{
				EditorGUILayout.BeginVertical("box");

				var eventName = eventInfo.EventName;
				var lastState = openedItems.Contains(eventName);
				var currentState = EditorGUILayout.Foldout(lastState, eventName);
				if (currentState != lastState)
				{
					if (currentState)
					{
						openedItems.Add(eventName);
					}
					else
					{
						openedItems.Remove(eventName);
					}
				}

				if (currentState)
				{
					foreach (var subscriber in eventInfo.SubscriberList)
					{
						EditorGUILayout.LabelField(subscriber);
					}

					EditorGUILayout.Separator();
				}

				EditorGUILayout.EndVertical();
			}
		}
	}
}