namespace FinalProject.Areas.AdminPanel.ViewModels
{
    public class DashboardVM
    {
        public int TotalAppointments { get; set; }
        public int NewAppointments { get; set; }
        public int ApprovedAppointments { get; set; }
        public int RejectedAppointments { get; set; }

        public int TotalArticles { get; set; }
        public int TotalDoctors { get; set; }

        public bool ShowDoctorColumn { get; set; }

        public int CurrentYear { get; set; }

        public List<string> MonthlyLabels { get; set; } = new();
        public List<int> MonthlyAppointmentCounts { get; set; } = new();

        public List<DashboardRecentAppointmentVM> RecentAppointments { get; set; } = new();
    }
}