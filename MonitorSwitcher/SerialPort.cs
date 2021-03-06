﻿namespace MonitorSwitcher
{
    using System.IO.Ports;
    using System.Threading;

    public class LocalSerialPort : IMessageTransport
    {
        private SerialPort comPort;

        public LocalSerialPort()
        {
            this.comPort = new SerialPort("COM1")
            {
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 100
            };
        }

        public int SendMessage(byte[] msgData, out byte[] msgResponse)
        {
            try
            {
                this.comPort.Open();

                this.comPort.Write(msgData, 0, msgData.Length);

                Thread.Sleep(200);

                if (this.comPort.BytesToRead > 0)
                {
                    msgResponse = new byte[this.comPort.BytesToRead];

                    this.comPort.Read(msgResponse, 0, this.comPort.BytesToRead);

                    return 0;
                }
                else
                {
                    msgResponse = null;

                    return 1;
                }
            }
            catch
            {
                msgResponse = null;

                return 2;
            }
            finally
            {
                this.comPort.Close();
            }
        }
    }
}
