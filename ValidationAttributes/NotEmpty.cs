namespace Eventures.Web.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class NotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string stringValue = value as string;

            if (string.IsNullOrEmpty(stringValue))
            {
                return false;
            }

            return true; ;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} shold be non empty";
        }
    }
}
