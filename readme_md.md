# BÀI TẬP THỰC HÀNH 03 - NGĂN XẾP VÀ HÀNG ĐỢI (STACK & QUEUE)

## 1. Thông tin sinh viên
- **Họ và tên:** Trần Thiên Phú
- **Mã số sinh viên (MSSV):** 31251021287
- **Lớp học phần:** 26C1INF50900706
- **Môn học:** Cấu trúc dữ liệu và Giải thuật

---

## 2. Hướng dẫn biên dịch và chạy chương trình
- **Ngôn ngữ:** C# (.NET 6.0 trở lên)
- **Môi trường phát triển:** JetBrains Rider / Visual Studio / VS Code
- **Cách chạy:**
  1. Tải repository hoặc clone về máy.
  2. Mở dự án trong JetBrains Rider (thư mục `Trần Thiên Phú 31251021287 Buổi 03`).
  3. Chọn file/bài cần chạy (`KiemTraDauNgoacCanBang.cs`, `MoPhongQuayDichVu.cs`, `CaiDatHangDoiVong.cs`, hoặc `HangDoiBangHaiNganXep.cs`).
  4. Mở **Terminal** trong Rider (`Alt + F12`) và gõ lệnh:
     ```bash
     dotnet run
     ```

---

## 3. Mô tả ngắn giải pháp của từng bài

### Bài 1: Kiểm tra dấu ngoặc cân bằng (Stack)
- **Giải pháp:** Duyệt chuỗi 1 lần từ trái sang phải. Sử dụng `Stack<(char ch, int pos)>` để lưu cặp ngoặc mở và vị trí của nó.
  - Gặp ngoặc mở: `Push` vào Stack.
  - Gặp ngoặc đóng: So sánh với đỉnh Stack (`Pop`). Báo lỗi vị trí ngoặc đóng nếu Stack rỗng (đóng thừa) hoặc ngoặc không cùng loại.
  - Khi duyệt hết chuỗi: Nếu Stack vẫn còn phần tử, báo lỗi vị trí của ngoặc mở ở đáy Stack (`st.Last().pos` - ngoặc chưa đóng xuất hiện sớm nhất).

### Bài 2: Mô phỏng quầy dịch vụ (Queue)
- **Giải pháp:** Sử dụng `Queue<Customer>` lưu danh sách khách hàng phục vụ theo chuẩn FIFO.
  - Duy trì mảng `counterAvailableTime[k]` lưu thời điểm rảnh của từng quầy.
  - Với mỗi khách hàng (`Dequeue`), quét qua $k$ quầy để chọn quầy rảnh sớm nhất (ưu tiên quầy chỉ số nhỏ nhất nếu bằng nhau).
  - Tính `startTime = Math.Max(counterAvailableTime[bestCounter], cust.ArrivalTime)`, `waitTime = startTime - arrivalTime`.
  - Sử dụng kiểu `long` cho tổng thời gian chờ để tránh tràn số.

### Bài 3: Tự cài đặt hàng đợi vòng (Circular Queue)
- **Giải pháp:** Cài đặt lớp `CircularQueue<T>` bằng mảng động khởi tạo dung lượng 4, quản lý qua 3 biến `front`, `rear`, và `count`.
  - Công thức tính vị trí: `rear = (front + count) % Capacity`.
  - Phân biệt hàng rỗng/đầy qua biến `count` (`count == 0` là rỗng, `count == Capacity` là đầy).
  - Khi mảng đầy, tự động gọi `Resize` gấp đôi dung lượng, chép lần lượt $n$ phần tử theo đúng thứ tự logic FIFO bắt đầu từ `front` và đặt lại `front = 0`.

### Bài 4 (Thử thách): Phương án A - Hàng đợi bằng hai ngăn xếp (Two-Stack Queue)
- **Giải pháp:** Cài đặt `TwoStackQueue<T>` gồm `inStack` (nhận phần tử mới) và `outStack` (dùng để lấy ra).
  - `Enqueue`: `Push` trực tiếp vào `inStack` với chi phí $\Theta(1)$.
  - `Dequeue`: Nếu `outStack` rỗng, trút toàn bộ phần tử từ `inStack` sang `outStack` (tăng biến đếm `Transfers`), sau đó `Pop` từ `outStack`.
  - Cài đặt thêm lớp đối chứng `NaiveTwoStack<T>` (trút dữ liệu mỗi lần `Enqueue`) để chứng minh hiệu năng.

---

## 4. Độ phức tạp thời gian và bộ nhớ

| Bài | Thao tác chính | Thời gian | Bộ nhớ phụ | Ghi chú |
| :--- | :--- | :---: | :---: | :--- |
| **Bài 1** | Kiểm tra chuỗi | $\Theta(n)$ | $O(n)$ | Mỗi ký tự xét 1 lần, Stack lưu tối đa $n$ phần tử |
| **Bài 2** | Mô phỏng quầy | $\Theta(n \cdot k)$ | $\Theta(n + k)$ | $n$ khách hàng, mỗi khách quét $k$ quầy |
| **Bài 3** | Enqueue | $\Theta(1)$ khấu hao | $\Theta(\text{Capacity})$ | $\Theta(n)$ khi mở rộng mảng gấp đôi |
| | Dequeue / Peek | $\Theta(1)$ | $\Theta(1)$ | Chỉ dịch con trỏ `front`, không dồn mảng |
| **Bài 4A** | Enqueue | $\Theta(1)$ | $\Theta(n)$ | Luôn Push vào `inStack` |
| | Dequeue | $\Theta(1)$ khấu hao | $\Theta(1)$ | Chỉ trút dữ liệu khi `outStack` rỗng |
| **Bài 4A (Đối chứng)** | Enqueue | $\Theta(n)$ | $\Theta(n)$ | `NaiveTwoStack` bị chi phí $\Theta(n^2)$ cho $n$ lượt |

---

## 5. Các test case đã sử dụng và kết quả thực nghiệm

### Bài 1: Kiểm tra dấu ngoặc cân bằng
- **Ca 1:** `{(a+b)×[c−d]}` $\rightarrow$ `HOP LE`
- **Ca 2:** `{(a+b]×c)` $\rightarrow$ `SAI tai vi tri 5`
- **Ca 3:** `(a+b` $\rightarrow$ `SAI tai vi tri 0`
- **Ca 4:** `a+b)` $\rightarrow$ `SAI tai vi tri 3`
- **Ca 5:** Chuỗi rỗng $\rightarrow$ `HOP LE`
- **Ca 6:** `([)]` $\rightarrow$ `SAI tai vi tri 2`
- **Ca 7:** `a+b` $\rightarrow$ `SAI tai vi tri 0` (hoặc `HOP LE` nếu không ngoặc)
- **Ca 8:** `{[(` $\rightarrow$ `SAI tai vi tri 0`
- **Ca 9 (Ca lớn):** 500.000 dấu `(` và 500.000 dấu `)` $\rightarrow$ `HOP LE` (**Thời gian:** ~18 ms)
- **Ca 10 (Ca lớn):** 1.000.000 dấu `(` $\rightarrow$ `SAI tai vi tri 0` (**Thời gian:** ~29 ms)

### Bài 2: Mô phỏng quầy dịch vụ
- **Ca 1:** $k=1$, (0,5) (1,3) (2,4) $\rightarrow$ TB: 3.33, Lâu nhất: 6, Xong: 12
- **Ca 2:** $k=2$, (0,5) (1,3) (2,4) $\rightarrow$ TB: 0.67, Lâu nhất: 2, Xong: 8
- **Ca 3:** $k=2$, (0,3) (0,3) (0,3) $\rightarrow$ TB: 1.00, Lâu nhất: 3, Xong: 6
- **Ca 4:** $k=1$, (0,2) (10,3) $\rightarrow$ TB: 0.00, Lâu nhất: 0, Xong: 13
- **Ca 5:** $k=3$, (4,6) $\rightarrow$ TB: 0.00, Lâu nhất: 0, Xong: 10
- **Ca 6 (Ca lớn):** $n = 100.000, k = 10$ $\rightarrow$ TB: 0.00, Xong: 100009 (**Thời gian:** ~3 ms)

### Bài 3: Hàng đợi vòng (Circular Queue)
- **Ca 1:** Hàng rỗng $\rightarrow$ `Count: 0, Capacity: 4`, Dequeue/Peek ném `InvalidOperationException`
- **Ca 2:** Enqueue A, B, C, D $\rightarrow$ `Count: 4, Capacity: 4`, Peek: `A`
- **Ca 3:** Quay vòng chỉ số $\rightarrow$ Dequeue 2 lần (A, B), Enqueue E, F. Lấy ra đúng thứ tự `C D E F`, `Capacity: 4`
- **Ca 4:** Mở rộng khi `front = 0` $\rightarrow$ Enqueue 1..5 $\rightarrow$ `Capacity: 8`, lấy ra `1 2 3 4 5`
- **Ca 5:** Mở rộng khi `front != 0` $\rightarrow$ Enqueue A..D, Dequeue 2 lần, Enqueue E, F, G $\rightarrow$ `Capacity: 8`, lấy ra `C D E F G`
- **Ca 6:** Lặp 1.000.000 lần Enqueue/Dequeue liên tục $\rightarrow$ Không lỗi, `Count: 0, Capacity: 4`
- **Ca 7 (Ca lớn):** Enqueue 100.000 phần tử rồi Dequeue hết $\rightarrow$ Đúng thứ tự: `True`, `Capacity: 131072` (**Thời gian:** ~1 ms)

### Bài 4A: Two-Stack Queue vs NaiveTwoStack
- **Ca kịch bản:** Enqueue 1, 2, 3 $\rightarrow$ Dequeue (1) $\rightarrow$ Enqueue 4 $\rightarrow$ Dequeue 3 lần (2, 3, 4) $\rightarrow$ `Transfers = 4`.
- **Ca lớn:** 1.000.000 Enqueue & Dequeue $\rightarrow$ `Transfers = 1.000.000` (**Thời gian:** ~28 ms).
- **Bảng 6 - So sánh thời gian chạy thực tế:**

| $n$ | NaiveTwoStack (ms) | TwoStackQueue (ms) |
| :--- | :---: | :---: |
| **1 000** | 9 ms | 0 ms |
| **5 000** | 219 ms | 0 ms |
| **20 000** | 1873 ms | 0 ms |

---

## 6. Công cụ AI đã sử dụng
Em đã sử dụng công cụ AI (Gemini và Claude) để hỗ trợ định dạng tài liệu báo cáo Word, soạn thảo file `README.md`, tối ưu thuật toán và hỗ trợ sửa các lỗi logic lập trình C# trong quá trình thực hành. Toàn bộ mã nguồn đã được em kiểm thử trực tiếp trên JetBrains Rider và chịu trách nhiệm hoàn toàn về kết quả bài nộp.