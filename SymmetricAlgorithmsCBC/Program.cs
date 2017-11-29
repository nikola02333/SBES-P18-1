using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Manager;


namespace SymmetricAlgorithmsCBC
{
	public class Program
	{
		#region DES Alogirthm

		static void Test_DES_Encrypt(string inputFile, string outputFile, string secretKey)
		{
            DES_Symm_Algorithm.EncryptFile(inputFile, outputFile, secretKey);
        }

		static void Test_DES_Decrypt(string inputFile, string outputFile, string secretKey)
		{
            DES_Symm_Algorithm.DecryptFile(inputFile, outputFile, secretKey);
        }

		#endregion

		#region 3DES Alogirthm

		static void Test_3DES_Encrypt(string inputFile, string outputFile, string secretKey)
		{
			///Perform 3DES encryption
		}

		static void Test_3DES_Decrypt(string inputFile, string outputFile, string secretKey)
		{
			///Perform 3DES decryption
		}

		#endregion

		#region AES Alogirthm

		static void Test_AES_Encrypt(string inputFile, string outputFile, string secretKey)
		{
			///Perform AES encryption
		}

		static void Test_AES_Decrypt(string inputFile, string outputFile, string secretKey)
		{
			///Perform AES decryption
		}

		#endregion

        public static void StartCBC()
        {
            string imgFile = "Baza.xml";                //source bitmap file
            string cipherFile = "Ciphered_CBC.xml";         //result of encryption
            string plaintextFile = "Plaintext_CBC.xml";     //result of decryption
            string keyFile = "SecretKey_CBC.txt";           //secret key storage

            Console.WriteLine("Symmetric Encryption Example - CBC mode");

            string secretKey = SecretKey.GenerateKey(AlgorithmType.DES);
            SecretKey.StoreKey(secretKey, keyFile);
            string eSecretKey = SecretKey.LoadKey(keyFile);


            Test_DES_Encrypt(imgFile, cipherFile, eSecretKey);

            Console.WriteLine(" CBC Encryption is done.");
            Console.ReadLine();

            Test_DES_Decrypt(cipherFile, plaintextFile, eSecretKey);

            Console.WriteLine(" CBC Decryption is done.");
        }

		static void Main(string[] args)
		{
            /*string imgFile = "Baza.xml";				//source bitmap file
			 string cipherFile = "Ciphered.xml";			//result of encryption
			 string plaintextFile = "Plaintext.xml";		//result of decryption
			string keyFile = "SecretKey.txt";			//secret key storage

			Console.WriteLine("Symmetric Encryption Example - CBC mode");

            string secretKey = SecretKey.GenerateKey(AlgorithmType.DES);
            SecretKey.StoreKey(secretKey, keyFile);
            string eSecretKey = SecretKey.LoadKey(keyFile);


            Test_DES_Encrypt(imgFile, cipherFile, eSecretKey);
			
			Console.WriteLine(" CBC Encryption is done.");
			Console.ReadLine();

			Test_DES_Decrypt(cipherFile, plaintextFile, eSecretKey);

			Console.WriteLine(" CBC Decryption is done.");*/
            StartCBC();

            Console.ReadLine();
		}
	}
}
