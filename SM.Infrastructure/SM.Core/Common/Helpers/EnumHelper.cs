using System.ComponentModel;

namespace SM.Core.Common.Helpers
{
    public static class EnumHelper
    {
        public static string DescriptionAttr<T>(this T source)
        {
            var fieldInfo = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes is not null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static List<string> GetEnumDescriptionList<T>(this T type) where T : System.Type
        {
            var list = new List<string>();
            foreach (var lEnum in Enum.GetValues(type))
            {
                list.Add(lEnum.DescriptionAttr());
            }
            return list;
        }
    }
}
