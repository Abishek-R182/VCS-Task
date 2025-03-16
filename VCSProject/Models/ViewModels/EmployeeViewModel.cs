namespace VCSProject.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public Employeedto Employeedto { get; set; } = new Employeedto();
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
