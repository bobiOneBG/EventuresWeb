namespace Eventures.Web.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class ExactLengthAttribute : ValidationAttribute
    {
        private const string RequiredStringLength = "10";
        public override bool IsValid(object value)
        {
            string stringValue = value as string;

            if (stringValue.Length != int.Parse(RequiredStringLength))
            {
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string displayName)
        {
            return $"{displayName} length should be exactly {RequiredStringLength} symbols";
        }
    }
}
