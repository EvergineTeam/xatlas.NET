using CppAst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XAtlasGen
{
    public static class Helpers
    {
        public static List<string> TypedefList;
        public static List<string> DelegateList;

        private static readonly Dictionary<string, string> csNameMappings = new Dictionary<string, string>()
        {
            { "bool", "bool" },
            { "uint8_t", "byte" },
            { "uint16_t", "ushort" },
            { "uint32_t", "uint" },
            { "uint64_t", "ulong" },
            { "int8_t", "sbyte" },
            { "int32_t", "int" },
            { "int16_t", "short" },
            { "int64_t", "long" },
            { "int64_t*", "long*" },
            { "char", "byte" },
            { "size_t", "nuint" },
            { "intptr_t", "nint" },
            { "uintptr_t", "nuint" },
        };

        public static string ConvertToCSharpType(CppType type, bool isPointer = false)
        {
            if (type is CppPrimitiveType primitiveType)
            {
                return GetCsTypeName(primitiveType, isPointer);
            }

            if (type is CppQualifiedType qualifiedType)
            {
                return ConvertToCSharpType(qualifiedType.ElementType, isPointer);
            }

            if (type is CppEnum enumType)
            {
                var enumCsName = GetCsCleanName(enumType.Name);
                if (isPointer)
                    return enumCsName + "*";

                return enumCsName;
            }

            if (type is CppTypedef typedef)
            {
                var typeDefCsName = GetCsCleanName(typedef.Name);

                if (isPointer)
                    return typeDefCsName + "*";

                return typeDefCsName;
            }

            if (type is CppClass @class)
            {
                var className = GetCsCleanName(@class.Name);
                if (isPointer)
                    return className + "*";

                return className;
            }

            if (type is CppPointerType pointerType)
            {
                return GetCsTypeName(pointerType);
            }

            if (type is CppArrayType arrayType)
            {
                return ConvertToCSharpType(arrayType.ElementType, isPointer);
            }

            return string.Empty;
        }

        private static string GetCsTypeName(CppPointerType pointerType)
        {
            var elementType = pointerType.ElementType;
            if (elementType is CppQualifiedType qualifiedType)
            {
                elementType = qualifiedType.ElementType;
            }

            if (elementType is CppPointerType)
            {
                return ConvertToCSharpType(elementType) + "*";
            }

            return ConvertToCSharpType(elementType, true);
        }

        private static string GetCsTypeName(CppPrimitiveType primitiveType, bool isPointer)
        {
            string result = string.Empty;

            switch (primitiveType.Kind)
            {
                case CppPrimitiveKind.Void:
                    result = "void";
                    break;
                case CppPrimitiveKind.Bool:
                    result = "bool";
                    break;
                case CppPrimitiveKind.Char:
                    result = "byte";
                    break;
                case CppPrimitiveKind.WChar:
                    result = "char";
                    break;
                case CppPrimitiveKind.Short:
                    result = "short";
                    break;
                case CppPrimitiveKind.Int:
                    result = "int";
                    break;
                case CppPrimitiveKind.UnsignedShort:
                    result = "ushort";
                    break;
                case CppPrimitiveKind.UnsignedInt:
                    result = "uint";
                    break;
                case CppPrimitiveKind.Float:
                    result = "float";
                    break;
                case CppPrimitiveKind.Double:
                    result = "double";
                    break;
                case CppPrimitiveKind.UnsignedChar:
                    result = "byte";
                    break;
                case CppPrimitiveKind.LongLong:
                    result = "long";
                    break;
                case CppPrimitiveKind.UnsignedLongLong:
                    result = "ulong";
                    break;
                case CppPrimitiveKind.LongDouble:
                    result = "double";
                    break;
                default:
                    break;
            }

            if (isPointer)
            {
                result += "*";
            }

            return result;
        }

        public enum Family
        {
            param,
            field,
            ret,
        }

        public static string ShowAsMarshalType(string type, Family family)
        {
            switch (type)
            {
                case "bool":
                    switch (family)
                    {
                        case Family.param:
                            return "[MarshalAs(UnmanagedType.Bool)] bool";
                        case Family.ret:
                            return "bool";
                        case Family.field:
                        default:
                            return "byte";
                    }
                case "bool*":
                    switch (family)
                    {
                        case Family.ret:
                            return "bool";
                        case Family.param:
                        case Family.field:
                        default:
                            return "byte*";
                    }
                case "char*":
                case "unsigned char*":
                    switch (family)
                    {
                        case Family.param:
                            return "[MarshalAs(UnmanagedType.LPStr)] string";
                        case Family.ret:
                            return "string";
                        case Family.field:
                        default:
                            return "byte*";
                    }
                default:
                    // Delegate types in struct fields must be IntPtr for blittable layout
                    if (family == Family.field && DelegateList != null && DelegateList.Any(d => StripPrefix(d) == type))
                    {
                        return "IntPtr";
                    }
                    return type;
            }
        }

        private static string GetCsCleanName(string name)
        {
            if (csNameMappings.TryGetValue(name, out string mappedName))
            {
                return GetCsCleanName(mappedName);
            }

            if (name.StartsWith("PFN"))
            {
                return "IntPtr";
            }

            if (name.Contains("Flags"))
            {
                return StripPrefix(name.Remove(name.Length - 5));
            }

            if (TypedefList.Contains(name))
            {
                return "IntPtr";
            }

            return StripPrefix(name);
        }

        public static void PrintComments(StreamWriter file, CppComment comment, string tabs = "", bool newLine = false)
        {
            if (comment != null)
            {
                if (newLine) file.WriteLine();

                file.WriteLine($"{tabs}/// <summary>");
                GetText(file, comment, tabs);
                file.WriteLine($"{tabs}/// </summary>");
            }
        }

        private static void GetText(StreamWriter file, CppComment comment, string tabs)
        {
            switch (comment.Kind)
            {
                case CppCommentKind.Text:
                    var commentText = comment as CppCommentTextBase;
                    file.WriteLine($"{tabs}/// {commentText.Text}");
                    break;
                case CppCommentKind.Paragraph:
                case CppCommentKind.Full:
                    foreach (var child in comment.Children)
                    {
                        GetText(file, child, tabs);
                    }
                    break;
                default:
                    ;
                    break;
            }
        }

        public static string ValidParamName(string name, string type, int i)
        {
            if (string.IsNullOrEmpty(name))
            {
                switch (type)
                {
                    case "void*":
                        return $"ptr{i}";
                    case "uint":
                    case "nuint":
                        return $"size{i}";
                    case "byte*":
                        return $"bytePtr{i}";
                    default:
                        return $"param{i}";
                }
            }
            else
            {
                return EscapeReservedKeyword(name);
            }
        }

        public static string StripPrefix(string name)
        {
            // Strip xatlas / XATLAS_ prefixes (case-sensitive matching)
            string[] prefixes = { "xatlas", "XATLAS_" };
            foreach (var prefix in prefixes)
            {
                if (name.StartsWith(prefix))
                {
                    return name.Substring(prefix.Length);
                }
            }

            return name;
        }

        public static string PascalCaseField(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            return char.ToUpperInvariant(name[0]) + name.Substring(1);
        }

        public static string FindCommonPrefix(IEnumerable<string> names)
        {
            var list = names.ToList();
            if (list.Count == 0)
                return string.Empty;

            var first = list[0];
            int prefixLen = first.Length;

            foreach (var name in list.Skip(1))
            {
                prefixLen = Math.Min(prefixLen, name.Length);
                for (int i = 0; i < prefixLen; i++)
                {
                    if (first[i] != name[i])
                    {
                        prefixLen = i;
                        break;
                    }
                }
            }

            // Snap back to the last underscore boundary
            var prefix = first.Substring(0, prefixLen);
            int lastUnderscore = prefix.LastIndexOf('_');
            if (lastUnderscore >= 0)
                return prefix.Substring(0, lastUnderscore + 1);

            return string.Empty;
        }

        public static string ScreamingToPascalCase(string screaming)
        {
            if (string.IsNullOrEmpty(screaming))
                return screaming;

            var parts = screaming.Split('_');
            var result = string.Empty;

            foreach (var part in parts)
            {
                if (part.Length == 0)
                    continue;

                // Preserve numeric-leading segments as-is (e.g., "2D")
                if (char.IsDigit(part[0]))
                {
                    result += part;
                }
                else
                {
                    result += char.ToUpperInvariant(part[0]) + part.Substring(1).ToLowerInvariant();
                }
            }

            return result;
        }

        public static string EscapeReservedKeyword(string name)
        {
            string[] reserved = { "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
                "char", "checked", "class", "const", "continue", "decimal", "default", "delegate",
                "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally",
                "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int",
                "interface", "internal", "is", "lock", "long", "namespace", "new", "null",
                "object", "operator", "out", "override", "params", "private", "protected", "public",
                "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc",
                "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof",
                "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void",
                "volatile", "while" };

            if (Array.IndexOf(reserved, name) >= 0)
                return "@" + name;

            return name;
        }
    }
}
