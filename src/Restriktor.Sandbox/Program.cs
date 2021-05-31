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
                .AllowType("System.Void")
                .AllowType("System.Int32");

            var nl = Environment.NewLine;

            var result = restrictor.Validate("public class A { public void B(int a) {} }");

            Console.WriteLine(result.ToString());
        }
    }
}