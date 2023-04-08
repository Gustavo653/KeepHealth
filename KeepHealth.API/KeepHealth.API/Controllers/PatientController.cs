using Common.DTO;
using KeepHealth.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KeepHealth.Domain.Enum;

namespace KeepHealth.API.Controllers
{
    public class PatientController : BaseController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("CreateOrUpdatePatient")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrUpdatePatient([FromQuery] long? patientId, CreatePatientDTO patientDTO)
        {
            var user = await _patientService.CreateOrUpdatePatient(patientId, patientDTO);
            return StatusCode(user.Code, user);
        }

        [HttpPost("CreateOrUpdateMedicalCondition")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateOrUpdateMedicalCondition(CreateMedicalDTO medicalDTO)
        {
            var user = await _patientService.CreateOrUpdateMedicalCondition(medicalDTO);
            return StatusCode(user.Code, user);
        }

        [HttpGet("GetAllMedicalCondition")]
        public async Task<IActionResult> GetAllMedicalCondition()
        {
            var user = await _patientService.GetAllMedicalCondition();
            return StatusCode(user.Code, user);
        }

        [HttpGet("GetMedicalConditionById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMedicalConditionById([FromQuery] long id)
        {
            var user = await _patientService.GetMedicalConditionById(id);
            return StatusCode(user.Code, user);
        }
    }
}
