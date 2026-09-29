int[] number=[5,57,72,61,6,31];
int[] salary=[50_000, 45_000, 25_000, 70_000, 60_000, 80_000];

System.Console.WriteLine($"min maaş: {GetMin(salary)}");
System.Console.WriteLine($"min value: {GetMin(number)}");

int GetMin(int[] array)
{
    int x = array[0];
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i]<x)
        {
            x = array[i];
        }
    }
    return x;
}
