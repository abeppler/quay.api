using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Wms.ProductionLine.CrossCutting
{
    public static class EnumExtensions
    {
        public static EnumModel<T> ObterModelo<T>(this Enum valor)
        {
            return new EnumModel<T>((T)(object)valor, valor.RecuperarDescricaoEnum());
        }

        public static string RecuperarDescricaoEnum(this Enum valor)
        {
            var displayName = valor.GetDisplayName();
            return displayName == valor.ToString() ? valor.GetDescription() : displayName;
        }

        public static string GetDisplayName(this Enum value)
        {
            var attribute = GetAttribute<DisplayAttribute>(value);
            return attribute == null ? value.ToString() : attribute.GetName();
        }

        public static string GetDisplayDescription(this Enum value)
        {
            var attribute = GetAttribute<DisplayAttribute>(value);
            return attribute == null ? value.ToString() : attribute.GetDescription();
        }

        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(T)) as T;
            return attribute;
        }

        public static List<KeyValuePair<TEnum, string>> GetList<TEnum>() where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();
            return ((TEnum[])Enum.GetValues(typeof(TEnum)))
               .ToDictionary(k => k, v => ((Enum)(object)v).GetDisplayName())
               .ToList();
        }
    }
}
