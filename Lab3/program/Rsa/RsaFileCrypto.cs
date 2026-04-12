using System.Buffers.Binary;
using System.Text;

namespace RsaLab.Rsa;

public sealed class RsaFileCrypto(int modulus, int publicExponent, int privateExponent)
{
    private readonly int _r = modulus;
    private readonly int _e = publicExponent;
    private readonly int _d = privateExponent;

    /// <summary>Шифрование: байт → два байта (BE), возвращает десятичные значения C для вывода.</summary>
    public (byte[] CipherBytes, string DecimalPreview) EncryptFile(string inputPath, string outputPath)
    {
        var plain = File.ReadAllBytes(inputPath);
        var cipher = new byte[plain.Length * 2];
        var decimals = new StringBuilder(plain.Length * 6);

        for (var i = 0; i < plain.Length; i++)
        {
            var m = plain[i];
            var c = RsaMath.ModPow(m, _e, _r);
            BinaryPrimitives.WriteUInt16BigEndian(cipher.AsSpan(i * 2, 2), (ushort)c);
            if (i > 0) decimals.Append(' ');
            decimals.Append(c);
        }

        File.WriteAllBytes(outputPath, cipher);
        return (cipher, decimals.ToString());
    }

    /// <summary>Расшифрование: блоки по 16 бит (BE) → исходные байты.</summary>
    public byte[] DecryptFile(string inputPath, string outputPath)
    {
        var cipher = File.ReadAllBytes(inputPath);
        if (cipher.Length % 2 != 0)
            throw new InvalidDataException("Размер зашифрованного файла должен быть чётным (блоки по 2 байта).");

        var plain = new byte[cipher.Length / 2];
        for (var i = 0; i < plain.Length; i++)
        {
            var c = BinaryPrimitives.ReadUInt16BigEndian(cipher.AsSpan(i * 2, 2));
            if (c >= _r)
                throw new InvalidDataException($"Блок шифротекста {c} недопустим для модуля r = {_r}.");
            plain[i] = (byte)RsaMath.ModPow(c, _d, _r);
        }

        File.WriteAllBytes(outputPath, plain);
        return plain;
    }
}
