using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Reflection;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    ///     Utility for Attributes class
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        ///     Return true if the attribute is defined on the object received.
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <param name="action">The action.</param>
        public static bool IsDefined<TAttribute>(ControllerActionDescriptor action) where TAttribute : Attribute
        {
            return action != null && (IsDefined<TAttribute>(action.ControllerTypeInfo) || IsDefined<TAttribute>(action.MethodInfo));
        }

        /// <summary>
        ///     Return true if the attribute is defined on the object received.
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <param name="provider">The provider.</param>
        /// <returns>
        ///     <c>true</c> if the specified provider is defined; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDefined<TAttribute>(ICustomAttributeProvider provider) where TAttribute : Attribute
        {
            var isDefined = provider.IsDefined(typeof(TAttribute), true);
            return isDefined;
        }
    }
}
