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

// load library

var h = NativeLibrary.Load("C:\\Program Files\\Microsoft Service Fabric\\bin\\Fabric\\Fabric.Code\\FabricClient.dll");
if (h == IntPtr.Zero)
{
    Debug.Fail("Fail to load.");
}

IFabricQueryClient c = Client.CreateClient();

WaitCallback callback = new();

FABRIC_NODE_QUERY_DESCRIPTION desc = new();
var ctx = c.BeginGetNodeList(desc, 1000, callback);
Debug.Assert(ctx != null);
callback.Wait();
var nodeRes = c.EndGetNodeList(ctx);
Debug.Assert(nodeRes != null);

unsafe
{
    var nodelist = nodeRes.get_NodeList();
    var num = nodelist->Count;
    Console.WriteLine($"NodeCount: {num}.");
    for (int i = 0; i < num; i++)
    {
        var item = nodelist->Items[i];
        Console.WriteLine($"NodeName {item.NodeName}, NodeType {item.NodeType}, Status: {item.NodeStatus}");
    }
}

class Client
{
    // Create the localclient
    public static IFabricQueryClient CreateClient()
    {

        var guidgen = typeof(IFabricQueryClient).GUID;
        unsafe
        {
            var clientRaw = IntPtr.Zero;
            HRESULT hr = PInvoke.FabricCreateLocalClient(&guidgen, (void**)&clientRaw);

            Debug.Assert(hr.Succeeded, $"Failed {hr}");

            var unknown = Marshal.GetObjectForIUnknown(clientRaw);

            var c = (IFabricQueryClient)unknown;
            
            return c;
        };
    }
}

class WaitCallback : IFabricAsyncOperationCallback
{
    public WaitCallback() { 
        m_event = new ManualResetEvent(false);
    }

    public void Invoke(IFabricAsyncOperationContext context)
    {
        Debug.Assert(context != null);
        bool ok = m_event.Set();
        Debug.Assert(ok);
    }

    public void Wait()
    {
        bool ok = m_event.WaitOne();
        Debug.Assert(ok);
    }

    private ManualResetEvent m_event;
}