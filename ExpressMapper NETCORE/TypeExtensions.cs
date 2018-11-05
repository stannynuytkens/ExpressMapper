using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpressMapper
{
    public static class TypeExtensions
    {
        public static TypeInfo GetInfo(this Type type)
            => type.GetTypeInfo();

        public static IEnumerable<Type> GetInterfaces(this Type type)
            => type.GetInfo().ImplementedInterfaces;

        public static MethodInfo GetMethod(this Type type, string name, Type[] parameters = null)
            => type.GetRuntimeMethod(name, parameters ?? new Type[] { });

        public static IEnumerable<MethodInfo> GetMethods(this Type type)
            => type.GetRuntimeMethods();

        public static bool IsAssignableFrom(this Type type, Type other)
            => type.GetInfo().IsAssignableFrom(other.GetInfo());

        public static Type[] GetGenericArguments(this Type type)
            => type.GenericTypeArguments;

        public static ConstructorInfo GetConstructor(this Type type, Type[] types)
        {
            return type.GetInfo().DeclaredConstructors
                .FirstOrDefault(ci => ci.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(types));
        }

        public static ConstructorInfo[] GetConstructors(this Type type)
            => type.GetInfo().DeclaredConstructors.ToArray();

        public static ConstructorInfo[] GetConstructors(this Type type, BindingFlags bindingFlags)
        {
            return type.GetInfo().DeclaredConstructors.Where(ci => !ci.IsStatic && ci.IsPublic).ToArray();
        }

        public static PropertyInfo[] GetProperties(this Type type)
            => type.GetInfo().DeclaredProperties.ToArray();

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags bindingFlags)
        {
            // TODO: BindingFlags.FlattenHierarchy 
            return type.GetInfo().DeclaredProperties.Where(pi => !pi.GetMethod.IsStatic && pi.GetMethod.IsPublic).ToArray();
        }

        public static FieldInfo[] GetFields(this Type type)
            => type.GetInfo().DeclaredFields.ToArray();

        public static FieldInfo[] GetFields(this Type type, BindingFlags bindingFlags)
        {
            // TODO: BindingFlags.FlattenHierarchy 
            return type.GetInfo().DeclaredFields.Where(fi => !fi.IsStatic && fi.IsPublic).ToArray();
        }
    }

    public static class PropertyInfoExtensions
    {
        public static MethodInfo GetSetMethod(this PropertyInfo propertyInfo, bool nonPublic)
        {
            return !nonPublic && !propertyInfo.SetMethod.IsPublic
                        ? null
                        : propertyInfo.SetMethod;
        }
    }
}