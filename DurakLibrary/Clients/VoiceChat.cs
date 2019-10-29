using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakLibrary.Clients
{
    public class VoiceChat
    {
        public readonly IPAddress HostIP;
        public readonly int HostPort;
        public Dictionary<IPAddress, bool> MutedPlayers { get; private set; }

        public VoiceChat(IPAddress ip, int port)
        {
            HostIP = ip;
            HostPort = port;
            MutedPlayers = new Dictionary<IPAddress, bool>();

            var waveFormat = new WaveFormat(22050, 16, 1);
            input = new WaveIn();
            input.WaveFormat = waveFormat;
            input.DataAvailable += VoiceInput;
            sendingClient = new UdpClient(HostIP.ToString(), HostPort);

            output = new WaveOut();
            bufferStream = new BufferedWaveProvider(waveFormat);
            bufferStream.DiscardOnBufferOverflow = true;
            output.Init(bufferStream);

            isConnected = true;
            Task.Factory.StartNew(Listening, TaskCreationOptions.LongRunning);
        }

        public void MicroTurnOnOff(ref bool isActivate)
        {
            Thread.Sleep(500); //If you quickly go into this method, the previous command (start or stop recording) does not have time to process.

            try
            {
                if (isActivate)
                    input.StartRecording();
                else
                    input.StopRecording();
            }
            catch
            {
                isActivate = false;
                MessageBox.Show("You do not have a microphone connected");
            }
        }

        public void CloseChat()
        {
            isConnected = false;
            listeningClient?.Close();
            output?.Dispose();
            output = null;

            sendingClient?.Close();
            input?.Dispose();
            input = null;

            bufferStream = null;
        }

        private bool isConnected;
        private UdpClient sendingClient;
        private WaveIn input;
        private WaveOut output;
        private BufferedWaveProvider bufferStream;
        private UdpClient listeningClient;

        private void VoiceInput(object sender, WaveInEventArgs e)
        {
            try { sendingClient.Send(e.Buffer, e.Buffer.Length); }
            catch { }
        }

        private void Listening()
        {
            try
            {
                listeningClient = new UdpClient(new IPEndPoint(HostIP, HostPort));
                IPEndPoint remoteIP = null;
                output.Play();

                while (isConnected)
                {
                    var data = listeningClient.Receive(ref remoteIP);

                    if (MutedPlayers.ContainsKey(remoteIP.Address) && !MutedPlayers[remoteIP.Address])
                        bufferStream.AddSamples(data, 0, data.Length);
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    Thread.Sleep(100); //For to appear after LobbyClient
                    MessageBox.Show("SocketException — AddressAlreadyInUse: The Listening method will be disabled!");
                    return;
                }
                if (ex.SocketErrorCode == SocketError.Interrupted)
                {
                    return;
                }
                throw;
            }
        }
    }
}
