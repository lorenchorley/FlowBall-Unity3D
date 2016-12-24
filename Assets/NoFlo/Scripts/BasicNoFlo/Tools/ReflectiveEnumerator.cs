using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectiveEnumerator {
    static ReflectiveEnumerator() { }

    public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class {
        return Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));
    }

    public static TValue GetAttributeValue<TAttribute, TValue>(this Type type,
                                                               Func<TAttribute, TValue> valueSelector)
                                                               where TAttribute : Attribute {
        var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        if (att != null) {
            return valueSelector(att);
        }
        return default(TValue);
    }

}
