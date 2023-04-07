using Common.DataAccess;
using KeepHealth.Application.Interface;
using KeepHealth.Domain;
using KeepHealth.Domain.Identity;
using KeepHealth.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepHealth.Application
{
    public class PatientRepository : BaseRepository<Patient, KeepHealthContext>, IPatientRepository
    {
        public PatientRepository(KeepHealthContext context) : base(context)
        {
        }
    }
}
