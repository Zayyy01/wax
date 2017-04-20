using System.Linq.Expressions;

namespace Wax
{
    /// <summary>
  /// An expression visitor that acts as a simple find
  /// and replace.
  /// </summary>
  internal class ReplaceVisitor : ExpressionVisitor
  {
    /// <summary>
    /// The expression to find.
    /// </summary>
    private readonly Expression _source;

    /// <summary>
    /// The replacement expression.
    /// </summary>
    private readonly Expression _replacement;

    /// <summary>
    /// Create a new replacement visitor.
    /// </summary>
    /// <param name="source">
    /// The expression to find.
    /// </param>
    /// <param name="replacement">
    /// The replacement expression.
    /// </param>
    public ReplaceVisitor(Expression source, Expression replacement)
    {
      _source = source;
      _replacement = replacement;
    }

    public override Expression Visit(Expression node)
    {
      if (_source == node) return _replacement;

      return base.Visit(node);
    }
  }
}
