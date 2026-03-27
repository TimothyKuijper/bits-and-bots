using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace Yakapedia
{
    public static class Encryption
    {
        /// <summary>
        /// Encrypts data into a byte[] using Rijndael.
        /// </summary>
        /// <param name="plain">Data to encrypt.</param>
        /// <param name="password">Password to use for encryption.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] plain, string password)
        {
            var rijndael = SetupRijndaelManaged;
            var deriveBytes = new Rfc2898DeriveBytes(password, 32);
            var bufferKey = deriveBytes.GetBytes(32);

            rijndael.Key = bufferKey;
            rijndael.GenerateIV();

            using ICryptoTransform encrypt = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);
            
            var dest = encrypt.TransformFinalBlock(plain, 0, plain.Length);
            var compile = new List<byte>(deriveBytes.Salt);
            
            compile.AddRange(rijndael.IV);
            compile.AddRange(dest);

            return compile.ToArray();
        }

        /// <summary>
        /// Decrypts data into a byte[] using Rijndael.
        /// </summary>
        /// <param name="encrypted">Data to decrypt.</param>
        /// <param name="password">Password to use for decryption.</param>
        /// <returns></returns>
        public static byte[] Decrypt(IEnumerable<byte> encrypted, string password)
        {
            var rijndael = SetupRijndaelManaged;
            var compile = encrypted.ToList();

            var salt = compile.GetRange(0, 32);
            var iv = compile.GetRange(32, 32);
            rijndael.IV = iv.ToArray();

            var deriveBytes = new Rfc2898DeriveBytes(password, salt.ToArray());
            var bufferKey = deriveBytes.GetBytes(32);
            rijndael.Key = bufferKey;
            var plain = compile.GetRange(32 * 2, compile.Count - (32 * 2)).ToArray();

            using ICryptoTransform decrypt = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);
            var dest = decrypt.TransformFinalBlock(plain, 0, plain.Length);

            return dest;
        }

        private static RijndaelManaged SetupRijndaelManaged
        {
            get
            {
                RijndaelManaged rijndael = new()
                {
                    BlockSize = 256,
                    KeySize = 256,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                return rijndael;
            }
        }
    }
}