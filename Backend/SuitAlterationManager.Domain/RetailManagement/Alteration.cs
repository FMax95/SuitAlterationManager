using SuitAlterationManager.Domain.AlterationManagement.Enum;
using SuitAlterationManager.Domain.AlterationManagement.ValueObjects;
using SuitAlterationManager.Domain.Base.Models;
using SuitAlterationManager.Domain.Base.Validation;
using System;
using System.Collections.Generic;

namespace SuitAlterationManager.Domain.AlterationManagement
{
    public class Alteration : AggregateRoot<AlterationID>
    {
        private static List<int> AvailableMeasures = new List<int> { -5, 5 };
        public string CustomerEmail { get; set; }
        public AlterationType Type { get; set; }
        public AlterationTypeDirection Direction { get; set; }
        public int MeasureCM { get; set; }
        public AlterationStatus Status { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }

        public Alteration()
        {
            this.CreateDate = DateTime.Now;
            this.Status = AlterationStatus.Created;
        }

        public static Alteration Create(string customerEmail, AlterationType alterationType, AlterationTypeDirection alterationTypeDirection, int measure)
        {
            try
            {
                new System.Net.Mail.MailAddress(customerEmail);
            }
            catch (FormatException)
            {
                throw new DomainException(DomainExceptionCode.CustomerEmailNotVaild);
            }

            if (!AvailableMeasures.Contains(measure))
                throw new DomainException(DomainExceptionCode.MeasureNotVaild);

            return new Alteration()
            {
                Id = new AlterationID(Guid.NewGuid()),
                CustomerEmail = customerEmail,
                Type = alterationType,
                Direction = alterationTypeDirection,
                MeasureCM = measure
            };
        }

        public void StartAlteration()
        {
            if (this.Status != AlterationStatus.Paid)
                throw new DomainException(DomainExceptionCode.CannotStartAlteration_NotPaid);
            this.Status = AlterationStatus.Started;
            this.UpdateDate = DateTime.Now;
        }

        public void PayAlteration()
        {
            if (this.Status != AlterationStatus.Created)
                throw new DomainException(DomainExceptionCode.CannotPayAlteration_NotCreated);
            this.Status = AlterationStatus.Paid;
            this.UpdateDate = DateTime.Now;
        }


        public void FinishAlteration()
        {
            if (this.Status != AlterationStatus.Started)
                throw new DomainException(DomainExceptionCode.CannotFinishAlteration_NotStarted);
            this.Status = AlterationStatus.Done;
            this.UpdateDate = DateTime.Now;
        }
    }
}
