using System;
using System.Collections.Generic;
using System.Text;

public class GUIDGeneratorWithNumber
{
    private long mGenNum;

    public GUIDGeneratorWithNumber()
    {
        this.mGenNum = 0;
    }

    public GUIDGeneratorWithNumber(long aStartNum)
    {
        this.mGenNum = aStartNum;
    }

    public long GetGUID()
    {
        return this.mGenNum++;
    }

    public void Clear()
    {
        this.mGenNum = 0;
    }
}

public class CSingleton<T> where T : new()
{
    private static T m_instance;
    public static T GetSingleton()
    {
        if (m_instance == null)
        {
            m_instance = new T();
        }
        return m_instance;
    }
}


public static class UtilityTool
{
}
