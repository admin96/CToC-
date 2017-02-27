/*
 * FILE         : Token.cs
 * AUTHOR       : A. Saad Imran
 * FIRST VER.   : February 26, 2017
 * DESCRIPTION  :
 *  This class models the functionality of a single a single
 * token. The main parsing functionality is handled by this class.
 * The Tokenizer creates individual tokens from the contents of
 * the input file. The parser then goes through each token in 
 * sequential order and parses the token to create the final
 * C# output. All this magic happens because of the Parse()
 * method in the Token class. 
 */
 
using System.Text.RegularExpressions;

namespace CToCS
{
    public class Token
    {
        // Our token as a string
        private string token;
        // The type of the token
        private TokenType type;
        // The parent tokenizer class
        private Tokenizer tokenizer;

        // Bunch of constructor declarations ------------------------/
        public Token(string token, Tokenizer tokenizer)
        {
            this.token = token;
            this.tokenizer = tokenizer;
        }

        public Token(string token, TokenType type)
        {
            this.token = token;
            this.type = type;
        }

        public Token(string token, TokenType type, Tokenizer tokenizer)
        {
            this.token = token;
            this.type = type;
            this.tokenizer = tokenizer;
        }

        // End of constructor declarations ----------------------------/

        /*
         * METHOD       : GetToken()
         * DESCRIPTION  : Gets token
         * RETURNS      :
         *  string - token
         */
        public string GetToken()
        {
            return token;
        }

        /*
         * METHOD       : GetTokenType()
         * DESCRIPTION  : Gets token type
         * RETURNS      :
         *  TokenType - token type
         */
        public TokenType GetTokenType()
        {
            return type;
        }

        /*
         * METHOD       : Parse()
         * DESCRIPTION  : 
         *  Parses the token and translates the token from C to C#
         * RETURNS      :
         *  string - token parsed as C# string
         */
        public string Parse()
        {
            // Variable to store the output we'll return from this method
            string output = null;
            // The type of the token. We'll use this for parsing the token 
            // appropriately
            string tokenType = type.ToString();
            // If this token is an fprintf statement, we'll grab the arguments from the
            // fprintf call. Then we'll assume that the first argument points to a 
            // StreamWriter object and we'll call the Write method using it. 
            // The specified arguments will be passed to the Write method of the 
            // StreamWriter object
            if (tokenType == "FPRINTF")
            {
                Match argsMatch = Regex.Match(token, "\\([\x20-\x7E\\s]*\\)");
                string argStr = Regex.Replace(argsMatch.Value, "^\\(|\\)$", "");
                string[] args = argStr.Split(',');
                output += args[0] + ".Write(" + args[1].Trim() + ");";                
            }
            // If this token is an fopen statement, we know a file is being opened for
            // reading or writing
            else if (tokenType == "FOPEN")
            {
                string parsed = null;
                // If the fopen call contains the "r" parameter, we're reading a file.
                // So, we'll declare a StreamReader object 
                if (token.Contains("\"r\""))
                {
                    parsed = Regex.Replace(token, "\"r\"|[,]*", "");
                    parsed = Regex.Replace(parsed, "fopen", "new StreamReader");
                }
                // If the fopen call contains the "w" parameter, we're writing to a file.
                // So, we'll declare a StreamWriter object 
                else if (token.Contains("\"w\""))
                {
                    parsed = Regex.Replace(token, "\"w\"|[,]*", "");
                    parsed = Regex.Replace(parsed, "fopen", "new StreamWriter");
                }
                output += parsed;
            }
            // If this token is a printf statement, we know we're printing to the console
            // using Console.Write()
            else if (tokenType == "PRINTF")
            {
                output += "Console.Write(";
                // Grab string which is being printed to console
                Match match = Regex.Match(token, "\x22[\x20-\x7E]+\x22");
                if (match.Success)
                {
                    string outStr = match.Value;
                    int formatStrVal = 0;
                    // Convert "%s", "%[num]d" and "%[num]i" to their C# counterparts
                    foreach (Match item in Regex.Matches(match.Value, "%[0-9]*[dis]"))
                    {
                        string formatInt = Regex.Match(item.Value, "[0-9]+").Value;
                        string formatType = Regex.Match(item.Value, "[dis]").Value;
                        string formatStr = "{" + formatStrVal + ":" + formatType + formatInt + "}";
                        formatStrVal++;
                        outStr = new Regex(item.Value).Replace(outStr, formatStr, 1);
                    }
                    output += outStr;
                }
                // Grab other arguments (besides string literal) and append them to the Console.Write() call
                Match argsMatch = Regex.Match(token, "\\([\x20-\x7E\\s]*\\)");
                string argStr = Regex.Replace(argsMatch.Value, "\x22[\x20-\x7E]+\x22|^\\(|\\)$", "");
                string[] args = argStr.Split(',');
                string argumentOutput = "";
                // If the additional arguments (besides string literal) contain a function call,
                // parse that appropriately
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = args[i].Trim();
                    if (args[i].Contains("strlen"))
                    {
                        Token strlen = new Token(args[i], TokenType.STRLEN, tokenizer);
                        argumentOutput += ", " + strlen.Parse();
                    }
                    else if (args[i].Contains("atoi"))
                    {
                        Token atoi = new Token(args[i], TokenType.ATOI, tokenizer);
                        argumentOutput += ", " + atoi.Parse();
                    }
                    else
                    {
                        if (args[i] != "" && args[i] != null)
                        {
                            argumentOutput += ", " + args[i];
                        }
                    }
                }
                output += argumentOutput;
                output += ");";
            }
            // If the token type is strlenm we'll remove "strlen" and brackets from the method
            // call and append ".Length" at the end
            else if (tokenType == "STRLEN")
            {
                token = Regex.Replace(token, "strlen|\\(|\\)", "");
                output = token + ".Length";
            }
            // If the token type is fclose, we know a StreamWriter or StreamReader object is being closed
            // So, we'll remove "fclose" and brackets from the method call and append ".Close();" at the 
            // end
            else if (tokenType == "FCLOSE")
            {
                token = Regex.Replace(token, "fclose|\\(|\\)", "").Trim().Replace(";", "");
                output = token + ".Close();";
            }
            // If this is a generic token type, or in other words, if we're assigning or altering a variable
            // declared elsewhere, we'll just output the token untouched
            else if (tokenType == "TOKEN")
            {
                output = token;
            }
            // If a file variable is being declared, we'll grab the file name from the FILE* c variable,
            // and depending on whether the fopen call for the variable is using a "r" or "w" parameter, 
            // we'll use a StreamWriter or a StreamReader object accordingly
            else if (tokenType == "FILE_VAR")
            {
                string filename = Regex.Replace(token, "FILE|\\*|[\\s]*|\\;", "").Trim();
                string outStr = "";
                foreach (Token line in tokenizer.GetLines())
                {
                    if (Regex.IsMatch(line.GetToken(), filename + "|fopen"))
                    {
                        if (line.GetToken().Contains("\"r\""))
                        {
                            outStr = "StreamReader ";
                        }
                        else if (line.GetToken().Contains("\"w\""))
                        {
                            outStr = "StreamWriter ";
                        }
                    }
                }
                output = outStr + filename;
            }
            // If the main method is being declared, we'll parse out the C# equivalent
            else if (tokenType == "MAIN_METHOD")
            {
                output += "static ";
                if (token.Contains("int"))
                {
                    output += "int ";
                }
                else
                {
                    output += "void ";
                }
                output += "Main(string[] args)";
                foreach (string subToken in token.Split(' '))
                {
                    if (subToken == "{")
                    {
                        output += "{";
                    }
                }
            }
            // If a gets() function call is being made, we'll take the argument from the gets call
            // and assign a "Console.ReadLine()" to it
            else if (tokenType == "GETS")
            {
                string outStr = token.Replace("gets", "");
                outStr = outStr.Replace("(", "");
                outStr = outStr.Replace(")", "");
                outStr = outStr.Replace(";", "");
                outStr = Regex.Replace(outStr, "[\\s]*", "");
                outStr += " = Console.ReadLine();";
                output += outStr;
            }
            // If an atoi function call is being made, we'll replace atoi with "Int32.Parse"
            else if (tokenType == "ATOI")
            {
                output += token.Replace("atoi", "Int32.Parse");
            }
            // If this token is creating a for loop or an if-else statement, we'll output
            // it untouched
            else if (tokenType == "FOR")
            {
                output += token;
            }
            else if (tokenType == "IF_ELSE")
            {
                output += token;
            }
            // If the token is declaring a while loop, we'll parse out any function calls within the
            // while loop appropriately. Outside of translating any function calls used in the while
            // loop, the output is parsed without altering the C source
            else if (tokenType == "WHILE")
            {
                if (token.Contains("strlen"))
                {
                    Match argsMatch = Regex.Match(token, "strlen[\\s]*\\([a-zA-Z0-9,\\s]*\\)");
                    Token strlen = new Token(argsMatch.Value, TokenType.STRLEN);
                    string parsedstrlen = strlen.Parse();
                    parsedstrlen = parsedstrlen.Replace(";", "");
                    token = token.Replace(argsMatch.Value, "(" + parsedstrlen + ")");
                }
                else if (token.Contains("fgets"))
                {
                    Match argsMatch = Regex.Match(token, "fgets[\\s]*\\([a-zA-Z0-9,\\s]*\\)");
                    Token fgets = new Token(argsMatch.Value, TokenType.FGETS);
                    string parsedfgets = fgets.Parse();
                    parsedfgets = parsedfgets.Replace(";", "");
                    token = token.Replace(argsMatch.Value, "(" + parsedfgets + ")");
                }
                else if (token.Contains("atoi"))
                {
                    Match argsMatch = Regex.Match(token, "atoi[\\s]*\\([a-zA-Z0-9,\\s]*\\)");
                    Token atoi = new Token(argsMatch.Value, TokenType.ATOI);
                    string parsedatoi = atoi.Parse();
                    parsedatoi = parsedatoi.Replace(";", "");
                    token = token.Replace(argsMatch.Value, "(" + parsedatoi + ")");
                }
                output += token;
            }
            // If the token is making an fgets call, we'll take the first parameter from the fgets call
            // and assign a value to it from a line read from the file specified in the third parameter
            else if (tokenType == "FGETS")
            {
                Match argsMatch = Regex.Match(token, "\\([a-zA-Z0-9,\\s]*\\)");
                string args = argsMatch.Value;
                args = args.Replace("(", "");
                args = args.Replace(")", "");
                string[] argArr = args.Split(',');
                string outStr = argArr[0] + " = " + argArr[2].Trim() + ".ReadLine();";
                output += outStr;
            }
            // If the token is declaring a method, we'll change all occurences of "char*" or "char var[num]" to string
            // besides that, the method declaration from the C source remains untounched
            else if (tokenType == "METHOD")
            {
                string outStr;
                Match match = Regex.Match(token, "\\(([a-zA-Z0-9,\\* ])*\\)");
                if (match.Success)
                {
                    token = token.Replace(match.Value, "");
                    output += token;
                    outStr = match.Value.Replace("void", "");
                    outStr = Regex.Replace(outStr, "(char[\\s]*\\*[\\s]*|char [\\s]*([a-zA-Z][a-zA-Z0-9])*[\\s]*(\\[[0-9]+\\])[\\s]*)", "string ");
                    outStr = outStr.Replace("*", "");
                    output += outStr;
                }
            }
            // If a random variable is being declared in this token, we'll output it untouched
            else if (tokenType == "ASSORTED_VAR")
            {
                output += token;
            }
            // If a string variable is being declared, we'll remove all occurences of "[num]" or "*" and
            // replace "char" wiith "string"
            else if (tokenType == "STRING_VAR")
            {
                token = Regex.Replace(token, "[\\s]*(\\[[0-9]+\\])[\\s]*|\\*", "");
                token = Regex.Replace(token, "char", "string");
                output += token;
            }
            // If this token is a return statement, we'll output it untouched
            else if (tokenType == "RETURN")
            {
                output += token;
            }
            // If this token is starting a block, we'll output a "{"
            else if (tokenType == "START_BLOCK")
            {
                output += "{";
            }
            // If this token is ending a block, we'll output a "}"
            else if (tokenType == "END_BLOCK")
            {
                output += "}";
            }
            return output;
        }
    }
}
