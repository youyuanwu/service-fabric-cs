
// See https://aka.ms/new-console-template for more information
using Microsoft.ServiceFabric.FabricCommon;
using Microsoft.ServiceFabric;
using Windows.Win32.Foundation;


FABRIC_ERROR_CODE e = 0;

IFabricStringResult s = new myString("World");
Console.WriteLine($"Hello, {s.get_String()}!");
Console.WriteLine($"Error: {e}!");

class myString : IFabricStringResult
{
    public string Value { get; set; }

    public myString(string value)
    {
        this.Value = value;
    }

    public PWSTR get_String()
    {
        unsafe
        {
            fixed (char* c = Value)
            {
                return c;
            }
        }
    }
}