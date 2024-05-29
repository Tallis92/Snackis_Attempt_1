using System.Text.Json;

namespace Snackis_Attempt_1.DAL
{
    public class CategoryManagerAPI
    {
        private static Uri BaseAddress = new Uri("https://tabletoptalk.azurewebsites.net/");
        public static async Task<List<Models.Category>> GetAllCategories()
        {

            List<Models.Category> categories = new List<Models.Category>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("/api/Categories");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    categories = JsonSerializer.Deserialize<List<Models.Category>>(responseString);
                }

                return categories;
            }

        }
    }
}
