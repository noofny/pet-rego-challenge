namespace PetRego.Common
{
    public static class Constants
    {
        // todo - do something smarter than this (consts used in link building but
        //        are also used when declaring the api route attributes.
        //        Maybe just create a custom Route attibute, put these in there?
        public const string API_ROUTE_BASE_PATH = "api/";
        public const string API_ROUTE_CONTROLLER_PATH = "[controller]/";


        public const string TOKENIZED_BASE_URL = "<<BASE_URL>>";
        public const string TOKENIZED_CURRENT_URL = "<<CURRENT_URL>>";
        public const string TOKENIZED_CONTROLLER_PATH = "<<CONTROLLER_PATH>>";
        public const string TOKENIZED_ACTION_PATH = "<<ACTION_PATH>>";

    }
}
