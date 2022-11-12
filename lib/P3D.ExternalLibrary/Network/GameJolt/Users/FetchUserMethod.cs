namespace P3D.ExternalLibrary.Network.GameJolt.Users;

public class FetchUserMethod
{
    private readonly string? _username;
    private readonly long[]? _userIds;

    public FetchUserMethod(string username)
    {
        _username = username;
    }

    public FetchUserMethod(long userId)
    {
        _userIds = new[] { userId };
    }

    public FetchUserMethod(long[] userIds)
    {
        _userIds = userIds;
    }

    public GameJoltRequest[] GenerateRequests()
    {
        var result = new List<GameJoltRequest>();
        var postParameter = new SortedDictionary<string, string>();
        var parameterSize = 0;
        
        if (_username != null)
        {
            if (!TryAddParameter(postParameter, "username", _username, ref parameterSize))
            {
                // Error as username is too long, this request cannot be executed.
            }
        }
        else if (_userIds != null)
        {
            var userIdValue = string.Join(",", _userIds.Select(a => a.ToString("D")));
            
            if (!TryAddParameter(postParameter, "user_id", userIdValue, ref parameterSize))
            {
                // We can split this into multiple request.
                var currentPointer = userIdValue.AsSpan();

                while (!currentPointer.IsEmpty)
                {
                    var validChunk = currentPointer[..(GameJoltApi.MaxPostSize - 8 - parameterSize)].LastIndexOf(',');

                    postParameter.Add("user_id", currentPointer[..validChunk].ToString());
                    result.Add(new GameJoltRequest("/users/", Array.Empty<KeyValuePair<string, string>>(), postParameter));
                    
                    currentPointer = currentPointer[(validChunk + 1)..];
                    postParameter = new SortedDictionary<string, string>();
                    parameterSize = 0;
                }

                return result.ToArray();
            }
        }

        result.Add(new GameJoltRequest("/users/", Array.Empty<KeyValuePair<string, string>>(), postParameter));
        return result.ToArray();
    }

    public static int CalculateParameterSize(string key, string value)
    {
        return 0;
        //return GameJoltRequest.EscapeValue(key).Length + 1 + GameJoltRequest.EscapeValue(value).Length;
    }
    
    private static bool TryAddParameter(IDictionary<string, string> parameter, string key, string value, ref int parameterSize)
    {
        var totalSize = CalculateParameterSize(key, value) + (parameter.Count > 0 ? 1 : 0);
        if (parameterSize + totalSize > GameJoltApi.MaxPostSize) return false;
        
        parameterSize += totalSize;
        parameter.Add(key, value);
        
        return true;
    }
}