using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.Persistence.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Extensions
{
    public static class Mapper
    {
        /// <summary>
        /// Преобразует запрос read-моделей объявлений в DTO опубликованных объявлений.
        /// </summary>
        /// <param name="query">Запрос read-моделей объявлений.</param>
        /// <returns>Запрос DTO опубликованных объявлений.</returns>
        public static IQueryable<PublishedAdvertItem> ToPublishedAdvertItem(this IQueryable<AdvertReadModel> query)
        {
            return query.Select(advert => new PublishedAdvertItem(
                    advert.Id,
                    advert.Title,
                    advert.Description,
                    advert.Price,
                    advert.CategoryId,
                    advert.Category.Title,
                    advert.AuthorId,
                    advert.Author.FullName,
                    advert.PublishedAt.Value));
        }

        /// <summary>
        /// Преобразует запрос read-моделей объявлений в DTO черновиков объявлений.
        /// </summary>
        /// <param name="query">Запрос read-моделей объявлений.</param>
        /// <returns>Запрос DTO черновиков объявлений.</returns>
        public static IQueryable<AdvertDraftItem> ToAdvertDraftItem(this IQueryable<AdvertReadModel> query)
        {
            return query.Select(advert => new AdvertDraftItem(
                    advert.Id,
                    advert.Title,
                    advert.Description,
                    advert.Price,
                    advert.CategoryId,
                    advert.Category.Title,
                    advert.CreatedAt,
                    advert.UpdatedAt,
                    advert.AuthorId));
        }

        /// <summary>
        /// Преобразует запрос read-моделей объявлений в DTO деталей черновика.
        /// </summary>
        /// <param name="query">Запрос read-моделей объявлений.</param>
        /// <returns>Запрос DTO деталей черновика.</returns>
        public static IQueryable<AdvertDraftDetailsResponse> ToAdvertDraftDetails(this IQueryable<AdvertReadModel> query)
        {
            return query.Select(advert => new AdvertDraftDetailsResponse(
                              advert.Id,
                              advert.Title,
                              advert.Description,
                              advert.Price,
                              advert.CategoryId,
                              advert.Category.Title,
                              advert.Status.ToString(),
                              advert.CreatedAt,
                              advert.UpdatedAt,
                              advert.AuthorId
                          ));
        }

        /// <summary>
        /// Преобразует запрос read-моделей объявлений в DTO деталей опубликованного объявления.
        /// </summary>
        /// <param name="query">Запрос read-моделей объявлений.</param>
        /// <returns>Запрос DTO деталей опубликованного объявления.</returns>
        public static IQueryable<PublishedAdvertDetailsResponse> ToPublishedAdvertDetails(this IQueryable<AdvertReadModel> query)
        {
            return query.Select(advert => new PublishedAdvertDetailsResponse(
                       advert.Id,
                       advert.Title,
                       advert.Description,
                       advert.Price,
                       advert.CategoryId,
                       advert.Category.Title,
                       advert.PublishedAt!.Value,
                       advert.AuthorId,
                       new UserContactInfoResponse(
                           advert.Author.FullName,
                           advert.Author.ContactEmail,
                           advert.Author.PhoneNumber)));
        }

        /// <summary>
        /// Преобразует запрос read-моделей комментариев в DTO комментариев.
        /// </summary>
        /// <param name="query">Запрос read-моделей комментариев.</param>
        /// <returns>Запрос DTO комментариев.</returns>
        public static IQueryable<CommentItem> ToCommentItem(this IQueryable<CommentReadModel> query)
        {
            return query.Select(comment => new CommentItem(
                       comment.Id,
                       comment.AdvertId,
                       comment.AuthorId,
                       comment.Author.FullName,
                       comment.Text,
                       comment.CreatedAt,
                       comment.UpdatedAt));
        }
    }
}
