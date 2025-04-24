using System;
using System.Runtime.InteropServices;

namespace Evergine.Bindings.XAtlas
{
	public unsafe delegate void xatlasParameterizeFunc(
		 float* positions,
		 float* texcoords,
		 uint vertexCount,
		 uint* indices,
		 uint indexCount);

	public unsafe delegate bool xatlasProgressFunc(
		 xatlasProgressCategory category,
		 int progress,
		 void* userData);

	public unsafe delegate void* xatlasReallocFunc(
		 void* ptr0,
		 uint size1);

	public unsafe delegate void xatlasFreeFunc(
		 void* ptr0);

	public unsafe delegate int xatlasPrintFunc(
		 byte* bytePtr0);

}
