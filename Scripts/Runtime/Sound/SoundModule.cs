//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.Sound
{
	/// <summary>
	/// 声音模块
	/// </summary>
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		private readonly Dictionary<string, SoundGroup> soundGroupDic = new Dictionary<string, SoundGroup>();
		private int soundIdAccumulator;

		[SerializeField] private SoundGroup[] soundGroupList;

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
			InternalInitializeSoundGroup();
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.Update();
			}
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.ReleaseAllLoadingSound();
			}
			
			soundGroupDic.Clear();
		}

		/// <summary>
		/// 是否存在声音组
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		/// <returns></returns>
		public bool HasSoundGroup(string groupName)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				if (soundGroup.Value.GroupName.Equals(groupName))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 获取声音组
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		/// <returns></returns>
		public ISoundGroup GetSoundGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Debug.LogError("SoundModule : Sound group name is invalid.");
				return null;
			}

			if (soundGroupDic.TryGetValue(groupName, out var soundGroup))
			{
				return soundGroup;
			}

			return null;
		}

		/// <summary>
		/// 播放声音
		/// </summary>
		/// <param name="soundAssetName"> 声音资源名称 </param>
		/// <param name="groupName"> 声音组名称 </param>
		/// <returns> 返回声音唯一Id </returns>
		public int PlaySound(string soundAssetName, string groupName)
		{
			return PlaySound(soundAssetName, groupName, null, null);
		}

		/// <summary>
		/// 播放声音
		/// </summary>
		/// <param name="soundAssetName"> 声音资源名称 </param>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="soundParams"> 声音参数 </param>
		/// <returns> 返回声音唯一Id </returns>
		public int PlaySound(string soundAssetName, string groupName, SoundParams soundParams)
		{
			return PlaySound(soundAssetName, groupName, null, soundParams);
		}

		/// <summary>
		/// 播放声音
		/// </summary>
		/// <param name="soundAssetName"> 声音资源名称 </param>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="bindingGameObj"> 声音绑定游戏物体 </param>
		/// <returns> 返回声音唯一Id </returns>
		public int PlaySound(string soundAssetName, string groupName, GameObject bindingGameObj)
		{
			return PlaySound(soundAssetName, groupName, bindingGameObj, null);
		}

		/// <summary>
		/// 播放声音
		/// </summary>
		/// <param name="soundAssetName"> 声音资源名称 </param>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="bindingGameObj"> 声音绑定游戏物体 </param>
		/// <param name="soundParams"> 声音参数 </param>
		/// <returns> 返回声音唯一Id </returns>
		public int PlaySound(string soundAssetName, string groupName, GameObject bindingGameObj, SoundParams soundParams)
		{
			if (string.IsNullOrEmpty(soundAssetName))
			{
				Debug.LogError("SoundModule : Sound asset name is invalid.");
				return 0;
			}

			var soundGroup = (SoundGroup) GetSoundGroup(groupName);
			if (soundGroup == null)
			{
				Debug.LogError($"SoundModule : Sound group is not exist. Sound group name :{groupName}");
				return 0;
			}

			if (soundParams == null)
			{
				soundParams = SoundParams.Create();
			}

			var soundId = GetSoundId();
			soundGroup.PlaySound(soundId, soundAssetName, bindingGameObj, soundParams);

			return soundId;
		}

		/// <summary>
		/// 停止声音
		/// </summary>
		/// <param name="soundId"> 声音唯一Id </param>
		public void StopSound(int soundId)
		{
			StopSound(soundId, SoundParams.DefaultFadeOutSeconds);
		}

		/// <summary>
		/// 停止声音
		/// </summary>
		/// <param name="soundId"> 声音唯一Id </param>
		/// <param name="fadeOutSeconds"> 声音淡出时间 </param>
		public void StopSound(int soundId, float fadeOutSeconds)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.StopSound(soundId, fadeOutSeconds);
			}
		}

		/// <summary>
		/// 停止所有加载完成正在播放的声音
		/// </summary>
		public void StopAllLoadedSound()
		{
			StopAllLoadedSound(SoundParams.DefaultFadeOutSeconds);
		}
		
		/// <summary>
		/// 停止所有加载完成正在播放的声音
		/// </summary>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		public void StopAllLoadedSound(float fadeOutSeconds)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.StopAllSound(fadeOutSeconds);
			}
		}

		/// <summary>
		/// 停止所有正在加载中的声音
		/// </summary>
		public void StopAllLoadingSound()
		{
			StopAllLoadingSound(SoundParams.DefaultFadeOutSeconds);
		}

		/// <summary>
		/// 停止所有正在加载中的声音
		/// </summary>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		public void StopAllLoadingSound(float fadeOutSeconds)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.ReleaseAllLoadingSound();
			}
		}

		/// <summary>
		/// 暂停声音
		/// </summary>
		/// <param name="soundId"> 唯一声音Id </param>
		public void PauseSound(int soundId)
		{
			PauseSound(soundId, SoundParams.DefaultFadeOutSeconds);
		}

		/// <summary>
		/// 暂停声音
		/// </summary>
		/// <param name="soundId"> 声音唯一Id </param>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		public void PauseSound(int soundId, float fadeOutSeconds)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.PauseSound(soundId, fadeOutSeconds);
			}
		}

		/// <summary>
		/// 暂停一个声音组的所有声音
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		public void PauseGroupSound(string groupName)
		{
			PauseGroupSound(groupName, SoundParams.DefaultFadeOutSeconds);
		}

		/// <summary>
		/// 暂停一个声音组的所有声音
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		public void PauseGroupSound(string groupName, float fadeOutSeconds)
		{
			var soundGroup = (SoundGroup) GetSoundGroup(groupName);
			if (soundGroup == null)
			{
				Debug.LogError($"SoundModule : Sound group is not exist. Sound group name :{groupName}");
				return;
			}
			
			soundGroup.PauseAllSound(fadeOutSeconds);
		}

		/// <summary>
		/// 暂停所有声音
		/// </summary>
		public void PauseAllSound()
		{
			PauseAllSound(SoundParams.DefaultFadeOutSeconds);
		}

		/// <summary>
		/// 暂停所有声音
		/// </summary>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		public void PauseAllSound(float fadeOutSeconds)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.PauseAllSound(fadeOutSeconds);
			}
		}

		/// <summary>
		/// 恢复声音
		/// </summary>
		/// <param name="soundId"> 声音唯一Id </param>
		public void ResumeSound(int soundId)
		{
			ResumeSound(soundId, SoundParams.DefaultFadeInSeconds);
		}

		/// <summary>
		/// 恢复声音
		/// </summary>
		/// <param name="soundId"> 唯一声音Id </param>
		/// <param name="fadeInSeconds"> 淡入时间 </param>
		public void ResumeSound(int soundId, float fadeInSeconds)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.ResumeSound(soundId, fadeInSeconds);
			}
		}

		/// <summary>
		/// 恢复一个声音组的所有声音
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		public void ResumeGroupSound(string groupName)
		{
			ResumeGroupSound(groupName, SoundParams.DefaultFadeInSeconds);
		}
		
		/// <summary>
		/// 恢复一个声音组的所有声音
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="fadeInSeconds"> 淡入时间 </param>
		public void ResumeGroupSound(string groupName, float fadeInSeconds)
		{
			var soundGroup = (SoundGroup) GetSoundGroup(groupName);
			if (soundGroup == null)
			{
				Debug.LogError($"SoundModule : Sound group is not exist. Sound group name :{groupName}");
				return;
			}
			
			soundGroup.ResumeAllSound(fadeInSeconds);
		}

		/// <summary>
		/// 恢复所有声音
		/// </summary>
		public void ResumeAllSound()
		{
			ResumeAllSound(SoundParams.DefaultFadeInSeconds);
		}
		
		/// <summary>
		/// 恢复所有声音
		/// </summary>
		/// <param name="fadeInSeconds"> 淡入时间 </param>
		public void ResumeAllSound(float fadeInSeconds)
		{
			foreach (var soundGroup in soundGroupDic)
			{
				soundGroup.Value.ResumeAllSound(fadeInSeconds);
			}
		}

		private void InternalInitializeSoundGroup()
		{
			foreach (var soundGroup in soundGroupList)
			{
				if (string.IsNullOrEmpty(soundGroup.GroupName))
				{
					Debug.LogError($"SoundModule : Can not create sound group because its name is invalid.");
					continue;
				}

				if (soundGroupDic.ContainsKey(soundGroup.GroupName))
				{
					Debug.LogError($"SoundModule : Can not create sound group because it is already exist. Name :{soundGroup.GroupName}");
					continue;
				}
				
				soundGroup.Init();
				soundGroupDic.Add(soundGroup.GroupName, soundGroup);
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
	}
}