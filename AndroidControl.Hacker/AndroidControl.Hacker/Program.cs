using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AndroidControl.Hacker
{

    class Program
    {
        static void Main(string[] args)
        {

            var meuIp = GetLocalIPAddress();
            Console.WriteLine("My IP: " + meuIp);
            var _servidor = new WebSocketServer("ws://" + meuIp + ":12345");
            var _conexoes = new List<IWebSocketConnection>();
            var swap = false;
            var cursor = false;
            var barra = 1;
            _servidor.Start((conexao) =>
            {
                conexao.OnOpen = () =>
                {
                    _conexoes.Add(conexao);
                    Console.WriteLine("User connected: " + conexao.ConnectionInfo.ClientIpAddress);

                };

                conexao.OnClose = () =>
                {
                    _conexoes.Remove(conexao);
                };

                conexao.OnMessage = (mensagem) =>
                {
                    mensagem = mensagem.ToLower();

                    switch (mensagem)
                    {
                        case "hide":
                            {
                                barra = barra == 0 ? 1 : 0;
                                Helper.TaskBar(barra);
                                break;
                            }
                        case "swap":
                            {
                                swap = !swap;
                                Helper.SwapMouse(swap);
                                break;
                            }
                        case "movecursor":
                            {
                                 
                                Helper.MouseCursor();
                                break;
                            }
                        case "opencddriver":
                            {

                                Helper.OpenCdDriver();
                                break;
                            }
                        case "lock":
                            {

                                Helper.Lock();
                                break;
                            }

                        case "wallpaper":
                            {
                                Helper.SetWallpaper(@"C:\Users\erick.silva\Downloads\vampeta.jpg");
                                break;

                            }
                    }

                };
            });

            Console.ReadKey();
        }


        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}