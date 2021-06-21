using System;
using Restriktor.Core;
using Restriktor.Policies;

namespace Restriktor.Sandbox
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var restrictor = new Restrictor();

            restrictor.Policies.DefaultPolicyType = PolicyType.Deny;

            restrictor.Policies
                .AllowNamespace("System.Encoding")
                .AllowType("System.Console")
                .AllowType("System.Void")
                .AllowType("System.Int32")
                .AllowMethod("System.Console.WriteLine(*)")
                .DenyMethod("System.Console.ReadLine(*)");

            var result = restrictor.Validate("public class A : System.Attribute { }");

            Console.WriteLine(result.ToString());
        }
    }
}