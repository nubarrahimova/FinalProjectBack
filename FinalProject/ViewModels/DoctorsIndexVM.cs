namespace FinalProject.ViewModels
{
    public class DoctorsIndexVM
    {
        public string CityTitle { get; set; } = "";
        public List<DoctorCardVM> Doctors { get; set; } = new();
    }

    public record DoctorCardVM(
        string Slug,
        string FullName,
        string Speciality,
        int ExperienceYears,
        string Clinic,
        string PhotoUrl
    );
}
