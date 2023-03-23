﻿using SaleSystem.BLL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SaleSystem.BLL.Implementations
{
    public class UtilidadesService : IUtilidadesService
    {

        public string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }
        public string ConvertirSha256(string texto)
        {

            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;

                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }

            }

            return sb.ToString();

        }


    }
}
