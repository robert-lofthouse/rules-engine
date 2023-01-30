using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Domain.RulesEngine.Business
{
	public static class EnumExtensions
	{
		public static string ToDescription(this Enum input)
		{
			FieldInfo fi = input.GetType().GetField(input.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0)
			{
				return attributes[0].Description;
			}
			else
			{
				return input.ToString();
			}
		}

		public static string ToName(this Enum input)
		{
			return Enum.GetName(input.GetType(), input);
		}

		public static List<string> GetListOfDescription<T>() where T : struct
		{
			Type t = typeof(T);
			return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.ToDescription()).ToList();
		}

		public static T ParseEnum<T>(this string input)
		{
			return (T)Enum.Parse(typeof(T), input, ignoreCase: true);
		}
	}
}
