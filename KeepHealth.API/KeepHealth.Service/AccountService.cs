using AutoMapper;
using Common.DTO;
using Common.Functions;
using KeepHealth.Application.Interface;
using KeepHealth.Domain.Enum;
using KeepHealth.Domain.Identity;
using KeepHealth.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KeepHealth.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper,
                              IUserRepository userRepository,
                              ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        private async Task<SignInResult> CheckUserPasswordAsync(UserLoginDTO userUpdateDto)
        {
            try
            {
                var user = await GetUserAsync(userUpdateDto.UserName);
                return await _signInManager.CheckPasswordSignInAsync(user, userUpdateDto.Password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar senha do usuário. Erro: {ex.Message}");
            }
        }

        private async Task<User> GetUserAsync(string userName)
        {

            var user = await _userRepository.GetEntities()
                                            .FirstOrDefaultAsync(x => x.UserName == userName) ??
                                            throw new Exception($"Usuário '{userName}' não encontrado!");
            return user;
        }

        public async Task<ResponseDTO> Login(UserLoginDTO userDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var user = await GetUserAsync(userDTO.UserName);
                var password = await CheckUserPasswordAsync(userDTO);
                if (user != null && password.Succeeded)
                {
                    responseDTO.Object = new
                    {
                        userName = user.UserName,
                        email = user.Email,
                        token = await _tokenService.CreateToken(_mapper.Map<UserDTO>(user))
                    };
                }
                else
                    responseDTO.Code = 401;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetUserByUserNameAsync(string userName)
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await GetUserAsync(userName);
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> CreateOrUpdateUser(long? id, UserDTO user)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var userEntity = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id || x.Email == user.Email || x.UserName == user.UserName);
                if (userEntity == null)
                {
                    userEntity = new();
                    PropertyCopier<UserDTO, User>.Copy(user, userEntity);
                    await _userManager.CreateAsync(userEntity, user.Password);
                }
                else
                {
                    PropertyCopier<UserDTO, User>.Copy(user, userEntity);
                    await _userManager.UpdateAsync(userEntity);
                }
                responseDTO.Object = userEntity;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> AddUserInRole(User user, RoleName role)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (!await _userManager.IsInRoleAsync(user, role.ToString()))
                    await _userManager.AddToRoleAsync(user, role.ToString());
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}