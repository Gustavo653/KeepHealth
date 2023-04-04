using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepHealth.Service.Interface
{
    public interface IPatientService
    {
        Task<ResponseDTO> CreateOrUpdateMedicalCondition(CreateMedicalDTO createMedicalDTO);
        Task<ResponseDTO> GetAllMedicalCondition();
        Task<ResponseDTO> GetMedicalConditionById(long id);
    }
}
