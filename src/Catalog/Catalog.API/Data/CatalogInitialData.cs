using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync())
                return;

            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
        {
            new Product
            {
                Id = new Guid("0199cc0d-c1c8-4299-b8c5-e9114ef96e99"),
                Name = "IPhone X",
                Category = new List<string>{ "Smart Phone" },
                Description = "Description IPhone X",
                ImageFile = "poduct-1.png",
                Price = 950M
            },
            new Product
            {
                Id = new Guid("0199cc0e-a7f8-40b1-90da-ba6caeaca5b3"),
                Name = "Samsung 10",
                Category = new List<string>{ "Smart Phone" },
                Description = "Description Samsung 10",
                ImageFile = "poduct-2.png",
                Price = 840M
            }
        };
       
    }
}
