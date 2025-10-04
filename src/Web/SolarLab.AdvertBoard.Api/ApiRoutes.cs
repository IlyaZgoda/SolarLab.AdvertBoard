namespace SolarLab.AdvertBoard.Api
{
    public static class ApiRoutes
    {
        public static class Users
        {
            public const string Login = "api/users/login";
            public const string Register = "api/users/register";
            public const string ConfirmEmail = "api/users/confirm-email";
        }

        public static class Categories
        {
            public const string GetById = "api/categories/{id}";
            public const string GetTree = "api/categories/tree";
        }

        public static class Adverts
        {
            public const string GetById = "api/adverts/{id}";
            public const string UpdateDraft = "api/adverts/drafts/{id}";
            public const string CreateDraft = "api/adverts/drafts/new";
            public const string GetAdvertDraft = "api/adverts/drafts/{id}";
            public const string DeleteAdvertDraft = "api/adverts/drafts/{id}";
            public const string PublishDraft = "api/adverts/drafts/{id}/publish";
        }
    }
}
