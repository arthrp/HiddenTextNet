using HiddenTextNet;

namespace HiddenTextNet.Tests;

public class WhitespaceTextTests
{
    private const char Zero = WhitespaceText.ZERO;
    private const char One = WhitespaceText.ONE;

    [Test]
    public void Encode_SingleByte_A_ProducesExpectedBits()
    {
        // 0x41 => 0100 0001 (MSB -> LSB)
        var expected = new string(new[] { Zero, One, Zero, Zero, Zero, Zero, Zero, One });
        var result = WhitespaceText.Encode("A");
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Decode_SingleBytePattern_ReturnsA()
    {
        var encoded = new string(new[] { Zero, One, Zero, Zero, Zero, Zero, Zero, One });
        var decoded = WhitespaceText.Decode(encoded);
        Assert.That(decoded, Is.EqualTo("A"));
    }

    [Test]
    public void EncodeDecode_RoundTrip_Ascii()
    {
        const string input = "Hello, World!";
        var encoded = WhitespaceText.Encode(input);
        var decoded = WhitespaceText.Decode(encoded);
        Assert.That(decoded, Is.EqualTo(input));
    }

    [Test]
    public void EncodeDecode_RoundTrip_Unicode()
    {
        const string input = "😀 café ₿";
        var encoded = WhitespaceText.Encode(input);
        var decoded = WhitespaceText.Decode(encoded);
        Assert.That(decoded, Is.EqualTo(input));
    }

    [Test]
    public void EncodeDecode_EmptyString()
    {
        var encoded = WhitespaceText.Encode(string.Empty);
        Assert.That(encoded, Is.EqualTo(string.Empty));

        var decoded = WhitespaceText.Decode(encoded);
        Assert.That(decoded, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Decode_InvalidLength_Throws()
    {
        // 7 bits only
        var invalid = new string(Zero, 7);
        Assert.Throws<ArgumentException>(() => WhitespaceText.Decode(invalid));
    }

    [Test]
    public void Decode_InvalidCharacter_Throws()
    {
        // 8-char chunk with an invalid non-zero-width character
        var invalid = new string(new[] { Zero, Zero, Zero, 'x', Zero, Zero, Zero, Zero });
        Assert.Throws<ArgumentException>(() => WhitespaceText.Decode(invalid));
    }
}