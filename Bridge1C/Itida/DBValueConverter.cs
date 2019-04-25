namespace DAL.Itida
{
    using System;

    public static class DBValueConverter<T>
    {
        public static T GetValueOrNull(object value)
        {
            if (value is string strVal)
                value = strVal.Trim();

            T result = value == DBNull.Value ? default : (T)value;
            return result;
        }
    }
}
