using System;
using AutoMapper;

namespace AD.ObjectMapping.AutoMapper
{
    /// <summary>
    /// An improvement on AutoMapper's TypeConverter type
    /// to include access to an existing Destination type
    /// </summary>
    /// <typeparam name="TSource">The Source type being converted from</typeparam>
    /// <typeparam name="TDestination">The Destination type being converted to</typeparam>
    public abstract class DestinationAwareTypeConverter<TSource, TDestination> : ITypeConverter<TSource, TDestination>
    {
        private ResolutionContext _context;

        public TDestination Convert(ResolutionContext context)
        {
            _context = context;

            if (_context.SourceValue != null && !(_context.SourceValue is TSource))
            {
                const string MESSAGE = "Value supplied is of type {0} but expected {1}.\n" +
                                       "Change the type converter source type, or redirect " +
                                       "the source value supplied to the value resolver using FromMember.";

                throw new AutoMapperMappingException(_context, string.Format(MESSAGE, typeof(TSource), _context.SourceValue.GetType()));
            }

            return ConvertCore((TSource)_context.SourceValue);
        }

        protected TDestination DestinationObject
        {
            get
            {
                if (_context == null)
                {
                    const string MESSAGE = "ResolutionContext is not yet set. Only call this property inside the 'ConvertCore' method.";
                    throw new InvalidOperationException(MESSAGE);
                }

                if (_context.DestinationValue != null && !(_context.DestinationValue is TDestination))
                {
                    const string MESSAGE = "Destination Value is of type {0} but expected {1}.";

                    throw new AutoMapperMappingException(_context, string.Format(MESSAGE, typeof(TDestination), _context.DestinationValue.GetType()));
                }

                return (TDestination)_context.DestinationValue;
            }
        }

        protected abstract TDestination ConvertCore(TSource source);
    }
}
