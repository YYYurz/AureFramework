//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ReferencePool;

namespace AureFramework.Sound
{
	/// <summary>
	/// 声音参数
	/// </summary>
	public sealed partial class SoundParams : IReference
	{
		public const float DefaultVolume = 1f;
		public const float DefaultFadeInSeconds = 0f;
		public const float DefaultFadeOutSeconds = 0f;
		public const float DefaultPitch = 1f;
		public const float DefaultPanStereo = 0f;
		public const float DefaultSpatialBlend = 0f;
		public const float DefaultMaxDistance = 10000f;
		public const float DefaultDopplerLevel = 1f;
		public const bool DefaultMute = false;
		public const bool DefaultLoop = false;
		
		public SoundParams()
		{
			Volume = DefaultVolume;
			FadeInSeconds = DefaultFadeInSeconds;
			Pitch = DefaultPitch;
			PanStereo = DefaultPanStereo;
			SpatialBlend = DefaultSpatialBlend;
			MaxDistance = DefaultMaxDistance;
			DopplerLevel = DefaultDopplerLevel;
			Mute = DefaultMute;
			Loop = DefaultLoop;
		}
		
		public float Volume
		{
			get;
			set;
		}
		
		public float FadeInSeconds
		{
			get;
			set;
		}
		
		public float Pitch
		{
			get;
			set;
		}
		
		public float PanStereo
		{
			get;
			set;
		}
		
		public float SpatialBlend
		{
			get;
			set;
		}
		
		public float MaxDistance
		{
			get;
			set;
		}
		
		public float DopplerLevel
		{
			get;
			set;
		}

		public bool Mute
		{
			get;
			set;
		}
		
		public bool Loop
		{
			get;
			set;
		}
		
		public static SoundParams Create()
		{
			var soundParams = Aure.GetModule<IReferencePoolModule>().Acquire<SoundParams>();

			return soundParams;
		}
		
		
		public void Clear()
		{
			Volume = DefaultVolume;
			FadeInSeconds = DefaultFadeInSeconds;
			Pitch = DefaultPitch;
			PanStereo = DefaultPanStereo;
			SpatialBlend = DefaultSpatialBlend;
			MaxDistance = DefaultMaxDistance;
			DopplerLevel = DefaultDopplerLevel;
			Mute = DefaultMute;
			Loop = DefaultLoop;
		}
	}
}