using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
    class Program
    {
        public static int Main(string[] args)
        {
            StartClient();
            return 0;
        }

        public static void StartClient()
        {
            byte[] bytes = new byte[1024];
			//teste
			//teste2
			
            try
            {
                //Conecta ao servidor remoto
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                //Cria o socket
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // Connect to Remote EndPoint 
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to: {0}",
                        sender.RemoteEndPoint.ToString());
                    string resposta = "y";

                    while (!resposta.Equals("n"))
                    {
                        //Envia a operação para o servidor
                        Console.WriteLine("Qual a operacao que pretende efetuar? (soma/sub/mult/div)");
                        string operacao_pedida = Console.ReadLine();
                        byte[] operacao = Encoding.ASCII.GetBytes(operacao_pedida);
                        int bytesOperacao = sender.Send(operacao);

                        //Envia o valor1 para o servidor
                        Console.WriteLine("Indique o primeiro valor: ");
                        string valor1_pedido = Console.ReadLine();
                        byte[] valor1 = Encoding.ASCII.GetBytes(valor1_pedido);
                        int bytesValor1 = sender.Send(valor1);

                        //Envia o valor2 para o servidor
                        Console.WriteLine("Indique o segundo valor: ");
                        string valor2_pedido = Console.ReadLine();
                        byte[] valor2 = Encoding.ASCII.GetBytes(valor2_pedido);
                        int bytesValor2 = sender.Send(valor2);

                        //Recebe o resultado calculado pelo servidor
                        int bytesRec = sender.Receive(bytes);
                        Console.WriteLine("Resultado: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                        //Espaçamento
                        Console.WriteLine("  ");

                        Console.WriteLine("Pretende continuar a calcular? (y/n)");
                        resposta = Console.ReadLine();
                        byte[] respostaByte = Encoding.ASCII.GetBytes(resposta);
                        int bytesResposta = sender.Send(respostaByte);

                        //Espaçamento
                        Console.WriteLine("  ");
                    }

                    //Desliga o socket
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException a)
                {
                    Console.WriteLine("ArgumentNullException: {0}", a.ToString());
                }
                catch (SocketException a)
                {
                    Console.WriteLine("SocketException: {0}", a.ToString());
                }
                catch (Exception a)
                {
                    Console.WriteLine("Unexpected exception: {0}", a.ToString());
                }
            }
            catch (Exception a)
            {
                Console.WriteLine(a.ToString());
            }
        }
    }
}
