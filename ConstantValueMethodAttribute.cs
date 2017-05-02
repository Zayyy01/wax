using System;

namespace Wax
{
    /// <summary>
  /// Used to mark constant expressions within queries.
  /// This attribute provides no additional functionality.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  internal class ConstantValueMethodAttribute : Attribute { }
}
