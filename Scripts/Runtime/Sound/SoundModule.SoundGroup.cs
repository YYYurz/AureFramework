//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.ObjectPool;
using UnityEngine;

namespace AureFramework.Sound
{
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		/// <summary>
		/// 内部声音组
		/// </summary>
		private sealed class SoundGroup : ISoundGroup
		{
			private readonly IObjectPoolModule objectPoolModule;
			private readonly IObjectPool<SoundObject> soundObjectPool;
			private List<SoundObject> usingSoundObjectList;
			private SoundModule soundModule;

			[SerializeField] private int soundAgentPoolCapacity;
			[SerializeField] private float soundAgentPoolExpireTime;
			
			public SoundGroup(SoundModule soundModule)
			{
				this.soundModule = soundModule;
				objectPoolModule = Aure.GetModule<IObjectPoolModule>();
				soundObjectPool = objectPoolModule.CreateObjectPool<SoundObject>("Sound Agent Pool", soundAgentPoolCapacity, soundAgentPoolExpireTime);
			}

			/// <summary>
			/// 获取或设置UI对象池容量
			/// </summary>
			public int SoundAgentPoolCapacity
			{
				get
				{
					return soundObjectPool.Capacity;
				}
				set
				{
					soundObjectPool.Capacity = soundAgentPoolCapacity = value;
				}
			}

			/// <summary>
			/// 获取或设置UI对象池过期时间
			/// </summary>
			public float SoundAgentPoolExpireTime
			{
				get
				{
					return soundAgentPoolExpireTime;
				}
				set
				{
					soundObjectPool.ExpireTime = soundAgentPoolExpireTime = value;
				}
			}

			public void Update()
			{
				
			}

			public void PlaySound(int soundId, AudioClip audioAsset, SoundParams soundParams)
			{
				foreach (var soundObject in usingSoundObjectList)
				{
					if (soundObject.SoundAgent.SoundId == soundId)
					{
						
					}
				}
			}

			public void StopSound(int soundId, float fadeOutSeconds)
			{
				
			}
		}
	}
}