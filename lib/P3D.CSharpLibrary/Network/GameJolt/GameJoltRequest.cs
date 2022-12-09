namespace P3D.ExternalLibrary.Network.GameJolt;

public class GameJoltRequest
{
    public string Endpoint { get; }
    
    public IEnumerable<KeyValuePair<string, string>> GetParameters { get; }
    
    public IEnumerable<KeyValuePair<string, string>> PostParameters { get; }

    public GameJoltRequest(string endpoint, IEnumerable<KeyValuePair<string, string>> getParameters, IEnumerable<KeyValuePair<string, string>> postParameters)
    {
        Endpoint = endpoint;
        GetParameters = getParameters;
        PostParameters = postParameters;
    }
}