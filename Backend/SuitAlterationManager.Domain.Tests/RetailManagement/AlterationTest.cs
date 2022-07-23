using SuitAlterationManager.Domain.AlterationManagement;
using SuitAlterationManager.Domain.AlterationManagement.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuitAlterationManager.Domain.Base.Validation;

namespace SuitAlterationManager.Domain.Tests.RetailManagement
{
    [TestClass]
    public class AlterationTest
    {
        [TestMethod]
        [DataRow("test@mail.it", 5, AlterationType.ShortenSleeves, AlterationTypeDirection.Left)]
        public void CreateAlteration(string customerEmail, int measure, AlterationType alterationType, AlterationTypeDirection alterationTypeDirection)
        {
            var alteration = Alteration.Create(customerEmail: customerEmail,
                                               alterationType: alterationType,
                                               alterationTypeDirection: alterationTypeDirection,
                                               measure: measure);
            Assert.AreEqual(customerEmail, alteration.CustomerEmail);
            Assert.AreEqual(alterationType, alteration.Type);
            Assert.AreEqual(measure, alteration.MeasureCM);
            Assert.AreEqual(alterationTypeDirection, alteration.Direction);
            Assert.IsNull(alteration.UpdateDate);
            Assert.IsNotNull(alteration.CreateDate);
            Assert.AreEqual(AlterationStatus.Started, alteration.Status);
        }


        [TestMethod]
        [DataRow("mail.it", 5, AlterationType.ShortenSleeves, AlterationTypeDirection.Left)]
        public void CreateAlteration_WrongEmail(string customerEmail, int measure, AlterationType alterationType, AlterationTypeDirection alterationTypeDirection)
        {
            Assert.That.ThrowsWithCode<DomainException>(DomainExceptionCode.CustomerEmailNotVaild, () =>
                Alteration.Create(customerEmail: customerEmail,
                                               alterationType: alterationType,
                                               alterationTypeDirection: alterationTypeDirection,
                                               measure: measure));
        }


        [TestMethod]
        [DataRow("test@mail.it", 8, AlterationType.ShortenSleeves, AlterationTypeDirection.Left)]
        public void CreateAlteration_WrongMeasure(string customerEmail, int measure, AlterationType alterationType, AlterationTypeDirection alterationTypeDirection)
        {
            Assert.That.ThrowsWithCode<DomainException>(DomainExceptionCode.MeasureNotVaild, () =>
                Alteration.Create(customerEmail: customerEmail,
                                               alterationType: alterationType,
                                               alterationTypeDirection: alterationTypeDirection,
                                               measure: measure));
        }
    }
}
