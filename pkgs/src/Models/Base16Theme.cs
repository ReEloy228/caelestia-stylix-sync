namespace CaelestiaStylixSync.Models;

public class Base16Theme
{
    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string system { get; set; } = "base16";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string name { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string author { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string variant { get; set; } = "";

    public Base16Palette palette { get; set; } = new();
}
