using System;
using AureFramework;
using AureFramework.Resource;
using UnityEngine;

[Serializable]
public class LuaAsset : ScriptableObject
{
    public static string LuaDecodeKey = "AureAureAure";
    public bool encode = true;
    public byte[] data;

    public static byte[] Require(string luaPath, string search = "", int retry = 0)
    {
        if(string.IsNullOrEmpty(luaPath))
            return null;
            
        byte[] bytes = null;
        var assetName = search + luaPath.Replace(".", "/");
        {
            var asset = Aure.GetModule<IResourceModule>().LoadAssetSync<LuaAsset>(assetName);
            if (asset != null)
            {
                bytes = asset.GetDecodeBytes();
            }
        }

        Debug.Assert(bytes != null, $"{luaPath} not found.");
        return bytes;
    }
    
    public byte[] GetDecodeBytes()
    {
        return encode ? Security.XXTEA.Decrypt(data, LuaDecodeKey) : data;
    }
}