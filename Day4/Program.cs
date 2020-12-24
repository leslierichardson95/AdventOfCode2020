using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Valid Passports: " + ValidPassports());
            Console.WriteLine("Legit Valid Passports: " + ValidPassportValues());
        }

        static int ValidPassportValues()
        {
            StreamReader sr = new StreamReader("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day4/input.txt");
            
            string[] passports = sr.ReadToEnd().Split("\n\n");
            int validPassportValues = 0;

            foreach (string passport in passports)
            {
                if (IsValidPassport(passport))
                {
                    string[] passportFields = passport.Replace("\n", " ").Split(" ");
                    int validFields = 0;

                    for (int i = 0; i < passportFields.Length; i++)
                    {
                        string passportField = passportFields[i];

                        if (passportField.Contains("byr"))
                        {
                            int birthYear = int.Parse(passportField.Substring(4));
                            if (birthYear >= 1920 && birthYear <= 2002) validFields++;
                        }
                        else if (passportField.Contains("iyr"))
                        {
                            int issueYear = int.Parse(passportField.Substring(4));
                            if (issueYear >= 2010 && issueYear <= 2020) validFields++;
                        }
                        else if (passportField.Contains("eyr"))
                        {
                            int expirationYear = int.Parse(passportField.Substring(4));
                            if (expirationYear >= 2020 && expirationYear <= 2030) validFields++;
                        }
                        else if (passportField.Contains("hgt"))
                        {
                            if (passportField.EndsWith("cm"))
                            {
                                passportField = passportField.Substring(4);
                                int height = int.Parse(passportField.Substring(0, passportField.Length - 2));
                                if (height >= 150 && height <= 193) validFields++;
                            }
                            else if (passportField.EndsWith("in"))
                            {
                                passportField = passportField.Substring(4);
                                int height = int.Parse(passportField.Substring(0, passportField.Length - 2));
                                if (height >= 59 && height <= 76) validFields++;
                            }
                        }
                        else if (passportField.Contains("hcl"))
                        {
                            string hairColor = passportField.Substring(4);
                            if (hairColor.StartsWith("#"))
                            {
                                hairColor = hairColor.Replace("#", "");

                                int result = 0;
                                // check if valid hex number
                                if (int.TryParse(hairColor, System.Globalization.NumberStyles.HexNumber, null, out result)) validFields++;
                            }
                        }
                        else if (passportField.Contains("ecl"))
                        {
                            string eyeColor = passportField.Substring(4);
                            if (eyeColor.Equals("amb") || eyeColor.Equals("blu") || eyeColor.Equals("brn") || eyeColor.Equals("gry") ||
                                eyeColor.Equals("grn") || eyeColor.Equals("hzl") || eyeColor.Equals("oth")) validFields++;
                        }
                        else if (passportField.Contains("pid"))
                        {
                            string passportId = passportField.Substring(4);
                            int idResult;
                            if (passportId.Length == 9 && int.TryParse(passportId, out idResult)) validFields++;
                        }
                    }

                    if (validFields == 7) {
                        validPassportValues++;
                    } 
                }
            }

            return validPassportValues;
        }

        static bool IsValidPassport(string passport)
        {
            if (passport.Contains("byr") && passport.Contains("iyr") && passport.Contains("eyr") &&
                passport.Contains("hgt") && passport.Contains("hcl") && passport.Contains("ecl") &&
                passport.Contains("pid"))
            {
                return true;
            }
            else return false;
        }

        static int ValidPassports()
        {
            StreamReader sr = new StreamReader("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day4/input.txt");
            string line = sr.ReadLine();

            int validPassports = 0;
            string passport = "";

            while (line != null)
            {

                if (!line.Equals("")) // if line is not empty
                {
                    passport += line;
                }
                else
                {
                    if (passport.Contains("byr") && passport.Contains("iyr") && passport.Contains("eyr") &&
                        passport.Contains("hgt") && passport.Contains("hcl") && passport.Contains("ecl") &&
                        passport.Contains("pid"))
                    {
                        Console.WriteLine(passport +"\n");
                        validPassports++;
                    }

                    passport = "";
                }

                line = sr.ReadLine();

            }

            // check final password
            if (passport.Contains("byr") && passport.Contains("iyr") && passport.Contains("eyr") &&
                passport.Contains("hgt") && passport.Contains("hcl") && passport.Contains("ecl") &&
                passport.Contains("pid"))
            {
                Console.WriteLine(passport + "\n");
                validPassports++;
            }

            return validPassports;
        }
    }
}
