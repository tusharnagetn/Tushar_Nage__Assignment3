using Microsoft.Azure.Cosmos;

namespace Library_Management_System
{
    public class DBConfig
    {
        private readonly string URI = "https://localhost:8081";
        private readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private readonly string DatabaseName = "Library";
        private readonly string ContainerName = "Container1";

        public Container GetContainer()
        {
            CosmosClient cosmosClient = new CosmosClient(URI, PrimaryKey);
            Database database = cosmosClient.GetDatabase(DatabaseName);
            Container container = database.GetContainer(ContainerName);
            return container;
        }
    }
}
