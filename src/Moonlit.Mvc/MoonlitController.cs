using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Templates;
using Newtonsoft.Json;

namespace Moonlit.Mvc
{
    public abstract class MoonlitController : Controller
    {
        public IFlash Flash { get; set; }


        public void SetFlash(object target)
        {
            Flash.Set(target);
        }
        public async Task SetFlashAsync(object target)
        {
            await Flash.SetAsync(target).ConfigureAwait(false);
        }



        protected virtual ActionResult Template(Template template)
        {
            return new TemplateResult(template, ViewData)
            {
                TempData = TempData,
                ViewEngineCollection = ViewEngineCollection,
            };
        }

        protected bool ValidateAs<T>(T entity, params  string[] properties)
        {
            ModelState.Clear();
            var metadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>)(() => entity), typeof(T));
            var modelValidator = ModelValidator.GetModelValidator(metadata, this.ControllerContext);
            foreach (ModelValidationResult validationResult in modelValidator.Validate((object)null))
            {
                if (properties.Any(x => string.Equals(x, validationResult.MemberName, StringComparison.OrdinalIgnoreCase)))
                {
                    this.ModelState.AddModelError(validationResult.MemberName, validationResult.Message);
                }
            }
            return ModelState.IsValid;
        }

        protected bool TryUpdateModel<TValidationAs, T>(T entity, TValidationAs model)
            where TValidationAs : IEntityMapper<T>
        {
            model.ToEntity(entity);
            return ValidateAs<TValidationAs, T>(entity);
        }
        protected bool ValidateAs<TValidationAs, T>(T entity)
        {
            var properties = ModelMetadataProviders.Current.GetMetadataForProperties(null, typeof(TValidationAs));

            return ValidateAs(entity, properties.Where(x => !x.IsReadOnly).Select(x => x.PropertyName).ToArray());
        }
    }
    public interface IEntityMapper<T>
    {
        void ToEntity(T entity);
    }
}
