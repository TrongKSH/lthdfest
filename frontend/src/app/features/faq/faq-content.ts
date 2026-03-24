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
      'Sự kiện sẽ diễn ra trong 2 ngày với 2 tính chất. Thời gian mở cổng và lịch diễn chi tiết sẽ được cập nhật trên website.',
  },
  {
    id: 2,
    question: 'Tôi có thể mua vé ở đâu?',
    answer:
      'Vé được bán chính thức trên website của festival. Khán giả nên mua vé từ nguồn chính thức để đảm bảo quyền lợi.',
  },
  {
    id: 3,
    question: 'Tôi nhận vé bằng cách nào sau khi thanh toán?',
    answer:
      'Vé điện tử sẽ được gửi qua Email trong 48 giờ sau khi thanh toán thành công. Hãy kiểm tra hộp thư của bạn.',
  },
  {
    id: 4,
    question: 'Tôi phải làm sao nếu như không nhận được vé Email?',
    answer:
      'Hãy kiểm tra kỹ các hộp thư, nếu vẫn không thấy, bạn có thể liên hệ chúng tôi qua FANPAGE/HOTLINE để được hỗ trợ.',
  },
  {
    id: 5,
    question: 'Vé đã mua có được hoàn tiền hoặc đổi trả không?',
    answer:
      'Vé đã thanh toán sẽ không được hoàn tiền hoặc đổi trả, trừ trường hợp sự kiện bị hủy bởi Ban Tổ Chức.',
  },
  {
    id: 6,
    question: 'Festival có giới hạn độ tuổi tham gia không?',
    answer:
      'Sự kiện phù hợp với người từ 16 tuổi trở lên. Người dưới 16 tuổi cần có người giám hộ đi kèm.',
  },
] as const;
