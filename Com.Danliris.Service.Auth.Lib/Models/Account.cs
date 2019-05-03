using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Models
{
    public class Account : StandardEntity, IValidatableObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLocked { get; set; }
        public virtual AccountProfile AccountProfile { get; set; }
        public virtual ICollection<AccountRole> AccountRoles { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            /* Service Validation */
            BusinessLogic.Services.AccountService service = (BusinessLogic.Services.AccountService)validationContext.GetService(typeof(BusinessLogic.Services.AccountService));

            if (service.DbContext.Set<Account>().Count(r => r.IsDeleted.Equals(false) && r.Id != this.Id && r.Username.Equals(this.Username)) > 0) /* Unique */
            {
                yield return new ValidationResult("Username already exists", new List<string> { "username" });
            }
        }
    }
}
