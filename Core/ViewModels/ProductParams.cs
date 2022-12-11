
namespace Core.ViewModels
{
    public class ProductParams
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                if(value != null)
                {
                    _search = value.ToLower();
                }
                else
                {
                    _search = value;
                }
            }
        }
    }
}
