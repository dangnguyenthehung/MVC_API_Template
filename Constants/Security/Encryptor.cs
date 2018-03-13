using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Constants.Security
{
    public class Encryptor
    {
        //MD5
        public static string EncryptMD5(string content)
        {
            // Tham khảo về MD5: https://msdn.microsoft.com/en-us/library/system.security.cryptography.md5(v=vs.110).aspx
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(content));
                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();
                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        //sha-1 
        public static string EncryptSHA1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
        //sha-256 
        public static string EncryptSHA256(string input)
        {
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
        //sha-384 
        public static string EncryptSHA384(string input)
        {
            using (SHA384Managed sha384 = new SHA384Managed())
            {
                var hash = sha384.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
        //sha-512 
        public static string EncryptSHA512(string input)
        {
            using (SHA512Managed sha512 = new SHA512Managed())
            {
                var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        //AES
        public class AESdecrypt
        {
            private byte[] EncKey;
            private byte[] IVKey;

            public AESdecrypt()
            {
                var AES = new ByteArray.AESKMain();
                EncKey = AES.GetKey();
                IVKey = AES.GetIVKey();
            }

            public string DecryptString_Aes(string originalStr)
            {
                byte[] Key = EncKey;
                byte[] IV = IVKey;

                // 
                byte[] cipherText = StrToByteArray(originalStr);
                // Check arguments.
                if (cipherText == null || cipherText.Length <= 0)
                {
                    //throw new ArgumentNullException("cipherText");
                    return string.Empty;
                }

                if (Key == null || Key.Length <= 0)
                {
                    //throw new ArgumentNullException("Key");
                    return string.Empty;
                }

                if (IV == null || IV.Length <= 0)
                {
                    //throw new ArgumentNullException("IV");
                    return string.Empty;
                }

                // Declare the string used to hold
                // the decrypted text.
                string plaintext = null;

                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }

                return plaintext;

            }

            /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
            //      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            //      return encoding.GetBytes(str);
            // However, this results in character values that cannot be passed in a URL.  So, instead, I just
            // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
            private byte[] StrToByteArray(string str)
            {
                try
                {
                    if (str.Length == 0)
                        throw new Exception("Invalid string value in StrToByteArray");

                    byte val;
                    byte[] byteArr = new byte[str.Length / 3];
                    int i = 0;
                    int j = 0;
                    do
                    {
                        val = byte.Parse(str.Substring(i, 3));
                        byteArr[j++] = val;
                        i += 3;
                    }
                    while (i < str.Length);
                    return byteArr;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public class AESencrypt
        {
            private byte[] EncKey;
            private byte[] IVKey;

            public AESencrypt()
            {
                var AES = new ByteArray.AESKMain();
                EncKey = AES.GetKey();
                IVKey = AES.GetIVKey();
            }

            public string EncryptString_Aes(string plainText)
            {
                byte[] Key = EncKey;
                byte[] IV = IVKey;
                // Check arguments.
                if (plainText == null || plainText.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");
                byte[] encrypted;
                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {

                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                // Return the encrypted bytes from the memory stream.
                return ByteArrToString(encrypted);
            }

            // Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
            //      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            //      return enc.GetString(byteArr);    
            private string ByteArrToString(byte[] byteArr)
            {
                byte val;
                string tempStr = "";
                for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
                {
                    val = byteArr[i];
                    if (val < (byte)10)
                        tempStr += "00" + val.ToString();
                    else if (val < (byte)100)
                        tempStr += "0" + val.ToString();
                    else
                        tempStr += val.ToString();
                }
                return tempStr;
            }

        }
    }
}
