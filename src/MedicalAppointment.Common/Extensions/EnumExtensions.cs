using System;
using System.ComponentModel;
using System.Linq;

namespace MedicalAppointment.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            
            return type.GetField(name)
                        .GetCustomAttributes(false)
                        .OfType<DescriptionAttribute>()
                        .Select(d => d.Description)
                        .SingleOrDefault();
        }

        public static TEnum GetValueFromDescription<TEnum>(this string description)
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (TEnum) field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }
    }

    public class T
    {
    }
}
