using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace PasswordSaver
{
    enum MD5MD5EncryptType
    {
        Key,
        Pwd
    }
   public class EncryptHelper
    {
        public static SymmetricKeyAlgorithmProvider Syprvd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesCbcPkcs7);
        private static IBuffer BuffIni = CryptographicBuffer.ConvertStringToBinary("45682547", BinaryStringEncoding.Utf8);
        public static string DESEncrypt(string pwd, string str)
        {
            IBuffer buffKey = CryptographicBuffer.ConvertStringToBinary(MD5Encrypt(pwd,MD5MD5EncryptType.Key), BinaryStringEncoding.Utf8);
            CryptographicKey myKey = Syprvd.CreateSymmetricKey(buffKey);
            IBuffer bufData = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            IBuffer buffLast = CryptographicEngine.Encrypt(myKey, bufData, BuffIni);
            return CryptographicBuffer.EncodeToHexString(buffLast);
        }

        public static string DESDecrypt(string pwd, string str)
        {
            IBuffer buffKey = CryptographicBuffer.ConvertStringToBinary(MD5Encrypt(pwd,MD5MD5EncryptType.Key), BinaryStringEncoding.Utf8);
            IBuffer cryptBuffer = CryptographicBuffer.DecodeFromHexString(str);
            CryptographicKey myKey = Syprvd.CreateSymmetricKey(buffKey);
            IBuffer decryptBuffer = CryptographicEngine.Decrypt(myKey, cryptBuffer, BuffIni);
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptBuffer);
        }

        private static string MD5Encrypt(string str, MD5MD5EncryptType mD5MD5EncryptType)
        {
            HashAlgorithmProvider hashprvd = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash objHash = hashprvd.CreateHash();
            IBuffer buffTemp = null;
            if (mD5MD5EncryptType==MD5MD5EncryptType.Key)
                buffTemp= CryptographicBuffer.ConvertStringToBinary(str + "高科,;;技的-撒ds客户ds如额我di", BinaryStringEncoding.Utf8);
            if(mD5MD5EncryptType == MD5MD5EncryptType.Pwd)
                buffTemp = CryptographicBuffer.ConvertStringToBinary(str + "高fdsfds科,;f;技fs的-a撒大ds客fd‘但是sfd打撒。打’撒户dff 大ss啊打如额f我di", BinaryStringEncoding.Utf8);
            objHash.Append(buffTemp);
            IBuffer buffLast = objHash.GetValueAndReset();
            return CryptographicBuffer.EncodeToHexString(buffLast);
        }
        
        public static string PwdEncrypt(string pwd)
        {
            return MD5Encrypt(pwd, MD5MD5EncryptType.Pwd);
        }

    }
}
