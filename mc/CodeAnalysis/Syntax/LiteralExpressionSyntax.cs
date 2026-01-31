namespace Minsk.CodeAnalysis.Syntax;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{
    public LiteralExpressionSyntax(SyntaxToken syntaxToken)
    {
        LiteralToken = syntaxToken;
    }

    public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

    public SyntaxToken LiteralToken { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return LiteralToken;
    }
}
