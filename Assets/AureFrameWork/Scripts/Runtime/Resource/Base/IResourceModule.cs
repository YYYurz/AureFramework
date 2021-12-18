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
		/// <param name="loadAssetCallbacks"> 加载资源回调 </param>
		/// <typeparam name="T"></typeparam>
		void LoadAssetAsync<T>(string assetName, LoadAssetCallbacks loadAssetCallbacks = null) where T : UnityEngine.Object;
				
		/// <summary>
		/// 异步克隆
		/// </summary>
		/// <param name="assetName"> 资源Key </param>
		/// <param name="instantiateGameObjectCallbacks"> 克隆游戏物体回调 </param>
		void InstantiateAsync(string assetName, InstantiateGameObjectCallbacks instantiateGameObjectCallbacks = null);

		/// <summary>
		/// 异步加载场景
		/// </summary>
		/// <param name="sceneAssetName"> 场景资源Key </param>
		/// <param name="loadSceneCallbacks"> 加载场景资源回调 </param>
		/// <returns></returns>
		void LoadSceneAsync(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks = null);

		/// <summary>
		/// 卸载资源
		/// </summary>
		/// <param name="asset"> 要卸载的资源 </param>
		void ReleaseAsset(UnityEngine.Object asset);

		/// <summary>
		///	终止正在加载的任务 
		/// </summary>
		/// <param name="taskId"></param>
		void ReleaseTask(int taskId);

		/// <summary>
		/// 异步卸载场景
		/// </summary>
		/// <param name="scene"> 场景Instance引用 </param>
		/// <param name="callBack"> 卸载完成回调 </param>
		void UnloadSceneAsync(SceneInstance scene, Action callBack = null);
	}
}