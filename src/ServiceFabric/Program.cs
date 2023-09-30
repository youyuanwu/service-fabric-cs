
// See https://aka.ms/new-console-template for more information
using Microsoft.ServiceFabric.FabricCommon;
using Windows.Win32.Foundation;

public class myString : IFabricStringResult
{
    public string Value { get; set; }

    public myString(string value)
    {
        this.Value = value;
    }

    public PCWSTR get_String()
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