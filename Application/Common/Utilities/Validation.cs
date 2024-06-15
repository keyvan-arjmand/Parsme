namespace Application.Common.Utilities;

public class Validation
{
    public static bool ValidationDate(DateTime timeStart, DateTime timeEnd)
    {
        switch (DateTime.Compare(timeStart, timeEnd))
        {
            case < 0:
                return true;
            case 0:
                return false;
            case > 0:
                return false;
        }
    }
}