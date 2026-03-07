namespace FinalProject.ViewModels
{
    public class DoctorDetailsVM
    {
        public string Slug { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Speciality { get; set; } = "";
        public int ExperienceYears { get; set; }
        public string Clinic { get; set; } = "";
        public string PhotoUrl { get; set; } = "";

        public DateTime? LastPeriodDate { get; set; }
        public int CycleLength { get; set; } = 28;
        public int PeriodLength { get; set; } = 5;

        public List<ScheduleItemVM> Schedule { get; set; } = new();
        public List<string> Services { get; set; } = new();
        public List<string> Biography { get; set; } = new();
    }

    public class ScheduleItemVM
    {
        public string Day { get; set; } = "";
        public string Hours { get; set; } = "";
    }
}