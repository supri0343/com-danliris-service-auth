using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces
{
    public interface IAccountService : IBaseService<Account>
    {
        Task<Account> Authenticate(string username, string password);
    }
}
