using System;
using System.Linq;
using System.Text;

namespace threadCipher;

/// <summary>
/// Поточное шифрование XOR с гаммой от LFSR (полином x^30 + x^16 + x^15 + x + 1).
/// </summary>
internal sealed class LfsrCipher
{
    private const int SeedBitCount = 30;
    private const uint RegisterMask = 0x3FFFFFFF;

    private uint _register;

    private uint GetNextBit()
    {
        uint b30 = (_register >> 29) & 1;
        uint b16 = (_register >> 15) & 1;
        uint b15 = (_register >> 14) & 1;
        uint b1 = _register & 1;

        uint feedback = b30 ^ b16 ^ b15 ^ b1;
        uint outputBit = _register & 1;
        _register = ((_register >> 1) | (feedback << 29)) & RegisterMask;

        return outputBit;
    }

    public (byte[] Result, string KeyStreamPreview) Execute(byte[] data, string seedString)
    {
        ArgumentNullException.ThrowIfNull(data);
        ValidateSeed(seedString);

        _register = 0;
        for (int i = 0; i < SeedBitCount; i++)
        {
            if (seedString[i] == '1')
                _register |= 1u << i;
        }

        var result = new byte[data.Length];
        var keyStreamBits = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            byte keyByte = 0;
            for (int bit = 0; bit < 8; bit++)
            {
                uint b = GetNextBit();
                keyByte |= (byte)(b << bit);

                if (i == 0 || i * 8 + bit < 64)
                    keyStreamBits.Append(b);
            }

            result[i] = (byte)(data[i] ^ keyByte);
        }

        return (result, keyStreamBits.ToString());
    }

    private static void ValidateSeed(string seed)
    {
        if (seed.Length != SeedBitCount)
            throw new ArgumentException($"Ключ должен быть ровно {SeedBitCount} бит.", nameof(seed));

        if (seed.Any(c => c is not ('0' or '1')))
            throw new ArgumentException("Ключ должен содержать только символы 0 и 1.", nameof(seed));
    }
}
