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
                .AddNamespacePolicy("System", PolicyType.Neutral)
                .AddNamespacePolicy("System.Reflection", PolicyType.Allow)
                .AddNamespacePolicy("System.Runtime.InteropServices", PolicyType.Allow)
                .AllowMethod("System.Console.WriteLine(System.String)");

            var nl = Environment.NewLine;

            var result = restrictor.Validate("using System; using System.Runtime.InteropServices; [DllImport(\"test.so\")] public static extern void B();");

            Console.WriteLine(result.ToString());
        }
    }
}