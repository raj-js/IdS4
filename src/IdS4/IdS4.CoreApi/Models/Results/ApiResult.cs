﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace IdS4.CoreApi.Models.Results
{
    public class ApiResult
    {
        public ApiResultCode Code { get; set; }

        public Dictionary<string, object> Errors { get; set; } = new Dictionary<string, object>();

        public string Msg { get; set; }

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

        public static ApiResult Success<T>(T data = default, string msg = null)
        {
            return new ApiResult<T>
            {
                Code = ApiResultCode.Success,
                Data = data,
                Msg = msg
            };
        }

        public static ApiResult Failure(ApiResultCode code = ApiResultCode.Failure, params KeyValuePair<string, object>[] errors)
        {
            var result = new ApiResult { Code = code };

            errors?.ToList()
                .ForEach(s => result.AddError(s.Key, s.Value));

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

        public static ApiResult Failure(IEnumerable<IdentityError> errors)
        {
            var result = new ApiResult { Code = ApiResultCode.Failure };

            if (errors == null) return result;

            foreach (var error in errors)
            {
                result.Errors.Add(error.Code, error.Description);
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
