namespace FluentDynamoDb.Extensions
{
    public static class StringExtensions
    {
        public static string ConvertToCamelCaseUnderscore(this string propertyName)
        {
            return "_" + propertyName[0].ToString().ToLower() + propertyName.Substring(1);
        }
    }
}
