using Minsk.CodeAnalysis;

namespace Minsk;

internal class Program
{
    static void Main()
    {
        var showTree = false;
        while (true)
        {
            Console.Write("> ");

            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                return;

            if (line.Equals("#showTree", StringComparison.OrdinalIgnoreCase))
            {
                showTree = !showTree;
                Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees.");
                continue;
            }
            else if (line.Equals("#cls"))
            {
                Console.Clear();
                continue;
            }

            var syntaxTree = SyntaxTree.Parse(line);

            if (showTree)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                PrettyPrint(syntaxTree.Root);
                Console.ResetColor();
            }

            if (!syntaxTree.Diagnostics.Any())
            {
                var e = new Evaluator(root: syntaxTree.Root);
                var result = e.Evaluate();
                Console.WriteLine(result);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var diagnostic in syntaxTree.Diagnostics)
                {
                    Console.WriteLine(diagnostic);
                }
                Console.ResetColor();
            }
        }
    }

    static void PrettyPrint(
        SyntaxNode node,
        string indent = "",
        bool isLast = false,
        bool isChild = false
    )
    {
        // │ , ├── , └──

        var marker = isChild
            ? isLast
                ? "└──"
                : "├──"
            : string.Empty;

        Console.Write(indent);
        Console.Write(marker);
        Console.Write(node.Kind);

        if (node is SyntaxToken token && token.Value is not null)
        {
            Console.Write($" {token.Value}");
        }

        indent += isChild
            ? isLast
                ? "    "
                : "│    "
            : "    ";
        Console.WriteLine();

        var last = node.GetChildren().LastOrDefault();
        foreach (var child in node.GetChildren())
        {
            PrettyPrint(node: child, indent: indent, isLast: child == last, isChild: true);
        }
    }
}
