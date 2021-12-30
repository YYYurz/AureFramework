//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Scene
{
	/// <summary>
	/// 场景模块接口
	/// </summary>
	public interface ISceneModule
	{
		/// <summary>
		/// 场景是否正在加载
		/// </summary>
		/// <param name="sceneName"> 场景名称 </param>
		/// <returns></returns>
		bool SceneIsLoading(string sceneName);

		/// <summary>
		/// 场景是否正在卸载
		/// </summary>
		/// <param name="sceneName">场景名称</param>
		/// <returns></returns>
		bool SceneIsUnloading(string sceneName);

		/// <summary>
		/// 场景是否已经加载完成
		/// </summary>
		/// <param name="sceneName"> 场景名称 </param>
		/// <returns></returns>
		bool SceneIsLoaded(string sceneName);

		/// <summary>
		/// 加载场景
		/// </summary>
		/// <param name="sceneName"> 场景名称 </param>
		void LoadScene(string sceneName);

		/// <summary>
		/// 卸载场景
		/// </summary>
		/// <param name="sceneName"> 场景名称 </param>
		void UnloadScene(string sceneName);
	}
}