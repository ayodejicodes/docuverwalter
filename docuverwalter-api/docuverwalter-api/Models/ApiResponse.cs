using System.Net;

namespace docuverwalter_api.Models
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public List<string> ErrorMessages { get; set; }

        public ApiResponse(HttpStatusCode statusCode, T data, bool isSuccess, List<string> errorMessages = null)
        {
            StatusCode = statusCode;
            Data = data;
            IsSuccess = isSuccess;
            ErrorMessages = errorMessages ?? new List<string>();
        }


        public static ApiResponse<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponse<T>(statusCode, data, true);
        }

        public static ApiResponse<T> Failure(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ApiResponse<T>(statusCode, default, false, errors);
        }
    }
}
