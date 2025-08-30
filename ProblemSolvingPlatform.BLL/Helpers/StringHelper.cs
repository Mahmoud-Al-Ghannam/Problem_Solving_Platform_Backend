using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Helpers {
    public class StringHelper {

        public static string RemoveWhiteSpaces(string str) {
            str = str.Trim();
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < str.Length; i++) {
                if (str[i] != ' ')
                    stringBuilder.Append(str[i]);
                else if (str[i - 1] != ' ')
                    stringBuilder.Append(str[i]);
            }

            return stringBuilder.ToString();
        }
        public static bool EqualEgnoreWhiteSpaces(string str1, string str2) {
            str1 = RemoveWhiteSpaces(str1);
            str2 = RemoveWhiteSpaces(str2);
            return str1.Equals(str2);
        }

        public static object TrimStringsOfPrimitive(object value, HashSet<object> visited) {
            Type type = value.GetType();
            if (!type.IsPrimitive) throw new Exception("The value must to be primitive type");

            visited ??= new();
            if (visited.Contains(value)) return value;
            visited.Add(value);

            if (type == typeof(string)) {
                return (value as string).Trim();
            }

            return value;
        }

        public static object TrimStringsOfArray(object value, HashSet<object> visited) {
            Type type = value.GetType();
            if (!type.IsArray) throw new Exception("The value must to be array");

            visited ??= new();
            if (visited.Contains(value)) return value;
            visited.Add(value);

            var arr = value as object[];

            for (int i = 0; i < arr.Length; i++)
                arr[i] = TrimStringsOfObject(arr[i], visited);

            return arr;
        }

        public static object TrimStringsOfList(object value, HashSet<object> visited) {
            Type type = value.GetType();
            if (!type.GetInterfaces().Any(t => t == typeof(IList))) throw new Exception("The value must to be list");

            visited ??= new();
            if (visited.Contains(value)) return value;
            visited.Add(value);

            var list = value as IList;
            for (int i = 0; i < list.Count; i++)
                list[i] = TrimStringsOfObject(list[i], visited);

            return list;
        }

        public static object TrimStringsOfDictionary(object value, HashSet<object> visited) {
            Type type = value.GetType();
            if (!type.GetInterfaces().Any(t => t == typeof(IDictionary))) throw new Exception("The value must to be dictionary");

            visited ??= new();
            if (visited.Contains(value)) return value;
            visited.Add(value);

            var dictionary = value as IDictionary;
            var dictionaryCopy = Activator.CreateInstance(type) as IDictionary;
            foreach (var key in dictionary.Keys) {
                object k = TrimStringsOfObject(key, visited);
                object v = TrimStringsOfObject(dictionary[key], visited);
                dictionaryCopy[k] = v;
            }

            return dictionaryCopy;
        }

        public static object TrimStringsOfCollection(object value, HashSet<object> visited) {
            Type type = value.GetType();
            if (!type.GetInterfaces().Any(t => t == typeof(ICollection))) throw new Exception("The value must to be collection");


            if (type.GetInterfaces().Any(t => t == typeof(IList)))
                return TrimStringsOfList(value, visited);

            if (type.GetInterfaces().Any(t => t == typeof(IDictionary)))
                return TrimStringsOfDictionary(value, visited);

            visited ??= new();
            if (visited.Contains(value)) return value;
            visited.Add(value);


            var collection = value as ICollection;
            var collectionCopy = Activator.CreateInstance(type) as ICollection<object>;
            foreach (var item in collection)
                collectionCopy.Add(TrimStringsOfObject(item, visited));

            return collectionCopy;
        }

        public static object TrimStringsOfClass(object value, HashSet<object> visited) {
            Type type = value.GetType();
            if (type.IsArray || type.GetInterfaces().Any(t => t == typeof(ICollection)))
                throw new Exception("The value must to be not collection or array type");

            visited ??= new();
            if (visited.Contains(value)) return value;
            visited.Add(value);

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                object propValue = prop.GetValue(value);
                propValue = TrimStringsOfObject(propValue, visited);
                prop.SetValue(value, propValue);
            }

            return value;
        }

        public static object TrimStringsOfObject(object value, HashSet<object> visited) {
            if (value == null) return null;
            Type type = value.GetType();


            try {
                if (type == typeof(string))
                    return (value as string).Trim();
                else if (type.IsPrimitive)
                    return TrimStringsOfPrimitive(value, visited);
                else if (type.IsArray)
                    return TrimStringsOfArray(value, visited);
                else if (type.GetInterfaces().Any(t => t == typeof(ICollection)))
                    return TrimStringsOfCollection(value, visited);
                else
                    return TrimStringsOfClass(value, visited);
            }
            catch (Exception ex) {
                return value;
            }
            
        }


    }
}
