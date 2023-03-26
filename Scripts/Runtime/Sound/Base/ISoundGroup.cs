//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Sound
{
	/// <summary>
	/// 声音组接口
	/// </summary>
	public interface ISoundGroup
	{
		/// <summary>
		/// 获取声音组名称
		/// </summary>
		string GroupName
		{
			get;
		}
		
		/// <summary>
		/// 获取或设置UI对象池容量
		/// </summary>
		int SoundAgentPoolCapacity
		{
			get;
			set;
		}
		
		/// <summary>
		/// 获取或设置UI对象池过期时间
		/// </summary>
		float SoundAgentPoolExpireTime
		{
			get;
			set;
		}
		
		/// <summary>
		/// 获取或设置声音组静音。
		/// </summary>
		bool Mute
		{
			get;
			set;
		}

		/// <summary>
		/// 获取或设置声音组音量。
		/// </summary>
		float Volume
		{
			get;
			set;
		}
	}
}