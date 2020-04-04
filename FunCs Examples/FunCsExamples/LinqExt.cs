using FunCs;
using System;
using System.Collections.Generic;

namespace FunCsExamples
{
    public class LinqExt
    {
        private static OptionF<double> ToRealNumber(string s)
        {
            bool ok = double.TryParse(s, out double result);
            if (ok)
                return OptionF<double>.Some(result);
            else
                return OptionF<double>.None();
        }

        private static OptionF<int> ToPassingGrade(int n)
        {
            if (n >= 5 && n <= 10)
                return OptionF<int>.Some(n);
            else
                return OptionF<int>.None();
        }

        public static void Ex1()
        {
            var list = "[ 10 3.50 absent 9.75 5 7.25 ]".ToStringEnumF()
                 .Map(s => ToRealNumber(s))
                 .Map(optd => optd.Map(d => (int)Math.Round(d)))
                 .Map(opti => opti.Bind(i => ToPassingGrade(i)))
                 .FilterSome();

            Console.WriteLine(list.ToStringF());
        }

        private class Student
        {
            public string Name { get; }
            public string Result { get; }

            public Student(string name, string result)
            {
                Name = name;
                Result = result;
            }
        }

        public static void Ex2()
        {
            var students = new List<Student>
            {
                new Student("Alex Neagoe",     "10"),
                new Student("Carla Stoenescu", "3.50"),
                new Student("Flavius Predoiu", "absent"),
                new Student("Doina Vasiliu",   "9.75"),
                new Student("Dan Draghicescu", "5"),
                new Student("Maria Raducanu",  "7.25")
            };

            var list = students
                .Map(s => (Name: s.Name, Result: s.Result))
                .Map(t => (Name: t.Name, OptRealResult: ToRealNumber(t.Result)))
                .Map(t => (Name: t.Name, OptIntResult: t.OptRealResult.Map(d => (int)Math.Round(d))))
                .Map(t => (Name: t.Name, OptGrade: t.OptIntResult.Bind(i => ToPassingGrade(i))))
                .Filter(t => t.OptGrade.IsSome)
                .Map(t => t.Name + " - " + t.OptGrade.Value);

            foreach (string s in list)
                Console.WriteLine(s);
        }
    }
}