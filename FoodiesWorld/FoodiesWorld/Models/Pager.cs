namespace FoodiesWorld.Models
{
    public class Pager
    {
        public int TotalItems {  get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int TotalPage { get; private set; }


        //Controller and Action for more universal use
        public string Controller { get; private set; }
        public string Action { get; private set; }

        public Dictionary<string, string> RouteData { get; set; } //Dictionary for aditional arguments
        public Pager()
        {
            
        }

        public Pager(int totalItems,int page,string controller, string action, Dictionary<string, string> routeData, int pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            int currentPage = page;

            int startPage = currentPage - 2;
            int endPage = currentPage + 2;

            if(startPage <= 0)
            {
                endPage = endPage - startPage - 1;
                startPage = 1;
            }

            if(endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 5)
                {
                    startPage = endPage - 5;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPage = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            Controller = controller;
            Action = action;
            RouteData = routeData;

        }
    }
}
