using System;
using System.Runtime.InteropServices;

namespace Evergine.Bindings.XAtlas
{
	public static unsafe partial class XAtlasNative
	{
		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern xatlasAtlas* xatlasCreate();

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasDestroy(xatlasAtlas* atlas);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern xatlasAddMeshError xatlasAddMesh(xatlasAtlas* atlas, xatlasMeshDecl* meshDecl, uint meshCountHint);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasAddMeshJoin(xatlasAtlas* atlas);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern xatlasAddMeshError xatlasAddUvMesh(xatlasAtlas* atlas, xatlasUvMeshDecl* decl);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasComputeCharts(xatlasAtlas* atlas, xatlasChartOptions* chartOptions);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasPackCharts(xatlasAtlas* atlas, xatlasPackOptions* packOptions);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasGenerate(xatlasAtlas* atlas, xatlasChartOptions* chartOptions, xatlasPackOptions* packOptions);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasSetProgressCallback(xatlasAtlas* atlas, xatlasProgressFunc progressFunc, void* progressUserData);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasSetAlloc(xatlasReallocFunc reallocFunc, xatlasFreeFunc freeFunc);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasSetPrint(xatlasPrintFunc print, [MarshalAs(UnmanagedType.Bool)] bool verbose);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte* xatlasAddMeshErrorString(xatlasAddMeshError error);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte* xatlasProgressCategoryString(xatlasProgressCategory category);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasMeshDeclInit(xatlasMeshDecl* meshDecl);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasUvMeshDeclInit(xatlasUvMeshDecl* uvMeshDecl);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasChartOptionsInit(xatlasChartOptions* chartOptions);

		[DllImport("xatlas", CallingConvention = CallingConvention.Cdecl)]
		public static extern void xatlasPackOptionsInit(xatlasPackOptions* packOptions);

	}
}
