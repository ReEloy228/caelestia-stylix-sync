namespace CaelestiaStylixSync.Models;

public class Base16Palette
{
    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base00 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base01 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base02 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base03 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base04 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base05 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base06 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base07 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base08 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base09 { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base0A { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base0B { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base0C { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base0D { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base0E { get; set; } = "";

    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string base0F { get; set; } = "";

    public string this[int index]
    {
        get => index switch
        {
            0 => base00,
            1 => base01,
            2 => base02,
            3 => base03,
            4 => base04,
            5 => base05,
            6 => base06,
            7 => base07,
            8 => base08,
            9 => base09,
            10 => base0A,
            11 => base0B,
            12 => base0C,
            13 => base0D,
            14 => base0E,
            15 => base0F,
            _ => throw new IndexOutOfRangeException($"Index {index} is out of range (0..15).")
        };
        set
        {
            switch (index)
            {
                case 0: base00 = value; break;
                case 1: base01 = value; break;
                case 2: base02 = value; break;
                case 3: base03 = value; break;
                case 4: base04 = value; break;
                case 5: base05 = value; break;
                case 6: base06 = value; break;
                case 7: base07 = value; break;
                case 8: base08 = value; break;
                case 9: base09 = value; break;
                case 10: base0A = value; break;
                case 11: base0B = value; break;
                case 12: base0C = value; break;
                case 13: base0D = value; break;
                case 14: base0E = value; break;
                case 15: base0F = value; break;
                default: throw new IndexOutOfRangeException($"Index {index} is out of range (0..15).");
            }
        }
    }
}
