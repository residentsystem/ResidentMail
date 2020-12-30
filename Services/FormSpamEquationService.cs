using System;
using ResidentMail.Models;

namespace ResidentMail.Services
{
    public class FormSpamEquationService
    {
        private readonly Random randomnumber;

        public FormSpamEquationService()
        {
            randomnumber = new Random();
        }
        public Equation GetEquationNumbers()
        {
            Equation equation = new Equation();

            equation.FirstNumber = randomnumber.Next(10);
            equation.SecondNumber = randomnumber.Next(10);
            equation.Sum = equation.FirstNumber + equation.SecondNumber;

            return equation;
        }
    }
}