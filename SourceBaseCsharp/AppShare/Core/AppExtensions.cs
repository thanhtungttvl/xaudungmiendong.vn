using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AppShare.Core
{
    public static class AppExtensions
    {
        public static string GetDescription<T>(this T source)
        {
            if (source is null)
            {
                return "NULL";
            }

            var fi = source.GetType().GetField(source.ToString()!);

            if (fi is null)
            {
                return "NULL";
            }

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes is null)
            {
                return "NULL";
            }

            string? data = attributes.Length > 0 ? attributes[0].Description : source.ToString();

            return data is null ? "NULL" : data;
        }

        public static List<T> RandomizeList<T>(this List<T> inputList)
        {
            Random rnd = new Random();
            List<T> randomizedList = [.. inputList];

            for (int i = randomizedList.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(0, i + 1);
                T temp = randomizedList[i];
                randomizedList[i] = randomizedList[j];
                randomizedList[j] = temp;
            }

            return randomizedList;
        }

        public static string ToSHA512Hash(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            using (var sha512 = SHA512.Create())
            {
                var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(value));

                var stringBuilder = new StringBuilder();

                foreach (var b in hash)
                    stringBuilder.AppendFormat(b.ToString("x2"));
                return stringBuilder.ToString().ToUpper();
            }
        }

        public static T Randomize<T>(this List<T> inputList)
        {
            Random rnd = new Random();
            List<T> randomizedList = [.. inputList];

            int index = rnd.Next(randomizedList.Count);

            return randomizedList[index];
        }

        // Hàm mở rộng để kết hợp các biểu thức "AndAlso"
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T));

            var combinedBody = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }

        #region Slug
        public static string GenerateSlug(this string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = GetStringSlug(text);
            text = RemoveAccentSlug(text).ToLower();
            text = Regex.Replace(text, @"[^a-z0-9\s-]", " ");
            text = Regex.Replace(text, @"\s+", " ").Trim();
            var list = new[] { " ", "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "+", "=", "/", "\\", "+", "?", "<", ">", ".", ",", "{", "}", "[", "]", "|" };

            var stringAsArr = text.Split(list, StringSplitOptions.RemoveEmptyEntries).ToList();

            var result = string.Join("-", stringAsArr);

            return result.ToLower();
        }

        private static string GetStringSlug(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).Replace("đ", "d").Replace("Đ", "D");
        }

        private static string RemoveAccentSlug(string txt)
        {
            string[] vietnameseChar =
            [
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            ];

            for (int i = 1; i < vietnameseChar.Length; i++)
            {
                for (int j = 0; j < vietnameseChar[i].Length; j++)
                    txt = txt.Replace(vietnameseChar[i][j], vietnameseChar[0][i - 1]);
            }

            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
        #endregion
    }
}
