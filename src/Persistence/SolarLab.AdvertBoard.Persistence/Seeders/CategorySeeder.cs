using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Categories;

namespace SolarLab.AdvertBoard.Persistence.Seeders
{
    public class CategorySeeder(ApplicationDbContext context)
    {
        public async Task SeedAsync()
        {
            if (await context.Categories.AnyAsync())
            {
                return;
            }

            var cars = Category.CreateRoot(CategoryTitle.Create("Автомобили").Value);
            cars.AddChild(CategoryTitle.Create("Легковые").Value);
            cars.AddChild(CategoryTitle.Create("Грузовые").Value);
            cars.AddChild(CategoryTitle.Create("Мотоциклы").Value);

            var realEstate = Category.CreateRoot(CategoryTitle.Create("Недвижимость").Value);
            realEstate.AddChild(CategoryTitle.Create("Квартиры").Value);
            realEstate.AddChild(CategoryTitle.Create("Дома").Value);
            realEstate.AddChild(CategoryTitle.Create("Земельные участки").Value);

            var electronics = Category.CreateRoot(CategoryTitle.Create("Электроника").Value);
            electronics.AddChild(CategoryTitle.Create("Телефоны").Value);
            electronics.AddChild(CategoryTitle.Create("Компьютеры").Value);
            electronics.AddChild(CategoryTitle.Create("Телевизоры").Value);

            await context.AddRangeAsync(cars, realEstate, electronics);

            await context.SaveChangesAsync();
        }
    }
}
