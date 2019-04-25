namespace DAL.Itida
{
    using System;

    public static class DBValueConverter<T>
    {
        private static T GetOrNullValue(object value)
        {
            T result = value == DBNull.Value ? default(T) : (T)value;
            return result;
        }
    }
}
