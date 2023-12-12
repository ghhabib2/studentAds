using System;
using System.Linq;
using System.Runtime.InteropServices;
using Guid = System.Guid;


namespace Classified.Services.Security
{
    public class ClassifiedTokenProvieser
    {
        #region Properties

        public static string NewToken
        {
            get
            {
                var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                var key = Guid.NewGuid().ToByteArray();
                return Base64Url.Encode(time.Concat(key).ToArray());
            }
        }

        /// <summary>
        /// Validate the Token To see if the record is Authorized or not
        /// </summary>
        /// <param name="token">Token as String</param>
        /// <param name="hoursToExpire">Hours To Expire</param>
        /// <returns>True if the token is valid and false if the token is not valid.</returns>
        public static bool ValidateToken(string token, int hoursToExpire)
        {
            var data = Base64Url.Decode(token);
            var when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            var datatoCompare= DateTime.UtcNow;
            var finalData=(datatoCompare - when).Hours;
            return (finalData <= hoursToExpire);
        }

        #endregion


    }

    /// <summary>
    /// Encode and Decode the 64 base strings in order to use them safely in URLs
    /// </summary>
    public static class Base64Url
    {
        /// <summary>
        /// Encoders
        /// </summary>
        /// <param name="input">64bit Array</param>
        /// <returns>64 base string</returns>
        public static string Encode(byte[] input)
        {
            var output = Convert.ToBase64String(input);

            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding

            return output;
        }

        /// <summary>
        /// Decode the previously encoded string
        /// </summary>
        /// <param name="input">64 base string</param>
        /// <returns>64 base array ready to use</returns>
        public static byte[] Decode(string input)
        {
            var output = input;

            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding

            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    output += "==";
                    break; // Two pad chars
                case 3:
                    output += "=";
                    break; // One pad char
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(input), "Illegal base64url string!");
            }

            var converted = Convert.FromBase64String(output); // Standard base64 decoder

            return converted;
        }
    }

   }
