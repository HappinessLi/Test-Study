using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TestTuple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MyComparer<string, float, int> myComparer = new MyComparer<string, float, int>();
        var t1 = new Tuple<string, float, bool>("aba", 1f, false);
        var t2 = new Tuple<string, float, bool>("Aba", 1f, false);

        myComparer.Add(new Tuple<string, float, int>("dba", 2f, 3));
        myComparer.Add(new Tuple<string, float, int>("cba", 3f, 3));
        myComparer.Add(new Tuple<string, float, int>("aba", 1f, 3));
        myComparer.Add(new Tuple<string, float, int>("Bba", 1f, 3));
        myComparer.Add(new Tuple<string, float, int>("Bba", 1f, 4));
        myComparer.Add(new Tuple<string, float, int>("cba", 1f, 3));
        myComparer.Add(new Tuple<string, float, int>("Dba", 1f, 3));
        myComparer.Add(new Tuple<string, float, int>("dba", 1f, 3));
  
  
      
        //UnityEngine.Debug.LogError($"lxf iscomparer ：{myComparer.Compare(t1,t2)} : isEqual : {t1.Equals(t2)}");
        StringBuilder sb = new StringBuilder();
        foreach(var t in myComparer.TestTupleList)
        {
            sb.Append(t.ToString());
            sb.Append('\n');
        }
        UnityEngine.Debug.LogError($"lxf before sort:{sb}");

        myComparer.SortSelf();

        sb.Clear();
        foreach (var t in myComparer.TestTupleList)
        {
            sb.Append(t.ToString());
            sb.Append('\n');
        }
        UnityEngine.Debug.LogError($"lxf before sort:{sb}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/// <summary>
/// 三参元组比较器
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class MyComparer<T1, T2, T3> : IComparer<Tuple<T1, T2, T3>>
    where T1 : class, IEquatable<T1>, IComparable<T1>
    where T2 : struct, IEquatable<T2>, IComparable<T2>
    where T3 : struct, IEquatable<T3>, IComparable<T3>
{
    public int Compare(object x, object y)
    {
        Tuple<T1, T2, T3> firstTuple = (Tuple<T1, T2, T3>)x;
        Tuple<T1, T2, T3> secondTuple = (Tuple<T1, T2, T3>)y;

        // 首先按照T1的大小比较
        int result = Comparer<T1>.Default.Compare(firstTuple.Item1, secondTuple.Item1);

        if (result == 0) // T1相等才比较T2
        {
            result = Comparer<T2>.Default.Compare(firstTuple.Item2, secondTuple.Item2);

            if (result == 0) // T2相等才比较T3
            {
                result = Comparer<T3>.Default.Compare(firstTuple.Item3, secondTuple.Item3);
            }
        }

        return result;
    }

    List<Tuple<T1, T2, T3>> m_testTupleList;
    public List<Tuple<T1, T2, T3>> TestTupleList => m_testTupleList;
    public MyComparer()
    {
        try
        {
            if (typeof(T1) != typeof(string))
                throw new MyComparerKeyInvaildException("第一个泛型应该为string类型！");
        }
        catch(MyComparerKeyInvaildException e)
        {
            UnityEngine.Debug.LogError($"lxf {e}");
        }
       
        m_testTupleList = new List<Tuple<T1, T2, T3>>();
    }

    public void Add(Tuple<T1, T2, T3> tup)
    {
        m_testTupleList.Add(tup);
    }

    public void SortSelf()
    {
        m_testTupleList.Sort((x,y) => Compare(y,x));
    }

    public int Compare(Tuple<T1, T2, T3> x, Tuple<T1, T2, T3> y)
    {
        int result = 0;
        string item1XString = x.Item1.ToString();
        string item1YString = y.Item1.ToString();

        char firstX = item1XString.First();
        char firstY = item1YString.First();

        char lowerFirstX = char.ToLower(firstX);
        char lowerFirstY = char.ToLower(firstY);
        result = lowerFirstX.CompareTo(lowerFirstY);
        if (result != 0) return result;
        result = firstX.CompareTo(firstY);
        if (result != 0) return result;
        result = Comparer<int>.Default.Compare(item1XString.Length, item1YString.Length);
        if (result != 0) return result;
        result = Comparer<T2>.Default.Compare(x.Item2, y.Item2);
        if (result != 0) return result;
        result = Comparer<T3>.Default.Compare(x.Item3, y.Item3);
        return result;
    }
}

public class MyComparerKeyInvaildException : Exception
{
    public MyComparerKeyInvaildException() : base() { }
    public MyComparerKeyInvaildException(string message) : base(message) { }
    public MyComparerKeyInvaildException(string message, Exception innerException) : base(message, innerException) { }

    //添加任何其他构造函数或成员方法，以符合需求

    public override string ToString()
    {
        //重写基类的ToString()方法，可添加其他信息
        return "MyComparerKeyInvaildException: " + Message;
    }
}
