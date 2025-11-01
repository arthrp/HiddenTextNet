using System.Text;

namespace HiddenTextNet;

public static class WhitespaceText
{
    public const char ZERO = '\u200C';
    public const char ONE = '\u200D';
    
    public static string Encode(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        var s = new StringBuilder();

        foreach (var b in bytes)
        {
            EncodeByte(b, s);
        }

        return s.ToString();
    }

    public static string Decode(string encodedText)
    {
        ReadOnlySpan<char> span = encodedText.AsSpan();
        List<byte> arr = new();
        
        for (int i = 0; i < span.Length; i += 8)
        {
            int len = Math.Min(8, span.Length - i);
            ReadOnlySpan<char> chunk = span.Slice(i, len);
            var b = DecodeByte(chunk);
            arr.Add(b);
        }
        
        var str = Encoding.UTF8.GetString(arr.ToArray());
        return str;
    }

    private static void EncodeByte(byte b, StringBuilder s)
    {
        s.Append((b & 0b10000000) == 0b10000000 ? ONE : ZERO);
        s.Append((b & 0b01000000) == 0b01000000 ? ONE : ZERO);
        s.Append((b & 0b00100000) == 0b00100000 ? ONE : ZERO);
        s.Append((b & 0b00010000) == 0b00010000 ? ONE : ZERO);
        s.Append((b & 0b00001000) == 0b00001000 ? ONE : ZERO);
        s.Append((b & 0b00000100) == 0b00000100 ? ONE : ZERO);
        s.Append((b & 0b00000010) == 0b00000010 ? ONE : ZERO);
        s.Append((b & 0b00000001) == 0b00000001 ? ONE : ZERO);
    }

    private static byte DecodeByte(ReadOnlySpan<char> chunk)
    {
        var s = new StringBuilder();
        if (chunk.Length != 8) throw new ArgumentException("Must have 8 bits");

        foreach (var el in chunk)
        {
            if (el == ONE) s.Append('1');
            else if (el == ZERO) s.Append('0');
            else throw new ArgumentException("Invalid character encountered");
        }
        var bs = s.ToString();
        return Convert.ToByte(bs, 2);
    }
}