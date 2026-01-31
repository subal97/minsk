namespace Minsk.CodeAnalysis.Syntax;

internal class Lexer
{
    private readonly string _text;
    private int _position;
    private List<string> _diagnostics = [];

    public IEnumerable<string> Diagnostics => _diagnostics;

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current
    {
        get
        {
            if (_position >= _text.Length)
                return '\0';

            return _text[_position];
        }
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

        var token = Current switch
        {
            '+' => new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null!),
            '-' => new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null!),
            '*' => new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null!),
            '/' => new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null!),
            '(' => new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null!),
            ')' => new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null!),
            _ => new SyntaxToken(
                SyntaxKind.BadToken,
                _position++,
                _text.Substring(_position - 1, 1),
                null!
            ),
        };

        if (token.Kind is SyntaxKind.BadToken)
        {
            _diagnostics.Add($"ERROR: bad character input: '{_text[_position - 1]}'");
        }

        return token;
    }
}
