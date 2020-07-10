using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.Test.Utils;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Auth.Test.DataUtils
{
    public class AccountDataUtil : BaseDataUtil<Account, AccountViewModel, AccountService>
    {
        public AccountDataUtil() : base()
        {

        }
        public AccountDataUtil(AccountService service) : base(service)
        {

        }

        public override Account GetNewData()
        {
            return new Account(){
                Username = "username",
                Password = "password",
                IsLocked = false,
                AccountProfile = new AccountProfile(){
                    Dob = DateTimeOffset.UtcNow,
                    Email = "email",
                    Gender = "male",
                    Firstname = "firstname",
                    Lastname = "lastname"
                },
                AccountRoles = new List<AccountRole>(){
                    new AccountRole(){
                        Role =new Role(){
                            Description="Description"
                        },
                    }
                }
            };
        }

        public Account GetDataInput(){
            return new Account(){
                Username = "username",
                Password = "password",
                IsLocked = false,
                UId = "UId",
                AccountProfile = new AccountProfile(){
                    Dob = DateTimeOffset.UtcNow,
                    Email = "email",
                    Gender = "male",
                    Firstname = "firstname",
                    Lastname = "lastname",
                    UId= "UId",
                },
                AccountRoles = new List<AccountRole>(){
                    new AccountRole(){
                        RoleId =2,
                        Role =new Role(){   
                            Name="Name",
                            UId="UId",
                            Id =2,
                            Description="Description"
                        }
                    }
                }
            };
        }

        public override AccountViewModel GetNewViewModel()
        {
            return new AccountViewModel()
            {
                username = "username",
                password = "password",
                isLocked = false,
                profile = new AccountProfileViewModel()
                {
                    dob = DateTimeOffset.UtcNow,
                    email = "email",
                    firstname = "firstname",
                    gender = "gender",
                    lastname = "lastname"
                },
                roles = new List<RoleViewModel>()
            };
        }
    }
}
