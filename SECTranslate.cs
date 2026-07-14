// Parses Traveller sector (`SEC`) lines and outputs the full system description,
// including UWP translation from the existing UWPTranslate logic.
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
    {'G', "Occassional non-Imperial"}
};

const string validStarportCodes = "ABCDEXFGHY";
const string validSizeCodes = "0123456789RSA";
const string validAtmosphereCodes = "0123456789ABCDEF";
const string validHydrographicsCodes = "0123456789A";
const string validPopulationCodes = "0123456789A";
const string validGovernmentCodes = "0123456789ABCDEF";
const string validLawLevelCodes = "0123456789A";
const string validTechLevelCodes = "0123456789ABCDEFG";

string? GetUwpValidationError(string uwp)
{
    if (string.IsNullOrWhiteSpace(uwp))
        return "Missing UWP field.";

    if (uwp.Length != 9)
        return $"Invalid length: expected 9 characters, got {uwp.Length}.";

    if (uwp[7] != '-')
        return $"Invalid format: expected '-' at position 8, found '{uwp[7]}' in position 8.";

    if (!Starport.ContainsKey(uwp[0]))
        return $"Invalid starport code '{uwp[0]}' at position 1; expected one of {validStarportCodes}.";

    if (!Size.ContainsKey(uwp[1]))
        return $"Invalid size code '{uwp[1]}' at position 2; expected one of {validSizeCodes}.";

    if (!Atmosphere.ContainsKey(uwp[2]))
        return $"Invalid atmosphere code '{uwp[2]}' at position 3; expected one of {validAtmosphereCodes}.";

    if (!Hydrographics.ContainsKey(uwp[3]))
        return $"Invalid hydrographics code '{uwp[3]}' at position 4; expected one of {validHydrographicsCodes}.";

    if (!Population.ContainsKey(uwp[4]))
        return $"Invalid population code '{uwp[4]}' at position 5; expected one of {validPopulationCodes}.";

    if (!Government.ContainsKey(uwp[5]))
        return $"Invalid government code '{uwp[5]}' at position 6; expected one of {validGovernmentCodes}.";

    if (!LawLevel.ContainsKey(uwp[6]))
        return $"Invalid law level code '{uwp[6]}' at position 7; expected one of {validLawLevelCodes}.";

    if (!Regex.IsMatch(uwp.Substring(8, 1), "^[0-9A-G]$"))
        return $"Invalid tech level code '{uwp[8]}' at position 9; expected one of {validTechLevelCodes}.";

    return null;
}

StringBuilder ParseUWP(string uwp, string pbg)
{
    var sb = new StringBuilder();
    sb.AppendLine($"Starport: {Starport[uwp[0]]}");
    sb.AppendLine($"Size: {Size[uwp[1]]}");
    sb.AppendLine($"Atmosphere: {Atmosphere[uwp[2]]}");
    sb.AppendLine($"Hydrographics: {Hydrographics[uwp[3]]}");
    int.TryParse(uwp[4].ToString(), out int populationValue);
    int.TryParse(pbg.Length > 0 ? pbg[0].ToString() : "0", out int pbgValue);
    var population = Math.Pow(10, populationValue) * pbgValue;
    sb.AppendLine($"Population: {population}");
    //sb.AppendLine($"Population: {Population[uwp[4]]}");
    sb.AppendLine($"Government: {Government[uwp[5]]}");
    sb.AppendLine($"Law Level: {LawLevel[uwp[6]]}");
    sb.AppendLine($"Tech Level: {TechLevel[uwp[8]]}");
    return sb;
}

string ParseSecLine(string line)
{
    if (string.IsNullOrWhiteSpace(line))
        return "Empty SEC line.";

    line = line.PadRight(74);

    var name = line.Substring(0, 14).Trim();
    var hexNbr = line.Substring(14, 4).Trim();
    var uwp = line.Substring(19, 9).Trim();
    var bases = line.Substring(29, 2).Trim();
    var tradeCodes = line.Substring(32, 16).Trim();
    var zone = line.Substring(48, 1).Trim();
    var pbg = line.Substring(51, 3).Trim();
    string pbgExt = extendPBG(pbg);
    var allegiance = line.Substring(55, 2).Trim();
    var stellarData = line.Substring(58, 16).Trim();

    var sb = new StringBuilder();
    sb.AppendLine($"System: {name}");
    sb.AppendLine($"Hex Number: {hexNbr}");
    sb.AppendLine($"UWP: {uwp}");
    sb.AppendLine($"Bases: {bases}");
    sb.AppendLine($"Trade/Comments: {tradeCodes}");
    sb.AppendLine($"Zone: {zone}");
    sb.AppendLine($"PBG: {pbgExt}");
    sb.AppendLine($"Allegiance: {allegiance}");
    sb.AppendLine($"Stellar Data: {stellarData}");

    var validationError = GetUwpValidationError(uwp);
    if (validationError is not null)
    {
        sb.AppendLine($"UWP validation error: {validationError}");
    }
    else
    {
        sb.AppendLine("UWP translation:");
        sb.Append(ParseUWP(uwp, pbg));
    }

    return sb.ToString();
}

string extendPBG(string pbg)
{
    if (string.IsNullOrWhiteSpace(pbg))
        return "000";
    var belts = pbg.Length > 0 ? pbg[1] : '0';
    var GG = pbg.Length > 0 ? pbg[2] : '0';
    return $"Belts: {belts} Gas Giants: {GG}";
}

Console.WriteLine("Enter Traveller SEC lines (blank line to exit):");
while (Console.ReadLine() is string line && line.Length > 0)
{
    Console.WriteLine(ParseSecLine(line));
}
