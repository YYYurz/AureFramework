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
		[SerializeField] private string filterPrefix;
		[SerializeField] private string filterSuffix;
		[SerializeField] private string filterString;
		[SerializeField] private bool filterPrefixMode;
		[SerializeField] private bool filterSuffixMode;
		[SerializeField] private bool filterStringMode;
		[SerializeField] private int ergodicLayers;
		[SerializeField] private int addressNaming;
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
		public string FilterPrefix
		{
			get
			{
				return filterPrefix;
			}
			set
			{
				filterPrefix = value;
			}
		}

		/// <summary>
		/// 过滤后缀
		/// </summary>
		public string FilterSuffix
		{
			get
			{
				return filterSuffix;
			}
			set
			{
				filterSuffix = value;
			}
		}
		
		/// <summary>
		/// 过滤字符串
		/// </summary>
		public string FilterString
		{
			get
			{
				return filterString;
			}
			set
			{
				filterString = value;
			}
		}
		
		/// <summary>
		/// 过滤前缀模式（筛选/排除）
		/// </summary>
		public bool FilterPrefixMode
		{
			get
			{
				return filterPrefixMode;
			}
			set
			{
				filterPrefixMode = value;
			}
		}
		
		/// <summary>
		/// 过滤后缀模式（筛选/排除）
		/// </summary>
		public bool FilterSuffixMode
		{
			get
			{
				return filterSuffixMode;
			}
			set
			{
				filterSuffixMode = value;
			}
		}
		
		/// <summary>
		/// 过滤字符串模式（筛选/排除）
		/// </summary>
		public bool FilterStringMode
		{
			get
			{
				return filterStringMode;
			}
			set
			{
				filterStringMode = value;
			}
		}

		/// <summary>
		/// 遍历方式
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
		/// 资源Key命名方式
		/// </summary>
		public int AddressNaming
		{
			get
			{
				return addressNaming;
			}
			set
			{
				addressNaming = value;
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