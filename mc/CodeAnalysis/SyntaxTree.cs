namespace Minsk.CodeAnalysis;

public sealed class SyntaxTree
{
    public SyntaxTree(
        ExpressionSyntax root,
        SyntaxToken endOfFileToken,
        IEnumerable<string> diagnostics
    )
    {
        Root = root;
        EndOfFileToken = endOfFileToken;
        Diagnostics = [.. diagnostics];
    }

    public IReadOnlyList<string> Diagnostics { get; }
    public SyntaxToken EndOfFileToken { get; }
    public ExpressionSyntax Root { get; }

    public static SyntaxTree Parse(string text)
    {
        var parser = new Parser(text);
        return parser.Parse();
    }
}
