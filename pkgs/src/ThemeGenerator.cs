namespace CaelestiaStylixSync;

public static class ThemeGenerator
{
    readonly static ISerializer serializer = new SerializerBuilder()
        .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
        .Build();

    readonly static string[] basePallete =
    [
        "base",
        "mantle",
        "overlay0",
        "overlay1",
        "subtext1",
        "text",
        "rosewater",
        "tertiary_paletteKeyColor",
        "red",
        "sapphire",
        "blue",
        "tertiary",
        "primaryContainer",
        "primary",
        "surfaceTint",
        "flamingo"
    ];

#pragma warning disable IDE0017
    public static string GenerateYaml(CaelestiaScheme scheme, string generateWith)
    {
        var theme = new Base16Theme
        {
            system = "base16",
            name = $"{scheme.Name} (Caelestia)",
            author = "Caelestia",
            variant = scheme.Mode == "dark" ? "dark" : "light",
        };
        theme.palette = generateWith.ToLowerInvariant() switch
        {
            "term" => GetTermScheme(scheme),
            _ => GetBaseScheme(scheme)
        };
        return serializer.Serialize(theme);
    }
#pragma warning restore IDE0017

    static Base16Palette GetBaseScheme(CaelestiaScheme scheme)
    {
        var palette = new Base16Palette();
        for (int i = 0; i < 16; i++)
            if (!scheme.Colours.TryGetValue(basePallete[i], out var hex))
                throw new KeyNotFoundException($"Missing colour key: {basePallete[i]}");
            else palette[i] = hex;
        return palette;
    }

    static Base16Palette GetTermScheme(CaelestiaScheme scheme)
    {
        var palette = new Base16Palette();
        for (int i = 0; i < 16; i++)
            if (!scheme.Colours.TryGetValue($"term{i}", out var hex))
                throw new KeyNotFoundException($"Missing colour key: term{i}");
            else palette[i] = hex;
        return palette;
    }
}
