namespace Minsk.CodeAnalysis;

enum SyntaxKind
{
    NumberToken,
    WhiteSpaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParanthesisToken,
    CloseParanthesisToken,
    BadToken,
    EOFToken,

    NumberExpression,
    BinaryExpression,
    ParenthesizedExpression,
}
