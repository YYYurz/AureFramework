//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFrameWork.Runtime
{
	public class Singleton<T> where T : class, new()
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null) {
					var t = new T();
					if (t is MonoBehaviour) {
						Debug.LogError("Singleton : MonoBehaviour can not be singleton");
						return null;
					}

					instance = t;
				}
				return instance;
			}
		}
	}

	public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
	{
		public static T Instance
		{
			get
			{
				var instance = FindObjectOfType(typeof(T)) as T;
				if (instance == null) {
					Debug.LogError("MonoBehaviourSingleton : Instance is null");
					return default;
				}

				return instance;
			}
		}
	}
}