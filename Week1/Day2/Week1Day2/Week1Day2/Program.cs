namespace Week1Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ValueTypeAndReferenceType();
            mutation();

            Console.Write("Enter Score : ");
            int score = int.Parse(Console.ReadLine());
            DescribeGrade(score);

            Console.Write("Enter your name : ");
            string name = Console.ReadLine();
            handleError(name);
        }

        private static void ValueTypeAndReferenceType()
        {
            int x = 5;
            double y = 5.4D;
            float z = 5.4f;

            Console.WriteLine(x.GetType());
            Console.WriteLine(y.GetType());
            Console.WriteLine(z.GetType());

            string name = "Heba Hesham";
            List<int> list = new List<int>();
            int[] arr = new int[5];

            Console.WriteLine(name.GetType());
            Console.WriteLine(list.GetType());
            Console.WriteLine(arr.GetType());
        }

        private static void mutation()
        {
            int x = 5;
            int y = x;

            Console.WriteLine($"Before Change x value :{y}");
            x = 10;
            Console.WriteLine($"After Change x value :{y}");

            List<int> list1 = new List<int> { 1, 2, 3 };
            List<int> list2 = list1;

            Console.WriteLine($"Before Change list1 {string.Join(", ", list2)}");
            list1.Add(4);
            Console.WriteLine($"After Change list1 {string.Join(", ", list2)}");
        }

        private static void DescribeGrade(int score)
        {
            switch (score)
            {
                case >= 90:
                    Console.WriteLine("Excellent");
                    break;
                case >= 70:
                    Console.WriteLine("Proficient");
                    break;
                case >= 50:
                    Console.WriteLine("Developing");
                    break;
                default:
                    Console.WriteLine("Below Standard");
                    break;
            }
        }

        private static void handleError(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("The name is null ! Please try again");
            }
            else
            {
                Console.WriteLine(name);
            }
        }
    }
}