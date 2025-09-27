using System;
using System.Text;

public class Matrix
{
    private readonly int[,] data;
    public int Rows { get; }
    public int Columns { get; }

    public Matrix(int rows, int columns)
    {
        if (rows < 0 || columns < 0)
        {
            throw new ArgumentException("So dong va cot khong duoc la so am.");
        }
        Rows = rows;
        Columns = columns;
        data = new int[rows, columns];
    }

    public int this[int row, int col]
    {
        get => data[row, col];
        set => data[row, col] = value;
    }
    
    public void Input()
    {
        for (int i = 0; i < Rows; i++)
        {
            while (true)
            {
                Console.Write($"Nhap cac phan tu cho dong {i + 1}: ");
                
                string? line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("Loi: Chuoi nhap rong, vui long thu lai.");
                    continue;
                }

                string[] numberStrings = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (numberStrings.Length != Columns)
                {
                    Console.WriteLine($"Loi: Vui long nhap dung {Columns} phan tu.");
                    continue;
                }

                try
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        this[i, j] = int.Parse(numberStrings[j]);
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Loi: Mot trong cac gia tri ban nhap khong phai la so hop le. Vui long nhap lai.");
                }
            }
        }
    }

    public void Display(string message)
    {
        Console.WriteLine(message);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Console.Write(this[i, j].ToString().PadRight(5));
            }
            Console.WriteLine();
        }
    }

    public static Matrix? operator +(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
        {
            return null;
        }
        
        Matrix result = new Matrix(a.Rows, a.Columns);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Columns; j++)
            {
                result[i, j] = a[i, j] + b[i, j];
            }
        }
        return result;
    }

    public static Matrix? operator *(Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows)
        {
            return null;
        }
        
        Matrix result = new Matrix(a.Rows, b.Columns);
        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                int sum = 0;
                for (int k = 0; k < a.Columns; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                result[i, j] = sum;
            }
        }
        return result;
    }
    
    public Matrix Transpose()
    {
        Matrix result = new Matrix(Columns, Rows);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                result[j, i] = this[i, j];
            }
        }
        return result;
    }

    public int FindMin()
    {
        if (Rows == 0 || Columns == 0)
        {
            throw new InvalidOperationException("Khong the tim Min trong ma tran rong.");
        }

        int min = this[0, 0];
        foreach (int value in data)
        {
            if (value < min) min = value;
        }
        return min;
    }

    public int FindMax()
    {
        if (Rows == 0 || Columns == 0)
        {
            throw new InvalidOperationException("Khong the tim Max trong ma tran rong.");
        }
        
        int max = this[0, 0];
        foreach (int value in data)
        {
            if (value > max) max = value;
        }
        return max;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Matrix a = InputMatrix("A");
        Matrix b = InputMatrix("B");

        while (true)
        {
            Console.WriteLine("\n1. Cong hai ma tran (A + B)");
            Console.WriteLine("2. Nhan hai ma tran (A x B)");
            Console.WriteLine("3. Tim ma tran chuyen vi (cho A va B)");
            Console.WriteLine("4. Tim Min/Max trong ma tran (cho A va B)");
            Console.WriteLine("5. Hien thi lai hai ma tran");
            Console.WriteLine("6. Nhap lai hai ma tran moi");
            Console.WriteLine("7. Thoat");
            Console.Write("Vui long chon chuc nang: ");
            
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1": PerformAddition(a, b); break;
                case "2": PerformMultiplication(a, b); break;
                case "3":
                    PerformTranspose(a, "A");
                    Console.WriteLine();
                    PerformTranspose(b, "B");
                    break;
                case "4":
                    PerformFindMinMax(a, "A");
                    Console.WriteLine();
                    PerformFindMinMax(b, "B");
                    break;
                case "5":
                    a.Display("\n - Ma tran A:");
                    b.Display("\n - Ma tran B:");
                    break;
                case "6":
                    a = InputMatrix("A");
                    b = InputMatrix("B");
                    break;
                case "7":
                    Console.WriteLine("Da thoat chuong trinh.");
                    return;
                default:
                    Console.WriteLine("Lua chon khong hop le. Vui long chon lai.");
                    break;
            }
        }
    }

    static Matrix InputMatrix(string name)
    {
        Console.WriteLine($"\n- Nhap thong tin cho ma tran {name}");
        int rows, cols;

        while (true)
        {
            Console.Write("Nhap so dong va so cot: ");
            string? line = Console.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            
            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2 && int.TryParse(parts[0], out rows) && int.TryParse(parts[1], out cols) && rows > 0 && cols > 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Dau vao khong hop le. Vui long nhap hai so nguyen duong cach nhau bang dau cach.");
            }
        }

        Matrix matrix = new(rows, cols);
        matrix.Input();
        return matrix;
    }

    static void PerformAddition(Matrix a, Matrix b)
    {
        Matrix? result = a + b;
        if (result != null)
        {
            result.Display("\n - Ma tran tong A + B:");
        }
        else
        {
            Console.WriteLine("\nLoi: Hai ma tran phai co cung kich thuoc de thuc hien phep cong.");
        }
    }

    static void PerformMultiplication(Matrix a, Matrix b)
    {
        Matrix? result = a * b;
        if (result != null)
        {
            result.Display("\n - Ma tran tich A x B:");
        }
        else
        {
            Console.WriteLine("\nLoi: So cot cua ma tran A phai bang so dong cua ma tran B.");
        }
    }

    static void PerformTranspose(Matrix matrix, string name)
    {
        Matrix result = matrix.Transpose();
        result.Display($"\n - Ma tran chuyen vi cua {name} ({name}^T):");
    }

    static void PerformFindMinMax(Matrix matrix, string name)
    {
        Console.WriteLine($"\n - Ket qua Min/Max cho ma tran {name}");
        try
        {
            Console.WriteLine($"Phan tu lon nhat la: {matrix.FindMax()}");
            Console.WriteLine($"Phan tu nho nhat la: {matrix.FindMin()}");
        }
        catch(InvalidOperationException ex)
        {
            Console.WriteLine($"Loi: {ex.Message}");
        }
    }
}
