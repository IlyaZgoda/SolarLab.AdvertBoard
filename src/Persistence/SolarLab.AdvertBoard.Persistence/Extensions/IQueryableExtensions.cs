using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Contracts.Base;

namespace SolarLab.AdvertBoard.Persistence.Extensions
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Выполняет пагинацию запроса и возвращает пагинированную коллекцию.
        /// </summary>
        /// <typeparam name="T">Тип элементов в коллекции.</typeparam>
        /// <param name="query">Запрос для пагинации.</param>
        /// <param name="page">Номер страницы (начинается с 1).</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Пагинированная коллекция элементов.</returns>
        public static async Task<PaginationCollection<T>> ToPagedAsync<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return new PaginationCollection<T>(items, page, pageSize, totalCount, totalPages);
        }
    }
}
