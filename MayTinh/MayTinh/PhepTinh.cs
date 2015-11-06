using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MayTinh {
    public delegate void ThucHienTinh(ref Stack nganS);
    class PhepTinh {
        public class ToanTu {
            public string TheHien;
            public int CapDo;
            public ThucHienTinh Tinh;
            public ToanTu(string theHien, int capDo, ThucHienTinh tinh) {
                TheHien = theHien;
                CapDo = capDo;
                Tinh = tinh;
            }
            public ToanTu(string theHien, ThucHienTinh tinh) {
                TheHien = theHien;
                CapDo = 0;
                Tinh = tinh;
            }
        }
        public Dictionary<string, ToanTu> CacPhepToan;
        public string Angle;
        public string Base;
        public int baseInt {
            get {
                switch (Base) {
                    case "Binary": return 2;
                    case "Octal": return 8;
                    default: return 10;
                }
            }
        }
        public double ans;
        public double M;
        /// <summary>
        /// Hàm thêm các phép tính
        /// CacPhepToan.Add("can*", new ToanTu("*√", 5, new ThucHienTinh(canx)));
        /// Trong đó
        /// can* là chuỗi ký tự đại diện, 
        /// *√ là chuỗi ký tự thể hiện
        /// 5 là độ ưu tiên
        /// ThucHienTinh(canx) là delegate để ủy quyền cách tính toán cho hàm ở dưới
        /// </summary>
        public PhepTinh() {
            CacPhepToan = new Dictionary<string, ToanTu>();
            CacPhepToan.Add("+", new ToanTu("+", 3, new ThucHienTinh(cong)));
            CacPhepToan.Add("-", new ToanTu("−", 3, new ThucHienTinh(tru)));
            CacPhepToan.Add("*", new ToanTu("×", 4, new ThucHienTinh(nhan)));
            CacPhepToan.Add("/", new ToanTu("÷", 4, new ThucHienTinh(chia)));
            CacPhepToan.Add("^", new ToanTu("^", 5, new ThucHienTinh(mu)));
            CacPhepToan.Add("can2", new ToanTu("√", 6, new ThucHienTinh(can2)));
            CacPhepToan.Add("can*", new ToanTu("*√", 5, new ThucHienTinh(canx)));
            CacPhepToan.Add("mod", new ToanTu("Mod", 5, new ThucHienTinh(mod)));
            CacPhepToan.Add("sin", new ToanTu("Sin", 6, new ThucHienTinh(sin)));
            CacPhepToan.Add("cos", new ToanTu("Cos", 6, new ThucHienTinh(cos)));
            CacPhepToan.Add("tan", new ToanTu("Tan", 6, new ThucHienTinh(tan)));
            CacPhepToan.Add("asin", new ToanTu("ArcSin", 6, new ThucHienTinh(asin)));
            CacPhepToan.Add("acos", new ToanTu("ArcCos", 6, new ThucHienTinh(acos)));
            CacPhepToan.Add("atan", new ToanTu("ArcTan", 6, new ThucHienTinh(atan)));
            CacPhepToan.Add("abs", new ToanTu("Abs", 6, new ThucHienTinh(abs)));
            CacPhepToan.Add("PI", new ToanTu("π", new ThucHienTinh(pi)));
            CacPhepToan.Add("E", new ToanTu("℮", new ThucHienTinh(e)));
            CacPhepToan.Add("RAN", new ToanTu("Ran", new ThucHienTinh(ran)));
            CacPhepToan.Add("ANS", new ToanTu("Ans", new ThucHienTinh(Ans)));
            CacPhepToan.Add("M", new ToanTu("M", new ThucHienTinh(m)));
        }
        public int CapDo(string s) {
            switch (s) {
                case "(": return 1;
                case ")": return 2;
            }
            double temp;
            if (double.TryParse(s, out temp)) {
                return 0;
            }
            return CacPhepToan[s].CapDo;
        }
        bool BaseConvert(ref double x, ref double y) {
            if (baseInt != 10) {
                x = Convert.ToInt32(x.ToString(), baseInt);
                y = Convert.ToInt32(y.ToString(), baseInt);
                return true;
            }
            else
                return false;
        }
        void cong(ref Stack nganS) {
            double y = (double)nganS.Pop();
            double x = (double)nganS.Pop();
            double kq;
            if (BaseConvert(ref x, ref y)) {
                string temp = Convert.ToString((int)(x + y), baseInt);
                kq = double.Parse(temp);
            }
            else {
                kq = x + y;
            }
            nganS.Push(kq);
        }
        void tru(ref Stack nganS) {
            double y = (double)nganS.Pop();
            double x = (double)nganS.Pop();
            double kq;
            if (BaseConvert(ref x, ref y)) {
                string temp = Convert.ToString((int)(x - y), baseInt);
                kq = double.Parse(temp);
            }
            else {
                kq = x - y;
            }
            nganS.Push(kq);
        }
        void nhan(ref Stack nganS) {
            double y = (double)nganS.Pop();
            double x = (double)nganS.Pop();
            double kq;
            if (BaseConvert(ref x, ref y)) {
                string temp = Convert.ToString((int)(x * y), baseInt);
                kq = double.Parse(temp);
            }
            else {
                kq = x * y;
            }
            nganS.Push(kq);
        }
        void chia(ref Stack nganS) {
            double y = (double)nganS.Pop();
            double x = (double)nganS.Pop();
            double kq;
            if (BaseConvert(ref x, ref y)) {
                string temp = Convert.ToString((int)(x / y), baseInt);
                kq = double.Parse(temp);
            }
            else {
                kq = x / y;
            }
            nganS.Push(kq);
        }
        void mu(ref Stack nganS) {
            double y = (double)nganS.Pop();
            double x = (double)nganS.Pop();
            nganS.Push(Math.Pow(x, y));
        }
        void can2(ref Stack nganS) {
            double x = (double)nganS.Pop();
            if (x < 0) throw new Exception("sqrt(x<0).");
            nganS.Push(Math.Sqrt(x));
        }
        void canx(ref Stack nganS) {
            double y = (double)nganS.Pop();
            double x = (double)nganS.Pop();
            nganS.Push(Math.Pow(y, 1/x));
        }
        void mod(ref Stack nganS) {
            string y = nganS.Pop().ToString();
            string x = nganS.Pop().ToString();
            int a, b;
            if (int.TryParse(y, out b) && int.TryParse(x, out a)) {
                int c = a % b;
                nganS.Push(double.Parse(c.ToString()));
            }
            else {
                throw new Exception("How to mod?");
            }
        }
        void sin(ref Stack nganS) {
            double x = (double)nganS.Pop();
            switch (Angle) {
                case "Degrees": x *= Math.PI / 180; break;
                case "Radians": break;
                case "Gradients": x *= Math.PI / 200; break;
            }
            nganS.Push(Math.Sin(x));
        }
        void asin(ref Stack nganS) {
            double x = (double)nganS.Pop();
            double goc = Math.Asin(x);
            switch (Angle) {
                case "Degrees": goc *= 180 / Math.PI; break;
                case "Radians": break;
                case "Gradients": goc *= 200 / Math.PI; break;
            }
            nganS.Push(goc);
        }
        void cos(ref Stack nganS) {
            double x = (double)nganS.Pop();
            switch (Angle) {
                case "Degrees": x *= Math.PI / 180; break;
                case "Radians": break;
                case "Gradients": x *= Math.PI / 200; break;
            }
            nganS.Push(Math.Cos(x));
        }
        void acos(ref Stack nganS) {
            double x = (double)nganS.Pop();
            double goc = Math.Acos(x);
            switch (Angle) {
                case "Degrees": goc *= 180 / Math.PI; break;
                case "Radians": break;
                case "Gradients": goc *= 200 / Math.PI; break;
            }
            nganS.Push(goc);
        }
        void tan(ref Stack nganS) {
            double x = (double)nganS.Pop();
            switch (Angle) {
                case "Degrees": x *= Math.PI / 180; break;
                case "Radians": break;
                case "Gradients": x *= Math.PI / 200; break;
            }
            nganS.Push(Math.Tan(x));
        }
        void atan(ref Stack nganS) {
            double x = (double)nganS.Pop();
            double goc = Math.Atan(x);
            switch (Angle) {
                case "Degrees": goc *= 180 / Math.PI; break;
                case "Radians": break;
                case "Gradients": goc *= 200 / Math.PI; break;
            }
            nganS.Push(goc);
        }
        void abs(ref Stack nganS) {
            double x = (double)nganS.Pop();
            nganS.Push(Math.Abs(x));
        }
        void pi(ref Stack nganS) {
            nganS.Push(Math.PI);
        }
        void e(ref Stack nganS) {
            nganS.Push(Math.E);
        }
        void ran(ref Stack nganS) {
            Random rd = new Random();
            int s = rd.Next(0, 100);
            string temp = Convert.ToString(s, baseInt);
            double kq = double.Parse(temp);
            nganS.Push(kq);
        }
        void Ans(ref Stack nganS) {
            nganS.Push(ans);
        }
        void m(ref Stack nganS) {
            nganS.Push(M);
        }
    }
}
