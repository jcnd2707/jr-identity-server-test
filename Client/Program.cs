// See https://aka.ms/new-console-template for more information
using IdentityModel.Client;
using System.Net;
using System.Text.Json.Nodes;

Console.WriteLine("Hello, World!");

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001/");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}


var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest 
{ 
    Address = disco.TokenEndpoint,
    ClientId = "jr-client",
    Scope = "jr-auth",
    ClientSecret = "secret"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine(tokenResponse.Json);


var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);

System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

var response = await apiClient.GetAsync("https://localhost:6001/Identity");

if(!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{ 
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(JsonArray.Parse(content));
}

Console.ReadKey();

