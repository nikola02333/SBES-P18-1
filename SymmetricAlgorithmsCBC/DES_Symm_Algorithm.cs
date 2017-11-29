using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;
using Manager;

namespace SymmetricAlgorithmsCBC
{
	public class DES_Symm_Algorithm
	{
		/// <summary>
		/// Function that encrypts the plaintext from inFile and stores cipher text to outFile
		/// </summary>
		/// <param name="inFile"> filepath where plaintext is stored </param>
		/// <param name="outFile"> filepath where cipher text is expected to be stored </param>
		/// <param name="secretKey"> symmetric encryption key </param>
		public static void EncryptFile(string inFile, string outFile, string secretKey)
		{
            
            byte[] xmlfile = File.ReadAllBytes(inFile);
            DESCryptoServiceProvider desCrypto = new DESCryptoServiceProvider();
            
            desCrypto.Mode = CipherMode.CBC; //
            desCrypto.Padding = PaddingMode.Zeros; 


            desCrypto.Key = Encoding.ASCII.GetBytes(secretKey);

            desCrypto.GenerateIV();		// use generated IV - IV lenght is 'block_size/8'

			ICryptoTransform desEncrypt = desCrypto.CreateEncryptor();
            /// CryptoStream cryptoStream

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
            desEncrypt, CryptoStreamMode.Write);
           
            cryptoStream.Write(xmlfile, 0, xmlfile.Length); 
            cryptoStream.Flush();
            cryptoStream.FlushFinalBlock();

            Formatter.Compose(memoryStream.ToArray(), xmlfile.Length, outFile);
        }


		/// <summary>
		/// Function that decrypts the cipher text from inFile and stores as plaintext to outFile
		/// </summary>
		/// <param name="inFile"> filepath where cipher text is stored </param>
		/// <param name="outFile"> filepath where plain text is expected to be stored </param>
		/// <param name="secretKey"> symmetric encryption key </param>
		public static void DecryptFile(string inFile, string outFile, string secretKey)
		{
            byte[] xmlfile = File.ReadAllBytes(inFile);
            DESCryptoServiceProvider desCrypto = new DESCryptoServiceProvider();
			/// desCrypto.IV -> take the IV off the beginning of the ciphertext message			
            desCrypto.Mode = CipherMode.CBC; 
            desCrypto.Padding = PaddingMode.Zeros; 
            desCrypto.Key = Encoding.ASCII.GetBytes(secretKey);

            for (int i = 0; i < 8; i++)
            {
                desCrypto.IV[i] = xmlfile[i];
            }

            int CrytoMessageLength = xmlfile.Length - 8;

            byte[] CrytoMessage = new byte[CrytoMessageLength];
            int counter = 0;
            for (int i = 8; i < xmlfile.Length; i++)
            {
                CrytoMessage[counter] = xmlfile[i];
                counter++;
            }

             MemoryStream memoryStream = new MemoryStream(CrytoMessage);
            ICryptoTransform desDecrypt = desCrypto.CreateDecryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, desDecrypt, CryptoStreamMode.Write);
            cryptoStream.Write(CrytoMessage, 0, memoryStream.ToArray().Length);
           
          Formatter.Compose(CrytoMessage, CrytoMessage.Length,outFile);	
        }
    }
}
