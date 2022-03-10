//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework.Sound
{
	/// <summary>
	/// 声音模块接口
	/// </summary>
	public interface ISoundModule
	{
		/// <summary>
		/// 是否存在声音组
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		/// <returns></returns>
		bool HasSoundGroup(string groupName);

		/// <summary>
		/// 获取声音组
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		/// <returns></returns>
		ISoundGroup GetSoundGroup(string groupName);

		/// <summary>
		/// 播放声音
		/// </summary>
		/// <param name="soundAssetName"> 声音资源名称 </param>
		/// <param name="groupName"> 声音组名称 </param>
		/// <returns> 返回声音唯一Id </returns>
		int PlaySound(string soundAssetName, string groupName);

		/// <summary>
		/// 播放声音
		/// </summary>
		/// <param name="soundAssetName"> 声音资源名称 </param>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="soundParams"> 声音参数 </param>
		/// <returns> 返回声音唯一Id </returns>
		int PlaySound(string soundAssetName, string groupName, SoundParams soundParams);

		/// <summary>
		/// 播放声音
		/// </summary>
		/// <param name="soundAssetName"> 声音资源名称 </param>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="bindingGameObj"> 声音绑定游戏物体 </param>
		/// <returns> 返回声音唯一Id </returns>
		int PlaySound(string soundAssetName, string groupName, GameObject bindingGameObj);

		/// <summary>
		/// 播放声音
		/// </summary>
		/// <param name="soundAssetName"> 声音资源名称 </param>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="bindingGameObj"> 声音绑定游戏物体 </param>
		/// <param name="soundParams"> 声音参数 </param>
		/// <returns> 返回声音唯一Id </returns>
		int PlaySound(string soundAssetName, string groupName, GameObject bindingGameObj, SoundParams soundParams);

		/// <summary>
		/// 停止声音
		/// </summary>
		/// <param name="soundId"> 声音唯一Id </param>
		void StopSound(int soundId);

		/// <summary>
		/// 停止声音
		/// </summary>
		/// <param name="soundId"> 声音唯一Id </param>
		/// <param name="fadeOutSeconds"> 声音淡出时间 </param>
		void StopSound(int soundId, float fadeOutSeconds);

		/// <summary>
		/// 停止所有加载完成正在播放的声音
		/// </summary>
		void StopAllLoadedSound();

		/// <summary>
		/// 停止所有加载完成正在播放的声音
		/// </summary>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		void StopAllLoadedSound(float fadeOutSeconds);

		/// <summary>
		/// 停止所有正在加载中的声音
		/// </summary>
		void StopAllLoadingSound();

		/// <summary>
		/// 停止所有正在加载中的声音
		/// </summary>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		void StopAllLoadingSound(float fadeOutSeconds);

		/// <summary>
		/// 暂停声音
		/// </summary>
		/// <param name="soundId"> 唯一声音Id </param>
		void PauseSound(int soundId);

		/// <summary>
		/// 暂停声音
		/// </summary>
		/// <param name="soundId"> 声音唯一Id </param>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		void PauseSound(int soundId, float fadeOutSeconds);

		/// <summary>
		/// 暂停一个声音组的所有声音
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		void PauseGroupSound(string groupName);

		/// <summary>
		/// 暂停一个声音组的所有声音
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		void PauseGroupSound(string groupName, float fadeOutSeconds);

		/// <summary>
		/// 暂停所有声音
		/// </summary>
		void PauseAllSound();

		/// <summary>
		/// 暂停所有声音
		/// </summary>
		/// <param name="fadeOutSeconds"> 淡出时间 </param>
		void PauseAllSound(float fadeOutSeconds);

		/// <summary>
		/// 恢复声音
		/// </summary>
		/// <param name="soundId"> 声音唯一Id </param>
		void ResumeSound(int soundId);

		/// <summary>
		/// 恢复声音
		/// </summary>
		/// <param name="soundId"> 唯一声音Id </param>
		/// <param name="fadeInSeconds"> 淡入时间 </param>
		void ResumeSound(int soundId, float fadeInSeconds);

		/// <summary>
		/// 恢复一个声音组的所有声音
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		void ResumeGroupSound(string groupName);

		/// <summary>
		/// 恢复一个声音组的所有声音
		/// </summary>
		/// <param name="groupName"> 声音组名称 </param>
		/// <param name="fadeInSeconds"> 淡入时间 </param>
		void ResumeGroupSound(string groupName, float fadeInSeconds);

		/// <summary>
		/// 恢复所有声音
		/// </summary>
		void ResumeAllSound();

		/// <summary>
		/// 恢复所有声音
		/// </summary>
		/// <param name="fadeInSeconds"> 淡入时间 </param>
		void ResumeAllSound(float fadeInSeconds);
	}
}