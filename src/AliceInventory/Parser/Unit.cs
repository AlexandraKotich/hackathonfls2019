using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ConsoleApp
{
    public class Unit
    {

        //this class for number units of objects 
        public string UnitName { get; set; }
        

        public Unit(string unitName)
        {
            Dictionary<string, string> unitDictionary = GetSynonyms();
            try { UnitName = unitDictionary[unitName]; }
            catch (Exception exc)
            {
                UnitName = "шт";
            }

        }
        //not sure, perhaps this method implements some part of parser analysis funcion TryParse()
        public Dictionary <string,string> GetSynonyms() 
        {
            Dictionary<string, string> unitDictionary = new Dictionary<string, string>();
            unitDictionary.Add("килограмм", "кг");
            unitDictionary.Add("килограмма", "кг");
            unitDictionary.Add("килограммов", "кг");
            unitDictionary.Add("кг", "кг");
            unitDictionary.Add("г", "г");
            unitDictionary.Add("грамм", "г");
            unitDictionary.Add("граммов", "г");
            unitDictionary.Add("миллиграммов", "мг");
            unitDictionary.Add("миллиграмм", "мг");
            unitDictionary.Add("миллиграмма", "мг");
            unitDictionary.Add("литров", "л");
            unitDictionary.Add("литр", "л");
            unitDictionary.Add("литра", "л");
            unitDictionary.Add("миллилитры", "мл");
            unitDictionary.Add("миллилитров", "мл");
            unitDictionary.Add("миллилитра", "мл");
            unitDictionary.Add("метра", "м");
            unitDictionary.Add("метров", "м");
            unitDictionary.Add("метр", "м");
            unitDictionary.Add("миллиметр", "мм");
            unitDictionary.Add("миллиметров", "мм");
            unitDictionary.Add("миллиметра", "мм");
            unitDictionary.Add("сантиметра", "см");
            unitDictionary.Add("сантиметров", "см");
            unitDictionary.Add("сантиметр", "см");
            unitDictionary.Add("штук", "шт");
            unitDictionary.Add("штуки", "шт");
            unitDictionary.Add("штука", "шт");
            return  unitDictionary;
        }

        public override string ToString()
        {
            return UnitName;
        }
       
    }
}
