namespace RsaLab.Rsa;

/// <summary>Модульная арифметика для RSA: расширенный алгоритм Евклида и быстрое возведение в степень по модулю.</summary>
public static class RsaMath
{
    public const int MinModule = 257;
    public const int MaxModule = 65535;

    public static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;
        for (var i = 3; i * (long)i <= n; i += 2)
            if (n % i == 0)
                return false;
        return true;
    }

    public static int Gcd(int a, int b)
    {
        while (b != 0)
            (a, b) = (b, a % b);
        return Math.Abs(a);
    }

    /// <summary>Расширенный алгоритм Евклида: находит gcd и коэффициенты x, y такие, что a·x + b·y = gcd.</summary>
    public static (int gcd, int x, int y) ExtendedGcd(int a, int b)
    {
        long oldR = a, r = b;
        long oldS = 1, s = 0;
        long oldT = 0, t = 1;

        while (r != 0)
        {
            var q = oldR / r;
            (oldR, r) = (r, oldR - q * r);
            (oldS, s) = (s, oldS - q * s);
            (oldT, t) = (t, oldT - q * t);
        }

        return ((int)oldR, (int)oldS, (int)oldT);
    }

    /// <summary>Мультипликативная инверсия d по модулю m: e·d ≡ 1 (mod m).</summary>
    public static int ModInverse(int d, int m)
    {
        var (g, x, _) = ExtendedGcd(d, m);
        if (g != 1 && g != -1)
            throw new InvalidOperationException($"Числа {d} и {m} не взаимно просты (НОД = {g}).");

        var inv = x % m;
        if (inv < 0) inv += m;
        return inv;
    }

    /// <summary>Быстрое возведение в степень по модулю: base^exp mod mod.</summary>
    public static int ModPow(int baseValue, int exp, int mod)
    {
        if (mod <= 0) throw new ArgumentOutOfRangeException(nameof(mod));
        if (mod == 1) return 0;

        long result = 1;
        var b = baseValue % mod;
        if (b < 0) b += mod;
        var e = (long)exp;

        while (e > 0)
        {
            if ((e & 1) == 1)
                result = result * b % mod;
            b = b * b % mod;
            e >>= 1;
        }

        return (int)result;
    }

    public static bool IsValidModule(int r) => r > 256 && r <= MaxModule;

    public static string FormatModuleConstraint() =>
        $"Модуль r = p·q должен удовлетворять: {MinModule - 1} < r ≤ {MaxModule}.";
}
