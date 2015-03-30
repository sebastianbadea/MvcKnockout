using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcKo.Web.ViewModels
{
    public class ModelStateException: Exception
    {
        public ModelStateException(ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentException("modelState");
            }

            Errors = new Dictionary<string, string>();
            if (!modelState.IsValid)
            {
                StringBuilder errors;
                foreach (var state in modelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        errors = new StringBuilder();
                        foreach (var error in state.Value.Errors)
                        {
                            errors.Append(error.ErrorMessage);
                        }
                        Errors.Add(state.Key, errors.ToString());
                    }
                }
            }
        }

        public ModelStateException(Exception e)
        {
            var message =
                (e.InnerException != null && e.InnerException.InnerException != null) ?
                e.InnerException.InnerException.Message :
                e.Message;
            Errors = new Dictionary<string, string>();
            Errors.Add(string.Empty, message);
        }
        public Dictionary<string, string> Errors { get; set; }

        public override string Message
        {
            get
            {
                if (Errors.Count > 0)
                {
                    return string.Join("|", Errors.ToArray());
                }
                return null;
            }
        }
    }
}