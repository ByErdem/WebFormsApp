using System;

namespace WebFormsApp.Entity.Dtos
{
    public class StudentDto : PageInfo
    {
        public int? Id { get; set; }
        public string UniqueId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime? RegistrationDateTime { get; set; } 
    }
}
