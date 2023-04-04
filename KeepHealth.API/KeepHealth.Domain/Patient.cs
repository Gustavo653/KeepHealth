using KeepHealth.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepHealth.Domain
{
    public class Patient : BaseEntity
    {
        public List<Patient_MedicalCondition> Patient_MedicalConditions { get; set; }
        public User User { get; set; }
    }
}
