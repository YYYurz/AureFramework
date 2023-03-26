//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ReferencePool;
using UnityEngine;

namespace AureFramework.Sound
{
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		private sealed partial class SoundGroup : ISoundGroup
		{
			/// <summary>
			/// 内部播放声音信息类
			/// </summary>
			private sealed class PlaySoundInfo : IReference
			{
				public int SoundId
				{
					get;
					private set;
				}

				public string SoundAssetName
				{
					get;
					private set;
				}

				public GameObject BindingGameObj
				{
					get;
					private set;
				}
				
				public SoundParams SoundParams
				{
					get;
					private set;
				}

				public static PlaySoundInfo Create(int soundId, string soundAssetName, GameObject bindingGameObj, SoundParams soundParams)
				{
					var playSoundInfo = Aure.GetModule<IReferencePoolModule>().Acquire<PlaySoundInfo>();
					playSoundInfo.SoundId = soundId;
					playSoundInfo.SoundAssetName = soundAssetName;
					playSoundInfo.BindingGameObj = bindingGameObj;
					playSoundInfo.SoundParams = soundParams;

					return playSoundInfo;
				}

				public void Clear()
				{
					SoundId = 0;
					SoundAssetName = null;
					BindingGameObj = null;
					SoundParams = null;
				}
			}
		}
	}
}