using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFormsApp.Data;
using WebFormsApp.Entity.Dtos;
using WebFormsApp.Service.Abstract;
using WebFormsApp.Service.Validation;
using WebFormsApp.Shared.Concrete;

namespace WebFormsApp.Service.Concrete
{
    public class StudentManager : IStudentService
    {
        private readonly IDBContextEntity _entity;
        private readonly IMapper _mapper;

        public StudentManager(IDBContextEntity entity, IMapper mapper)
        {
            _entity = entity;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<StudentDto>>> GetStudents(
            StudentDto dto,
            int pageNumber,
            int pageSize)
        {
            var rsp = new ResponseDto<List<StudentDto>>();

            //Burada henüz tolist yapmıyoruz.
            //Öncelikle filtremizi vererek sorgumuzu hazırlıyoruz
            //skip ve take komutlarımızı uyguladıktan sonra tolist yaptığımızda verileri çekmiş olacağız
            //Eğer verilerimizi başlangıçta çekip daha sonra filtre, skip ve take uygulasaydık, filtresiz bir şekilde
            //tüm verileri çekmeye çalışıp sunucumuzda ağırlığa sebep  olacaktı. 

            IQueryable<Students> query = _entity.Students;
            if (!string.IsNullOrEmpty(dto.UniqueId))
            {
                query = query.Where(x => x.UniqueId == dto.UniqueId);
            }

            if (!string.IsNullOrEmpty(dto.FirstName))
            {
                query = query.Where(x => x.FirstName == dto.FirstName);
            }

            if (!string.IsNullOrEmpty(dto.LastName))
            {
                query = query.Where(x => x.LastName == dto.LastName);
            }

            if (!string.IsNullOrEmpty(dto.PlaceOfBirth))
            {
                query = query.Where(x => x.PlaceOfBirth == dto.PlaceOfBirth);
            }

            if (dto.BirthDate != null)
            {
                query = query.Where(x => x.BirthDate == dto.BirthDate);
            }

            // Toplam kayıt sayısını al
            int totalRecords = query.Count();

            //Paging işlemini uygula
            //Eğer skip ve take işlemi olmasaydı, herhangi bir filtre uygulanmadığında, verilerin tamamını getirmeye çalışacaktı.
            //Bu da sunucumuza ağırlık yapacaktı.
            var pagedStudents = query
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);


            rsp.Data = _mapper.Map<List<StudentDto>>(pagedStudents);
            rsp.ResultStatus = ResultStatus.Success;
            rsp.SuccessMessage = "Veriler listelendi";
            rsp.TotalRecords = totalRecords;
            rsp.PageNumber = pageNumber;
            rsp.PageSize = pageSize;

            return rsp;
        }

        public async Task<ResponseDto<bool>> Add(StudentDto dto)
        {
            var rsp = new ResponseDto<bool>();

            //Validation için FluentValidation kütüphanesi ekledim.
            //Kurallarımızı da StudentValidator'da ekledim.
            var validator = new StudentValidator();
            var validationResult = validator.Validate(dto);

            if (validationResult.IsValid)
            {
                var student = _entity.Students.FirstOrDefault(x => x.UniqueId == dto.UniqueId);
                if (student == null)
                {
                    _entity.Students.Add(new Students
                    {
                        UniqueId = dto.UniqueId,
                        FirstName = dto.FirstName.Trim(),
                        LastName = dto.LastName.Trim(),
                        BirthDate = dto.BirthDate,
                        PlaceOfBirth = dto.PlaceOfBirth.Trim(),
                        RegistrationDateTime = DateTime.Now
                    });

                    _entity.SaveChanges();

                    rsp.Data = true;
                    rsp.ResultStatus = ResultStatus.Success;
                    rsp.SuccessMessage = dto.FirstName + " " + dto.LastName + " isimli öğrenci başarıyla sisteme kaydedilmiştir";
                }
                else
                {
                    rsp.Data = false;
                    rsp.ResultStatus = ResultStatus.Error;
                    rsp.ErrorMessage = "Sistemde bu bilgilerle eşleşen başka bir kayıt bulunmaktadır. Lütfen girmek istediğiniz kaydı kontrol ediniz.";
                }
            }
            else
            {
                rsp.ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                rsp.ResultStatus = ResultStatus.Error;
            }

            return rsp;
        }

        public async Task<ResponseDto<bool>> Update(StudentDto dto)
        {
            var rsp = new ResponseDto<bool>();
            var student = _entity.Students.FirstOrDefault(x => x.UniqueId == dto.UniqueId);
            if (student != null)
            {
                student.UniqueId = dto.UniqueId;
                student.FirstName = dto.FirstName;
                student.LastName = dto.LastName;
                student.PlaceOfBirth = dto.PlaceOfBirth;
                student.BirthDate = dto.BirthDate;
                _entity.SaveChanges();

                rsp.Data = true;
                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = "Değişiklikler başarıyla kaydedildi";
            }
            else
            {
                rsp.Data = false;
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Hata oluştu";
            }

            return rsp;
        }

        public async Task<ResponseDto<bool>> Delete(int Id)
        {
            var rsp = new ResponseDto<bool>();
            var user = _entity.Students.FirstOrDefault(x => x.Id == Id);
            if(user!=null)
            {
                _entity.Students.Remove(user);
                _entity.SaveChanges();

                rsp.Data = true;
                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = "Öğrenci kaydı başarıyla silindi.";
            }
            else
            {
                rsp.Data = false;
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Hata oluştu";
            }

            return rsp;
        }
    }
}
