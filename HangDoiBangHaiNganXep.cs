using System.Diagnostics;

namespace BD2.DSA;


    public class TwoStackQueue<T>
    {
        private readonly Stack<T> inStack = new Stack<T>();  // Ngăn xếp nhận phần tử mới
        private readonly Stack<T> outStack = new Stack<T>(); // Ngăn xếp dùng để lấy phần tử ra

        // Tổng số phần tử đã đổ từ inStack sang outStack
        public long Transfers { get; private set; } = 0;

        // Tổng số phần tử hiện có trong hàng đợi
        public int Count => inStack.Count + outStack.Count;

        // Thêm phần tử vào hàng đợi: luôn Push trực tiếp vào inStack (Chi phí O(1))
        public void Enqueue(T value)
        {
            inStack.Push(value);
        }

        // Lấy và xóa phần tử ở đầu hàng đợi (Chi phí O(1) khấu hao)
        public T Dequeue()
        {
            // Ném ngoại lệ nếu cả hai ngăn xếp đều không có phần tử
            if (Count == 0)
            {
                throw new InvalidOperationException("Hàng đợi đang rỗng.");
            }

            // Chỉ thực hiện chuyển dữ liệu khi outStack đang rỗng
            if (outStack.Count == 0)
            {
                while (inStack.Count > 0)
                {
                    // Lấy từng phần tử từ inStack đẩy sang outStack
                    outStack.Push(inStack.Pop());
                    // Tăng số lượt chuyển đổi phần tử
                    Transfers++;
                }
            }

            // Lấy phần tử nằm trên đỉnh outStack (chính là phần tử đến sớm nhất)
            return outStack.Pop();
        }
    }

    // Lớp đối chứng NaiveTwoStack: mỗi lần Enqueue đều chuyển toàn bộ phần tử
    public class NaiveTwoStack<T>
    {
        private readonly Stack<T> mainStack = new Stack<T>();
        private readonly Stack<T> tempStack = new Stack<T>();

        public int Count => mainStack.Count;

        // Thao tác Enqueue ngây thơ: đổ main -> temp, push mới vào main, đổ temp -> main
        public void Enqueue(T value)
        {
            // Đổ toàn bộ phần tử từ mainStack sang tempStack
            while (mainStack.Count > 0)
            {
                tempStack.Push(mainStack.Pop());
            }

            // Đưa phần tử mới vào đáy mainStack
            mainStack.Push(value);

            // Đổ ngược lại từ tempStack sang mainStack
            while (tempStack.Count > 0)
            {
                mainStack.Push(tempStack.Pop());
            }
        }

        public T Dequeue()
        {
            if (mainStack.Count == 0)
            {
                throw new InvalidOperationException("Hàng đợi đang rỗng.");
            }

            return mainStack.Pop();
        }
    }

    public class HangDoiBangHaiNganXep
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding =  System.Text.Encoding.UTF8;
            
            Console.WriteLine("Chọn Phương án 4A: TWO-STACK QUEUE\n");

            // 1. Ca kiểm thử bắt buộc theo kịch bản đề bài
            RunRequiredTestCase();

            // 2. Chạy thử nghiệm ca lớn (1.000.000 phần tử)
            RunLargeTestCase();

            // 3. So sánh thời gian thực thi điền Bảng 6
            RunBenchmarkTable();
        }

        static void RunRequiredTestCase()
        {
            Console.WriteLine("1. Ca kiểm thử kịch bản bắt buộc");
            var q = new TwoStackQueue<int>();

            q.Enqueue(1);
            q.Enqueue(2);
            q.Enqueue(3);
            Console.WriteLine($"Dequeue lần 1: {q.Dequeue()} (Kỳ vọng: 1)");

            q.Enqueue(4);
            Console.Write("Dequeue 3 lần tiếp theo: ");
            Console.Write($"{q.Dequeue()} ");
            Console.Write($"{q.Dequeue()} ");
            Console.Write($"{q.Dequeue()} "); 
            Console.WriteLine();

            Console.WriteLine($"Tổng số phần tử đã Transfers: {q.Transfers} (Kỳ vọng: 4)\n");
        }

        static void RunLargeTestCase()
        {
            Console.WriteLine("2. Ca kiểm thử lớn (1.000.000 Enqueue & Dequeue)");
            var q = new TwoStackQueue<int>();
            int n = 1000000;

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 1; i <= n; i++) q.Enqueue(i);
            for (int i = 1; i <= n; i++) q.Dequeue();

            sw.Stop();

            Console.WriteLine($"Thời gian thực thi: {sw.ElapsedMilliseconds} ms (Yêu cầu < 1000 ms)");
            Console.WriteLine($"Số phần tử Transfers: {q.Transfers} (Kỳ vọng: 1000000)\n");
        }

        static void RunBenchmarkTable()
        {
            Console.WriteLine("3. Bảng so sánh thời gian chạy (Bảng 6)");
            int[] testSizes = { 1000, 5000, 20000 };

            Console.WriteLine("| n | NaiveTwoStack (ms) | TwoStackQueue (ms) |");
            Console.WriteLine("");

            foreach (int n in testSizes)
            {
                // Đo NaiveTwoStack
                var naiveQ = new NaiveTwoStack<int>();
                Stopwatch swNaive = Stopwatch.StartNew();
                for (int i = 0; i < n; i++) naiveQ.Enqueue(i);
                for (int i = 0; i < n; i++) naiveQ.Dequeue();
                swNaive.Stop();

                // Đo TwoStackQueue
                var optQ = new TwoStackQueue<int>();
                Stopwatch swOpt = Stopwatch.StartNew();
                for (int i = 0; i < n; i++) optQ.Enqueue(i);
                for (int i = 0; i < n; i++) optQ.Dequeue();
                swOpt.Stop();

                Console.WriteLine($"| {n,5} | {swNaive.ElapsedMilliseconds,18} | {swOpt.ElapsedMilliseconds,18} |");
            }
        }
    }