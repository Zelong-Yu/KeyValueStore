using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyValueStore
{
    class Program
    {
        struct KeyValue
        {
            public readonly string key;
            public readonly object value;

            public KeyValue(string key, object value)
            {
                this.key = key;
                this.value = value;
            }
        }

        class MyDictionary
        {
            private int capacity = 4;

            private KeyValue[] kva;
            private int numElements = 0;

            public MyDictionary()
            {
                this.kva = new KeyValue[capacity];
            }

            public object this[string key]
            {
                get
                {
                    foreach (var k in kva)
                    {
                        if (k.key == key) return k.value;
                    }
                    throw new KeyNotFoundException(key);
                }

                set
                {
                    bool hasSeen = false;
                    for (int i = 0; i < numElements; i++)
                    {
                        if (kva[i].key==key)
                        {
                            kva[i] = new KeyValue(key,value);
                            hasSeen = true;
                            break;
                        }
                    }
                    if (!hasSeen)
                        kva[numElements++] = new KeyValue(key, value);

                    //double the capacity when numElements more than 75% of capacity
                    if (numElements>=capacity*0.75)
                    {
                        capacity <<= 1;

                        //create a new array with a temp reference
                        KeyValue[] temp = new KeyValue[capacity];

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
            var d = new MyDictionary();
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
            Console.WriteLine($"{(int)d["Cats"]}, {(int)d["Dogs"]}");
            Console.WriteLine($"Current numbers of elements: {d.Count()}, current capacity: {d.Capacity()}");

            d["Dragon"] = 99;
            d["Grandma"] = 100;
            Console.WriteLine($"{(int)d["Grandma"]}, {(int)d["Dragon"]}");
            Console.WriteLine($"Current numbers of elements: {d.Count()}, current capacity: {d.Capacity()}");
        }
    }
}
