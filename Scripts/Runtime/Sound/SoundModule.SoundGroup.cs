//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using AureFramework.Resource;
using AureFramework.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AureFramework.Sound
{
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		/// <summary>
		/// 内部声音组
		/// </summary>
		[Serializable]
		private sealed partial class SoundGroup : ISoundGroup
		{
			
			private readonly List<int> loadingSoundTaskIdList = new List<int>();
			private readonly List<SoundAgentObject> usingSoundAgentObjectList = new List<SoundAgentObject>();
			private readonly List<SoundAssetObject> usingSoundAssetObjectList = new List<SoundAssetObject>();
			private IObjectPool<SoundAgentObject> soundAgentObjectPool;
			private IObjectPool<SoundAssetObject> soundAssetObjectPool;
			private IResourceModule resourceModule;
			private IReferencePoolModule referencePoolModule;
			private LoadAssetCallbacks loadAssetCallbacks;
			private const string AgentName = "SoundAgent";

			[SerializeField] private string groupName;
			[SerializeField] private int soundAgentPoolCapacity;
			[SerializeField] private float soundAgentPoolExpireTime;
			[SerializeField] private int soundAssetPoolCapacity;
			[SerializeField] private float soundAssetPoolExpireTime;
			[SerializeField] private bool mute;
			[SerializeField, Range(0f, 1f)] private float volume;

			public void Init()
			{
				resourceModule = Aure.GetModule<IResourceModule>();
				referencePoolModule = Aure.GetModule<IReferencePoolModule>();
				soundAgentObjectPool = Aure.GetModule<IObjectPoolModule>().CreateObjectPool<SoundAgentObject>("Sound Group " + groupName +  " Agent Pool", soundAgentPoolCapacity, soundAgentPoolExpireTime);
				soundAssetObjectPool = Aure.GetModule<IObjectPoolModule>().CreateObjectPool<SoundAssetObject>("Sound Group " + groupName +  " Asset Pool", soundAssetPoolCapacity, soundAssetPoolExpireTime);
				
				Mute = mute;
				Volume = volume;
				loadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetBegin, OnLoadAssetSuccess, null, OnLoadAssetFailed);
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
			/// 轮询
			/// </summary>
			public void Update()
			{
				CheckUnusedObjects();

				foreach (var soundAgentObject in usingSoundAgentObjectList)
				{
					soundAgentObject.SoundAgent.UpdatePosition();
				}
			}

			/// <summary>
			/// 播放声音
			/// </summary>
			/// <param name="soundId"> 唯一声音Id </param>
			/// <param name="soundAssetName"> 声音资源名称 </param>
			/// <param name="bindingGameObj"> 声音绑定游戏物体 </param>
			/// <param name="soundParams"> 声音参数 </param>
			public void PlaySound(int soundId, string soundAssetName, GameObject bindingGameObj, SoundParams soundParams)
			{
				if (!InternalTryGetAvailableSoundAssetObject(soundAssetName, out var soundAssetObject))
				{
					resourceModule.LoadAssetAsync<AudioClip>(soundAssetName, loadAssetCallbacks, PlaySoundInfo.Create(soundId, soundAssetName, bindingGameObj, soundParams));
					return;
				}
				
				if (InternalTryGetAvailableSoundAgentObject(out var soundAgentObject))
				{
					soundAgentObject.SoundAgent.InitAgent(this, soundId, soundAssetObject, bindingGameObj, soundParams);
					soundAgentObject.SoundAgent.Play(soundParams.FadeInSeconds);
					referencePoolModule.Release(soundParams);
				}
				else
				{
					soundAssetObjectPool.Recycle(soundAssetObject);
					usingSoundAssetObjectList.Remove(soundAssetObject);
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
						CheckUnusedObjects();
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
				}
				
				CheckUnusedObjects();
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
						break;
					}
				}
			}

			/// <summary>
			/// 暂停所有声音
			/// </summary>
			/// <param name="fadeOutSeconds"> 淡入时间 </param>
			public void PauseAllSound(float fadeOutSeconds)
			{
				foreach (var soundAgentObject in usingSoundAgentObjectList)
				{
					soundAgentObject.SoundAgent.Pause(fadeOutSeconds);
				}
			}
			
			/// <summary>
			/// 恢复声音
			/// </summary>
			/// <param name="soundId"> 唯一声音Id </param>
			/// <param name="fadeInSeconds"> 淡入时间 </param>
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

			/// <summary>
			/// 恢复所有声音
			/// </summary>
			/// <param name="fadeInSeconds"> 淡入时间 </param>
			public void ResumeAllSound(float fadeInSeconds)
			{
				foreach (var soundAgentObject in usingSoundAgentObjectList)
				{
					soundAgentObject.SoundAgent.Resume(fadeInSeconds);
				}
			}

			/// <summary>
			/// 释放所有加载中的任务
			/// </summary>
			public void ReleaseAllLoadingSound()
			{
				foreach (var taskId in loadingSoundTaskIdList)
				{
					resourceModule.ReleaseTask(taskId);
				}
			}

			private void CheckUnusedObjects()
			{
				for (var i = usingSoundAgentObjectList.Count - 1; i >= 0; i--)
				{
					var soundAgentObject = usingSoundAgentObjectList[i];
					if (!soundAgentObject.SoundAgent.IsPause && !soundAgentObject.SoundAgent.IsPlaying)
					{
						soundAssetObjectPool.Recycle(soundAgentObject.SoundAgent.SoundAssetObject);
						soundAgentObjectPool.Recycle(soundAgentObject);
						usingSoundAssetObjectList.Remove(soundAgentObject.SoundAgent.SoundAssetObject);
						usingSoundAgentObjectList.Remove(soundAgentObject);
					}
				}
			}

			private bool InternalTryGetAvailableSoundAssetObject(string soundAssetName, out SoundAssetObject soundAssetObject)
			{
				soundAssetObject = null;
				if (soundAssetObjectPool.CanSpawn(soundAssetName))
				{
					soundAssetObject = soundAssetObjectPool.Spawn(soundAssetName);
					usingSoundAssetObjectList.Add(soundAssetObject);
					
					return true;
				}

				return false;
			}
			
			private bool InternalCreateSoundAssetObject(string soundAssetName, AudioClip audioAsset)
			{
				if (soundAssetObjectPool.UsingCount + soundAssetObjectPool.UnusedCount >= soundAssetObjectPool.Capacity)
				{
					Debug.LogError($"SoundGroup : Sound asset object pool has reached its maximum capacity, group name :{groupName}");
					return false;
				}
				
				var soundAssetObject = SoundAssetObject.Create(audioAsset);
				soundAssetObjectPool.Register(soundAssetObject, false, soundAssetName);

				return true;
			}

			private bool InternalTryGetAvailableSoundAgentObject(out SoundAgentObject soundAgentObject)
			{
				soundAgentObject = null;
				if (soundAgentObjectPool.CanSpawn(AgentName))
				{
					soundAgentObject = soundAgentObjectPool.Spawn(AgentName);
					soundAgentObject.SoundAgent.ResetAgent();
					usingSoundAgentObjectList.Add(soundAgentObject);
					
					return true;
				}

				soundAgentObject = InternalCreateSoundAgentObject();
				return true;
			}

			private SoundAgentObject InternalCreateSoundAgentObject()
			{
				if (soundAgentObjectPool.UsingCount + soundAgentObjectPool.UnusedCount >= soundAgentObjectPool.Capacity)
				{
					Debug.LogError($"SoundGroup : Sound agent object pool has reached its maximum capacity, group name :{groupName}");
					return null;
				}
				
				var soundGameObj = new GameObject(AgentName);
				var soundAgent = soundGameObj.GetOrAddComponent<SoundAgent>();
				var soundAgentObject = SoundAgentObject.Create(soundGameObj, soundAgent);
				
				soundAgentObjectPool.Register(soundAgentObject, true, "SoundAgent");
				usingSoundAgentObjectList.Add(soundAgentObject);

				return soundAgentObject;
			}
			
			private void OnLoadAssetBegin(string assetName, int taskId)
			{
				loadingSoundTaskIdList.Add(taskId);
			}

			private void OnLoadAssetSuccess(string assetName, int taskId, Object asset, object userData)
			{
				var playSoundInfo = (PlaySoundInfo) userData;
				var audioAsset = (AudioClip) asset;

				var soundId = playSoundInfo.SoundId;
				var soundAssetName = playSoundInfo.SoundAssetName;
				var bindingGameObj = playSoundInfo.BindingGameObj;
				var soundParams = playSoundInfo.SoundParams;

				referencePoolModule.Release(playSoundInfo);
				loadingSoundTaskIdList.Remove(taskId);

				if (InternalCreateSoundAssetObject(assetName, audioAsset))
				{
					PlaySound(soundId, soundAssetName, bindingGameObj, soundParams);
				}
			}

			private void OnLoadAssetFailed(string assetName, int taskId, string errorMessage, object userData)
			{
				loadingSoundTaskIdList.Remove(taskId);
				Debug.LogError($"SoundModule : Load sound asset failed. Asset name :{assetName}, error message :{errorMessage}");
			}
		}
	}
}