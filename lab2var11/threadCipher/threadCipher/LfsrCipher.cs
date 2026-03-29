using System;
using System.Linq;
using System.Text;

namespace threadCipher;

/// <summary>
/// Поточное шифрование XOR с гаммой от LFSR (полином x^33 + x^13 + 1).
/// </summary>
internal sealed class LfsrCipher
{
    private const int SeedBitCount = 33;
    private const UInt64 RegisterMask = 0x1FFFFFFFF;

    private UInt64 _register;

    private UInt64 GetNextBit()
    {
        
        UInt64 outputBit = (_register >> 32) & 1;

        
        UInt64 b33 = (_register >> 32) & 1;
        UInt64 b13 = (_register >> 12) & 1;
        UInt64 feedback = b33 ^ b13;

        _register = ((_register << 1) | feedback) & RegisterMask;

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
            {
                _register |= (1ul << (SeedBitCount - 1 - i));
            }
        }

        var result = new byte[data.Length];
        var keyStreamBits = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            byte keyByte = 0;
            for (int bit = 0; bit < 8; bit++)
            {
                UInt64 b = GetNextBit();
                keyByte |= (byte)(b << (7 - bit));

                if (keyStreamBits.Length < 64)
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