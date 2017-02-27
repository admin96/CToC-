/* File created by PortWizard */
/* 27-02-2017 --------------- */

using System;
using System.IO;

class FileOut
{
	static int Main(string[] args)
	{
		StreamWriter fp;
		fp = new StreamWriter ("testOutput.txt" );
		if (fp == null)
		{
			Console.Write("**Error: Unable to open file : testOutput.txt for writing\n");
			return 1;
		}
		fp.Write("hello world\n");
		fp.Close();
		Console.Write("File testOutput.txt has been created\n");
		return 0;
	}
}
