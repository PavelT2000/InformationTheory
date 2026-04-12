using System.Collections.Generic;

namespace RsaLab.Rsa;

/// <summary>Поиск первообразных корней по модулю простого p (для отчёта: вариант p = 31).</summary>
public static class PrimitiveRootFinder
{
    public static IReadOnlyList<int> DistinctPrimeFactors(int n)
    {
        var list = new List<int>();
        var x = n;
        for (var d = 2; d * (long)d <= x; d++)
        {
            if (x % d == 0)
            {
                list.Add(d);
                while (x % d == 0) x /= d;
            }
        }
        if (x > 1) list.Add((int)x);
        return list;
    }

    /// <summary>Все первообразные корни по модулю простого p.</summary>
    public static IReadOnlyList<int> FindPrimitiveRoots(int p)
    {
        if (!RsaMath.IsPrime(p))
            throw new ArgumentException("Модуль p должен быть простым.", nameof(p));

        var phi = p - 1;
        var factors = DistinctPrimeFactors(phi);
        var roots = new List<int>();

        for (var g = 2; g < p; g++)
        {
            var isRoot = true;
            foreach (var q in factors)
            {
                if (RsaMath.ModPow(g, phi / q, p) == 1)
                {
                    isRoot = false;
                    break;
                }
            }
            if (isRoot) roots.Add(g);
        }

        return roots;
    }
}
