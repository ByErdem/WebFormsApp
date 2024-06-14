using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFormsApp.Data;
using WebFormsApp.Entity.Concrete;
using WebFormsApp.Entity.Dtos;
using WebFormsApp.Service.Abstract;
using WebFormsApp.Shared.Concrete;

namespace WebFormsApp.Service.Concrete
{
    public class UserManager:IUserService
    {
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITokenService _tokenService;
        private readonly ISessionService _sessionService;
        private readonly IDBContextEntity _entity;


        public UserManager(IMapper mapper, IEncryptionService encryptionService, ITokenService tokenService, RedisCacheManager redisCacheService, ISessionService sessionService, IDBContextEntity entity)
        {
            _mapper = mapper;
            _encryptionService = encryptionService;
            _tokenService = tokenService;
            _redisCacheService = redisCacheService;
            _sessionService = sessionService;
            _entity = entity;
        }

        public async Task<ResponseDto<MUser>> Register(UserRegisterDto user)
        {
            await _entity.Students.FirstOrDefaultAsync(x => x.FirstName == user.Name);


            var rsp = new ResponseDto<MUser>();
            var usr = await _entity.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
            if (usr != null)
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Böyle bir kullanıcı bulunmaktadır, lütfen başka bir kullanıcı adı deneyiniz.";
                return rsp;
            }

            Users newUser = new Users
            {
                HashPassword = _encryptionService.AESEncrypt(user.Password + user.SaltPassword),
                SaltPassword = user.SaltPassword,
                Username = user.Username,
                Name = user.Name,
                Surname = user.Surname
            };

            _entity.Users.Add(newUser);
            await _entity.SaveChangesAsync();

            rsp.ResultStatus = ResultStatus.Success;
            rsp.Data = _mapper.Map<MUser>(newUser);
            rsp.SuccessMessage = "Kullanıcı oluşturuldu";

            return rsp;
        }

        public async Task<ResponseDto<UserParameter>> SignIn(UserLoginDto userDto)
        {
            var rsp = new ResponseDto<UserParameter>();

            try
            {
                var usr = await _entity.Users.FirstOrDefaultAsync(x => x.Username == userDto.Username);
                if (usr != null)
                {
                    var encrypted = _encryptionService.AESEncrypt(userDto.Password + usr.SaltPassword);
                    var usrpass = await _entity.Users.FirstOrDefaultAsync(x => x.Username == userDto.Username && x.HashPassword == encrypted);
                    if (usrpass != null)
                    {
                        var tokenParameters = _tokenService.GenerateToken(userDto.Username);

                        var dto = new UserParameter
                        {
                            UserId = usr.Id,
                            Username = userDto.Username,
                            Token = tokenParameters.Item1,
                            SecretKey = tokenParameters.Item2,
                            GuidKey = Guid.NewGuid().ToString()
                        };

                        //Client verilerini direk Session'da tutmak sakıncalı olabilir.
                        //Sadece session'da client verilerine karşılık bir guid'i tutmak ve tüm bilgileri redis'de saklamak daha güvenli olacaktır.
                        //Normalde client tarafına bir guid gönderiyoruz ve token doğrulaması için bu guid kullanılıyor.
                        //Ancak herhangi bir menüden örneğin /Dashboard/Index gibi bir istek geldiğinde, bunun üzerinden gelebilecek bir token olmadığı için
                        //Session'da bir anahtar değeri saklamamız gerekiyor.
                        _sessionService.SetSessionValue("GuidKey", dto.GuidKey);
                        await _redisCacheService.SetAsync(dto.GuidKey, JsonConvert.SerializeObject(dto));

                        //User'a önemli bilgilerin gitmemesi için temizliyoruz.
                        dto.Username = "";
                        dto.SecretKey = "";
                        dto.Token = "";
                        dto.UserId = -1;

                        rsp.ResultStatus = ResultStatus.Success;
                        rsp.Data = dto;
                        rsp.SuccessMessage = "Giriş başarılı";
                    }
                }
                else
                {
                    rsp.ResultStatus = ResultStatus.Error;
                    rsp.ErrorMessage = "Kullanıcı adı veya şifre yanlış!";
                }
            }
            catch (Exception ex)
            {

            }

            return rsp;
        }

        public async Task<ResponseDto<UserParameter>> GetUserFromRedis()
        {
            var rsp = new ResponseDto<UserParameter>();
            var guidKey = _sessionService.GetSessionValue("GuidKey");
            var parameters = await _redisCacheService.GetAsync(guidKey);
            var userParameter = JsonConvert.DeserializeObject<UserParameter>(parameters);

            rsp.Data = userParameter;
            rsp.ResultStatus = ResultStatus.Success;
            rsp.SuccessMessage = "Kullanıcı parametreleri alındı.";

            return rsp;
        }

        public async Task<ResponseDto<int>> GetUserId()
        {
            var rsp = new ResponseDto<int>();
            var guidKey = _sessionService.GetSessionValue("GuidKey");
            var parameters = await _redisCacheService.GetAsync(guidKey);
            var userParameter = JsonConvert.DeserializeObject<UserParameter>(parameters);

            rsp.Data = userParameter.UserId;
            rsp.ResultStatus = ResultStatus.Success;
            rsp.SuccessMessage = "Kullanıcı parametreleri alındı.";

            return rsp;
        }

        public async Task<ResponseDto<UserInformationsDto>> GetUserInformations()
        {
            var rsp = new ResponseDto<UserInformationsDto>();
            var rspUser = await GetUserId();
            var user = await _entity.Users.FirstOrDefaultAsync(x => x.Id == rspUser.Data);

            rsp.ResultStatus = ResultStatus.Success;
            rsp.Data = new UserInformationsDto()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname
            };
            rsp.SuccessMessage = "Kullanıcı bilgileri alındı.";
            return rsp;
        }

        public async Task<ResponseDto<int>> Logout()
        {
            var rsp = new ResponseDto<int>();
            var guidKey = _sessionService.GetSessionValue("GuidKey");
            _sessionService.SetSessionValue("GuidKey", "");
            await _redisCacheService.DeleteAsync(guidKey);
            rsp.ResultStatus = ResultStatus.Success;
            rsp.Data = 1;
            rsp.SuccessMessage = "Oturum başarıyla kapatıldı";
            return rsp;

        }

    }
}

