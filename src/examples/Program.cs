using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.FabricCommon;
using Microsoft.ServiceFabric.FabricCommon.FabricClient;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
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

// load library

var h = NativeLibrary.Load("C:\\Program Files\\Microsoft Service Fabric\\bin\\Fabric\\Fabric.Code\\FabricClient.dll");
if (h == IntPtr.Zero)
{
    Debug.Fail("Fail to load.");
}

IFabricQueryClient c = Client.CreateClient();


class Client
{
    public static IFabricQueryClient CreateClient()
    {

        var guidgen = typeof(IFabricQueryClient).GUID; //This does not work.
        var guid = Guid.Parse("c629e422-90ba-4efd-8f64-cecf51bc3df0");

        // Not equal c629e422-90ba-4efd-8f64-cecf51bc3df0 and 872b8dc7-d6af-3a59-9ecc-2edbb26e1ffc
        //Debug.Assert(Debug.Equals(guid, guidgen), $"Not equal {guid} and {guidgen}");

        unsafe
        {
            var clientRaw = IntPtr.Zero;
            HRESULT hr = PInvoke.FabricCreateLocalClient(&guid, (void**)&clientRaw);

            Debug.Assert(hr.Succeeded, $"Failed {hr}");

            var unknown = Marshal.GetObjectForIUnknown(clientRaw);

            // casting does not work since the guid generated is wrong.
            var c = (IFabricQueryClient)unknown;
            //Unhandled exception. System.InvalidCastException: Unable to cast COM object of type 'System.__ComObject' to interface type 'Microsoft.ServiceFabric.FabricCommon.FabricClient.IFabricQueryClient'. This operation failed because the QueryInterface call on the COM component for the interface with IID '{872B8DC7-D6AF-3A59-9ECC-2EDBB26E1FFC}' failed due to the following error: No such interface supported (0x80004002 (E_NOINTERFACE)).

            return c;
        };
    }
}
