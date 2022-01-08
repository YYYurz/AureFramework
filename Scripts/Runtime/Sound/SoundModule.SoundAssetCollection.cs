//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Resource;
using UnityEngine;

namespace AureFramework.Sound
{
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		/// <summary>
		/// 声音资源容器
		/// </summary>
		private class SoundAssetCollection
		{
			private readonly Dictionary<AudioClip, int> audioAssetReferenceDic = new Dictionary<AudioClip, int>();
			private readonly List<AudioClip> needReleaseAudioList = new List<AudioClip>();
			private readonly IResourceModule resourceModule;

			public SoundAssetCollection()
			{
				resourceModule = Aure.GetModule<IResourceModule>();
			}
			
			/// <summary>
			/// 释放所有声音资源
			/// </summary>
			public void ReleaseALlSoundAssets()
			{
				foreach (var audioAssetReference in audioAssetReferenceDic)
				{
					resourceModule.ReleaseAsset(audioAssetReference.Key);
				}

				foreach (var needReleaseAudioAsset in needReleaseAudioList)
				{
					resourceModule.ReleaseAsset(needReleaseAudioAsset);
				}
				
				audioAssetReferenceDic.Clear();
				needReleaseAudioList.Clear();
			}

			/// <summary>
			/// 声音资源引用计数+1
			/// </summary>
			/// <param name="audioAsset"></param>
			public void AddSoundAssetReference(AudioClip audioAsset)
			{
				if (audioAssetReferenceDic.ContainsKey(audioAsset))
				{
					audioAssetReferenceDic[audioAsset]++;
				}
				else
				{
					audioAssetReferenceDic.Add(audioAsset, 1);
				}
			}

			/// <summary>
			/// 声音资源引用计数-1
			/// </summary>
			/// <param name="audioAsset"></param>
			public void ReduceSoundAssetReference(AudioClip audioAsset)
			{
				if (audioAssetReferenceDic.ContainsKey(audioAsset))
				{
					audioAssetReferenceDic[audioAsset]--;
				}

				CheckUnusedSoundAssets();
			}

			/// <summary>
			/// 检查没有引用的声音资源
			/// </summary>
			public void CheckUnusedSoundAssets()
			{
				needReleaseAudioList.Clear();
				foreach (var audioAssetReference in audioAssetReferenceDic)
				{
					if (audioAssetReference.Value <= 0)
					{
						needReleaseAudioList.Add(audioAssetReference.Key);
					}
				}

				InternalReleaseUnusedSoundAssets();
			}

			private void InternalReleaseUnusedSoundAssets()
			{
				foreach (var needReleaseAudioAsset in needReleaseAudioList)
				{
					audioAssetReferenceDic.Remove(needReleaseAudioAsset);
					resourceModule.ReleaseAsset(needReleaseAudioAsset);
				}
			}
		}
	}
}