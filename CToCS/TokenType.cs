/*
 * FILE         : TokenType.cs
 * AUTHOR       : A. Saad Imran
 * FIRST VER.   : February 26, 2017
 * DESCRIPTION:
 *  This file contains an enum to store the different token types
 * This enum is used in the Token, TokenData and Tokenizer classes
 * to identify and deal with the different kinds of tokens
 */

namespace CToCS
{
    public enum TokenType
    {
        TOKEN,
        START_BLOCK,
        END_BLOCK,
        MAIN_METHOD,
        METHOD,
        METHOD_CALL,
        RETURN,
        FILE_VAR,
        STRING_VAR,
        ASSORTED_VAR,
        PRINTF,
        FOPEN,
        FGETS,
        STRLEN,
        GETS,
        ATOI,
        FPRINTF,
        FCLOSE,
        FOR,
        IF_ELSE,
        WHILE,
        NONE
    }
}
