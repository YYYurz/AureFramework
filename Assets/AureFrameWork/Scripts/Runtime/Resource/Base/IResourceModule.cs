//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace AureFramework.Resource {
	public interface IResourceModule {
		/// <summary>
		/// 同步克隆
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		GameObject InstantiateSync(string assetName);

		/// <summary>
		/// 异步克隆
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		/// <param name="beginCallBack"> 克隆开始回调，返回异步任务Id </param>
		/// <param name="endCallBack"> 克隆完成回调，返回结果 </param>
		void InstantiateAsync(string assetName, Action<uint> beginCallBack, Action<GameObject> endCallBack = null);

		/// <summary>
		/// 同步加载
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T LoadAssetSync<T>(string assetName) where T : UnityEngine.Object;

		/// <summary>
		/// 异步加载资源
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		/// <param name="beginCallBack"> 克隆开始回调，返回异步任务Id </param>
		/// <param name="endCallBack"> 克隆完成回调，返回结果 </param>
		/// <typeparam name="T"></typeparam>
		void LoadAssetAsync<T>(string assetName, Action<uint> beginCallBack, Action<T> endCallBack = null) where T : UnityEngine.Object;

		/// <summary>
		/// 异步加载场景
		/// </summary>
		/// <param name="sceneName"> 场景资源Key </param>
		/// <param name="percentCallBack"> 加载百分比回调 </param>
		/// <param name="endCallBack"> 加载完成回调 </param>
		/// <returns></returns>
		void LoadSceneAsync(string sceneName, Action<float> percentCallBack, Action<SceneInstance> endCallBack);

		/// <summary>
		/// 卸载资源
		/// </summary>
		/// <param name="asset"> 要卸载的资源 </param>
		void ReleaseAsset(UnityEngine.Object asset);

		/// <summary>
		///	终止正在加载的任务 
		/// </summary>
		/// <param name="taskId"></param>
		void ReleaseTask(uint taskId);

		/// <summary>
		/// 异步卸载场景
		/// </summary>
		/// <param name="scene"> 场景Instance引用 </param>
		/// <param name="callBack"> 卸载完成回调 </param>
		void UnloadSceneAsync(SceneInstance scene, Action callBack = null);
	}
}