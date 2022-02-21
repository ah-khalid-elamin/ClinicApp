using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
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
    }
}
