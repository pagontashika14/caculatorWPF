using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace MayTinh {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            BieuThucChinh = new TinhBieuThuc();
            SoHang = "";
            bieuThuc = BieuThucChinh.BieuThuc;
            BieuThucChinh.MoiTruong = "Degrees";
            Fix = 10;
            BieuThucChinh.Base = "Decimal";
            shift = false;
            lamMoiTrangThai();
            back = @"backGround3.png";
            isClose = true;
            backGround = new Thread(autoChangeBackground);
            backGround.SetApartmentState(ApartmentState.STA);
            BieuThucChinh.PhepTinh.ans = 0;
            BieuThucChinh.PhepTinh.M = 0;
            //backGround.Start();
            his = new DataBaseHistory();
        }
        void autoChangeBackground() {
            while (isClose) {
                Thread.Sleep(5000);
                changeBackGround();
            }
        }
        bool isClose;
        Thread backGround;
        TinhBieuThuc BieuThucChinh;
        ArrayList bieuThuc;
        Dictionary<string, string> status;
        string SoHang;
        int Fix;
        string back;
        bool shift;
        DataBaseHistory his;
        private object LastIndex(ArrayList a) {
            if (a.Count > 0) {
                return a[a.Count - 1];
            }
            else return "Null";
        }
        void lamMoiTrangThai() {
            string trangThai = "";
            status = new Dictionary<string, string>();
            status.Add("Angle", BieuThucChinh.MoiTruong);
            status.Add("Fix", Fix.ToString());
            status.Add("Base", BieuThucChinh.Base);

            foreach (KeyValuePair<string, string> item in status) {
                trangThai += item.Key + "=" + item.Value + "   ";
            }
            TrangThai.Content = trangThai.Substring(0, trangThai.Length - 3);
        }
        void resetSoHang() {
            if (SoHang != "") {
                try {
                    bieuThuc.Add(double.Parse(SoHang));
                }
                catch (Exception) {

                }
                SoHang = "";
            }
        }
        void setBieuThuc() {
            BieuThuc.Content = BieuThucChinh.Text + SoHang;
        }
        private void button_Num_Click(object sender, RoutedEventArgs e) {
            string temp = LastIndex(bieuThuc).ToString();
            double temp2;
            if (double.TryParse(temp, out temp2)) {
                bieuThuc.RemoveAt(bieuThuc.Count - 1);
            }
            Button num = (Button)sender;
            SoHang += (string)num.Content;
            setBieuThuc();
        }

        private void button_Delete(object sender, RoutedEventArgs e) {
            resetSoHang();
            if (SoHang != "" || bieuThuc.Count > 0) {
                if (SoHang != "") {
                    SoHang = SoHang.Substring(0, SoHang.Length - 1);
                }
                else {
                    if (LastIndex(bieuThuc).GetType() == typeof(double)) {
                        string temp = LastIndex(bieuThuc).ToString();
                        bieuThuc.RemoveAt(bieuThuc.Count - 1);
                        if ((temp[0] == '-' && temp.Length > 2)
                            || (temp[0] != '-' && temp.Length > 1)) {
                            if (temp[temp.Length - 2] == '.') {
                                temp = temp.Substring(0, temp.Length - 1);
                            }
                            temp = temp.Substring(0, temp.Length - 1);
                            bieuThuc.Add(double.Parse(temp));
                        }
                    }
                    else {
                        bieuThuc.RemoveAt(bieuThuc.Count - 1);
                    }
                }
            }
            setBieuThuc();
        }

        private void button_Cong_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("+");
            setBieuThuc();
        }

        private void button_Chia_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("/");
            setBieuThuc();
        }
        private void button_Nhan_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("*");
            setBieuThuc();
        }
        private void button_Tru_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            string temp = LastIndex(bieuThuc).ToString();
            double temp2;
            if (double.TryParse(temp, out temp2)) {
                bieuThuc.Add("-");
            }
            else {
                SoHang = "-";
            }
            setBieuThuc();
        }
        private void button_Bang_Click(object sender, RoutedEventArgs e) {
            if (BieuThuc.Content.ToString() == "0") {
                KetQua.Content = "0";
            }
            else {
                try {
                    resetSoHang();
                    BieuThucChinh.ThucHien();
                }
                catch (Exception er) {
                    KetQua.Content = er.Message;
                    //MessageBox.Show(er.Message);
                    return;
                }

                KetQua.Content = Math.Round(BieuThucChinh.KetQua, Fix);
                Thread history = new Thread(saveHistory);
                history.Start();
                BieuThucChinh.PhepTinh.ans = BieuThucChinh.KetQua;
            }
        }
        void saveHistory() {
            Dispatcher.BeginInvoke(new Action(() => {
                his.insertOneRow(BieuThuc.Content.ToString(), KetQua.Content.ToString());
            }));
        }
        private void button_NgoacMo_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("(");
            setBieuThuc();
        }

        private void button_NgoacDong_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add(")");
            setBieuThuc();
        }
        void resetAll() {
            SoHang = "";
            bieuThuc.Clear();
            BieuThuc.Content = "0";
            KetQua.Content = "0";
            if (BieuThucChinh.Base == "Decimal") {
                Fix = 10;
                lamMoiTrangThai();
            }
        }

        private void button_Reset_Click(object sender, RoutedEventArgs e) {
            if (shift) {
                BieuThucChinh.PhepTinh.M = 0;
                BieuThucChinh.PhepTinh.ans = 0;
                button_Shift_Click(null, null);
            }
            resetAll();
        }

        private void button_Mu_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("^");
            setBieuThuc();
        }

        private void button_Can2_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            if (shift) {
                bieuThuc.Add("can*");
                button_Shift_Click(null, null);
            }
            else {
                bieuThuc.Add("can2");
            }
            setBieuThuc();
        }

        private void button_Mod_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("mod");
            setBieuThuc();
        }

        private void button_Sin_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            if (shift) {
                bieuThuc.Add("asin");
                button_Shift_Click(null, null);
            }
            else {
                bieuThuc.Add("sin");
            }
            setBieuThuc();
        }

        private void button_Cos_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            if (shift) {
                bieuThuc.Add("acos");
                button_Shift_Click(null, null);
            }
            else {
                bieuThuc.Add("cos");
            }
            setBieuThuc();
        }

        private void button_Tan_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            if (shift) {
                bieuThuc.Add("atan");
                button_Shift_Click(null, null);
            }
            else {
                bieuThuc.Add("tan");
            }
            setBieuThuc();
        }

        private void button_PI_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("PI");
            setBieuThuc();
        }

        private void button_Angle_Click(object sender, RoutedEventArgs e) {
            switch (BieuThucChinh.MoiTruong) {
                case "Degrees": BieuThucChinh.MoiTruong = "Radians"; break;
                case "Radians": BieuThucChinh.MoiTruong = "Gradients"; break;
                case "Gradients": BieuThucChinh.MoiTruong = "Degrees"; break;
            }
            lamMoiTrangThai();
        }

        private void button_Fix_Click(object sender, RoutedEventArgs e) {
            switch (Fix) {
                case 10: Fix = 8; break;
                case 8: Fix = 6; break;
                case 6: Fix = 4; break;
                case 4: Fix = 2; break;
                case 2: Fix = 0; break;
                case 0: Fix = 10; break;
            }
            if (KetQua.Content.ToString() != "0") {
                KetQua.Content = Math.Round(BieuThucChinh.KetQua, Fix);
            }
            lamMoiTrangThai();
        }

        private void button_Abs_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("abs");
            setBieuThuc();
        }

        private void button_Ran_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("RAN");
            setBieuThuc();
        }

        private void button_E_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("E");
            setBieuThuc();
        }

        private void button_Base_Click(object sender, RoutedEventArgs e) {
            try {
                switch (BieuThucChinh.Base) {
                    case "Decimal":
                        SoHang = "";
                        BieuThuc.Content = "";
                        KetQua.Content = Convert.ToString(int.Parse(KetQua.Content.ToString()), 2);
                        BieuThucChinh.Base = "Binary";
                        hideForBinaryMode(false, 0.5);
                        TrangThai.Content = "Base = Binary";
                        break;
                    case "Binary":
                        SoHang = "";
                        BieuThuc.Content = "";
                        KetQua.Content = Convert.ToInt32(KetQua.Content.ToString(), 2);
                        KetQua.Content = Convert.ToString(int.Parse(KetQua.Content.ToString()), 8);
                        BieuThucChinh.Base = "Octal";
                        hideForBinaryMode(true, 0.9);
                        hideForOctalMode(false, 0.5);
                        TrangThai.Content = "Base = Octal";
                        break;
                    case "Octal":
                        KetQua.Content = Convert.ToInt32(KetQua.Content.ToString(), 8);
                        BieuThucChinh.Base = "Decimal";
                        hideForOctalMode(true, 0.9);
                        lamMoiTrangThai();
                        break;
                }
            }
            catch (Exception) {
                resetAll();
            }
        }
        void hideForBinaryMode(bool isActive, double opacity) {
            setButton(ref button_2, isActive, opacity);
            setButton(ref button_3, isActive, opacity);
            setButton(ref button_4, isActive, opacity);
            setButton(ref button_5, isActive, opacity);
            setButton(ref button_6, isActive, opacity);
            setButton(ref button_7, isActive, opacity);
            hideForOctalMode(isActive, opacity);
        }
        void hideForOctalMode(bool isActive, double opacity) {
            setButton(ref button_8, isActive, opacity);
            setButton(ref button_9, isActive, opacity);
            setButton(ref button_Dot, isActive, opacity);
            setButton(ref button_Fix, isActive, opacity);
            setButton(ref button_Angle, isActive, opacity);
            //setButton(ref button_E, isActive, opacity);
            setButton(ref button_PI, isActive, opacity);
            setButton(ref button_Sin, isActive, opacity);
            setButton(ref button_Cos, isActive, opacity);
            setButton(ref button_Tan, isActive, opacity);
            setButton(ref button_Can2, isActive, opacity);
            setButton(ref button_Abs, isActive, opacity);
            setButton(ref button_Mod, isActive, opacity);
            setButton(ref button_Mu, isActive, opacity);
        }
        void setButton(ref Button button, bool isActive, double opacity) {
            button.Opacity = opacity;
            button.IsEnabled = isActive;
        }
        #region ButtonBeauty
        bool xuLyKetQua(double d, ref string a, ref string b, ref string c) {
            d *= d;
            double tuSo = 0, mauSo = 1, temp = 1;
            if (tachPhanSo(d, ref tuSo, ref mauSo)) {
                if (!kiemTraSoNguyen(Math.Sqrt(mauSo))) {
                    tuSo *= mauSo;
                    mauSo *= mauSo;
                }
                int i, k = 0;
                for (i = 4; i <= tuSo; i++) {
                    if (kiemTraChinhPhuong(i, ref k)) {
                        if (kiemTraSoNguyen(tuSo / i)) {
                            temp *= k;
                            tuSo /= i;
                        }
                    }
                }
                a = temp.ToString();
                b = tuSo.ToString();
                c = Math.Sqrt(mauSo).ToString();
                return true;
            }
            return false;
        }
        bool tachPhanSo(double d, ref double a, ref double b) {
            int i;
            for (i = 1; i < 1000000; i++) {
                if (kiemTraSoNguyen(d * i)) {
                    a = Math.Floor(d * i + 0.5);
                    b = i;
                    return true;
                }
            }
            return false;
        }
        bool kiemTraSoNguyen(double d) {
            return Math.Abs(d - Math.Floor(d + 0.5)) < 0.0000001 ? true : false;
        }
        bool kiemTraChinhPhuong(int n, ref int k) {
            int i = 1, j = 2, t;
            t = n;
            while (t / 100 >= 1) {
                t /= 100;
                i *= 10;
            }
            while (j * j < t) j++;
            for (i *= (j - 1); i * i <= n; i++) {
                if (i * i == n) {
                    k = i;
                    return true;
                }
            }
            return false;
        }
        void convertToFraction() {
            string a = "", b = "", c = "";
            if (xuLyKetQua(BieuThucChinh.KetQua, ref a, ref b, ref c)) {
                if (a == "1") a = "";
                c = c == "1" ? "" : "/" + c;
                b = b == "1" ? "" : "√" + b;
                if (a == "" && b == "") a = "1";
                if (b == "√0") {
                    KetQua.Content = "0";
                }
                else {
                    KetQua.Content = string.Format("{0}{1}{2}", a, b, c);
                }
            }
        }
        private void button_Beauty_Click(object sender, RoutedEventArgs e) {

            convertToFraction();
        }
        #endregion
        void changeBackGround() {
            switch (back) {
                case "backGround3.png": back = "background.jpg"; break;
                case "background.jpg": back = "backGround1.jpg"; break;
                case "backGround1.jpg": back = "hackers.png"; break;
                case "hackers.png": back = "backGround3.png"; break;
            }
            Dispatcher.BeginInvoke(new Action(() => {
                ImageBrush myBrush = new ImageBrush();
                Image image = new Image();
                image.Source = new BitmapImage(
                    new Uri(BaseUriHelper.GetBaseUri(this), @"Resources/Images/" + back));
                myBrush.ImageSource = image.Source;
                Background = myBrush;
            }));
        }

        private void MayTinh_MouseDown(object sender, MouseButtonEventArgs e) {
            changeBackGround();
        }

        private void MayTinh_Closed(object sender, EventArgs e) {
            isClose = false;
        }

        private void button_Ans_Click(object sender, RoutedEventArgs e) {
            resetSoHang();
            bieuThuc.Add("ANS");
            setBieuThuc();
        }

        private void button_Shift_Click(object sender, RoutedEventArgs e) {
            if (shift) {
                hideAllButton(true, 0.9);
                button_Reset.Content = "Reset";
                button_Can2.Content = "√";
                button_Sin.Content = "Sin";
                button_Cos.Content = "Cos";
                button_Tan.Content = "Tan";
            }
            else {
                hideAllButton(false, 0.5);
                setButton(ref button_M, true, 0.9);
                setButton(ref button_Reset, true, 0.9);
                button_Reset.Content = "Reset All";
                setButton(ref button_Can2, true, 0.9);
                button_Can2.Content = "*√";
                setButton(ref button_Sin, true, 0.9);
                button_Sin.Content = "ASin";
                setButton(ref button_Cos, true, 0.9);
                button_Cos.Content = "ACos";
                setButton(ref button_Tan, true, 0.9);
                button_Tan.Content = "ATan";
            }
            shift = !shift;
        }
        void hideAllButton(bool isActive, double opacity) {
            hideForBinaryMode(isActive, opacity);
            setButton(ref button_1, isActive, opacity);
            setButton(ref button_0, isActive, opacity);
            setButton(ref button_Cong, isActive, opacity);
            setButton(ref button_Tru, isActive, opacity);
            setButton(ref button_Nhan, isActive, opacity);
            setButton(ref button_Chia, isActive, opacity);
            setButton(ref button_Del, isActive, opacity);
            setButton(ref button_Ans, isActive, opacity);
            setButton(ref button_Bang, isActive, opacity);
            setButton(ref button_NgoacDong, isActive, opacity);
            setButton(ref button_NgoacMo, isActive, opacity);
            setButton(ref button_Ran, isActive, opacity);
            setButton(ref button_Reset, isActive, opacity);
            setButton(ref button_Beauty, isActive, opacity);
            setButton(ref button_Base, isActive, opacity);
            setButton(ref button_M, isActive, opacity);
            setButton(ref button_History, isActive, opacity);
        }

        private void button_M_Click(object sender, RoutedEventArgs e) {
            
            if (shift) {
                BieuThucChinh.PhepTinh.M = BieuThucChinh.KetQua;
                button_Shift_Click(null, null);
            }
            else {
                resetSoHang();
                bieuThuc.Add("M");
                setBieuThuc();
            }
        }

        private void MayTinh_KeyUp(object sender, KeyEventArgs e) {
            //MessageBox.Show(e.Key.ToString());
            switch (e.Key) {
                case Key.NumPad0: button_Num_Click(button_0, null); break;
                case Key.Decimal: button_Num_Click(button_Dot, null); break;
                case Key.NumPad1: button_Num_Click(button_1, null); break;
                case Key.NumPad2: button_Num_Click(button_2, null); break;
                case Key.NumPad3: button_Num_Click(button_3, null); break;
                case Key.NumPad4: button_Num_Click(button_4, null); break;
                case Key.NumPad5: button_Num_Click(button_5, null); break;
                case Key.NumPad6: button_Num_Click(button_6, null); break;
                case Key.NumPad7: button_Num_Click(button_7, null); break;
                case Key.NumPad8: button_Num_Click(button_8, null); break;
                case Key.NumPad9: button_Num_Click(button_9, null); break;
                case Key.Add: button_Cong_Click(button_Cong, null); break;
                case Key.Subtract: button_Tru_Click(button_Tru, null); break;
                case Key.Multiply: button_Nhan_Click(button_Nhan, null); break;
                case Key.Divide: button_Chia_Click(button_Chia, null); break;
                case Key.Space: button_Bang_Click(button_Bang, null); break;
                case Key.Delete: button_Delete(button_Del, null); break;
                case Key.Back: button_Reset_Click(button_Reset, null); break;
                case Key.RightShift: button_Shift_Click(button_Shift, null); break;
                case Key.LeftShift: button_Shift_Click(button_Shift, null); break;
            }
        }

        private void button_History_Click(object sender, RoutedEventArgs e) {
            WindowHistory winH = new WindowHistory();
            winH.ShowDialog();
        }
    }
}
