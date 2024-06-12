using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFormsApp.Entity.Concrete;
using WebFormsApp.Entity.Dtos;

namespace WebFormsApp.Service.Abstract
{
    public interface IUserService
    {
        Task<ResponseDto<UserParameter>> SignIn(UserLoginDto userDto);
        Task<ResponseDto<MUser>> Register(UserRegisterDto user);
        Task<ResponseDto<UserParameter>> GetUserFromRedis();
        Task<ResponseDto<int>> GetUserId();
        Task<ResponseDto<UserInformationsDto>> GetUserInformations();
        Task<ResponseDto<int>> Logout();
    }
}
