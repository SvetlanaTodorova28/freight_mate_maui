using Mde.Project.Mobile.Domain.Services.Interfaces;

public static class GeocodingHelper
{
    public static async Task<bool> ValidateDestination(Location location, string destination, IGeocodingService geocodingService)
    {
        var placemarks = await geocodingService.GetPlacemarksAsync(location);
        var placemark = placemarks.FirstOrDefault();
        if (placemark == null)
        {
            return false;
        }

        var inputWords = CleanString(destination).Split(new char[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
        var thoroughfareWords = new List<string>();

        if (!string.IsNullOrWhiteSpace(placemark.CountryName))
            thoroughfareWords.AddRange(CleanString(placemark.CountryName).Split(' ', StringSplitOptions.RemoveEmptyEntries));;
        if (!string.IsNullOrWhiteSpace(placemark.Locality))
            thoroughfareWords.AddRange(CleanString(placemark.Locality).Split(' ', StringSplitOptions.RemoveEmptyEntries));
        if (!string.IsNullOrWhiteSpace(placemark.PostalCode))
            thoroughfareWords.Add(CleanString(placemark.PostalCode).Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
        if (!string.IsNullOrWhiteSpace(placemark.FeatureName))
            thoroughfareWords.AddRange(CleanString(placemark.FeatureName).Split(' ', StringSplitOptions.RemoveEmptyEntries));
        if (!string.IsNullOrWhiteSpace(placemark.Thoroughfare))
            thoroughfareWords.AddRange(CleanString(placemark.Thoroughfare).Split(' ', StringSplitOptions.RemoveEmptyEntries));

        int minimumMatchesRequired = 5;
        int matchCount = inputWords
            .Count(inputWord => thoroughfareWords.Any(thoroughfareWord => thoroughfareWord.Contains(inputWord)));

        return matchCount >= minimumMatchesRequired;
    }

    private static string CleanString(string input)
    {
        return input.Replace("\"", "").Trim().ToLowerInvariant();
    }
}