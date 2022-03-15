using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    public class Patient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }

        public override String ToString()
        {
            return $"{Id}, {Name}, {BirthDate}, {Gender}";
        }
    }
}
