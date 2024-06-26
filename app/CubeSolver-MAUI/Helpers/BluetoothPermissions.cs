// https://erikthiago.medium.com/net-maui-e-bluetooth-classic-no-android-59614cfd16c3

using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CubeSolver_MAUI.Helpers
{
    internal class BluetoothPermissions : BasePlatformPermission
    {
#if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {
            ("android.permission.BLUETOOTH", true),                 // BLE
            ("android.permission.BLUETOOTH_ADMIN", true),           // BLE
            ("android.permission.BLUETOOTH_SCAN", true),            // BLE
            ("android.permission.BLUETOOTH_CONNECT", true),         // BLE
            ("android.permission.ACCESS_COARSE_LOCATION", true),    // Bluetooth Classic
            ("android.permission.ACCESS_FINE_LOCATION", true)       // Bluetooth Classic
            }.ToArray();
        //  This list includes Bluetooth permissions. 
        //  You will need to include these permissions in your Android Manifest, too.
#endif
    }
}
