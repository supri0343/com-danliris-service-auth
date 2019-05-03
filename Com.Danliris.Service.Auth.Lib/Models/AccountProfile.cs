using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Models
{
    public class AccountProfile : StandardEntity, IValidatableObject
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string Email { get; set; }

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
