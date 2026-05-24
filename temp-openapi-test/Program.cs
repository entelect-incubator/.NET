using System;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi;
var asm = typeof(OpenApiDocument).Assembly;
foreach (var t in asm.GetTypes().Where(t => t.Namespace != null && t.Namespace.StartsWith("Microsoft.OpenApi")).OrderBy(t => t.FullName))
{
    Console.WriteLine(t.FullName);
}
