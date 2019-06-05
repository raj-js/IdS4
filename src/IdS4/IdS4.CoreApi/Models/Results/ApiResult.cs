using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace IdS4.CoreApi.Models.Results
{
    public class ApiResult
    {
        public ApiResultCode Code { get; set; }

        public Dictionary<string, object> Errors { get; set; } = new Dictionary<string, object>();

        public void AddError(object error)
        {
            Errors.Add(string.Empty, error);
        }

        public void AddError(string key, object error)
        {
            Errors.Add(key, error);
        }

        public static ApiResult Success()
        {
            return new ApiResult
            {
                Code = ApiResultCode.Success
            };
        }

        public static ApiResult Success<T>(T data = default)
        {
            return new ApiResult<T>
            {
                Code = ApiResultCode.Success,
                Data = data
            };
        }

        public static ApiResult Failure(ApiResultCode code = ApiResultCode.Failure, Dictionary<string, object> errors = null)
        {
            var result = new ApiResult { Code = code };

            if (errors != null)
                result.Errors = errors;

            return result;
        }

        public static ApiResult Failure(ModelStateDictionary modelState)
        {
            var result = new ApiResult { Code = ApiResultCode.InValidForm };

            foreach (var model in modelState)
            {
                RetrieveErrors(result, model.Key, model.Value);
            }

            return result;
        }

        public static ApiResult NotFound(object id)
        {
            return new ApiResult
            {
                Code = ApiResultCode.NotFound,
                Errors =
                {
                    { nameof(ApiResultCode.NotFound), $"{id} not found" } 
                }
            };
        }

        private static void RetrieveErrors(ApiResult res, string key, ModelStateEntry entry)
        {
            if (entry.Errors == null) return;

            foreach (var error in entry.Errors)
            {
                res.AddError(key, error.Exception == null ? error.ErrorMessage : error.Exception.ToString());
            }
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }

    public enum ApiResultCode
    {
        Failure = 0,
        Success = 1,
        InValidForm = 2,
        NotFound = 3
    }
}
