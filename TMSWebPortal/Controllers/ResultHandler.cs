namespace TMSWebPortal.Controllers
{
    public class ResultHandler
    {
        public bool IsErrorOccured { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public bool IsErrorKnown { get; set; }
    }
}