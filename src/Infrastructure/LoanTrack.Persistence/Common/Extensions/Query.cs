using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Common.Extensions;

internal static class Query
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }

    public static IQueryable<T> SmartPaging<T>(this IQueryable<T> query, int page, int pageSize)
        => query.Skip((page - 1) * pageSize).Take(pageSize);

    public static IQueryable<T> SmartSearch<T>(
        this IQueryable<T> query,
        string searchColumn,
        string searchValue
    ) where T : class
    {
        if (string.IsNullOrWhiteSpace(searchValue) || string.IsNullOrWhiteSpace(searchColumn))
        {
            return query;
        }

        try
        {
            var propertyInfo = typeof(T).GetProperty(searchColumn,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null || propertyInfo.PropertyType != typeof(string))
                return query;

            // EF Core compatible case-insensitive search
            return query.Where(e => 
                EF.Functions.Like(
                    EF.Property<string>(e, propertyInfo.Name),
                    $"%{searchValue}%"));
        }
        catch
        {
            return query;
        }
    }
    
    public static IQueryable<T> SmartSort<T>(
        this IQueryable<T> query,
        string? sortColumn,
        bool descending = false
    ) where T : class
    {
        if (string.IsNullOrWhiteSpace(sortColumn))
            return query;

        try
        {
            var propertyInfo = typeof(T).GetProperty(sortColumn,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                return query;

            return descending 
                ? query.OrderByDescending(e => EF.Property<object>(e, propertyInfo.Name))
                : query.OrderBy(e => EF.Property<object>(e, propertyInfo.Name));
        }
        catch
        {
            return query;
        }
    }
}
