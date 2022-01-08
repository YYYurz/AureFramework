//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using UnityEngine;

namespace AureFramework.Sound
{
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		/// <summary>
		/// 内部声音对象
		/// </summary>
		private sealed class SoundObject : ObjectBase
		{
			private SoundAssetCollection soundAssetCollection;
			
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

			public static SoundObject Create(GameObject soundGameObject, SoundAgent soundAgent, SoundAssetCollection collection)
			{
				var soundObject = Aure.GetModule<IReferencePoolModule>().Acquire<SoundObject>();
				soundObject.soundAssetCollection = collection;
				soundObject.SoundGameObject = soundGameObject;
				soundObject.SoundAgent = soundAgent;

				return soundObject;
			}

			public override void OnRelease()
			{
				base.OnRelease();
				
				soundAssetCollection.ReduceSoundAssetReference(SoundAgent.AudioSource.clip);
				Destroy(SoundGameObject);
			}

			public override void Clear()
			{
				base.Clear();
				
				soundAssetCollection = null;
				SoundGameObject = null;
				SoundAgent = null;
			}
		}
	}
}