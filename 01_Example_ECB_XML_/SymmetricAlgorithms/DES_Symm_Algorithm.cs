using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;
using Manager;
// using KeyManagement;

namespace SymmetricAlgorithms
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

            desCrypto.Mode = CipherMode.ECB; //
            desCrypto.Padding = PaddingMode.Zeros; // dopuni nulama ili cime oces 
            desCrypto.Key = Encoding.ASCII.GetBytes(secretKey);


            ICryptoTransform desEncrypt = desCrypto.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            
            // nad tim strimom  , smesta u memoryStream
            CryptoStream cryptoStream = new CryptoStream(memoryStream,  
        desEncrypt, CryptoStreamMode.Write);

            cryptoStream.Write(xmlfile, 0, xmlfile.Length); // moj kstrim upisati 
          //cryptoStream.Write(header, 0, header.Length);  
            cryptoStream.FlushFinalBlock();

            // u memoristrim se nalazi
            //u body se nalazi 
            int length = xmlfile.Length;

            Formatter.Compose(memoryStream.ToArray(), length,outFile);
        }


		/// <summary>
		/// Function that decrypts the cipher text from inFile and stores as plaintext to outFile
		/// </summary>
		/// <param name="inFile"> filepath where cipher text is stored </param>
		/// <param name="outFile"> filepath where plain text is expected to be stored </param>
		/// <param name="secretKey"> symmetric encryption key </param>
		public static void DecryptFile(string inFile, string outFile, string secretKey)
		{
			byte[] header = null;		//image header (54 byte) should not be decrypted
			byte[] body = null;			//image body to be decrypted

            byte[] xmlfile = File.ReadAllBytes(inFile); 
			 //Formatter.Decompose(xmlfile, out body );			
			
			DESCryptoServiceProvider desCrpto = new DESCryptoServiceProvider();

            desCrpto.Mode = CipherMode.ECB; //
            desCrpto.Padding = PaddingMode.Zeros; // 
            desCrpto.Key = Encoding.ASCII.GetBytes(secretKey);


            MemoryStream memoryStream = new MemoryStream(xmlfile);

             ICryptoTransform desDecrpt = desCrpto.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, desDecrpt, CryptoStreamMode.Write);

            cryptoStream.Write(xmlfile, 0 , memoryStream.ToArray().Length);

            //int lenght = xmlfile.Length;
           // Formatter.Compose(xmlfile, lenght, outFile);

            /// desCrypto.Padding = PaddingMode.None;

            /// ICryptoTransform desDecrypt = desCrypto.CreateDecryptor();
            /// CryptoStream cryptoStream

            /// output = header + decrypted_body
             Formatter.Compose(xmlfile,xmlfile.Length,outFile);					
        }
	}
}
