using System.Reflection;

namespace FluentDynamoDb.Configuration
{
    public static class FluentDynamoDbConfiguration
    {
        internal static Assembly ClassMapLocationAssembly { get; set; }

        public static void Configure(Assembly classMapLocationAssembly)
        {
            ClassMapLocationAssembly = classMapLocationAssembly;
        }
    }
}