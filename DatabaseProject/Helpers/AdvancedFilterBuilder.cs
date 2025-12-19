using System.Linq.Expressions;

namespace DatabaseProject.Helpers
{
    public static class AdvancedFilterBuilder
    {
        public static Expression<Func<T, bool>> BuildFilter<T>(string filter)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            // Split by OR first, then AND
            var orGroups = filter.Split(new[] { "OR" }, StringSplitOptions.RemoveEmptyEntries);

            Expression? finalExpr = null;

            foreach (var group in orGroups)
            {
                var andConditions = group.Split(new[] { "AND" }, StringSplitOptions.RemoveEmptyEntries);
                Expression? andExpr = null;

                foreach (var condition in andConditions)
                {
                    var parts = condition.Trim().Split(' ', 3);
                    if (parts.Length < 3)
                        throw new ArgumentException($"Invalid condition: {condition}");

                    string propertyName = parts[0];
                    string op = parts[1].ToLower();
                    string valueStr = parts[2].Trim('\'');

                    var property = Expression.Property(parameter, propertyName);
                    var propertyType = property.Type;

                    object value = Convert.ChangeType(valueStr, propertyType);
                    var constant = Expression.Constant(value);

                    Expression comparison = op switch
                    {
                        ">" => Expression.GreaterThan(property, constant),
                        "<" => Expression.LessThan(property, constant),
                        ">=" => Expression.GreaterThanOrEqual(property, constant),
                        "<=" => Expression.LessThanOrEqual(property, constant),
                        "=" or "==" => Expression.Equal(property, constant),
                        "!=" => Expression.NotEqual(property, constant),
                        "contains" => Expression.Call(property,
                            typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                            constant),
                        "startswith" => Expression.Call(property,
                            typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!,
                            constant),
                        "endswith" => Expression.Call(property,
                            typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!,
                            constant),
                        _ => throw new NotSupportedException($"Operator {op} not supported")
                    };

                    andExpr = andExpr == null ? comparison : Expression.AndAlso(andExpr, comparison);
                }

                finalExpr = finalExpr == null ? andExpr : Expression.OrElse(finalExpr, andExpr!);
            }

            return Expression.Lambda<Func<T, bool>>(finalExpr!, parameter);
        }
    }
}
