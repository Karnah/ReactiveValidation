using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReactiveValidation.Helpers;

namespace ReactiveValidation
{
    public class ValidationBuilder<TObject> : IValidationBuilder<TObject>, IObjectValidatorBuilder<TObject>
        where TObject : IValidatableObject
    {
        private readonly List<PropertiesAdaptersBuilder> _adaptersBuilders;

        public ValidationBuilder()
        {
            _adaptersBuilders = new List<PropertiesAdaptersBuilder>();
        }


        public ISinglePropertyRuleBuilderInitial<TObject, TProp> RuleFor<TProp>(Expression<Func<TObject, TProp>> property)
        {
            var propertyName = GetPropertyNameForValidator(property);
            var ruleBuilder = new SinglePropertyRuleBuilder<TObject, TProp>();

            _adaptersBuilders.Add(new PropertiesAdaptersBuilder(ruleBuilder, propertyName));

            return ruleBuilder;
        }

        public ISinglePropertyRuleBuilderInitial<TObject, object> RuleFor(string propertyName)
        {
            var ruleBuilder = new SinglePropertyRuleBuilder<TObject, object>();
            _adaptersBuilders.Add(new PropertiesAdaptersBuilder(ruleBuilder, propertyName));

            return ruleBuilder;
        }


        public IPropertiesRuleBuilderInitial<TObject> RuleFor(params Expression<Func<TObject, object>>[] properties)
        {
            var propertiesNames = properties.Select(GetPropertyNameForValidator).ToArray();

            return RuleFor(propertiesNames);
        }

        public IPropertiesRuleBuilderInitial<TObject> RuleFor(params string[] propertiesNames)
        {
            var ruleBuilder = new PropertiesRuleBuilder<TObject>();
            _adaptersBuilders.Add(new PropertiesAdaptersBuilder(ruleBuilder, propertiesNames));

            return ruleBuilder;
        }


        public ICollectionRuleBuilderInitial<TObject, IEnumerable<TProp>, TProp> RuleForCollection<TProp>(
            Expression<Func<TObject, IEnumerable<TProp>>> collection)
        {
            return RuleForStrongTypedCollection<IEnumerable<TProp>, TProp>(collection);
        }

        public ICollectionRuleBuilderInitial<TObject, TCollection, TProp> RuleForStrongTypedCollection<TCollection, TProp>(
            Expression<Func<TObject, TCollection>> collection)
            where TCollection : IEnumerable<TProp>
        {
            var propertyName = GetPropertyNameForValidator(collection);
            var ruleBuilder = new CollectionPropertyRuleBuilder<TObject, TCollection, TProp>();

            _adaptersBuilders.Add(new PropertiesAdaptersBuilder(ruleBuilder, propertyName));

            return ruleBuilder;
        }



        public IObjectValidator Build(TObject instance)
        {
            var validator = new ObjectValidator<TObject>(instance);

            foreach (var adaptersBuilder in _adaptersBuilders) {
                var adapter = adaptersBuilder.Builder.Build(validator, adaptersBuilder.TargetProperties);
                validator.RegisterAdapter(adapter, adaptersBuilder.TargetProperties);
            }

            validator.Revalidate();

            return validator;
        }


        /// <summary>
        /// Get property name by expression for validator. Throws ArgumentException, if couldn't validate this property
        /// </summary>
        /// <typeparam name="TProp">The type of element of collection</typeparam>
        /// <param name="property">Expression which contains property</param>
        /// <returns>Property name</returns>
        private static string GetPropertyNameForValidator<TProp>(Expression<Func<TObject, TProp>> property)
        {
            var propertyInfo = ReactiveValidationHelper.GetPropertyInfo(typeof(TObject), property);
            return propertyInfo.Name;
        }


        private class PropertiesAdaptersBuilder
        {
            public PropertiesAdaptersBuilder(IAdapterBuilder<TObject> builder, params string[] targetProperties)
            {
                Builder = builder;
                TargetProperties = targetProperties;
            }


            public IAdapterBuilder<TObject> Builder { get; }

            public string[] TargetProperties { get; }
        }
    }
}
