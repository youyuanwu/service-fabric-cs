# service-fabric-cs
service fabric bindings for csharp

# dependencies
* service fabric runtime installation. See [get-started](https://learn.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started)
* cmake (for c dependencies)
* [fabric-metadata](https://github.com/youyuanwu/fabric-metadata). Auto downloaded by cmake. For SF winmd files.
* dotnet

# build
Prepare env and dependencies
```ps1
cmake . -B build
cmake --build build
```
Build CSharp code
```ps1
dotnet build .\src\ServiceFabric\ServiceFabric.csproj
```