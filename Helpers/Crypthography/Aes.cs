using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Helpers.Cryptography
{
    class Aes
    {
        // Key
        private const string key = "prEH52eseSpawredRacrugafePhath4H";
        // Open a symmetric algorithm provider for the specified algorithm.
        private static SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
        // Symmetric key.
        private static CryptographicKey hashedKey = aes.CreateSymmetricKey(GetSHA256Hash(key));
        // Inicialization vector
        private static byte[] iv_bytes = { 201, 100, 171, 12, 44, 91, 73, 84, 2, 175, 177, 113, 222, 117, 131, 11 };
        private static IBuffer iv = CryptographicBuffer.CreateFromByteArray(iv_bytes);

        private static IBuffer GetSHA256Hash(string text)
        {
            // Put the text into a utf-8 encoded buffer
            IBuffer input = CryptographicBuffer.ConvertStringToBinary(text,
                BinaryStringEncoding.Utf8);

            // Hash input
            var sha256 = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            IBuffer hash = sha256.HashData(input);

            return hash;
        }

        /// <summary>
        /// Encrypts string.
        /// </summary>
        /// <param name="textToEncrypt">Text to encrypt.</param>
        /// <returns>Returns encrypted string.</returns>
        public static string Encrypt(string textToEncrypt)
        {
            // Create a buffer that contains the encoded message to be encrypted.
            var buffer = CryptographicBuffer.ConvertStringToBinary(textToEncrypt, BinaryStringEncoding.Utf8);

            // Encrypt
            var encryptedBuffer = CryptographicEngine.Encrypt(hashedKey, buffer, iv);

            // Convert the encrypted buffer to a string (for display).
            // We are using Base64 to convert bytes to string since you might get unmatched characters
            // in the encrypted buffer that we cannot convert to string with UTF8.
            var encryptedString = CryptographicBuffer.EncodeToBase64String(encryptedBuffer);

            return encryptedString;
        }

        /// <summary>
        /// Decrypts string.
        /// </summary>
        /// <param name="textToDecrypt">Text do decrypt.</param>
        /// <returns>Returns decrypted string.</returns>
        public static string Decrypt(string textToDecrypt)
        {
            // Create a buffer that contains the encoded message to be decrypted.
            IBuffer buffer = CryptographicBuffer.DecodeFromBase64String(textToDecrypt);

            //Decrypt
            var buffDecrypted = CryptographicEngine.Decrypt(hashedKey, buffer, iv);

            // Conver the decrypted buffer to a string
            string decryptedString = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffDecrypted);

            return decryptedString;
        }
    }
}
