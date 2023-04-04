using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepHealth.Domain
{
    public class MedicalCondition : BaseEntity
    {
        public List<Patient_MedicalCondition> Patient_MedicalConditions { get; set; }
        public string Name { get; set; }
    }
}
