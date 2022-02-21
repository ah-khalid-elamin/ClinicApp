using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    public class Doctor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Speciality { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
