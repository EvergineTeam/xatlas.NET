using System;
using System.Runtime.InteropServices;

namespace Evergine.Bindings.XAtlas
{
	public static unsafe partial class XAtlas
	{
		[DllImport("xatlas", EntryPoint = "xatlasCreate", CallingConvention = CallingConvention.Cdecl)]
		public static extern Atlas* Create();

		[DllImport("xatlas", EntryPoint = "xatlasDestroy", CallingConvention = CallingConvention.Cdecl)]
		public static extern void Destroy(Atlas* atlas);

		[DllImport("xatlas", EntryPoint = "xatlasAddMesh", CallingConvention = CallingConvention.Cdecl)]
		public static extern AddMeshError AddMesh(Atlas* atlas, MeshDecl* meshDecl, uint meshCountHint);

		[DllImport("xatlas", EntryPoint = "xatlasAddMeshJoin", CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddMeshJoin(Atlas* atlas);

		[DllImport("xatlas", EntryPoint = "xatlasAddUvMesh", CallingConvention = CallingConvention.Cdecl)]
		public static extern AddMeshError AddUvMesh(Atlas* atlas, UvMeshDecl* decl);

		[DllImport("xatlas", EntryPoint = "xatlasComputeCharts", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ComputeCharts(Atlas* atlas, ChartOptions* chartOptions);

		[DllImport("xatlas", EntryPoint = "xatlasPackCharts", CallingConvention = CallingConvention.Cdecl)]
		public static extern void PackCharts(Atlas* atlas, PackOptions* packOptions);

		[DllImport("xatlas", EntryPoint = "xatlasGenerate", CallingConvention = CallingConvention.Cdecl)]
		public static extern void Generate(Atlas* atlas, ChartOptions* chartOptions, PackOptions* packOptions);

		[DllImport("xatlas", EntryPoint = "xatlasSetProgressCallback", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetProgressCallback(Atlas* atlas, ProgressFunc progressFunc, void* progressUserData);

		[DllImport("xatlas", EntryPoint = "xatlasSetAlloc", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetAlloc(ReallocFunc reallocFunc, FreeFunc freeFunc);

		[DllImport("xatlas", EntryPoint = "xatlasSetPrint", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetPrint(PrintFunc print, [MarshalAs(UnmanagedType.Bool)] bool verbose);

		[DllImport("xatlas", EntryPoint = "xatlasAddMeshErrorString", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte* AddMeshErrorString(AddMeshError error);

		[DllImport("xatlas", EntryPoint = "xatlasProgressCategoryString", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte* ProgressCategoryString(ProgressCategory category);

		[DllImport("xatlas", EntryPoint = "xatlasMeshDeclInit", CallingConvention = CallingConvention.Cdecl)]
		public static extern void MeshDeclInit(MeshDecl* meshDecl);

		[DllImport("xatlas", EntryPoint = "xatlasUvMeshDeclInit", CallingConvention = CallingConvention.Cdecl)]
		public static extern void UvMeshDeclInit(UvMeshDecl* uvMeshDecl);

		[DllImport("xatlas", EntryPoint = "xatlasChartOptionsInit", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ChartOptionsInit(ChartOptions* chartOptions);

		[DllImport("xatlas", EntryPoint = "xatlasPackOptionsInit", CallingConvention = CallingConvention.Cdecl)]
		public static extern void PackOptionsInit(PackOptions* packOptions);

	}
}
