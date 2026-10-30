using System.Diagnostics;
using System.Globalization;

namespace BD2.DSA;

public class MoPhongQuayDichVu
{
    // Cấu trúc dữ liệu đại diện cho một khách hàng
    // Dùng record/class để lưu thông tin định danh, thời điểm đến và thời gian dịch vụ
    record Customer(int Id, long ArrivalTime, long ServiceTime);

    static void Main1(string[] args)
    {
        // Thiết lập định dạng in số thực dùng dấu chấm (.) làm dấu phân cách thập phân
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        RunTestCases();
    }

    // Hàm mô phỏng xử lý hàng đợi quầy dịch vụ
    static void Simulate(int n, int k, List<Customer> customerList)
    {
        // Khởi tạo hàng chờ phục vụ theo nguyên tắc FIFO
        var queue = new Queue<Customer>();

        // Đưa khách hàng vào queue
        foreach (var c in customerList)
        {
            queue.Enqueue(c);
        }

        // Mảng lưu thời điểm rảnh tiếp theo của từng quầy
        long[] counterAvailableTime = new long[k];
        
        long totalWaitTime = 0; 
        long maxWaitTime = 0;       
        long maxFinishTime = 0; 

        // Vòng lặp xử lý lần lượt từng khách hàng
        while (queue.Count > 0)
        {
            // Lấy khách hàng đứng đầu hàng chờ ra khỏi queue
            Customer cust = queue.Dequeue();

            // Tìm quầy rảnh sớm nhất. Nếu nhiều quầy rảnh cùng lúc, ưu tiên quầy có chỉ số nhỏ nhất.
            int bestCounter = 0;
            for (int q = 1; q < k; q++)
            {
                // Quét qua k quầy để tìm thời điểm rảnh nhỏ hơn
                if (counterAvailableTime[q] < counterAvailableTime[bestCounter])
                {
                    bestCounter = q;
                }
            }
            
            // Nếu quầy rảnh trước khi khách đến, quầy phải đợi khách tới mới bắt đầu phục vụ được.
            long startTime = Math.Max(counterAvailableTime[bestCounter], cust.ArrivalTime);

            // Thời gian khách phải chờ = Thời điểm bắt đầu - Thời điểm đến
            long waitTime = startTime - cust.ArrivalTime;

            // Thời điểm hoàn thành = Thời điểm bắt đầu + Thời gian phục vụ
            long finishTime = startTime + cust.ServiceTime;

            // Cập nhật lại thời điểm rảnh mới cho quầy vừa tiếp nhận khách
            counterAvailableTime[bestCounter] = finishTime;
            
            totalWaitTime += waitTime;
            maxWaitTime = Math.Max(maxWaitTime, waitTime);
            maxFinishTime = Math.Max(maxFinishTime, finishTime);
        }

        // Tính thời gian chờ trung bình
        double avgWaitTime = (double)totalWaitTime / n;

        // In 3 dòng tổng hợp kết quả theo đúng chuẩn định dạng yêu cầu
        Console.WriteLine($"Cho trung binh: {avgWaitTime:F2}");
        Console.WriteLine($"Cho lau nhat: {maxWaitTime}");
        Console.WriteLine($"Hoan thanh luc: {maxFinishTime}");
    }
    
    static void RunTestCases()
    {
        Console.WriteLine("ca 1");
        Simulate(3, 1, new List<Customer> { new(1, 0, 5), new(2, 1, 3), new(3, 2, 4) });

        Console.WriteLine("\nca 2");
        Simulate(3, 2, new List<Customer> { new(1, 0, 5), new(2, 1, 3), new(3, 2, 4) });

        Console.WriteLine("\nca 3");
        Simulate(3, 2, new List<Customer> { new(1, 0, 3), new(2, 0, 3), new(3, 0, 3) });

        Console.WriteLine("\nca 4");
        Simulate(2, 1, new List<Customer> { new(1, 0, 2), new(2, 10, 3) });

        Console.WriteLine("\nca 5");
        Simulate(1, 3, new List<Customer> { new(1, 4, 6) });

        Console.WriteLine("\nca 6 (n = 100 000, k = 10)");
        int largeN = 100000;
        int largeK = 10;
        var largeCustomers = new List<Customer>(largeN);
        for (int i = 1; i <= largeN; i++)
        {
            largeCustomers.Add(new Customer(i, i - 1, 10));
        }

        Stopwatch sw = Stopwatch.StartNew();
        Simulate(largeN, largeK, largeCustomers);
        sw.Stop();

        Console.WriteLine($"Thoi gian ca 6: {sw.ElapsedMilliseconds} ms");
    }
}