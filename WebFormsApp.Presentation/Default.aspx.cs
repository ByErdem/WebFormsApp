using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using WebFormsApp.Data;
using WebFormsApp.Entity.Dtos;
using WebFormsApp.Presentation.Models;
using WebFormsApp.Service.Abstract;

namespace WebFormsApp.Presentation
{
    public partial class _Default : Page
    {
        private readonly IStudentService _studentService;

        public _Default()
        {
            _studentService = DependencyResolverHelper.Resolve<IStudentService>();
        }

        private static _Default GetInstance()
        {
            return (_Default)HttpContext.Current.Handler;
        }

        [WebMethod]
        public static async Task<ResponseDto<List<StudentDto>>> GetAll(StudentDto dto)
        {
            var instance = GetInstance();
            var rsp = await instance._studentService.GetStudents(dto, dto.PageNumber, dto.PageSize);
            return rsp;
        }

        [WebMethod]
        public static async Task<ResponseDto<bool>> Update(StudentDto dto)
        {
            var instance = GetInstance();
            var rsp = await instance._studentService.Update(dto);
            return rsp;
        }

        [WebMethod]
        public static async Task<ResponseDto<bool>> Add(StudentDto dto)
        {
            var instance = GetInstance();
            var rsp = await instance._studentService.Add(dto);
            return rsp;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}