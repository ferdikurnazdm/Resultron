using System;
using FluentAssertions.Primitives;

namespace Resultron.FluentAssertions;

/// <summary>
/// Provides fluent assertion methods for verifying the state of <see cref="Result"/> objects.
/// </summary>
public partial class ResultAssertions : ReferenceTypeAssertions<Result, ResultAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResultAssertions"/> class.
    /// </summary>
    /// <param name="instance">The <see cref="Result"/> instance to be tested.</param>
    public ResultAssertions(Result instance) : base(instance)
    {
        
    }

    /// <summary>
    /// Gets the identifier for the subject under test.
    /// </summary>
    protected override string Identifier => "result";
}
