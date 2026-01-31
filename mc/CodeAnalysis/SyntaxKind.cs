namespace Minsk.CodeAnalysis;

public enum SyntaxKind
{
    // Tokens
    BadToken,
    EOFToken,
    WhiteSpaceToken,
    NumberToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,

    // Expressions
    LiteralExpression,
    BinaryExpression,
    ParenthesizedExpression,
}
