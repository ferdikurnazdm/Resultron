using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Resultron.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ResultUnusedAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "RES001";
    private const string Title = "Result Ignored";
    private const string MessageFormat = "The Result returned from method '{0}' must be checked or assigned to a variable";
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Result objects cannot be ignored; their error status must be checked.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeMethodInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeMethodInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken);

        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol) return;

        var returnTypeName = methodSymbol.ReturnType.Name;

        if (!returnTypeName.Contains("Result")) return;

        if (invocation.Parent is ExpressionStatementSyntax)
        {
            var methodName = methodSymbol.Name;

            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), methodName);
            
            context.ReportDiagnostic(diagnostic);
        }
    }
}
