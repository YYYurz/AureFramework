//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;

namespace AureFramework.Editor
{
	/// <summary>
	/// 单组配置
	/// </summary>
	[Serializable]
	public class GroupSetting
	{
		[SerializeField] private string assetPath;
		[SerializeField] private string prefix;
		[SerializeField] private string suffix;
		[SerializeField] private int ergodicLayers;
		[SerializeField] private int maxByte;

		public GroupSetting()
		{
			ergodicLayers = 0;
			maxByte = int.MaxValue;
		}
		
		/// <summary>
		/// 分组文件夹路径
		/// </summary>
		public string AssetPath
		{
			get
			{
				return assetPath;
			}
			set
			{
				assetPath = value;
			}
		}

		/// <summary>
		/// 过滤前缀
		/// </summary>
		public string Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix = value;
			}
		}

		/// <summary>
		/// 过滤后缀
		/// </summary>
		public string Suffix
		{
			get
			{
				return suffix;
			}
			set
			{
				suffix = value;
			}
		}

		/// <summary>
		/// 遍历层数
		/// </summary>
		public int ErgodicLayers
		{
			get
			{
				return ergodicLayers;
			}
			set
			{
				ergodicLayers = value;
			}
		}

		/// <summary>
		/// 单组最大字节限制
		/// </summary>
		public int MaxByte
		{
			get
			{
				return maxByte;
			}
			set
			{
				maxByte = value;
			}
		}
	}
}