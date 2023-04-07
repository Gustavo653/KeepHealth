using AutoMapper;
using Common.DTO;
using KeepHealth.Application.Interface;
using KeepHealth.Domain;
using KeepHealth.Domain.Enum;
using KeepHealth.Domain.Identity;
using KeepHealth.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KeepHealth.Service
{
    public class PatientService : IPatientService
    {
        private readonly IMedicalConditionRepository _medicalConditionRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPatientRepository _patientRepository;
        private readonly IPatient_MedicalConditionRepository _patient_MedicalConditionRepository;

        public PatientService(IMedicalConditionRepository medicalConditionRepository,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IPatientRepository patientRepository,
                              IPatient_MedicalConditionRepository patient_MedicalConditionRepository)
        {
            _medicalConditionRepository = medicalConditionRepository;
            _mapper = mapper;
            _userManager = userManager;
            _patientRepository = patientRepository;
            _patient_MedicalConditionRepository = patient_MedicalConditionRepository;
        }

        public async Task<ResponseDTO> CreateOrUpdateMedicalCondition(CreateMedicalDTO createMedicalDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                MedicalCondition? entity = new();
                entity = await _medicalConditionRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == createMedicalDTO.Id) ?? new();
                entity = _mapper.Map<MedicalCondition>(createMedicalDTO);
                if (createMedicalDTO.Id == null)
                    await _medicalConditionRepository.InsertAsync(entity);
                else
                    _medicalConditionRepository.Update(entity);
                await _medicalConditionRepository.SaveChangesAsync();
                responseDTO.Object = entity;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> CreateOrUpdatePatient(CreatePatientDTO createPatientDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var userEntity = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == createPatientDTO.Id);
                userEntity = _mapper.Map<User>(createPatientDTO);
                if (userEntity?.Id == 0)
                    await _userManager.CreateAsync(userEntity, createPatientDTO.Password);
                else
                    await _userManager.UpdateAsync(userEntity);

                userEntity = await _userManager.FindByEmailAsync(createPatientDTO.Email);

                var patientEntity = new Patient
                {
                    User = userEntity,
                };

                if (!await _userManager.IsInRoleAsync(userEntity, RoleName.Patient.ToString()))
                    await _userManager.AddToRoleAsync(userEntity, RoleName.Patient.ToString());

                await _patientRepository.InsertAsync(patientEntity);

                var medicalConditions = _medicalConditionRepository.GetEntities().Where(x => createPatientDTO.MedicalConditions.Contains(x.Id));

                foreach (var item in medicalConditions)
                {
                    Patient_MedicalCondition patient = new()
                    {
                        Patient = patientEntity,
                        MedicalCondition = item
                    };
                    await _patient_MedicalConditionRepository.InsertAsync(patient);
                }
                await _patientRepository.SaveChangesAsync();
                responseDTO.Object = userEntity;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetAllMedicalCondition()
        {
            ResponseDTO responseDTO = new();
            try
            {
                var entity = await _medicalConditionRepository.GetListAsync();
                if (entity == null || !entity.Any())
                    responseDTO.SetNotFound();
                else
                    responseDTO.Object = entity;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetMedicalConditionById(long id)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var entity = await _medicalConditionRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                    responseDTO.SetNotFound();
                else
                    responseDTO.Object = entity;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}
