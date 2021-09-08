//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Text;
using UnityEngine;

namespace AureFramework.Runtime
{
	public class Aure : MonoBehaviour
	{
		private void Awake() {
		
		}

		private void Start() {
			// LoadAssetBundle();
		}
		
		

		private void LoadAssetBundle() {
			var assetBundle = AssetBundle.LoadFromFile("Assets/AB/Lua.bundle");
			var allAssetName = assetBundle.GetAllAssetNames();
			foreach (var fileName in allAssetName) {
				var file = assetBundle.LoadAsset<TextAsset>(fileName);
				var byteList = file.bytes;
				
				var content = Encoding.Default.GetString(byteList);
			}
			Debug.Log("");
		}
	}
}