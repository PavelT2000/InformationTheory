using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CryptoSystems
{
    public class DecimationCryptoSystem : ICryptoSystem
    {
        //private static Dictionary<char, int> alphToNum = new Dictionary<char, int>()
        //{
        //    {'а',1 },{'б',2 },{'в',3 },{'г',4 },{'д',5 },
        //    {'е',6 },{'ё',7 },{'ж',8 },{'з',9 },{'и',10 },
        //    {'й',11 },{'к',12 },{'л',13},{'м',14 },{'н',15 },
        //    {'о',16 },{'п',17 },{'р',18},{'с',19 },{'т',20 },
        //    {'у',21 },{'ф',22 },{'х',23},{'ц',24 },{'ч',25 },
        //    {'ш',26 },{'щ',27 },{'ъ',28},{'ы',29 },{'ь',30 },
        //    {'э',31 },{'ю',32 },{'я',33}
        //};
        private static Dictionary<char, int> alphToNum = new Dictionary<char, int>()
        {
            {'а',0 },{'б',1 },{'в',2 },{'г',3 },{'д',4 },
            {'е',5 },{'ё',6 },{'ж',7 },{'з',8 },{'и',9 },
            {'й',10 },{'к',11 },{'л',12},{'м',13 },{'н',14 },
            {'о',15 },{'п',16 },{'р',17},{'с',18 },{'т',19 },
            {'у',20 },{'ф',21 },{'х',22},{'ц',23 },{'ч',24 },
            {'ш',25 },{'щ',26 },{'ъ',27},{'ы',28 },{'ь',29 },
            {'э',30 },{'ю',31 },{'я',32}
        };
        private static char[] numToAlph = new char[]
        {
            //' ', // Индекс 0 не используется (заглушка)
            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
            'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т',
            'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь',
            'э', 'ю', 'я'
        };

        private static BigInteger key { get; set; }

        private const int dgvMax = 15;
        private const int n = 33;



        public bool ValidateSymbol(char symbol)
        {
            if (alphToNum.ContainsKey(symbol))
                return true;
            else
                return false;
        }

        public string ValidateKey(string key, out string output)
        {
            output = string.Empty;
            string validatedKey = "";
            int skippedCount = 0;
            for (int i = 0; i < key.Length; i++)
            {
                char c = key[i];
                if (char.IsDigit(c))
                    validatedKey += c;
                else
                    skippedCount++;
            }
            if (validatedKey == "" && skippedCount != 0)
            {
                output = $"В ключе отсутствуют цифры, пропущено {skippedCount} лишних символов, берётся ключ по умолчанию (1)\n";
                validatedKey = "1";
            }
            else if (validatedKey == "")
            {
                output = $"Ключ пустой, берётся ключ по умолчанию (1)\n";
                validatedKey = "1";
            }
            else if (skippedCount != 0)
            {
                output = $"Ключ содержит что-то кроме цифр, пропущено {skippedCount} лишних символов\n";
            }
            BigInteger preKey = BigInteger.Parse(validatedKey);
            if (BigInteger.GreatestCommonDivisor(preKey, n) != 1)
            {
                BigInteger nearest;
                long offset = 1;
                while (true)
                {
                    if (BigInteger.GreatestCommonDivisor(preKey + offset, n) == 1)
                    {
                        nearest = preKey + offset;
                        break;
                    }


                    if (preKey - offset > 0 && BigInteger.GreatestCommonDivisor(preKey - offset, n) == 1)
                    {
                        nearest = preKey - offset;
                        break;
                    }
                    offset++;
                }
                validatedKey = nearest.ToString();
                output += $"Ключ не взаимно простой с 33, используется ближайший походящий ключ({validatedKey.ToString()})";


            }
            else
            {
                validatedKey = preKey.ToString();
            }
            return validatedKey;
        }

        public string Cipher(string plainText, DataGridView dgv)
        {
            if (plainText.Length == 0)
                return "";

            string cipherText = "";
            dgv.Columns.Clear();
            dgv.Rows.Clear();
            int j = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                if (ValidateSymbol(char.ToLower(plainText[i])))
                {

                    dgv.Columns.Add($"col({j}", "");
                    dgv.Columns[j].Width = 130;
                    
                    if (j >= dgvMax)
                        break;
                    j++;
                }
            }
            if (dgv.Rows.Count > 0)
            {
                dgv.Rows.Add(3);
            }
            j = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                char plainSymbol = char.ToLower(plainText[i]);
                if (ValidateSymbol(plainSymbol))
                {

                    int plainVal = alphToNum[char.ToLower(plainSymbol)];


                    int cipherVal = (int)(plainVal * key % n);
                    char cipherSymbol = numToAlph[cipherVal];
                    if (j < dgvMax)
                    {
                        dgv[j, 0].Value = plainSymbol;
                        dgv[j, 1].Value = $"{plainVal}({plainSymbol})*{key}%{n}={cipherVal}({cipherSymbol})";
                        dgv[j, 2].Value = cipherSymbol;
                    }
                    cipherText += cipherSymbol;
                    j++;
                }
                else
                {
                    cipherText += plainSymbol;
                }
            }
            return cipherText;
        }

        public string DeCipher(string cipherText, DataGridView dgv)
        {
            if (cipherText.Length == 0)
                return "";
            string plainText = "";
            dgv.Columns.Clear();
            dgv.Rows.Clear();
            int j = 0;
            for (int i = 0; i < cipherText.Length; i++)
            {
                if (ValidateSymbol(char.ToLower(cipherText[i])))
                {
                    dgv.Columns.Add($"col({i}", "");
                    dgv.Columns[j].Width = 130;
                    j++;
                }
            }
            if (dgv.Rows.Count > 0)
            {
                dgv.Rows.Add(3);
            }
            j = 0;
            for (int i = 0; i < cipherText.Length; i++)
            {
                var cipherSymbol = cipherText[i];
                if (ValidateSymbol(char.ToLower(cipherSymbol)))
                {
                    int cipherVal = alphToNum[char.ToLower(cipherSymbol)];
                    BigInteger temp = cipherVal;
                    while (temp % key != 0)
                        temp += n;
                    int plainVal = (int)(temp / key);
                    char plainSymbol = numToAlph[plainVal];
                    if (j < dgvMax)
                    {
                        dgv[j, 0].Value = plainSymbol;
                        dgv[j, 1].Value = $"{plainVal}({plainSymbol})*{key}%{n}={cipherVal}({cipherSymbol})";
                        dgv[j, 2].Value = cipherSymbol;
                    }
                    plainText += plainSymbol;
                    j++;
                }
                else
                {
                    plainText += cipherSymbol;
                }
            }
            return plainText;
        }

        public void SetKey(string key, out string output)
        {
            string validatedKey = ValidateKey(key, out output);
            DecimationCryptoSystem.key = BigInteger.Parse(validatedKey);
        }

        public string GetKey()
        {
            return DecimationCryptoSystem.key.ToString();
        }


    }
}

