using System;
using FluentAssertions.Primitives;

namespace Resultron.FluentAssertions;

/// <summary>
/// Provides fluent assertion methods for verifying the state of <see cref="Result{T}"/> objects.
/// </summary>
/// <typeparam name="T">The type of the value contained in the result.</typeparam>
public partial class ResultAssertions<T> : ReferenceTypeAssertions<Result<T>, ResultAssertions<T>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResultAssertions{T}"/> class.
    /// </summary>
    /// <param name="instance">The <see cref="Result{T}"/> instance to be tested.</param>
    public ResultAssertions(Result<T> instance) : base(instance)
    {
        
    }

    /// <summary>
    /// Gets the identifier for the subject under test.
    /// </summary>
    protected override string Identifier => "result";
}