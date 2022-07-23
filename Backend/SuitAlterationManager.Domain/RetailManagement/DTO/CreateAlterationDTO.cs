using SuitAlterationManager.Domain.AlterationManagement.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuitAlterationManager.Domain.RetailManagement.DTO
{
    public class CreateAlterationDTO
    {
        public string CustomerEmail { get; set; }
        public AlterationType AlterationType    { get; set; }
        public AlterationTypeDirection AlterationDirection { get; set; }
        public int Measure { get; set; }
    }
}
