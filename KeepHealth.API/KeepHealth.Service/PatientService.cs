using AutoMapper;
using Common.DTO;
using KeepHealth.Application.Interface;
using KeepHealth.Domain;
using KeepHealth.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepHealth.Service
{
    public class PatientService : IPatientService
    {
        private readonly IMedicalConditionRepository _medicalConditionRepository;
        private readonly IMapper _mapper;

        public PatientService(IMedicalConditionRepository medicalConditionRepository, IMapper mapper)
        {
            _medicalConditionRepository = medicalConditionRepository;
            _mapper = mapper;
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
