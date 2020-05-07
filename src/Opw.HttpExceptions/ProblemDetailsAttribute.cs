using System;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Include the property in the problem details in exception details.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ProblemDetailsAttribute : Attribute
    {
    }
}
