using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Utilities
{
    public class ServiceValidationException : ServiceValidationExeption
    {
        public ServiceValidationException(ValidationContext validationContext, IEnumerable<ValidationResult> validationResults) : base("Validation Error", validationContext, validationResults)
        {

        }
    }
}
