using System;

namespace Opw.HttpExceptions
{
    public class ProtectedException<T> : ConflictException
    {
        public ProtectedException() : base("Protected.")
        { }

        public ProtectedException(string id)
            : base($"{typeof(T).Name} \"{id}\" is protected and can not be modified or deleted.")
        {
        }

        protected override string GetTitle(string typeName = null)
        {
            typeName = typeof(T).Name;
            if (typeof(T).IsInterface)
                typeName = typeName.Substring(1);

            return base.GetTitle(typeName);
        }
    }
}
