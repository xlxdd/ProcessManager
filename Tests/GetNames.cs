using System.Collections;
using System.Diagnostics;

namespace Tests;

public static class GetNames
{
    public static void GetCategoryNameList()
    {
        PerformanceCounterCategory[] myCat2;
        myCat2 = PerformanceCounterCategory.GetCategories();
        for (int i = 0; i < myCat2.Length; i++)
        {
            Console.WriteLine(myCat2[i].CategoryName.ToString());
        }
    }
    public static void GetInstanceNameListANDCounterNameList(string CategoryName)
    {
        string[] instanceNames;
        ArrayList counters = new ArrayList();
        PerformanceCounterCategory mycat = new PerformanceCounterCategory(CategoryName);
        try
        {
            instanceNames = mycat.GetInstanceNames();
            if (instanceNames.Length == 0)
            {
                counters.AddRange(mycat.GetCounters());
            }
            else
            {
                for (int i = 0; i < instanceNames.Length; i++)
                {
                    counters.AddRange(mycat.GetCounters(instanceNames[i]));
                }
            }
            for (int i = 0; i < instanceNames.Length; i++)
            {
                Console.WriteLine(instanceNames[i]);
            }
            Console.WriteLine(" ****************************** ");
            foreach (PerformanceCounter counter in counters)
            {
                Console.WriteLine(counter.CounterName);
            }
        }
        catch (Exception)
        {
            Console.WriteLine(" Unable to list the counters for this category ");
        }
    }
    private static void PerformanceCounterFun(string CategoryName, string InstanceName, string CounterName)
    {
        PerformanceCounter pc = new PerformanceCounter(CategoryName, CounterName, InstanceName);
        while (true)
        {
            Thread.Sleep(1000);  //  wait for 1 second 
            float cpuLoad = pc.NextValue();
            Console.WriteLine(" CPU load =  " + cpuLoad + "  %. ");
        }
    }
}
