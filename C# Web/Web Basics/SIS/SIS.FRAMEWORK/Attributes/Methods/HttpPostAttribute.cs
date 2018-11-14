﻿namespace SIS.FRAMEWORK.Attributes.Methods
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return requestMethod.ToUpper() == "POST";
        }
    }
}