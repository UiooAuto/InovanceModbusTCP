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
        private IPEndPoint ipEndPoint;//服务器的通信节点
        private Thread readThread;//接受响应线程
        private Ping ping;//测试网络连接状态
        private int overTime = 1000;//ping的超时时间
        private int recLength;//接收到的数据长度
        private int recSessionNum;//接收到的会话号
        private byte[] recData;//接收到的数据包

        private static UInt16 ComSessionNum = 0;//生成用公共会话编号
        private UInt16 sessionNum;//会话编号
        private UInt16 tag;//modbusTCP表示符 0
        private UInt16 length;//后续报文的字节长度
        private byte slaveNum;//从站号
        private CmdCode cmdCode;//命令码
        private UInt16 startAddress;//起始地址

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

        /// <summary>
        /// 创建一个汇川ModbusTCP通信对象
        /// </summary>
        /// <param name="iPAddress">服务器IP</param>
        /// <param name="port">服务器端口</param>
        public InovanceModbusTCPTool(string iPAddress, int port)
        {
            this.ipAddress = IPAddress.Parse(iPAddress);
            this.port = port;
            this.ipEndPoint = new IPEndPoint(ipAddress,port);
            this.ping = new Ping();
        }

        public InovanceModbusTCPTool()
        {
        }

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

            return true;
        }

        public void CloseConnect()
        {
            if (socket != null)
            {
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

        private bool SendTo(IRequestCmd cmd)
        {
            bool success = false;
            int v;
            try
            {
                v = socket.Send(cmd.GetBytes());
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

        }

        public ReadResult<bool> ReadBoolean(byte slaveNum, ushort startAddress, ushort reqNum)
        {
            ReadResult<bool> readResult = new ReadResult<bool>();

            IRequestCmd cmd = new RequestCMDRead(slaveNum, CmdCode.ReadBooleanQ, startAddress, reqNum);
            bool v = SendTo(cmd);

            return readResult;
        }
    }

    public class ReadResult<T>
    {
        public bool success;
        public T data;

        public ReadResult()
        {
            this.success = false;
            this.data = default(T);
        }
    }

    enum CmdCode : byte
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

    public interface IRequestCmd
    {
        byte[] GetBytes();
    }

    class RequestCMDRead : IRequestCmd
    {
        private UInt16 sessionNum;//会话编号
        private UInt16 tag;//modbusTCP表示符 0
        private UInt16 length;//后续报文的字节长度
        private byte slaveNum;//从站号
        private CmdCode cmdCode;//命令码
        private UInt16 startAddress;//起始地址
        private UInt16 reqNum;//读取的数量

        public RequestCMDRead(byte slaveNum, CmdCode cmdCode, ushort startAddress, ushort reqNum)
        {
            this.tag = 0;
            this.length = 6;
            this.slaveNum = slaveNum;
            this.cmdCode = cmdCode;
            this.startAddress = startAddress;
            this.reqNum = reqNum;
        }

        public byte[] GetBytes()
        {
            this.sessionNum = InovanceModbusTCPTool.GetSessionNum();
            byte [] bytes = new byte[this.length+6];
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

    class RequestCMDWriteSingle : IRequestCmd
    {
        private UInt16 sessionNum;//会话编号
        private UInt16 tag;//modbusTCP表示符 0
        private UInt16 length;//后续报文的字节长度
        private byte slaveNum;//从站号
        private CmdCode cmdCode;//命令码
        private UInt16 startAddress;//操作的目标地址
        private UInt16 value;//写入的值

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

        public byte[] GetBytes()
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

    class RequestCMDWriteMore : IRequestCmd
    {
        private UInt16 sessionNum;//会话编号
        private UInt16 tag;//modbusTCP表示符 0
        private UInt16 length;//后续报文的字节长度
        private byte slaveNum;//从站号
        private CmdCode cmdCode;//命令码
        private UInt16 startAddress;//起始地址
        private UInt16 reqNum;//写入的数量
        private byte byteNum;//字节数量
        private byte[] bytes;//要写入的内容

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
            this.bytes = bytes;
        }

        public byte[] GetBytes()
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

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i + 13] = bytes[i];
            }

            return bytes;
        }
    }
}
