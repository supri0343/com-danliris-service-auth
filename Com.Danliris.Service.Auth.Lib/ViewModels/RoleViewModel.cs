using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;

namespace Com.Danliris.Service.Auth.Lib.ViewModels
{
    public class RoleViewModel : BaseOldViewModel, IValidatableObject
    {
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public ICollection<Permission2ViewModel> permissions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int Count = 0;

            //if (string.IsNullOrWhiteSpace(this.code))
            //    yield return new ValidationResult("Code is required", new List<string> { "code" });

            if (string.IsNullOrWhiteSpace(this.name))
                yield return new ValidationResult("Name is required", new List<string> { "name" });


            if (string.IsNullOrWhiteSpace(this.description))
                yield return new ValidationResult("Description is required", new List<string> { "description" });

            /* Service Validation */
            var service = validationContext.GetService<IRoleService>();

            //if (service.CheckDuplicate(_id, code)) /* Unique */
            //{
            //    yield return new ValidationResult("Code already exists", new List<string> { "code" });
            //}

            string permissionError = "[";

            if(permissions != null)
            {
                foreach (Permission2ViewModel permission in permissions)
                {
                    if (string.IsNullOrWhiteSpace(permission.Code))
                    {
                        Count++;
                        permissionError += "{ menu: 'Menu is required' }, ";
                    }
                    else
                    {
                        permissionError += "{}, ";
                    }
                }
            }
            

            permissionError += "]";

            if (Count > 0)
            {
                yield return new ValidationResult(permissionError, new List<string> { "unit" });
            }
        }
    }
}
