using System.Linq.Expressions;
using System.Reflection;

namespace Task6.Breakfast_Test
{
    /// <summary>
    /// Обработчик Expression выражений
    /// </summary>
    public class Visitor : ExpressionVisitor
    {
        /// <summary>
        /// Устанавливает корректное значение для Expression
        /// </summary>
        /// <param name="memberExpression">Корректируемый Expression</param>
        /// <returns>Expression с корректным значением</returns>
        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            // Отправляет выражение одному из более специализированных методов посещения в этом классе.
            Expression expression = Visit(memberExpression.Expression);
            // Измененное выражение, если оно или любое подвыражение было изменено; в противном случае возвращает исходное выражение.

            if (expression is ConstantExpression)
            {
                object container = ((ConstantExpression)expression).Value;
                MemberInfo member = memberExpression.Member;

                if (member is FieldInfo)
                {
                    object value = ((FieldInfo)member).GetValue(container);
                    return Expression.Constant(value);
                }
                if (member is PropertyInfo)
                {
                    object value = ((PropertyInfo)member).GetValue(container, null);
                    return Expression.Constant(value);
                }
            }

            // Измененное выражение, если оно или какое-либо подвыражение было изменено иначе, возвращает исходное выражение.
            return base.VisitMember(memberExpression);
        }
    }
}
