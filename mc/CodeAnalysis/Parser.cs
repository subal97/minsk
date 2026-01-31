namespace Minsk.CodeAnalysis;

internal sealed class Parser
{
    private List<string> _diagnostics = [];
    private readonly SyntaxToken[] _tokens;
    private int _position;

    public List<string> Diagnostics => _diagnostics;

    public Parser(string text)
    {
        var tokens = new List<SyntaxToken>();
        var lexer = new Lexer(text);
        SyntaxToken token;

        do
        {
            token = lexer.Lex();

            if (token.Kind != SyntaxKind.WhiteSpaceToken && token.Kind != SyntaxKind.BadToken)
            {
                tokens.Add(token);
            }
        } while (token.Kind != SyntaxKind.EOFToken);

        _tokens = [.. tokens];
        _diagnostics.AddRange(lexer.Diagnostics);
    }

    private SyntaxToken Peek(int offset)
    {
        int index = _position + offset;
        return _tokens[index >= _tokens.Length ? ^1 : index];
    }

    public SyntaxToken Current => Peek(0);

    public SyntaxToken NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    public SyntaxToken MatchToken(SyntaxKind kind)
    {
        if (Current.Kind == kind)
        {
            return NextToken();
        }

        _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
        return new(kind, Current.Position, null!, null!);
    }

    public SyntaxTree Parse()
    {
        var expression = ParseExpression();
        var eofToken = MatchToken(SyntaxKind.EOFToken);

        return new SyntaxTree(
            root: expression,
            endOfFileToken: eofToken,
            diagnostics: _diagnostics
        );
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParseTerm();
    }

    private ExpressionSyntax ParseTerm()
    {
        var left = ParseFactor();

        while (Current.Kind is SyntaxKind.PlusToken or SyntaxKind.MinusToken)
        {
            var operatorToken = NextToken();
            var right = ParseFactor();
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpressionSyntax ParseFactor()
    {
        var left = ParsePrimaryExpression();

        while (Current.Kind is SyntaxKind.StarToken or SyntaxKind.SlashToken)
        {
            var operatorToken = NextToken();
            var right = ParsePrimaryExpression();
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpressionSyntax ParsePrimaryExpression()
    {
        if (Current.Kind is SyntaxKind.OpenParenthesisToken)
        {
            var left = NextToken();
            var expression = ParseExpression();
            var right = MatchToken(SyntaxKind.CloseParenthesisToken);

            return new ParenthesizedExpressionSyntax(
                openParenthesisToken: left,
                expression: expression,
                closeParenthesisToken: right
            );
        }

        var numberToken = MatchToken(SyntaxKind.NumberToken);
        return new LiteralExpressionSyntax(numberToken);
    }
}
