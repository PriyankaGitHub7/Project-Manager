namespace ProjectManager.Models
{
    public class ResponseModel<T>
    {
        public string Message { get; set; }

        public bool IsSuccessful { get; set; }

        public T ResultSet { get; set; }
    }
}
