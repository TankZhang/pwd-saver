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
   public class DESHelper
    {
        public static SymmetricKeyAlgorithmProvider Syprvd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesCbcPkcs7);
        private static IBuffer BuffIni = CryptographicBuffer.ConvertStringToBinary("45682547", BinaryStringEncoding.Utf8);
        public static string DESEncrypt(string key, string str)
        {
            IBuffer buffKey = CryptographicBuffer.ConvertStringToBinary(MD5Encrypt(key), BinaryStringEncoding.Utf8);
            CryptographicKey myKey = Syprvd.CreateSymmetricKey(buffKey);
            IBuffer bufData = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            IBuffer buffLast = CryptographicEngine.Encrypt(myKey, bufData, BuffIni);
            return CryptographicBuffer.EncodeToHexString(buffLast);
        }

        public static string DESDecrypt(string key, string str)
        {
            IBuffer buffKey = CryptographicBuffer.ConvertStringToBinary(MD5Encrypt(key), BinaryStringEncoding.Utf8);
            IBuffer cryptBuffer = CryptographicBuffer.DecodeFromHexString(str);
            CryptographicKey myKey = Syprvd.CreateSymmetricKey(buffKey);
            IBuffer decryptBuffer = CryptographicEngine.Decrypt(myKey, cryptBuffer, BuffIni);
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptBuffer);
        }

        private static string MD5Encrypt(string str)
        {
            HashAlgorithmProvider hashprvd = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash objHash = hashprvd.CreateHash();
            IBuffer buffTemp = CryptographicBuffer.ConvertStringToBinary(str + "高科,;;技的-撒ds客户ds如额我di", BinaryStringEncoding.Utf8);
            objHash.Append(buffTemp);
            IBuffer buffLast = objHash.GetValueAndReset();
            return CryptographicBuffer.EncodeToHexString(buffLast);

        }
    }
}
