/* File created by PortWizard */
/* 27-02-2017 --------------- */

using System;
using System.IO;

class Test
{
	static int Main(string[] args)
	{
		string buffer;
		int variable = 0;
		Console.Write("Hello world! \n");
		buffer = Console.ReadLine();
		variable = Int32.Parse(buffer);
		for (int i = 0; i < 5; i++)
		{
			Console.Write("{0:i}\n", i);
		}
		while (variable <= 5)
		{
			variable++;
			if (variable % 2 == 0)
			{
				Console.Write("{0:s} {1:d3}", "Hi", variable);
			}
		}
		return 0;
	}

	void someFunction(int arg, string arg2, float arg3)
	{
		int x = 0;
		int y = 5;
	}
}
