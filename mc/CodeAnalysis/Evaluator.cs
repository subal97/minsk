using Minsk.CodeAnalysis.Binding;

namespace Minsk.CodeAnalysis;

internal class Evaluator
{
    private readonly BoundExpression _expression;

    public Evaluator(BoundExpression expression)
    {
        _expression = expression;
    }

    public int Evaluate()
    {
        var result = EvaluateExpression(_expression);
        return result;
    }

    private int EvaluateExpression(BoundExpression node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node is BoundLiteralExpression n)
        {
            return (int)n.Value;
        }

        if (node is BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

            return u.OperatorKind switch
            {
                BoundUnaryOperatorKind.Identity => operand,
                BoundUnaryOperatorKind.Negation => -operand,
                _ => throw new Exception($"Enexpected unary operator <{u.OperatorKind}>"),
            };
        }

        if (node is BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            return b.OperatorKind switch
            {
                BoundBinaryOperatorKind.Addition => left + right,
                BoundBinaryOperatorKind.Subtraction => left - right,
                BoundBinaryOperatorKind.Multiplication => left * right,
                BoundBinaryOperatorKind.Division => left / right,
                _ => throw new Exception($"Enexpected binary operator <{b.OperatorKind}>"),
            };
        }

        throw new Exception($"Unsupported expression {node.Kind}");
    }
}
