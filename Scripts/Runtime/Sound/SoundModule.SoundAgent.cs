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
		/// <summary>
		/// 内部声音代理
		/// </summary>
		private sealed class SoundAgent : MonoBehaviour
		{
			private AudioSource audioSource;
			private GameObject bindingGameObj;
			private int soundId;
			private bool isPause;

			public AudioSource AudioSource
			{
				get
				{
					return audioSource;
				}
			}

			public GameObject BindingGameObj
			{
				get
				{
					return bindingGameObj;
				}
			}

			public int SoundId
			{
				get
				{
					return soundId;
				}
			}

			public float Volume
			{
				get
				{
					return audioSource.volume;
				}
				set
				{
					audioSource.volume = value;
				}
			}
			
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
			
			public bool IsPlaying
			{
				get
				{
					return audioSource.isPlaying;
				}
			}

			public bool Mute
			{
				get
				{
					return audioSource.mute;
				}
				set
				{
					audioSource.mute = value;
				}
			}

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

			public bool IsPause
			{
				get
				{
					return isPause;
				}
			}
			
			public void InitAgent(int id, AudioClip audioAsset, GameObject bindingObj, SoundParams soundParams)
			{
				audioSource = gameObject.GetOrAddComponent<AudioSource>();
				audioSource.clip = audioAsset;
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

			public void ResetAgent()
			{
				audioSource = gameObject.GetOrAddComponent<AudioSource>();
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

			public void Play(float fadeInSeconds)
			{
				StopAllCoroutines();
				
				audioSource.Play();
				if (fadeInSeconds > 0f)
				{
					Volume = 0f;
					StartCoroutine(FadeToVolume(Volume, fadeInSeconds));
				}
			}

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

			public void Pause(float fadeOutSeconds)
			{
				StopAllCoroutines();

				isPause = true;
				if (fadeOutSeconds > 0f && gameObject.activeInHierarchy)
				{
					StartCoroutine(PauseCo(fadeOutSeconds));
				}
				else
				{
					audioSource.Pause();
				}
			}

			public void Resume(float fadeInSeconds)
			{
				StopAllCoroutines();

				audioSource.UnPause();
				if (fadeInSeconds > 0f)
				{
					StartCoroutine(FadeToVolume(m_AudioSource, m_VolumeWhenPause, fadeInSeconds));
				}
				else
				{
					m_AudioSource.volume = m_VolumeWhenPause;
				}
			}

			public void UpdatePosition()
			{
				if (bindingGameObj == null)
				{
					return;
				}

				transform.position = bindingGameObj.transform.position;
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

			private IEnumerator FadeToVolume(float volume, float duration)
			{
				var time = 0f;
				var originalVolume = audioSource.volume;
				while (time < duration)
				{
					time += Time.deltaTime;
					audioSource.volume = Mathf.Lerp(originalVolume, volume, time / duration);
					yield return new WaitForEndOfFrame();
				}

				audioSource.volume = volume;
			}
		}
	}
}