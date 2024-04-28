using TZTDate_BlazorWebAssembly.Models;
using TZTDateBlazorWebAssembly.Enums;

public static class ZodiacSign
{
    public static ZodiacDateType? GetZodiacSign(this User user)
    {
        int day = user.BirthDateTime.Day;
        int month = user.BirthDateTime.Month;
        switch (month)
        {
            case 1:
                return day <= 19 ? ZodiacDateType.Capricorn : ZodiacDateType.Aquarius;
            case 2:
                return day <= 18 ? ZodiacDateType.Aquarius : ZodiacDateType.Pisces;
            case 3:
                return day <= 21 ? ZodiacDateType.Pisces : ZodiacDateType.Aries;
            case 4:
                return day <= 19 ? ZodiacDateType.Aries : ZodiacDateType.Taurus;
            case 5:
                return day <= 20 ? ZodiacDateType.Taurus : ZodiacDateType.Gemini;
            case 6:
                return day <= 20 ? ZodiacDateType.Gemini : ZodiacDateType.Cancer;
            case 7:
                return day <= 22 ? ZodiacDateType.Cancer : ZodiacDateType.Leo;
            case 8:
                return day <= 22 ? ZodiacDateType.Leo : ZodiacDateType.Virgo;
            case 9:
                return day <= 22 ? ZodiacDateType.Virgo : ZodiacDateType.Libra;
            case 10:
                return day <= 22 ? ZodiacDateType.Libra : ZodiacDateType.Scorpio;
            case 11:
                return day <= 21 ? ZodiacDateType.Scorpio : ZodiacDateType.Sagittarius;
            case 12:
                return day <= 21 ? ZodiacDateType.Sagittarius : ZodiacDateType.Capricorn;
            default:
                return null;
        }
    }

    // public static RelationshipData? CompatibilityByZodiacSign(this User currentUser, User userForCompare)
    // {
    //     string jsonFilePath = "../TZTDate.Core/Data/DateUser/Zodiac/Assets/CompabilityZodiac.json";
    //     string json = File.ReadAllText(jsonFilePath);
    //     var relationshipList = JsonConvert.DeserializeObject<List<RelationshipData>>(json);
    //     var zodiacSignDistance = currentUser.GetZodiacSign() - userForCompare.GetZodiacSign();
    //     switch (zodiacSignDistance)
    //     {
    //         case 0:
    //             return relationshipList?[0];
    //         case 1:
    //         case -1:
    //         case 11:
    //         case -11:
    //             return relationshipList?[1];
    //         case 2:
    //         case -2:
    //         case 10:
    //         case -10:
    //             return relationshipList?[2];
    //         case 3:
    //         case -3:
    //         case 9:
    //         case -9:
    //             return relationshipList?[3];
    //         case 4:
    //         case -4:
    //         case 8:
    //         case -8:
    //             return relationshipList?[4];
    //         case 5:
    //         case -5:
    //         case 7:
    //         case -7:
    //             return relationshipList?[5];
    //         case 6:
    //         case -6:
    //             return relationshipList?[6];
    //         default:
    //             return null;
    //     }
    // }
}