//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Resource;
using AureFramework.Utility;
using UnityEngine;

namespace AureFramework.Sound
{
	/// <summary>
	/// 声音模块
	/// </summary>
	public class SoundModule : AureFrameworkModule, ISoundModule
	{
		private IResourceModule resourceModule;
		private LoadAssetCallbacks loadAssetCallbacks;
		
		public override void Init()
		{
			resourceModule = Aure.GetModule<IResourceModule>();
			loadAssetCallbacks = new LoadAssetCallbacks(null, OnLoadAssetSuccess, null, OnLoadAssetFailed);
		}

		public override void Tick(float elapseTime, float realElapseTime)
		{
			
		}

		public override void Clear()
		{
			
		}

		public void PlaySound(string soundName, GameObject gameObj)
		{
			var audioSource = gameObj.GetOrAddComponent<AudioSource>();
			resourceModule.LoadAssetAsync<AudioClip>(soundName, loadAssetCallbacks, null);
			
		}

		private void OnLoadAssetSuccess(string assetName, int taskId, Object asset, object userData)
		{
			
		}

		private void OnLoadAssetFailed(string assetName, int taskId, string errorMessage)
		{
			
		}
	}
}