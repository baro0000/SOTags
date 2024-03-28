using SOTags.DataAccess;

namespace SOTags
{
    public static class DatabaseInitializer
    {
        public static async Task Initialize(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseDbContext>();

                await dbContext.Database.EnsureCreatedAsync();

                if (!dbContext.Tags.Any())
                {
                    using (var httpClient = new HttpClient())
                    {
                        var apiUrl = "https://api.stackexchange.com/2.3/tags?order=desc&sort=popular&site=stackoverflow";
                        var response = await httpClient.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();
                            // Process response data and save to the database
                            // You can use libraries like Newtonsoft.Json for JSON deserialization
                        }
                        else
                        {
                            throw new Exception("Failed to fetch data from Stack Overflow API.");
                        }
                    }
                }
            }
        }
    }
}
