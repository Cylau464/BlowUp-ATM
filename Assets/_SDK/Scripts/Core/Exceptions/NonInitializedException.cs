namespace apps.exception
{
    public class NonInitializedException : System.Exception
    {
        public NonInitializedException(string message = "The object is not intialized, initialize first.") : base(message)
        {

        }
    }
}