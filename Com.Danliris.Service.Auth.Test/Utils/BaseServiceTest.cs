using Com.Danliris.Service.Auth.Lib;
using Com.Danliris.Service.Auth.Lib.Services.IdentityService;
using Com.Danliris.Service.Auth.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Auth.Lib.Utilities.BaseInterface;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Auth.Test.Utils
{
    public abstract class BaseServiceTest<TModel, TViewModel, TService, TDataUtil>
        where TModel : StandardEntity, new()
        where TService : class, IBaseService<TModel>
        where TViewModel : BaseOldViewModel, IValidatableObject, new()
        where TDataUtil : BaseDataUtil<TModel, TViewModel, TService>
    {
        protected readonly string ENTITY;
        public BaseServiceTest(string entity)
        {
            ENTITY = entity;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        protected string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }

        protected AuthDbContext _dbContext(string testName)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            DbContextOptionsBuilder<AuthDbContext> optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                 .UseInternalServiceProvider(serviceProvider);

            AuthDbContext dbContext = new AuthDbContext(optionsBuilder.Options);

            return dbContext;
        }

        protected virtual Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset=1 });


            return serviceProvider;
        }

        protected TDataUtil _dataUtil(TService service)
        {

            GetServiceProvider();
            TDataUtil dataUtil = (TDataUtil)Activator.CreateInstance(typeof(TDataUtil), service);
            return dataUtil;
        }

        protected TService GetService(IServiceProvider serviceProvider, AuthDbContext dbContext)
        {
            TService service = (TService)Activator.CreateInstance(typeof(TService), serviceProvider, dbContext);

            return service;
        }

        [Fact]
        public async void Should_Success_Get_Data()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            await _dataUtil(service).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async void Should_Success_Get_Data_By_Id()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var model = await _dataUtil(service).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public virtual async void Should_Success_Create_Data()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentAsyncMethod()));

            var model = _dataUtil(service).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public virtual void Should_Success_Validate_All_Null_Data()
        {
            TViewModel vm = new TViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async void Should_Success_Update_Data()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var model = await _dataUtil(service).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            newModel = OnUpdating(newModel);

            var Response = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async void Should_Success_Delete_Data()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var model = await _dataUtil(service).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            var deletedModel = await service.ReadByIdAsync(newModel.Id);
            Assert.Null(deletedModel);
        }

        protected abstract TModel OnUpdating(TModel model);

    }
}
