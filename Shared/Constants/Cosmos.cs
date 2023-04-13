namespace Cosmos.Example.Shared.Constants;

public static class Cosmos
{
    public const string DATABASE_NAME = "cosmicworks";

    public const string PEOPLE_CONTAINER_NAME = "people";

    public const string PEOPLE_CONTAINER_PARTITION_KEY_PATH = "/lastName";

    public const string PRODUCTS_CONTAINER_NAME = "products";

    public const string PRODUCTS_CONTAINER_PARTITION_KEY_PATH = "/category";

    public const string LEASE_CONTAINER_NAME = "changefeed-leases";
}