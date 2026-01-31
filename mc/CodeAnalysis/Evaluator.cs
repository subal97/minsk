namespace Minsk.CodeAnalysis;

public class Evaluator
{
    private readonly ExpressionSyntax _root;

    public Evaluator(ExpressionSyntax root)
    {
        _root = root;
    }

    public int Evaluate()
    {
        var result = EvaluateExpression(_root);
        return result;
    }

    private int EvaluateExpression(ExpressionSyntax node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node is LiteralExpressionSyntax n)
        {
            return (int)n.LiteralToken.Value;
        }

        if (node is UnaryExpressionSyntax u)
        {
            var operand = EvaluateExpression(u.Operand);

            return u.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => operand,
                SyntaxKind.MinusToken => -operand,
                _ => throw new Exception($"Enexpected unary operator <{u.OperatorToken.Kind}>"),
            };
        }

        if (node is BinaryExpressionSyntax b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            return b.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => left + right,
                SyntaxKind.MinusToken => left - right,
                SyntaxKind.StarToken => left * right,
                SyntaxKind.SlashToken => left / right,
                _ => throw new Exception($"Enexpected binary operator <{b.OperatorToken.Kind}>"),
            };
        }

        if (node is ParenthesizedExpressionSyntax p)
        {
            return EvaluateExpression(p.Expression);
        }

        throw new Exception($"Unsupported expression {node.Kind}");
    }
}
