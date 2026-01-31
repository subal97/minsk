namespace Minsk.CodeAnalysis.Syntax;

internal static class SyntaxFacts
{
    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.StarToken or SyntaxKind.SlashToken => 2,
            SyntaxKind.PlusToken or SyntaxKind.MinusToken => 1,
            _ => 0,
        };
    }

    public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.MinusToken or SyntaxKind.PlusToken => 3,
            _ => 0,
        };
    }
}
