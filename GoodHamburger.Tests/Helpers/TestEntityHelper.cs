using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Tests.Helpers;


internal static class TestEntityHelper
{
    public static T WithId<T>(this T entity, long id) where T : BaseEntity
    {
        typeof(BaseEntity)
            .GetProperty(nameof(BaseEntity.Id))!
            .SetValue(entity, id);
        return entity;
    }
}