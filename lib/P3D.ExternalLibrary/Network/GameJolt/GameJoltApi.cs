using System.Buffers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using P3D.ExternalLibrary.Json;

namespace P3D.ExternalLibrary.Network.GameJolt;

public sealed class GameJoltApi
{
    public const int MaxPostSize = 1 * 1024 * 1024;
    
    public string GameId { get; }

    public string GameKey { get; }

    private const string Host = "https://api.gamejolt.com/api/game/v1_2";

    private HttpClient _httpClient;
    private JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() };

    public GameJoltApi(string gameId, string gameKey, HttpClient? httpClient = null)
    {
        GameId = gameId;
        GameKey = gameKey;

        _httpClient = httpClient ?? new HttpClient();
    }

    private async Task ExecuteRequestsAsync(GameJoltRequest[] requests, bool isParallel = false, bool breakOnError = false)
    {
        var responses = new List<Task<HttpResponseMessage>>();

        foreach (var chunk in requests.Chunk(50))
        {
            var postData = new StringBuilder();
            var additionalData = new StringBuilder();

            if (chunk.Length == 1)
            {
                // Single request mode.
                //foreach (var parameter in chunk[0].Parameters)
                //{
                //    var key = Uri.EscapeDataString(parameter.Key).Replace("%20", "+");
                //    var value = Uri.EscapeDataString(parameter.Value).Replace("%20", "+");

                //    postData.Append($"{key}={value}&");

                //    additionalData.Append(key);
                //    additionalData.Append(value);
                //}

                //var postData = new FormUrlEncodedContent(chunk[0].Parameters);

                //var requestUri = BuildHashedRequestUri(chunk[0], additionalData.ToString(), true);
                //Debug.Print(requestUri);

                //responses.Add(_httpClient.PostAsync(requestUri, postData));
            }
            // Batch request mode.
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string EscapeValue(string value)
    {
        return Uri.EscapeDataString(value).Replace("%20", "+");
    }

    [SkipLocalsInit]
    private string BuildRequestUri(string endpoint, IEnumerable<KeyValuePair<string, string>> getParameters, IEnumerable<KeyValuePair<string, string>> postParameters)
    {
        var uriBuilder = new StringBuilder();
        uriBuilder.Append($"{endpoint}?");

        foreach (var (key, value) in getParameters)
        {
            uriBuilder.Append($"{EscapeValue(key)}={EscapeValue(value)}&");
        }

        if (uriBuilder[^1] == '&')
        {
            uriBuilder.Remove(uriBuilder.Length - 1, 1);
        }
        
        var additionalDataLength = 0;

        foreach (var (key, value) in postParameters)
        {
            additionalDataLength += key.Length;
            additionalDataLength += value.Length;

            uriBuilder.Append(key);
            uriBuilder.Append(value);
        }

        additionalDataLength += GameKey.Length;
        uriBuilder.Append(GameKey);

        var uri = uriBuilder.ToString();
        var uriBytes = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetByteCount(uri));
        Span<byte> hash = stackalloc byte[16];

        try
        {
            Encoding.UTF8.GetBytes(uri, uriBytes);
            MD5.HashData(uriBytes, hash);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(uriBytes);
        }

        uriBuilder.Remove(uriBuilder.Length - additionalDataLength, additionalDataLength);
        uriBuilder.Append(uriBuilder[^1] == '?' ? "signature=" : "&signature=");

        foreach (var h in hash)
        {
            uriBuilder.Append($"{h:x2}");
        }

        return uriBuilder.ToString();
    }
}