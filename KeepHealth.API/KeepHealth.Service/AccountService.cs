using AutoMapper;
using Common.DTO;
using KeepHealth.Application.Interface;
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

        public async Task<ResponseDTO> CreateAccountAsync(UserDTO userDto)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (await UserExists(userDto.UserName))
                    throw new Exception($"Usuário {userDto.UserName} já existe.");

                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                    responseDTO.Object = _mapper.Map<UserDTO>(user);
                else
                {
                    string error = string.Empty;
                    foreach (var item in result.Errors)
                    {
                        error += item.Description + "\n";
                    }
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
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

        public async Task<ResponseDTO> UpdateAccount(UserDTO userDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var user = await GetUserAsync(userDTO.UserName) ?? throw new Exception($"Usuário '{userDTO.UserName}' não encontrado!"); ;
                userDTO.Id = user.Id;

                _mapper.Map(userDTO, user);

                if (userDTO.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userDTO.Password);
                }

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
                var userRetorno = await GetUserAsync(user.UserName);

                responseDTO.Object = _mapper.Map<UserDTO>(userRetorno);
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        private async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName == userName.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar se usuário existe. Erro: {ex.Message}");
            }
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
    }
}