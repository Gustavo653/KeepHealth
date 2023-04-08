using AutoMapper;
using Common.DTO;
using Common.Functions;
using KeepHealth.Application.Interface;
using KeepHealth.Domain;
using KeepHealth.Domain.Enum;
using KeepHealth.Domain.Identity;
using KeepHealth.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace KeepHealth.Service
{
    public class PatientService : IPatientService
    {
        private readonly IMedicalConditionRepository _medicalConditionRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPatientRepository _patientRepository;
        private readonly IPatient_MedicalConditionRepository _patient_MedicalConditionRepository;
        private readonly IAccountService _accountService;

        public PatientService(IMedicalConditionRepository medicalConditionRepository,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IPatientRepository patientRepository,
                              IPatient_MedicalConditionRepository patient_MedicalConditionRepository,
                              IAccountService accountService)
        {
            _medicalConditionRepository = medicalConditionRepository;
            _mapper = mapper;
            _userManager = userManager;
            _patientRepository = patientRepository;
            _patient_MedicalConditionRepository = patient_MedicalConditionRepository;
            _accountService = accountService;
        }

        public async Task<ResponseDTO> AddPatientInMedicalCondition(long patientId, List<long> conditions)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var medicalConditions = await _medicalConditionRepository.GetEntities().Where(x => conditions.Contains(x.Id)).ToListAsync();
                var patientEntity = await _patientRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == patientId) ?? throw new Exception("Paciente não encontrado!");

                foreach (var item in medicalConditions)
                {
                    Patient_MedicalCondition patient = new()
                    {
                        Patient = patientEntity,
                        MedicalCondition = item
                    };
                    await _patient_MedicalConditionRepository.InsertAsync(patient);
                }
                await _patient_MedicalConditionRepository.SaveChangesAsync();
                responseDTO.Object = new
                {
                    patientId,
                    medicalConditions
                };
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
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

        public async Task<ResponseDTO> CreateOrUpdatePatient(long? id, CreatePatientDTO createPatientDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (id == null)
                {
                    User? userExists;
                    userExists = await _userManager.FindByEmailAsync(createPatientDTO.Email);
                    if (userExists != null)
                        throw new Exception("Este usuário não pode ser cadastrado, pois já está em uso.");
                    userExists = await _userManager.FindByNameAsync(createPatientDTO.UserName);
                    if (userExists != null)
                        throw new Exception("Este usuário não pode ser cadastrado, pois já está em uso.");
                }

                UserDTO userDTO = new();
                PropertyCopier<CreatePatientDTO, UserDTO>.Copy(createPatientDTO, userDTO);
                var responseUser = await _accountService.CreateOrUpdateUser(null, userDTO);

                var userEntity = await _userManager.FindByEmailAsync(createPatientDTO.Email) ?? throw new Exception("Usuário não cadastrado");
                await _accountService.AddUserInRole(userEntity, RoleName.Patient);

                var patientEntity = await _patientRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == id || x.User.Email == createPatientDTO.Email);
                if (patientEntity == null)
                {
                    patientEntity = new Patient() { User = userEntity };
                    await _patientRepository.InsertAsync(patientEntity);
                }
                else
                {
                    _patientRepository.Update(patientEntity);
                }
                await _patientRepository.SaveChangesAsync();

                var conditions = new ResponseDTO();
                if (createPatientDTO.MedicalConditions != null && createPatientDTO.MedicalConditions.Any())
                    conditions = await AddPatientInMedicalCondition(patientEntity.Id, createPatientDTO.MedicalConditions);

                responseDTO.Object = new
                {
                    id,
                    patient = createPatientDTO
                };
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
