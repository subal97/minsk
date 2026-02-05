using Minsk.CodeAnalysis.Binding;

namespace Minsk.CodeAnalysis;

internal class Evaluator
{
    private readonly BoundExpression _expression;

    public Evaluator(BoundExpression expression)
    {
        _expression = expression;
    }

    public object Evaluate()
    {
        var result = EvaluateExpression(_expression);
        return result;
    }

    private object EvaluateExpression(BoundExpression node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node is BoundLiteralExpression n)
        {
            return n.Value;
        }

        if (node is BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

            return u.OperatorKind switch
            {
                BoundUnaryOperatorKind.Identity => (int)operand,
                BoundUnaryOperatorKind.Negation => -(int)operand,
                BoundUnaryOperatorKind.LogicalNegation => (bool)operand,
                _ => throw new Exception($"Unexpected unary operator {u.OperatorKind}"),
            };
        }

        if (node is BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            return b.OperatorKind switch
            {
                // Arithematic
                BoundBinaryOperatorKind.Addition => (int)left + (int)right,
                BoundBinaryOperatorKind.Subtraction => (int)left - (int)right,
                BoundBinaryOperatorKind.Multiplication => (int)left * (int)right,
                BoundBinaryOperatorKind.Division => (int)left / (int)right,

                // Logical
                BoundBinaryOperatorKind.LogcalOr => (bool)left || (bool)right,
                BoundBinaryOperatorKind.LogicalAnd => (bool)left && (bool)right,

                _ => throw new Exception($"Unexpected binary operator {b.OperatorKind}"),
            };
        }

        throw new Exception($"Unsupported expression {node.Kind}");
    }
}
