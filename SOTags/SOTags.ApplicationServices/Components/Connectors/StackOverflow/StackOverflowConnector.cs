using SOTags.DataAccess.Entities;
using System.IO.Compression;

namespace SOTags.ApplicationServices.Components.Connectors.StackOverflow
{
    public class StackOverflowConnector : IStackOverflowConnector
    {
        public async Task DownloadData()
        {
            int pageSize = 100;
            int page = 1;
            bool hasMore = true;
            HttpClient client = new HttpClient();
            var file = Config.stackOverflowJsonFile; 

            if(File.Exists(file))
            {
                File.Delete(file);
            }

            try
            {
                while (hasMore)
                {
                    string apiUrl = $"{Config.stackOverflowUrl}2.3/tags?site=stackoverflow&pagesize={pageSize}&page={page}&key={Config.apiKey}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    string responseBody = "";

                    if (response.IsSuccessStatusCode)
                    {
                        // Sprawdź nagłówek Content-Encoding
                        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                        {
                            using (Stream stream = await response.Content.ReadAsStreamAsync())
                            using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                            using (StreamReader reader = new StreamReader(gzipStream))
                            {
                                responseBody = await reader.ReadToEndAsync();
                                // Przetwórz treść odpowiedzi
                            }
                        }
                        else
                        {
                            // Obsłuż inne typy kompresji tutaj (np. Deflate)
                            using (Stream stream = await response.Content.ReadAsStreamAsync())
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                responseBody = await reader.ReadToEndAsync();
                                // Przetwórz treść odpowiedzi
                            }
                        }
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        using (var writer = File.AppendText(file))
                        {
                            writer.WriteLine(responseBody);
                        }

                        if (page >= 10)
                        {
                            break;
                        }
                        // Sprawdź czy są więcej wyników
                        if (!responseBody.Contains("\"has_more\":true"))
                        {
                            hasMore = false;
                        }

                        page++; // Przejdź do następnej strony
                    }
                    else
                    {
                        Console.WriteLine("Błąd podczas pobierania danych: " + response.ReasonPhrase);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}