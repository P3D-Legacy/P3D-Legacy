using System.Text;
using System.Text.Json;

namespace P3D.CSharpLibrary.Json;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        var result = new StringBuilder();
        var nameChars = name.AsSpan();

        for (var i = 0; i < nameChars.Length; i++)
        {
            var c = nameChars[i];

            if (i > 0 && char.IsUpper(c))
            {
                result.Append($"_{char.ToLowerInvariant(c)}");
            }
            else
            {
                result.Append(char.ToLowerInvariant(c));
            }
        }
        
        return result.ToString();
    }
}