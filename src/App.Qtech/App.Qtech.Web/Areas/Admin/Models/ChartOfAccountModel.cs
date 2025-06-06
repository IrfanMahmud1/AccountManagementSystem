namespace App.Qtech.Web.Areas.Admin.Models
{
    public class ChartOfAccountModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }
        public List<ChartOfAccountModel> Children { get; set; } = new();
    }

}
