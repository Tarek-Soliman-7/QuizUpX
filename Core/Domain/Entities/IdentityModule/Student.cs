using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.IdentityModule
{
    public class Student
    {


        public int Id { get; set; }
        public string UniversityCode { get; set; } = null!;
        public string Pin { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }

        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();


    }
}
