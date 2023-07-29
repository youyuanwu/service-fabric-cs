using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.FabricCommon;
using Microsoft.ServiceFabric.FabricCommon.FabricClient;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

FABRIC_ERROR_CODE e = 0;

IFabricStringResult s = new myString("World");
Console.WriteLine($"Hello, {s.get_String()}!");
Console.WriteLine($"Error: {e}!");


//IFabricQueryClient c = default;

//public static class PInvoke
//{
//    public static HRESULT FabricCreateLocalClient(in Guid iid, out void* fabricClient);
//    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
//    public static HRESULT FabricCreateLocalClient(Guid* iid, void** fabricClient);
//}

IFabricQueryClient c;

unsafe {
    var guid = typeof(IFabricQueryClient).GUID;
    var clientRaw = IntPtr.Zero;
    HRESULT hr = PInvoke.FabricCreateLocalClient(&guid, (void**)&clientRaw);

    Debug.Assert(hr.Succeeded);
   
    var cc = Marshal.PtrToStructure<IFabricQueryClient>(clientRaw);
    Debug.Assert(cc != null);
    if(cc != null)
    {
        c = cc;
    }
};