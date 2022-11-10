using System;

namespace apps.exception
{
    public class ArrayHasObjectNullException : ArgumentNullException
    {
        public ArrayHasObjectNullException(string message = "One of objects in array has null value.") : base(message)
        {

        }
    }
}
