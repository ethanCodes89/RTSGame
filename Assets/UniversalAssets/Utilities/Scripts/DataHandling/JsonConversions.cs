using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UniversalAssetsProject.Utilities
{
    public static class JsonConversions
    {
        static byte[] ivBytes = new byte[16];
        static byte[] keyBytes = new byte[16]; 
        public static void SerializeToJson(object state, string file)
        {
            string jsonString = JsonConvert.SerializeObject(state, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
#if UNITY_EDITOR
            File.WriteAllText(file, jsonString);
#else
            string encrypted = EncryptData(jsonString);
            File.WriteAllText(file, encrypted);
#endif
        }

        public static Dictionary<string, object> DeserializeToDictionary(string file)
        {
#if UNITY_EDITOR
            string json = File.ReadAllText(file);
#else
            string encrypted = File.ReadAllText(file);
            string json = DecryptAES(encrypted);
#endif
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        public static  Dictionary<string, object> DeserializeToDictionary(object _object)
        {
            var data = JsonConvert.SerializeObject(_object);

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
        }

        public static string EncryptData(string data)
        {
            GenerateIVBytes();
            GenerateKeyBytes();

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(keyBytes, ivBytes);
            byte[] inputBuffer = Encoding.Unicode.GetBytes(data);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string ivString = Encoding.Unicode.GetString(ivBytes);
            string encryptedString = Convert.ToBase64String(outputBuffer);

            return ivString + encryptedString;
        }

        public static string DecryptAES(this string text)
        {
            GenerateIVBytes();
            GenerateKeyBytes();

            int endOfIVBytes = ivBytes.Length / 2;
            string ivString = text.Substring(0, endOfIVBytes);
            byte[] extractedIvBytes = Encoding.Unicode.GetBytes(ivString);

            string encryptedString = text.Substring(endOfIVBytes);

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(keyBytes, extractedIvBytes);
            byte[] inputBuffer = Convert.FromBase64String(encryptedString);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            return Encoding.Unicode.GetString(outputBuffer);
        }

        public static void GenerateIVBytes()
        {
            System.Random rnd = new System.Random();
            rnd.NextBytes(ivBytes);
        }
        const string keyWord = "jljfanef89e9";
        public static void GenerateKeyBytes()
        {
            int sum = 0;
            foreach(char curChar in keyWord)
            {
                sum += curChar;
            }
            System.Random rnd = new System.Random(sum);
            rnd.NextBytes(keyBytes);
        }
    }
}
