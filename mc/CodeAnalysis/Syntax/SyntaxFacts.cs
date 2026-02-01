namespace Minsk.CodeAnalysis.Syntax;

internal static class SyntaxFacts
{
    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.StarToken or SyntaxKind.SlashToken => 4,
            SyntaxKind.PlusToken or SyntaxKind.MinusToken => 3,
            SyntaxKind.AmpersandAmpersandToken => 2,
            SyntaxKind.PipePipeToken => 1,
            _ => 0,
        };
    }

    public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.MinusToken or SyntaxKind.PlusToken or SyntaxKind.BangToken => 5,
            _ => 0,
        };
    }

    public static SyntaxKind GetKeywordKind(string text)
    {
        return text switch
        {
            "false" => SyntaxKind.FalseKeyword,
            "true" => SyntaxKind.TrueKeyword,
            _ => SyntaxKind.IdentifierToken,
        };
    }
}
