using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LoadBalancer
{
    internal static class Algoritme
    {
        public static object algoritme;
        public static int roundRobinPos = 0;
        public static List<string[]> requestPerServer = new List<string[]>();
        public static List<string[]> sessionsPerServer = new List<string[]>(); 

        /// <summary>
        /// Get's the current algorithm selected by the user
        /// </summary>
        /// <returns></returns>
        public static string Get()
        {
            return (string) algoritme;
        }

        /// <summary>
        /// Calculate a MD5 has of the give input
        /// </summary>
        /// <param name="input">A string that will be converted to MD5</param>
        /// <returns>MD5 String</returns>
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
