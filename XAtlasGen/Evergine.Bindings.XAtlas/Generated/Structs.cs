using System;
using System.Runtime.InteropServices;

namespace Evergine.Bindings.XAtlas
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct xatlasChart
	{
		public uint* faceArray;
		public uint atlasIndex;
		public uint faceCount;
		public xatlasChartType type;
		public uint material;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct xatlasVertex
	{
		public int atlasIndex;
		public int chartIndex;
		public fixed float uv[2];
		public uint xref;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct xatlasMesh
	{
		public xatlasChart* chartArray;
		public uint* indexArray;
		public xatlasVertex* vertexArray;
		public uint chartCount;
		public uint indexCount;
		public uint vertexCount;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct xatlasAtlas
	{
		public uint* image;
		public xatlasMesh* meshes;
		public float* utilization;
		public uint width;
		public uint height;
		public uint atlasCount;
		public uint chartCount;
		public uint meshCount;
		public float texelsPerUnit;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct xatlasMeshDecl
	{
		public void* vertexPositionData;
		public void* vertexNormalData;
		public void* vertexUvData;
		public void* indexData;
		public byte* faceIgnoreData;
		public uint* faceMaterialData;
		public byte* faceVertexCount;
		public uint vertexCount;
		public uint vertexPositionStride;
		public uint vertexNormalStride;
		public uint vertexUvStride;
		public uint indexCount;
		public int indexOffset;
		public uint faceCount;
		public xatlasIndexFormat indexFormat;
		public float epsilon;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct xatlasUvMeshDecl
	{
		public void* vertexUvData;
		public void* indexData;
		public uint* faceMaterialData;
		public uint vertexCount;
		public uint vertexStride;
		public uint indexCount;
		public int indexOffset;
		public xatlasIndexFormat indexFormat;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct xatlasChartOptions
	{
		public  paramFunc;
		public float maxChartArea;
		public float maxBoundaryLength;
		public float normalDeviationWeight;
		public float roundnessWeight;
		public float straightnessWeight;
		public float normalSeamWeight;
		public float textureSeamWeight;
		public float maxCost;
		public uint maxIterations;
		public byte useInputMeshUvs;
		public byte fixWinding;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct xatlasPackOptions
	{
		public uint maxChartSize;
		public uint padding;
		public float texelsPerUnit;
		public uint resolution;
		public byte bilinear;
		public byte blockAlign;
		public byte bruteForce;
		public byte createImage;
		public byte rotateChartsToAxis;
		public byte rotateCharts;
	}

}

