using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CryptoSystems
{
    public interface ICryptoSystem
    {
        public bool ValidateSymbol(char  symbol);
        public string ValidateKey(string key, out string output);

        public string Cipher(string plainText, DataGridView dgv);

        public string DeCipher(string cipherText, DataGridView dgv);

        public void SetKey(string key, out string output);

        public string GetKey();
        
    }
}
