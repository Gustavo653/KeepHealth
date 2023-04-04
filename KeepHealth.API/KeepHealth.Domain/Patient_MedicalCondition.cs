using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepHealth.Domain
{
    public class Patient_MedicalCondition : BaseEntity
    {
        public Patient Patient { get; set; }
        public MedicalCondition MedicalCondition { get; set; }
    }
}
