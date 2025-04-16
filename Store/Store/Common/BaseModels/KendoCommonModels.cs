using Store.Common.ConfigModel;

namespace Store.Common.BaseModels
{
    #region Paging
    public class PagingModel
    {
        public int pageNumber { get; set; } = DefaultConfig.DefaultPageNumber;
        public int pageSize { get; set; } = DefaultConfig.DefaultPageSize;
    }
    #endregion
    #region RESPONSE
    public class JsonResultPaging<T> : PagingModel where T : class
    {
        public T data { get; set; }
        public int total { get; set; }
    }
   

    public class SearchPagingModel<T> : PagingModel
    {
      
    }
    #endregion
    #region RESPONSE SERVICE
   
    #endregion

    #region 
    public class KendoDropdownListModel<T>
    {
        public string Value { get; set; }
        public string Text { get; set; }
      
    }
    public class DropdownListModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    #endregion
}
