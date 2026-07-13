// 1 page application for UWP translating
using System;
using System.Text.RegularExpressions;

using System.Text;

Dictionary<char, string> Starport = new Dictionary<char, string>
{
    {'A', "Excellent quality with refined fuel, overhaul and maintenance"},
    {'B', "Good quality with refined fuel, overhaul and maintenance for non-starships"},
    {'C', "Routine quality with unrefined fuel, some repair facilities"},
    {'D', "Poor quality with unrefined fuel, no repair facilities"},
    {'E', "Frontier installation with no facilities"},
    {'X', "No starport"},
    {'F', "Good qualitity space port with unrefined fuel, some facilities"},
    {'G', "Poor quality with unrefined fuel, no repair facilities"},
    {'H', "Primitive installation with no facilities"},
    {'Y', "No space port"}
};

Dictionary<char, string> Size = new Dictionary<char, string>
{
    {'0', "Asteriod/planetoid belt"},
    {'1', "1600 km / 0.122G"},
    {'2', "3200 km / 0.240G"},
    {'3', "4800 km / 0.377G"},
    {'4', "6400 km / 0.500G"},
    {'5', "6400 km / 0.625G"},
    {'6', "8000 km / 0.840G"},
    {'7', "11200 km / 0.875G"},
    {'8', "12800 km / 1.00G"},
    {'9', "14400 km / 1.120G"},
    {'A', "16000 km / 1.125G"},
    {'R', "Ring around a world"},
    {'S', "Small world, 200km // 0.024G"}
};

Dictionary<char, string> Atmosphere = new Dictionary<char, string>
{
    {'0', "None"},
    {'1', "Trace"},
    {'2', "Very thin, tainted"},
    {'3', "Very thin"},
    {'4', "Thin, tainted"},
    {'5', "Thin"},
    {'6', "Standard"},
    {'7', "Standard, tainted"},
    {'8', "Dense"},
    {'9', "Dense, tainted"},
    {'A', "Exotic"},
    {'B', "Corrosive"},
    {'C', "Insidious"},
    {'D', "Dense, low"},
    {'E', "Ellipsoid"},
    {'F', "Thin, low"}
};

Dictionary<char, string> Hydrographics = new Dictionary<char, string>
{
    {'0', "No water"},
    {'1', "10% water"},
    {'2', "20% water"},
    {'3', "30% water"},
    {'4', "40% water"},
    {'5', "50% water"},
    {'6', "60% water"},
    {'7', "70% water"},
    {'8', "80% water"},
    {'9', "90% water"},
    {'A', "100% water"}
};  

Dictionary<char, string> Population = new Dictionary<char, string>
{
    {'0', "None"},
    {'1', "Tens"},
    {'2', "Hundreds"},
    {'3', "Thousands"},
    {'4', "Tens of thousands"},
    {'5', "Hundreds of thousands"},
    {'6', "Millions"},
    {'7', "Tens of millions"},
    {'8', "Hundreds of millions"},
    {'9', "Billions"},
    {'A', "Tens of billions"}
};

Dictionary<char, string> Government = new Dictionary<char, string>
{
    {'0', "No government"},
    {'1', "Company/Corporation"},
    {'2', "Participating Democracy"},
    {'3', "Self-Perpetuating Oligarchy"},
    {'4', "Representative Democracy"},
    {'5', "Feudal Technocracy"},
    {'6', "Captive Government"},
    {'7', "Balkanization"},
    {'8', "Civil Service Bureaucracy"},
    {'9', "Impersonal Bureaucracy"},
    {'A', "Charismatic Dictator"},
    {'B', "Non-Charismatic Leader"},
    {'C', "Charismatic Oligarchy"},
    {'D', "Religious Dictatorship"},
    {'E', "Religious Autocracy"},
    {'F', "Totalitarian Oligarchy"}
};

Dictionary<char, string> LawLevel = new Dictionary<char, string>
{
    {'0', "No prohibitions"},
    {'1', "Body pistols undetectable, explosives, poison gas, and poison weapons prohibited"},
    {'2', "Portable energy weapons, explosives, poison gas, and poison weapons prohibited"},
    {'3', "Weapons of a strict military nature prohibited"},
    {'4', "Light assault weapons prohibited"},
    {'5', "Personal concealable weapons prohibited"},
    {'6', "Most firearms (all except shotguns) prohibited, carrying of weapons in public discouraged"},
    {'7', "Shotguns prohibited"},
    {'8', "Long blade weapons are controlled nd open possession is prohibited"},
    {'9', "Any possession ofweapons outside of the home is prohibited"},
    {'A', "Weapon possession is prohibited, except for government agents and military"}
};

Dictionary<char, string> TechLevel = new Dictionary<char, string>
{
    {'0', "Stone age, primitive"},
    {'1', "Bronze Age to Middle Ages"},
    {'2', "Circa 1400-1700"},
    {'3', "Circa 1700-1860"},
    {'4', "Circa 1860-1900"},
    {'5', "Circa 1900-1939"},
    {'6', "Circa 1940-1969"},
    {'7', "Circa 1970-1979"},
    {'8', "Circa 1980-1989"},
    {'9', "Circa 1990-2000"},
    {'A', "Interstellar community"},
    {'B', "Average Imperial"},
    {'C', "Average Imperial"},
    {'D', "Above average Imperial"},
    {'E', "Above average Imperial"},
    {'F', "Technical maximum Imperial"},
    {'G', "Occassional non-Imperial"},
};

var pattern = @"^[ABCDEXFGHY][0-9RSA][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A]-[0-9A-G]$";

StringBuilder parseUWP(string uwp)
{
    var sb = new StringBuilder();
    sb.AppendLine($"Starport: {Starport[uwp[0]]}");
    sb.AppendLine($"Size: {Size[uwp[1]]}");
    sb.AppendLine($"Atmosphere: {Atmosphere[uwp[2]]}");
    sb.AppendLine($"Hydrographics: {Hydrographics[uwp[3]]}");
    sb.AppendLine($"Population: {Population[uwp[4]]}");
    sb.AppendLine($"Government: {Government[uwp[5]]}");
    sb.AppendLine($"Law Level: {LawLevel[uwp[6]]}");
    sb.AppendLine($"Tech Level: {TechLevel[uwp[8]]}");
    return sb;
}

// main loop to read input and output translation
Console.WriteLine("Please enter a UWP string (or press Enter to exit):");
while (Console.ReadLine() is string line && line.Length > 0)
{
    if (!Regex.IsMatch(line, pattern))
        Console.WriteLine("Invalid UWP string. Please enter a valid UWP string.");
    else
        Console.WriteLine(parseUWP(line).ToString());
}