using System.Text.RegularExpressions;
using ClinicManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.API.Utils
{
    public static class TimeHelper
    {
        // 1. Hàm chuyển đổi "T2" -> DayOfWeek.Monday
        private static DayOfWeek? ParseVietnameseDay(string dayStr)
        {
            dayStr = dayStr.Trim().ToUpper();
            return dayStr switch
            {
                "CN" => DayOfWeek.Sunday,
                "T2" => DayOfWeek.Monday,
                "T3" => DayOfWeek.Tuesday,
                "T4" => DayOfWeek.Wednesday,
                "T5" => DayOfWeek.Thursday,
                "T6" => DayOfWeek.Friday,
                "T7" => DayOfWeek.Saturday,
                _ => null
            };
        }
// 2. Hàm kiểm tra: Bác sĩ có làm việc vào giờ này không? (PHIÊN BẢN NÂNG CẤP)
        // Hỗ trợ nhiều ca: "8h-12h, 13h-17h"
        public static bool IsDoctorWorkingAt(string scheduleStr, DateTime requestedTime)
        {
            if (string.IsNullOrWhiteSpace(scheduleStr)) return false;

            try
            {
                // Tách phần ngày và giờ
                var parts = scheduleStr.Split(':');
                if (parts.Length < 2) return false;

                string daysPart = parts[0].Trim();
                // Nối lại phần giờ phòng trường hợp giờ có dính dấu : (dù format chuẩn là h)
                string timePart = string.Join(":", parts.Skip(1)).Trim();

                // A. KIỂM TRA NGÀY (Giữ nguyên logic cũ)
                bool isDayMatch = false;
                var currentDay = requestedTime.DayOfWeek;

                if (daysPart.Contains("-")) // T2-T6
                {
                    var range = daysPart.Split('-');
                    var startDay = ParseVietnameseDay(range[0]);
                    var endDay = ParseVietnameseDay(range[1]);

                    if (startDay != null && endDay != null)
                    {
                        // Lưu ý: DayOfWeek.Sunday là 0. Logic này đúng cho T2(1) đến T7(6).
                        // Nếu khoảng qua tuần (VD: T6-T2) cần logic phức tạp hơn, nhưng T2-T6 thì ok.
                        if (currentDay >= startDay && currentDay <= endDay) isDayMatch = true;
                    }
                }
                else // T2, T4, CN
                {
                    var days = daysPart.Split(',');
                    foreach (var d in days)
                    {
                        if (ParseVietnameseDay(d) == currentDay)
                        {
                            isDayMatch = true;
                            break;
                        }
                    }
                }

                if (!isDayMatch) return false;

                // B. KIỂM TRA GIỜ (NÂNG CẤP: Hỗ trợ nhiều ca làm việc)
                // Tách các ca làm việc bằng dấu phẩy (VD: "8h-12h, 13h-17h")
                var timeRanges = timePart.Split(',');

                foreach (var range in timeRanges)
                {
                    // Regex tìm giờ trong từng ca
                    var match = Regex.Match(range.Trim(), @"(\d{1,2})h(\d{1,2})?-(\d{1,2})h(\d{1,2})?");
                    
                    if (match.Success)
                    {
                        int startH = int.Parse(match.Groups[1].Value);
                        int startM = string.IsNullOrEmpty(match.Groups[2].Value) ? 0 : int.Parse(match.Groups[2].Value);
                        
                        int endH = int.Parse(match.Groups[3].Value);
                        int endM = string.IsNullOrEmpty(match.Groups[4].Value) ? 0 : int.Parse(match.Groups[4].Value);

                        TimeSpan shiftStart = new TimeSpan(startH, startM, 0);
                        TimeSpan shiftEnd = new TimeSpan(endH, endM, 0);
                        TimeSpan requestTimeSlot = requestedTime.TimeOfDay;

                        // Nếu giờ khách chọn nằm trong CA NÀY -> Hợp lệ -> Return True luôn
                        if (requestTimeSlot >= shiftStart && requestTimeSlot <= shiftEnd)
                        {
                            return true; 
                        }
                    }
                }

                // Chạy hết các ca mà không khớp cái nào
                return false;
            }
            catch
            {
                return false;
            }
        }
        // 3. Hàm kiểm tra trùng lịch (Overlap 30 phút)
        // Trả về: True nếu CÓ THỂ ĐẶT (Trống), False nếu bị TRÙNG
		public static async Task<bool> IsSlotAvailable(ClinicManagementContext context, int doctorId, DateTime requestedTime, int? excludeAppointmentId = null)
        {
            // Lấy danh sách lịch hẹn của bác sĩ
            var query = context.LichHens
                .Where(l => l.MaBacSi == doctorId 
                            && l.TrangThai != "Đã hủy" // <--- CẬP NHẬT TRẠNG THÁI CHUẨN
                            && l.NgayHen.Date == requestedTime.Date);

            // Nếu đang sửa lịch, thì loại trừ chính cái lịch đang sửa ra khỏi danh sách kiểm tra
            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(l => l.MaLichHen != excludeAppointmentId.Value);
            }

            var doctorAppointments = await query.ToListAsync();

            double requestMinutes = requestedTime.TimeOfDay.TotalMinutes;

            foreach (var appt in doctorAppointments)
            {
                double existingMinutes = appt.NgayHen.TimeOfDay.TotalMinutes;
                
                // Logic: Khoảng cách giữa 2 lịch phải >= 30 phút
                double diff = Math.Abs(existingMinutes - requestMinutes);
                
                if (diff < 30) 
                {
                    return false; // Bị trùng
                }
            }

            return true;
        }
        public static List<int> GetWorkingDays(string scheduleStr)
{
    var result = new List<int>();
    if (string.IsNullOrWhiteSpace(scheduleStr)) return result; // Trả về rỗng

    try
    {
        var parts = scheduleStr.Split(':');
        string daysPart = parts[0].Trim();

        if (daysPart.Contains("-")) // T2-T6
        {
            var range = daysPart.Split('-');
            var startDay = ParseVietnameseDay(range[0]);
            var endDay = ParseVietnameseDay(range[1]);

            if (startDay != null && endDay != null)
            {
                for (int i = (int)startDay; i <= (int)endDay; i++)
                {
                    result.Add(i);
                }
            }
        }
        else // T2, T4, CN
        {
            var days = daysPart.Split(',');
            foreach (var d in days)
            {
                var dayEnum = ParseVietnameseDay(d);
                if (dayEnum != null)
                {
                    result.Add((int)dayEnum.Value);
                }
            }
        }
    }
    catch
    {
        // Ignore error
    }

    return result;
}
    }
    
}