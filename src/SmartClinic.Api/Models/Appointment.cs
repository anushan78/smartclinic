namespace SmartClinic.Api.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Doctor { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}