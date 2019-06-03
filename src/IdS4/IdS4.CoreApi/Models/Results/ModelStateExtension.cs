using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace IdS4.CoreApi.Models.Results
{
    public static class ModelStateExtension
    {
        public static ApiResult ToApiResult(this ModelStateDictionary self)
        {
            if (self == null) 
                throw new ArgumentException(nameof(self));

            return ApiResult.Failure(self);
        }
    }
}
