using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    public class Doctor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Speciality { get; set; }
        public DateTime BirthDate { get; set; }

        
        public override String ToString()
        {
            return $"{Id}, {Name}, {Speciality}, {BirthDate}";
        }
    }
}
