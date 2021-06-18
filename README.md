# Restriktor

## ⚠️ This is a work-in-progress project and it is not ready for production

__Restrict C# namespace, type and member usage from source code with the help of Roslyn__

Build and tests status (master):

![](https://github.com/vfrz/Restriktor/actions/workflows/dotnet.yml/badge.svg)

## Documentation

Example usage:
```csharp
var restrictor = new Restrictor();

restrictor.Policies.DefaultPolicyType = PolicyType.Deny;

restrictor.Policies
    .AllowNamespace("System.Encoding")
    .AllowType("System.Console")
    .AllowType("System.Void")
    .AllowType("System.Int32")
    .AllowMethod("System.Console.WriteLine(*)")
    .DenyMethod("System.Console.ReadLine(*)");

var result = restrictor.Validate(code);

Console.WriteLine(result.ToString());
```