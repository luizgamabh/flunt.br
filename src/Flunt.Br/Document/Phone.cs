using System.Text.RegularExpressions;
using Flunt.Br.Document.interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Flunt.Br.Document
{
    internal class Phone : IValidateV2
    {

        public Phone()
        {
            this.format = null;
        }
        public Phone(string numberFormat)
        {
            var regex = new Regex(@"[0-9]+");
            var phonePattern = Regex.Replace(numberFormat, @"[0-9,?]+", m => { 
                var optionalsCount = m.Value.Split(new char[] { '?' }).Length - 1;
                var stringRegex = "";
                if(optionalsCount != 0)
                {
                    stringRegex += (m.Length - optionalsCount);
                    stringRegex += ",";
                    stringRegex += (optionalsCount + (m.Length - optionalsCount));
                }
                else
                {
                     stringRegex = m.Length.ToString();
                }
                return $@"\d{{{stringRegex}}}";
            });
            phonePattern = @"^" + Regex.Replace(phonePattern, @"(?:(?(1)(?!))(\+)|(?(2)(?!))(\()|(?(3)(?!))(\)))+", m => $@"\{m.Value}").Replace("-", @"\-").Replace(" ", @"\s") + "$";
            this.format = new Regex(phonePattern);
        }

        public bool Validate(string value, bool strictNineDigit = false, bool cellPhonesOnly = false)
        {
            if (this.format != null)
            {
                return this.format.IsMatch(value);
            }
            else
            {
                var brazilianPhonePattern = cellPhonesOnly
                    ? strictNineDigit
                        ? new Regex(@"^(?:(?:\+?55)?[ .-]*\(?0?[1-9][0-9]\)?)[ .-]?(?:(?:(?:9[ .-]*)[0-9]{4})[ .-]*[0-9]{4})$")
                        : new Regex(@"^(?:(?:\+?55)?[ .-]*\(?0?[1-9][0-9]\)?)[ .-]?(?:(?:(?:9[ .-]*)[0-9]{4}|[8-9][0-9]{3})[ .-]*[0-9]{4})$")
                    : strictNineDigit
                        ? new Regex(@"^(?:(?:\+?55)?[ .-]*\(?0?[1-9][0-9]\)?)[ .-]?(?:(?:9[ .-]*[0-9]{4}|[1-7][0-9]{3})[ .-]*[0-9]{4})$")
                        : new Regex(@"^(?:(?:\+?55)?[ .-]*\(?0?[1-9][0-9]\)?)[ .-]?(?:(?:(?:9[ .-]*)?[0-9]{4})[ .-]*[0-9]{4})$");
                return brazilianPhonePattern.IsMatch(value);
            }
        }

        private Regex format { get; set; }
    }
}