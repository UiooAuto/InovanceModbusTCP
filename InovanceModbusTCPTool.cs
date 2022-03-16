using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InovanceModbusTCP
{
    public class InovanceModbusTCPTool
    {
        private Socket socket;//用于通信的套接字
        private IPAddress ipAddress;//服务器的IP地址
        private int port;//服务器的端口
        private byte slaveNum;

        private IPEndPoint ipEndPoint;//服务器的通信节点
        private Thread readThread;//接受响应线程
        private Ping ping;//测试网络连接状态
        private int overTime = 1000;//ping的超时时间
        private int recLength;//接收到的数据长度
        private int recSessionNum;//接收到的会话号
        private byte[] recData;//接收到的数据包

        public static UInt16 ComSessionNum = 0;//生成用公共会话编号

        #region 构造方法及GetSet

        public IPAddress IpAddress
        {
            get => ipAddress;
            set => ipAddress = value;
        }
        public int Port
        {
            get => port;
            set => port = value;
        }
        public int OverTime
        {
            get => overTime;
            set => overTime = value;
        }
        public byte SlaveNum
        {
            get => slaveNum;
            set => slaveNum = value;
        }

        /// <summary>
        /// 创建一个汇川ModbusTCP通信对象
        /// </summary>
        /// <param name="iPAddress">服务器IP</param>
        /// <param name="port">服务器端口</param>
        public InovanceModbusTCPTool(string iPAddress, int port, byte slaveNum)
        {
            this.ipAddress = IPAddress.Parse(iPAddress);
            this.port = port;
            this.ipEndPoint = new IPEndPoint(ipAddress, port);
            this.ping = new Ping();
            this.slaveNum = slaveNum;
        }

        /*public InovanceModbusTCPTool()
        {
        }*/

        #endregion

        #region 测试连接、开启连接、关闭连接

        /// <summary>
        /// 测试与服务器的连接状态
        /// </summary>
        /// <returns>是否可以正常连接</returns>
        public bool Ping()
        {
            PingReply pingReply = ping.Send(this.ipAddress, this.overTime);
            if (pingReply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        public bool Connect()
        {
            try
            {
                if (Ping())
                {
                    this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.socket.SendTimeout = overTime;
                    this.socket.ReceiveTimeout = overTime;
                    this.socket.Connect(this.ipEndPoint);
                }
                else
                {
                    //MessageBox.Show("连接超时");
                    return false;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                return false;
            }
            readThread = new Thread(ReceiveFrom);
            readThread.IsBackground = true;
            readThread.Start();
            return true;
        }

        public void CloseConnect()
        {
            if (socket != null)
            {
                readThread.Abort();
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                try
                {
                    socket.Close();
                }
                catch
                {
                }
            }
            try
            {
                ((IDisposable)this).Dispose();
                ping.Dispose();
            }
            catch
            {
            }
        }

        #endregion

        public static UInt16 GetSessionNum()
        {
            return ComSessionNum++;
        }

        private bool SendTo(RequestCmd cmd)
        {
            bool success = false;
            int v;
            try
            {
                byte[] vs = cmd.GetBytes();
                v = socket.Send(vs);
                //v = socket.Send(cmd.GetBytes());
            }
            catch
            {
                return false;
            }
            if (v > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ReceiveFrom()
        {
            while (true)
            {
                byte[] buffer = new byte[4096];
                int v = 0;
                try
                {
                    v = socket.Receive(buffer);
                }
                catch (Exception)
                {
                }
                if (v != 0)
                {
                    recData = new byte[v];
                    Array.ConstrainedCopy(buffer, 0, recData, 0, v);
                }
                else
                {
                    recData = null;
                }
            }
        }

        private void CheckRespones(Object o)
        {
            CheckRes checkRes = (CheckRes)o;
            while (true)
            {
                if (recData != null && checkRes != null)
                {
                    Respones respones = new Respones(recData);
                    if (checkRes.ReuqestSessionNum == respones.recSessionNum)
                    {
                        checkRes.Respones = respones;
                        checkRes.FindSuccess = true;
                        return;
                    }
                }
            }
        }
        #region 读取数据

        #region 读取bool量
        /*
        /// <summary>
        /// 读取单个bool量
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">读取的地址</param>
        /// <returns></returns>
        public ReadResult<bool> ReadBoolean(byte slaveNum, string startAddress)
        {
            ReadResult<bool> readResult = new ReadResult<bool>();//预备结果
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("Q") || startAddress.Contains("q"))
            {
                cmdCode = CmdCode.ReadBooleanQ;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SM") || startAddress.Contains("sm"))
            {
                cmdCode = CmdCode.ReadBooleanSM;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return readResult;
            }

            RequestCmd cmd = new RequestCMDRead(slaveNum, cmdCode, address, 1);//创建请求报文
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            checkThread.Abort();

            readResult.success = checkRes.FindSuccess;
            if (!checkRes.FindSuccess)
            {
                return readResult;
            }

            readResult.data = ByteToBool(checkRes.Respones.data[9]);
            return readResult;
        }

        /// <summary>
        /// 读取连续的多个bool量
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="reqNum">读取长度</param>
        /// <returns></returns>
        public ReadResult<bool[]> ReadBoolean(byte slaveNum, string startAddress, ushort reqNum)
        {
            ReadResult<bool[]> readResult = new ReadResult<bool[]>();//预备结果
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("Q") || startAddress.Contains("q"))
            {
                cmdCode = CmdCode.ReadBooleanQ;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SM") || startAddress.Contains("sm"))
            {
                cmdCode = CmdCode.ReadBooleanSM;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return readResult;
            }
            RequestCmd cmd = new RequestCMDRead(slaveNum, cmdCode, address, reqNum);//创建请求报文
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            checkThread.Abort();

            readResult.success = checkRes.FindSuccess;//结果查找成功
            if (!checkRes.FindSuccess)//如果查找不成功则直接返回失败结果
            {
                return readResult;
            }
            byte[] resultByte = new byte[checkRes.Respones.data[8]];//如果查找成功，则新建结果数组
            Array.ConstrainedCopy(checkRes.Respones.data, 9, resultByte, 0, checkRes.Respones.data[8]);//从结果报文中获取结果内容
            readResult.data = ByteToBool(resultByte, reqNum);//将byte数组的结果转换为bool数组
            return readResult;
        }*/

        #endregion

        #region 读取UInt16量

        /// <summary>
        /// 读取一个字
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">读取的地址</param>
        /// <returns>读取到的字</returns>
        public ReadResult<UInt16> ReadU16(string startAddress)
        {
            ReadResult<UInt16> readResult = new ReadResult<UInt16>();//预备结果
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.ReadWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.ReadWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return readResult;
            }

            RequestCmd cmd = new RequestCMDRead(slaveNum, cmdCode, address, 1);//创建请求报文
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            checkThread.Abort();

            readResult.isSuccess = checkRes.FindSuccess;
            if (!checkRes.FindSuccess)
            {
                return readResult;
            }
            readResult.result = checkRes.Respones.data[9];
            readResult.result = (UInt16)(readResult.result << 8);
            readResult.result = (UInt16)(readResult.result | checkRes.Respones.data[10]);

            return readResult;
        }

        /// <summary>
        /// 读取多个字
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="reqNum">读取长度</param>
        /// <returns>读取到的字数组</returns>
        public ReadResult<UInt16[]> ReadU16(string startAddress, ushort reqNum)
        {
            ReadResult<UInt16[]> readResult = new ReadResult<UInt16[]>();//预备结果
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.ReadWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.ReadWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return readResult;
            }

            RequestCmd cmd = new RequestCMDRead(slaveNum, cmdCode, address, reqNum);//创建请求报文
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            checkThread.Abort();

            readResult.isSuccess = checkRes.FindSuccess;//结果查找成功
            if (!checkRes.FindSuccess)//如果查找不成功则直接返回失败结果
            {
                return readResult;
            }
            byte[] resultByte = new byte[checkRes.Respones.data[8]];//如果查找成功，则新建结果数组
            Array.ConstrainedCopy(checkRes.Respones.data, 9, resultByte, 0, checkRes.Respones.data[8]);//从结果报文中获取结果内容
            readResult.result = BytesToUInt16(resultByte);

            return readResult;
        }

        #endregion

        #region 读取int16量

        /// <summary>
        /// 读取一个字
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">读取的地址</param>
        /// <returns>读取到的字</returns>
        public ReadResult<Int16> Read16(string startAddress)
        {
            ReadResult<Int16> readResult = new ReadResult<Int16>();//预备结果
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.ReadWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.ReadWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return readResult;
            }

            RequestCmd cmd = new RequestCMDRead(slaveNum, cmdCode, address, 1);//创建请求报文
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            checkThread.Abort();

            readResult.isSuccess = checkRes.FindSuccess;
            if (!checkRes.FindSuccess)
            {
                return readResult;
            }
            UInt16 tempValue = 0;
            tempValue = checkRes.Respones.data[9];
            tempValue = (UInt16)(tempValue << 8);
            tempValue = (UInt16)(tempValue | (UInt16)checkRes.Respones.data[10]);

            readResult.result = (Int16)tempValue;

            return readResult;
        }

        /// <summary>
        /// 读取多个字
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="reqNum">读取长度</param>
        /// <returns>读取到的字数组</returns>
        public ReadResult<Int16[]> Read16(string startAddress, ushort reqNum)
        {
            ReadResult<Int16[]> readResult = new ReadResult<Int16[]>();//预备结果
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.ReadWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.ReadWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return readResult;
            }

            RequestCmd cmd = new RequestCMDRead(slaveNum, cmdCode, address, reqNum);//创建请求报文
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            checkThread.Abort();

            readResult.isSuccess = checkRes.FindSuccess;//结果查找成功
            if (!checkRes.FindSuccess)//如果查找不成功则直接返回失败结果
            {
                return readResult;
            }
            byte[] resultByte = new byte[checkRes.Respones.data[8]];//如果查找成功，则新建结果数组
            Array.ConstrainedCopy(checkRes.Respones.data, 9, resultByte, 0, checkRes.Respones.data[8]);//从结果报文中获取结果内容
            UInt16[] tempArr = BytesToUInt16(resultByte);
            readResult.result = new Int16[tempArr.Length];

            for (int i = 0; i < tempArr.Length; i++)
            {
                readResult.result[i] = (Int16)tempArr[i];
            }

            return readResult;
        }

        #endregion

        #region 读取Uint32量

        /// <summary>
        /// 读取一个双字
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">读取的地址</param>
        /// <returns>读取到的字</returns>
        public ReadResult<uint> ReadU32(string startAddress)
        {
            ReadResult<uint> readResult = new ReadResult<uint>();//预备结果

            ReadResult<ushort[]> readResultUint16 = ReadU16(startAddress, 2);

            readResult.result = readResultUint16.result[1];
            readResult.result = readResult.result << 16;
            readResult.result = readResult.result | readResultUint16.result[0];

            return readResult;
        }

        /// <summary>
        /// 读取多个双字
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="reqNum">读取长度</param>
        /// <returns>读取到的字数组</returns>
        public ReadResult<uint[]> ReadU32(string startAddress, ushort reqNum)
        {
            ReadResult<uint[]> readResult = new ReadResult<uint[]>();//预备结果
            readResult.result = new uint[reqNum];

            ReadResult<ushort[]> readResultUint16 = ReadU16(startAddress, (UInt16)(2 * reqNum));

            for (int i = 0; i < reqNum; i++)
            {
                readResult.result[i] = readResultUint16.result[(2 * i) + 1];
                readResult.result[i] = readResult.result[i] << 16;
                readResult.result[i] = readResult.result[i] | readResultUint16.result[2 * i];
            }

            return readResult;
        }

        #endregion

        #region 读取int32量

        /// <summary>
        /// 读取一个双字
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">读取的地址</param>
        /// <returns>读取到的字</returns>
        public ReadResult<int> Read32(string startAddress)
        {
            ReadResult<int> readResult = new ReadResult<int>();//预备结果
            int result = 0;

            ReadResult<ushort[]> readResultUint16 = ReadU16(startAddress, 2);

            result = readResultUint16.result[1];
            result = result << 16;
            result = result | readResultUint16.result[0];
            readResult.result = result;

            return readResult;
        }

        /// <summary>
        /// 读取多个双字
        /// </summary>
        /// <param name="slaveNum">从站号</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="reqNum">读取长度</param>
        /// <returns>读取到的字数组</returns>
        public ReadResult<int[]> Read32(string startAddress, ushort reqNum)
        {
            ReadResult<int[]> readResult = new ReadResult<int[]>();//预备结果
            readResult.result = new int[reqNum];

            ReadResult<ushort[]> readResultUint16 = ReadU16(startAddress, (UInt16)(2 * reqNum));

            for (int i = 0; i < reqNum; i++)
            {
                uint tempVar;
                tempVar = readResultUint16.result[(2 * i) + 1];
                tempVar = tempVar << 16;
                tempVar = tempVar | readResultUint16.result[2 * i];
                readResult.result[i] = (int)tempVar;
            }

            return readResult;
        }

        #endregion

        #endregion

        #region 写入数据

        #region 单个写入

        /*public bool Write(byte slaveNum, string startAddress, bool value)
        {
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("Q")|| startAddress.Contains("q"))
            {
                cmdCode = CmdCode.WriteSingleBooleanQ;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SM") || startAddress.Contains("sm"))
            {
                cmdCode = CmdCode.WriteSingleBooleanSM;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return false;
            }
            UInt16 i = (UInt16)(value ? 0xff00 : 0);
            RequestCmd cmd = new RequestCMDWriteSingle(slaveNum, cmdCode, address, i);
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            if (!checkRes.FindSuccess)
            {
                return false;
            }
            return ByteArrayEquals(cmd.GetBytes(), checkRes.Respones.data);
        }*/

        public bool Write(string startAddress, UInt16 value)
        {
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.WriteSingleWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.WriteSingleWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return false;
            }
            RequestCmd cmd = new RequestCMDWriteSingle(slaveNum, cmdCode, address, value);
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            if (!checkRes.FindSuccess)
            {
                return false;
            }
            return ByteArrayEquals(cmd.GetBytes(), checkRes.Respones.data);
        }

        public bool Write(string startAddress, Int16 value)
        {
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.WriteSingleWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.WriteSingleWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return false;
            }
            RequestCmd cmd = new RequestCMDWriteSingle(slaveNum, cmdCode, address, (UInt16)value);
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            if (!checkRes.FindSuccess)
            {
                return false;
            }
            return ByteArrayEquals(cmd.GetBytes(), checkRes.Respones.data);
        }

        public bool Write(string startAddress, uint value)
        {
            UInt16[] tempArr = new UInt16[2];
            tempArr[0] = (UInt16)(value & 0x0000ffff);
            tempArr[1] = (UInt16)(value >> 16);
            bool v = Write(startAddress, tempArr);

            return v;
        }

        public bool Write(string startAddress, int value)
        {
            UInt16[] tempArr = new UInt16[2];
            uint valueU = (uint)value;
            tempArr[0] = (UInt16)(valueU & 0x0000ffff);
            tempArr[1] = (UInt16)(valueU >> 16);
            bool v = Write(startAddress, tempArr);

            return v;
        }

        #endregion

        #region 多个写入

        /*public bool Write(byte slaveNum, string startAddress, bool[] value)
        {
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("Q") || startAddress.Contains("q"))
            {
                cmdCode = CmdCode.WriteMoreBooleanQ;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SM") || startAddress.Contains("sm"))
            {
                cmdCode = CmdCode.WriteMoreBooleanSM;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return false;
            }
            byte[] vs = BoolsToBytes(value);
            RequestCmd cmd = new RequestCMDWriteMore(slaveNum, cmdCode, address, (UInt16)value.Length, vs);
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            if (!checkRes.FindSuccess)
            {
                return false;
            }

            byte[] vs1 = MakeTargetRespones(cmd.GetBytes());
            vs1[4] = 0;
            vs1[5] = 6;

            return ByteArrayEquals(vs1, checkRes.Respones.data);
        }*/

        public bool Write(string startAddress, UInt16[] value)
        {
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.WriteMoreWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.WriteMoreWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return false;
            }
            byte[] vs = Uint16ToBytes(value);
            RequestCmd cmd = new RequestCMDWriteMore(slaveNum, cmdCode, address, (UInt16)value.Length, vs);
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            if (!checkRes.FindSuccess)
            {
                return false;
            }

            byte[] vs1 = MakeTargetRespones(cmd.GetBytes());
            vs1[4] = 0;
            vs1[5] = 6;

            return ByteArrayEquals(vs1, checkRes.Respones.data);
        }

        public bool Write(string startAddress, Int16[] value)
        {
            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.WriteMoreWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.WriteMoreWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return false;
            }
            byte[] vs = Int16ToBytes(value);
            RequestCmd cmd = new RequestCMDWriteMore(slaveNum, cmdCode, address, (UInt16)value.Length, vs);
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            if (!checkRes.FindSuccess)
            {
                return false;
            }

            byte[] vs1 = MakeTargetRespones(cmd.GetBytes());
            vs1[4] = 0;
            vs1[5] = 6;

            return ByteArrayEquals(vs1, checkRes.Respones.data);
        }

        public bool Write(string startAddress, uint[] value)
        {
            UInt16[] tempArr = new ushort[value.Length * 2];

            for (int i = 0; i < value.Length; i++)
            {
                tempArr[i * 2] = (UInt16)(value[i] & 0x0000ffff);
                tempArr[(i * 2) + 1] = (UInt16)(value[i] >> 16);
            }

            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.WriteMoreWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.WriteMoreWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return false;
            }
            byte[] vs = Uint16ToBytes(tempArr);
            RequestCmd cmd = new RequestCMDWriteMore(slaveNum, cmdCode, address, (UInt16)value.Length, vs);
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            if (!checkRes.FindSuccess)
            {
                return false;
            }

            byte[] vs1 = MakeTargetRespones(cmd.GetBytes());
            vs1[4] = 0;
            vs1[5] = 6;

            return ByteArrayEquals(vs1, checkRes.Respones.data);
        }

        public bool Write(string startAddress, int[] value)
        {
            UInt16[] tempArr = new UInt16[value.Length * 2];

            for (int i = 0; i < value.Length; i++)
            {
                uint tempVar = (uint)value[i];
                tempArr[i * 2] = (UInt16)(tempVar & 0x0000ffff);
                tempArr[(i * 2) + 1] = (UInt16)(tempVar >> 16);
            }

            CmdCode cmdCode;
            ushort address = 0;
            if (startAddress.Contains("M") || startAddress.Contains("m"))
            {
                cmdCode = CmdCode.WriteMoreWordM;
                address = ushort.Parse(startAddress.Substring(1));
            }
            else if (startAddress.Contains("SD") || startAddress.Contains("sd"))
            {
                cmdCode = CmdCode.WriteMoreWordSD;
                address = ushort.Parse(startAddress.Substring(2));
            }
            else
            {
                return false;
            }
            byte[] vs = Uint16ToBytes(tempArr);
            RequestCmd cmd = new RequestCMDWriteMore(slaveNum, cmdCode, address, (UInt16)value.Length, vs);
            CheckRes checkRes = new CheckRes(cmd.sessionNum);//创建检查响应目标对象
            bool v = SendTo(cmd);//发送请求
            Thread checkThread = new Thread(CheckRespones);//开启检查响应线程
            checkThread.IsBackground = true;
            checkThread.Start(checkRes);//启动线程，参数为具有本会话号的对象
            checkThread.Join(overTime);//利用检查线程阻塞本线程，超时时间由程序指定
            if (!checkRes.FindSuccess)
            {
                return false;
            }

            byte[] vs1 = MakeTargetRespones(cmd.GetBytes());
            vs1[4] = 0;
            vs1[5] = 6;

            return ByteArrayEquals(vs1, checkRes.Respones.data);
        }

        #endregion

        #endregion

        public bool ByteArrayEquals(byte[] b1, byte[] b2)
        {
            if (b1.Length == b2.Length)
            {
                for (int i = 0; i < b1.Length; i++)
                {
                    if (b1[i] != b2[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool ByteToBool(byte value)
        {
            return value == 0 ? false : true;
        }

        /// <summary>
        /// 将byte数组中的bool按规律和数量要求提取为Bool数组
        /// </summary>
        /// <param name="bytes">要转换的Byte数组</param>
        /// <param name="num">要转换多少Bool量</param>
        /// <returns>结果Bool数组</returns>
        public bool[] ByteToBool(byte[] bytes, int num)
        {
            bool[] bools = new bool[num];

            for (int i = 0; i < bytes.Length; i++)//遍历所有数组
            {
                if (i != num / 8)//计算整字节数量，比较是否是最后一个字节
                {
                    byte tool = 0x01;//用于位与的工具数字
                    for (int j = 0; j < 8; j++)//遍历本byte中的每一位
                    {
                        bools[j + (i * 8)] = ByteToBool((byte)(bytes[i] & tool));//工具数与要检查的byte位与后仅保留1位，转化为bool值后赋给结果
                        tool = (byte)(tool << 1);
                    }
                }
                else
                {
                    byte tool = 0x01;//用于位与的工具数字
                    for (int j = 0; j < num % 8; j++)//遍历本byte中的每一位
                    {
                        bools[j + (i * 8)] = ByteToBool((byte)(bytes[i] & tool));//工具数与要检查的byte位与后仅保留1位，转化为bool值后赋给结果
                        tool = (byte)(tool << 1);
                    }
                }
            }

            return bools;
        }

        public byte[] BoolsToBytes(bool[] values)
        {
            byte[] result = new byte[(values.Length / 8) + 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = 0;
                byte tool = 0x01;
                for (int j = 0; (i * 8) + j < values.Length; j++)
                {
                    if (values[(i * 8) + j])
                    {
                        result[i] = (byte)(result[i] | tool);
                    }
                    tool = (byte)(tool << 1);
                }
            }
            return result;
        }

        public UInt16[] BytesToUInt16(byte[] bytes)
        {
            UInt16[] ints = new UInt16[bytes.Length / 2];
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = bytes[i * 2];
                ints[i] = (UInt16)(ints[i] << 8);
                ints[i] = (UInt16)(ints[i] | bytes[(i * 2) + 1]);
            }
            return ints;
        }

        public byte[] Uint16ToBytes(UInt16[] value)
        {
            byte[] bytes = new byte[value.Length * 2];
            for (int i = 0; i < value.Length; i++)
            {
                bytes[i * 2] = (byte)(value[i] >> 8);
                bytes[(i * 2) + 1] = (byte)(value[i] & 0x00ff);
            }
            return bytes;
        }

        public byte[] Int16ToBytes(Int16[] value)
        {
            byte[] bytes = new byte[value.Length * 2];
            for (int i = 0; i < value.Length; i++)
            {
                bytes[i * 2] = (byte)(value[i] >> 8);
                bytes[(i * 2) + 1] = (byte)(value[i] & 0x00ff);
            }
            return bytes;
        }

        public byte[] MakeTargetRespones(byte[] value)
        {
            byte[] result = new byte[12];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = value[i];
            }
            return result;
        }
    }

    public class ReadResult<T>
    {
        public bool isSuccess;
        public T result;

        public ReadResult()
        {
            this.isSuccess = false;
            this.result = default(T);
        }
    }

    public enum CmdCode : byte
    {
        //命令码
        ReadBooleanQ = 0x01,//读线圈Q
        ReadBooleanSM = 0x31,//读线圈SM

        ReadWordM = 0x03,//读寄存器M
        ReadWordSD = 0x33,//读寄存器SD

        WriteSingleBooleanQ = 0x05,//写单个线圈Q
        WriteSingleBooleanSM = 0x35,//写单个线圈SM

        WriteSingleWordM = 0x06,//写单个寄存器M
        WriteSingleWordSD = 0x36,//写单个寄存器SD

        WriteMoreBooleanQ = 0x0F,//写多个线圈Q
        WriteMoreBooleanSM = 0x3F,//写多个线圈SM

        WriteMoreWordM = 0x10,//写多个寄存器M
        WriteMoreWordSD = 0x40//写多个线圈SD
    }

    public abstract class RequestCmd
    {
        public UInt16 sessionNum;//会话编号
        public UInt16 tag;//modbusTCP表示符 0
        public UInt16 length;//后续报文的字节长度
        public byte slaveNum;//从站号
        public CmdCode cmdCode;//命令码

        abstract public byte[] GetBytes();
    }

    class RequestCMDRead : RequestCmd
    {
        public UInt16 startAddress;//起始地址
        public UInt16 reqNum;//读取的数量



        public RequestCMDRead(byte slaveNum, CmdCode cmdCode, ushort startAddress, ushort reqNum)
        {
            this.sessionNum = InovanceModbusTCPTool.GetSessionNum();
            this.tag = 0;
            this.length = 6;
            this.slaveNum = slaveNum;
            this.cmdCode = cmdCode;
            this.startAddress = startAddress;
            this.reqNum = reqNum;
        }

        override public byte[] GetBytes()
        {
            byte[] bytes = new byte[this.length + 6];
            bytes[0] = (byte)(sessionNum >> 8);
            bytes[1] = (byte)(sessionNum & 0x00ff);
            bytes[2] = (byte)(tag >> 8);
            bytes[3] = (byte)(tag & 0x00ff);
            bytes[4] = (byte)(length >> 8);
            bytes[5] = (byte)(length & 0x00ff);
            bytes[6] = slaveNum;
            bytes[7] = (byte)cmdCode;
            bytes[8] = (byte)(startAddress >> 8);
            bytes[9] = (byte)(startAddress & 0x00ff);
            bytes[10] = (byte)(reqNum >> 8);
            bytes[11] = (byte)(reqNum & 0x00ff);
            return bytes;
        }
    }

    class RequestCMDWriteSingle : RequestCmd
    {
        public UInt16 startAddress;//操作的目标地址
        public UInt16 value;//写入的值

        public RequestCMDWriteSingle(byte slaveNum, CmdCode cmdCode, ushort startAddress, ushort value)
        {
            this.sessionNum = InovanceModbusTCPTool.GetSessionNum();
            this.tag = 0;
            this.length = 6;
            this.slaveNum = slaveNum;
            this.cmdCode = cmdCode;
            this.startAddress = startAddress;
            this.value = value;
        }

        override public byte[] GetBytes()
        {
            byte[] bytes = new byte[this.length + 6];
            bytes[0] = (byte)(sessionNum >> 8);
            bytes[1] = (byte)(sessionNum & 0x00ff);
            bytes[2] = (byte)(tag >> 8);
            bytes[3] = (byte)(tag & 0x00ff);
            bytes[4] = (byte)(length >> 8);
            bytes[5] = (byte)(length & 0x00ff);
            bytes[6] = slaveNum;
            bytes[7] = (byte)cmdCode;
            bytes[8] = (byte)(startAddress >> 8);
            bytes[9] = (byte)(startAddress & 0x00ff);
            bytes[10] = (byte)(value >> 8);
            bytes[11] = (byte)(value & 0x00ff);
            return bytes;
        }
    }

    class RequestCMDWriteMore : RequestCmd
    {
        public UInt16 startAddress;//起始地址
        public UInt16 reqNum;//写入的数量
        public byte byteNum;//字节数量
        public byte[] data;//要写入的内容

        public RequestCMDWriteMore(byte slaveNum, CmdCode cmdCode, ushort startAddress, ushort reqNum, byte[] bytes)
        {
            this.sessionNum = InovanceModbusTCPTool.GetSessionNum();
            this.tag = 0;
            this.length = (ushort)(bytes.Length + 7);
            this.slaveNum = slaveNum;
            this.cmdCode = cmdCode;
            this.startAddress = startAddress;
            this.reqNum = reqNum;
            this.byteNum = (byte)bytes.Length;
            this.data = bytes;
        }

        override public byte[] GetBytes()
        {
            byte[] bytes = new byte[this.length + 6];
            bytes[0] = (byte)(sessionNum >> 8);
            bytes[1] = (byte)(sessionNum & 0x00ff);
            bytes[2] = (byte)(tag >> 8);
            bytes[3] = (byte)(tag & 0x00ff);
            bytes[4] = (byte)(length >> 8);
            bytes[5] = (byte)(length & 0x00ff);
            bytes[6] = slaveNum;
            bytes[7] = (byte)cmdCode;
            bytes[8] = (byte)(startAddress >> 8);
            bytes[9] = (byte)(startAddress & 0x00ff);
            bytes[10] = (byte)(reqNum >> 8);
            bytes[11] = (byte)(reqNum & 0x00ff);
            bytes[12] = byteNum;

            for (int i = 0; i < data.Length; i++)
            {
                bytes[i + 13] = data[i];
            }

            return bytes;
        }
    }

    class CheckRes
    {
        private UInt16 reuqestSessionNum;
        private bool findSuccess = false;

        private Respones respones;

        public CheckRes(ushort reuqestSessionNum)
        {
            this.reuqestSessionNum = reuqestSessionNum;
        }

        public ushort ReuqestSessionNum { get => reuqestSessionNum; set => reuqestSessionNum = value; }
        public bool FindSuccess { get => findSuccess; set => findSuccess = value; }
        internal Respones Respones { get => respones; set => respones = value; }
    }

    class Respones
    {
        public UInt16 recSessionNum;
        public UInt16 tag;
        public UInt16 length;
        public byte slaveNum;
        public CmdCode cmdCode;
        public byte[] data;

        public Respones(byte[] bytes)
        {
            this.recSessionNum = (UInt16)(bytes[0] << 8);
            this.recSessionNum = (UInt16)(this.recSessionNum | bytes[1]);
            this.tag = (UInt16)(bytes[2] << 8);
            this.tag = (UInt16)(this.tag | bytes[3]);
            this.length = (UInt16)(bytes[4] << 8);
            this.length = (UInt16)(this.length | bytes[5]);
            this.slaveNum = bytes[6];
            this.cmdCode = (CmdCode)bytes[7];
            data = new byte[bytes.Length];
            Array.ConstrainedCopy(bytes, 0, data, 0, bytes.Length);
        }
    }
}
