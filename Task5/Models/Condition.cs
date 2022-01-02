using System.Diagnostics;
using BreakfastTaskGenerator.Enums;

namespace BreakfastTaskGenerator.Models
{
    [DebuggerDisplay("{ToString()}")]
    public class Condition : IEquatable<Condition>, ICloneable<Condition>
    {
        public DataListItem? MainPerson { get; set; }
        public IEnumerable<DataListItem>? DependentPersons { get; set; }

        public PersonNameType NameType { get; set; }
        public PersonNameType[]? DependentNameTypes { get; set; }

        public ConditionType ConditionType { get; set; }

        public override string ToString()
        {
            string mainPersonName = NameType == PersonNameType.FirstName ? MainPerson.FirstName : MainPerson.LastName;

            if (ConditionType == ConditionType.FirstNameEqual)
                return $"{ConditionType} {mainPersonName}";

            string otherPersonNames = string.Join(", ", DependentPersons.Select((x, i) =>
                DependentNameTypes[i] == PersonNameType.FirstName ? x.FirstName : x.LastName));

            return $"{mainPersonName} {ConditionType} {otherPersonNames}";
        }

        public bool Equals(Condition? other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (MainPerson != null)
            {
                if (MainPerson.Equals(other.MainPerson) == false)
                    return false;
            }

            if ((DependentPersons == null || other.DependentPersons == null) == false)
            {
                var thisDependentPersons = DependentPersons.ToArray();
                var otherDependentPersons = other.DependentPersons.ToArray();

                if (thisDependentPersons.Length != otherDependentPersons.Length)
                    return false;

                for (int i = 0; i < thisDependentPersons.Length; i++)
                {
                    if (thisDependentPersons[i].Equals(otherDependentPersons[i]) == false)
                        return false;
                }
            }

            if ((DependentNameTypes == null || other.DependentNameTypes == null) == false)
            {
                if (DependentNameTypes.Length != other.DependentNameTypes.Length)
                    return false;

                for (int i = 0; i < DependentNameTypes.Length; i++)
                {
                    if (DependentNameTypes[i] != other.DependentNameTypes[i])
                        return false;
                }
            }

            return (NameType == other.NameType) && (ConditionType == other.ConditionType);
        }

        public override bool Equals(object? obj) => Equals(obj as Condition);

        public override int GetHashCode() => throw new NotImplementedException();

        public object Clone()
        {
            var mainPerson = (MainPerson == default) ? default
                : new DataListItem(MainPerson);

            var dependentPersons = (DependentPersons == default) ? default
                : DependentPersons.Select(x => new DataListItem(x));

            var dependentNameTypes = (DependentNameTypes == default) ? default
                : (PersonNameType[])DependentNameTypes.Clone();

            return new Condition
            {
                ConditionType = ConditionType,
                DependentNameTypes = dependentNameTypes,
                DependentPersons = dependentPersons,
                NameType = NameType,
                MainPerson = mainPerson
            };
        }
    }
}