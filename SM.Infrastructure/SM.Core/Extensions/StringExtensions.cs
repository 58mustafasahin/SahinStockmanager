using Microsoft.EntityFrameworkCore;
using SM.Core.DataAccess;

namespace SM.Core.Extensions
{
    public static class StringExtensions
    {
        public static string AddStringWithDot(this string value, string addedString)
        {
            return value + "." + addedString;
        }

        public static string MaskPhoneNumber(this string value)
        {
            return "(***) *** - " + value.Substring(value.Length - 4);
        }

        public static string MaskEMail(this string value)
        {
            if (string.IsNullOrEmpty(value) || (value ?? "").IndexOf("@") == -1)
            {
                return value;
            }

            var mailSplit = value.Split("@");
            var mailMask = "";
            if (mailSplit[0].Length > 1)
            {
                mailMask = mailSplit[0].Substring(0, 2);
            }
            else if (mailSplit[0].Length > 0)
            {
                mailMask = mailSplit[0].Substring(0, 1);
            }
            mailMask = mailMask + "**" + "@" + mailSplit[(mailSplit.Length - 1)];

            return mailMask;
        }

        public static string GeneratePublishName(this IPublishEntity entity, EntityState entityState)
        {
            return entity.GetType().FullName + "_" + entityState;
        }
    }
}
