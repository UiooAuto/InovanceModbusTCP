using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InovanceModbusTCP
{
    public class InovanceModbusTCPTool
    {
        private Socket socket;//用于通信的套接字
        private IPAddress iPAddress;//服务器的IP地址
        private int port;//服务器的端口
        private IPEndPoint iPEndPoint;//服务器的通信节点
    }

    public enum CmdCode : Int16
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
}
