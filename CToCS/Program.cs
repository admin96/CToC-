/*
 * FILE         : Program.cs
 * AUTHOR       : A. Saad Imran
 * FIRST VER.   : February 26, 2017
 * DESCRIPTION  :
 *  This file defines the main command line interface for
 * the C to C# program. This file grabs the input and 
 * output file names, opens the file for reading, parses
 * the content of the file and outputs the parsed content
 * to the specified output file. If no output file is 
 * specified, the program infers a class/file name based
 * on the input
 */

using System;

namespace CToCS
{
    class Program
    {
        /*
         * Main entry point for the command line program
         */
        static void Main(string[] args)
        {
            // Here is a variable to store our C-to-C# parser
            // The parser class provides an interface for doing this
            // C-to-C# parsing
            Parser parser;
            // Variable to keep track of whether the inputted arguments
            // are valid
            bool validArgs = false;
            // Variable to keep track of whether a file could be opened
            // based on the input arguments
            bool validInputFile = false;
            // Variable to keep track of whether the outputted file could
            // be written using the specified input arguments
            bool validOutputFile = false;
            // Variable to keep track of the number of arguments
            int inputArgLength = args.Length;
            // Variable to store the parsed contents of the output file 
            string convertedCS = null;
            // Variable to store the content of the input file
            string inputText = null;
            // Variable to store the output filename
            string outputFileName = null;
            if (inputArgLength > 1)
            {
                // If there is more than 1 argument and the first argument is "-i",
                // we'll look for a filename
                if (args[0] == "-i")
                {
                    // If there is a second argument, we'll use it as the filename
                    if (inputArgLength >= 2)
                    {
                        // Arguments are valid
                        validArgs = true;
                        try
                        {
                            // Read content of C file into the inputText variable
                            inputText = System.IO.File.ReadAllText(args[1]);
                            // The input file is valid
                            validInputFile = true;
                        }
                        // If the file wasn't found, let the user know
                        catch (System.IO.FileNotFoundException e)
                        {
                            Console.Write("File couldn't be found. ");
                            Console.Write(args[1] + " doesn't exist\n");
                        }
                        // If there was another error, let the user know that the file
                        // couldn't be opened
                        catch (Exception e)
                        {
                            Console.WriteLine("File (" + args[1] + ") couldn't be opened");
                        }
                        // If there are three or more arguments and the input file was valid,
                        // we'll look for an output filename in the command line arguments
                        if (inputArgLength >= 3 && validInputFile)
                        {
                            if (args[2] == "-o")
                            {
                                // If the third argument is "-o" and there are 4 arguments, 
                                // then the fourth argument is the output filename
                                if (inputArgLength == 4)
                                {
                                    outputFileName = args[3];
                                    // Try creating and writing content to the output file
                                    // specified in the input arguments and let the user 
                                    // know if the file couldn't be written
                                    try
                                    {
                                        using (System.IO.StreamWriter file =
                                            new System.IO.StreamWriter(outputFileName))
                                        {
                                            // Create parser
                                            parser = new Parser(inputText, args[1]);
                                            // Parse C contents of input file and convert them
                                            // to C#
                                            convertedCS = parser.Parse();
                                            // Write the new C# code to the specified output file
                                            file.Write(convertedCS);
                                            file.Close();
                                        }
                                        // The specified output file is valid if there were no errors
                                        validOutputFile = true;
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Couldn't write file");
                                    }
                                }
                            }
                        }
                    }
                    // If the output file wasn't valid and input file was, we'll output
                    // a file with a generic name ("Output.cs")
                    if (!validOutputFile && validInputFile)
                    {
                        outputFileName = "Output.cs";
                        try
                        {
                            using (System.IO.StreamWriter file =
                                new System.IO.StreamWriter(outputFileName))
                            {
                                parser = new Parser(inputText, args[1]);
                                convertedCS = parser.Parse();
                                file.Write(convertedCS);
                            }
                            validOutputFile = true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Couldn't write file");
                        }
                    }
                }
            }
            // If arguments weren't valid, let the user know how to use the program
            if (!validArgs)
            {
                Console.WriteLine("Usage: portwizard -i [filename] -o [filename]");
            }
        }
    }
}
