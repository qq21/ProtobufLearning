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
            #region Test1
            //   
            //    // NetMode m1 = new NetMode(1, "a");

            //    NetModeSub m1 = new NetModeSub(1,"2",3,new Vector3(1,9,9));


            //    m1.vector3 -= new Vector3(1, 9, 9);

            //    Console.WriteLine("序列化前v3:" + m1.vector3);

            //    Transform transform = new Transform(new Vector3(1, 12.1231565232131231246f, 1), new Vector4(0, 0, 0, 1), new Vector3(1, 1, 1), new NetMode() { id = 1111,type=IpType.IPV6 });
            //    m1.transform = transform;
            // //   Console.WriteLine(m1.transform);


            //    //模拟发送数据
            //    Console.WriteLine("发送数据，进行序列化");

            //    Console.WriteLine("序列化后:id：{0} name:{1}", m1.id, m1.name);
            //    //序列化
            //    byte[] data= PbTools.Serialize(m1);

            //    Console.WriteLine("----客服端收到数据-----开始反序列化");
            //    NetMode m2= PbTools.PBDSerialize<NetMode>(data);
            //  // Console.WriteLine("反序列化后:id：{0} name:{1}", m2.id, m2.name);
            //  //  Console.WriteLine(m2.transform);
            ////    Console.WriteLine(m2.transform.netMode.type);
            //    Console.WriteLine(m2.vector3);
            //    //   Console.WriteLine(m2.vector3[1]);
            //    
            #endregion

            #region test2
            Car tools = new XCar("blue", 10);
            tools.Drive();

            byte[] data= PbTools.Serialize(tools);
            //反序列化子类的时候，反序列化的对象一定得是 原来的对象，不能是父类也不能是子类
            PbTools.PBDSerialize<Car>(data).Drive();

            #endregion
        }
    }
    
    [ProtoContract]
    public struct Transform
    {
        [ProtoMember(1)]
        public Vector3 Position;
        [ProtoMember(2)]
        public Vector4 Rotation;
        [ProtoMember(3)]
        public Vector3 Scale;

        //Test:
        [ProtoMember(5)]
        public NetMode netMode;
 
        public Transform(Vector3 pos,Vector4 rot,Vector3 sca,NetMode netMode)
        {
            this.Position = pos;
            this.Rotation = rot;
            this.Scale = sca;
            this.netMode = netMode;

        }

        public override string ToString()
        {
            return string.Format("Position:{0},Rotation:{1},Scale:{2},id:{3}", Position, Rotation, Scale,netMode.id);
        }
    }
  
    [ProtoInclude(1,typeof(Car))]
    [ProtoContract]
    interface Vehicle
    {
        void Drive();
        string name { get; }
        float speed { get; }

    }
    [ProtoInclude(1, typeof(XCar))]
    [ProtoContract]
    public class Car:Vehicle 
    {
        [ProtoMember(2)]
        public string _name;
         
        [ProtoMember(3)]
        public float _speed;

 

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Car(string name, float speed)
        {
            _name = name;
            _speed = speed;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Car()
        {
        }

        #region Implementation of Vehicle

        public virtual void Drive()
        {
            Console.WriteLine("{0}的速度是{1}", _name, _speed);
        }

        public string name => _name;

        public float speed => _speed;

        #endregion
    }

    [ProtoContract]
    public class XCar : Car
    {
        public XCar()
        {
        }
        
        public XCar(string name, float speed) 
        {
            this._name = name;
            this._speed = speed;
        }
     
    }

    [ProtoContract]
    public struct Vector3
    {
        /// <summary>
        /// 结构体中  需要序列化的数组 都需要 加上 ProtoMember 特性， 否则 序列化后再 反序列化数值 会 丢失，变成默认值; 
        /// </summary>     
        [ProtoMember(1)]
        public  float x;
        [ProtoMember(2)]
        public float y; 
        [ProtoMember(3)]
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

         
        public bool IsZero()
        {
            return x ==0&& y==0 && z==0;
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", x, y, z);
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
        }

        //public static Vector3 operator -(Vector3 a, Vector3 b)
        //{
        //    return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        //}
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }


    }

    [ProtoContract]
    public struct Vector4
    {
        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;
        [ProtoMember(4)]
        public float w;
        

        public Vector4(float x, float y, float z,float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public override string ToString()
        {
            return string.Format("({0},{1},{2},{3})", x, y, z,w);
        }
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    case 3:
                        return this.w;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector4 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    case 3:
                        this.w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector4 index!");
                }
            }
        }
    }

    /// <summary>
    /// 坑1  一个类 上的特性上的  编号 都不能重复  0  不是int  0 会报错
    /// </summary>
    [ProtoInclude(3,typeof(NetModeSub))]
    [ProtoContract]
    public class NetMode
    {
       
        [ProtoMember(1)]
        public int id;
        [ProtoMember(2)]
        public string name;


        [ProtoMember(4)] public Vector3 vector3;  //坑1 当互相减 为0的时候， 0就 代表，反序列化后 是默认值

        [ProtoMember(5)] public Transform transform ;


        [ProtoMember(6)]
        public IpType type;

        public NetMode()
        {
        }
         
        public NetMode(int id,string name,Vector3 v3)
        {
            this.id = id;
            this.name = name;
            this.vector3 = v3;
        }

    }

   
    public enum IpType
    {
        [ProtoEnum]
        IPV4 =101,
        [ProtoEnum]
        IPV6 =102
    }

    [ProtoContract]
    class NetModeSub:NetMode
    {
        [ProtoMember(2)] public float range;
        /// <summary>
        ///   子类的构造函数也一定要有，不然也会报错
        /// </summary>
        public NetModeSub()
        {
            range = 12;
        }

        
        public NetModeSub(float range)
        {
            this.range = range;
        }

        public NetModeSub(int id, string name, float range,Vector3 v3) : base(id, name,v3)
        {
            this.range = range;
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
                  //  RuntimeTypeModel.Default.Serialize(ms, value);
                  
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
