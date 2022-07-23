using SuitAlterationManager.Domain.AlterationManagement.Enum;
using SuitAlterationManager.Domain.Base;

namespace SuitAlterationManager.Api.Client.RetailManagement.Models
{
    public class CreateAlterationModel
    {
        public string CustomerEmail { get; set; }
        public int Measure { get; set; }
        public string AlterationType { get; set; }
        public string AlterationTypeDirection { get; set; }

        internal AlterationType AlterationTypeEnum => AlterationType.ToEnum<AlterationType>();
        internal AlterationTypeDirection AlterationTypeDirectionEnum => AlterationTypeDirection.ToEnum<AlterationTypeDirection>();
    }
}
