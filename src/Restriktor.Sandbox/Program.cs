using System;
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
                .AllowType("System.Attribute")
                .AllowType("System.Void")
                .AllowType("System.Int32");

            var nl = Environment.NewLine;

            var result = restrictor.Validate("public class A : System.Attribute { }");

            Console.WriteLine(result.ToString());
        }
    }
}