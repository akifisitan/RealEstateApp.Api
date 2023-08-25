namespace RealEstateApp.Api.DTO
{
    public class GenericResponse<T> where T : class
    {
        public string Message { get; set; }
        public T? Data { get; set; }

        public GenericResponse()
        {
            Message = string.Empty;
        }

        public GenericResponse(string message, T data)
        {
            Message = message;
            Data = data;
        }
    }
}
