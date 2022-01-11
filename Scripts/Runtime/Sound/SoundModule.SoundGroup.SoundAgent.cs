//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections;
using AureFramework.Utility;
using UnityEngine;

namespace AureFramework.Sound
{
	public sealed partial class SoundModule : AureFrameworkModule, ISoundModule
	{
		private sealed partial class SoundGroup : ISoundGroup
		{
			/// <summary>
			/// 内部声音代理
			/// </summary>
			private sealed class SoundAgent : MonoBehaviour
			{
				private ISoundGroup soundGroup;
				private SoundAssetObject soundAssetObject;
				private AudioSource audioSource;
				private GameObject bindingGameObj;
				private int soundId;
				private bool isPause;
				private bool mute;
				private float volumeWhenPause;
				private float volume;
				
				/// <summary>
				/// 获取声音资源对象
				/// </summary>
				public SoundAssetObject SoundAssetObject
				{
					get
					{
						return soundAssetObject;
					}
				}

				/// <summary>
				/// 获取声音源组件
				/// </summary>
				public AudioSource AudioSource
				{
					get
					{
						return audioSource;
					}
				}

				/// <summary>
				/// 获取声音Id
				/// </summary>
				public int SoundId
				{
					get
					{
						return soundId;
					}
				}

				/// <summary>
				/// 获取或设置音量
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
						RefreshVolume();
					}
				}

				/// <summary>
				/// 获取或设置音调
				/// </summary>
				public float Pitch
				{
					get
					{
						return audioSource.pitch;
					}
					set
					{
						audioSource.pitch = value;
					}
				}

				/// <summary>
				/// 获取或设置立体声声相
				/// </summary>
				public float PanStereo
				{
					get
					{
						return audioSource.panStereo;
					}
					set
					{
						audioSource.panStereo = value;
					}
				}

				/// <summary>
				/// 获取或设置声音空间混合量
				/// </summary>
				public float SpatialBlend
				{
					get
					{
						return audioSource.spatialBlend;
					}
					set
					{
						audioSource.spatialBlend = value;
					}
				}

				/// <summary>
				/// 获取或设置声音最大距离
				/// </summary>
				public float MaxDistance
				{
					get
					{
						return audioSource.maxDistance;
					}
					set
					{
						audioSource.maxDistance = value;
					}
				}

				/// <summary>
				/// 获取或设置多普勒等级
				/// </summary>
				public float DopplerLevel
				{
					get
					{
						return audioSource.dopplerLevel;
					}
					set
					{
						audioSource.dopplerLevel = value;
					}
				}

				/// <summary>
				/// 获取是否正在播放
				/// </summary>
				public bool IsPlaying
				{
					get
					{
						return audioSource.isPlaying;
					}
				}

				/// <summary>
				/// 获取或设置静音
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
						RefreshMute();
					}
				}

				/// <summary>
				/// 获取或设置循环播放
				/// </summary>
				public bool Loop
				{
					get
					{
						return audioSource.loop;
					}
					set
					{
						audioSource.loop = value;
					}
				}

				/// <summary>
				/// 获取是否暂停
				/// </summary>
				public bool IsPause
				{
					get
					{
						return isPause;
					}
				}

				/// <summary>
				/// 初始化声音代理
				/// </summary>
				/// <param name="group"> 所属声音组 </param>
				/// <param name="id"> 唯一声音Id </param>
				/// <param name="assetObject"> 声音资源对象 </param>
				/// <param name="bindingObj"> 绑定游戏物体 </param>
				/// <param name="soundParams"> 声音参数 </param>
				public void InitAgent(ISoundGroup group, int id, SoundAssetObject assetObject, GameObject bindingObj, SoundParams soundParams)
				{
					soundGroup = group;
					audioSource = gameObject.GetOrAddComponent<AudioSource>();
					soundAssetObject = assetObject;
					audioSource.clip = soundAssetObject.AudioAsset;
					bindingGameObj = bindingObj;
					soundId = id;
					Volume = soundParams.Volume;
					Pitch = soundParams.Pitch;
					PanStereo = soundParams.PanStereo;
					SpatialBlend = soundParams.SpatialBlend;
					MaxDistance = soundParams.MaxDistance;
					DopplerLevel = soundParams.DopplerLevel;
					Mute = soundParams.Mute;
					Loop = soundParams.Loop;
				}

				/// <summary>
				/// 重置声音代理
				/// </summary>
				public void ResetAgent()
				{
					audioSource = gameObject.GetOrAddComponent<AudioSource>();
					soundAssetObject = null;
					audioSource.clip = null;
					bindingGameObj = null;
					soundId = 0;
					Volume = SoundParams.DefaultVolume;
					Pitch = SoundParams.DefaultPitch;
					PanStereo = SoundParams.DefaultPanStereo;
					SpatialBlend = SoundParams.DefaultSpatialBlend;
					MaxDistance = SoundParams.DefaultMaxDistance;
					DopplerLevel = SoundParams.DefaultDopplerLevel;
					Mute = SoundParams.DefaultMute;
					Loop = SoundParams.DefaultLoop;
				}

				/// <summary>
				/// 播放
				/// </summary>
				/// <param name="fadeInSeconds"> 淡入时间 </param>
				public void Play(float fadeInSeconds)
				{
					StopAllCoroutines();

					audioSource.Play();
					if (fadeInSeconds > 0f)
					{
						var targetVolume = audioSource.volume;
						audioSource.volume = 0f;
						StartCoroutine(FadeToVolume(targetVolume, fadeInSeconds));
					}
				}

				/// <summary>
				/// 停止
				/// </summary>
				/// <param name="fadeOutSeconds"> 淡出时间 </param>
				public void Stop(float fadeOutSeconds)
				{
					StopAllCoroutines();

					if (fadeOutSeconds > 0f && gameObject.activeInHierarchy)
					{
						StartCoroutine(StopCo(fadeOutSeconds));
					}
					else
					{
						audioSource.Stop();
					}
				}

				/// <summary>
				/// 暂停
				/// </summary>
				/// <param name="fadeOutSeconds"> 淡出时间 </param>
				public void Pause(float fadeOutSeconds)
				{
					StopAllCoroutines();

					isPause = true;
					volumeWhenPause = volume;
					if (fadeOutSeconds > 0f && gameObject.activeInHierarchy)
					{
						StartCoroutine(PauseCo(fadeOutSeconds));
					}
					else
					{
						audioSource.Pause();
					}
				}

				/// <summary>
				/// 恢复
				/// </summary>
				/// <param name="fadeInSeconds"> 淡入时间 </param>
				public void Resume(float fadeInSeconds)
				{
					StopAllCoroutines();

					audioSource.UnPause();
					if (fadeInSeconds > 0f)
					{
						StartCoroutine(FadeToVolume(fadeInSeconds, fadeInSeconds));
					}
					else
					{
						Volume = volumeWhenPause;
					}
				}

				/// <summary>
				/// 更新跟随绑定游戏物体位置
				/// </summary>
				public void UpdatePosition()
				{
					if (bindingGameObj == null)
					{
						return;
					}

					transform.position = bindingGameObj.transform.position;
				}

				/// <summary>
				/// 刷新音量
				/// </summary>
				public void RefreshVolume()
				{
					AudioSource.volume = volume * soundGroup.Volume;
				}

				/// <summary>
				/// 刷新静音
				/// </summary>
				public void RefreshMute()
				{
					AudioSource.mute = mute || soundGroup.Mute;
				}

				private IEnumerator StopCo(float fadeOutSeconds)
				{
					yield return FadeToVolume(0f, fadeOutSeconds);
					audioSource.Stop();
				}

				private IEnumerator PauseCo(float fadeOutSeconds)
				{
					yield return FadeToVolume(0f, fadeOutSeconds);
					audioSource.Pause();
				}

				private IEnumerator FadeToVolume(float targetVolume, float duration)
				{
					var time = 0f;
					var originalVolume = audioSource.volume;
					while (time < duration)
					{
						time += Time.deltaTime;
						audioSource.volume = Mathf.Lerp(originalVolume, targetVolume, time / duration);
						yield return new WaitForEndOfFrame();
					}

					audioSource.volume = targetVolume;
				}
			}
		}
	}
}