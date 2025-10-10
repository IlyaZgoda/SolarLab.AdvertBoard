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
            public const string GetPublishedById = "api/adverts/{id}";
            public const string GetPublishedByFilter = "api/adverts";
            public const string GetMyPublished = "api/adverts/my/published";

            public const string CreateDraft = "api/adverts/drafts";
            public const string GetDraftById = "api/adverts/drafts/{id}";
            public const string UpdateDraft = "api/adverts/drafts/{id}";
            public const string DeleteDraft = "api/adverts/drafts/{id}";
            public const string PublishDraft = "api/adverts/drafts/{id}/publish";
            public const string GetMyDrafts = "api/adverts/my/drafts";

            public const string DeletePublished = "api/adverts/{id}";
        }

        public static class Comments
        {
            public const string Create = "api/adverts/{advertId}/comments";
            public const string Update = "api/comments/{commentId}";
            public const string Delete = "api/comments/{commentId}";
            public const string GetById = "api/comments/{commentId}";
            public const string GetByAdvertId = "api/adverts/{advertId}/comments";
        }
    }
}
