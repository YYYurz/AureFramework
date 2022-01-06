//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
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

			public SoundGroup SoundGroup
			{
				get;
				private set;
			}
			
			public SoundParams SoundParams
			{
				get;
				private set;
			}

			public GameObject BindingGameObj
			{
				get;
				private set;
			}

			public static PlaySoundInfo Create(int soundId, SoundGroup soundGroup, SoundParams soundParams, GameObject bindingGameObj)
			{
				var playSoundInfo = Aure.GetModule<IReferencePoolModule>().Acquire<PlaySoundInfo>();
				playSoundInfo.SoundId = soundId;
				playSoundInfo.SoundGroup = soundGroup;
				playSoundInfo.SoundParams = soundParams;
				playSoundInfo.BindingGameObj = bindingGameObj;

				return playSoundInfo;
			}
			
			public void Clear()
			{
				SoundId = 0;
				SoundGroup = null;
				SoundParams = null;
				BindingGameObj = null;
			}
		}
	}
}