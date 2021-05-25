using System;
using System.Reflection;
using Restriktor.Policies;

namespace Restriktor.Sandbox
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var restrictor = new Restrictor();

            restrictor.Policies.DefaultPolicyType = PolicyType.Allow;
            
            restrictor.Policies
                .AllowNamespace("System")
                .DenyType(typeof(Assembly));

            var nl = Environment.NewLine;

            var result = restrictor.Validate("using System; using System.Reflection; public void B() { Assembly a; }");

            Console.WriteLine(result.ToString());
        }
    }
}