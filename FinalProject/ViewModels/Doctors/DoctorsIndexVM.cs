using FinalProject.Models;

namespace FinalProject.ViewModels.Doctors
{
    public class DoctorsIndexVM
    {
        public string CityTitle { get; set; } = "";
        public List<DoctorCardVM> Doctors { get; set; } = new();

        public string? Search { get; set; }
        public int? SpecialtyId { get; set; }
        public string? SortBy { get; set; }

        public List<Speciality> Specialities { get; set; } = new();
    }
}