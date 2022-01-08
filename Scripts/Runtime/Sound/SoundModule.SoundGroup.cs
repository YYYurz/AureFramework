 //------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using AureFramework.Utility;
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
			private readonly List<SoundObject> usingSoundAgentObjectList = new List<SoundObject>();
			private readonly SoundAssetCollection soundAssetCollection;
			private readonly IObjectPool<SoundObject> soundAgentObjectPool;
			private readonly IReferencePoolModule referencePoolModule;
			private readonly string groupName;
			private const string AgentName = "SoundAgent";
			private bool mute;
			private float volume;

			[SerializeField] private int soundAgentPoolCapacity;
			[SerializeField] private float soundAgentPoolExpireTime;
			
			public SoundGroup(string groupName, SoundAssetCollection soundAssetCollection)
			{
				this.groupName = groupName;
				this.soundAssetCollection = soundAssetCollection;
				referencePoolModule = Aure.GetModule<IReferencePoolModule>();
				soundAgentObjectPool = Aure.GetModule<IObjectPoolModule>().CreateObjectPool<SoundObject>("Group " + groupName +  " Agent Pool", soundAgentPoolCapacity, soundAgentPoolExpireTime);
				soundAgentObjectPool.Capacity = soundAgentPoolCapacity;
				soundAgentObjectPool.ExpireTime = soundAgentPoolExpireTime;
			}

			/// <summary>
			/// 获取声音组名称
			/// </summary>
			public string GroupName
			{
				get
				{
					return groupName;
				}
			}

			/// <summary>
			/// 获取或设置UI对象池容量
			/// </summary>
			public int SoundAgentPoolCapacity
			{
				get
				{
					return soundAgentObjectPool.Capacity;
				}
				set
				{
					soundAgentObjectPool.Capacity = soundAgentPoolCapacity = value;
				}
			}

			/// <summary>
			/// 获取或设置声音组静音。
			/// </summary>
			public bool Mute
			{
				get
				{
					return mute;
				}
				set
				{
					mute = value;

					foreach (var soundAgentObject in usingSoundAgentObjectList)
					{
						soundAgentObject.SoundAgent.RefreshMute();
					}
				}
			}
			
			/// <summary>
			/// 获取或设置声音组音量。
			/// </summary>
			public float Volume
			{
				get
				{
					return volume;
				}
				set
				{
					volume = value;

					foreach (var soundAgentObject in usingSoundAgentObjectList)
					{
						soundAgentObject.SoundAgent.RefreshVolume();
					}
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
					soundAgentObjectPool.ExpireTime = soundAgentPoolExpireTime = value;
				}
			}

			/// <summary>
			/// 轮询
			/// </summary>
			public void Update()
			{
				CheckUnusedSoundAgentObject();

				foreach (var soundAgentObject in usingSoundAgentObjectList)
				{
					soundAgentObject.SoundAgent.UpdatePosition();
				}
			}

			/// <summary>
			/// 播放声音
			/// </summary>
			/// <param name="soundId"> 唯一声音Id </param>
			/// <param name="audioAsset"> 声音资源 </param>
			/// <param name="bindingGameObj"> 声音绑定游戏物体 </param>
			/// <param name="soundParams"> 声音参数 </param>
			public void PlaySound(int soundId, AudioClip audioAsset, GameObject bindingGameObj, SoundParams soundParams)
			{
				if (InternalTryGetAvailableSoundObject(out var soundAgentObject))
				{
					soundAgentObject.SoundAgent.InitAgent(this, soundId, audioAsset, bindingGameObj, soundParams);
					soundAgentObject.SoundAgent.Play(soundParams.FadeInSeconds);
					soundAssetCollection.AddSoundAssetReference(audioAsset);
					referencePoolModule.Release(soundParams);
				}
				else
				{
					Debug.LogError($"SoundGroup : Sound agent object pool has reached its maximum capacity, play sound failed, sound Id :{soundId}");
				}
			}

			/// <summary>
			/// 停止声音
			/// </summary>
			/// <param name="soundId"> 声音唯一Id </param>
			/// <param name="fadeOutSeconds"> 声音淡出时间 </param>
			public void StopSound(int soundId, float fadeOutSeconds)
			{
				foreach (var soundAgentObject in usingSoundAgentObjectList)
				{
					if (soundAgentObject.SoundAgent.SoundId == soundId)
					{
						soundAgentObject.SoundAgent.Stop(fadeOutSeconds);
						CheckUnusedSoundAgentObject();
						break;
					}
				}
			}

			/// <summary>
			/// 停止所有声音
			/// </summary>
			/// <param name="fadeOutSeconds"> 淡出时间 </param>
			public void StopAllSound(float fadeOutSeconds)
			{
				foreach (var soundAgentObject in usingSoundAgentObjectList)
				{
					soundAgentObject.SoundAgent.Stop(fadeOutSeconds);
					CheckUnusedSoundAgentObject();
					break;
				}
			}

			/// <summary>
			/// 暂停声音
			/// </summary>
			/// <param name="soundId"> 唯一声音Id </param>
			/// <param name="fadeOutSeconds"> 淡出时间 </param>
			public void PauseSound(int soundId, float fadeOutSeconds)
			{
				foreach (var soundAgentObject in usingSoundAgentObjectList)
				{
					if (soundAgentObject.SoundAgent.SoundId == soundId)
					{
						soundAgentObject.SoundAgent.Pause(fadeOutSeconds);
					}
				}
			}
			
			/// <summary>
			/// 恢复声音
			/// </summary>
			/// <param name="soundId"> 唯一声音Id </param>
			/// <param name="fadeInSeconds"> 淡出时间 </param>
			public void ResumeSound(int soundId, float fadeInSeconds)
			{
				foreach (var soundAgentObject in usingSoundAgentObjectList)
				{
					if (soundAgentObject.SoundAgent.SoundId == soundId)
					{
						soundAgentObject.SoundAgent.Resume(fadeInSeconds);
					}
				}
			}

			private void CheckUnusedSoundAgentObject()
			{
				for (var i = usingSoundAgentObjectList.Count; i >= 0; i--)
				{
					var soundAgentObject = usingSoundAgentObjectList[i];
					if (!soundAgentObject.SoundAgent.IsPause && !soundAgentObject.SoundAgent.IsPlaying)
					{
						soundAgentObjectPool.Recycle(soundAgentObject);
						usingSoundAgentObjectList.Remove(soundAgentObject);
					}
				}
			}

			private bool InternalTryGetAvailableSoundObject(out SoundObject soundAgentObject)
			{
				soundAgentObject = null;
				if (soundAgentObjectPool.CanSpawn(AgentName))
				{
					soundAgentObject = soundAgentObjectPool.Spawn();
					soundAssetCollection.ReduceSoundAssetReference(soundAgentObject.SoundAgent.AudioSource.clip);
					soundAgentObject.SoundAgent.ResetAgent();
					return true;
				}
				
				if (soundAgentObjectPool.UsingCount + soundAgentObjectPool.UnusedCount >= soundAgentObjectPool.Capacity)
				{
					return false;
				}

				soundAgentObject = CreateSoundObject();
				return true;
			}

			private SoundObject CreateSoundObject()
			{
				var soundGameObj = new GameObject();
				var soundAgent = soundGameObj.GetOrAddComponent<SoundAgent>();
				var soundAgentObject = SoundObject.Create(soundGameObj, soundAgent, soundAssetCollection);
				
				soundAgentObjectPool.Register(soundAgentObject, true, "SoundAgent");
				usingSoundAgentObjectList.Add(soundAgentObject);

				return soundAgentObject;
			}
		}
	}
}