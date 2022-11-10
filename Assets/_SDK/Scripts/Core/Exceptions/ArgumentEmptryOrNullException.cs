namespace apps.exception
{
    public class ArgumentEmptryOrNullException : System.ArgumentException
    {
        public ArgumentEmptryOrNullException(string message = "The argument is Emptry Or Null.") : base(message)
        {

        }
    }
}
