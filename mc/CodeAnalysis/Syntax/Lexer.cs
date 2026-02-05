namespace Minsk.CodeAnalysis.Syntax;

internal class Lexer
{
    private readonly string _text;
    private int _position;
    private List<string> _diagnostics = [];

    public Lexer(string text)
    {
        _text = text;
    }

    public IEnumerable<string> Diagnostics => _diagnostics;

    private char Current => Peek(0);

    private char LookAhead => Peek(1);

    private char Peek(int offset)
    {
        int index = _position + offset;

        if (index >= _text.Length)
            return '\0';

        return _text[index];
    }

    private void Next()
    {
        _position++;
    }

    public SyntaxToken Lex()
    {
        // <numbers>
        // + - * / ()
        // <whitespace>
        // <boolean>
        // EOF

        if (_position >= _text.Length)
        {
            return new SyntaxToken(SyntaxKind.EOFToken, _position, "\0", null!);
        }

        if (char.IsDigit(Current))
        {
            var start = _position;

            while (char.IsDigit(Current))
                Next();

            var text = _text[start.._position];
            if (!int.TryParse(text, out int value))
            {
                _diagnostics.Add($"The number <{text}> isn't a valid Int32.");
            }
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }

        if (char.IsWhiteSpace(Current))
        {
            var start = _position;

            while (char.IsWhiteSpace(Current))
                Next();

            var text = _text[start.._position];
            return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, null!);
        }

        if (char.IsLetter(Current))
        {
            var start = _position;

            while (char.IsLetter(Current))
                Next();

            var text = _text[start.._position];
            var kind = SyntaxFacts.GetKeywordKind(text);
            return new SyntaxToken(kind, start, text, null!);
        }

        switch (Current)
        {
            // Arithematic tokens
            case '+':
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null!);
            case '-':
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null!);
            case '*':
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null!);
            case '/':
                return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null!);

            // Parenthesis tokens
            case '(':
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null!);
            case ')':
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null!);

            // Logical token
            case '!':
                return new SyntaxToken(SyntaxKind.BangToken, _position++, "!", null!);
            case '&':
                if (LookAhead == '&')
                    return new SyntaxToken(
                        SyntaxKind.AmpersandAmpersandToken,
                        _position += 2,
                        "&&",
                        null!
                    );
                break;
            case '|':
                if (LookAhead == '|')
                    return new SyntaxToken(SyntaxKind.PipePipeToken, _position += 2, "||", null!);
                break;
        }

        _diagnostics.Add($"ERROR: bad character input: '{Current}'");
        return new SyntaxToken(
            SyntaxKind.BadToken,
            _position++,
            _text.Substring(_position - 1, 1),
            null!
        );
    }
}
