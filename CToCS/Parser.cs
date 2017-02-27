/*
 * FILE         : Parser.cs
 * AUTHOR       : A. Saad Imran
 * FIRST VER.   : February 26, 2017
 * DESCRIPTION  :
 *  The Parser class provides a clean and simple interface
 * for translating C code into C#
 */

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CToCS
{
    public class Parser
    {
        // Tokenizer to tokenize the input text
        private Tokenizer tokenizer;
        // Variable to store the output C#
        private string output;
        // Filename of the inputted C file. This is used as a class name
        // for the C# class
        private string filename;
        // We'll keep track of the "block level" for indentation purposes.
        // Each time we open a block (like this -> {}), this variable is
        // incremented. Then, we add this number of "\t" (tabs) before 
        // outputting the C# statement
        private int blockLevel = 0;
        public Parser(string inputText, string filename)
        {
            // Replace the .c for the class name
            filename = filename.Replace(".c", "");
            // Remove any non-alphabetic characters
            filename = Regex.Replace(filename, "[^a-zA-Z]*", "");
            // and capitalize first letter
            this.filename = FirstCharToUpper(filename);
            // Replace all occurences of "NULL" to the C# friendly "null"
            inputText = Regex.Replace(inputText, "NULL", "null");
            // Create tokenizer with input text
            tokenizer = new Tokenizer(inputText);
        }

        /*
         * METHOD       : FirstCharToUpper()
         * DESCRIPTION  : Parses the inputted C code into C# code
         * PARAMETERS   :
         *  string input - the string which needs its first letter
         *      capitalized
         * RETURNS      :
         *  string - inputted string with first letter capitalized
         */
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        /*
         * METHOD       : Parse()
         * DESCRIPTION  : Parses the inputted C code into C# code
         * RETURNS      :
         *  string - Parsed C# code
         */
        public string Parse()
        {
            // Create header comment
            output = "/* File created by PortWizard */\n";
            output += "/* " + DateTime.Today.ToString("dd/MM/yyyy") + " --------------- */\n\n";
            // Create "using" statements
            output += "using System;\nusing System.IO;\n\n";
            // Declare class
            output += "class " + filename + "\n{\n";
            // Increment blockLevel
            blockLevel = 1;
            // Parse each token as C#
            foreach (Token token in tokenizer.GetLines())
            {
                // If token type isn't "NONE", or in other words, if we're
                // dealing with a valid token, we'll parse the output
                String tokenType = token.GetTokenType().ToString();
                if (tokenType != "NONE")
                {
                    // If we're ending a block, we want to decrement "blockLevel"
                    if (tokenType == "END_BLOCK")
                    {
                        blockLevel--;
                    }
                    if (tokenType == "METHOD")
                    {
                        output += "\n";
                    }
                    // Add appropriate indentation
                    for (int i = 0; i < blockLevel; i++)
                    {
                        output += "\t";
                    }
                    // Parse token and append to output
                    output += token.Parse() + "\n";
                    // If we're starting a new block, increment "blockLevel" for 
                    // indentation
                    if (tokenType == "START_BLOCK")
                    {
                        blockLevel++;
                    }
                }
            }
            // End class declaration and return parsed C# output from function
            output += "}\n";
            return output;
        }
    }
}
