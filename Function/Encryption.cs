using System.Security.Cryptography;
using System;
using System.Text;

public class Encryption {
    public static string Encrypt(string unencrypt,string key)
    {
        //密钥
        byte[] keyArray = Encoding.UTF8.GetBytes(key);
        //待加密明文数组
        byte[] UnencryptArray = Encoding.UTF8.GetBytes(unencrypt);

        //Rijndael加密算法
        RijndaelManaged rDel = new RijndaelManaged
        {
            Key = keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        //返回加密后的密文
        byte[] resultArray = cTransform.TransformFinalBlock(UnencryptArray, 0, UnencryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    public static string Dencrypt(string encryted,string key)
    {
        //解密密钥
        byte[] keyArray = Encoding.UTF8.GetBytes(key);
        //待解密密文数组
        byte[] EncryptArray = Convert.FromBase64String(encryted);

        //Rijndael解密算法
        RijndaelManaged rDel = new RijndaelManaged
        {
            Key = keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        //返回解密后的明文
        byte[] resultArray = cTransform.TransformFinalBlock(EncryptArray, 0, EncryptArray.Length);
        return Encoding.UTF8.GetString(resultArray);
    }
}
