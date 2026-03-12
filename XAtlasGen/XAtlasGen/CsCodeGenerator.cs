using CppAst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace XAtlasGen
{
    public static class CsCodeGenerator
    {
        public static void Generate(CppCompilation compilation, string outputPath)
        {
            Helpers.TypedefList = compilation.Typedefs
                    .Where(t => t.TypeKind == CppTypeKind.Typedef
                           && t.ElementType is CppPointerType
                           && ((CppPointerType)t.ElementType).ElementType.TypeKind != CppTypeKind.Function)
                    .Select(t => t.Name).ToList();

            GenerateEnums(compilation, outputPath);
            GenerateDelegates(compilation, outputPath);
            GenerateStructs(compilation, outputPath);
            GenerateFunctions(compilation, outputPath);
        }

        public static void GenerateEnums(CppCompilation compilation, string outputPath)
        {
            Debug.WriteLine("Generating Enums...");

            using (StreamWriter file = File.CreateText(Path.Combine(outputPath, "Enums.cs")))
            {
                file.WriteLine("using System;\n");
                file.WriteLine("namespace Evergine.Bindings.XAtlas");
                file.WriteLine("{");

                var enums = compilation.Enums.Where(e => e.Items.Count > 0 && !e.IsAnonymous).ToList();

                foreach (var cppEnum in enums)
                {
                    Helpers.PrintComments(file, cppEnum.Comment, "\t");
                    if (compilation.Typedefs.Any(t => t.Name == cppEnum.Name + "Flags"))
                    {
                        file.WriteLine("\t[Flags]");
                    }

                    file.WriteLine($"\tpublic enum {cppEnum.Name}");
                    file.WriteLine("\t{");

                    foreach (var member in cppEnum.Items)
                    {
                        Helpers.PrintComments(file, member.Comment, "\t\t", true);
                        file.WriteLine($"\t\t{member.Name} = {member.Value},");
                    }

                    file.WriteLine("\t}\n");
                }

                file.WriteLine("}");
            }
        }

        private static void GenerateDelegates(CppCompilation compilation, string outputPath)
        {
            Debug.WriteLine("Generating Delegates...");

            var delegates = compilation.Typedefs
                .Where(t => t.TypeKind == CppTypeKind.Typedef
                       && t.ElementType is CppPointerType
                       && ((CppPointerType)t.ElementType).ElementType.TypeKind == CppTypeKind.Function)
                .ToList();

            using (StreamWriter file = File.CreateText(Path.Combine(outputPath, "Delegates.cs")))
            {
                file.WriteLine("using System;");
                file.WriteLine("using System.Runtime.InteropServices;\n");
                file.WriteLine("namespace Evergine.Bindings.XAtlas");
                file.WriteLine("{");

                foreach (var funcPointer in delegates)
                {
                    Helpers.PrintComments(file, funcPointer.Comment, "\t");
                    CppFunctionType pointerType = ((CppPointerType)funcPointer.ElementType).ElementType as CppFunctionType;

                    var returnType = Helpers.ConvertToCSharpType(pointerType.ReturnType);
                    returnType = Helpers.ShowAsMarshalType(returnType, Helpers.Family.ret);
                    file.Write($"\tpublic unsafe delegate {returnType} {funcPointer.Name}(");

                    if (pointerType.Parameters.Count > 0)
                    {
                        file.Write("\n");

                        for (int i = 0; i < pointerType.Parameters.Count; i++)
                        {
                            if (i > 0)
                                file.Write(",\n");

                            var parameter = pointerType.Parameters[i];
                            var convertedType = Helpers.ConvertToCSharpType(parameter.Type);
                            convertedType = Helpers.ShowAsMarshalType(convertedType, Helpers.Family.param);
                            var validName = Helpers.ValidParamName(parameter.Name, convertedType, i);
                            file.Write($"\t\t {convertedType} {validName}");
                        }
                    }

                    file.Write(");\n\n");
                }

                file.WriteLine("}");
            }
        }

        private static void GenerateStructs(CppCompilation compilation, string outputPath)
        {
            Debug.WriteLine("Generating Structs...");

            using (StreamWriter file = File.CreateText(Path.Combine(outputPath, "Structs.cs")))
            {
                file.WriteLine("using System;");
                file.WriteLine("using System.Runtime.InteropServices;\n");
                file.WriteLine("namespace Evergine.Bindings.XAtlas");
                file.WriteLine("{");

                var structs = compilation.Classes.Where(c => c.ClassKind == CppClassKind.Struct && c.IsDefinition == true);

                foreach (var structure in structs)
                {
                    Helpers.PrintComments(file, structure.Comment, "\t");
                    file.WriteLine("\t[StructLayout(LayoutKind.Sequential)]");
                    file.WriteLine($"\tpublic unsafe struct {structure.Name}");
                    file.WriteLine("\t{");
                    foreach (var member in structure.Fields)
                    {
                        Helpers.PrintComments(file, member.Comment, "\t\t", true);
                        string type = Helpers.ConvertToCSharpType(member.Type);
                        type = Helpers.ShowAsMarshalType(type, Helpers.Family.field);
                        // Check if this is an array
                        if (member.Type is CppArrayType)
                        {
                            int count = (member.Type as CppAst.CppArrayType).Size;
                            file.WriteLine($"\t\tpublic fixed {type} {member.Name}[{count}];");
                        }
                        else // default case
                        {

                            file.WriteLine($"\t\tpublic {type} {member.Name};");
                        }
                    }

                    file.WriteLine("\t}\n");
                }
                file.WriteLine("}\n");
            }
        }

        private static void GenerateFunctions(CppCompilation compilation, string outputPath)
        {
            Debug.WriteLine("Generating Functions...");

            using (StreamWriter file = File.CreateText(Path.Combine(outputPath, "Functions.cs")))
            {
                file.WriteLine("using System;");
                file.WriteLine("using System.Runtime.InteropServices;\n");
                file.WriteLine($"namespace Evergine.Bindings.XAtlas");
                file.WriteLine("{");
                file.WriteLine($"\tpublic static unsafe partial class XAtlasNative");
                file.WriteLine("\t{");

                foreach (var cppFunction in compilation.Functions)
                {
                    if ((cppFunction.Flags & CppFunctionFlags.FunctionTemplate) != CppFunctionFlags.None) continue;
                    if ((cppFunction.Flags & CppFunctionFlags.Inline) != CppFunctionFlags.None) continue;
                    if (cppFunction.Name == "meshopt_setAllocator") continue;

                    Helpers.PrintComments(file, cppFunction.Comment, "\t\t");
                    file.WriteLine($"\t\t[DllImport(\"xatlas\", CallingConvention = CallingConvention.Cdecl)]");
                    string returnType = Helpers.ConvertToCSharpType(cppFunction.ReturnType);
                    returnType = Helpers.ShowAsMarshalType(returnType, Helpers.Family.ret);
                    file.Write($"\t\tpublic static extern {returnType} {cppFunction.Name}(");
                    for (int i = 0; i < cppFunction.Parameters.Count; i++)
                    {
                        if (i > 0)
                            file.Write(", ");

                        var parameter = cppFunction.Parameters[i];
                        var convertedType = Helpers.ConvertToCSharpType(parameter.Type);
                        convertedType = Helpers.ShowAsMarshalType(convertedType, Helpers.Family.param);
                        file.Write($"{convertedType} {parameter.Name}");
                    }
                    file.WriteLine(");\n");
                }
                file.WriteLine("\t}\n}");
            }
        }
    }
}
