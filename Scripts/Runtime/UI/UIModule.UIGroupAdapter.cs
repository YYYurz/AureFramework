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

namespace AureFramework.UI
{
	public sealed partial class UIModule : AureFrameworkModule, IUIModule
	{
		/// <summary>
		/// UI组适配器
		/// </summary>
		private sealed class UIGroupAdapter : MonoBehaviour
		{
			private Canvas cachedCanvas;
			private int groupDepth;

			public void SetDepth(int depth)
			{
				cachedCanvas.overrideSorting = true;
				cachedCanvas.sortingOrder = depth;
				groupDepth = depth;
			}

			private void Awake()
			{
				cachedCanvas = gameObject.GetOrAddComponent<Canvas>();
				gameObject.GetOrAddComponent<GraphicRaycaster>();
			}

			private void Start()
			{
				gameObject.layer = LayerMask.NameToLayer("UI");

				cachedCanvas.overrideSorting = true;
				cachedCanvas.sortingOrder = groupDepth;

				var rectTransform = gameObject.GetComponent<RectTransform>();
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.sizeDelta = Vector2.zero;
				rectTransform.localScale = Vector2.one;
				rectTransform.anchoredPosition3D = Vector3.zero;
			}
		}
	}
}