using System;
using NUnit.Framework;
using Task5.BreakfastTaskGenerator.IterationHandlers;
using Task5.Common.Enums;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator.Tests.Generators.IterationHandlers
{
    public class BaseIterationHandlerTests
    {
        [Test]
        public void DeterminePersonNameType_IsReturnValueFirstNameIfisUsedNameParameterEqualTrue()
        {
            var person = new DataListItem() { UsedFirstName = true, UsedLastName = false };

            var personNameType = DerivativeFromBaseIterationHandler.DeterminePersonNameTypeTest(person, true);

            Assert.AreEqual(PersonNameType.FirstName, personNameType);
        }

        [Test]
        public void DeterminePersonNameType_IsReturnValueLastNameIfisUsedNameParameterEqualTrue()
        {
            var person = new DataListItem() { UsedFirstName = false, UsedLastName = true };

            var personNameType = DerivativeFromBaseIterationHandler.DeterminePersonNameTypeTest(person, true);

            Assert.AreEqual(PersonNameType.LastName, personNameType);
        }

        [Test]
        public void DeterminePersonNameType_IsReturnValueFirstNameIfisUsedNameParameterEqualFalse()
        {
            var person = new DataListItem() { UsedFirstName = false, UsedLastName = true };

            var personNameType = DerivativeFromBaseIterationHandler.DeterminePersonNameTypeTest(person, false);

            Assert.AreEqual(PersonNameType.FirstName, personNameType);
        }

        [Test]
        public void DeterminePersonNameType_IsReturnValueLastNameIfisUsedNameParameterEqualFalse()
        {
            var person = new DataListItem() { UsedFirstName = true, UsedLastName = false };

            var personNameType = DerivativeFromBaseIterationHandler.DeterminePersonNameTypeTest(person, false);

            Assert.AreEqual(PersonNameType.LastName, personNameType);
        }
    }

    class DerivativeFromBaseIterationHandler : BaseIterationHandler
    {
        public DerivativeFromBaseIterationHandler(PersonsTableList personTableList) : base(personTableList) { }

        public override Condition ExecuteIteration() => throw new NotImplementedException();

        public static PersonNameType DeterminePersonNameTypeTest(DataListItem person, bool isUsedName)
        {
            return DeterminePersonNameType(person, isUsedName);
        }
    }
}
