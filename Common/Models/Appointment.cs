using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public bool Confirmed { get; set; }

        public override String ToString()
        {
            return $"{Id}, {StartDate}, {EndDate}, {Doctor}, {Patient}, {Confirmed}";
        }
    }
}
