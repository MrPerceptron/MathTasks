using System;
using System.Linq.Expressions;
using BreakfastTaskGenerator.Enums;
using BreakfastTaskGenerator.Models;

namespace Tesk6.Tests
{
    public static class ExpressionHelper
    {
        private readonly static Visitor _visitor = new();

        /// <summary>
        /// Создаёт выражение выбирая правильное свойство
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///     Если <paramref name="personNameType"/> содержит что-то, помимо FirstName и LastName
        /// </exception>
        public static Expression<Func<Person, bool>> GetExpression
            (DataListItem dataListItem, PersonNameType personNameType)
        {
            Expression<Func<Person, bool>> expression = personNameType switch
            {
                PersonNameType.FirstName => x => x.FirstName == dataListItem.FirstName,
                PersonNameType.LastName => x => x.LastName == dataListItem.LastName,
                _ => throw new NotSupportedException("Человек может содержать только \"Имя\" и \"Фамилию\"")
            };
            Expression modifiedExpression = _visitor.Visit(expression);

            return modifiedExpression as Expression<Func<Person, bool>>;
        }
    }
}
