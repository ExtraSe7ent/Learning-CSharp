using System;
using System.Linq;

public static class NumberConverter
{
    public static string ConvertBase(string inputValue, int fromBase, int toBase)
    {
        try
        {
            int decimalValue = Convert.ToInt32(inputValue, fromBase);
            string result = Convert.ToString(decimalValue, toBase);

            if (toBase == 2)
            {
                int currentLength = result.Length;
                int targetLength = ((currentLength - 1) / 4 + 1) * 4;
                result = result.PadLeft(targetLength, '0');
            }
            else if (toBase == 16)
            {
                result = result.ToUpper();
            }

            return result;
        }
        catch (FormatException)
        {
            Console.WriteLine($"\nLoi: Gia tri '{inputValue}' chua ky tu khong hop le cho he co so {fromBase}.");
            return null;
        }
        catch (OverflowException)
        {
            Console.WriteLine($"\nLoi: Gia tri '{inputValue}' qua lon de xu ly.");
            return null;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            int inputBase = ChooseBase("dau vao");
            if (inputBase == 0) break;

            int outputBase = ChooseBase("dau ra");
            if (outputBase == 0) break;

            Console.Write("\nNhap gia tri can chuyen doi: ");
            string inputValue = Console.ReadLine().Trim().ToUpper();

            string outputValue = NumberConverter.ConvertBase(inputValue, inputBase, outputBase);

            if (outputValue != null)
            {
                Console.WriteLine($"\nKet qua: Gia tri '{inputValue}' (he {inputBase}) tuong duong voi '{outputValue}' (he {outputBase}).");
            }
        }
    }

    static int ChooseBase(string type)
    {
        while (true)
        {
            Console.WriteLine($"\nChon he co so {type}:");
            Console.WriteLine("1. Binary (He 2)");
            Console.WriteLine("2. Decimal (He 10)");
            Console.WriteLine("3. Hexadecimal (He 16)");
            Console.WriteLine("4. Thoat");
            Console.Write("Lua chon cua ban: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": return 2;
                case "2": return 10;
                case "3": return 16;
                case "4": return 0;
                default:
                    Console.WriteLine("Lua chon khong hop le. Vui long chon lai.");
                    break;
            }
        }
    }
}
