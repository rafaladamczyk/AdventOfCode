using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2022
{
    class Day13
    {
        public class ListOfStuff
        {
            public List<ListOfStuff> elements = new List<ListOfStuff>();
            public int? singleValue;
            public ListOfStuff parent;
            public bool IsNumber => singleValue != null;
            public string Name;
        }

        public class StuffComparer : IComparer<ListOfStuff>
        {
            public int Compare(ListOfStuff x, ListOfStuff y)
            {
                var result = AreInOrder(x, y);
                if (result == null)
                {
                    return 0;
                }

                return result.Value ? -1 : 1;
            }
        }

        public static bool? AreInOrder(ListOfStuff first, ListOfStuff second)
        {
            if (first.IsNumber && second.IsNumber)
            {
                if (first.singleValue == second.singleValue)
                {
                    return null;
                }

                return first.singleValue.Value < second.singleValue;
            }

            if (first.IsNumber)
            {
                first = new ListOfStuff { elements = { first } };
            }

            if (second.IsNumber)
            {
                second = new ListOfStuff { elements = { second } };
            }

            for (int i = 0; i < first.elements.Count; i++)
            {
                if (second.elements.Count - 1 < i)
                {
                    return false;
                }

                var compareElements = AreInOrder(first.elements[i], second.elements[i]);
                if (compareElements != null)
                {
                    return compareElements;
                }
            }

            if (second.elements.Count > first.elements.Count)
            {
                return true;
            }

            return null;
        }

        static string PrintList(ListOfStuff list)
        {
            var x = new StringBuilder();

            x.Append('[');

            if (list.IsNumber)
            {
                x.Append(list.singleValue.Value);
            }

            else
            {
                foreach (var element in list.elements)
                {
                    x.Append(PrintList(element));
                    x.Append(',');
                }

                if (list.elements.Any())
                {
                    x.Remove(x.Length - 1, 1);
                }
            }

            x.Append(']');

            return x.ToString();
        }

        public static void Run()
        {
            List<ListOfStuff> input = new List<ListOfStuff>();

            using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\input-13.txt"))
            //using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\example.txt"))
            {
                using (var reader = new StreamReader(f))
                {
                    while (!reader.EndOfStream)
                    {
                        var list1 = ParseListOfStuff(reader.ReadLine());
                        var list2 = ParseListOfStuff(reader.ReadLine());
                        reader.ReadLine();

                        input.Add(list1);
                        input.Add(list2);
                    }
                }
            }

            var first = ParseListOfStuff("[[2]]");
            var second = ParseListOfStuff("[[6]]");

            first.Name = "first";
            second.Name = "second";

            input.Add(first);
            input.Add(second);

            input.Sort(new StuffComparer());

            var firstIndex = -123;
            var secondInex = -111;

            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].Name == "first")
                {
                    firstIndex = i + 1;
                }

                if (input[i].Name == "second")
                {
                    secondInex = i + 1;
                }
            }

            foreach (var list in input)
            {
                Console.WriteLine(PrintList(list));
            }

            Console.WriteLine(firstIndex * secondInex);
        }

        public static ListOfStuff ParseListOfStuff(string text)
        {
            ListOfStuff current = null;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                
                switch (c)
                {
                    case '[':
                        var newList = new ListOfStuff();
                        if (current != null)
                        {
                            newList.parent = current;
                            current.elements.Add(newList);
                        }

                        current = newList;
                        break;

                    case ']':
                        if (sb.Length > 0)
                        {
                            var newElement = int.Parse(sb.ToString());
                            sb = new StringBuilder();
                            current.elements.Add(new ListOfStuff { singleValue = newElement, parent = current });
                        }

                        if (current.parent != null)
                        {
                            current = current.parent;
                        }

                        break;
                    
                    case ',':
                        if (sb.Length > 0)
                        {
                            var newElement = int.Parse(sb.ToString());
                            sb = new StringBuilder();
                            current.elements.Add(new ListOfStuff { singleValue = newElement, parent = current });
                        }

                        break;
                    
                    default:
                    {
                        sb.Append(c);
                        break;
                    }
                }
            }

            return current;
        }
    }
}
