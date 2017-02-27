/* File created by PortWizard */
/* 27-02-2017 --------------- */

using System;
using System.IO;

class FileIn
{
	static int Main(string[] args)
	{
		int	fileLine = 1;
		StreamReader fp;
		string	inBuff;
		fp = new StreamReader ("test.txt" );
		if (fp == null)
		{
			Console.Write("**Error: Unable to open file : test.txt for reading\n");
			return 1;
		}
		Console.Write("The following are the lines read from file \"test.txt\"\n");
		while ((inBuff = fp.ReadLine()) != null)
		{
			Console.Write("{0:d2}) [Length : {1:d3}] {2:s}", fileLine, inBuff.Length, inBuff);
			fileLine++;
		}
		Console.Write("<<< EOF\n");
		fp.Close();
		return 0;
	}
}
