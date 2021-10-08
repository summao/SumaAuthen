using System;

namespace Suma.Authen.Exceptions
{
    public class SignInException : Exception
    {
        public SignInException(string message) : base(message)
        {

        }
    }
}