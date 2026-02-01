namespace Minsk.CodeAnalysis.Syntax;

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
    IdentifierToken,

    // Keywords
    FalseKeyword,
    TrueKeyword,

    // Expressions
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
}
