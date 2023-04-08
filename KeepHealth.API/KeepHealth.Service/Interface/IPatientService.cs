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
        Task<ResponseDTO> CreateOrUpdatePatient(long? id, CreatePatientDTO createPatientDTO);
        Task<ResponseDTO> AddPatientInMedicalCondition(long patientId, List<long> conditions);
        Task<ResponseDTO> CreateOrUpdateMedicalCondition(CreateMedicalDTO createMedicalDTO);
        Task<ResponseDTO> GetAllMedicalCondition();
        Task<ResponseDTO> GetMedicalConditionById(long id);
    }
}
