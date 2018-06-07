namespace PetRego.Api
{
    public abstract class Service
    {
        // Making these public so the controllers consuming them have access.
        public const string API_ROUTE_BASE_PATH = "api/";
        public const string API_ROUTE_CONTROLLER_PATH = "[controller]/";

    }
}
