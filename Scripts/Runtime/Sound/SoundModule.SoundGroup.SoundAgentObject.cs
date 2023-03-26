//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using UnityEngine;

namespace AureFramework.Sound
{
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		private sealed partial class SoundGroup : ISoundGroup
		{
			/// <summary>
			/// 内部声音代理对象
			/// </summary>
			private sealed class SoundAgentObject : ObjectBase
			{
				/// <summary>
				/// 声音游戏物体
				/// </summary>
				public GameObject SoundGameObject
				{
					get;
					private set;
				}

				/// <summary>
				/// 声音代理
				/// </summary>
				public SoundAgent SoundAgent
				{
					get;
					private set;
				}

				public static SoundAgentObject Create(GameObject soundGameObject, SoundAgent soundAgent)
				{
					var soundObject = Aure.GetModule<IReferencePoolModule>().Acquire<SoundAgentObject>();
					soundObject.SoundGameObject = soundGameObject;
					soundObject.SoundAgent = soundAgent;

					return soundObject;
				}

				public override void OnRelease()
				{
					base.OnRelease();

					Destroy(SoundGameObject);
				}

				public override void Clear()
				{
					base.Clear();

					SoundGameObject = null;
					SoundAgent = null;
				}
			}
		}
	}
}