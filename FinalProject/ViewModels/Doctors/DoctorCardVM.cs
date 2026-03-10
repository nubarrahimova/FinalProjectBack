namespace FinalProject.ViewModels.Doctors
{
    public class DoctorCardVM
    {
        public int Id { get; set; }
        public string Slug { get; set; } = "";
        public string FullName { get; set; } = "";
        public string SpecialityName { get; set; } = "";
        public int ExperienceYears { get; set; }
        public string Clinic { get; set; } = "";
        public string PhotoUrl { get; set; } = "";

        public DoctorCardVM()
        {
        }

        public DoctorCardVM(
            int id,
            string slug,
            string fullName,
            string specialityName,
            int experienceYears,
            string clinic,
            string photoUrl)
        {
            Id = id;
            Slug = slug;
            FullName = fullName;
            SpecialityName = specialityName;
            ExperienceYears = experienceYears;
            Clinic = clinic;
            PhotoUrl = photoUrl;
        }
    }
}