using System;

namespace Evergine.Bindings.XAtlas
{
	public enum ChartType
	{
		Planar = 0,
		Ortho = 1,
		Lscm = 2,
		Piecewise = 3,
		Invalid = 4,
	}

	public enum IndexFormat
	{
		Uint16 = 0,
		Uint32 = 1,
	}

	public enum AddMeshError
	{
		Success = 0,
		Error = 1,
		Indexoutofrange = 2,
		Invalidfacevertexcount = 3,
		Invalidindexcount = 4,
	}

	public enum ProgressCategory
	{
		Addmesh = 0,
		Computecharts = 1,
		Packcharts = 2,
		Buildoutputmeshes = 3,
	}

}
