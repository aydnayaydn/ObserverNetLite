using System;

namespace ObserverNetLite.API.Middlewares
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequireRoleAttribute : Attribute
    {
        public string[] Roles { get; }

        public RequireRoleAttribute(params string[] roles)
        {
            Roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }
    }
}
