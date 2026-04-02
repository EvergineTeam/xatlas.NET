using System;
using System.Runtime.InteropServices;

namespace Evergine.Bindings.XAtlas
{
	public unsafe delegate void ParameterizeFunc(
		 float* positions,
		 float* texcoords,
		 uint vertexCount,
		 uint* indices,
		 uint indexCount);

	public unsafe delegate bool ProgressFunc(
		 ProgressCategory category,
		 int progress,
		 void* userData);

	public unsafe delegate void* ReallocFunc(
		 void* ptr0,
		 nuint size1);

	public unsafe delegate void FreeFunc(
		 void* ptr0);

	public unsafe delegate int PrintFunc(
		 byte* bytePtr0);

}
