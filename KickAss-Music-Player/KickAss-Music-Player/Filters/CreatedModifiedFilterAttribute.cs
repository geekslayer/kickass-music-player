namespace API.Filters
{
    using DataModels.Dto;
    using KickAss_Music_Player.DataModels.Dto.User;
    using KickAss_Music_Player.Security;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This filter fill all the CreatedBy/CreationDate/ModifiedBy/ModificationDate field into input commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CreatedModifiedFilterAttribute : ActionFilterAttribute
    {
        private readonly ITokenInterpretor _tokenInterpretor;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tokenInterpretor">Token information processor</param>
        public CreatedModifiedFilterAttribute(ITokenInterpretor tokenInterpretor)
        {
            _tokenInterpretor = tokenInterpretor;
        }
        /// <summary>
        /// This is the main method after the controller is loaded and when controller actions are called this will log the call 
        /// and all arguments passed to the method
        /// </summary>
        /// <param name="actionContext">context in which we are in</param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _tokenInterpretor.InitUser(actionContext.HttpContext.User);
            var user = _tokenInterpretor.GetUser();
            if (user != null)
            {
                foreach (var arg in actionContext.ActionArguments)
                {
                    FillAuthorFields(arg.Value, user);
                }
            }

            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// Fill the argument object
        /// </summary>
        /// <param name="item">argument</param>
        /// <param name="user">user</param>
        protected void FillAuthorFields(object item, UserResult user)
        {
            if (item == null) return;
            var type = item.GetType();
            var isArray = false;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) && type.GetGenericArguments().Any())
            {
                isArray = true;
                type = type.GetGenericArguments().First();
            }
            if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)) && type.GetGenericArguments().Any())
            {
                isArray = true;
                type = type.GetGenericArguments().First();
            }

            // ICurrentUser interface processing
            if (type.GetInterfaces().Any(i => i == typeof(ICurrentUser)))
            {
                if (isArray)
                {
                    foreach (var update in (IEnumerable<ICurrentUser>)item)
                    {
                        update.CurrentUser = user;
                    }
                }
                else
                {
                    ((ICurrentUser)item).CurrentUser = user;
                }
            }
            
            //foreach (var propertyInfo in type.GetProperties().Where(v => v.PropertyType.FullName != null && v.PropertyType.FullName.Contains("DataModels.")))
            //{
            //    if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>) && propertyInfo.PropertyType.GetGenericArguments().Any() ||
            //        propertyInfo.PropertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)) && propertyInfo.PropertyType.GetGenericArguments().Any())
            //    {
            //        var list = propertyInfo.GetValue(item) as IEnumerable ?? new List<object>();
            //        foreach (var i in list)
            //        {
            //            FillAuthorFields(i, user);
            //        }
            //    }
            //    else
            //    {
            //        FillAuthorFields(propertyInfo.GetValue(item), user);
            //    }
            //}
        }
    }
}
