namespace Minsk.CodeAnalysis.Syntax;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{
    public LiteralExpressionSyntax(SyntaxToken syntaxToken)
        : this(syntaxToken, syntaxToken.Value) { }

    public LiteralExpressionSyntax(SyntaxToken syntaxToken, object value)
    {
        LiteralToken = syntaxToken;
        Value = value;
    }

    public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

    public SyntaxToken LiteralToken { get; }

    public object Value { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return LiteralToken;
    }
}
