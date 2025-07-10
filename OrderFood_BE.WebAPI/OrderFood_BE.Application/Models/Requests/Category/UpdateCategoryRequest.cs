namespace OrderFood_BE.Application.Models.Requests.Category
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}