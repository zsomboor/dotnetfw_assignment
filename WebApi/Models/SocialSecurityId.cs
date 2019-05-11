using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class SocialSecurityId
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public SocialSecurityId(string idString)
        {
            int partA;
            int partB;
            int partC;
            if (int.TryParse(idString.Substring(0, 3), out partA) && int.TryParse(idString.Substring(4, 3), out partB) && int.TryParse(idString.Substring(8, 3), out partC))
            {
                A = partA;
                B = partB;
                C = partC;
            }
            else
                throw new FormatException();
        }

        public static implicit operator SocialSecurityId(string idString)
        {
            try
            {
                return new SocialSecurityId(idString);
            }
            catch (FormatException ex)
            {
                Debug.WriteLine("Failed to convert string to SocialSecurityId");
                throw ex;
            }
        }

        public static implicit operator string(SocialSecurityId idString)
        {
            try
            {
                return idString.ToString();
            }
            catch (FormatException ex)
            {
                Debug.WriteLine("Failed to convert string to SocialSecurityId");
                throw ex;
            }
        }

        public override string ToString()
        {
            return $"{A}-{B}-{C}";
        }
    }
}