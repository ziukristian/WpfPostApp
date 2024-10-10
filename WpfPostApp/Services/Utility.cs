﻿namespace WpfPostApp.Services;

public class Utility
{
    public static (int, int) CalculateGridSizes(int numberOfItems)
    {
        var result = Convert.ToInt32(Math.Ceiling(Math.Sqrt(numberOfItems)));
        return (result, result);
    }
}
