//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace NoFrameWork.Runtime
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
}