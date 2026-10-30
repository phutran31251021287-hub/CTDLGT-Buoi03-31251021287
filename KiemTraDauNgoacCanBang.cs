using System.Diagnostics;

namespace BD2.DSA;

public class KiemTraDauNgoacCanBang
{
    static void Main1(string[] args)
    {
        string[] testCases = new string[]
        {
            // Ca 9: 500.000 dấu ( và 500.000 dấu )
            new string('(', 500000) + new string(')', 500000),

            // Ca 10: 1.000.000 dấu (
            new string('(', 1000000)
        };

        for (int i = 0; i < testCases.Length; i++)
        {
            int caNo = i == 0 ? 9 : 10;
            Console.WriteLine($"\n Dang chay Ca {caNo}");

            // Khởi động đồng hồ đo thời gian
            Stopwatch sw = Stopwatch.StartNew();

            // Thực thi thuật toán kiểm tra
            int errorPos = Balanced(testCases[i]);

            // Dừng đồng hồ
            sw.Stop();

            // In kết quả
            if (errorPos == -1)
                Console.WriteLine("HOP LE");
            else
                Console.WriteLine($"SAI tai vi tri {errorPos}");

            // In thời gian chạy tính bằng miligiây (ms)
            Console.WriteLine($"Thoi gian thuc thi {sw.ElapsedMilliseconds} ms");
        }
    }
    
    static int Balanced(string s)
    {
        var st = new Stack<(char ch, int pos)>();

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            // Nếu gặp ngoặc mở thì đưa cả ký tự và vị trí hiện tại i vào Stack
            if (c is '(' or '[' or '{')
            {
                st.Push((c, i));
            }
            // Nếu gặp ngoặc đóng thì kiểm tra xem có khớp với ngoặc mở ở đỉnh stack không
            else if (c is ')' or ']' or '}')
            {
                // Lỗi 1 gặp ngoặc đóng nhưng Stack rỗng => ngoặc đóng bị thừa
                if (st.Count == 0) return i;

                var top = st.Pop();
                // Lỗi 2 ngoặc đóng không cùng loại với ngoặc mở trên đỉnh Stack
                if (!Match(top.ch, c)) return i;
            }
        }
        // Lỗi 3 duyệt hết chuỗi nhưng vẫn còn ngoặc mở chưa đóng
        if (st.Count > 0)
        {
            return st.Last().pos;
        }

        // Không có lỗi
        return -1;
    }
    // Hàm kiểm tra cặp ngoặc mở và đóng
    static bool Match(char open, char close)
    {
        return (open == '(' && close == ')') ||
               (open == '[' && close == ']') ||
               (open == '{' && close == '}');
    }
}