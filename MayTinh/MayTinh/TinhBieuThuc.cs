using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MayTinh {
    class TinhBieuThuc {
        #region Properties
        public ArrayList BieuThuc;
        ArrayList hauToP;
        double ketQua;
        public PhepTinh PhepTinh;
        public string MoiTruong {
            get {
                return PhepTinh.Angle;
            }
            set {
                PhepTinh.Angle = value;
            }
        }
        public string Base {
            get {
                return PhepTinh.Base;
            }
            set {
                PhepTinh.Base = value;
            }
        }
        public double KetQua {
            get {
                return ketQua;
            }
        }
        #endregion
        #region Publish Functions
        /// <summary>
        /// Bước 1: Đưa về dạng trung tố
        /// Bước 2: Kiểm tra tính đúng sai
        /// Bước 3: Chuyển trung tố sang hậu tố
        /// Bước 4: Tính hậu tố
        /// </summary>
        public TinhBieuThuc() {
            BieuThuc = new ArrayList();
            PhepTinh = new PhepTinh();
        }
        public TinhBieuThuc(string input) {
            PhepTinh = new PhepTinh();
            BieuThuc = new ArrayList();
            input = input.Replace(" ", "") + ")";
            string temp = "";
            int tag = 0;
            while (input != "") {
                bool isDigit = isNumber(input[0])
                    ||
                    (tag == 0 && input[0] == '-' && char.IsDigit(input[1]));
                if (isDigit) {
                    tag = 1;
                    do {
                        temp += input[0].ToString();
                        input = input.Substring(1);
                    } while (isNumber(input[0]) && input[0] != '-');
                    BieuThuc.Add(double.Parse(temp));
                    temp = "";
                }
                else {
                    tag = 0;
                    BieuThuc.Add(input[0].ToString());
                    input = input.Substring(1);
                }
            }
            for (int i = 0; i < BieuThuc.Count - 1; i++) {
                if (isDouble(BieuThuc[i]) && isDouble(BieuThuc[i + 1])) {
                    BieuThuc.Insert(i + 1, "+");
                }
            }

        }
        /// <summary>
        /// Hiển thị biểu thức
        /// </summary>
        public string Text {
            get {
                string text = "";
                foreach (object s in BieuThuc) {
                    double temp;
                    if (double.TryParse(s.ToString(), out temp)) {
                        text += s.ToString();
                    }
                    else if (s.ToString() == "(" || s.ToString() == ")") {
                        text += s.ToString();
                    }
                    else {
                        text += PhepTinh.CacPhepToan[s.ToString()].TheHien;
                    }
                }
                return text;
            }
        }
        public void ThucHien() {
            chuyenSangHauTo();
            tinhHauTo();
        }
        #endregion
        #region Private Functions
        void chuyenSangHauTo() {
            hauToP = new ArrayList();
            Stack nganS = new Stack();
            nganS.Push("(");
            ArrayList bieuThuc = (ArrayList)BieuThuc.Clone();
            kiemTraNgoac(bieuThuc);

            for (int i = 1; i < bieuThuc.Count; i++) {
                if (!isDouble(bieuThuc[i])) {
                    if ((capDo(bieuThuc[i].ToString()) <= 1
                        || capDo(bieuThuc[i].ToString()) > 5)
                        && isDouble(bieuThuc[i - 1])) {
                        bieuThuc.Insert(i, "*");
                    }
                }
            }
            kiemTraToanTu(bieuThuc);
            bieuThuc.Add(")");
            foreach (var item in bieuThuc) {
                if (isDouble(item)) {
                    hauToP.Add((double)item);
                }
                else
                if ((string)item == "(") {
                    nganS.Push("(");
                }
                else
                if (capDo((string)item) > 2) {
                    while (capDo((string)nganS.Peek()) >= capDo((string)item)) {
                        hauToP.Add((string)nganS.Pop());
                    }
                    nganS.Push((string)item);
                }
                else
                if ((string)item == ")") {
                    while (capDo((string)nganS.Peek()) > 1) {
                        hauToP.Add((string)nganS.Pop());
                    }
                    nganS.Pop();
                }
                else {
                    hauToP.Add((string)item);
                }
            }
        }
        void kiemTraNgoac(ArrayList bieuThuc) {
            Stack kiemTraNgoac = new Stack();
            kiemTraNgoac.Push("Start");
            for (int i = 0; i < bieuThuc.Count; i++) {
                // Phần này không liên quan. (Base Exeption)
                // >>>>>>>>>>>>>>>>>>>>>>>>
                if (Base != "Decimal") {
                    if (isDouble(bieuThuc[i]) && (double)bieuThuc[i] < 0)
                        throw new Exception("Do it yourself!");
                }
                // <<<<<<<<<<<<<<<<<<<<<<<<<
                if (bieuThuc[i].ToString() == "(") {
                    kiemTraNgoac.Push("(");
                }
                else if (bieuThuc[i].ToString() == ")") {
                    kiemTraNgoac.Pop();
                }
                if (kiemTraNgoac.Count == 0) {
                    throw new Exception("Excess right parenthesis.");
                }
            }
            if (kiemTraNgoac.Peek().ToString() != "Start") {
                throw new Exception("Missing right parenthesis.");
            }
        }
        void kiemTraToanTu(ArrayList bieuThuc) {
            for (int i = 0; i < bieuThuc.Count - 1; i++) {
                if (capDo(bieuThuc[i].ToString()) == capDo(bieuThuc[i + 1].ToString())) {
                    throw new Exception("Operator error.");
                }
                if (capDo(bieuThuc[i].ToString()) > 2 && capDo(bieuThuc[i].ToString()) < 5
               && capDo(bieuThuc[i + 1].ToString()) > 2 && capDo(bieuThuc[i + 1].ToString()) < 5
               && bieuThuc[i + 1].ToString() != "-") {
                    throw new Exception("Operator error.");
                }
                if (bieuThuc[i].ToString() == "-" && capDo(bieuThuc[i + 1].ToString()) > 1
                    && capDo(bieuThuc[i + 1].ToString()) < 4) {
                    throw new Exception("Missing operand.");
                }
            }
        }
        void tinhHauTo() {
            Stack nganS = new Stack();
            foreach (var item in hauToP) {
                if (isDouble(item)) {
                    nganS.Push((double)item);
                }
                else {
                    ThucHienTinh tinh = PhepTinh.CacPhepToan[(string)item].Tinh;
                    tinh(ref nganS);
                }
            }
            ketQua = (double)nganS.Peek();
        }
        int capDo(string a) {
            return PhepTinh.CapDo(a);
        }
        bool isNumber(char c) {
            return char.IsDigit(c) || c == '.';
        }
        bool isDouble(object o) {
            double temp;
            return double.TryParse(o.ToString(), out temp);
        }
        #endregion
    }
}
