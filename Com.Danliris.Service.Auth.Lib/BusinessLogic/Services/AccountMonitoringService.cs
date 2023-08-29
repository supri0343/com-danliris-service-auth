using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Services.IdentityService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using OfficeOpenXml;
using Com.Danliris.Service.Auth.Lib.Helpers;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;

namespace Com.Danliris.Service.Auth.Lib.BusinessLogic.Services
{
    public class AccountMonitoringService : IAccountMonitoringService
    {
        private const string UserAgent = "auth-service";
        protected DbSet<Account> DbSet;
        protected DbSet<AccountRole> DbSetAccountRole;
        protected DbSet<Role> DbSetRole;
        protected DbSet<Permission2> DbSetPermission2;
        protected IIdentityService IdentityService;
        public AuthDbContext dbContext;

        public AccountMonitoringService(IServiceProvider serviceProvider, AuthDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.DbSet = dbContext.Set<Account>();
            this.DbSetAccountRole = dbContext.Set<AccountRole>();
            this.DbSetRole = dbContext.Set<Role>();
            this.DbSetPermission2 = dbContext.Set<Permission2>();
            this.IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public IQueryable<AccountMonitoringViewModel> GetQuery(int userId, string menu)
        {
            IQueryable<Account> Account = DbSet;
            IQueryable<Permission2> Permissionss = DbSetPermission2;

            if (userId != 0) {
                Account = DbSet.Where(x => x.Id == userId);
            }

            if (menu != null) {
                Permissionss = DbSetPermission2.Where(x => x.Menu.Contains(menu));
            }
            var query = (from a in Account
                         join b in DbSetAccountRole on a.Id equals b.AccountId
                         join c in DbSetRole on b.RoleId equals c.Id
                         join d in Permissionss on c.Id equals d.RoleId
                         where
                         !a.IsDeleted
                         && !b.IsDeleted
                         && !c.IsDeleted
                         && !d.IsDeleted
                         select new AccountMonitoringViewModel
                         {
                             UserId = a.Id,
                             UserName =a.Username,
                             Menu = d.Menu,
                             SubMenu = d.SubMenu,
                             MenuItems = d.MenuName
                         }).Distinct();

            return query.OrderBy( x => x.UserName).ThenBy(x => x.Menu);

        }
        public MemoryStream GetExcel(int userId, string menu)
        {
            var Query = GetQuery(userId, menu);
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama User", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Menu", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "SubMenu", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Menu", DataType = typeof(String) });

            ExcelPackage package = new ExcelPackage();
            if (Query.ToArray().Count() == 0)
            {
                result.Rows.Add("", "", "", "", "");

            }
               
            // to allow column name to be generated properly for empty data as template
            else
            {

               // int index = 0;
                var Qr = Query.ToArray();
                var q = Query;

                var datum = new List<AccountMonitoringViewModel>();
                var index = 0;
                foreach (AccountMonitoringViewModel a in q)
                {
                    AccountMonitoringViewModel dup = Array.Find(Qr, o => o.UserName == a.UserName);
                    if (dup != null)
                    {
                        if (dup.Count == 0)
                        {
                            index++;
                            dup.Count = index;
                        }
                    }
                    a.Count += dup.Count;

                    datum.Add(a);

                }
                //Query = q.AsQueryable();
                foreach (var item in datum)
                {
                    index++;
                    

                    result.Rows.Add(item.Count, item.UserName, item.Menu, item.SubMenu, item.MenuItems);
                }

                
                bool styling = true;
                var Data = Query;

                foreach (KeyValuePair<DataTable, String> item in new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") })
                {
                    var sheet = package.Workbook.Worksheets.Add(item.Value);
                    sheet.Cells["A1"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

                    Dictionary<string, int> counts = new Dictionary<string, int>();
                    Dictionary<string, int> countsMenu = new Dictionary<string, int>();
                    Dictionary<string, int> countSubMenu = new Dictionary<string, int>();
                    //var docNo = Data.ToArray();
                    int value;

                    foreach (var a in Data)
                    {
                        if (counts.TryGetValue(a.UserName, out value))
                        {
                            counts[a.UserName]++;
                        }
                        else
                        {
                            counts[a.UserName] = 1;
                        }

                        if (countsMenu.TryGetValue( a.UserName + a.Menu, out value))
                        {
                            countsMenu[a.UserName + a.Menu]++;
                        }
                        else
                        {
                            countsMenu[a.UserName + a.Menu] = 1;
                        }
                        if (countSubMenu.TryGetValue(a.UserName + a.Menu + a.SubMenu, out value))
                        {
                            countSubMenu[a.UserName + a.Menu + a.SubMenu]++;
                        }
                        else
                        {
                            countSubMenu[a.UserName + a.Menu + a.SubMenu] = 1;
                        }
                    }

                    //foreach (var a in Data)
                    //{
                    //    if (countsMenu.TryGetValue(a.Menu, out value))
                    //    {
                    //        countsMenu[a.Menu]++;
                    //    }
                    //    else
                    //    {
                    //        countsMenu[a.Menu] = 1;
                    //    }
                    //}

                    index = 2;
                    foreach (KeyValuePair<string, int> b in counts)
                    {
                        sheet.Cells["A" + index + ":A" + (index + b.Value - 1)].Merge = true;
                        sheet.Cells["A" + index + ":A" + (index + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        sheet.Cells["B" + index + ":B" + (index + b.Value - 1)].Merge = true;
                        sheet.Cells["B" + index + ":B" + (index + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        //sheet.Cells["C" + index + ":C" + (index + b.Value - 1)].Merge = true;
                        //sheet.Cells["C" + index + ":C" + (index + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        //sheet.Cells["D" + index + ":D" + (index + b.Value - 1)].Merge = true;
                        //sheet.Cells["D" + index + ":D" + (index + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        //var startRow = b;

                        //  sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                    }

                    
                   
                    index = 2;
                    foreach (KeyValuePair<string, int> c in countsMenu)
                    {
                        //sheet.Cells["A" + index + ":A" + (index + b.Value - 1)].Merge = true;
                        //sheet.Cells["A" + index + ":A" + (index + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        //sheet.Cells["B" + index + ":B" + (index + b.Value - 1)].Merge = true;
                        //sheet.Cells["B" + index + ":B" + (index + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        sheet.Cells["C" + index + ":C" + (index + c.Value - 1)].Merge = true;
                        sheet.Cells["C" + index + ":C" + (index + c.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        //sheet.Cells["D" + index + ":D" + (index + c.Value - 1)].Merge = true;
                        //sheet.Cells["D" + index + ":D" + (index + c.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;



                        index += c.Value;


                    }

                    

                    index = 2;
                    foreach (KeyValuePair<string, int> d in countSubMenu)
                    {
                        //sheet.Cells["A" + index + ":A" + (index + b.Value - 1)].Merge = true;
                        //sheet.Cells["A" + index + ":A" + (index + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        //sheet.Cells["B" + index + ":B" + (index + b.Value - 1)].Merge = true;
                        //sheet.Cells["B" + index + ":B" + (index + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        //sheet.Cells["C" + index + ":C" + (index + d.Value - 1)].Merge = true;
                        //sheet.Cells["C" + index + ":C" + (index + d.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        sheet.Cells["D" + index + ":D" + (index + d.Value - 1)].Merge = true;
                        sheet.Cells["D" + index + ":D" + (index + d.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;



                        index += d.Value;


                    }

                    sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                }
            }

            //ExcelPackage package = new ExcelPackage();
            //foreach (KeyValuePair<DataTable, String> item in result)
            //{
            //    var sheet = package.Workbook.Worksheets.Add(item.Value);
            //    sheet.Cells["A1"].LoadFromDataTable(item.Key, true, (true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
            //    sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            //}
            //MemoryStream stream = new MemoryStream();
            //package.SaveAs(stream);
            //return stream;

            //return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") }, true);

            

            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;


        }
    }
}
