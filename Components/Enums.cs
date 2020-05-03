using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LoginApp.Components
{
    public class Enums
    {
        public class Api
        {
            public enum Error
            {
                // Membership
                [Description("PasswordsMustMatch")]
                PasswordsMustMatch = 100,
                [Description("PhoneIsNotValid")]
                PhoneIsNotValid = 101,
                [Description("PasswordIsNotValid")]
                PasswordIsNotValid = 102,
                [Description("PhoneIsAlreadyUsed")]
                PhoneIsAlreadyUsed = 103,
                [Description("CodeIsNotValid")]
                CodeIsNotValid = 104,
                [Description("EmailIsNotValid")]
                EmailIsNotValid = 105,
                [Description("EmailOrPhoneNotRegistered")]
                EmailOrPhoneNotRegistered = 106,
                [Description("PasswordIsWrong")]
                PasswordIsWrong = 107,
                [Description("CannotFindData")]
                CannotFindData = 108,

                // General
                [Description("GeneralException")]
                GeneralException = 500,
                [Description("MissingData")]
                MissingData = 501
            }
        }

        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static Dictionary<int, string> GetDescriptions(Type enumType)
        {
            Array enumTypeValues = Enum.GetValues(enumType);

            Dictionary<int, string> descriptions = new Dictionary<int, string>(enumTypeValues.Length);
            for (int i = 0; i <= enumTypeValues.Length - 1; i++)
            {
                descriptions.Add(Convert.ToInt32(enumTypeValues.GetValue(i)), GetDescription((Enum)enumTypeValues.GetValue(i)));
            }
            return descriptions;
        }
    }
}
