namespace Application.Birthdays;

public static class BirthdayUtil
{
    public static int GetAgeFromBirthday(DateTime birthday)
    {
        DateTime today = DateTime.Today;

        int age = today.Year - birthday.Year;

        // if (birthday.Date > today.AddYears(-age))
        if (today.Month < birthday.Month || (today.Month == birthday.Month && today.Day < birthday.Day))
        {
            age--;
        }

        return age;
    }

    public static int GetDaysUntilBirthday(DateTime birthday)
    {
        DateTime today = DateTime.Today;
        DateTime nextBirthday = new DateTime(today.Year, birthday.Month, birthday.Day);

        if (nextBirthday < today)
        {
            nextBirthday = nextBirthday.AddYears(1);
        }

        return (nextBirthday - today).Days;
    }
}
