/**
 * Static FAQ copy — bundled with the app, no API or database.
 * Edit here only; content ships to the client on build.
 */
export type FaqItem = {
  id: number;
  question: string;
  answer: string;
};

export const FAQ_ITEMS: readonly FaqItem[] = [
  {
    id: 1,
    question: 'Festival sẽ diễn ra khi nào?',
    answer:
      'Festival diễn ra trong hai ngày 8–9 tháng 5 năm 2026. Thời gian cụ thể và sân khấu sẽ được cập nhật trên website và kênh chính thức của ban tổ chức.',
  },
  {
    id: 2,
    question: 'Tôi có thể mua vé ở đâu?',
    answer:
      'Vé được bán qua các kênh chính thức do ban tổ chức công bố (website, đối tác bán vé). Vui lòng không mua vé từ nguồn không rõ nguồn gốc để tránh rủi ro.',
  },
  {
    id: 3,
    question: 'Tôi nhận vé bằng cách nào sau khi thanh toán?',
    answer:
      'Sau khi thanh toán thành công, vé điện tử (e-ticket) thường được gửi qua email bạn đã đăng ký. Kiểm tra cả hộp thư spam. Nếu cần hỗ trợ, liên hệ email hoặc hotline trong mục Liên hệ.',
  },
  {
    id: 4,
    question: 'Tôi phải làm sao nếu như không nhận được vé Email?',
    answer:
      'Kiểm tra lại địa chỉ email và thư mục spam. Nếu vẫn không có, liên hệ ban tổ chức kèm mã đơn hàng / thông tin thanh toán để được xử lý lại.',
  },
  {
    id: 5,
    question: 'Vé đã mua có được hoàn tiền hoặc đổi trả không?',
    answer:
      'Chính sách hoàn vé / đổi vé tuân theo điều khoản tại thời điểm mua. Thông tin chi tiết sẽ có trên trang vé và email xác nhận.',
  },
  {
    id: 6,
    question: 'Festival có giới hạn độ tuổi tham gia không?',
    answer:
      'Ban tổ chức có thể áp dụng giới hạn độ tuổi hoặc điều kiện vào cửa theo quy định pháp luật và nội dung chương trình. Vui lòng xem thông báo chính thức trước ngày diễn ra.',
  },
] as const;
