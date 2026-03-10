namespace FinalProject.Areas.AdminPanel.ViewModels
{
    public class DashboardRecentAppointmentVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public DateTime Date { get; set; }
        public string Time { get; set; } = "";
        public string Status { get; set; } = "";
        public string? DoctorName { get; set; }
    }
}