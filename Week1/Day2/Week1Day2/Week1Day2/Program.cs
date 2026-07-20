namespace Week1Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //// A.Stirng and Character :
            //Console.WriteLine("Heba Hesham");
            //Console.WriteLine("My First Character Of My Name is :");
            //char ch = 'H';
            //Console.WriteLine(ch);

            //// B. Numaric Data Type :

            //// 1. integer 
            //int num1 = 5;
            //// 2. double :
            //double num2 = 5.1D;
            //// 3.float
            //float num3 = 5.1f;
            //// 4. decimal :
            //decimal num4 = 5.1M;

            //// C. Convert sting to integer:

            //// 1. Small Numbeer :
            //string textNum1 = "5";
            //int convertNum1 = Convert.ToInt32(textNum1);
            //Console.WriteLine(convertNum1);

            //// 2. Big Number :

            //string textNum2 = "800400600700";
            //long convertNum2 = Convert.ToInt64(textNum2);
            //Console.WriteLine(convertNum2);

            //// 3.Double Number :
            //string textNum3 = "8004.00600700";
            //double convertNum3 = Convert.ToDouble(textNum2);
            //Console.WriteLine(convertNum3);

            //// 4.Float Number :
            //string textNum4 = "800.400600700";
            //float convertNum4 = Convert.ToSingle(textNum2);
            //Console.WriteLine(convertNum4);

            //// 5.Decimal Number :
            //string textNum5 = "8.23";
            //decimal convertNum5 = Convert.ToDecimal(textNum5);
            //Console.WriteLine(convertNum5);

            //// D. Boolean Data Type :

            //bool isFemale = true;
            //bool isMale = false;

            //Console.WriteLine(isFemale);
            //Console.WriteLine(isMale);

            //// E. Operators :

            //// 1. Number Operator :
            //int age = 22;
            //age++; // same : age+=1 , age = age+1
            //Console.WriteLine(age);
            //age--; // same : age-=1 , age = age-1
            //Console.WriteLine(age);

            //// 2. String Operator :
            //string name = "Heba";
            //name += " ";
            //name += "Hesham";
            //Console.WriteLine(name);

            //// F. Remander 
            //int fisrNum = 10;
            //int SecondNum = 2;
            //Console.WriteLine(fisrNum % SecondNum); // this operater using when determine if the number is even or odd (popular example)

            //// G. Keyword Types :
            //// 1. Var Key :
            //// Define the type at complier not run 
            //var num7 = 10; // int
            //               // 2. Const key
            //               // Fixed value not change at anewhere in the code
            //const int num8 = 1;
            //// num8 = 2; error 



            //// H. Control Flow :
            //// 1. If Statements :
            //if (num1 < 5)
            //    Console.WriteLine("num1 is less 5");
            //else if (num1 > 5)
            //    Console.WriteLine("num1 is grater 5");
            //else
            //    Console.WriteLine("num1 is equal 5");

            //int x = 5;
            //while (x-- > 0)
            //{
            //    Console.Write(x+" ");

            //}
            //Console.WriteLine();
            //x = 5;
            //for (int i = 0;i< x; i++)
            //{
            //    Console.Write(i+" ");
            //}
            //Console.WriteLine();


            //// Switch :
            //Console.WriteLine("Enter Any Number : ");
            //int sw = int.Parse(Console.ReadLine());
            //switch (sw)
            //{
            //    case 1:
            //        Console.WriteLine("The num equal 1");
            //        break;
            //    case 2:
            //        Console.WriteLine("The num equal 2");
            //        break;
            //    case 3:
            //        Console.WriteLine("The num equal 3");
            //        break;
            //    default:
            //        Console.WriteLine("The number is greater than 3");
            //        break;
            //}

            //Exercises();
            // tasks : 
            ValueTypeAndReferenceType();
            mutation();
            Console.Write("Enter Score : ");
            int score = int.Parse(Console.ReadLine());
            DescribeGrade(score);

        }

        //private static void Exercises()
        //{
        //    // 1. Storing User Data :
        //    // any number contain zero number at the first : must store it at stirng type or simplify all storage operater using "va"

        //    // 2. odd/even checking :

        //    int num1 = 5;
        //    int num2 = 2;
        //    int remender = num1 % num2; // 0 => even / 1 => odd
        //    Console.WriteLine(remender +" number is odd "); // number is odd

        //    int num3 = 10;
        //    remender = num1 % num2;
        //    Console.WriteLine(remender +" number is even "); // number is even

        //    // 3. Console Output :
        //    Console.WriteLine("Heba Hesham");

        //    // 4.Console Input :

        //    // int :
        //    Console.WriteLine("enter any number you want print it ");
        //    int num4 = int.Parse(Console.ReadLine());
        //    Console.WriteLine(num4);

        //    // string : 
        //    Console.ReadLine();

        //}
        // 2.1 : Value Types vs. Reference Types
        private static void ValueTypeAndReferenceType()
        {
            // 1. Write a console program with at least 3 value-type and 3 reference-type variables, printing each one's type using 
            //"GetType()".

            int x = 5;
            double y = 5.4D;
            float z = 5.4f;

            Console.WriteLine(x.GetType());
            Console.WriteLine(y.GetType());
            Console.WriteLine(z.GetType());

            string name = "Heba Hesham";
            List<int> list = new List<int>();
            int[]arr = new int[5];
            Console.WriteLine(name.GetType());
            Console.WriteLine(list.GetType());
            Console.WriteLine(arr.GetType());

        }
        // 2.2
        private static void mutation()
        {
            int x = 5;
            int y = x;
            Console.WriteLine($"Befor Change x value :{y}"); 
            x = 10;
            Console.WriteLine($"Aftere Change x value :{y}"); // x dont effect y

            List<int> list1= new List<int> { 1, 2, 3 };
            List<int> list2 = list1;

            Console.WriteLine($"Befor Change list1 {string.Join(", ", list2)}");
            list1.Add(4);
            Console.WriteLine($"After Change list1 {string.Join(", ", list2)}");// list 1 effect list 2

        }
        // 2.3 
        private static void DescribeGrade(int score)
        {
            switch (score) {
                case >= 90:  Console.WriteLine("Excellent");
                    break;
                case >= 70:
                    Console.WriteLine("Proficient");
                    break;
                case >= 50:
                    Console.WriteLine("Developing");
                    break;
                case < 50:
                    Console.WriteLine("Below Standard");
                    break;
            }
        }

    }
}

