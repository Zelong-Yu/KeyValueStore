using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyValueStore
{
    class Program
    {
        struct KeyValue<TKey,TValue>
        {
            public readonly TKey key;
            public readonly TValue value;

            public KeyValue(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }

        class MyDictionary<TKey, TValue> where TKey : IEquatable<TKey>
        {
            private int capacity = 4;

            private KeyValue<TKey, TValue>[] kva;
            private int numElements = 0;

            public MyDictionary()
            {
                this.kva = new KeyValue<TKey, TValue>[capacity];
            }

            public TValue this[TKey key]
            {
                get
                {
                    foreach (var k in kva)
                    {
                        if (k.key.Equals(key)) return k.value;
                    }
                    throw new KeyNotFoundException(key.ToString());
                }

                set
                {
                    bool hasSeen = false;
                    for (int i = 0; i < numElements; i++)
                    {
                        if (kva[i].key.Equals(key))
                        {
                            kva[i] = new KeyValue<TKey, TValue>(key,value);
                            hasSeen = true;
                            break;
                        }
                    }
                    if (!hasSeen)
                        kva[numElements++] = new KeyValue<TKey, TValue>(key, value);

                    //double the capacity when numElements more than 75% of capacity
                    if (numElements>=capacity)
                    {
                        capacity <<= 1;

                        //create a new array with a temp reference
                        KeyValue<TKey, TValue>[] temp = new KeyValue<TKey, TValue>[capacity];

                        //copy all old values to new
                        kva.CopyTo(temp, 0);

                        //set the kva reference to temp array
                        this.kva = temp;
                    }
                }
            }

            public int Count()
            {
                return numElements;
            }

            public int Capacity()
            {
                return capacity;
            }
        }

        static void Main(string[] args)
        {
            var d = new MyDictionary<string,int>();
            try
            {
                Console.WriteLine(d["Cats"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            d["Cats"] = 42;
            d["Dogs"] = 17;
            Console.WriteLine($"{d["Cats"]}, {d["Dogs"]}");
            Console.WriteLine($"Current numbers of elements: {d.Count()}, current capacity: {d.Capacity()}");

            d["Dragon"] = 99;
            Console.WriteLine($"Current numbers of elements: {d.Count()}, current capacity: {d.Capacity()}");
            d["Grandma"] = 100;
            Console.WriteLine($"{d["Grandma"]}, {d["Dragon"]}");
            Console.WriteLine($"Current numbers of elements: {d.Count()}, current capacity: {d.Capacity()}");
        }
    }
}
