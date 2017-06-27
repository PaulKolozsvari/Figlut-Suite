namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
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

        private const string INITIALIZATION_VECTOR_DEFAULT_VALUE = "FiglutInitVector";

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

        internal string Encrypt(string decryptedString)
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

        internal string Decrypt(string encryptedString)
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

        #endregion //Methods
    }
}
