using System.Text;
using Newtonsoft.Json;

DotNetEnv.Env.Load();

if (args.Length > 0)
{
  HttpClient client = new HttpClient();

  var secret = System.Environment.GetEnvironmentVariable("OPENAI_API_KEY");

  client.DefaultRequestHeaders.Add("authorization", $"Bearer {secret} ");

  var content = new StringContent
  (
      "{\"model\": \"text-davinci-001\", \"prompt\": \"" + args[0] + "\",\"temperature\": 1,\"max_tokens\": 100 }",
      Encoding.UTF8,
      "application/json"
  );

  HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

  string responseString = await response.Content.ReadAsStringAsync();

  try
  {
    var data = JsonConvert.DeserializeObject<dynamic>(responseString);

    string guess = GuessCommand(data!.choices[0].text);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(guess);
    Console.ResetColor();
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Não foi possível desserializar o JSON: {ex.Message}");
  }

}
else Console.WriteLine("Você precisa passar um argumento para o programa");

static string GuessCommand(string raw)
{
  Console.ForegroundColor = ConsoleColor.Yellow;

  var lastINdex = raw.LastIndexOf('\n');

  string guess = raw.Substring(lastINdex + 1);
  Console.ResetColor();

  return guess;
}
