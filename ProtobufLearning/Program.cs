using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf.ServiceModel;
using ProtoBuf;
using ProtoBuf.Meta;

namespace ProtobufLearning
{
    class Program
    {
        static void Main(string[] args)
        {

            NetMode m1 = new NetMode(1, "a");
            Console.WriteLine("发送数据，进行序列化");
            Console.WriteLine("序列化后:id：{0} name:{1}", m1.id, m1.name);

            //模拟发送数据
            byte[] data= PbTools.Serialize(m1);
            Console.WriteLine("----客服端收到数据-----开始反序列化");
            NetMode m2= PbTools.PBDSerialize<NetMode>(data);
            Console.WriteLine("反序列化后:id：{0} name:{1}", m2.id, m2.name);

        }
    }

    [ProtoContract]
    class NetMode
    {

        [ProtoMember(1)]
        public int id;
        [ProtoMember(2)]
        public string name;

        /// <summary>
        /// 使用构造函数时 ，一定要加上默认的构造函数，不然会报错
        /// </summary>
        public NetMode()
        {
        }
         
        public NetMode(int id,string name)
        {
            this.id = id;
            this.name = name;
        }
    }

    public class PbTools
    {
        /// <summary>
        /// 通过Protobuffer 序列化对象 返回byte[]数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns>二进制数据</returns>
        public static byte[] PBSerialize(object value)
        {
            byte[] data = null;
            //在范围结束时 处理对象
            using (MemoryStream ms=new MemoryStream())
            {
                if (value!=null)
                {
                    // 序列化 到内存里 
                    Serializer.Serialize(ms, value); //此方法 会调用到以下方法. 实际上是同个方法，多层封装
                    RuntimeTypeModel.Default.Serialize(ms, value);
                  
                }
                //设置当前 位置
                ms.Position = 0;
                //写入 数组
                long length = ms.Length;
                data = new byte[length];
                //从内存流中读取  写入到buffer中
                ms.Read(data, 0, (int)length);
                
            }
            return data;
        }
        public static byte[] Serialize(object obj)
        {
            using (var memory = new MemoryStream())
            {
                Serializer.Serialize(memory, obj);  
                return memory.ToArray();
            }
        }

        public static object PBDSerialize( byte[] value ,Type type)
        {
            object o = null;
            using (var memory=new MemoryStream(value))
            {
                o=   RuntimeTypeModel.Default.Deserialize(memory,null, type);
            }
            return o;
        }

        public static T PBDSerialize<T>(byte[] value)
        {
            using (MemoryStream memory = new MemoryStream(value))
                return Serializer.Deserialize<T>(memory);
        }

        public static T PBDSerialize<T>(byte[] value, int offest, int count)
        {
            using (MemoryStream memory = new MemoryStream(value, offest, count))
                return Serializer.Deserialize<T>(memory);
        }
    }
}
