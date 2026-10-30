using System.Diagnostics;
using System.Text;

namespace BD2.DSA;
    // Cài đặt Hàng đợi vòng (Circular Queue) bằng mảng động
    public class CircularQueue<T>
    {
        private T[] items;    
        private int front;     
        private int count;    

        // Số phần tử hiện có
        public int Count => count;

        // Dung lượng mảng hiện tại
        public int Capacity => items.Length;

        // Khởi tạo hàng đợi với dung lượng ban đầu là 4
        public CircularQueue(int initialCapacity = 4)
        {
            items = new T[initialCapacity];
            front = 0;
            count = 0;
        }

        // Thêm phần tử vào cuối hàng đợi; tự động mở rộng mảng gấp đôi khi đầy
        public void Enqueue(T value)
        {
            // Kiểm tra nếu mảng đã đạt dung lượng tối đa thì tiến hành mở rộng
            if (count == items.Length)
            {
                Resize(items.Length * 2);
            }

            // Tính vị trí rear theo công thức quay vòng
            int rear = (front + count) % items.Length;
            
            // Đưa phần tử mới vào vị trí rear
            items[rear] = value;
            
            // Tăng số lượng phần tử hiện có
            count++;
        }

        // Lấy và xóa phần tử ở đầu hàng đợi
        public T Dequeue()
        {
            // Kiểm tra hàng rỗng để tránh truy cập hợp lệ
            if (count == 0)
            {
                throw new InvalidOperationException("Hang doi dang rong");
            }

            // Lưu phần tử đầu hàng để trả về
            T value = items[front];

            // Gán default cho ô vừa lấy để giải phóng tham chiếu cũ, giúp GC thu gom rác
            items[front] = default;

            // Dịch chuyển front sang vị trí tiếp theo theo công thức quay vòng
            front = (front + 1) % items.Length;

            // Giảm số lượng phần tử hiện có
            count--;

            return value;
        }

        // Xem phần tử ở đầu hàng đợi mà không xóa
        public T Peek()
        {

            if (count == 0)
            {
                throw new InvalidOperationException("Hang doi dang rong");
            }

            // Trả về giá trị tại vị trí front hiện tại
            return items[front];
        }

        // Hàm mở rộng dung lượng mảng và sắp xếp lại dữ liệu theo đúng thứ tự FIFO
        private void Resize(int newCapacity)
        {
            T[] newArray = new T[newCapacity];

            // Chép lần lượt n phần tử từ mảng cũ sang mảng mới theo thứ tự logic bắt đầu từ front
            for (int i = 0; i < count; i++)
            {
                newArray[i] = items[(front + i) % items.Length];
            }

            // Cập nhật mảng lưu trữ mới
            items = newArray;

            // Đặt lại front = 0 vì các phần tử đã được xếp lại từ đầu mảng mới
            front = 0;
        }
    }

    public class CaiDatHangDoiVong
    
    {
        static void Main1(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Bài 3: CIRCULAR QUEUE\n");

            // ca 1 hàng rỗng
            Console.WriteLine("Ca 1: Kiểm tra hàng rỗng");
            var q1 = new CircularQueue<string>();
            Console.WriteLine($"Count: {q1.Count}, Capacity: {q1.Capacity}");
            try { q1.Dequeue(); }
            catch (InvalidOperationException ex) { Console.WriteLine($"Dequeue Exception: {ex.Message}"); }
            try { q1.Peek(); }
            catch (InvalidOperationException ex) { Console.WriteLine($"Peek Exception: {ex.Message}"); }

            // ca 2 vừa đầy
            Console.WriteLine("\nCa 2: Vừa đầy");
            var q2 = new CircularQueue<string>();
            q2.Enqueue("A"); q2.Enqueue("B"); q2.Enqueue("C"); q2.Enqueue("D");
            Console.WriteLine($"Count: {q2.Count}, Capacity: {q2.Capacity}");
            Console.WriteLine($"Peek: {q2.Peek()}");

            // ca 3 quay vòng chỉ số, chưa mở rộng
            Console.WriteLine("\nCa 3: Quay vòng chỉ số (Capacity vẫn 4)");
            var q3 = new CircularQueue<string>();
            q3.Enqueue("A"); q3.Enqueue("B"); q3.Enqueue("C"); q3.Enqueue("D");
            Console.WriteLine($"Dequeue 1: {q3.Dequeue()}, Dequeue 2: {q3.Dequeue()}"); 
            q3.Enqueue("E"); q3.Enqueue("F");
            Console.Write("Lấy các phần tử còn lại: ");
            while (q3.Count > 0)
            {
                Console.Write($"{q3.Dequeue()} ");
            }
            Console.WriteLine($"\nCapacity cuối: {q3.Capacity}");

            //  ca 4 mở rộng khi front = 0
            Console.WriteLine("\nCa 4: Mở rộng khi front = 0");
            var q4 = new CircularQueue<int>();
            q4.Enqueue(1); q4.Enqueue(2); q4.Enqueue(3); q4.Enqueue(4); q4.Enqueue(5); // Mở rộng tại 5
            Console.WriteLine($"Capacity sau khi Enqueue 5 phần tử: {q4.Capacity}"); // Kỳ vọng: 8
            Console.Write("Lấy ra: ");
            while (q4.Count > 0)
            {
                Console.Write($"{q4.Dequeue()} "); // Kết quả kỳ vọng: 1 2 3 4 5
            }
            Console.WriteLine();

            //ca 5 mở rộng khi front khác 0 
            Console.WriteLine("\nCa 5: Mở rộng khi front khác 0");
            var q5 = new CircularQueue<string>();
            q5.Enqueue("A"); q5.Enqueue("B"); q5.Enqueue("C"); q5.Enqueue("D");
            q5.Dequeue(); q5.Dequeue(); // Xóa A, B (front chuyển sang C)
            q5.Enqueue("E"); q5.Enqueue("F"); q5.Enqueue("G"); // Kích hoạt mở rộng mảng
            Console.WriteLine($"Capacity sau khi mở rộng: {q5.Capacity}"); // Kỳ vọng: 8
            Console.Write("Lấy ra: ");
            while (q5.Count > 0)
            {
                Console.Write($"{q5.Dequeue()} "); // Kết quả kỳ vọng: C D E F G
            }
            Console.WriteLine();

            //  ca 6 lặp 1 000 000 lần Enqueue rồi Dequeue
            Console.WriteLine("\n Ca 6: Lặp 1.000.000 lần (Enqueue & Dequeue liên tục)");
            var q6 = new CircularQueue<int>();
            for (int i = 0; i < 1000000; i++)
            {
                q6.Enqueue(i);
                q6.Dequeue();
            }
            Console.WriteLine($"Không lỗi! Count: {q6.Count}, Capacity: {q6.Capacity}"); // Kỳ vọng: Count 0, Capacity 4

            //  Ca 7: Ca lớn - Enqueue 100.000 phần tử rồi Dequeue hết 
            Console.WriteLine("\nCa 7: Ca lớn (Enqueue 100.000 phần tử)");
            var q7 = new CircularQueue<int>();
            Stopwatch sw = Stopwatch.StartNew();
            
            for (int i = 1; i <= 100000; i++)
            {
                q7.Enqueue(i);
            }
            
            bool isCorrectOrder = true;
            for (int i = 1; i <= 100000; i++)
            {
                if (q7.Dequeue() != i)
                {
                    isCorrectOrder = false;
                    break;
                }
            }
            sw.Stop();

            Console.WriteLine($"Đúng thứ tự: {isCorrectOrder}");
            Console.WriteLine($"Capacity cuối: {q7.Capacity}");
            Console.WriteLine($"Thời gian thực thi: {sw.ElapsedMilliseconds} ms");
        }
    }
