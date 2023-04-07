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
    public class MedicalConditionRepository : BaseRepository<MedicalCondition, KeepHealthContext>, IMedicalConditionRepository
    {
        public MedicalConditionRepository(KeepHealthContext context) : base(context)
        {
        }
    }
}
