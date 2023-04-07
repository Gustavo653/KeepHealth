using Common.Infrastructure;
using KeepHealth.Domain;
using KeepHealth.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepHealth.Application.Interface
{
    public interface IMedicalConditionRepository : IRepositoryBase<MedicalCondition>
    {
    }
}
