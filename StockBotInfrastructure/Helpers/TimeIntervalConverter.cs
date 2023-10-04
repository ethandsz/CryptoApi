namespace StockBotInfrastructure.Helpers;

public static class TimeIntervalConverter
{
    public static int TimeIntervalToMilliseconds(string timeInterval)
    {
        int milliseconds;

        if (string.IsNullOrWhiteSpace(timeInterval))
        {
            throw new ArgumentException("Invalid time interval format.");
        }

        // Extract the numeric portion of the time interval string
        var numericPart = new string(timeInterval.Where(char.IsDigit).ToArray());

        if (int.TryParse(numericPart, out var numericValue))
        {
            // Determine the unit (e.g., "h" for hours)
            if (timeInterval.EndsWith("m"))
            {
                milliseconds = numericValue * 60 * 1000;
            }
            else if (timeInterval.EndsWith("h"))
            {
                milliseconds = numericValue * 60 * 60 * 1000;
            }
            else if (timeInterval.EndsWith("d"))
            {
                milliseconds = numericValue * 24 * 60 * 60 * 1000;
            }
            else if (timeInterval.EndsWith("w"))
            {
                milliseconds = numericValue * 7 * 24 * 60 * 60 * 1000;
            }
            else
            {
                throw new ArgumentException("Invalid time interval unit.");
            }
        }
        else
        {
            throw new ArgumentException("Invalid numeric part of the time interval.");
        }

        return milliseconds;
    }
}