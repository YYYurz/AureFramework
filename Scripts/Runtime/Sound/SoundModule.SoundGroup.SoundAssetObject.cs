//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using AureFramework.Resource;
using UnityEngine;

namespace AureFramework.Sound
{
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		private sealed partial class SoundGroup : ISoundGroup
		{
			/// <summary>
			/// 内部声音资源对象
			/// </summary>
			private sealed class SoundAssetObject : ObjectBase
			{
				/// <summary>
				/// 声音资源
				/// </summary>
				public AudioClip AudioAsset
				{
					get;
					private set;
				}

				public static SoundAssetObject Create(AudioClip audioAsset)
				{
					var soundAssetObject = Aure.GetModule<IReferencePoolModule>().Acquire<SoundAssetObject>();
					soundAssetObject.AudioAsset = audioAsset;

					return soundAssetObject;
				}

				public override void OnRelease()
				{
					base.OnRelease();
					
					Aure.GetModule<IResourceModule>().ReleaseAsset(AudioAsset);
				}

				public override void Clear()
				{
					base.Clear();

					AudioAsset = null;
				}
			}
		}
	}
}