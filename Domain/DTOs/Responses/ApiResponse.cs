namespace Kepas.Core.Api.Domain.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponse<T> Ok(T data, string message = null, string code = "SUCCESS") 
            => new ApiResponse<T> { Success = true, Data = data, Message = message, Code = code };

        public static ApiResponse<T> Error(string message, string code = "INTERNAL_SERVER_ERROR") 
            => new ApiResponse<T> { Success = false, Message = message, Code = code };
    }
}
