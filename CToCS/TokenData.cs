/*
 * FILE         : TokenData.cs
 * AUTHOR       : A. Saad Imran
 * FIRST VER.   : February 26, 2017
 * DESCRIPTION:
 *  The TokenData class is used to specify the criteria for identifying
 * different tokens using regular expressions. Each TokenData instance
 * has a Regex and a TokenType data member. The Regex object specifies 
 * how the TokenType is to be identified.
 */

using System.Text.RegularExpressions;

namespace CToCS
{
    public class TokenData
    {
        private Regex pattern = null;
        private TokenType type;
        public TokenData(Regex pattern, TokenType type)
        {
            this.pattern = pattern;
            this.type = type;
        }

        /*
         * METHOD       : GetPattern()
         * DESCRIPTION  : Gets regex pattern for this token type
         */
        public Regex GetPattern()
        {
            return pattern;
        }

        /*
         * METHOD       : GetTokenType()
         * DESCRIPTION  : Gets the token type for this TokenData type
         */
        public TokenType GetTokenType()
        {
            return type;
        }
    }
}
