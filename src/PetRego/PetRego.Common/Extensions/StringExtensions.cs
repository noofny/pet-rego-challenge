using System;

namespace PetRego.Common.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string enumValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Type T expected type of enum but received type of " + typeof(T).FullName);
            }
            if (string.IsNullOrEmpty(enumValue))
            {
                return default(T);
            }
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                if (value.ToString().ToLower().Equals(enumValue.ToLower()))
                {
                    return (T)value;
                }
            }
            return default(T);
        }


    }
}
