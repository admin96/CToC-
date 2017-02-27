/* File created by PortWizard */
/* 27-02-2017 --------------- */

using System;
using System.IO;

class Times
{
	static int Main(string[] args)
	{
		string buffer;
		int table;
		int x;
		int product;
		Console.Write("Specify which TIMES-TABLE you would like to generate: ");
		buffer = Console.ReadLine();
		table = Int32.Parse (buffer);
		for (x = 1; x <= 10; x++)
		{
			product = x * table;
			Console.Write("{0:d3} x {1:d3} = {2:d4}\n", x, table, product);
		}
		return 0;
	}
}
