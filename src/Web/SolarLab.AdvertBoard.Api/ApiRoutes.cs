namespace SolarLab.AdvertBoard.Api
{
    /// <summary>
    /// Статический класс с маршрутами.
    /// </summary>
    public static class ApiRoutes
    {
        /// <summary>
        /// Статический класс с маршрутами, связаннами с операциями над пользователями.
        /// </summary>
        public static class Users
        {
            public const string Login = "api/users/login";
            public const string Register = "api/users/register";
            public const string ConfirmEmail = "api/users/confirm-email";
        }

        /// <summary>
        /// Статический класс с маршрутами, связаннами с операциями над категориями.
        /// </summary>
        public static class Categories
        {
            public const string GetById = "api/categories/{id}";
            public const string GetTree = "api/categories/tree";
        }

        /// <summary>
        /// Статический класс с маршрутами, связаннами с операциями над объявлениями.
        /// </summary>
        public static class Adverts
        {
            public const string GetPublished = "api/adverts/{id}";
            public const string GetPublishedByFilter = "api/adverts";
            public const string GetMyPublished = "api/adverts/my/published";

            public const string CreateDraft = "api/adverts/drafts";
            public const string GetDraft = "api/adverts/drafts/{id}";
            public const string UpdateDraft = "api/adverts/drafts/{id}";
            public const string DeleteDraft = "api/adverts/drafts/{id}";
            public const string PublishDraft = "api/adverts/drafts/{id}/publish";
            public const string GetMyDrafts = "api/adverts/my/drafts";

            public const string DeletePublished = "api/adverts/{id}";
        }

        /// <summary>
        /// Статический класс с маршрутами, связаннами с операциями над изображениями.
        /// </summary>
        public static class Files
        {
            public const string Upload = "api/adverts/drafts/{advertId}/images";
            public const string GetImage = "api/images/{id}";
            public const string DownloadImage = "api/images/{id}/download";
            public const string DeleteImage = "api/adverts/drafts/{advertId}/images/{id}";
        }

        /// <summary>
        /// Статический класс с маршрутами, связаннами с операциями над комментариями.
        /// </summary>
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
