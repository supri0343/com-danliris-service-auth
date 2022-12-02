using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Utilities.BaseClass;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;

namespace Com.Danliris.Service.Auth.Lib.ViewModels
{
    public class AccountViewModel : BaseOldViewModel, IValidatableObject
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool isLocked { get; set; }
        public AccountProfileViewModel profile { get; set; }
        public List<RoleViewModel> roles { get; set; }
        //public ICollection<Permission2ViewModel> permissions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int Count = 0;

            if (string.IsNullOrWhiteSpace(this.username))
                yield return new ValidationResult("Username is required", new List<string> { "username" });

            if (this._id.Equals(0) && string.IsNullOrWhiteSpace(this.password))
                yield return new ValidationResult("Password is required", new List<string> { "password" });

            if (this.profile == null || string.IsNullOrWhiteSpace(this.profile.firstname))
                yield return new ValidationResult("{ firstname: 'First Name is required' }", new List<string> { "profile" });

            /* Service Validation */
            var service = validationContext.GetService<IAccountService>();

            if (service.CheckDuplicate(_id, username)) /* Unique */
            {
                yield return new ValidationResult("Username already exists", new List<string> { "username" });
            }

            string accountRoleError = "[";
            if (roles != null)
            {
                if (roles.GroupBy(x => x._id).Any(x => x.Count() > 1))
                {
                    yield return new ValidationResult("{ roles: 'Cannot have duplicate role' }", new List<string> { "roles" });
                }



                foreach (RoleViewModel role in roles)
                {
                    if (role._id.Equals(0))
                    {
                        Count++;
                        accountRoleError += "{ name: 'Role is required' }, ";
                    }
                    else
                    {
                        accountRoleError += "{}, ";
                    }
                }


            }
        accountRoleError += "]";
            
            if (Count > 0)
            {
                yield return new ValidationResult(accountRoleError, new List<string> { "roles" });
            }
        }
    }
}
