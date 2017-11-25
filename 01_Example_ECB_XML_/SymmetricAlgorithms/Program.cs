using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Manager;


namespace SymmetricAlgorithms
{
	public class Program
	{
		#region DES Alogirthm

	public	static void Test_DES_Encrypt(string inputFile, string outputFile, string secretKey)
		{
            DES_Symm_Algorithm.EncryptFile(inputFile, outputFile, secretKey);
		}

	public	static void Test_DES_Decrypt(string inputFile, string outputFile, string secretKey)
		{
            DES_Symm_Algorithm.DecryptFile(inputFile, outputFile, secretKey); // ovde mala izmena 
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

		public static void Start()
        {

            string myFileName = @"C:\Documents and Settings\user\Desktop\xxx.txt";

            string cipherFile = "Ciphered.xml";         //result of encryption
            string plaintextFile = "Plainxml.xml";      //result of decryption
            string keyFile = "SecretKey.txt";

            string secretKey = SecretKey.GenerateKey(AlgorithmType.DES);
            SecretKey.StoreKey(secretKey, keyFile);
            string eSecretKey = SecretKey.LoadKey(keyFile);

            Test_DES_Decrypt(cipherFile, plaintextFile, eSecretKey);


        }
		static void Main(string[] args)
		{
			/*string xmlFile = "Baza.xml";				//source bitmap file
			string cipherFile = "Ciphered.xml";			//result of encryption
			string plaintextFile = "Plainxml.xml";		//result of decryption
			string keyFile = "SecretKey.txt";			//secret key storage

			Console.WriteLine("Symmetric Encryption Example - ECB mode");

            ///Generate secret key for appropriate symmetric algorithm and store it to 'keyFile' for further usage
            string secretKey = SecretKey.GenerateKey(AlgorithmType.DES);
            SecretKey.StoreKey(secretKey, keyFile);
            string eSecretKey = SecretKey.LoadKey(keyFile);

            Test_DES_Encrypt(xmlFile, cipherFile, eSecretKey);
			///Test_AES_Encrypt(imgFile, cipherFile, eSecretKey);
			///Test_3DES_Encrypt(imgFile, cipherFile, eSecretKey);
			Console.WriteLine("Encryption is done.");
			Console.ReadLine();

			Test_DES_Decrypt(cipherFile, plaintextFile, eSecretKey);
			///Test_AES_Decrypt(cipherFile, plaintextFile, SecretKey.LoadKey(keyFile));
			///Test_3DES_Decrypt(cipherFile, plaintextFile, SecretKey.LoadKey(keyFile));
			Console.WriteLine("Decryption is done.");*/
			Console.ReadLine();
		}
	}
}
