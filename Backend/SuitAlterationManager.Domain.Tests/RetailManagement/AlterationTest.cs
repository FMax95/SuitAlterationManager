using SuitAlterationManager.Domain.AlterationManagement;
using SuitAlterationManager.Domain.AlterationManagement.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.Tests.RetailManagement.Builders;

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
            Assert.AreEqual(AlterationStatus.Created, alteration.Status);
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

        [TestMethod]
        [DataRow(AlterationStatus.Started)]
        [DataRow(AlterationStatus.Created)]
        [DataRow(AlterationStatus.Done)]
        public void StartAlteration_WrongStatus(AlterationStatus status)
        {
            Alteration alteration = new AlterationBuilder().WithStatus(status);
            Assert.That.ThrowsWithCode<DomainException>(DomainExceptionCode.CannotStartAlteration_NotPaid, () =>
                alteration.StartAlteration());
        }

        [TestMethod]
        public void StartAlteration()
        {
            Alteration alteration = new AlterationBuilder().WithStatus(AlterationStatus.Paid);
            alteration.StartAlteration();
            Assert.AreEqual(AlterationStatus.Started, alteration.Status);
            Assert.IsNotNull(alteration.UpdateDate);
        }

        [TestMethod]
        [DataRow(AlterationStatus.Paid)]
        [DataRow(AlterationStatus.Created)]
        [DataRow(AlterationStatus.Done)]
        public void FinishAlteration_WrongStatus(AlterationStatus status)
        {
            Alteration alteration = new AlterationBuilder().WithStatus(status);
            Assert.That.ThrowsWithCode<DomainException>(DomainExceptionCode.CannotFinishAlteration_NotStarted, () =>
                alteration.FinishAlteration());
        }

        [TestMethod]
        public void FinishAlteration()
        {
            Alteration alteration = new AlterationBuilder().WithStatus(AlterationStatus.Started);
            alteration.FinishAlteration();
            Assert.AreEqual(AlterationStatus.Done, alteration.Status);
            Assert.IsNotNull(alteration.UpdateDate);
        }

        [TestMethod]
        [DataRow(AlterationStatus.Started)]
        [DataRow(AlterationStatus.Paid)]
        [DataRow(AlterationStatus.Done)]
        public void PayAlteration_WrongStatus(AlterationStatus status)
        {
            Alteration alteration = new AlterationBuilder().WithStatus(status);
            Assert.That.ThrowsWithCode<DomainException>(DomainExceptionCode.CannotPayAlteration_NotCreated, () =>
                alteration.PayAlteration());
        }

        [TestMethod]
        public void PayAlteration()
        {
            Alteration alteration = new AlterationBuilder().WithStatus(AlterationStatus.Created);
            alteration.PayAlteration();
            Assert.AreEqual(AlterationStatus.Paid, alteration.Status);
            Assert.IsNotNull(alteration.UpdateDate);
        }
    }
}
