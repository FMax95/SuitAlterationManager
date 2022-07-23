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
        private List<int> AvailableMeasures = new List<int> { -5 , 5 };
        public string CustomerEmail { get; set; }
        public AlterationType Type { get; set; }
        public AlterationTypeDirection Direction { get; set; }
        public int Measure { get; set; }
        public AlterationStatus Status { get; set; }

        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        public Alteration()
        {
            this.CreateDate = DateTime.Now;
            this.Status = AlterationStatus.Started;
        }

        public Alteration Create(string customerEmail, AlterationType alterationType, AlterationTypeDirection alterationTypeDirection, int measure)
        {
            if (!this.AvailableMeasures.Contains(measure))
                throw new DomainException(DomainExceptionCode.MeasureNotVaild);

            var entity = new Alteration();
            entity.CustomerEmail = customerEmail;
            entity.Type = alterationType;
            entity.Direction = alterationTypeDirection;
            entity.Measure = measure;

            return entity;
        }
    }
}
