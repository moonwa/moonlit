using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Moonlit.Mvc.Templates
{
    public class ControlBuilderCriteria
    {
        private readonly Predicate<ModelMetadata> _func;

        public ControlBuilderCriteria(Predicate<ModelMetadata> func, IControllBuilder controllBuilder)
        {
            ControllBuilder = controllBuilder;
            _func = func;
        }

        public bool IsSupport(ModelMetadata metadata)
        {
            return _func(metadata);
        }

        public IControllBuilder ControllBuilder { get; private set; }
    }
}