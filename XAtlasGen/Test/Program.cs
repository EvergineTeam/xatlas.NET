using Evergine.Bindings.XAtlas;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;
using OBJRuntime.DataTypes;
using OBJRuntime.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Test
{
    internal unsafe class Program
    {
        static void Main(string[] args)
        {
            // This will not be needed when we use a nuget package
            NativeLibrary.SetDllImportResolver(typeof(Evergine.Bindings.XAtlas.xatlasMesh).Assembly, ResolveRuntimes);

            Console.WriteLine("Hello, xatlas!");

            // Read obj file
            var attrib = new OBJAttrib();
            var shapes = new List<OBJShape>();
            var materials = new List<OBJMaterial>();
            var warning = string.Empty;
            var error = string.Empty;
            using (var stream = new FileStream("horse_statue.obj", FileMode.Open, FileAccess.Read))
            using (var srObj = new StreamReader(stream))
            {
                bool success = OBJLoader.Load(srObj, ref attrib, shapes, materials, ref warning, ref error, null, string.Empty, true, true);
                if (!success)
                {
                    throw new Exception($"OBJ Load failed. Error:{error}");
                }
            }

            // Create empty atlas
            xatlasAtlas* atlas = XAtlasNative.xatlasCreate();

            // Add mesh to atlas.
            Vector3[] positions = attrib.Vertices.ToArray();
            float* pVertices = (float*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(positions));
            Vector3[] normals = attrib.Normals.ToArray();
            float* pNormals = (float*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(normals));
            Vector2[] texcoords = attrib.Texcoords.ToArray();
            float* pTexCoords = (float*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(texcoords));

            var mesh = shapes[0].Mesh;
            uint meshNumIndices = (uint)mesh.Indices.Count;
            uint[] indices = new uint[meshNumIndices];
            for (int i = 0; i < meshNumIndices; i++)
            {
                indices[i] = (uint)mesh.Indices[i].VertexIndex;
            }
            uint* pIndices = (uint*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(indices));

            xatlasMeshDecl meshDecl;
            XAtlasNative.xatlasMeshDeclInit(&meshDecl);
            meshDecl.vertexCount = (uint)attrib.Vertices.Count;
            meshDecl.vertexPositionData = pVertices;
            meshDecl.vertexPositionStride = 3 * sizeof(float);
            if (attrib.Normals.Count > 0)
            {
                meshDecl.vertexNormalData = pNormals;
                meshDecl.vertexNormalStride = 3 * sizeof(float);
            }
            if (attrib.Texcoords.Count > 0)
            {
                meshDecl.vertexUvData = pTexCoords;
                meshDecl.vertexUvStride = 2 * sizeof(float);
            }
            meshDecl.indexCount = meshNumIndices;
            meshDecl.indexData = pIndices;
            meshDecl.indexFormat = xatlasIndexFormat.XATLAS_INDEX_FORMAT_UINT32;

            xatlasAddMeshError xError = XAtlasNative.xatlasAddMesh(atlas, &meshDecl, 1);
            if (xError != xatlasAddMeshError.XATLAS_ADD_MESH_ERROR_SUCCESS)
            {
                XAtlasNative.xatlasDestroy(atlas);
                Console.WriteLine("Error adding mesh.");
                return;
            }

            XAtlasNative.xatlasAddMeshJoin(atlas); // Not necessary. Only called here so geometry totals are printed after the AddMesh progress indicator.
            Console.WriteLine($"   {meshDecl.vertexCount} total vertices");
            Console.WriteLine($"   {meshDecl.indexCount / 3} total faces");

            // Generate atlas.
            Console.WriteLine($"Generating atlas");
            XAtlasNative.xatlasGenerate(atlas, null, null);
            Console.WriteLine($"   {atlas->chartCount} charts");
            Console.WriteLine($"   {atlas->atlasCount} atlases");
            for (uint i = 0; i < atlas->atlasCount; i++)
                Console.WriteLine($"      {i}: {atlas->utilization[i] * 100.0f}% utilization");
            Console.WriteLine($"   {atlas->width} {atlas->height} resolution");
            Console.WriteLine($"   {atlas->meshes[0].vertexCount} total vertices");

            // Cleanup.
            XAtlasNative.xatlasDestroy(atlas);
            Console.WriteLine("Done.");
        }

        private static IntPtr ResolveRuntimes(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            var rid = RuntimeInformation.RuntimeIdentifier;
            var runtimesFolder = Path.Combine(Path.GetDirectoryName(assembly.Location), $@"runtimes/{rid}/native");
            if (Directory.Exists(runtimesFolder))
            {
                var files = Directory.GetFiles(runtimesFolder, $"*{libraryName}.*");
                foreach (var file in files)
                {
                    if (NativeLibrary.TryLoad(file, out var handle))
                    {
                        return handle;
                    }
                }
            }

            return NativeLibrary.Load(libraryName, assembly, searchPath);
        }
    }
}
