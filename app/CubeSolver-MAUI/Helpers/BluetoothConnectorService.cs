// https://erikthiago.medium.com/net-maui-e-bluetooth-classic-no-android-59614cfd16c3

#if ANDROID
using Android.Bluetooth;
using Java.Util;
using System.Text;
using Xamarin.Google.Crypto.Tink.Subtle;
#endif


namespace CubeSolver_MAUI.Helpers
{
    public class BluetoothConnectorService
    {
#if ANDROID
        private BluetoothAdapter _adapter;
        private BluetoothSocket? _socket;

        // Como fazer a config para tirar o warning de obsoleto: https://stackoverflow.com/questions/76766303/how-to-solve-bluetoothadapter-defaultadapter-is-obsoleted-on-android-31-0
#endif
        public BluetoothConnectorService()
        {
#if ANDROID
            BluetoothManager? bluetoothManager = MauiApplication.Current.GetSystemService("bluetooth") as BluetoothManager;
            if (bluetoothManager == null)
                throw new Exception("No Bluetooth service found.");
            if (bluetoothManager.Adapter == null)
                throw new Exception("No Bluetooth adapter found.");
            
            _adapter = bluetoothManager.Adapter;
            _socket = null; // Use ConnectAsync() to set connection using the socket
#endif
        }

        public bool IsTurnedOn()
        {
#if ANDROID
            return _adapter.IsEnabled;
#else
            return false;
#endif
        }

        public List<string> GetBondedDevices()
        {
            // Como fazer a configuração pra acessar o codigo nativo: https://stackoverflow.com/questions/74236294/maui-dependencyservice-getimyservice-return-null/74236424#74236424
            // https://github.com/dotnet/maui-samples/blob/main/8.0/PlatformIntegration/InvokePlatformCodeDemos/InvokePlatformCodeDemos/Services/ConditionalCompilation/DeviceOrientationService.cs
#if ANDROID
            if (_adapter.IsEnabled)
            {
                if (_adapter.BondedDevices.Count > 0)
                    return _adapter.BondedDevices.Select(d => d.Name).ToList();
            }
            
            else
                Console.Write("Bluetooth is not enabled on device");

#endif
            return new List<string>();
        }

        public async Task<bool> ConnectAsync(string deviceName)
        {
#if ANDROID
            try
            {
                var device = _adapter.BondedDevices.FirstOrDefault(d => d.Name == deviceName);
                var robot_uuid = device.GetUuids().First();
                _socket = device.CreateInsecureRfcommSocketToServiceRecord(robot_uuid.Uuid);
                await _socket.ConnectAsync();
                // \/ The next code was used for testing connection to other cellphone \/
                //_serverSocket = _adapter.ListenUsingInsecureRfcommWithServiceRecord("Teste", UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                //_socket = await _serverSocket.AcceptAsync(timeout:7000); // timeout in ms
            }
            catch (Exception) { } // Just return IsConnected() below in the end, if anything happens
#endif
            // It may happen that it doesn't connect, but also doesn't throw an exception, returning a socket.
            // So we test the socket in the end!
            return IsConnected();
        }

        public bool IsConnected()
        {
            bool result = false;
#if ANDROID
            // If it's not null, there's a chance the other device disconnected unexpectedly.
            // To adress this, we try to contact the other side to make sure it's still there.
            if (_socket != null) 
            {
                result = _socket.IsConnected;

                try
                {
                    _socket.OutputStream.Write([], 0, 0);
                }
                catch
                {
                    result = false;
                    Disconnect();
                }
            }
#endif
            return result;
        }

        public void Disconnect()
        {
#if ANDROID
            _socket.OutputStream.Close();
            Thread.Sleep(1500);
            _socket.Close();
            _socket = null;
#endif
        }
        public async Task SendString(string str)
        {
#if ANDROID
            await _socket.OutputStream.WriteAsync(Encoding.ASCII.GetBytes(str), 0, str.Length);
#endif
        }
    }
}
