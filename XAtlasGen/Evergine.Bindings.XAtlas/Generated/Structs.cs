using System;
using System.Runtime.InteropServices;

namespace Evergine.Bindings.XAtlas
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Chart
	{
		public uint* FaceArray;
		public uint AtlasIndex;
		public uint FaceCount;
		public ChartType Type;
		public uint Material;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Vertex
	{
		public int AtlasIndex;
		public int ChartIndex;
		public fixed float Uv[2];
		public uint Xref;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Mesh
	{
		public Chart* ChartArray;
		public uint* IndexArray;
		public Vertex* VertexArray;
		public uint ChartCount;
		public uint IndexCount;
		public uint VertexCount;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Atlas
	{
		public uint* Image;
		public Mesh* Meshes;
		public float* Utilization;
		public uint Width;
		public uint Height;
		public uint AtlasCount;
		public uint ChartCount;
		public uint MeshCount;
		public float TexelsPerUnit;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MeshDecl
	{
		public void* VertexPositionData;
		public void* VertexNormalData;
		public void* VertexUvData;
		public void* IndexData;
		public byte* FaceIgnoreData;
		public uint* FaceMaterialData;
		public byte* FaceVertexCount;
		public uint VertexCount;
		public uint VertexPositionStride;
		public uint VertexNormalStride;
		public uint VertexUvStride;
		public uint IndexCount;
		public int IndexOffset;
		public uint FaceCount;
		public IndexFormat IndexFormat;
		public float Epsilon;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct UvMeshDecl
	{
		public void* VertexUvData;
		public void* IndexData;
		public uint* FaceMaterialData;
		public uint VertexCount;
		public uint VertexStride;
		public uint IndexCount;
		public int IndexOffset;
		public IndexFormat IndexFormat;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct ChartOptions
	{
		public IntPtr ParamFunc;
		public float MaxChartArea;
		public float MaxBoundaryLength;
		public float NormalDeviationWeight;
		public float RoundnessWeight;
		public float StraightnessWeight;
		public float NormalSeamWeight;
		public float TextureSeamWeight;
		public float MaxCost;
		public uint MaxIterations;
		public byte UseInputMeshUvs;
		public byte FixWinding;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct PackOptions
	{
		public uint MaxChartSize;
		public uint Padding;
		public float TexelsPerUnit;
		public uint Resolution;
		public byte Bilinear;
		public byte BlockAlign;
		public byte BruteForce;
		public byte CreateImage;
		public byte RotateChartsToAxis;
		public byte RotateCharts;
	}

}

