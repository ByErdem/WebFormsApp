using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFormsApp.Data;
using WebFormsApp.Entity.Dtos;

namespace WebFormsApp.Service.Abstract
{
    public interface IStudentService
    {
        Task<ResponseDto<List<StudentDto>>> GetStudents(StudentDto dto, int pageNumber, int pageSize);
        Task<ResponseDto<bool>> Add(StudentDto dto);
        Task<ResponseDto<bool>> Update(StudentDto dto);
    }
}
