using System;

namespace Opw.HttpExceptions.Attributes
{
    /// <summary>
    /// Set the exception properties visibility to public.
    /// All attributed properties are included in the problem details as additional properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ProblemDetailsAttribute : Attribute
    {
    }
}
