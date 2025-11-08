# Poco2Proto

Convert simple C# POCO classes into `.proto` message definitions for gRPC, automatically.  
This library is designed to be lightweight, simple, and dependency-free and ideal for quickly generating Protobuf message contracts from your existing models.

---

## ✨ Features

- Converts public C# class properties into `.proto` message fields
- Supports `string`, numeric types, `bool`, `byte[]`
- Supports `List<T>` and arrays (`repeated` fields)
- Supports nullable types (`int?`, `bool?`, etc.)
- Converts property names to `snake_case`
- Maps `DateTime` to `string` (simple & practical)
- Zero external dependencies

---

## 📦 Installation

### NuGet Package Manager
```

Install-Package Poco2Proto

```

### .NET CLI
```

dotnet add package Poco2Proto

````

---

## 🚀 Usage Example

### C# Class
```csharp
public class UserProfile
{
    public string FullName { get; set; }
    public int? Age { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; }
    public string[] Tags { get; set; }
}
````

### Generate `.proto`

```csharp
using Poco2Proto;

string proto = Poco2Proto.GenerateProto<UserProfile>();
Console.WriteLine(proto);
// Or save to file
File.WriteAllText("UserProfile.proto", proto);
```

---

## ✅ Example Output

```
syntax = "proto3";
package YourNamespace;

message UserProfile {
  string full_name = 1;
  int32 age = 2;
  bool is_active = 3;
  repeated string roles = 4;
  repeated string tags = 5;
}
```

You can now place this `.proto` file in your gRPC project and compile it with `protoc`.

---

## 🔍 Notes & Limitations

This package is intentionally simple and works best when:

* Your classes are pure POCO DTOs
* No complex inheritance or polymorphism is needed
* You generate `.proto` definitions as a starting point and refine manually if needed

Future enhancements may include:

* Recursive nested message generation
* Enum support
* Custom type mappings

---

## 🛠️ Contributing

Pull requests are welcome!
If you'd like to extend the mapper behavior, open an issue or submit a PR.

---

## 📄 License

Check the repository.
