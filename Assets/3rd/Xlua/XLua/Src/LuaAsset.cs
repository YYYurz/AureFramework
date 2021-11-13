using System;
using System.Security.Cryptography;
using System.Text;
using GameTest;
using UnityEngine;

[Serializable]
public class LuaAsset : ScriptableObject
{
    public static string LuaDecodeKey = "LuaDecodeKey"; //TODO: use a safe method to hide decode key
    public static string[] LuaSearchingPaths = new []{
        "lua/",
        "lua/utility/",
    };
    
    public bool encode = true;
    public byte[] data;
    
    public byte[] GetDecodeBytes()
    {
        byte[] decode = this.data;
        
        // TODO: your decode function
        decode = encode ? Security.XXTEA.Decrypt(this.data, LuaAsset.LuaDecodeKey) : this.data;
        
        return data;
    }
    
    public static byte[] Require(ref string luapath)
    {
        return Require(luapath);
    }

    public static byte[] Require(string luapath, string search = "", int retry = 0)
    {
        if(string.IsNullOrEmpty(luapath))
            return null;
            
        var LuaExtension = ".lua";

        if(luapath.EndsWith(LuaExtension))
        {
            luapath = luapath.Remove(luapath.LastIndexOf(LuaExtension));
        }

        byte[] bytes = null;
        var assetName = search + luapath.Replace(".", "/");
        {
            var asset = GameEntrance.Resource.LoadAssetSync<LuaAsset>(assetName);
            if (asset != null)
            {
                bytes = asset.GetDecodeBytes();
            }
        }

        // try next searching path
        if(bytes == null && retry < LuaSearchingPaths.Length)
        {
            bytes = Require(luapath, LuaSearchingPaths[retry], 1+retry);
        }

        var content = Encoding.UTF8.GetString(bytes);
        Debug.Assert(bytes != null, $"{luapath} not found.");
        return bytes;
    }
}