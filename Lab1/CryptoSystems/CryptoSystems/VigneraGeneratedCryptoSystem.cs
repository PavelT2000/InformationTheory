using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CryptoSystems
{
    public class VigneraGeneratedCryptoSystem : ICryptoSystem
    {
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
            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
            'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т',
            'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь',
            'э', 'ю', 'я'
        };

        private static string key { get; set; }

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
                if (ValidateSymbol(char.ToLower(c)))
                    validatedKey += c;
                else
                    skippedCount++;
            }
            if (validatedKey == "" && skippedCount != 0)
            {
                output = $"В ключе отсутсвует необходимые символы, пропущено {skippedCount} лишних символов, берётся ключ по умолчанию (а)\n";
                validatedKey = "а";
            }
            else if (validatedKey == "")
            {
                output = $"Ключ пустой, берётся ключ по умолчанию (а)\n";
                validatedKey = "а";
            }
            else if (skippedCount != 0)
            {
                output = $"Ключ содержит что-то кроме необходимых символов, пропущено {skippedCount} лишних символов\n";
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
            j= 0;
            for (int i = 0; i < plainText.Length; i++)
            {

                
                
                char plainSymbol = char.ToLower(plainText[i]);
                if (ValidateSymbol(plainSymbol))
                {
                    int k = j;
                    bool keyFlag = false;
                    if (k > key.Length - 1)
                    {
                        k -= key.Length-1;
                        keyFlag = true;
                    }

                    int plainVal = alphToNum[char.ToLower(plainSymbol)];
                    if (keyFlag)
                    {
                        for(int l=0,f=0;l<plainText.Length; l++)
                        {
                            if (ValidateSymbol(char.ToLower(plainText[l])))
                            {
                                f++;
                                if(f==k)
                                {
                                    k = l;
                                    break;
                                }    
                                
                            }
                        }

                    }
                    char keyChar = char.ToLower(keyFlag ? plainText[k] : key[k]);
                    int keyVal = alphToNum[keyChar];

                    int cipherVal = (plainVal+keyVal)%n;
                    char cipherSymbol = numToAlph[cipherVal];
                    if (j < dgvMax)
                    {
                        dgv[j, 0].Value = plainSymbol;
                        dgv[j, 1].Value = $"{plainVal}({plainSymbol})+{keyVal}({keyChar})%{n}={cipherVal}({cipherSymbol})";
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
            for (int i = 0; i < cipherText.Length; i++)
            {
                var cipherSymbol = cipherText[i];
                if (ValidateSymbol(char.ToLower(cipherSymbol)))
                {
                    int cipherVal = alphToNum[char.ToLower(cipherSymbol)];
                    int keyVal;
                    if (j >= key.Length)
                    {
                        int k = j - key.Length + 1;
                        for (int l = 0, f = 0; l < plainText.Length; l++)
                        {
                            if (ValidateSymbol(char.ToLower(plainText[l])))
                            {
                                f++;
                                if (f == k)
                                {
                                    k = l;
                                    break;
                                }

                            }
                        }
                        keyVal = alphToNum[plainText[k]];
                    }
                    else
                    {
                        keyVal = alphToNum[char.ToLower(key[j])];
                    }
                    char keyChar = numToAlph[keyVal];
                    if (cipherVal < keyVal)
                        cipherVal += n;

                    int plainVal = cipherVal-keyVal;
                    char plainSymbol = numToAlph[plainVal];
                    if (j < dgvMax)
                    {
                        dgv[j, 0].Value = plainSymbol;
                        dgv[j, 1].Value = $"{plainVal}({plainSymbol})+{keyVal}({keyChar})%{n}={cipherVal}({cipherSymbol})";
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
            VigneraGeneratedCryptoSystem.key = validatedKey;
        }

        public string GetKey()
        {
            return VigneraGeneratedCryptoSystem.key.ToString();
        }


    }
}

