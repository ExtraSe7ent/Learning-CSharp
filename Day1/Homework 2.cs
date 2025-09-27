using System;
using System.Linq;

public class ArrayProcessor
{
    private int[] dataArray;
    private int[] originalArray;

    public int Length
    {
        get { return dataArray.Length; }
    }

    public void Input()
    {
        Console.Write("\n - Nhap so luong phan tu cua mang: ");
        int n = int.Parse(Console.ReadLine());
        
        dataArray = new int[n];
        originalArray = new int[n];

        for (int i = 0; i < n; i++)
        {
            Console.Write($"Nhap phan tu thu {i + 1}: ");
            int value = int.Parse(Console.ReadLine());
            dataArray[i] = value;
            originalArray[i] = value;
        }
    }

    public void Display(string message)
    {
        Console.WriteLine($"{message}: [" + string.Join(", ", dataArray) + "]");
    }
    
    public void ResetArray()
    {
        originalArray.CopyTo(dataArray, 0);
    }

    public void BubbleSort()
    {
        int n = dataArray.Length;
        bool swapped;
        for (int i = 0; i < n - 1; i++)
        {
            swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (dataArray[j] > dataArray[j + 1])
                {
                    int temp = dataArray[j];
                    dataArray[j] = dataArray[j + 1];
                    dataArray[j + 1] = temp;
                    swapped = true;
                }
            }
            if (!swapped) break;
        }
    }

    public void QuickSort(int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(left, right);
            QuickSort(left, pivotIndex - 1);
            QuickSort(pivotIndex + 1, right);
        }
    }

    private int Partition(int left, int right)
    {
        int pivot = dataArray[right];
        int i = (left - 1);

        for (int j = left; j < right; j++)
        {
            if (dataArray[j] <= pivot)
            {
                i++;
                int temp = dataArray[i];
                dataArray[i] = dataArray[j];
                dataArray[j] = temp;
            }
        }

        int tempPivot = dataArray[i + 1];
        dataArray[i + 1] = dataArray[right];
        dataArray[right] = tempPivot;

        return i + 1;
    }

    public int LinearSearch(int key)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            if (dataArray[i] == key)
            {
                return i;
            }
        }
        return -1;
    }

    public int BinarySearch(int key)
    {
        int left = 0;
        int right = dataArray.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (dataArray[mid] == key)
                return mid;

            if (dataArray[mid] < key)
                left = mid + 1;
            else
                right = mid - 1;
        }
        return -1;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ArrayProcessor processor = new ArrayProcessor();

        processor.Input();
        Console.WriteLine();

        processor.Display("Mang ban dau");

        processor.BubbleSort();
        processor.Display("Mang sau khi sap xep Bubble Sort");

        processor.ResetArray();

        processor.QuickSort(0, processor.Length - 1);
        processor.Display("Mang sau khi sap xep Quick Sort");

        Console.Write("\n - Nhap so can tim: ");
        int key = int.Parse(Console.ReadLine());

        int linearIndex = processor.LinearSearch(key);
        if (linearIndex != -1)
        {
            Console.WriteLine($"Tim kiem tuyen tinh: Tim thay '{key}' tai vi tri {linearIndex}.");
        }
        else
        {
            Console.WriteLine($"Tim kiem tuyen tinh: Khong tim thay '{key}'.");
        }

        int binaryIndex = processor.BinarySearch(key);
        if (binaryIndex != -1)
        {
            Console.WriteLine($"Tim kiem nhi phan: Tim thay '{key}' tai vi tri {binaryIndex}.");
        }
        else
        {
            Console.WriteLine($"Tim kiem nhi phan: Khong tim thay '{key}'.");
        }
    }
}
