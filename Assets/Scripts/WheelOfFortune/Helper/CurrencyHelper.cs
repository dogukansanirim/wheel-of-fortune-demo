
namespace WheelOfFortune.Helper
{
    public static class CurrencyHelper
    {
        // Returns currency's int value to string value if it's in the billion("B"), million("M") or thousand("K) range.
        // Exp: 2500000 -> x2.5M
        // Exp: 9000 -> x9K
        public static string DigitStringFormatWithLetter(int value)
        {
            switch (value)
            {
                case >= 1_000_000_000:
                    return "x" + value/1_000_000 + "B";
                case >= 1_000_000:
                    return "x" + value/1_000_000 + "M";
                case >= 1_000:
                    return "x" + value/1_000 + "K";
                case > 0:
                    return "x" + value;
            }
            
            return "";
        }
        
        // Returns int value to string with commas in between.
        // Exp: 2000000 -> 2,000,000
        public static string DigitStringFormatWithComma(int value)
        {
            return $"{value:n0}";
        }
    }
}