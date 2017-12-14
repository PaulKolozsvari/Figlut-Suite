namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;

    #endregion //Using Directives

    public class Encryptor
    {
        #region Constructors

        public Encryptor(string keyGeneratorPassword)
        {
            if (string.IsNullOrEmpty(keyGeneratorPassword))
            {
                if (string.IsNullOrEmpty(keyGeneratorPassword))
                {
                    throw new NullReferenceException(string.Format(
                        "keyGeneratorPassword may not be null when constructing a {0}.",
                        this.GetType().FullName));
                }
            }
            _keyGeneratorPassword = keyGeneratorPassword;
            _initializationVector = INITIALIZATION_VECTOR_DEFAULT_VALUE;
        }

        public Encryptor(string keyGeneratorPassword, string initializationVector)
        {
            if (string.IsNullOrEmpty(keyGeneratorPassword))
            {
                throw new NullReferenceException(string.Format(
                    "keyGeneratorPassword may not be null when constructing a {0}.",
                    this.GetType().FullName));
            }
            if (string.IsNullOrEmpty(initializationVector))
            {
                throw new NullReferenceException(string.Format(
                    "initializationVector may not be null when constructing a {0}.",
                    this.GetType().FullName));
            }
            _keyGeneratorPassword = keyGeneratorPassword;
            _initializationVector = initializationVector;
        }

        #endregion //Constructors

        #region Constants

        private const string INITIALIZATION_VECTOR_DEFAULT_VALUE = "FiglutIV";

        #endregion //Constants

        #region Fields

        private string _keyGeneratorPassword;
        private string _initializationVector;

        #endregion //Fields

        #region Properties

        private byte[] StringToByteArray(string value)
        {
            byte[] result = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                result[i] = (byte)value[i];
            }
            return result;
        }

        private string ByteArrayToString(byte[] value)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte b in value)
            {
                result.Append(Convert.ToChar(b));
            }
            return result.ToString();
        }

        #endregion //Properties

        #region Methods

        public string Encrypt(string decryptedString)
        {
            PasswordDeriveBytes keyGenerator = new PasswordDeriveBytes(_keyGeneratorPassword, null);
            byte[] cryptoKey = keyGenerator.CryptDeriveKey("RC2", "SHA1", 128, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            using (RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider())
            {
                using (ICryptoTransform encryptor = rc2.CreateEncryptor(cryptoKey, StringToByteArray(_initializationVector)))
                {
                    byte[] decryptedBytes = StringToByteArray(decryptedString);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(StringToByteArray(decryptedString), 0, decryptedBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public string Decrypt(string encryptedString)
        {
            PasswordDeriveBytes keyGenerator = new PasswordDeriveBytes(_keyGeneratorPassword, null);
            byte[] cryptoKey = keyGenerator.CryptDeriveKey("RC2", "SHA1", 128, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            using (RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider())
            {
                using (ICryptoTransform decryptor = rc2.CreateDecryptor(cryptoKey, StringToByteArray(_initializationVector)))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return ByteArrayToString(decryptedBytes);
                }
            }
        }

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

        /// <summary>
        /// Return unique Int64 value for input string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Int64 GetInt64HashCode(string text)
        {
            Int64 hashCode = 0;
            if (!string.IsNullOrEmpty(text))
            {
                //Unicode Encode Covering all characterset
                byte[] byteContents = Encoding.Unicode.GetBytes(text);
                System.Security.Cryptography.SHA256 hash =
                new System.Security.Cryptography.SHA256CryptoServiceProvider();
                byte[] hashText = hash.ComputeHash(byteContents);
                //32Byte hashText separate
                //hashCodeStart = 0~7  8Byte
                //hashCodeMedium = 8~23  8Byte
                //hashCodeEnd = 24~31  8Byte
                //and Fold
                Int64 hashCodeStart = BitConverter.ToInt64(hashText, 0);
                Int64 hashCodeMedium = BitConverter.ToInt64(hashText, 8);
                Int64 hashCodeEnd = BitConverter.ToInt64(hashText, 24);
                hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
            }
            return (hashCode);
        }

        #endregion //Methods
    }
}
