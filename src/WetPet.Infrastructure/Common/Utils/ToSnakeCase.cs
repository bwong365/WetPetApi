using System.Text.RegularExpressions;

namespace WetPet.Infrastructure.Common.Utils;

public static class ToSnakeCaseExtension
{
    public static string ToSnakeCase(this string str) => Regex.Replace(str, "(\\B)([A-Z])", "_$2").ToLower();
}