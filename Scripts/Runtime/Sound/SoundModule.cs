//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Resource;
using BiuBiu;
using UnityEngine;

namespace AureFramework.Sound
{
	/// <summary>
	/// 声音模块
	/// </summary>
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		private readonly Dictionary<string, SoundGroup> soundGroupDic = new Dictionary<string, SoundGroup>();  
		private IResourceModule resourceModule;
		private LoadAssetCallbacks loadAssetCallbacks;
		private int soundIdAccumulator;

		public override void Init()
		{
			resourceModule = Aure.GetModule<IResourceModule>();
			loadAssetCallbacks = new LoadAssetCallbacks(null, OnLoadAssetSuccess, null, OnLoadAssetFailed);
		}

		public override void Tick(float elapseTime, float realElapseTime)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.Update();
			}
		}

		public override void Clear()
		{
			
		}

		public bool HasSoundGroup(string soundGroupName)
		{
			return false;
		}

		public ISoundGroup GetSoundGroup(string soundGroupName)
		{
			if (string.IsNullOrEmpty(soundGroupName))
			{
				Debug.LogError("SoundModule : Sound group name is invalid.");
				return null;
			}

			if (soundGroupDic.TryGetValue(soundGroupName, out var soundGroup))
			{
				return soundGroup;
			}

			return null;
		}
		
		public int PlaySound(string soundAssetName, string soundGroupName)
		{
			return PlaySound(soundAssetName, soundGroupName, null, null);
		}
		
		public int PlaySound(string soundAssetName, string soundGroupName, SoundParams soundParams)
		{
			return PlaySound(soundAssetName, soundGroupName, null, soundParams);
		}
		
		public int PlaySound(string soundAssetName, string soundGroupName, GameObject bindingGameObj)
		{
			return PlaySound(soundAssetName, soundGroupName, bindingGameObj, null);
		}
		
		public int PlaySound(string soundAssetName, string soundGroupName, GameObject bindingGameObj, SoundParams soundParams)
		{
			if (string.IsNullOrEmpty(soundAssetName))
			{
				Debug.LogError("SoundModule : Sound asset name is invalid.");
				return 0;
			}

			var soundGroup = (SoundGroup) GetSoundGroup(soundGroupName);
			if (soundGroup == null)
			{
				Debug.LogError($"SoundModule : Sound group is not exist. Sound group name :{soundGroupName}");
				return 0;
			}

			if (soundParams == null)
			{
				soundParams = SoundParams.Create();
			}

			var soundId = GetSoundId();
			resourceModule.LoadAssetAsync<AudioClip>(soundAssetName, loadAssetCallbacks, PlaySoundInfo.Create(soundId, soundGroup, soundParams, bindingGameObj));

			return soundId;
		}

		public void StopSound(int soundId)
		{
			StopSound(soundId, SoundParams.DefaultFadeOutSeconds);
		}

		public void StopSound(int soundId, float fadeOutSeconds)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.StopSound(soundId, fadeOutSeconds);
			}
		}

		private int GetSoundId()
		{
			while (true)
			{
				++soundIdAccumulator;
				if (soundIdAccumulator == int.MaxValue)
				{
					soundIdAccumulator = 1;
				}

				return soundIdAccumulator;
			}
		}
		
		private void OnLoadAssetSuccess(string assetName, int taskId, Object asset, object userData)
		{
			var playSoundInfo = (PlaySoundInfo) userData;

			var soundGroup = playSoundInfo.SoundGroup;
			soundGroup.
		}

		private void OnLoadAssetFailed(string assetName, int taskId, string errorMessage, object userData)
		{
			
		}
	}
}