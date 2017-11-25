using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Manager
{
	public class Formatter
	{
		//const int headerLenght = 54;

		/// <summary>
		/// Splits image bytes to the header and body
		/// </summary>
		public static void Decompose(byte[] bytePicture, out byte[] body)
		{
			
			body = new byte[bytePicture.Length];

		}

		/// <summary>
		///  Links the image header and body together into one array
		/// </summary>
		public static void Compose(byte[] body, int outputLenght, string outFile)
		{

            BinaryWriter writter = new BinaryWriter(File.OpenWrite(outFile));
            //  TextWriter writter = new StreamWriter(outFile);
            writter.Write(body);      // Writer raw data    
            writter.Flush();
            writter.Close();
		}
	}
}
