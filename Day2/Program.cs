using System;
using System.Collections.Generic;
using System.IO;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Valid Passwords: " + NewValidPasswords());
        }

        static int ValidPasswords()
        {
            StreamReader sr = new StreamReader("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day2/input.txt");
            
            int minLetterCount;
            int maxLetterCount;
            char requiredLetter;
            string password;
            int validPasswords = 0;

            string line = sr.ReadLine();

            while (line != null)
            {
                string[] str = line.Split(new Char[] { ' ', ':', '-' });

                minLetterCount = Int32.Parse(str[0]);
                maxLetterCount = Int32.Parse(str[1]);
                requiredLetter = Char.Parse(str[2]);
                password = str[4];

                int letterCount = 0;

                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] == requiredLetter)
                    {
                        letterCount++;
                        if (letterCount > maxLetterCount) break;
                    }
                    if (i == password.Length - 1)
                    {
                        if (letterCount >= minLetterCount && letterCount <= maxLetterCount) validPasswords++;
                        Console.WriteLine(password);
                    }                 
                }

                line = sr.ReadLine();
            }

            return validPasswords;
        }

        static int NewValidPasswords()
        {
            StreamReader sr = new StreamReader("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day2/input.txt");

            int firstPosition;
            int secondPosition;
            char requiredLetter;
            string password;
            int validPasswords = 0;

            string line = sr.ReadLine();

            while (line != null)
            {
                string[] str = line.Split(new Char[] { ' ', ':', '-' });

                firstPosition = Int32.Parse(str[0]);
                secondPosition = Int32.Parse(str[1]);
                requiredLetter = Char.Parse(str[2]);
                password = str[4];


                if (password[firstPosition-1] == requiredLetter)
                {

                    if (password[secondPosition - 1] != requiredLetter)
                    {
                        Console.WriteLine(password);
                        validPasswords++;
                    }
                }
                else
                {
                    if (password[secondPosition-1] == requiredLetter)
                    {
                        Console.WriteLine(password);
                        validPasswords++;
                    }
                }

                line = sr.ReadLine();
            }

            return validPasswords;
        }
    }
}
