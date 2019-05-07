using Com.Danliris.Service.Auth.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Auth.Lib.Utilities.BaseInterface;
using Com.Moonlay.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Auth.Test.DataUtils
{
    public abstract class BaseDataUtil<TModel, TViewModel, TService>
        where TModel : StandardEntity, IValidatableObject, new()
        where TViewModel : BaseOldViewModel, IValidatableObject, new()
        where TService : class, IBaseService<TModel>
    {
        private readonly TService Service;
        public BaseDataUtil()
        {

        }
        public BaseDataUtil(TService service)
        {
            Service = service;
        }

        public async Task<TModel> GetTestData()
        {
            TModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        public abstract TModel GetNewData();

        public abstract TViewModel GetNewViewModel();
    }
}
