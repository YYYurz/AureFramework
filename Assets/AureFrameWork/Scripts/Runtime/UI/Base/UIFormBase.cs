//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace AureFramework.UI {
	/// <summary>
	/// UI实体类
	/// </summary>
	public abstract class UIFormBase : MonoBehaviour {
		private Canvas cachedCanvas;
		private bool isAlreadyInit;

		public bool IsAlreadyInit
		{
			get
			{
				return isAlreadyInit;
			}
		}
		
		/// <summary>
		/// 初始化
		/// </summary>
		public virtual void OnInit() {
			cachedCanvas = gameObject.GetOrAddComponent<Canvas>();
			cachedCanvas.overrideSorting = true;

			var rectTransform = GetComponent<RectTransform>();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.anchoredPosition3D = Vector3.zero;
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.localScale = Vector3.one;

			gameObject.GetOrAddComponent<GraphicRaycaster>();
			isAlreadyInit = true;
		}

		/// <summary>
		/// 打开
		/// </summary>
		/// <param name="userData"> 用户自定义数据 </param>
		public virtual void OnOpen(object userData) { }

		/// <summary>
		///	暂停 
		/// </summary>
		public virtual void OnPause() { }

		/// <summary>
		/// 暂停恢复
		/// </summary>
		public virtual void OnResume() { }

		/// <summary>
		/// 关闭
		/// </summary>
		public virtual void OnClose() { }

		/// <summary>
		/// 销毁
		/// </summary>
		public virtual void OnDestroy() { }

		/// <summary>
		/// 深度改变
		/// </summary>
		public virtual void OnDepthChange() {
			
		}

		/// <summary>
		/// 轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public virtual void OnUpdate(float elapseTime) { }
	}
}