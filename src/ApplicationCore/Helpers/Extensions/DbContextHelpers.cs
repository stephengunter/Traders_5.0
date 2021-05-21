using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helpers
{
    public static class DbContextHelpers
    {
        public static IQueryable<object> Set(this DbContext context, Type T)
        {
            MethodInfo method = typeof(DbContext).GetMethods()
                .Where(p => p.Name == "Set" && p.ContainsGenericParameters).FirstOrDefault();

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(T);

            return (IQueryable<object>)method.Invoke(context, null);
        }
        //public static IQueryable<object> Set(this DbContext context, Type t)
        //{

        //    return (IQueryable<object>)context.GetType()
        //          .GetMethod("Set")?
        //          .MakeGenericMethod(t)
        //          .Invoke(context, null);

        //}
        //public static IQueryable<T> Set<T>(this DbContext context)
        //{
        //    // Get the generic type definition 
        //    MethodInfo method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);

        //    // Build a method with the specific type argument you're interested in 
        //    method = method.MakeGenericMethod(typeof(T));

        //    return method.Invoke(context, null) as IQueryable<T>;
        //}
    }
}
