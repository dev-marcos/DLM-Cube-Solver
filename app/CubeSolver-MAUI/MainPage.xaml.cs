using CommunityToolkit.Maui.Views;
using CubeSolver_MAUI.Views;
using CubeSolver_MAUI.Helpers;
using Kociemba;

namespace CubeSolver_MAUI;

public partial class MainPage : ContentPage
{
    private BluetoothConnectorService? bluetoothService = null;
    Color selected_color = Colors.Gray;
    FaceletCube cube = new();

    public MainPage()
    {
        InitializeComponent();
    }

    private void BTN_Colors_Clicked(object sender, EventArgs e)
    {
        Button[] buttons = { BTN_White, BTN_Green, BTN_Red, BTN_Yellow, BTN_Blue, BTN_Orange };
        foreach (Button b in buttons) 
        {
            b.BorderColor = Colors.Black;
            b.BorderWidth = 4;
        }

        var clicked_button = (Button) sender;
        clicked_button.BorderColor = Colors.DeepSkyBlue;
        clicked_button.BorderWidth = 5;
        selected_color = clicked_button.BackgroundColor;
    }
    private void BTN_Reset_Clicked(object sender, EventArgs e)
    {
        Button[] buttons = {
            BTN_U1, BTN_U2, BTN_U3, BTN_U4, BTN_U5, BTN_U6, BTN_U7, BTN_U8, BTN_U9,
            BTN_R1, BTN_R2, BTN_R3, BTN_R4, BTN_R5, BTN_R6, BTN_R7, BTN_R8, BTN_R9,
            BTN_F1, BTN_F2, BTN_F3, BTN_F4, BTN_F5, BTN_F6, BTN_F7, BTN_F8, BTN_F9,
            BTN_D1, BTN_D2, BTN_D3, BTN_D4, BTN_D5, BTN_D6, BTN_D7, BTN_D8, BTN_D9,
            BTN_L1, BTN_L2, BTN_L3, BTN_L4, BTN_L5, BTN_L6, BTN_L7, BTN_L8, BTN_L9,
            BTN_B1, BTN_B2, BTN_B3, BTN_B4, BTN_B5, BTN_B6, BTN_B7, BTN_B8, BTN_B9
        };

        for (int i = 0; i < 54; ++i)
        {
            cube[i] = Colors.Gray;
            buttons[i].BackgroundColor = Colors.Gray;
        }

        // This part is just for making the Reset button set the cube to solved state, to facilitate testing
        //for (int i = 0; i < 54; ++i)
        //{
        //    if (i < 9)
        //    {
        //        cube[i] = Colors.White;
        //        buttons[i].BackgroundColor = Colors.White;
        //    }
        //    else if (i >= 9 && i < 18)
        //    {
        //        cube[i] = Colors.Red;
        //        buttons[i].BackgroundColor = Colors.Red;
        //    }
        //    else if (i >= 18 && i < 27)
        //    {
        //        cube[i] = Colors.Green;
        //        buttons[i].BackgroundColor = Colors.Green;
        //    }
        //    else if (i >= 27 && i < 36)
        //    {
        //        cube[i] = Colors.Yellow;
        //        buttons[i].BackgroundColor = Colors.Yellow;
        //    }
        //    else if (i >= 36 && i < 45)
        //    {
        //        cube[i] = Colors.Orange;
        //        buttons[i].BackgroundColor = Colors.Orange;
        //    }
        //    else
        //    {
        //        cube[i] = Colors.Blue;
        //        buttons[i].BackgroundColor = Colors.Blue;
        //    }
        //}
    }

    private void BTN_Facelet_Clicked(object sender, EventArgs e)
    {
        var clicked_button = (Button) sender;
        clicked_button.BackgroundColor = selected_color;
        cube[int.Parse(clicked_button.ClassId)] = selected_color;
    }

    private async void BTN_Connect_ClickedAsync(object sender, EventArgs e)
    {           
#if ANDROID
        PermissionStatus status;
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
        {
            status = await Permissions.CheckStatusAsync<BluetoothPermissions>();

            if (status != PermissionStatus.Granted)
                await DisplayAlert("Permission required", "Bluetooth permission is required.", "OK");

            status = await Permissions.RequestAsync<BluetoothPermissions>();
        }
        else
        {
            status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
                await DisplayAlert("Permission required", "Location permission is required for bluetooth scanning. We do not store or use your location at all.", "OK");

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
                await DisplayAlert("Permission required", "Location permission is required for bluetooth scanning. We do not store or use your location at all.", "OK");

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }
#endif
        if (bluetoothService == null)
        {
            try
            {
                bluetoothService = new BluetoothConnectorService();
            }
            catch (Exception ex)
            {
                bluetoothService = null;
                await DisplayAlert("Bluetooth service error", ex.Message, "OK");
                return;
            }
        }

        if (!bluetoothService.IsTurnedOn())
        {
            await DisplayAlert("Bluetooth is turned off", "Enable Bluetooth to be able to connect to other devices", "OK");
            return;
        }

        List<string> bluetoothDevices = bluetoothService.GetBondedDevices();

        string deviceName = await DisplayActionSheet("Available Bluetooth devices", "Cancel", null, bluetoothDevices.ToArray());

        if (deviceName == "Cancel" || deviceName == null)
            return;

        LoadingPopup loadingPopup = new();
        this.ShowPopup(loadingPopup);

        if (bluetoothService.IsConnected())
            bluetoothService.Disconnect();

        bool gotConnected = await bluetoothService.ConnectAsync(deviceName);

        loadingPopup.Close();

        if (gotConnected)
            await DisplayAlert("Bluetooth connection", "Connected successfully", "OK");
        else
            await DisplayAlert("Bluetooth connection", "Connection timed out", "OK");
    }

    private async void BTN_Solve_ClickedAsync(object sender, EventArgs e)
    {
        var solve_button = (Button) sender;
        solve_button.IsEnabled = false;
        LBL_Info.Text = "Solving...";
        string cubeString = cube.ToStr();
        string readable_solution = "", solution = "", info = "";


        if (Tools.NeedToCreateTables())
        {
            Console.WriteLine("Criando tabelas...");
            await Task.Run(() => solution = SearchRunTime.solution(cubeString, out info, buildTables: true, timeOutMillis: 10000));
        }
        else
        {
            Console.WriteLine("Tabelas ja tinham sido criadas.");
            await Task.Run(() => solution = Search.solution(cubeString, out info));
        }

        readable_solution = Tools.solutionToReadable(solution);
        if (solution != "")
        {
            LBL_Info.Text = "Solution: " + readable_solution;

            if (bluetoothService != null && bluetoothService.IsConnected())
            {
                bluetoothService.SendString(solution + "\n");
                LBL_Info.Text += "\nCommands sent: " + solution;
            }
            else
            {
                LBL_Info.Text += "\nNot connected to the cube solver.";
            }

            LBL_Info.Text += "\nInfo: " + info;
        }
        else
        {
            LBL_Info.Text = info;
        }

        solve_button.IsEnabled = true;
    }

}
