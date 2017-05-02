using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Wax
{
    /// <summary>
  /// Unwraps calls to <see cref="Wax.Expand{TParameter, TResult}(Expression{Func{TParameter, TResult}}, TParameter)"/>
  /// into the definition of the expression they call.
  /// </summary>
  internal class UnwrapVisitor : ExpressionVisitor
  {
    private static readonly Type DeclaringType = typeof(Wax);

    private static readonly Type UnwrappableType = typeof(UnwrappableMethodAttribute);

    private static readonly Type ConstantValueType = typeof(ConstantValueMethodAttribute);

    private static bool MethodIsThunk(MethodInfo method)
    {
      return method.DeclaringType == DeclaringType
        && method.IsPublic
        && method.CustomAttributes
          .Any(attr => attr.AttributeType == ConstantValueType);
    }

    private static bool MethodIsUnwrappable(MethodInfo method)
    {
      return method.DeclaringType == DeclaringType
        && method.IsPublic
        && method.CustomAttributes
          .Any(attr => attr.AttributeType == UnwrappableType);
    }

    // A dictionary of parameters to replace.
    private readonly Dictionary<ParameterExpression, Expression> _replacements;

    /// <summary>
    /// Unwraps calls to unwrappable methods
    /// into the definition of the expression they call.
    /// </summary>
    internal UnwrapVisitor()
    {
      _replacements = new Dictionary<ParameterExpression, Expression>();
    }

    private UnwrapVisitor(Dictionary<ParameterExpression, Expression> replacements)
    {
      _replacements = replacements;
    }

    // Replace a parameter if it's in our dictionary of replacements
    protected override Expression VisitParameter(ParameterExpression node)
    {
      if (_replacements.ContainsKey(node))
        return _replacements[node];
      else
        return base.VisitParameter(node);
    }

    private Expression Unwrap(MethodCallExpression node)
    {
      // The first argument of an unwrappable call
      // is the expression to unwrap into.
      var expression = (node.Arguments[0] as MemberExpression);

      if (expression == null)
        return base.VisitMethodCall(node);

      // The owning object that holds our method.
      object constant;

      var e = expression.Expression;
      var member = expression.Member;

      if (e == null)
      {
        // This is a static field or property
        constant = member.ReflectedType;
      }
      else
      {
        while (true)
        {
          // Dig down to the underlying
          // constant value of the expression
          if (e is ConstantExpression)
          {
            constant = (e as ConstantExpression).Value;
            break;
          }

          if (e is MemberExpression)
          {
            var m = e as MemberExpression;
            e = m.Expression;
            member = m.Member;
            continue;
          }

          throw new InvalidOperationException(
              $"Can't work with expression {e} of type {e.GetType()}.");
        }
      }

      // The field or property of `constant` that we want.
      var field = member as FieldInfo;
      var property = member as PropertyInfo;

      // The value of the field of `constant` - our method body.
      LambdaExpression lambda;

      if (property != null)
      {
        lambda = property.GetValue(constant) as LambdaExpression;
      }
      else
      {
          if (field != null) lambda = field.GetValue(constant) as LambdaExpression;
          else lambda = null;
      }

      if (lambda == null)
      {
        return base.VisitMethodCall(node);
      }

      var replacements = new Dictionary<ParameterExpression, Expression>();

      // Unwrap each parameter of the lambda by replacing
      // it with the correspoding argument to the outer
      // expression (the method call)
      for (var i = 0; i < lambda.Parameters.Count; i++)
      {
        // Recursively unwrap the entire tree.
        var replacement = Visit(node.Arguments[i + 1]);

        replacements.Add(lambda.Parameters[i], replacement);
      }

      // Allow another visit to replace parameters defined here and
      // to recursively unwrap method calls.
      return new UnwrapVisitor(replacements).Visit(lambda.Body);
    }

    private Expression Unthunk(MethodCallExpression node)
    {
      var lambda = Expression.Lambda(node.Arguments[0]);
      return Expression.Constant(lambda.Compile().DynamicInvoke());
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
      if (MethodIsUnwrappable(node.Method))
      {
        return Unwrap(node);
      }

      if (MethodIsThunk(node.Method))
      {
        return Unthunk(node);
      }

      return base.VisitMethodCall(node);
    }
  }
}
