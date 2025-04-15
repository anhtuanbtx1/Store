using Microsoft.AspNetCore.Mvc;
using static Store.Enum.EnumResponse;

namespace Store.Models
{
    public class HTTPResponseModel
    {
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _message = string.Empty;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private object _value = null;
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private object _data = null;
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private Meta _meta = null;
        public Meta Meta
        {
            get { return _meta; }
            set { _meta = value; }
        }

        public HTTPResponseModel(REPONSE_ENUM status, string message, object value, object data, Meta meta)
        {
            this.Status = (int)status;
            this.Message = message;
            this.Value = value;
            this.Data = data;
            this.Meta = meta;
        }

        public HTTPResponseModel(REPONSE_ENUM status, string message, object value, object data)
        {
            this.Status = (int)status;
            this.Message = message;
            this.Value = value;
            this.Data = data;
        }

        public HTTPResponseModel(REPONSE_ENUM status, string message, object value)
        {
            this.Status = (int)status;
            this.Message = message;
            this.Value = value;
            this.Data = null;
        }

        public HTTPResponseModel(REPONSE_ENUM status, string message)
        {
            this.Status = (int)status;
            this.Message = message;
            this.Value = null;
            this.Data = null;
        }

        public static HTTPResponseModel Make(REPONSE_ENUM status, string message)
        {
            return new HTTPResponseModel(status, message);
        }

        public static HTTPResponseModel Make(REPONSE_ENUM status, string message, object value, object data)
        {
            return new HTTPResponseModel(status, message, value, data);
        }

        public static HTTPResponseModel Make(REPONSE_ENUM status, string message, object value, object data, Meta meta)
        {
            return new HTTPResponseModel(status, message, value, data, meta);
        }

        public static HTTPResponseModel Make(REPONSE_ENUM status, string message, object value)
        {
            return new HTTPResponseModel(status, message, value);
        }
    }

    public class Meta
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }

    public class Paging
    {
        [FromQuery]
        public int Page { get; set; }
        [FromQuery]
        public int PageSize { get; set; }
    }
}

