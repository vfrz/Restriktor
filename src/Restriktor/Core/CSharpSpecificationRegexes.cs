using System.Text.RegularExpressions;

namespace Restriktor.Core
{
    /*
     * Based on https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/lexical-structure#lexical-analysis
     */
    internal static class CSharpSpecificationRegexes
    {
        public static readonly Regex LetterCharacter = new(@"\p{Lu}|\p{Ll}|\p{Lt}|\p{Lm}|\p{Lo}|\p{Nl}");

        public static readonly Regex CombiningCharacter = new(@"\p{Mc}|\p{Mn}");

        public static readonly Regex DecimalDigitCharacter = new(@"\p{Nd}");

        public static readonly Regex ConnectingCharacter = new(@"\p{Pc}");

        public static readonly Regex FormattingCharacter = new(@"\p{Cf}");

        public static readonly Regex IdentifierPartCharacter = new($"{LetterCharacter}|{CombiningCharacter}|{DecimalDigitCharacter}|{ConnectingCharacter}|{FormattingCharacter}");

        public static readonly Regex IdentifierStartCharacter = new($"{LetterCharacter}|_");

        public static readonly Regex Identifier = new($"(?:{IdentifierStartCharacter})(?:{IdentifierPartCharacter})*");

        public static readonly Regex Namespace = new($@"(?:({Identifier})(?:\.({Identifier}))*)");

        public static readonly Regex Type = new($@"(?:(?<Namespace>{Namespace})\.)?(?<Type>{Identifier})");
    }
}