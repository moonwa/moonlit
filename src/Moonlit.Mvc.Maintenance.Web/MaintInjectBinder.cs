using System.Web.Mvc;
using Autofac;

namespace Moonlit.Mvc.Maintenance
{
    public class MaintInjectBinder : IModelBinder
    {
        private readonly IModelBinder _defaultBinder;
        private readonly IContainer _container;

        public MaintInjectBinder(IModelBinder defaultBinder, IContainer container)
        {
            _defaultBinder = defaultBinder;
            _container = container;
        }

        #region Implementation of IModelBinder

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = _defaultBinder.BindModel(controllerContext, bindingContext);
            IInjectModel injectModel = model as IInjectModel;
            if (injectModel != null)
            {
                _container.InjectUnsetProperties(model);
            }
            return model;
        }

        #endregion
    }
}