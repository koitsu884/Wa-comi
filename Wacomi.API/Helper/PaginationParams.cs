namespace Wacomi.API.Helper
{
    public class PaginationParams
    {
        private const int MaxPageSize = 100;
        public int pageNumber = 1;
        public int PageNumber {
            get { return pageNumber;}
            set { pageNumber = (value < 1) ? 1 : value;}
        }
        private int pageSize = 3;

        public int PageSize{
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value ;}
        }
    }
}